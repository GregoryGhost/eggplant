namespace Eggplant.Telegram
{
#if RELEASE
    using System;
#endif

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
#if DEBUG
            return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
#else
            var port = Environment.GetEnvironmentVariable("PORT");
            if (port is null)
                throw new NullReferenceException("Not setted the bot port");

            return Host.CreateDefaultBuilder(args)
                       .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>().UseUrls("http://*:" + port));
#endif
        }
    }
}