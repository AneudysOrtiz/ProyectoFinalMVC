using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemaRH3.DAL;
using SistemaRH3.Models;

namespace SistemaRH3.Controllers
{
    [Authorize]
    public class VacantesController : Controller
    {
        private GeneralContext db = new GeneralContext();

        
        public ActionResult Index()
        {
            var vacantes = db.Vacantes.Include(v => v.Departamentos);
            return View(vacantes.ToList());
        }

       

        //buscar vacante
        public ActionResult Buscar(List<Vacantes> vacantes)
        {
            return View(vacantes);
        }

        public List<Vacantes> BuscarDept(string text)
        {
            var result = from c in db.Vacantes
                         where
                c.Departamentos.nombre.Contains(text)
                         select c;

            return result.ToList();
        }

        public List<Vacantes> BuscarPuesto(string text)
        {
            var result = from c in db.Vacantes
                         where
                c.puesto.Contains(text)
                         select c;

            return result.ToList();
        }
        public List<Vacantes> BuscarCantidad(string text)
        {
            var result = from c in db.Vacantes
                         where
                c.cantidad.Equals(text)
                         select c;

            return result.ToList();
        }

        [HttpPost]
        public ActionResult BuscarVacante(string filtro, string parametro)
        {
            var resultado = BuscarDept("");

            if (filtro.ToLower() == "departamento")
            {
                resultado = BuscarDept(parametro);
            }
            else if (filtro.ToLower() == "puesto")
            {
                resultado = BuscarPuesto(parametro);
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


        //Registrar Historial
        public ActionResult Historial(string desc, string usuarioNombre, string usuarioApellido)
        {
            try
            {
                var result = db.Historial.Add(new Models.Historial
                {
                    descripcion = desc,
                    elementoNombre = usuarioNombre,
                    elementoApellido = usuarioApellido,
                    elemento = "vacantes",
                    fecha = DateTime.Now
                });
                db.SaveChanges();
                return Json(new
                {
                    Estatus = "OK",
                    Record = new
                    {
                        descripcion = result.descripcion,
                        elementoNombre = result.elementoNombre,
                        elementoApellido = result.elementoApellido,
                        elemento = result.elemento,
                        Fecha = result.fecha
                    }
                });
            }
            catch
            {
                return Json(new { Estatus = "ERROR" });
            }
        }


        //borrar vacante
        [HttpPost]
        public ActionResult Borrar(int id)
        {
            var product = db.Vacantes.ToList().Find(x => x.VacanteID == id);
            var dept= db.Departamentos.ToList().Find(x => x.DepartamentoID == product.DepartamentoID);
            string nombre= dept.nombre;
            if (product != null)
            {
                //var product2 = db.Empleados.ToList().Find(x => x.EmpleadoID == product.EmpleadoID);
                //string nombre = product2.nombre;
                //string apellido = product2.apellido;
                db.Vacantes.Remove(product);
                db.SaveChanges();
                Historial("Se elimino la vacante de ", product.puesto, "en "+nombre);
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        //cambiar estado de vacante
        [HttpPost]
        public ActionResult CambiarEstado(int id, string estadoNuevo)
        {
            var product = db.Vacantes.ToList().Find(x => x.VacanteID == id);
            product.estadoVacante = estadoNuevo;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new
            {
                Estatus = "OK"
            });
        }


        //cambiar estado de vacante
        [HttpPost]
        public ActionResult CambiarEstado2(int id, string estadoNuevo)
        {
            var product = db.Vacantes.ToList().Find(x => x.VacanteID == id);
            product.estadoVacante = estadoNuevo;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new
            {
                Estatus = "OK"
            });
        }

        //agregar vacante
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar([Bind(Include = "VacanteID,cantidad,DepartamentoID,puesto,estadoVacante")] Vacantes vacantes)
        {
            if (ModelState.IsValid)
            {
                var dept = db.Departamentos.ToList().Find(x => x.DepartamentoID == vacantes.DepartamentoID);
                string nombre = dept.nombre;

                vacantes.fechaVacante = DateTime.Now;
                db.Vacantes.Add(vacantes);
                db.SaveChanges();
                Historial("Se agrego vacante de ", vacantes.puesto, "en " + nombre);
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre", vacantes.DepartamentoID);
            return View(vacantes);
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
