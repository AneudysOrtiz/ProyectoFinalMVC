using SistemaRH3.DAL;
using SistemaRH3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaRH3.Controllers
{
    [Authorize]
    public class HistorialController : Controller
    {
        private GeneralContext db = new GeneralContext();

        // GET: Historial
        public ActionResult Index()
        {
            return View(db.Historial);
        }

        public ActionResult Buscar()
        {
            return View(db.Historial);
        }

        
        [HttpPost]
        public ActionResult BuscarHistorial(DateTime desde, DateTime hasta)
        {
            ViewBag.desde = desde;
            ViewBag.hasta = hasta;

            return View("Buscar", db.Historial);
        }

        
    }
}