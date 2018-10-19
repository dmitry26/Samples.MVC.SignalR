using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SamplesSignalR.Backend;
using SamplesSignalR.Hubs;
using SamplesSignalR.Services;

namespace SamplesSignalR
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<CustomerRepository>();
			services.AddTransient<ProgressService>();			
			services.AddSignalR();

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app,IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseStaticFiles();

			app.UseSignalR(routes =>
			{
				routes.MapHub<UpdaterHub>("/updaterDemo");
				routes.MapHub<ProgressHub>("/progressDemo");
				routes.MapHub<ClockHub>("/clockDemo");
			});

			app.UseMvcWithDefaultRoute();
		}
	}
}
