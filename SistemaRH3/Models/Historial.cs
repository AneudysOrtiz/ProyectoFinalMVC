using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Historial
    {
        [Key]
        public int HistorialID { get; set; }

        public string descripcion { get; set; }

        public string elementoNombre { get; set; }

        public string elementoApellido { get; set; }

        public string elemento { get; set; }

        public DateTime? fecha { get; set; }
    }
}