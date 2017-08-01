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

        // GET: Vacantes
        public async Task<ActionResult> Index()
        {
            var vacantes = db.Vacantes.Include(v => v.Departamentos);
            return View(await vacantes.ToListAsync());
        }

        // GET: Vacantes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacantes vacantes = await db.Vacantes.FindAsync(id);
            if (vacantes == null)
            {
                return HttpNotFound();
            }
            return View(vacantes);
        }

        // GET: Vacantes/Create
        public ActionResult Create()
        {
            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre");
            return View();
        }

        // POST: Vacantes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VacanteID,cantidad,DepartamentoID")] Vacantes vacantes)
        {
            if (ModelState.IsValid)
            {
                db.Vacantes.Add(vacantes);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre", vacantes.DepartamentoID);
            return View(vacantes);
        }

        // GET: Vacantes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacantes vacantes = await db.Vacantes.FindAsync(id);
            if (vacantes == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre", vacantes.DepartamentoID);
            return View(vacantes);
        }

        // POST: Vacantes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VacanteID,cantidad,DepartamentoID")] Vacantes vacantes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacantes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartamentoID = new SelectList(db.Departamentos, "DepartamentoID", "nombre", vacantes.DepartamentoID);
            return View(vacantes);
        }

        // GET: Vacantes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacantes vacantes = await db.Vacantes.FindAsync(id);
            if (vacantes == null)
            {
                return HttpNotFound();
            }
            return View(vacantes);
        }

        // POST: Vacantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vacantes vacantes = await db.Vacantes.FindAsync(id);
            db.Vacantes.Remove(vacantes);
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
