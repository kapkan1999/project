using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using LAB.Models;
using LAB.Storage;
using Serilog;

namespace LAB
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {   Log.Information("Information in Startup:");
            Log.Warning("Some warning in Startup");
            Log.Error("Here comes an error in Startup");
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
         public void ConfigureServices(IServiceCollection services)
       {    Log.Information("Information in ConfigureServices:");
            Log.Warning("Some warning in ConfigureServices");
            Log.Error("Here comes an error in ConfigureServices");
           services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                       ConfigureLogger();
           switch (Configuration["Storage:Type"].ToStorageEnum())
           {
               case StorageEnum.MemCache:
                   services.AddSingleton<IStorage<MyModData>, MemCache>();
                   break;
               case StorageEnum.FileStorage:
                   services.AddSingleton<IStorage<MyModData>>(
                       x => new FileStorage(Configuration["Storage:FileStorage:Filename"], int.Parse(Configuration["Storage:FileStorage:FlushPeriod"])));
                   break;
               default:
                   throw new IndexOutOfRangeException($"Storage type '{Configuration["Storage:Type"]}' is unknown");
           }
            services.AddScoped<StorageService, StorageService>();
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {   Log.Information("Information in Configure:");
            Log.Warning("Some warning in Configure");
            Log.Error("Here comes an error in Configure");
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
            //Log.Information($"RollingInterval {}");
            Log.Debug($"Full log info app: {@app}");
            Log.Debug($"Full log info env: {@env}");
        }
        private void ConfigureLogger()
        {   Log.Information("Information:");
            Log.Warning("Some warning");
            Log.Error("Here comes an error");

            var log = new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.File("logs\\LAB.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();                
                Log.Logger = log;
                //Log.Information($"RollingInterval {}");
                Log.Debug($"Full log info: {@log}");
        }

    }
}