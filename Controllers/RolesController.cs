using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetoTecnico.Custom;
using RetoTecnico.Data;
using RetoTecnico.Model;

namespace RetoTecnico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly Utilities _utilities;


        public RolesController(IConfiguration configuration, ApplicationDbContext context, Utilities utilities)
        {
            _configuration = configuration;
            _context = context;
            _utilities = utilities;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(Roles rol)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var existing = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre.ToUpper() == rol.Nombre.ToUpper());

                if (existing != null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Este rol ya se encuentra registrado", title = "Error" });
                }

                rol.Nombre = rol.Nombre.ToUpper();
                await _context.Roles.AddAsync(rol);
                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se registró correctamente el rol.", title = "Éxito" });
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
                var roles = await _context.Roles.ToListAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, response = roles });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }

    }
}
