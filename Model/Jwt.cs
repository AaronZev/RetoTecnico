using System.Security.Claims;

namespace RetoTecnico.Model
{
    public class Jwt
    {
        public string Key { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;

        public static dynamic ValidarToken(ClaimsIdentity identity)
        {
            try
            {
                if (identity.Claims.Count() == 0) return new { Issucess = false, Message = "Verificar si es token token válido" };

                var rol= identity.Claims.FirstOrDefault(x => x.Type == "rol").Value;

                return rol;
            }
            catch (Exception ex)
            {
                return new  { Issucess = false, Message = "Catch: " + ex.Message };
            }
        }
    }

}
