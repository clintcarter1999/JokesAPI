using System.Text;
using AutoMapper;
using JokesAPI.Middleware;
using JokesAPI.Models;
using JokesAPI.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;


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
            var connection = Configuration.GetConnectionString("JokesDatabase");
            services.AddDbContext<AppDbContext>(option => option.UseSqlite(connection));

            services.AddScoped<IJokesRepository, JokesRepository>();

            //TODO: I need to research Cors this further.  
            // https://thecodebuzz.com/enable-cors-asp-net-core/
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().Build());
            });

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };

                });


            services.AddMvc();
            
            services.AddAutoMapper(typeof(Startup));

            services.AddSwagger();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (app == null)
                throw new System.Exception("app was null inside Startup.Configure(IApplicationBuilder");

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

            app.EnsureDatabaseIsSeeded();

            // Handles non-success status codes with empty body
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            // C.Carter 1/16/2020
            //
            // Something in this middleware was causing my authorization to not work.
            // Commented out for now. At least that is my best guess.  It was the last thing I added.
            // Auth stopped working...even taking [Authorize] attributes off the put was causing
            // problems with the POST/PUT.  Will work on adding back after committing a working 
            // copy of the API.
            // app.UseMiddleware<RequestResponseLoggingMiddleware>();

            app.UseCustomSwagger();

      


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }

    }
}
