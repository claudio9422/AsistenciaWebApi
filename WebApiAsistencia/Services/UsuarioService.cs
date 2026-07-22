using Microsoft.EntityFrameworkCore;
using WebApiAsistencia.Data;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Interfaces;
using WebApiAsistencia.Models;

namespace WebApiAsistencia.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly DbAsistenciaContext _context;

        public UsuarioService(DbAsistenciaContext context)
        {
            _context = context;
        }

        public async Task<bool> RegistrarUsuarioAsync(UsuarioRegistroDto dto)
        {
            // Validar si el DNI ya existe
            var existe = await _context.Usuarios.AnyAsync(u => u.DocumentoIdentidad == dto.DocumentoIdentidad);
            if (existe) return false;

            // Crear la entidad mapeada
            var nuevoUsuario = new Usuario
            {
                Nombre = dto.Nombre,
                Apellido = dto.Apellido,
                DocumentoIdentidad = dto.DocumentoIdentidad,
                IdSucursal = dto.IdSucursal,
                IdHorario = dto.IdHorario,
                IdRol = dto.IdRol,
                // ENCRIPTACIÓN: Hasheamos la contraseña con BCrypt antes de guardarla
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordClara),
                EsPrimerIngreso = true,       // Obligatorio cambiar en su primer login
                PermiteCambioClave = false,
                Estado = true,
                IntentosFallidosGps = 0,
                BloqueadoPorGps = false
            };

            await _context.Usuarios.AddAsync(nuevoUsuario);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
