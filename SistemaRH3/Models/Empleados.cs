using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class Empleados
    {

        [Key]
        [Display(Name = "ID")]
        public int EmpleadoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(13)]
        [Display(Name = "Cedula")]
        public string cedula { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(50)]
        [Display(Name = "Apellido")]
        public string apellido { get; set; }


        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(10)]
        [Display(Name = "Sexo")]
        public string sexo { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime FechaNac { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Telefono")]
        [DataType(DataType.PhoneNumber)]
        public string telefono { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Estado Civil")]
        public string estadoCivil { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [Display(Name = "Direccion de residencia")]
        [DataType(DataType.MultilineText)]
        public string direccion { get; set; }

        [Display(Name = "Estuios completados")]
        [DataType(DataType.MultilineText)]
        public string estudios { get; set; }

        [Display(Name = "Departamento")]
        public int DepartamentoID { get; set; }

        [Display(Name = "Puesto")]
        public string puesto { get; set; }

        [Display(Name = "Salario")]
        public decimal salario { get; set; }

        public DateTime? fechaRegistro { get; set; }

        public virtual Departamentos Departamentos { get; set; }
        public virtual ICollection<Vacaciones> Vacaciones { get; set; }
        public virtual ICollection<Asistencias> Asistencias { get; set; }
    }
}