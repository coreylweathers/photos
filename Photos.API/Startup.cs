using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Photos.Shared.Models.Options;
using Photos.Shared.Options;
using Photos.Shared.Services;

namespace Photos.API
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
            services.AddControllers();

            // ADD TWILIOSERVICE
            services.AddTransient<ITwilioService, TwilioService>();
            services.AddSingleton<IStorageService, TableStorageService>();

            // ADD TWILIOOPTIONS
            services.Configure<TwilioOptions>(Configuration.GetSection("Twilio"));
            services.Configure<TableOptions>(Configuration.GetSection("TableStorage"));

            // ADDING CORS SUPPORT
            services.AddCors(opts => 
            {
                opts.AddDefaultPolicy(policy => 
                {
                    policy.WithOrigins("https://localhost:44390")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
