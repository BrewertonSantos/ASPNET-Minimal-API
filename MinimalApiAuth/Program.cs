using Microsoft.AspNetCore.Authentication.JwtBearer;
using MinimalApiAuth;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MinimalApiAuth.Models;
using MinimalApiAuth.Repositories;
using MinimalApiAuth.Services;

var builder = WebApplication.CreateBuilder(args);

var key = Encoding.ASCII.GetBytes(Configuration.Secret);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("manager"));
    options.AddPolicy("Employee", policy => policy.RequireRole("employee"));
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthentication();

app.MapPost("/accounts/login", (UserModel model) =>
{
    var user = UserRepository.Get(model.UserName, model.Password);

    if (user is null)
        return Results.NotFound(new {message = "Nome de usuário ou senha incorretos."});

    var token = TokenService.GenerateToken(user);

    return Results.Ok(new {user, token});
});

app.MapGet("/anonymous", () => { Results.Ok("Anônimo"); }).AllowAnonymous();

app.Run();