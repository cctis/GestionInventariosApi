using GestionInventariosApi.Application.Services;
using GestionInventariosApi.Domain.Models;
using GestionInventariosApi.Infrastructure.Context;
using GestionInventariosApi.Utils.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
var builder = WebApplication.CreateBuilder(args);
IEncryptionService Encrypt = new EncryptionService();
IConfigurationSection seccionConfiguracion = builder.Configuration.GetSection("SectionConfiguration");
IConfigurationSection seccionConnectionStrings = builder.Configuration.GetSection("ConnectionStrings");

builder.Services.Configure<SectionConfiguration>(seccionConfiguracion);
builder.Services.Configure<ConnectionStrings>(seccionConnectionStrings);
var configuracionAppSettings = seccionConfiguracion.Get<SectionConfiguration>();
var configuracionConnectionStrings = seccionConnectionStrings.Get<ConnectionStrings>();

string DecryptConnectionString(string encryptedConnectionString)
{
    return string.IsNullOrEmpty(encryptedConnectionString) ? null : Encrypt.Decrypt(encryptedConnectionString);
}

if (builder.Configuration.GetSection("ConnectionStrings:ConnetionToken").Exists())
{
    string GetConnetionToken = DecryptConnectionString(configuracionConnectionStrings.ConnetionToken);
}

if (builder.Configuration.GetSection("ConnectionStrings:ConnetionGestionInventario").Exists())
{
    string ConnetionGestionInventario = DecryptConnectionString(configuracionConnectionStrings.ConnetionGestionInventario);
    if (!string.IsNullOrEmpty(ConnetionGestionInventario))
    {
        builder.Services.AddDbContext<ContextSql>(opt => opt.UseSqlServer(ConnetionGestionInventario));
    }
}

builder.Services.AddScoped<IEncryptionService, EncryptionService>();

#region Registro dinámico de servicios (Dynamic Services Injection)
var generalServices = typeof(_Service).Assembly.GetTypes()
    .Where(type => !type.Name.StartsWith("_") && type.Name.EndsWith("Service"))
    .ToList();

var serviceInterfaces = generalServices.Where(type => type.IsInterface);
var serviceImplementations = generalServices.Where(type => type.IsClass);

foreach (var implementation in serviceImplementations)
{
    var interfaceName = $"I{implementation.Name}";
    var serviceInterface = serviceInterfaces.FirstOrDefault(i => i.Name == interfaceName);
    if (serviceInterface != null)
    {
        builder.Services.AddScoped(serviceInterface, implementation);
    }
}
#endregion

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

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("V1", new OpenApiInfo { Title = "API Gestor de Inventarios", Version = "V1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        In = ParameterLocation.Header,
        Description = "Enter JWT with bearer format like 'Bearer [Token]'"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
    opt.CustomSchemaIds(type => type.FullName);
    opt.DocInclusionPredicate((docName, apiDesc) =>
    {
        return apiDesc.GroupName == null || !apiDesc.GroupName.Equals("Hidden", StringComparison.OrdinalIgnoreCase);
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowPolicySecureDomains", policy =>
    {
        policy.WithOrigins(configuracionAppSettings.SecureDomains)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseCors("AllowPolicySecureDomains");

if (app.Environment.IsDevelopment())///cambiar
{
    app.UseSwagger(opt => opt.RouteTemplate = "swagger/{documentName}/swagger.json");
    app.UseSwaggerUI(opt => opt.SwaggerEndpoint("V1/swagger.json", "API Gestor de Inventarios V1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();