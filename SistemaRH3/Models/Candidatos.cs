using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Candidatos
    {
        [Key]
        public int CandidatoID { get; set; }

        [Display(Name = "Vacante a la que aplica")]
        public int VacanteID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Apellido")]
        public string apellido { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Telefono")]
        public string telefono { get; set; }

        [Display(Name = "Imagen")]
        public string imagen { get; set; }

        [Display(Name = "Curriculum")]
        public string cv { get; set; }

        [Display(Name = "Fecha de Aplicacion")]
        public DateTime? fechaAp { get; set; }        

        public virtual Vacantes Vacantes { get; set; }
    }
}