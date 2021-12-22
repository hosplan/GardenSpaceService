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

            //interface 추가
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
                    //Issuer의 유효성 여부
                    ValidateIssuer = true,
                    //Audience의 유효성 여부
                    ValidateAudience = true,
                    //Token의 생명주기
                    ValidateLifetime = true,
                    //Token의 유효성을 검증
                    ValidateIssuerSigningKey = true,
                    //Token의 발행자
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    //청중을 지정한다 특별한 경우가 아니면 JWT인증을 수행하는 도메인 지정
                    ValidAudience = Configuration["Jwt:Audience"],
                    //Token을 발행할 암호화키 지정
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),
                    //시간을 확인할 대 적용할 클럭 스큐 단위 시간 설정
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
           builder.WithOrigins("https://localhost:49157")
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
