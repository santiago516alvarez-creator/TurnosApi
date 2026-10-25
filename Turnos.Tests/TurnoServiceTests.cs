using Moq;
using Turnos.API.Models;
using Turnos.API.Repositories;
using Turnos.API.Services;
using Xunit;

namespace Turnos.Tests
{
    public class TurnoServiceTests
    {
        private readonly Mock<ITurnoRepository> _turnoRepo = new();
        private readonly Mock<IProfesionalRepository> _profesionalRepo = new();
        private readonly Mock<IClienteRepository> _clienteRepo = new();
        private readonly TurnoService _service;

        private static readonly DateTime Horario = new(2026, 11, 2, 10, 0, 0);

        public TurnoServiceTests()
        {
            _service = new TurnoService(_turnoRepo.Object, _profesionalRepo.Object, _clienteRepo.Object);

            // Por defecto existen el profesional 1, el profesional 2 y el cliente 1
            _profesionalRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(new Profesional { Id = 1 });
            _profesionalRepo.Setup(r => r.ObtenerPorIdAsync(2)).ReturnsAsync(new Profesional { Id = 2 });
            _clienteRepo.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(new Cliente { Id = 1 });
        }

        [Fact]
        public async Task CrearTurno_HorarioLibre_CreaTurnoPendiente()
        {
            _turnoRepo.Setup(r => r.ExisteTurnoEnHorarioAsync(1, Horario)).ReturnsAsync(false);
            var turno = new Turno { ProfesionalId = 1, ClienteId = 1, FechaHora = Horario };

            var (exito, _) = await _service.CrearTurnoAsync(turno);

            Assert.True(exito);
            Assert.Equal(EstadoTurno.Pendiente, turno.Estado);
            _turnoRepo.Verify(r => r.CrearAsync(turno), Times.Once);
        }

        [Fact]
        public async Task CrearTurno_HorarioOcupadoMismoProfesional_NoCreaTurno()
        {
            _turnoRepo.Setup(r => r.ExisteTurnoEnHorarioAsync(1, Horario)).ReturnsAsync(true);
            var turno = new Turno { ProfesionalId = 1, ClienteId = 1, FechaHora = Horario };

            var (exito, mensaje) = await _service.CrearTurnoAsync(turno);

            Assert.False(exito);
            Assert.Contains("horario", mensaje);
            _turnoRepo.Verify(r => r.CrearAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task CrearTurno_MismoHorarioOtroProfesional_SePermite()
        {
            // El profesional 1 está ocupado, el 2 no
            _turnoRepo.Setup(r => r.ExisteTurnoEnHorarioAsync(1, Horario)).ReturnsAsync(true);
            _turnoRepo.Setup(r => r.ExisteTurnoEnHorarioAsync(2, Horario)).ReturnsAsync(false);
            var turno = new Turno { ProfesionalId = 2, ClienteId = 1, FechaHora = Horario };

            var (exito, _) = await _service.CrearTurnoAsync(turno);

            Assert.True(exito);
            _turnoRepo.Verify(r => r.CrearAsync(turno), Times.Once);
        }

        [Fact]
        public async Task CrearTurno_ProfesionalInexistente_NoCreaTurno()
        {
            var turno = new Turno { ProfesionalId = 99, ClienteId = 1, FechaHora = Horario };

            var (exito, _) = await _service.CrearTurnoAsync(turno);

            Assert.False(exito);
            _turnoRepo.Verify(r => r.CrearAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task CrearTurno_ClienteInexistente_NoCreaTurno()
        {
            var turno = new Turno { ProfesionalId = 1, ClienteId = 99, FechaHora = Horario };

            var (exito, _) = await _service.CrearTurnoAsync(turno);

            Assert.False(exito);
            _turnoRepo.Verify(r => r.CrearAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task CancelarTurno_Existente_CambiaEstadoACancelado()
        {
            var turno = new Turno { Id = 5, Estado = EstadoTurno.Pendiente };
            _turnoRepo.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(turno);

            var (exito, _) = await _service.CancelarTurnoAsync(5);

            Assert.True(exito);
            Assert.Equal(EstadoTurno.Cancelado, turno.Estado);
            _turnoRepo.Verify(r => r.ActualizarAsync(turno), Times.Once);
        }

        [Fact]
        public async Task CancelarTurno_YaCancelado_DevuelveError()
        {
            var turno = new Turno { Id = 5, Estado = EstadoTurno.Cancelado };
            _turnoRepo.Setup(r => r.ObtenerPorIdAsync(5)).ReturnsAsync(turno);

            var (exito, _) = await _service.CancelarTurnoAsync(5);

            Assert.False(exito);
            _turnoRepo.Verify(r => r.ActualizarAsync(It.IsAny<Turno>()), Times.Never);
        }

        [Fact]
        public async Task CancelarTurno_Inexistente_DevuelveError()
        {
            _turnoRepo.Setup(r => r.ObtenerPorIdAsync(99)).ReturnsAsync((Turno?)null);

            var (exito, _) = await _service.CancelarTurnoAsync(99);

            Assert.False(exito);
        }
    }
}