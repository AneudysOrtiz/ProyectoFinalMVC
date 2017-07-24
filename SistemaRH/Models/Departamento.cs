using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH.Models
{
    public class Departamento
    {   
        [Key]
        [Display(Name = "ID")]
        public int departamentoID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string nombre { get; set; }

        [Display(Name = "Funcion")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string funcion { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }
       
    }
}