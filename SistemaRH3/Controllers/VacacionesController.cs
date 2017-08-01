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

namespace SistemaRH3.Controllers
{
    [Authorize]
    public class VacacionesController : Controller
    {
        private GeneralContext db = new GeneralContext();

        // GET: Vacaciones
        public async Task<ActionResult> Index()
        {
            var vacacioness = db.Vacacioness.Include(v => v.Empleados);
            return View(await vacacioness.ToListAsync());
        }

        // GET: Vacaciones/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = await db.Vacacioness.FindAsync(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            return View(vacaciones);
        }

        // GET: Vacaciones/Create
        public ActionResult Create()
        {
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula");
            return View();
        }

        // POST: Vacaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "VacacionesID,EmpleadoID,fechaInicio,fechaFin,estado")] Vacaciones vacaciones)
        {
            if (ModelState.IsValid)
            {
                db.Vacacioness.Add(vacaciones);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", vacaciones.EmpleadoID);
            return View(vacaciones);
        }

        // GET: Vacaciones/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = await db.Vacacioness.FindAsync(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", vacaciones.EmpleadoID);
            return View(vacaciones);
        }

        // POST: Vacaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "VacacionesID,EmpleadoID,fechaInicio,fechaFin,estado")] Vacaciones vacaciones)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vacaciones).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmpleadoID = new SelectList(db.Empleados, "EmpleadoID", "cedula", vacaciones.EmpleadoID);
            return View(vacaciones);
        }

        // GET: Vacaciones/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacaciones vacaciones = await db.Vacacioness.FindAsync(id);
            if (vacaciones == null)
            {
                return HttpNotFound();
            }
            return View(vacaciones);
        }

        // POST: Vacaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vacaciones vacaciones = await db.Vacacioness.FindAsync(id);
            db.Vacacioness.Remove(vacaciones);
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