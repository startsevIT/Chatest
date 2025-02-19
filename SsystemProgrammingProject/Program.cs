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
app.MapGet("/users/account",async (HttpContext ctx) =>
{
	foreach (var item in ctx.Response.Headers)
		Console.WriteLine(item.Key + " " + item.Value);
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

app.Run();