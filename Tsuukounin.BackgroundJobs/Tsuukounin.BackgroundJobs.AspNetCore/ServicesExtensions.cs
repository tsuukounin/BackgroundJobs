using Microsoft.Extensions.DependencyInjection;
using Tsuukounin.BackgroundJobs.Core;

namespace Tsuukounin.BackgroundJobs.AspNetCore
{
	public static class ServicesExtensions
	{
		public static IServiceCollection AddBackgroundJobs(this IServiceCollection services)
		{
			services.AddSingleton<IBackgroundTaskExecutor, BackgroundJobsHostedService>();
			services.AddHostedService(p => p.GetService<IBackgroundTaskExecutor>() as BackgroundJobsHostedService);
			services.AddTransient<IBackgroundJobsService, BackgroundJobsService>();
			return services;
		}
	}
}
