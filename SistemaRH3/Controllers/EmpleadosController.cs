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
using System.IO;

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
                if (product.imagen != null)
                {
                    System.IO.File.Delete(product.imagen);
                }
                if (product.cv != null)
                {
                    System.IO.File.Delete(product.cv);
                }

                db.Empleados.Remove(product);
                db.SaveChanges();
                Historial("Se elimino el empleado", product.nombre, product.apellido);
            }
            
            return Json(product, JsonRequestBehavior.AllowGet);
        }


        //buscar empleados
        public ActionResult Buscar(List<Empleados> empleados)
        {
            return View(empleados);
        }

        public List<Empleados> BuscarNombre(string text)
        {
            var result = from c in db.Empleados
                         where
                c.nombre.Contains(text)                
                         select c;

            return result.ToList();
        }

        public List<Empleados> BuscarApellido(string text)
        {
            var result = from c in db.Empleados
                         where
                c.apellido.Contains(text)
                         select c;

            return result.ToList();
        }
        public List<Empleados> BuscarDepartamento(string text)
        {
            var result = from c in db.Empleados
                         where
                c.Departamentos.nombre.Contains(text)
                         select c;

            return result.ToList();
        }

        public List<Empleados> BuscarCedula(string text)
        {
            var result = from c in db.Empleados
                         where
                c.cedula.Contains(text)
                         select c;

            return result.ToList();
        }
        public List<Empleados> BuscarPuesto(string text)
        {
            var result = from c in db.Empleados
                         where
                c.puesto.Contains(text)
                         select c;

            return result.ToList();
        }
      


        //Buscar Empleadooo 
        [HttpPost]
        public ActionResult BuscarEmp(string filtro, string parametro)
        {
            var resultado = BuscarNombre("");

            if (filtro.ToLower() == "cedula")
            {
                resultado = BuscarCedula(parametro);
            }else if (filtro.ToLower() == "nombre")
            {
                resultado = BuscarNombre(parametro);
            }
            else if (filtro.ToLower() == "apellido")
            {
                resultado = BuscarApellido(parametro);
            }
            else if (filtro.ToLower() == "departamento")
            {
                resultado = BuscarDepartamento(parametro);
            }
            else if (filtro.ToLower() == "puesto")
            {
                resultado = BuscarPuesto(parametro);
            }
            
           
            if (resultado.Count()!=0)
            {
                return PartialView("ResultadoBuscar", resultado);
            }else
            {
                return PartialView("BusquedaNula");
            }
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
        public ActionResult Agregar( [Bind(Include = "EmpleadoID,cedula,nombre,apellido,sexo,FechaNac,telefono,estadoCivil,email,direccion,nivelAlcanzado,titulo,estudios,DepartamentoID,puesto,salario,imagen,cv")]
        Empleados empleado, HttpPostedFileBase file, HttpPostedFileBase file2)
        {
            var path = "";
            if (ModelState.IsValid)
            {
                empleado.fechaRegistro = DateTime.Now;
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        if(Path.GetExtension(file.FileName).ToLower()==".jpg"
                            || Path.GetExtension(file.FileName).ToLower() == ".png"
                            || Path.GetExtension(file.FileName).ToLower() == ".jpeg"
                            || Path.GetExtension(file.FileName).ToLower() == ".gif")
                        {
                            var fileName = Path.GetFileName(file.FileName); //getting only file name 
                            fileName = empleado.nombre.ToString()+empleado.apellido.ToString()+fileName;
                            path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);                                                       
                            empleado.imagen = path;
                            file.SaveAs(path);
                            
                        }
                    }
                }

                if (file2 != null)
                {
                    if (file2.ContentLength > 0)
                    {
                        if (Path.GetExtension(file2.FileName).ToLower() == ".jpg"
                            || Path.GetExtension(file2.FileName).ToLower() == ".png"
                            || Path.GetExtension(file2.FileName).ToLower() == ".jpeg"
                            || Path.GetExtension(file2.FileName).ToLower() == ".pdf"
                            || Path.GetExtension(file2.FileName).ToLower() == ".docx"
                            || Path.GetExtension(file2.FileName).ToLower() == ".doc")
                        {
                            var fileName = Path.GetFileName(file2.FileName); //getting only file name 
                            fileName = empleado.nombre.ToString() + empleado.apellido.ToString() + fileName;
                            path = Path.Combine(Server.MapPath("~/Content/CV"), fileName);
                            empleado.cv = path;
                            file2.SaveAs(path);

                        }
                    }
                }

                empleado.estado = "Activo";
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
        public ActionResult Editar(
             [Bind(Include = "EmpleadoID,cedula,nombre,apellido,sexo,FechaNac,telefono,estadoCivil,email,direccion,nivelAlcanzado,titulo,estudios,DepartamentoID,puesto,salario,imagen,cv,estado")]
        Empleados empleados,HttpPostedFileBase file, HttpPostedFileBase file2)
        {
         
            if (ModelState.IsValid)
            {
                var path = "";
                if (file != null)
                {
                    if (file.ContentLength > 0)
                    {
                        if (Path.GetExtension(file.FileName).ToLower() == ".jpg"
                            || Path.GetExtension(file.FileName).ToLower() == ".png"
                            || Path.GetExtension(file.FileName).ToLower() == ".jpeg"
                            || Path.GetExtension(file.FileName).ToLower() == ".gif")
                        {
                            var fileName = Path.GetFileName(file.FileName); //getting only file name 
                            fileName = empleados.nombre.ToString() + empleados.apellido.ToString() + fileName;
                            path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                            empleados.imagen = path;
                            file.SaveAs(path);

                        }
                    }
                }

                if (file2 != null)
                {
                    if (file2.ContentLength > 0)
                    {
                        if (Path.GetExtension(file2.FileName).ToLower() == ".jpg"
                            || Path.GetExtension(file2.FileName).ToLower() == ".png"
                            || Path.GetExtension(file2.FileName).ToLower() == ".jpeg"
                            || Path.GetExtension(file2.FileName).ToLower() == ".pdf"
                            || Path.GetExtension(file2.FileName).ToLower() == ".docx"
                            || Path.GetExtension(file2.FileName).ToLower() == ".doc")
                        {
                            var fileName = Path.GetFileName(file2.FileName); //getting only file name 
                            fileName = empleados.nombre.ToString() + empleados.apellido.ToString() + fileName;
                            path = Path.Combine(Server.MapPath("~/Content/CV"), fileName);
                            empleados.cv = path;
                            file2.SaveAs(path);

                        }
                    }
                }

              
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
