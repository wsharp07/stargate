using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stargate.API.Data;
using AutoMapper;
using Stargate.API.Data.Repository;
using Stargate.API.Services;

namespace Stargate.API
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
            services.AddMvc();
            services.AddCors();
            // Database
            var connectionString = Configuration.GetConnectionString("StargateContext");

            services.AddDbContext<StargateContext>(options =>
                options.UseSqlite(connectionString));

            services.AddAutoMapper();

            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var seeder = scope.ServiceProvider.GetService<StargateSeeder>();
                    seeder.Seed();
                }
            }

            app.UseCors(
                options => options
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
            app.UseMvc();
            
        }

        /// <summary>
        /// Setup dependencies 
        /// </summary>
        /// <param name="services"></param>
        private void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<StargateSeeder>();
            services.AddScoped<IStargateRepository, StargateRepository>();
            services.AddScoped<IFileUploader, AzureBlobFileUploader>();
            services.AddSingleton<IUriShortener, BijectiveUriService>();
            services.AddSingleton<IConfiguration>(Configuration);
        }
    }
}
