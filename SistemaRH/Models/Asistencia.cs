using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH.Models
{
    public class Asistencia
    {
        [Key]
        [Display(Name = "ID")]
        public int asistenciaID { get; set; }

        [Display(Name = "Empleado")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public int EmpleadoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Cantidad de Ausencias")]
        public int cantidad { get; set; }

        [Display(Name = "Desde")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime fechaDesde { get; set; }

        [Display(Name = "Hasta")]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public DateTime fechaHasta { get; set; }

        public virtual ICollection<Empleado> Empleado { get; set; }
    }
}