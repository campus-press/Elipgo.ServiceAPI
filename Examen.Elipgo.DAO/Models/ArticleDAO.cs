using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examen.Elipgo.DAO.Models
{
    public class ArticleDAO
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser maximo de 50 caracteres")]
        public string Name { get; set; }
        [StringLength(100)]
        [MaxLength(100, ErrorMessage = "la descripción debe ser maximo de 100 caracteres")]
        public string Description { get; set; }
        [StringLength(30)]
        [Required(ErrorMessage = "El código es obligatorio")]
        [MaxLength(30, ErrorMessage = "El código debe ser maximo de 30 caracteres")]
        public string Code { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "La cantida es obligatoria")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "El total en estante es obligatorio")]
        public int TotalInShelf { get; set; }
        [Required(ErrorMessage = "El total en bóveda es obligatorio")]
        public int TotalInVault { get; set; }
        public int StoreId { get; set; }
        public StoreDAO Store { get; set; }
    }
}
