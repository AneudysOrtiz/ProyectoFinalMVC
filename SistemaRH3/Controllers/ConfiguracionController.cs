using SistemaRH3.DAL;
using SistemaRH3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaRH3.Controllers
{
    public class ConfiguracionController : Controller
    {
        GeneralContext db = new GeneralContext();

        // GET: Configuracion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Interfaz()
        {
            return View();
        }

        public ActionResult BarraHerramientas()
        {
            return View();
        }

        public ActionResult Correo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ConfigCorreo(string correo,string pass)
        {
            ConfigCorreo correos = new Models.ConfigCorreo();

            correos.Email = correo;
            correos.Password = pass;
            correos.fechaRegistro = DateTime.Now;

            db.ConfigCorreo.Add(correos);
            db.SaveChanges();

            return RedirectToAction("Correo", "Configuracion");
        }

        //cambiar fondo del body

        [HttpPost]
        public ActionResult CambiarFondo(string fondo)
        {
            ConfigBackground config = new ConfigBackground();

            config.fechaCambio = DateTime.Now;
            config.fondo = fondo;
            db.ConfigBackground.Add(config);
            db.SaveChanges();

            return Json(new { Estatus="OK"});
        }

        //cambiar fondo del sidebar
        [HttpPost]
        public ActionResult CambiarFondoNav(string fondo)
        {
            ConfigSideBar config = new ConfigSideBar();

            config.fechaCambio = DateTime.Now;
            config.fondo = fondo;
            db.ConfigSideBar.Add(config);
            db.SaveChanges();

            return Json(new { Estatus = "OK" });
        }

    }
}