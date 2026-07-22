using Microsoft.EntityFrameworkCore;
using WebApiAsistencia.Data;
using WebApiAsistencia.DTOs;
using WebApiAsistencia.Interfaces;

namespace WebApiAsistencia.Services
{
    public class TipoMarcacionService : ITipoMarcacionService
    {
        private readonly DbAsistenciaContext _context;

        public TipoMarcacionService(DbAsistenciaContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<TipoMarcacionResponseDto>> ListarTiposMarcacionesParaComboAsync()
        {
            return await _context.TiposMarcacions
                .Where(s => s.Estado) // Solo tipos de marcaciones activas
                .Select(s => new TipoMarcacionResponseDto
                {
                    IdTipoMarcacion = s.IdTipoMarcacion,
                    Nombre = s.Nombre
                })
                .ToListAsync();
        }
    }
}
