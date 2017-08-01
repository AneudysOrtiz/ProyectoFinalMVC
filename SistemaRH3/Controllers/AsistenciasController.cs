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
    public class AsistenciasController : Controller
    {
        private GeneralContext db = new GeneralContext();

        // GET: Asistencias
        public async Task<ActionResult> Index()
        {
            var asistencias = db.Asistencias.Include(a => a.Empleados);
            return View(await asistencias.ToListAsync());
        }

        // GET: Asistencias/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencias asistencias = await db.Asistencias.FindAsync(id);
            if (asistencias == null)
            {
                return HttpNotFound();
            }
            return View(asistencias);
        }

        // GET: Asistencias/Create
        public ActionResult Create()
        {
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "nombre" );
            return View();
        }

        // POST: Asistencias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AsistenciaID,EmpleadoID,cantidad,fechaDesde,fechaHasta")] Asistencias asistencias)
        {
            if (ModelState.IsValid)
            {
                db.Asistencias.Add(asistencias);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", asistencias.EmpleadoID);
            return View(asistencias);
        }

        // GET: Asistencias/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencias asistencias = await db.Asistencias.FindAsync(id);
            if (asistencias == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", asistencias.EmpleadoID);
            return View(asistencias);
        }

        // POST: Asistencias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AsistenciaID,EmpleadoID,cantidad,fechaDesde,fechaHasta")] Asistencias asistencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asistencias).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", asistencias.EmpleadoID);
            return View(asistencias);
        }

        // GET: Asistencias/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asistencias asistencias = await db.Asistencias.FindAsync(id);
            if (asistencias == null)
            {
                return HttpNotFound();
            }
            return View(asistencias);
        }

        // POST: Asistencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Asistencias asistencias = await db.Asistencias.FindAsync(id);
            db.Asistencias.Remove(asistencias);
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
