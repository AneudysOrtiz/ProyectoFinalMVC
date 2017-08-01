using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Vacaciones
    {
        [Key]
        [Display(Name = "ID")]
        public int VacacionesID { get; set; }

        [Display(Name = "Empleado")]
        public int EmpleadoID { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [DataType(DataType.Date)]
        public DateTime fechaInicio { get; set; }

        [Display(Name = "Fecha de Finalizacion")]
        [DataType(DataType.Date)]
        public DateTime fechaFin { get; set; }

        [Display(Name = "Estado")]
        public string estado { get; set; }

        public virtual Empleados Empleados { get; set; }
    }
}