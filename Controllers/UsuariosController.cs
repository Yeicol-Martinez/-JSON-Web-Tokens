using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuariosAPI.Data;
using UsuariosAPI.Models;

namespace UsuariosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }

        // GET /api/usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return Ok(await _context.Usuarios.ToListAsync());
        }

        // GET /api/usuarios/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}." });

            return Ok(usuario);
        }

        // GET /api/usuarios/me  →  perfil del usuario autenticado
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            if (!int.TryParse(userIdClaim, out int userId))
                return Unauthorized(new { mensaje = "Token sin claim de usuario." });

            var usuario = await _context.Usuarios.FindAsync(userId);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            return Ok(usuario);
        }

        // POST /api/usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> CreateUsuario([FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _context.Usuarios.AnyAsync(u => u.Correo.ToLower() == usuario.Correo.ToLower()))
                return BadRequest(new { mensaje = $"El correo '{usuario.Correo}' ya está en uso." });

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        // PUT /api/usuarios/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] Usuario usuarioActualizado)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}." });

            if (await _context.Usuarios.AnyAsync(u => u.Correo.ToLower() == usuarioActualizado.Correo.ToLower() && u.Id != id))
                return BadRequest(new { mensaje = $"El correo '{usuarioActualizado.Correo}' ya está en uso." });

            usuario.Nombre            = usuarioActualizado.Nombre;
            usuario.Correo            = usuarioActualizado.Correo;
            usuario.FechaDeNacimiento = usuarioActualizado.FechaDeNacimiento;

            await _context.SaveChangesAsync();
            return Ok(usuario);
        }

        // DELETE /api/usuarios/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound(new { mensaje = $"No se encontró el usuario con ID {id}." });

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return Ok(new { mensaje = $"Usuario con ID {id} eliminado correctamente." });
        }
    }
}
