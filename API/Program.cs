using API.Data;
using API.Middleware;
using API.Middleware.LogHandling.ExceptionHandling;
using API.Middleware.LogHandling.ResponseHandle;
using API.Models.Entities;
using API.Services.Token;
using Shared.DTO.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Please enter the token in the format 'Bearer {your token}'",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    options.AddSecurityDefinition("Bearer", jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>((serviceProvider, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Identity's config 
builder.Services.AddIdentity<SystemOperator, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddRoleManager<RoleManager<IdentityRole>>()
.AddDefaultTokenProviders()
.AddApiEndpoints();

// JWT's config
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Recommendation: Enable it in production
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"])),

        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role,

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7106") // URL del cliente Blazor
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Configuración de autorización
builder.Services.AddAuthorization();

builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ResponseMiddleware>();

app.UseMiddleware<ExceptionMiddleware>();

//app.UseMiddleware<RolesMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowBlazorClient");

app.UseAuthentication();
app.UseAuthorization();

//app.MapIdentityApi<SystemOperator>();  This maps every identity's endpoint
// Manually map each desired endpoint
app.MapPost("/identity/register", async (
    [FromBody] RegisterRequest request,
    UserManager<SystemOperator> userManager) =>
{
    var user = new SystemOperator { Email = request.Email, UserName = request.Email };
    var result = await userManager.CreateAsync(user, request.Password);

    return result.Succeeded
        ? Results.Ok("Usuario creado")
        : Results.BadRequest(result.Errors);
});

app.MapPost("/identity/forgotPassword", async (
    [FromBody] ForgotPasswordRequest request,
    UserManager<SystemOperator> userManager,
    IEmailSender emailSender) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user == null) return Results.NotFound();

    var token = await userManager.GeneratePasswordResetTokenAsync(user);
    await emailSender.SendEmailAsync(request.Email, "Resetear contraseña", $"Token: {token}");

    return Results.Ok();
});

app.MapPost("/identity/resetPassword", async (
    [FromBody] ResetPasswordRequest request,
    UserManager<SystemOperator> userManager) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user == null)
    {
        return Results.NotFound();
    }

    // Reset the password using the token provided by the user
    var result = await userManager.ResetPasswordAsync(user, request.ResetCode, request.NewPassword);

    return result.Succeeded
        ? Results.Ok("La contraseña ha sido reestablecida correctamente.")
        : Results.BadRequest(result.Errors);
});

app.MapPost("/identity/confirmEmail", async (
    [FromBody] ConfirmEmailRequestDTO request,
    UserManager<SystemOperator> userManager) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user == null)
    {
        return Results.NotFound("Usuario no encontrado.");
    }

    var result = await userManager.ConfirmEmailAsync(user, request.Token);

    return result.Succeeded
        ? Results.Ok("Email confirmado correctamente.")
        : Results.BadRequest(result.Errors);
});

app.MapPost("/identity/resendConfirmationEmail", async (
    [FromBody] ResendConfirmationEmailRequest request,
    UserManager<SystemOperator> userManager,
    IEmailSender emailSender) =>
{
    var user = await userManager.FindByEmailAsync(request.Email);
    if (user == null)
    {
        return Results.NotFound();
    }

    // Generate an email confirmation token
    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

    // Optionally, encode the token if needed
    // token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

    // Create the confirmation link. Adjust the URL to your application's frontend route.
    var confirmationLink = $"https://yourdomain.com/confirm?email={Uri.EscapeDataString(request.Email)}&token={Uri.EscapeDataString(token)}";

    // Send the confirmation email
    await emailSender.SendEmailAsync(
        request.Email,
        "Confirma tu email",
        $"Por favor confirma tu cuenta haciendo click <a href='{confirmationLink}'>acá</a>.");

    return Results.Ok("Mail de confirmación enviado.");
});

app.MapControllers();

app.MapRazorPages();

//// Db seeding for roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Operador" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

app.Run();
