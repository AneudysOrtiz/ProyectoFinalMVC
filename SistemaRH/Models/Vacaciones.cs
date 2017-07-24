using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH.Models
{
    public class Vacaciones
    {   
        [Key]
        [Display(Name ="ID")]
        public int vacacionesID { get; set; }

        [Display(Name = "Empleado")]
        public int empleadoID { get; set; }

        [Display(Name = "Fecha de Inicio")]
        public DateTime fechaInicio { get; set; }

        [Display(Name = "Fecha de Finalizacion")]
        public DateTime fechaFin { get; set; }

        [Display(Name = "Estado")]
        public string estado { get; set; }
        
        public virtual Empleado empleado { get; set; }
    }
}