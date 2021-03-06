using GerenciadorCursos.Data;
using GerenciadorCursos.Repository;
using GerenciadorCursos.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GerenciadorCursos
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

           var configuracoesSection =  Configuration.GetSection("ConfiguracoesJWT");
            var configuracoesJWT = configuracoesSection.Get<ConfiguracoesJWT>();

            services.Configure<ConfiguracoesJWT>(configuracoesSection);


            services.AddControllers().AddJsonOptions(x =>
            {
                // serialize enums as strings in api responses (e.g. Role)
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });


            services.AddDbContext<GerenciadorContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MeuSqlServer")
                    );
            });

             services.AddScoped<IRepository, cursorepository>();

            services.AddAuthentication(opcoes =>
            {
                opcoes.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                opcoes.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                    .AddJwtBearer(opcoes =>
                    {
                        opcoes.TokenValidationParameters = new TokenValidationParameters
                        {
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuracoesJWT.Segredo)),
                            ValidAudience = "https://localhost:44398",
                            ValidIssuer = "CursosValid",
                            ValidateLifetime = true,
                            ClockSkew = TimeSpan.Zero
                        };

                    });


            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GerenciadorCursos", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                       "JWT Authorization Header - utilizado com Bearer Authentication.\r\n\r\n" +
                       "Digite 'Bearer' [espa?o] e ent?o seu token no campo abaixo.\r\n\r\n" +
                       "Exemplo (informar sem as aspas): 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });  
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GerenciadorCursos v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });




        }
    }
}
