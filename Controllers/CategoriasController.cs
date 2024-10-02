using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetoTecnico.Custom;
using RetoTecnico.Data;
using RetoTecnico.Model;
using System;
using System.Security.Claims;

namespace RetoTecnico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriasController : ControllerBase
    {

        public IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly Utilities _utilities;


        public CategoriasController(IConfiguration configuration, ApplicationDbContext context, Utilities utilities)
        {
            _configuration = configuration;
            _context = context;
            _utilities = utilities;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterCategories(string Nombre)
        {
            try
            {
                var existing = await _context.Categorias.FirstOrDefaultAsync(u => u.Nombre == Nombre.ToUpper());

                if (existing != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Esta categoría ya se encuentra registrada", title = "Error" });
                }

                var newCategoria = new Categorias
                {
                    Nombre = Nombre.ToUpper()
                };

                await _context.Categorias.AddAsync(newCategoria);

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se registró correctamente la categoría.", title = "Éxito" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var identify = HttpContext.User.Identity as ClaimsIdentity;

                var rol = Jwt.ValidarToken(identify);

                if (rol != "1")
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No cuenta con permisos para realizar esta acción.", title = "Error" });
                }

                var categoria = await _context.Categorias.FirstOrDefaultAsync(u => u.Id == id);


                if (categoria == null)
                {
                    return NotFound(new { isSuccess = false, message = "categoria no encontrada.", title = "Error" });
                }

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Esta categoría fue eliminada.", title = "Éxito" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }

        [HttpGet("List")]
        public async Task<IActionResult> List()
        {
            try
            {

                var categorias = await _context.Categorias.ToListAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, response = categorias });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(Categorias categoria)
        {
            try
            {
                var identify = HttpContext.User.Identity as ClaimsIdentity;

                var rol = Jwt.ValidarToken(identify);

                if (rol != "1")
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No cuenta con permisos para realizar esta acción.", title = "Error" });
                }

                var cat = await _context.Categorias.FirstOrDefaultAsync(u => u.Id == categoria.Id);

                if (cat == null) return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No se encontró registros para actualizar.", title = "Error" });

                cat.Nombre = categoria.Nombre.ToUpper();
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se actualizó correctamente el registro.", response = categoria });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }




    }
}
