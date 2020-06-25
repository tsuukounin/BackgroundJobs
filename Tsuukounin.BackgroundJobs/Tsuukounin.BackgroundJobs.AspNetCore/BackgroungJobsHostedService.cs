using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;
using Tsuukounin.BackgroundJobs.Core;
using Microsoft.Extensions.Hosting;

namespace Tsuukounin.BackgroundJobs.AspNetCore
{
	internal class BackgroundJobsHostedService : IHostedService, IBackgroundTaskExecutor, IDisposable
    {
        private readonly ILogger<BackgroundJobsHostedService> _logger;
        private object _lock = new object();
        private List<Task> _tasks = new List<Task>();
        private AutoResetEvent _autoReset = new AutoResetEvent(false);
        private readonly CancellationTokenSource _stoppingCts =
                                               new CancellationTokenSource();

        public BackgroundJobsHostedService(ILogger<BackgroundJobsHostedService> logger)
        {
            _logger = logger;
        }

        public void AddTask(Task task)
        {
            lock (_lock)
            {
                _tasks.Add(task);
            }
            _autoReset.Set();
        }

        public void Dispose()
        {
            _stoppingCts.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var thread = new Thread(() => Execute(_stoppingCts.Token));
            thread.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Signal cancellation to the executing method
                _stoppingCts.Cancel();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error  while stoping BackgroundJobsHostedService");
            }
            return Task.CompletedTask;
        }

        private void Execute(CancellationToken stoppingToken)
        {
            _logger.LogInformation("BackgroundJobsHostedService started");

            while (!stoppingToken.IsCancellationRequested)
            {
                _autoReset.WaitOne();
                List<Task> tasks;
                lock (_lock)
                {
                    tasks = new List<Task>(_tasks);
                    _tasks.Clear();
                }
                if (!tasks.Any())
                    continue;

                Task aggregationTask = null;
                try
                {
                    aggregationTask = Task.WhenAll(_tasks);
                    aggregationTask.Wait();
                }
                catch (Exception ex)
                {
                    if (aggregationTask?.Exception?.InnerExceptions != null && aggregationTask.Exception.InnerExceptions.Any()) 
                        foreach (var innerEx in aggregationTask.Exception.InnerExceptions)
                        {
                            _logger.LogError(innerEx, "Background task exception");
                        }
                    else
                        _logger.LogError(ex, "Background task processing exception");
                }
            }
        }
    }
}
