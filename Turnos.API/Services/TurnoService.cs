using Turnos.API.Models;
using Turnos.API.Repositories;

namespace Turnos.API.Services
{
    public class TurnoService : ITurnoService
    {
        private readonly ITurnoRepository _turnoRepository;
        private readonly IProfesionalRepository _profesionalRepository;
        private readonly IClienteRepository _clienteRepository;

        public TurnoService(ITurnoRepository turnoRepository, IProfesionalRepository profesionalRepository, IClienteRepository clienteRepository)
        {
            _turnoRepository = turnoRepository;
            _profesionalRepository = profesionalRepository;
            _clienteRepository = clienteRepository;
        }

        public async Task<IEnumerable<Turno>> ObtenerTodosAsync()
        {
            return await _turnoRepository.ObtenerTodosAsync();
        }

        public async Task<Turno?> ObtenerPorIdAsync(int id)
        {
            return await _turnoRepository.ObtenerPorIdAsync(id);
        }

        public async Task<IEnumerable<Turno>> ObtenerPorDiaAsync(DateTime fecha)
        {
            return await _turnoRepository.ObtenerPorDiaAsync(fecha);
        }

        public async Task<IEnumerable<Turno>> ObtenerPorClienteAsync(int clienteId)
        {
            return await _turnoRepository.ObtenerPorClienteAsync(clienteId);
        }

        public async Task<(bool Exito, string Mensaje)> CrearTurnoAsync(Turno turno)
        {
            // 1. Validar que exista el profesional
            var profesional = await _profesionalRepository.ObtenerPorIdAsync(turno.ProfesionalId);
            if (profesional == null)
            {
                return (false, "El profesional especificado no existe.");
            }

            // 2. Validar que exista el cliente
            var cliente = await _clienteRepository.ObtenerPorIdAsync(turno.ClienteId);
            if (cliente == null)
            {
                return (false, "El cliente especificado no existe.");
            }

            // 3. Validar el requerimiento clave: No permitir dos turnos en el mismo horario con el mismo profesional
            bool horarioOcupado = await _turnoRepository.ExisteTurnoEnHorarioAsync(turno.ProfesionalId, turno.FechaHora);
            if (horarioOcupado)
            {
                return (false, "El profesional ya tiene un turno asignado en ese horario.");
            }

            // 4. Configurar estado inicial y guardar
            turno.Estado = EstadoTurno.Pendiente;
            await _turnoRepository.CrearAsync(turno);

            return (true, "Turno creado con éxito.");
        }

        public async Task<(bool Exito, string Mensaje)> CancelarTurnoAsync(int id)
        {
            var turno = await _turnoRepository.ObtenerPorIdAsync(id);
            if (turno == null)
            {
                return (false, "El turno no existe.");
            }

            if (turno.Estado == EstadoTurno.Cancelado)
            {
                return (false, "El turno ya se encuentra cancelado.");
            }

            turno.Estado = EstadoTurno.Cancelado;
            await _turnoRepository.ActualizarAsync(turno);

            return (true, "Turno cancelado correctamente.");
        }
    }
}