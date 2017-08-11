using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Usuarios
    {
        [Key]
        public int UsuarioID { get; set; }

        public string nombre { get; set; }

        public string apellido { get; set; }

        public string correo { get; set; }

        public string telefono { get; set; }

        public string direccion { get; set; }

        public string foto { get; set; }
    }
}