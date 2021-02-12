using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AutoLotDALCore.DataInitialization;
using AutoLotDALCore.EF;
using Microsoft.Extensions.DependencyInjection;

namespace AutoLotAPICore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var hostScope = host.Services.CreateScope())
            {
                var services = hostScope.ServiceProvider;
                var context = services.GetRequiredService<AutoLotContext>();
                MyDataInitializer.RecreateDataBase(context);
                MyDataInitializer.InitializeData(context);
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
