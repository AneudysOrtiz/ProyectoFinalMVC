using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SistemaRH.Models
{
    public class Empleado
    {
        [Key]
        [Display(Name = "ID")]
        public int empleadoID { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        [StringLength(13)]
        [Display(Name ="Cedula")]
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
        public string direccion { get; set; }

        [Display(Name = "Estuios completados")]
        public string estudios { get; set; }

        [Display(Name = "Departamento")]
        public int DepartamentoID { get; set; }

        [Display(Name = "Puesto")]
        public string puesto { get; set; }

        [Display(Name = "Salario")]
        public decimal salario { get; set; }

        public virtual Departamento Departamento { get; set; }
        public virtual ICollection<Vacaciones> Vacaciones { get; set; }
        public virtual ICollection<Asistencia> Asistencia { get; set; }
    }
}