using ChatestDomain.BusinessEntities.DTOs;
using ChatestInfrastructure.Auth;
using ChatestInfrastructure.Storage.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SsystemProgrammingProject.Helpers;
using SsystemProgrammingProject.Swagger;
using SsystemProgrammingProject.WebSockets;

var builder = WebApplication.CreateBuilder();

// Добавление сервисов авторизации и аутентификации (JWT)
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o => o.TokenValidationParameters = AuthService.BuildJwtTokenParameters());

// Добавление Swagger с настройками безопасности
builder.Services.AddSwaggerGen(SwaggerConfig.GenerateOptions);

builder.Services.AddCors();


var app = builder.Build();

// Подключение middleware для аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

// Подключение Swagger UI для документации API
app.UseSwagger();
app.UseSwaggerUI();
app.UseWebSockets();

app.UseCors(options =>
    options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());



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
        return Results.BadRequest(ex.Message); // Возвращаем ошибку, если регистрация не удалась
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
        return Results.BadRequest(ex.Message);
    }
});

// Получение информации о текущем пользователе
app.MapGet("/users/account", [Authorize] async (HttpContext ctx) =>
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

app.MapGet("/users", async () =>
{
    return await userRepo.ListAsync();
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
app.MapGet("/chats/{chatId}", [Authorize] async (Guid chatId, HttpContext ctx) =>
{
	try
	{
        Guid userId = new(ctx.User.FindFirst("id").Value);
        return Results.Ok(await chatRepo.ReadAndLinkAsync(chatId, userId));
	}
	catch (Exception ex)
	{
		return Results.NotFound(ex);
	}
});



MessageRepo messageRepo = new();
Hub hub = new ();
app.Map("/connect/{roomId}", [Authorize] async (HttpContext context, Guid roomId) =>
{
    Guid userId = new(context.User.FindFirst("id").Value);

	if (!await chatRepo.CheckUserAsync(roomId, userId))
		return Results.NotFound();

    var websocket = await context.WebSockets.AcceptWebSocketAsync();

	hub.AddSocket(roomId, websocket);

	var WSResult = await hub.ReceiveResultAsync(websocket);

	while (!WSResult.CloseStatus.HasValue)
	{
		string messageText = hub.EncodingMessage(WSResult);

        Guid messageId = await messageRepo.CreateAsync(new (messageText),userId, roomId);

		ReadMessageDTO dto = await messageRepo.ReadAsync(messageId);

		var bytes = MessageEncoder.MessageSerialize(dto);
        await hub.SendAllAsync(roomId, WSResult, bytes);

		WSResult = await hub.ReceiveResultAsync(websocket);
	}

    hub.RemoveSocket(roomId,websocket);
	return Results.Ok();
});

app.Run();
