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
    public class VacacionesController : Controller
    {
        private GeneralContext db = new GeneralContext();

        // GET: Vacaciones
        public ActionResult Index()
        {
            var vacacioness = db.Vacacioness.Include(v => v.Empleados);
            return View( vacacioness.ToList());
        }

        // GET: Vacaciones/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = db.Vacacioness.Find(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            return View(vacaciones);
        }


            //registrar vacaciones   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar([Bind(Include = "VacacionesID,EmpleadoID,fechaInicio,fechaFin,estado,fechaSolicitud")] Vacaciones vacaciones)
        {
            if (ModelState.IsValid)
            {
                var product = db.Empleados.ToList().Find(x => x.EmpleadoID == vacaciones.EmpleadoID);
                string nombre = product.nombre;
                string apellido = product.apellido;
                vacaciones.fechaSolicitud = DateTime.Now;
                db.Vacacioness.Add(vacaciones);

                //buscando el empleado para cambiar su estado 
                

                if (vacaciones.estado == "Aprobada")
                {
                    product.estado = "De Vacaciones";

                }
                else if (vacaciones.estado == "Completada")
                {
                    product.estado = "Activo";
                }
                else if (vacaciones.estado == "Pendiente")
                {
                    product.estado = "Activo";
                }

                db.Entry(product).State = EntityState.Modified;


                db.SaveChanges();
                Historial("Se registro vaciones para: ", nombre, apellido);
                return RedirectToAction("Index");
            }
            
            return View(vacaciones);
        }

        //cambiar estado de vacaciones 
        [HttpPost]
        public ActionResult CambiarEstado(int id, string estadoNuevo)
        {
            var product = db.Vacacioness.ToList().Find(x => x.VacacionesID == id);
            product.estado=estadoNuevo;

            //buscando el empleado para cambiar su estado 
            var empleado = db.Empleados.ToList().Find(x => x.EmpleadoID == product.EmpleadoID);

            if (estadoNuevo == "Aprobada")
            {
                empleado.estado = "De Vacaciones";

            }else if (estadoNuevo == "Completada")
            {
                empleado.estado = "Activo";
            }
            else if (estadoNuevo == "Pendiente")
            {
                empleado.estado = "Activo";
            }

            db.Entry(empleado).State = EntityState.Modified;
            db.Entry(product).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new
            {
                Estatus = "OK"
            });
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
                    elemento = "vacaciones",
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

        //borrar vacaciones
        [HttpPost]
        public ActionResult Borrar(int id)
        {
            var product = db.Vacacioness.ToList().Find(x => x.VacacionesID == id);
            if (product != null)
            {
                var product2 = db.Empleados.ToList().Find(x => x.EmpleadoID == product.EmpleadoID);
                string nombre = product2.nombre;
                string apellido = product2.apellido;
                
                product2.estado = "Activo";
                

                db.Entry(product2).State = EntityState.Modified;

                db.Vacacioness.Remove(product);
                db.SaveChanges();
                Historial("Se elimino vaciones de: ", nombre, apellido);
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        // GET: Vacaciones/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = db.Vacacioness.Find(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", vacaciones.EmpleadoID);
            return View(vacaciones);
        }

        // POST: Vacaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VacacionesID,EmpleadoID,fechaInicio,fechaFin,estado")] Vacaciones vacaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacaciones).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", vacaciones.EmpleadoID);
            return View(vacaciones);
        }

        // GET: Vacaciones/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones =  db.Vacacioness.Find(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            return View(vacaciones);
        }

        // POST: Vacaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vacaciones vacaciones = db.Vacacioness.Find(id);
            db.Vacacioness.Remove(vacaciones);
            db.SaveChanges();
            return RedirectToAction("Index");
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
