using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Proyecto_RadixWeb.Models;

namespace Proyecto_RadixWeb.Controllers
{
    public class CargosController : Controller
    {
        private radixEntities db = new radixEntities();

        // GET: cargos
        public ActionResult Index(string emp_nom, string emp_id)
        {

            ViewBag.empresa = HttpContext.Session["Empresa"].ToString();
            ViewBag.emp_id = Convert.ToInt32(emp_id);
           

            return View(db.cargos.ToList());
        }

        // GET: cargos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cargos cargos = db.cargos.Find(id);
            if (cargos == null)
            {
                return HttpNotFound();
            }
            return View(cargos);
        }

        // GET: cargos/Create
        public ActionResult Create()
        {
            ViewBag.empresa = HttpContext.Session["Empresa"].ToString();
            return View();
        }

        // POST: cargos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MultiplesClases cargos)
        {
            if (ModelState.IsValid)
            {
                //Cargos que ingrese tiene que ser diferente a radox
                if (cargos.ObjCargos.Car_Nom != "Radix")
                {

                    var buscarRoles = db.aspnetroles.Count(r => r.Name == cargos.ObjCargos.Car_Nom);
                    var buscarCargo = db.cargos.Count(r => r.Car_Nom == cargos.ObjCargos.Car_Nom);

                    // Si el cargo es diferente a Agricola lo agrega en roles de cuenta, sino solo lo agrega en cargos
                    if (cargos.ObjCargos.Car_Nom != "Agricola")
                    {

                        if (buscarRoles == 0)
                        {
                            var role = new IdentityRole(cargos.ObjCargos.Car_Nom);
                            var roleresult = await RoleManager.CreateAsync(role);
                            if (!roleresult.Succeeded)
                            {

                                ModelState.AddModelError("", roleresult.Errors.First());
                                return View();

                            }
                        }
                    }

                    if (buscarCargo == 0)
                    {
                        db.cargos.Add(cargos.ObjCargos);
                        db.SaveChanges();
                    }

                }


                return RedirectToAction("Index");
            }

            return View(cargos);
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        // GET: cargos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cargos cargos = db.cargos.Find(id);
            if (cargos == null)
            {
                return HttpNotFound();
            }
            return View(cargos);
        }

        // POST: cargos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Car_Id,Car_Nom")] cargos cargos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cargos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cargos);
        }

        // GET: cargos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cargos cargos = db.cargos.Find(id);
            if (cargos == null)
            {
                return HttpNotFound();
            }
            return View(cargos);
        }

        // POST: cargos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cargos cargos = db.cargos.Find(id);
            db.cargos.Remove(cargos);
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
