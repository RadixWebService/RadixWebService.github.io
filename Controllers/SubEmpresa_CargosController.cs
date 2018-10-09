using Proyecto_RadixWeb.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Proyecto_RadixWeb.Controllers
{
    public class SubEmpresa_CargosController : Controller
    {
        private radixEntities db = new radixEntities();

        // GET: Subempresa_Cargo
        public ActionResult Index(int? subemp_id)
        {
            string emp_nom = HttpContext.Session["Empresa"].ToString();

            ViewBag.subemp_id = subemp_id;
            ViewBag.empresa = emp_nom;
            var subempresa_cargo = db.subempresa_cargo.Include(s => s.cargos).Include(s => s.subempresas);
            return View(subempresa_cargo.Where(s => s.Sub_Id == subemp_id).ToList());
        }

        // GET: Subempresa_Cargo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subempresa_cargo subempresa_cargo = db.subempresa_cargo.Find(id);
            if (subempresa_cargo == null)
            {
                return HttpNotFound();
            }
            return View(subempresa_cargo);
        }

        // GET: Subempresa_Cargo/Create
        public ActionResult Create(int? subemp_id)
        {
            string emp_nom = HttpContext.Session["Empresa"].ToString();

            int emp_id = Convert.ToInt32(HttpContext.Session["Emp_id"].ToString());
            ViewBag.empresa = emp_nom;

            //ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom");

           
            List<empresa_cargo> listacargos = db.empresa_cargo.ToList();
            ViewBag.listacargos = new SelectList(listacargos, "Car_Id", "cargos.Car_Nom");

            return View();
        }

        // POST: Subempresa_Cargo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Subempcar_id,Car_Id")] subempresa_cargo subempresa_cargo, int subemp_id)
        {
            if (ModelState.IsValid)
            {
                subempresa_cargo.Sub_Id = subemp_id;
                db.subempresa_cargo.Add(subempresa_cargo);
                db.SaveChanges();
                return RedirectToAction("Index",new { subemp_id });
            }

            ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom", subempresa_cargo.Car_Id);
            ViewBag.Sub_Id = new SelectList(db.subempresas, "Sub_Id", "Sub_Nom", subempresa_cargo.Sub_Id);
            return View(subempresa_cargo);
        }

        // GET: Subempresa_Cargo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subempresa_cargo subempresa_cargo = db.subempresa_cargo.Find(id);
            if (subempresa_cargo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom", subempresa_cargo.Car_Id);
            ViewBag.Sub_Id = new SelectList(db.subempresas, "Sub_Id", "Sub_Nom", subempresa_cargo.Sub_Id);
            return View(subempresa_cargo);
        }

        // POST: Subempresa_Cargo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Subempcar_id,Sub_Id,Car_Id")] subempresa_cargo subempresa_cargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subempresa_cargo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom", subempresa_cargo.Car_Id);
            ViewBag.Sub_Id = new SelectList(db.subempresas, "Sub_Id", "Sub_Nom", subempresa_cargo.Sub_Id);
            return View(subempresa_cargo);
        }

        // GET: Subempresa_Cargo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subempresa_cargo subempresa_cargo = db.subempresa_cargo.Find(id);
            if (subempresa_cargo == null)
            {
                return HttpNotFound();
            }
            return View(subempresa_cargo);
        }

        // POST: Subempresa_Cargo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subempresa_cargo subempresa_cargo = db.subempresa_cargo.Find(id);
            db.subempresa_cargo.Remove(subempresa_cargo);
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
