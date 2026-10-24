using Microsoft.EntityFrameworkCore;
using Turnos.API.Data;
using Turnos.API.Models;

namespace Turnos.API.Repositories
{
    public class ProfesionalRepository : IProfesionalRepository
    {
        private readonly AppDbContext _context;

        public ProfesionalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Profesional>> ObtenerTodosAsync()
        {
            return await _context.Profesionales.ToListAsync();
        }

        public async Task<Profesional?> ObtenerPorIdAsync(int id)
        {
            return await _context.Profesionales.FindAsync(id);
        }

        public async Task CrearAsync(Profesional profesional)
        {
            await _context.Profesionales.AddAsync(profesional);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarAsync(Profesional profesional)
        {
            _context.Profesionales.Update(profesional);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(int id)
        {
            var profesional = await ObtenerPorIdAsync(id);
            if (profesional != null)
            {
                _context.Profesionales.Remove(profesional);
                await _context.SaveChangesAsync();
            }
        }
    }
}