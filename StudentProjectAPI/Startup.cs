using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StudentProjectAPI.DataModels;
using StudentProjectAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StudentProjectAPI
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
            //cors policy hatas� ��z�m
            services.AddCors(o => o.AddPolicy("MyPolicy", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddDbContext<ProjectContext>(options => options.UseSqlServer(Configuration.GetConnectionString("StudentPortalDb"))); //bu kod ile sql ba�lant�s� sa�lan�or

             //burada miras al�nan yer i�in ekleme yap�yoruz.
            services.AddScoped<IStudentRepository, SqlStudentRepository>();
            services.AddScoped<IImageRepository, LoacStorageImageRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StudentProjectAPI", Version = "v1" });
            });

            services.AddAutoMapper(typeof(Startup).Assembly); //bu kod ile proje a��ld���nda map leme i�leminin yap�lmas� sa�lan�yor
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentProjectAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider=new PhysicalFileProvider(Path.Combine(env.ContentRootPath,"Resources")),
                RequestPath = "/Resources"
            });

            app.UseRouting();

            //yukar�da tan�mlanan corsdaki ismi burada tan�mlamak gerekiyor.
            app.UseCors("MyPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
