using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemaRH3.Models
{
    public class ConfigSideBar
    {
        [Key]
        public int id { get; set; }

        public string fondo { get; set; }

        public DateTime? fechaCambio { get; set; }

    }
}