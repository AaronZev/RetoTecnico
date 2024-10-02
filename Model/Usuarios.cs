using System.ComponentModel.DataAnnotations;

namespace RetoTecnico.Model
{
    public class Usuarios
    {
        [Key]
        public int Id { get; set; }  

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }  = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int RolId { get; set; }

    }
}
