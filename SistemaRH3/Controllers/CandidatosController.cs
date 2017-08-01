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
    public class CandidatosController : Controller
    {
        private GeneralContext db = new GeneralContext();

        // GET: Candidatos
        public async Task<ActionResult> Index()
        {
            var candidatos = db.Candidatos.Include(c => c.Vacante);
            return View(await candidatos.ToListAsync());
        }

        // GET: Candidatos/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidatos candidatos = await db.Candidatos.FindAsync(id);
            if (candidatos == null)
            {
                return HttpNotFound();
            }
            return View(candidatos);
        }

        // GET: Candidatos/Create
        public ActionResult Create()
        {
            ViewBag.VacanteID = new SelectList(db.Vacantes, "VacanteID", "VacanteID");
            return View();
        }

        // POST: Candidatos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CandidatoID,VacanteID,nombre,apellido,email,telefono,fechaAp")] Candidatos candidatos)
        {
            if (ModelState.IsValid)
            {
                db.Candidatos.Add(candidatos);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.VacanteID = new SelectList(db.Vacantes, "VacanteID", "VacanteID", candidatos.VacanteID);
            return View(candidatos);
        }

        // GET: Candidatos/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidatos candidatos = await db.Candidatos.FindAsync(id);
            if (candidatos == null)
            {
                return HttpNotFound();
            }
            ViewBag.VacanteID = new SelectList(db.Vacantes, "VacanteID", "VacanteID", candidatos.VacanteID);
            return View(candidatos);
        }

        // POST: Candidatos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CandidatoID,VacanteID,nombre,apellido,email,telefono,fechaAp")] Candidatos candidatos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidatos).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.VacanteID = new SelectList(db.Vacantes, "VacanteID", "VacanteID", candidatos.VacanteID);
            return View(candidatos);
        }

        // GET: Candidatos/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidatos candidatos = await db.Candidatos.FindAsync(id);
            if (candidatos == null)
            {
                return HttpNotFound();
            }
            return View(candidatos);
        }

        // POST: Candidatos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Candidatos candidatos = await db.Candidatos.FindAsync(id);
            db.Candidatos.Remove(candidatos);
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
