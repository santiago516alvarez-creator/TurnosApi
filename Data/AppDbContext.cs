using Microsoft.EntityFrameworkCore;
using Turnos.API.Models;

namespace Turnos.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Profesional> Profesionales {get;set;}
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Turno> Turnos { get; set; }
    }


}