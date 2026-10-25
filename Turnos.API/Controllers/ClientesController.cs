using Microsoft.AspNetCore.Mvc;
using Turnos.API.Models;
using Turnos.API.Repositories;

namespace Turnos.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;

        public ClientesController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> ObtenerTodos()
        {
            var clientes = await _clienteRepository.ObtenerTodosAsync();
            return Ok(clientes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> ObtenerPorId(int id)
        {
            var cliente = await _clienteRepository.ObtenerPorIdAsync(id);
            if (cliente == null) return NotFound();
            return Ok(cliente);
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> Crear(Cliente cliente)
        {
            await _clienteRepository.CrearAsync(cliente);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, Cliente cliente)
        {
            if (id != cliente.Id) return BadRequest();
            await _clienteRepository.ActualizarAsync(cliente);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _clienteRepository.EliminarAsync(id);
            return NoContent();
        }
    }
}