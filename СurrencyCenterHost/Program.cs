using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace СurrencyCenterHost
{
    class Program
    {
        static Timer timer;
        static IHubContext<ChatHub> _hubContext;

        static void Main(string[] args)
        {
            timer = new Timer(MakeInform, null, 0, 200);
            var host = CreateHostBuilder(args).Build();
            _hubContext = host.Services.GetService(typeof(IHubContext<ChatHub>)) as IHubContext<ChatHub>;
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => {
                    webBuilder.UseStartup<Startup>();
                });

        private static async void MakeInform(object state)
        {
            if (_hubContext != null)
            {
                HttpClient httpClient = new HttpClient();
                string request = "https://poloniex.com/public?command=returnTicker";
                HttpResponseMessage response =
                    (await httpClient.GetAsync(request)).EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                await _hubContext.Clients.All.SendCoreAsync("ReceiveMessage", new[] { responseBody });
            }
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapHub<ChatHub>("/chatHub");
                });
        }
    }
}
