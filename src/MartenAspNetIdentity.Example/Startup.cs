using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Examples.MvcSecurity.Data;
using Examples.MvcSecurity.Services;
using MartenAspNetIdentity;

namespace Examples.MvcSecurity
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// docker run -d -p 5432:5432 --name aspnetidentity-postgres -e POSTGRES_USER=aspnetidentity -e POSTGRES_PASSWORD=aspnetidentity postgres
		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging();

			string connectionString = "server=localhost;database=aspnetidentity;uid=aspnetidentity;pwd=aspnetidentity;";

			services.AddIdentity<ApplicationUser, IdentityRole>()
				.AddMartenStores<ApplicationUser, IdentityRole>(connectionString)
				.AddDefaultTokenProviders();

			services.AddMvc()
				.AddRazorPagesOptions(options =>
				{
					options.Conventions.AuthorizeFolder("/Account/Manage");
					options.Conventions.AuthorizePage("/Account/Logout");
				});

			// Register no-op EmailSender used by account confirmation and password reset during development
			// For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
			services.AddSingleton<IEmailSender, EmailSender>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseBrowserLink();
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Error");
			}

			app.UseStaticFiles();

			app.UseAuthentication();

			app.UseMvc();
		}
	}
}