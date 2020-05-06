using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAB_A3_SocialNetwork.Controllers;
using DAB_A3_SocialNetwork.Models;
using DAB_A3_SocialNetwork.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using DAB_A3_SocialNetwork;
using DAB_A3_SocialNetwork.Controllers;

namespace DAB_A3_SocialNetwork
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
            // requires using Microsoft.Extensions.Options
            services.Configure<SocialNetworkDBSettings>(
                Configuration.GetSection(nameof(SocialNetworkDBSettings)));

            services.AddSingleton<ISocialNetworkDBSettings>(sp =>
                sp.GetRequiredService<IOptions<SocialNetworkDBSettings>>().Value);

            services.AddSingleton<DatabaseServices>();

            services.AddControllers().AddNewtonsoftJson(options => options.UseMemberCasing());

           

        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
