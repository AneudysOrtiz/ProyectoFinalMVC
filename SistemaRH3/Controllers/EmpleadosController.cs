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
    public class EmpleadosController : Controller
    {
        
        private GeneralContext db = new GeneralContext();

        //Eliminar empleado
        [HttpPost]
        public ActionResult Borrar(int id)
        {
            var product = db.Empleados.ToList().Find(x => x.EmpleadoID == id);
            if (product != null)
            {
                db.Empleados.Remove(product);
                db.SaveChanges();
                Historial("Se elimino el empleado", product.nombre, product.apellido);
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }

        //cantidad de empleados
        public int cantidadEmp (int idDept)
        {

            int cantidad = db.Empleados
             .Where(x => x.DepartamentoID == idDept)
             .Count();                       
            
            return cantidad;

        }

        //porcentaje de empleados por departamento
        public double porcentajeEmps(int idDept)
        {
            int c=cantidadEmp(idDept);
            double cantidadDept = c;
            int c2 = 0;
            double porcentaje =0;

            c2 = db.Empleados.Count();
            double cantidadTotal = c2;
            if (cantidadDept != 0)
            {
                porcentaje = (cantidadDept / cantidadTotal) * 100;
            }                   
            return porcentaje;

        }
        

        // Index Empleados
        public ActionResult Index()
        {
            var empleados = db.Empleados.Include(e => e.Departamentos);
            return View(empleados.ToList());
        }

        // Detalles de los empleados
        public ActionResult Detalles(int? id)
        {
           
            Empleados empleados = db.Empleados.Find(id);

            if (empleados == null)
            {
                return HttpNotFound();
            }

            return View(empleados);
        }

        // Agregar Empleados
        public ActionResult Agregar()
        {
            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre");
            return View();
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
                    elementoApellido=usuarioApellido,
                    elemento="empleado",
                    fecha = DateTime.Now
                });
                db.SaveChanges();
                return Json(new { Estatus = "OK", Record = new { descripcion = result.descripcion,
                    elementoNombre = result.elementoNombre,
                    elementoApellido = result.elementoApellido,
                    elemento = result.elemento,
                    Fecha = result.fecha } });
            }
            catch
            {
                return Json(new { Estatus = "ERROR" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar([Bind(Include = "EmpleadoID,cedula,nombre,apellido,sexo,FechaNac,telefono,estadoCivil,email,direccion,nivelAlcanzado,titulo,estudios,DepartamentoID,puesto,salario")] Empleados empleado)
        {
            if (ModelState.IsValid)
            {
                empleado.fechaRegistro = DateTime.Now;
                db.Empleados.Add(empleado);
                db.SaveChanges();
                Historial("Se agrego el empleado", empleado.nombre, empleado.apellido);
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre", empleado.DepartamentoID);
            return View(empleado);
        }

        

        // Editar empleado
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Empleados empleados = db.Empleados.Find(id);
            if (empleados == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre", empleados.DepartamentoID);
            return View(empleados);
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "EmpleadoID,cedula,nombre,apellido,sexo,FechaNac,telefono,estadoCivil,email,direccion,nivelAlcanzado,titulo,estudios,DepartamentoID,puesto,salario")] Empleados empleados)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empleados).State = EntityState.Modified;
                 db.SaveChanges();
                Historial("Se edito el empleado", empleados.nombre, empleados.apellido);
                return RedirectToAction("Index");
            }
            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre", empleados.DepartamentoID);
            return View(empleados);
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
