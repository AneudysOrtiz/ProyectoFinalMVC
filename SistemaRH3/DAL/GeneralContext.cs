using SistemaRH3.Models;
using System.Data.Entity;

namespace SistemaRH3.DAL
{
    public class GeneralContext :DbContext
    {

        public GeneralContext() : base("name=DataBase") { }

        public virtual DbSet<Departamentos> Departamentos { get; set; }
        public virtual DbSet<Empleados> Empleados { get; set; }
        public virtual DbSet<Asistencias > Asistencias { get; set; }
        public virtual DbSet<Vacaciones> Vacacioness { get; set; }
        public virtual DbSet<Vacantes> Vacantes { get; set; }
        public virtual DbSet<Candidatos> Candidatos { get; set; }
        public virtual DbSet<Historial> Historial { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
           
        }
    }
}