using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Asistencias
    {
        [Key]
        [Display(Name = "ID")]
        public int AsistenciaID { get; set; }

        [Display(Name = "Empleado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int EmpleadoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Cantidad de Ausencias")]
        public int cantidad { get; set; }

        [Display(Name = "Desde")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime fechaDesde { get; set; }

        [Display(Name = "Hasta")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime fechaHasta { get; set; }

        public virtual Empleados Empleados { get; set; }
    }
}