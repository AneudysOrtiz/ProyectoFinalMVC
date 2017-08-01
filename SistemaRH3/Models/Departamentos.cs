using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Departamentos
    {
        [Key]
        [Display(Name = "ID")]
        public int DepartamentoID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string nombre { get; set; }

        [Display(Name = "Funcion")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [DataType(DataType.MultilineText)]
        public string funcion { get; set; }

        public virtual ICollection<Empleados> Empleados { get; set; }
        public virtual ICollection<Vacantes> Vacantes { get; set; }

    }
}