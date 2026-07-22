using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WebApiAsistencia.Data;
using WebApiAsistencia.Interfaces;
using WebApiAsistencia.Services;

var builder = WebApplication.CreateBuilder(args);

// REGISTRO DE CAPA DE NEGOCIO (Inyección de Dependencias)
builder.Services.AddScoped<IAsistenciaService, AsistenciaService>();
builder.Services.AddScoped<ISucursalService, SucursalService>();
builder.Services.AddScoped<ITipoMarcacionService, TipoMarcacionService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IAuthService, AuthService>();


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSwaggerGen(options =>
{
    // 1. Definir el esquema de seguridad JWT Bearer
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe la palabra 'Bearer' seguida de un espacio y tu token JWT.\n\nEjemplo: \"Bearer eyJhbGciOiJIUzI1Ni...\""
    });

    // 2. Hacer que todas las rutas protegidas requieran este esquema en la interfaz
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] {}
        }
    });
});

// Registrar el DbContext
builder.Services.AddDbContext<DbAsistenciaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CadenaSQL")));


builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodo", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var secretKey = builder.Configuration["Jwt:Key"] ?? "ClaveSuperSecretaDeAsistenciaParaElTrabajo2026!";
var key = Encoding.ASCII.GetBytes(secretKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Cambiar a true en producción con SSL
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = builder.Configuration["Jwt:Issuer"] != null,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = builder.Configuration["Jwt:Audience"] != null,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true, // Valida que el token de 12 horas no haya expirado
        ClockSkew = TimeSpan.Zero // Elimina el tiempo de gracia de 5 minutos por defecto
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PermitirTodo");

//app.UseHttpsRedirection();

app.UseStaticFiles(); // <--- ¡ESTA LÍNEA ES CLAVE! Habilita el acceso seguro a wwwroot

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
