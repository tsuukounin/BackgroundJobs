using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Tsuukounin.BackgroundJobs.Example
{
	public static class SwaggerExtensions
	{
		public static string ApiName = "Tsuukounin.BackgroundJobs.Example";
		public static string ApiVersion = "v1";

		public static void AddSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc(ApiVersion, new OpenApiInfo { Title = ApiName, Version = ApiVersion });
			});
		}

		public static void UseSwaggerWithUI(this IApplicationBuilder app)
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint($"/swagger/{ApiVersion}/swagger.json", $"{ApiName} {ApiVersion}");
			});
		}
	}
}
