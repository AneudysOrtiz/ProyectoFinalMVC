using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SistemaRH.Models;

namespace SistemaRH.DAL
{
    public class GeneralContext:DbContext
    {
        public GeneralContext() : base("conexion") { }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Vacante> Vacantes { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Vacaciones> Vacaciones { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }
        public DbSet<Asistencia> Asistencia { get; set; }
    }
}