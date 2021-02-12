using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using AutoLotDALCore.EF;
using AutoLotDALCore.Repos;
using Microsoft.Extensions.Hosting;
using AutoLotAPICore.Filters;

namespace AutoLotAPICore
{
    public class Startup
    {
        private readonly IWebHostEnvironment webHostEnv;
        public Startup(IConfiguration configuration,
            IWebHostEnvironment webHostEnv)
        {
            this.webHostEnv = webHostEnv;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // from camelCase to PascalCase
            services.AddControllers(
                config => config.Filters.Add(new AutoLotExceptionFilter(webHostEnv)))
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = null;
                    opt.JsonSerializerOptions.DictionaryKeyPolicy = null;
                });

            services.AddDbContextPool<AutoLotContext>(
                opt => opt.UseSqlServer(Configuration.GetConnectionString("AutoLot"),
                o => o.EnableRetryOnFailure())
                .ConfigureWarnings(warn => warn.Throw(/*Skip because have no eventId*/)));

            services.AddScoped<IInventoryRepo, InventoryRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
