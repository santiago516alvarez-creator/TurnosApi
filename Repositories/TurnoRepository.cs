using Microsoft.EntityFrameworkCore;
using Turnos.API.Data;
using Turnos.API.Models;

namespace Turnos.API.Repositories
{
    public class TurnoRepository : ITurnoRepository
    {
        private readonly AppDbContext _context;

        public TurnoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Turno>> ObtenerTodosAsync()
        {
            return await _context.Turnos
                .Include(t => t.Profesional)
                .Include(t => t.Cliente)
                .ToListAsync();
        }

        public async Task<Turno?> ObtenerPorIdAsync(int id)
        {
            return await _context.Turnos
                .Include(t => t.Profesional)
                .Include(t => t.Cliente)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Turno>> ObtenerPorDiaAsync(DateTime fecha)
        {
            // Filtra los turnos que coincidan con el mismo año, mes y día
            return await _context.Turnos
                .Include(t => t.Profesional)
                .Include(t => t.Cliente)
                .Where(t => t.FechaHora.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Turno>> ObtenerPorClienteAsync(int clienteId)
        {
            return await _context.Turnos
                .Include(t => t.Profesional)
                .Include(t => t.Cliente)
                .Where(t => t.ClienteId == clienteId)
                .ToListAsync();
        }

        public async Task<bool> ExisteTurnoEnHorarioAsync(int profesionalId, DateTime fechaHora)
        {
            // Revisa si ya existe un turno pendiente o atendido con el mismo profesional a la misma hora y fecha exactas
            return await _context.Turnos
                .AnyAsync(t => t.ProfesionalId == profesionalId 
                            && t.FechaHora == fechaHora 
                            && t.Estado != EstadoTurno.Cancelado);
        }

        public async Task CrearAsync(Turno turno)
        {
            await _context.Turnos.AddAsync(turno);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Turno turno)
        {
            _context.Turnos.Update(turno);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var turno = await ObtenerPorIdAsync(id);
            if (turno != null)
            {
                _context.Turnos.Remove(turno);
                await _context.SaveChangesAsync();
            }
        }
    }
}