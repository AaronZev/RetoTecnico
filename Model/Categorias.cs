using System.ComponentModel.DataAnnotations;

namespace RetoTecnico.Model
{
    public class Categorias
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;
    }
}
