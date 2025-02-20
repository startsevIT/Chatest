using ChatestDomain.BusinessEntities.DTOs;
using ChatestInfrastructure.Auth;
using ChatestInfrastructure.Storage.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SsystemProgrammingProject.Swagger;

var builder = WebApplication.CreateBuilder();

// Добавление сервисов авторизации и аутентификации (JWT)
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = AuthService.BuildJwtTokenParameters());

// Добавление Swagger с настройками безопасности
builder.Services.AddSwaggerGen(SwaggerConfig.GenerateOptions);

var app = builder.Build();

// Подключение middleware для аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Подключение Swagger UI для документации API
app.UseSwagger();
app.UseSwaggerUI();

// Репозиторий для работы с пользователями
UserRepo userRepo = new();

// Регистрация пользователя
app.MapPost("/users/register", async (RegisterUserDTO dto) =>
{
    try
    {
        await userRepo.RegisterAsync(dto); // Регистрируем пользователя на основенне данных
        return Results.Created(); // Возвращаем созданное
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex); // Возвращаем ошибку, если регистрация не удалась
    }
});

// Авторизация пользователя
app.MapPost("/users/login", async (LoginUserDTO dto) =>
{
    try
    {
        return Results.Ok(await userRepo.LoginAsync(dto)); // Возвращаем токен при успешном входе
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
});

// Получение информации о текущем пользователе
app.MapGet("/users/account", async (HttpContext ctx) =>
{
    // Вывод заголовков ответа в консоль (для отладки)
    foreach (var item in ctx.Response.Headers)
        Console.WriteLine(item.Key + " " + item.Value);

    try
    {
        Guid userId = new(ctx.User.FindFirst("id").Value); // Получаем ID пользователя из токена
        return Results.Ok(await userRepo.ReadAsync(userId)); // Возвращаем найденную информацию
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex);
    }
});

// Репозиторий для работы с чатами
ChatRepo chatRepo = new();

// Создание нового чата (требуется аутентификация)
app.MapPost("/chats", [Authorize] async (CreateChatDTO dto, HttpContext ctx) =>
{
    try
    {
        Guid userId = new(ctx.User.FindFirst("id").Value); // Получаем ID пользователя из токена
        await chatRepo.CreateAsync(dto, userId); // Создаем чат от имени пользователя
        return Results.Created();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
});

// Получение информации о чате (проверка наличия пользователя в чате)
app.MapGet("/chats/{chatId}", async (Guid chatId, HttpContext ctx) =>
{
    try
    {
        Guid userId = new(ctx.User.FindFirst("id").Value); // Получаем ID пользователя из токена
        return Results.Ok(await chatRepo.ReadAndLinkAsync(chatId, userId)); // Читаем чат и проверяем доступ
    }
    catch (Exception ex)
    {
        return Results.NotFound(ex);
    }
});

// Запуск приложения
app.Run();
