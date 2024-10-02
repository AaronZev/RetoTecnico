using System.ComponentModel.DataAnnotations;

namespace RetoTecnico.Model
{
    public class Productos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Precio { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        [Required]
        public int UsuarioId { get; set; }
    }
}
