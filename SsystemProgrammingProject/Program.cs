using ChatestDomain.BusinessEntities.DTOs;
using ChatestInfrastructure.Auth;
using ChatestInfrastructure.Storage.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using SsystemProgrammingProject.Swagger;

var builder = WebApplication.CreateBuilder();
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(o => o.TokenValidationParameters = AuthService.BuildJwtTokenParameters());
builder.Services.AddSwaggerGen(SwaggerConfig.GenerateOptions);

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI();
app.UseWebSockets();

UserRepo userRepo = new();
app.MapPost("/users/register", async (RegisterUserDTO dto) =>
{
	try
	{
        await userRepo.RegisterAsync(dto);
		return Results.Created();
    }
	catch (Exception ex)
	{
		return Results.BadRequest(ex);
	}
});
app.MapPost("/users/login", async (LoginUserDTO dto) =>
{
	try
	{
		return Results.Ok(await userRepo.LoginAsync(dto));
	}
	catch (Exception ex)
	{
		return Results.BadRequest(ex);
	}
});
app.MapGet("/users/account", [Authorize] async (HttpContext ctx) =>
{
	try
	{
        Guid userId = new(ctx.User.FindFirst("id").Value);
        return Results.Ok(await userRepo.ReadAsync(userId));
	}
	catch (Exception ex)
	{

		return Results.NotFound(ex);
	}
});

ChatRepo chatRepo = new();
app.MapPost("/chats", [Authorize] async (CreateChatDTO dto, HttpContext ctx) =>
{
    try
    {
		Guid userId = new(ctx.User.FindFirst("id").Value);
        await chatRepo.CreateAsync(dto, userId);
        return Results.Created();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex);
    }
});
app.MapGet("/chats/{chatId}", async (Guid chatId, HttpContext ctx) =>
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
