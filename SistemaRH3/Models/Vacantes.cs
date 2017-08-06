using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Vacantes
    {
        [Key]
        public int VacanteID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Cantidad de Puestos")]
        public int cantidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Departamento de Vacante")]
        public int DepartamentoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Puesto")]
        public string puesto { get; set; }

        public string estadoVacante { get; set; }

        public DateTime? fechaVacante { get; set; }

        public virtual Departamentos Departamentos { get; set; }
        public virtual ICollection<Candidatos> Candidatos { get; set; }
    }
}