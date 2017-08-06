using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaRH3.Models
{
    public class CorreosMultiples
    {

        [Key]
        public int CorreoID { get; set; }

        public string asunto { get; set; }

        [AllowHtml]
        public string mensaje { get; set; }

        public string[] empleadosIDs { get; set; }

        public string estado { get; set; }

        public string destinatarios { get; set; }

        public DateTime? fecha { get; set; }
        
    }
}