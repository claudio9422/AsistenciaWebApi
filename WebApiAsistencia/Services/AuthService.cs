using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApiAsistencia.Data;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Interfaces;
using WebApiAsistencia.Models;

namespace WebApiAsistencia.Services
{
    public class AuthService : IAuthService
    {
        private readonly DbAsistenciaContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(DbAsistenciaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto)
        {
            try
            {
                // 1. Buscar usuario incluyendo su Rol
                var usuario = await _context.Usuarios
                    .Include(u => u.IdRolNavigation)
                    .FirstOrDefaultAsync(u => u.DocumentoIdentidad == dto.DocumentoIdentidad);

                if (usuario == null)
                    return new LoginResponseDto { Success = false, Message = "Usuario o contraseña incorrectos." };

                if (!usuario.Estado)
                    return new LoginResponseDto { Success = false, Message = "El usuario se encuentra inactivo." };

                // 2. Verificar contraseña con BCrypt
                bool esValida = BCrypt.Net.BCrypt.Verify(dto.Password, usuario.PasswordHash);
                if (!esValida)
                    return new LoginResponseDto { Success = false, Message = "Usuario o contraseña incorrectos." };

                string tokenString = GenerarToken(usuario);

                return new LoginResponseDto
                {
                    Success = true,
                    Message = "Login exitoso",
                    Token = tokenString,
                    EsPrimerIngreso = usuario.EsPrimerIngreso,
                    PermiteCambioClave = usuario.PermiteCambioClave,
                    NombreCompleto = $"{usuario.Nombre} {usuario.Apellido}",
                    Rol = usuario.IdRolNavigation.NombreRol
                };
            }
            catch (Exception)
            {
                return new LoginResponseDto { Success = false, Message = "Ocurrió un error interno en el servidor" };
            }
        }

        public string GenerarToken(Usuario usuario)
        {
            // 3. GENERAR EL TOKEN JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _configuration["Jwt:Key"] ?? "ClaveSeguraPorDefectoDeMinimo32Caracteres2026!";
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("DocumentoIdentidad", usuario.DocumentoIdentidad), // Aquí viaja el DNI seguro para el SP de marcación
                new Claim(ClaimTypes.Role, usuario.IdRolNavigation.NombreRol) // Rol para proteger vistas
            }),
                Expires = DateTime.UtcNow.AddDays(7), // Duración del token en el celular
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
    }
}
