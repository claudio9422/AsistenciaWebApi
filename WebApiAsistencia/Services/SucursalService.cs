using Microsoft.EntityFrameworkCore;
using WebApiAsistencia.Data;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Interfaces;
using WebApiAsistencia.Models;

namespace WebApiAsistencia.Services
{
    public class SucursalService : ISucursalService
    {
        private readonly DbAsistenciaContext _context;

        public SucursalService(DbAsistenciaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SucursalResponseDto>> ListarSucursalesParaComboAsync()
        {
            return await _context.Sucursales
                .Where(s => s.Estado) // Solo sucursales activas
                .Select(s => new SucursalResponseDto
                {
                    IdSucursal = s.IdSucursal,
                    Nombre = s.Nombre
                })
                .ToListAsync();
        }

        public async Task RegistrarSucursalAsync(SucursalRequestDto sucursalRequest)
        {
            if (sucursalRequest.RadioPermitidoMetros < 10)
            {
                throw new ArgumentException("El radio permitido de la sucursal no puede ser menor a 10 metros.");
            }

            var nuevaSucursal = new Sucursale
            {
                Nombre = sucursalRequest.Nombre,
                Latitud = sucursalRequest.Latitud,
                Longitud = sucursalRequest.Longitud,
                RadioPermitidoMetros = sucursalRequest.RadioPermitidoMetros,
                Estado = true
            };

            _context.Sucursales.Add(nuevaSucursal);
            await _context.SaveChangesAsync();
        }
    }
}
