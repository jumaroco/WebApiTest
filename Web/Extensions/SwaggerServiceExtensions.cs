﻿using Microsoft.OpenApi.Models;

namespace Web.Extensions;

public static class SwaggerServiceExtensions
{
    public static IServiceCollection AddSwaggerDocumentation
    (
        this IServiceCollection services
    )
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen( c =>
        {
            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization Bearer Schema",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            c.AddSecurityDefinition("Bearer", securitySchema);

            var sercurityRequirement = new OpenApiSecurityRequirement
            {
                {
                    securitySchema, new [] {"Bearer"}
                }
            };

            c.AddSecurityRequirement( sercurityRequirement );
        });

        return services;
    }

    public static IApplicationBuilder UseSwaggerDocumentation
    (
      this IApplicationBuilder app
    )
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
