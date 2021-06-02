using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examen.Elipgo.DAO.Models
{
    public class StoreDAO
    {
        public int Id { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(50, ErrorMessage = "El nombre debe ser maximo de 50 caracteres")]
        public string Name { get; set; }
        [StringLength(60)]
        [Required(ErrorMessage = "La dirección es obligatorio")]
        [MaxLength(60, ErrorMessage = "La dirección debe ser maximo de 60 caracteres")]
        public string Address { get; set; }
        public ICollection<ArticleDAO> Articles { get; set; }
    }
}
