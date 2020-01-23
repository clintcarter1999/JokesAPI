using System;
using JokesAPI.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;


namespace JokesAPI
{
    public static class Program
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

                IHost host = CreateHostBuilder(args).Build();


                //////
                ////// If your DB is empty then you can uncomment this section and it will provide seed data.
                ////// I need to figure out the Seeding method...perhaps moving this into OnModelCreating and using 
                ////// the modelBuilder.Entity<JokeItems>().HasData( ...new JokeItem { } ... );
                //////
                ////// TODO: Figure Entity Frameworks Seeding Model which makes use of Migrations
                ////// https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
                ////// https://www.learnentityframeworkcore.com/migrations/seeding
                //////
                ////using (var scope = host.Services.CreateScope())
                ////{
                ////    var services = scope.ServiceProvider;
                ////    var context = services.GetService<JokesContext>();


                ////    DbHelper.InsertData(context);
                ////}

                host.Run();

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
