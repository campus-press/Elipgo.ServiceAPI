using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Examen.Elipgo.DAO.Models
{
    public class LoginDAO
    {
        [Required(ErrorMessage = "Nombre de usuario es requerido")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Contraseña es requerida")]
        public string Password { get; set; }
    }
}
