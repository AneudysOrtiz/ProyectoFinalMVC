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
using Rotativa;

namespace SistemaRH3.Controllers
{
    [Authorize]
    public class DepartamentosController : Controller
    {
        private GeneralContext db = new GeneralContext();


        [HttpPost]
        public ActionResult Crear( Departamentos departamentos)
        {
            if (ModelState.IsValid)
            {
                db.Departamentos.Add(departamentos);
                db.SaveChanges();
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
            }

            return Json(product, JsonRequestBehavior.AllowGet);
        }



        // GET: Departamentos
        public async Task<ActionResult> Index()
        {
            return View(await db.Departamentos.ToListAsync());
        }

        // GET: Departamentos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamentos departamentos = await db.Departamentos.FindAsync(id);
            if (departamentos == null)
            {
                return HttpNotFound();
            }
            return View(departamentos);
        }


        public ActionResult DownloadPDF()
        {
            return new ActionAsPdf("Index", "Departamentos")
            {
                FileName=Server.MapPath("~/Content/Departamentos.pdf")
            };
        }

        // GET: Departamentos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departamentos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartamentoID,nombre,funcion")] Departamentos departamentos)
        {
            if (ModelState.IsValid)
            {
                db.Departamentos.Add(departamentos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(departamentos);
        }

        // GET: Departamentos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamentos departamentos = await db.Departamentos.FindAsync(id);
            if (departamentos == null)
            {
                return HttpNotFound();
            }
            return View(departamentos);
        }

        // POST: Departamentos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartamentoID,nombre,funcion")] Departamentos departamentos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(departamentos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(departamentos);
        }

        // GET: Departamentos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departamentos departamentos = await db.Departamentos.FindAsync(id);
            if (departamentos == null)
            {
                return HttpNotFound();
            }
            return View(departamentos);
        }

        // POST: Departamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Departamentos departamentos = await db.Departamentos.FindAsync(id);
            db.Departamentos.Remove(departamentos);
            await db.SaveChangesAsync();
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
