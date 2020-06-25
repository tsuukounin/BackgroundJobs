using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tsuukounin.BackgroundJobs.Core;

namespace Tsuukounin.BackgroundJobs.AspNetCore
{
    internal class BackgroundJobsService : IBackgroundJobsService
	{
		private readonly IBackgroundTaskExecutor _backgroundTaskExecutor;

		public BackgroundJobsService(IBackgroundTaskExecutor backgroundTaskExecutor)
		{
			_backgroundTaskExecutor = backgroundTaskExecutor;
		}

		public void Execute(Task task)
		{
			_backgroundTaskExecutor.AddTask(task);
		}

		public void ExecuteInNamedQueue(string queueName, Task task)
		{
			throw new NotImplementedException();
		}
	}
}
