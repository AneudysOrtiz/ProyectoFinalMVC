﻿using System;
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
using Rotativa;

namespace SistemaRH3.Controllers
{
    [Authorize]
    public class DepartamentosController : Controller
    {
        private GeneralContext db = new GeneralContext();

        //Buscar Departamento
        public ActionResult Buscar(List<Departamentos> departamentos)
        {
            return View(departamentos);
        }

        public List<Departamentos> BuscarNombre(string text)
        {
            var result = from c in db.Departamentos
                         where
                c.nombre.Contains(text)
                         select c;

            return result.ToList();
        }

        public List<Departamentos> BuscarFuncion(string text)
        {
            var result = from c in db.Departamentos
                         where
                c.funcion.Contains(text)
                         select c;

            return result.ToList();
        }

        [HttpPost]
        public ActionResult BuscarDept(string filtro, string parametro)
        {
            var resultado = BuscarNombre("");

             if (filtro.ToLower() == "nombre")
            {
                resultado = BuscarNombre(parametro);
            }
            else if (filtro.ToLower() == "funcion")
            {
                resultado = BuscarFuncion(parametro);
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
                    elemento="departamento",
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

        [HttpPost]
        public ActionResult Crear( Departamentos departamentos)
        {
            if (ModelState.IsValid)
            {
                db.Departamentos.Add(departamentos);
                db.SaveChanges();
                Historial("Se agrego el departamento", departamentos.nombre,"");
            }

            return Json(departamentos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Borrar(int id)
        {
            var product = db.Departamentos.ToList().Find(x => x.DepartamentoID == id);
            if (product != null)
            {
                db.Departamentos.Remove(product);
                db.SaveChanges();
                Historial("Se elimino el departamento", product.nombre, "");
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }



        // GET: Departamentos
        public ActionResult Index()
        {
            return View( db.Departamentos.ToList());
        }

        


        public ActionResult DownloadPDF()
        {
            return new ActionAsPdf("Index", "Departamentos")
            {
                FileName=Server.MapPath("~/Content/Departamentos.pdf")
            };
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
