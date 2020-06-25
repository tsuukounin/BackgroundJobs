using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tsuukounin.BackgroundJobs.Core;

namespace Tsuukounin.BackgroundJobs.Tests
{
	[TestFixture]
	public class Tests
	{
		private APIWebApplicationFactory _factory;
		private HttpClient _client;

		[OneTimeSetUp]
		public void GivenARequestToTheController()
		{
			_factory = new APIWebApplicationFactory();
			_client = _factory.CreateClient();
		}

		[OneTimeTearDown]
		public void TearDown()
		{
			_client.Dispose();
			_factory.Dispose();
		}

		[Test]
		public async Task ControllerReturnsOK()
		{
			var result = await _client.GetAsync("/test/StartBackgroundJobs");
			Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public async Task BackgroundJobServiceIsCompletingTasks()
		{
			using (var scope = _factory.Services.CreateScope())
			{
				var service = scope.ServiceProvider.GetRequiredService<IBackgroundJobsService>();
				var task1 = GetNumber(1);
				var task2 = GetNumber(2);
				service.Execute(task1);
				service.Execute(task2);
				await Task.Delay(200);
				Assert.That(task1.Result, Is.EqualTo(1));
				Assert.That(task2.Result, Is.EqualTo(2));
			}
		}

		private async Task<int> GetNumber(int num)
		{
			return num;
		}
	}
}