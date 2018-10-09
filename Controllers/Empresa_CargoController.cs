using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_RadixWeb.Models;
using Cloudmersive.APIClient.NET.DocumentAndDataConvert.Api;
using Cloudmersive.APIClient.NET.DocumentAndDataConvert.Client;
using Cloudmersive.APIClient.NET.DocumentAndDataConvert.Model;
using System.Diagnostics;
using System.Text;

namespace Proyecto_RadixWeb.Controllers
{
    public class Empresa_CargoController : Controller
    {
        private radixEntities db = new radixEntities();

        // GET: Empresa_Cargo
        public ActionResult Index()
        {

            string emp_nom = HttpContext.Session["Empresa"].ToString();

            int emp_id =Convert.ToInt32( HttpContext.Session["Emp_id"].ToString());
            ViewBag.empresa = emp_nom;
            var empresa_cargo = db.empresa_cargo.Include(e => e.cargos).Include(e => e.empresas);

            MultiplesClases multiples = new MultiplesClases
            {
                ObjEEmpresa_Cargo = empresa_cargo.Where(e => e.Emp_Id == emp_id).ToList()
            };


            return View(multiples);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "PC_Id,PC_Nom,empcar_id")] planillascontratos planillascontratos, HttpPostedFileBase plantilla)
        {
            if (plantilla != null && plantilla.ContentLength > 0)
            {
                var length = plantilla.InputStream.Length; //Length: 103050706

               
                byte[] datoplantilla = null;
                using (var binarydoc = new BinaryReader(plantilla.InputStream))
                {
                    datoplantilla = binarydoc.ReadBytes(plantilla.ContentLength);
                }
                planillascontratos.PC_Binario = datoplantilla;
                planillascontratos.PC_Ext = Path.GetExtension(plantilla.FileName);
            }

            var empresa_Cargo = db.empresa_cargo.Find(planillascontratos.empcar_id);

            if (ModelState.IsValid)
            {
                //planillascontratos.PC_Ext =".docx";
                db.planillascontratos.Add(planillascontratos);

                empresa_Cargo.PC_Id = planillascontratos.PC_Id;
                db.Entry(empresa_Cargo).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(planillascontratos);
        }

        public ActionResult ViewDocx(int? id)
        {
            return View();
        }

       

        public FileResult ViewPdf(int? id)
        {
            var archivo = db.planillascontratos.Where(dp => dp.PC_Id == id).FirstOrDefault();
            // Configure API key authorization: Apikey
            Configuration.Default.AddApiKey("Apikey", "768fa287-07a9-41f9-962d-d1b9356a6b04");
            ConvertDocumentApi apiInstance = new ConvertDocumentApi();
            //convertir un binario a system.io.stream
            MemoryStream stream = new MemoryStream(archivo.PC_Binario);
        
                // convertir
                byte[] result = apiInstance.ConvertDocumentDocxToPdf(stream);

            //return File(result, "document/pdf", archivo.PC_Nom + ".pdf");

            ViewBag.nombre = archivo.PC_Nom + "" + archivo.PC_Ext;
                return File(result, "application/pdf");

        }



        // GET: Empresa_Cargo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empresa_cargo empresa_cargo = db.empresa_cargo.Find(id);
            if (empresa_cargo == null)
            {
                return HttpNotFound();
            }
            return View(empresa_cargo);
        }

        // GET: Empresa_Cargo/Create
        public ActionResult Create()
        {

            string emp_nom = HttpContext.Session["Empresa"].ToString();

            int emp_id = Convert.ToInt32(HttpContext.Session["Emp_id"].ToString());
            ViewBag.empresa = emp_nom;
            ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom");
        
            return View();
        }

        // POST: Empresa_Cargo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Empcar_id,Car_Id")] empresa_cargo empresa_cargo)
        {
            if (ModelState.IsValid)
            {
                string emp_nom = HttpContext.Session["Empresa"].ToString();

                int emp_id = Convert.ToInt32(HttpContext.Session["Emp_id"].ToString());
                empresa_cargo.Emp_Id = emp_id;
                db.empresa_cargo.Add(empresa_cargo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom", empresa_cargo.Car_Id);
            ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom", empresa_cargo.Emp_Id);
            return View(empresa_cargo);
        }

        // GET: Empresa_Cargo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empresa_cargo empresa_cargo = db.empresa_cargo.Find(id);
            if (empresa_cargo == null)
            {
                return HttpNotFound();
            }
            ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom", empresa_cargo.Car_Id);
            ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom", empresa_cargo.Emp_Id);
            return View(empresa_cargo);
        }

        // POST: Empresa_Cargo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Empcar_id,Emp_Id,Car_Id")] empresa_cargo empresa_cargo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(empresa_cargo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Car_Id = new SelectList(db.cargos, "Car_Id", "Car_Nom", empresa_cargo.Car_Id);
            ViewBag.Emp_Id = new SelectList(db.empresas, "Emp_Id", "Emp_Nom", empresa_cargo.Emp_Id);
            return View(empresa_cargo);
        }

        // GET: Empresa_Cargo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            empresa_cargo empresa_cargo = db.empresa_cargo.Find(id);
            if (empresa_cargo == null)
            {
                return HttpNotFound();
            }
            return View(empresa_cargo);
        }

        // POST: Empresa_Cargo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            empresa_cargo empresa_cargo = db.empresa_cargo.Find(id);
            db.empresa_cargo.Remove(empresa_cargo);
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
