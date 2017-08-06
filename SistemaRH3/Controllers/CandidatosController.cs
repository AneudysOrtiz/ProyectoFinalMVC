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
    public class CandidatosController : Controller
    {
        private GeneralContext db = new GeneralContext();

        // GET: Candidatos
        public ActionResult Index()
        {
            var candidatos = db.Candidatos.Include(c => c.Vacantes);
            return View(candidatos.ToList());
        }

        //agregar candidato
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Agregar([Bind(Include = "CandidatoID,VacanteID,nombre,apellido,email,telefono,imagen,cv")]
        Candidatos candidatos, HttpPostedFileBase file, HttpPostedFileBase file2)
        {
            var path = "";
            if (ModelState.IsValid)
            {
                candidatos.fechaAp = DateTime.Now;
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
                            fileName = candidatos.nombre.ToString() + candidatos.apellido.ToString() + fileName;
                            path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                            candidatos.imagen = path;
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
                            fileName = candidatos.nombre.ToString() + candidatos.apellido.ToString() + fileName;
                            path = Path.Combine(Server.MapPath("~/Content/CV"), fileName);
                            candidatos.cv = path;
                            file2.SaveAs(path);

                        }
                    }
                }

                
                db.Candidatos.Add(candidatos);
                db.SaveChanges();
                Historial("Se agrego el candidato", candidatos.nombre, candidatos.apellido);
                return RedirectToAction("Index");
            }

            return View("Index", candidatos);
        }

        public ActionResult Buscar(List<Candidatos> candidatos)
        {
            return View(candidatos);
        }

        public List<Candidatos> BuscarNombre(string text)
        {
            var result = from c in db.Candidatos
                         where
                c.nombre.Contains(text)
                         select c;

            return result.ToList();
        }

        public List<Candidatos> BuscarPuesto(string text)
        {
            var result = from c in db.Candidatos
                         where
                c.Vacantes.puesto.Contains(text)
                         select c;

            return result.ToList();
        }



        [HttpPost]
        public ActionResult BuscarCand(string filtro, string parametro)
        {
            var resultado = BuscarNombre("");

            if (filtro.ToLower() == "nombre")
            {
                resultado = BuscarNombre(parametro);
            }
            else if (filtro.ToLower() == "puesto")
            {
                resultado = BuscarPuesto(parametro);
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


        //Contratar Candidatos
        [HttpPost]
        public ActionResult Contratar(int id, string cedula,string sexo,
            DateTime fechaNac, string estadoCivil, string direccion,
            string nivelAlcanzado, string titulo, string estudios,
            decimal salario)
        {

            var candidato = db.Candidatos.ToList().Find(x => x.CandidatoID == id);

            Empleados empleado = new Empleados();

            empleado.cedula = cedula;
            empleado.nombre = candidato.nombre;
            empleado.apellido = candidato.apellido;
            empleado.sexo = sexo;
            empleado.fechaRegistro = DateTime.Now;
            empleado.FechaNac = fechaNac;
            empleado.telefono = candidato.telefono;
            empleado.estadoCivil = estadoCivil;
            empleado.email = candidato.email;
            empleado.direccion = direccion;
            empleado.nivelAlcanzado = nivelAlcanzado;
            empleado.titulo = titulo;
            empleado.estudios = estudios;
            empleado.DepartamentoID = candidato.Vacantes.Departamentos.DepartamentoID;
            empleado.puesto = candidato.Vacantes.puesto;
            empleado.salario = salario;
            empleado.imagen = candidato.imagen;
            empleado.cv = candidato.cv;
            empleado.estado = "Activo";


            db.Empleados.Add(empleado);
            db.Candidatos.Remove(candidato);
            db.SaveChanges();
            Historial("Se contrato a ", empleado.nombre, empleado.apellido);

            return Json(new { Estatus = "OK" });
        }

        //Eliminar candidatos
        [HttpPost]
        public ActionResult Borrar(int id)
        {
            var product = db.Candidatos.ToList().Find(x => x.CandidatoID == id);
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

                db.Candidatos.Remove(product);
                db.SaveChanges();
                Historial("Se elimino el candidato", product.nombre, product.apellido);
            }

            return Json(product, JsonRequestBehavior.AllowGet);
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
                    elemento = "empleado",
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
