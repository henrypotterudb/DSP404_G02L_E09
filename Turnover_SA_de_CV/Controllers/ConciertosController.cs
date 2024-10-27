using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Turnover_SA_de_CV;

namespace Turnover_SA_de_CV.Controllers
{
    public class ConciertosController : Controller
    {
        private Turnover_databaseEntities1 db = new Turnover_databaseEntities1();

        // GET: Conciertos
        public ActionResult Index()
        {
            return View(db.Conciertos.ToList());
        }

        // GET: Conciertos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concierto concierto = db.Conciertos.Find(id);
            if (concierto == null)
            {
                return HttpNotFound();
            }
            return View(concierto);
        }

        // GET: Conciertos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Conciertos/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nombre,FechaConcierto,Lugar,EntradasPlateaDisponibles,EntradasVIPDisponibles,EntradasGeneralDisponibles")] Concierto concierto)
        {
            if (ModelState.IsValid)
            {
                db.Conciertos.Add(concierto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(concierto);
        }

        // GET: Conciertos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concierto concierto = db.Conciertos.Find(id);
            if (concierto == null)
            {
                return HttpNotFound();
            }
            return View(concierto);
        }

        // POST: Conciertos/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nombre,FechaConcierto,Lugar,EntradasPlateaDisponibles,EntradasVIPDisponibles,EntradasGeneralDisponibles")] Concierto concierto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(concierto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(concierto);
        }

        // GET: Conciertos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Concierto concierto = db.Conciertos.Find(id);
            if (concierto == null)
            {
                return HttpNotFound();
            }
            return View(concierto);
        }

        // POST: Conciertos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Concierto concierto = db.Conciertos.Find(id);
            db.Conciertos.Remove(concierto);
            db.SaveChanges();
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
