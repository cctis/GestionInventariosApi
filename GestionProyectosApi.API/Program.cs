using ApiTableroPowerBiFepep.Domain.Models;
using ApiTableroPowerBiFepep.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiTableroPowerBIFepep.Utils.Security;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
IConfigurationSection seccionConfiguracion = builder.Configuration.GetSection("SectionConfiguration");
builder.Services.Configure<SectionConfiguration>(seccionConfiguracion);
SectionConfiguration? configuracionAppSettings = seccionConfiguracion.Get<SectionConfiguration>();
IEncryptionService Encrypt = new EncryptionService();
string connectionVisionQuality = builder.Configuration.GetValue<string>("ConnectionStrings:ConnetionVQualityFepep") ?? "";
string connectionVisionCenter = builder.Configuration.GetValue<string>("ConnectionStrings:ConnetionVQualityFepep") ?? "";
string GetConnectionStringVisionQuality = Encrypt.Decrypt(connectionVisionQuality);
string GetConnectionStringVisionCenter = Encrypt.Decrypt(connectionVisionCenter);
builder.Services.AddDbContext<ContextSql>(opt => opt.UseSqlServer(GetConnectionStringVisionQuality));
builder.Services.AddDbContext<ContextSql>(opt => opt.UseSqlServer(GetConnectionStringVisionCenter));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:SecretKey"]))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddRazorPages();

builder.Services.AddCors(options => options.AddPolicy("AllowPolicySecureDomains", x =>
{
    _ = x.AllowAnyOrigin()
     .WithOrigins(configuracionAppSettings.SecureDomains)
     .AllowAnyHeader()
     .AllowCredentials()
     .AllowAnyMethod();
}));

// Build de la aplicación
WebApplication app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    _ = app.UseExceptionHandler("/Error");
    _ = app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

// Ejecutar la aplicación
app.Run();