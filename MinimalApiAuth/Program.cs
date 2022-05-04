using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MinimalApiAuth;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MinimalApiAuth.Models;
using MinimalApiAuth.Repositories;
using MinimalApiAuth.Services;

var key = Encoding.ASCII.GetBytes(Configuration.Secret);

var builder = WebApplication.CreateBuilder(args);

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
app.UseAuthorization();

app.MapGet("/anonymous", () => { Results.Ok("Anônimo"); }).AllowAnonymous();

app.MapPost("/accounts/login", (UserModel model) =>
{
    var user = UserRepository.Get(model.UserName, model.Password);

    if (user is null)
        return Results.NotFound(new {message = "Nome de usuário ou senha incorretos."});

    var token = TokenService.GenerateToken(user);

    return Results.Ok(new {user, token});
});

app.MapGet("/authenticated", (ClaimsPrincipal user) =>
{
    if (user.Identity != null)
        return Results.Ok(new {message = $"Autenticação efetuada com sucesso para {user.Identity.Name}."});
    
    return Results.NotFound(new {message = "Não foi possível recuperar as informações de usuário."});
}).RequireAuthorization();

app.MapGet("/main/admin", (ClaimsPrincipal user) =>
{
    if (user.Identity != null)
        return Results.Ok(new {message = $"Autenticação efetuada com sucesso para {user.Identity.Name}."});
    
    return Results.NotFound(new {message = "Não foi possível recuperar as informações de usuário."});
}).RequireAuthorization("Admin");

app.MapGet("/main/employee", (ClaimsPrincipal user) =>
{
    if (user.Identity != null)
        return Results.Ok(new {message = $"Autenticação efetuada com sucesso para {user.Identity.Name}."});
    
    return Results.NotFound(new {message = "Não foi possível recuperar as informações de usuário."});
}).RequireAuthorization("Employee");

app.Run();