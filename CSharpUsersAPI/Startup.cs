using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpUsersAPI.Mapeador;
using CSharpUsersAPI.Utils.Singletons;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using CSharpUsersAPI.Utils.JWT;
using CSharpUsersAPI.Utils;

namespace CSharpUsersAPI
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
            services.AddSingleton<IAuthorizationHandler, ValidJWTHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllers();

            Mapeador.Mapeador.Obter().RegistrarMapeamentos();

            string connectionStringName = Configuration.GetSection("ConnectionStringOptions:Name").Value;
            string connectionStringProviderName = Configuration.GetSection("ConnectionStringOptions:ProviderName").Value;
            string connectionString = Configuration.GetSection("ConnectionStringOptions:ConnectionString").Value;
            ConnectionStringSettingsSingleton.Obter().setConnectionStringSettings(connectionStringName, connectionString, connectionStringProviderName);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("JWT", policy =>
                    policy.Requirements.Add(new ValidJWTRequirement()));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CSharpUsersAPI",
                    Version = "v1"
                });
                c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.HttpMethod}_{apiDesc.RelativePath}");
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },new List<string>()
                    }
                });
            });

            services.AddHttpContextAccessor();

            services.AddCors();

            services.AddControllers()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                options.HttpsPort = 443;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(
                options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
            );

            if (env.IsDevelopment())
            {

            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CSharpUsersAPI v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //UserUtils.SetHttpContextAccessor(app.ApplicationServices.GetService<IHttpContextAccessor>());
        }
    }
}
