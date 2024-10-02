using System.ComponentModel.DataAnnotations;

namespace RetoTecnico.Model
{
    public class Roles
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;


    }
}
