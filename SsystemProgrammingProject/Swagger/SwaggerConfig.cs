using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SsystemProgrammingProject.Swagger;

public static class SwaggerConfig
{
    // Метод для настройки опций Swagger
    public static void GenerateOptions(SwaggerGenOptions options)
    {
        // Определение схемы безопасности
        // Здесь мы добавляем параметр Authorization для Swagger UI
        options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization", // Название в заголовке
            Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`", // Описание для Swagger UI
            In = ParameterLocation.Header, // Передавать через заголовок
            Type = SecuritySchemeType.Http, // По HTTP
            Scheme = "Bearer" // Схема "Bearer"
        });

        // Требования безопасности для всех запросов
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Name = "Bearer", // Название
                    In = ParameterLocation.Header, // Указываем, что токен в заголовке
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer", // ID
                        Type = ReferenceType.SecurityScheme // Ссылка на ранее определенную схему безопасности
                    }
                },
                new List<string>() // Пустой список, так как доп требований к ролям нет
            }
        });
    }
}
