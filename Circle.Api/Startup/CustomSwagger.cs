using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Circle.Api.Startup
{
    public static class CustomSwagger
    {
        public static void UseCustomSwaggerApi(this IApplicationBuilder app, string name = "Ipos APIs Documentation")
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint
            //app.UseSwagger();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "swagger/{documentname}/swagger.json";

            });

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", name);
                c.DocExpansion(DocExpansion.None);

            });
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services, string xmlCommentFile,
            string title = "Ipos APIs Documentation"
           )
        {
            services.AddSwaggerGen(c =>
            {
                //c.CustomSchemaIds(i => i.FullName);
                c.IncludeXmlComments(xmlCommentFile, true);

                c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
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
                            new string[] {}
                    }
                });
            });
            return services;
        }
    }
}
