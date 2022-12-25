﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MP.Infrastructure.Logger;
using MP.Infrastructure.Mailer;
using MP.Infrastructure.Persistance.Mssql;
using MP.Infrastructure.Persistance.Redis;
using System;
using System.IO.Compression;
using System.Text;

namespace MP.Api.CreditCardApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            LoggerRegister.Register(services);
            MailerRegister.Register(services);
            PersistanceMssqlRegister.Register(services);
            PersistanceRedisRegister.Register(services);

            //services.Configure<BrotliCompressionProviderOptions>(options =>
            //{
            //    options.Level = CompressionLevel.Optimal;
            //});

            services.Configure(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
                options.HttpOnly = HttpOnlyPolicy.Always;
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Credit Card API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "'Bearer' kelimesinden sonra boşluk bırakarak tokenı yazınız. Token almak için 'http://localhost:4479/auth/test-user' adresini kullanın\r\n\r\nÖrnek: \"Bearer xxx\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:56689").AllowAnyHeader().AllowAnyMethod();
                    });
            });


            services.AddResponseCompression(options =>
            {
                options.Providers.Add<BrotliCompressionProvider>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Credit Card API V1"));
            }
            app.Use(async (context, next) =>
            {
                await next();
            });
            app.UseHttpsRedirection();
            app.UseResponseCompression();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors();
            app.UseHsts();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}