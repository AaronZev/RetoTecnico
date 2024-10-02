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
    public class ProductosController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly Utilities _utilities;


        public ProductosController(IConfiguration configuration, ApplicationDbContext context, Utilities utilities)
        {
            _configuration = configuration;
            _context = context;
            _utilities = utilities;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(Productos producto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existing = await _context.Productos.FirstOrDefaultAsync(p => p.Nombre.ToUpper() == producto.Nombre.ToUpper());

                if (existing != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Este producto ya se encuentra registrado", title = "Error" });
                }

                var Categoria = await _context.Categorias.FirstOrDefaultAsync(u => u.Id == producto.CategoriaId);
                if (Categoria == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Debe registrar la categoría primero para poder registrar un producto", title = "Error" });
                }

                var Usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == producto.UsuarioId);
                if (Usuario == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Debe registrar un usuario primero para poder registrar un producto", title = "Error" });
                }

                producto.Nombre = producto.Nombre.ToUpper();
                producto.UsuarioId = producto.UsuarioId;
                await _context.Productos.AddAsync(producto);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se registró correctamente el producto.", title = "Éxito" });
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

                var producto = await _context.Productos.FirstOrDefaultAsync(u => u.Id == id);

                if (producto == null)
                {
                    return NotFound(new { isSuccess = false, message = "Producto no encontrado.", title = "Error" });
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "El producto fue eliminado.", title = "Éxito" });
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
                var productos = await (from p in _context.Productos
                                       join c in _context.Categorias on p.CategoriaId equals c.Id
                                       select new
                                       {
                                           p.Id,
                                           p.Nombre,
                                           p.Descripcion,
                                           p.Precio,
                                           CategoriaId = c.Id,
                                           CategoriaNombre = c.Nombre
                                       })
                                        .ToListAsync();


                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, response = productos });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(Productos producto)
        {
            try
            {
                var identify = HttpContext.User.Identity as ClaimsIdentity;
                var rol = Jwt.ValidarToken(identify);

                if (rol != "1")
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No cuenta con permisos para realizar esta acción.", title = "Error" });
                }

                var existingProducto = await _context.Productos.FirstOrDefaultAsync(u => u.Id == producto.Id);

                if (existingProducto == null)
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No se encontró el registro para actualizar.", title = "Error" });

                existingProducto.Nombre = producto.Nombre.ToUpper();
                existingProducto.Descripcion = producto.Descripcion;
                existingProducto.Precio = producto.Precio;
                existingProducto.CategoriaId = producto.CategoriaId;
                existingProducto.UsuarioId = producto.UsuarioId;

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se actualizó correctamente el producto.", response = existingProducto });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }

    }
}
