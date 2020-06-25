using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Tsuukounin.BackgroundJobs.Example.Sevices
{

	public class TimeConsumingTaskService : ITimeConsumingTaskService
	{
		private readonly ILogger<TimeConsumingTaskService> _logger;

		public TimeConsumingTaskService(ILogger<TimeConsumingTaskService> logger)
		{
			_logger = logger;
		}

		public async Task DoWork(string name = "Unnamed")
		{
			_logger.LogInformation($"Work {name} started");
			await Task.Delay(3000);
			_logger.LogInformation($"Work {name} finished");
		}
	}
}
