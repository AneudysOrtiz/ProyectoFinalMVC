using System.ComponentModel.DataAnnotations;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SistemaRH.Models
{
    public class Vacante
    {
        [Key]
        public int vacanteID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Cantidad de Puestos")]
        public int cantidad { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Departamento de Vacante")]
        public int DepartamentoID { get; set; }

        public virtual ICollection<Departamento> Departamento { get; set; }

    }
}