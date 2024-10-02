using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RetoTecnico.Custom;
using RetoTecnico.Data;
using RetoTecnico.Model;
using System;
using System.Net;
using System.Security.Claims;

namespace RetoTecnico.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly ApplicationDbContext _context;
        private readonly Utilities _utilities;


        public UsuariosController(IConfiguration configuration, ApplicationDbContext context, Utilities utilities)
        {
            _configuration = configuration;
            _context = context;
            _utilities = utilities;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {

            try
            {
                var userData = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == Email && u.Password == _utilities.encrypt(Password));

                if (userData == null)
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Ocurrió un error al loggearse, vuelva a intentarlo.", title = "Error" });
                }

                var jwt = _utilities.generateJwt(userData);

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se Loggeo con éxito el usuario.", title = "Éxito", token = jwt });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }

        }


        [HttpPost("Register")]
        public async Task<IActionResult> UserRegister([FromBody] Usuarios objeto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingUser = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == objeto.Email);
                    if (existingUser != null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "El email de usuario ya se encuentra registrado.", title = "Error" });
                    }

                    var rolEx = await _context.Roles.FirstOrDefaultAsync(u => u.Id == objeto.RolId);
                    if (rolEx == null)
                    {
                        return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Debe registrar el rol primero para poder registrar un usuario", title = "Error" });
                    }

                    var passwordEncript = _utilities.encrypt(objeto.Password);
                    objeto.Password = passwordEncript;
                    _context.Usuarios.Add(objeto);
                    await _context.SaveChangesAsync();

                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se registró correctamente el usuario.", title = "Éxito" });
                }

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "Datos inválidos.", title = "Error" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }

        }

        [Authorize]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var identify = HttpContext.User.Identity as ClaimsIdentity;

                var rol = Jwt.ValidarToken(identify);

                if (rol != "1")
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No cuenta con permisos para realizar esta acción.", title = "Error" });
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);


                if (usuario == null)
                {
                    return NotFound(new { isSuccess = false, message = "Usuario no encontrado.", title = "Error" });
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "El usuario fue eliminado.", title = "Éxito" });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }

        [Authorize]
        [HttpGet("ListUsers")]
        public async Task<IActionResult> ListUsers()
        {
            try
            {
                var identify = HttpContext.User.Identity as ClaimsIdentity;

                var rol = Jwt.ValidarToken(identify);

                if (rol != "1")
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No cuenta con permisos para realizar esta acción.", title = "Error" });
                }

                var usuario = await _context.Usuarios.ToListAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, response = usuario });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }


        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> Update(Usuarios user)
        {
            try
            {
                var identify = HttpContext.User.Identity as ClaimsIdentity;

                var rol = Jwt.ValidarToken(identify);

                if (rol != "1")
                {
                    return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No cuenta con permisos para realizar esta acción.", title = "Error" });
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == user.Id);

                if(usuario == null) return StatusCode(StatusCodes.Status200OK, new { isSuccess = false, message = "No se encontró registros para actualizar.", title = "Error" });

                usuario.Nombre = user.Nombre;
                usuario.Email = user.Email;
                usuario.Password = _utilities.encrypt(user.Password);
                usuario.RolId = user.RolId;

                await _context.SaveChangesAsync();

                return StatusCode(StatusCodes.Status200OK, new { isSuccess = true, message = "Se actualizó correctamente el registro.", response = usuario });
            }
            catch
            {
                return StatusCode(StatusCodes.Status409Conflict, new { isSuccess = false, message = "Ocurrió un error interno.", title = "Error" });
            }
        }
    }
}
