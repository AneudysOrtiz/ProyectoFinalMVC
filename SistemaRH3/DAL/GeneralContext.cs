using SistemaRH3.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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

        

        public DbSet<Correos> Correos { get; set; }
        public DbSet<CorreosMultiples> CorreosMultiples { get; set; }
        public DbSet<CorreosNoRegistrados> CorreosNoRegistrados { get; set; }
        public DbSet<ConfigBackground> ConfigBackground { get; set; }
        public DbSet<ConfigSideBar> ConfigSideBar { get; set; }
        public DbSet<ConfigCorreo> ConfigCorreo { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}