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
    public class EntradasController : Controller
    {
        private Turnover_databaseEntities1 db = new Turnover_databaseEntities1();

        // GET: Entradas
        public ActionResult Index()
        {
            var entradas = db.Entradas.Include(e => e.Concierto).Include(e => e.Usuario);
            return View(entradas.ToList());
        }

        // GET: Entradas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrada entrada = db.Entradas.Find(id);
            if (entrada == null)
            {
                return HttpNotFound();
            }
            return View(entrada);
        }

        // GET: Entradas/Create
        public ActionResult Create()
        {
            ViewBag.ConciertoId = new SelectList(db.Conciertos, "Id", "Nombre");
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre");
            return View();
        }

        // POST: Entradas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UsuarioId,ConciertoId,Seccion,Cantidad,FechaCompra,TotalPagado")] Entrada entrada)
        {
            if (ModelState.IsValid)
            {
                // Obtener el concierto seleccionado
                var concierto = db.Conciertos.Find(entrada.ConciertoId);

                // Calcular el TotalPagado basado en la sección y la cantidad de entradas
                switch (entrada.Seccion)
                {
                    case "Platea":
                        entrada.TotalPagado = concierto.PrecioPlatea * entrada.Cantidad;
                        break;
                    case "VIP":
                        entrada.TotalPagado = concierto.PrecioVIP * entrada.Cantidad;
                        break;
                    case "General":
                        entrada.TotalPagado = concierto.PrecioGeneral * entrada.Cantidad;
                        break;
                    default:
                        entrada.TotalPagado = 0; // Manejar un valor predeterminado por si algo sale mal
                        break;
                }

                // Agregar la entrada con el TotalPagado calculado
                db.Entradas.Add(entrada);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConciertoId = new SelectList(db.Conciertos, "Id", "Nombre", entrada.ConciertoId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", entrada.UsuarioId);
            return View(entrada);
        }


        // GET: Entradas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrada entrada = db.Entradas.Find(id);
            if (entrada == null)
            {
                return HttpNotFound();
            }
            ViewBag.ConciertoId = new SelectList(db.Conciertos, "Id", "Nombre", entrada.ConciertoId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", entrada.UsuarioId);
            return View(entrada);
        }

        // POST: Entradas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UsuarioId,ConciertoId,Seccion,Cantidad,FechaCompra,TotalPagado")] Entrada entrada)
        {
            if (ModelState.IsValid)
            {
                // Obtener el concierto actualizado
                var concierto = db.Conciertos.Find(entrada.ConciertoId);

                // Recalcular el TotalPagado basado en la sección y la cantidad de entradas
                switch (entrada.Seccion)
                {
                    case "Platea":
                        entrada.TotalPagado = concierto.PrecioPlatea * entrada.Cantidad;
                        break;
                    case "VIP":
                        entrada.TotalPagado = concierto.PrecioVIP * entrada.Cantidad;
                        break;
                    case "General":
                        entrada.TotalPagado = concierto.PrecioGeneral * entrada.Cantidad;
                        break;
                    default:
                        entrada.TotalPagado = 0; // Valor predeterminado
                        break;
                }

                // Actualizar la entrada con el TotalPagado calculado
                db.Entry(entrada).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ConciertoId = new SelectList(db.Conciertos, "Id", "Nombre", entrada.ConciertoId);
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "Id", "Nombre", entrada.UsuarioId);
            return View(entrada);
        }


        // GET: Entradas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entrada entrada = db.Entradas.Find(id);
            if (entrada == null)
            {
                return HttpNotFound();
            }
            return View(entrada);
        }

        // POST: Entradas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entrada entrada = db.Entradas.Find(id);
            db.Entradas.Remove(entrada);
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
