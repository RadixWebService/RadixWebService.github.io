using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_RadixWeb.Models;

namespace Proyecto_RadixWeb.Controllers
{
    public class SubEmpresasController : Controller
    {
        private radixEntities db = new radixEntities();

        // GET: subempresas
        public ActionResult Index()
        {

            string emp_nom= HttpContext.Session["Empresa"].ToString(); 

            ViewBag.emp_id = HttpContext.Session["Emp_id"].ToString();
            ViewBag.empresa = emp_nom;

            List<regiones> listaregiones = db.regiones.ToList();
            ViewBag.regiones = new SelectList(listaregiones, "Reg_Id", "Reg_Nom");

            
            var subempresas = db.subempresas.Include(s => s.comunas).Include(s => s.empresas);
            MultiplesClases multiples = new MultiplesClases
            {
                ObjESubEmpresas= subempresas.Where(s => s.empresas.Emp_Nom == emp_nom).ToList()
            };

            return View(multiples);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "Sub_Id,Sub_Nom,Sub_Cant,Sub_Estado,Sub_Dir,Com_Id")]  subempresas subempresas)
        {
            if (ModelState.IsValid)
            {
                string emp_id = HttpContext.Session["Emp_id"].ToString();
                string empresa = HttpContext.Session["Empresa"].ToString();

                subempresas.Emp_Id = Convert.ToInt32(emp_id);
                db.subempresas.Add(subempresas);
                db.SaveChanges();
                return RedirectToAction("Index");
                //return RedirectToAction("Index", "SubEmpresas", new { emp_nom = empresa, emp_id });
            }

            //ViewBag.Com_Id = new SelectList(db.comunas, "Com_Id", "Com_Nom", subempresas.Com_Id);
            //ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom", subempresas.Emp_Id);
            return View(subempresas);
        }


        // GET: subempresas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subempresas subempresas = db.subempresas.Find(id);
            if (subempresas == null)
            {
                return HttpNotFound();
            }
            return View(subempresas);
        }

        public ActionResult Agregar()
        {

            List<regiones> listaregiones = db.regiones.ToList();
            ViewBag.regiones = new SelectList(listaregiones, "Reg_Id", "Reg_Nom");



            return PartialView();
        }

        


        // GET: subempresas/Create
        public ActionResult Create()
        {

            List<regiones> listaregiones = db.regiones.ToList();
            ViewBag.regiones = new SelectList(listaregiones, "Reg_Id", "Reg_Nom");


            //ViewBag.Com_Id = new SelectList(db.comunas, "Com_Id", "Com_Nom");

        
            ViewBag.empresa = HttpContext.Session["Empresa"].ToString();

            //ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom");
            return View();
        }

        // POST: subempresas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Sub_Id,Sub_Nom,Sub_Cant,Sub_Estado,Sub_Dir,Com_Id")] subempresas subempresas)
        {
            if (ModelState.IsValid)
            {
                string emp_id = HttpContext.Session["Emp_id"].ToString();
                string empresa = HttpContext.Session["Empresa"].ToString();

                subempresas.Emp_Id = Convert.ToInt32(emp_id);
                db.subempresas.Add(subempresas);
                db.SaveChanges();
                return RedirectToAction("Index", "SubEmpresas", new { emp_nom = empresa, emp_id });
            }

            //ViewBag.Com_Id = new SelectList(db.comunas, "Com_Id", "Com_Nom", subempresas.Com_Id);
            //ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom", subempresas.Emp_Id);
            return View(subempresas);
        }



        public JsonResult ObtenerProvincia(int Reg_Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<provincias> listaprovincia = db.provincias.Where(p => p.Reg_Id == Reg_Id).OrderBy(pp => pp.Prov_Nom).ToList();

            return Json(listaprovincia, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ObtenerComuna(int Prov_Id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<comunas> listacomuna = db.comunas.Where(p => p.Prov_Id == Prov_Id).OrderBy(pp => pp.Com_Nom).ToList();

            return Json(listacomuna, JsonRequestBehavior.AllowGet);

        }


        // GET: subempresas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subempresas subempresas = db.subempresas.Find(id);
            if (subempresas == null)
            {
                return HttpNotFound();
            }
            ViewBag.Com_Id = new SelectList(db.comunas, "Com_Id", "Com_Nom", subempresas.Com_Id);
            ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom", subempresas.Emp_Id);
            return View(subempresas);
        }

        // POST: subempresas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Sub_Id,Sub_Nom,Sub_Cant,Emp_Id,Sub_Estado,Sub_Dir,Com_Id")] subempresas subempresas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subempresas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Com_Id = new SelectList(db.comunas, "Com_Id", "Com_Nom", subempresas.Com_Id);
            ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom", subempresas.Emp_Id);
            return View(subempresas);
        }

        // GET: subempresas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            subempresas subempresas = db.subempresas.Find(id);
            if (subempresas == null)
            {
                return HttpNotFound();
            }
            return View(subempresas);
        }

        // POST: subempresas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            subempresas subempresas = db.subempresas.Find(id);
            db.subempresas.Remove(subempresas);
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
