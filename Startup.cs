using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using JokesAPI.Models;
using JokesAPI.Middleware;


namespace JokesAPI
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
            // This is interesting.  I was able to easily swap out data repositories using Entity Framework.
            // InMemory, then I switched to SqlServer, then to SqlList.
            // I was able to swap out data store types and not have to retool the API.
            // Nice.

            //services.AddDbContext<JokesContext>(opt =>  opt.UseInMemoryDatabase("JokesList"));
            //  services.AddDbContext<JokesContext>(option => option.UseSqlServer(@"Data Source=CCARTERDEV\SETBASE;Initial Catalog=JokesDb;Trusted_Connection=True;"));

            var connection = Configuration.GetConnectionString("JokesDatabase");
            services.AddDbContext<JokesContext>(option => option.UseSqlite(connection));
            
            services.AddControllers();

            //// Register the Swagger API Documentation generator
            services.AddSwagger();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Dev Environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCustomSwagger();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

    }
}
