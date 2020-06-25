using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tsuukounin.BackgroundJobs.Core;
using Tsuukounin.BackgroundJobs.Example.Sevices;

namespace Tsuukounin.BackgroundJobs.Example.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class TestController: Controller
	{
		private readonly IBackgroundJobsService _BackgroundJobsService;
		private readonly ITimeConsumingTaskService _timeConsumingTaskService;

		public TestController(IBackgroundJobsService BackgroundJobsService, ITimeConsumingTaskService timeConsumingTaskService)
		{
			_BackgroundJobsService = BackgroundJobsService;
			_timeConsumingTaskService = timeConsumingTaskService;
		}

		[HttpGet]
		public async Task<IActionResult> StartBackgroundJobs()
		{
			_BackgroundJobsService.Execute(_timeConsumingTaskService.DoWork("1"));
			_BackgroundJobsService.Execute(_timeConsumingTaskService.DoWork("2"));
			await Task.Delay(1000);
			_BackgroundJobsService.Execute(_timeConsumingTaskService.DoWork("3"));
			await Task.Delay(3500);
			_BackgroundJobsService.Execute(_timeConsumingTaskService.DoWork("4"));
			return Ok("Success");
		}

		[HttpGet]
		public async Task<IActionResult> StartJobWithException()
		{
			_BackgroundJobsService.Execute(EcxeptionTask());
			return Ok("Success");
		}

		private async Task EcxeptionTask()
		{
			throw new System.Exception("Exception task");
		}
	}
}
