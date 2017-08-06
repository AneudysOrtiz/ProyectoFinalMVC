using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaRH3.DAL;
using SistemaRH3.Models;

namespace SistemaRH3.Controllers
{
    [Authorize]
    public class AsistenciasController : Controller
    {
        private GeneralContext db = new GeneralContext();
        private Asistencias asistencia = new Asistencias();
        
        public ActionResult Buscar()
        {
            return View(asistencia);
        }


        public List<Asistencias> BuscarNombre(string text)
        {
            var result = from c in db.Asistencias
                         where
                c.Empleados.nombre.Contains(text)
                         select c;

            return result.ToList();
        }

        public List<Asistencias> BuscarCantidad(string text)
        {
            int numero = int.Parse(text);

            var result = from c in db.Asistencias
                         where
                c.cantidad.Equals(numero)
                         select c;

            return result.ToList();
        }

        [HttpPost]
        public ActionResult BuscarAsis(string filtro, string parametro)
        {
            var resultado = BuscarNombre("");

            if (filtro.ToLower() == "nombre")
            {
                resultado = BuscarNombre(parametro);
            }
            else if (filtro.ToLower() == "cantidad")
            {
                resultado = BuscarCantidad(parametro);
            }

            if (resultado.Count() != 0)
            {
                return PartialView("ResultadoBuscar", resultado);
            }
            else
            {
                return PartialView("BusquedaNula");
            }
        }


        public ActionResult Index()
        {
            var asistencias = db.Asistencias.Include(a => a.Empleados);
            return View(asistencias.ToList());
        }

        //agregar asistencia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar([Bind(Include = "AsistenciaID,EmpleadoID,cantidad,fechaDesde,fechaHasta")]
        Asistencias asistencias)
        {
           
            if (ModelState.IsValid)
            {
                db.Asistencias.Add(asistencias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Index", asistencias);
        }


        //Eliminar asistencia
        [HttpPost]
        public ActionResult Borrar(int id)
        {
            var product = db.Asistencias.ToList().Find(x => x.AsistenciaID == id);
            if (product != null)
            {
                
                db.Asistencias.Remove(product);
                db.SaveChanges();
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
