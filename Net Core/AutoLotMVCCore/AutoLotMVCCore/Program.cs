using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using AutoLotDALCore.DataInitialization;
using AutoLotDALCore.EF;
using Microsoft.Extensions.DependencyInjection;

namespace AutoLotMVCCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<AutoLotContext>();
                MyDataInitializer.RecreateDataBase(context);
                MyDataInitializer.InitializeData(context);
            }
            webHost.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
