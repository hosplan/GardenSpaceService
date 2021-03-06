using GardenSpaceService.Data;
using GardenSpaceService.Services;
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
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GardenSpaceService
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
            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "GardenSpaceService", Version = "v1" });
            });
            services.AddCors();
            services.AddDbContext<GardenSpaceContext>(options =>
             options.UseSqlServer("Server=192.168.0.10,1433;Database=GardenSpaceSDB;User Id=SA;Password=emth022944w!;"));

            //interface ?߰?
            services.AddScoped<IJWTService, JWTService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Headers.ContainsKey("Authorization"))
                        {
                            context.Token = context.Request.Headers["Authorization"];
                        }
                        else if (context.Request.Query.ContainsKey("token"))
                        {
                            context.Token = context.Request.Query["token"];
                        }

                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //Issuer?? ??ȿ?? ????
                    ValidateIssuer = true,
                    //Audience?? ??ȿ?? ????
                    ValidateAudience = true,
                    //Token?? ?????ֱ?
                    ValidateLifetime = true,
                    //Token?? ??ȿ???? ????
                    ValidateIssuerSigningKey = true,
                    //Token?? ??????
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    //û???? ?????Ѵ? Ư???? ???찡 ?ƴϸ? JWT?????? ?????ϴ? ?????? ????
                    ValidAudience = Configuration["Jwt:Audience"],
                    //Token?? ?????? ??ȣȭŰ ????
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                    //?ð??? Ȯ???? ?? ?????? Ŭ?? ??ť ???? ?ð? ????
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "GardenSpaceService v1"));
            }

           app.UseCors(builder =>
           builder.WithOrigins("https://localhost:49155")
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowCredentials());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
