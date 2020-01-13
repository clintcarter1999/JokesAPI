using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace JokesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Setup Serilog writing to the datalust Seq structured log viewer.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Seq(Environment.GetEnvironmentVariable("SEQ_URL") ?? "http://localhost:5341")
                .CreateLogger();

            try
            {
                Log.Information("Starting Up Jokes REST API");

                CreateHostBuilder(args).Build().Run();

                Log.Information("Successfully started the Jokes REST API");
            }
            catch (Exception ex1)
            {
                Log.Fatal(ex1, "Unable to start Jokes API due to Unexpected Error in Program.cs");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
