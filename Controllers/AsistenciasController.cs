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
    public class AsistenciasController : Controller
    {
        private radixEntities db = new radixEntities();

        // GET: Asistencias
        public ActionResult Index(int? subemp_id, int? car_id)
        {
            var subempcar = db.subempresa_cargo.FirstOrDefault(s => s.Sub_Id == subemp_id && s.Car_Id == car_id);

            var asistencias = db.asistencias.Include(a => a.contratos).Include(a => a.horario_laboral);
            return View(asistencias.Where(a => a.horario_laboral.Subempcar_id == subempcar.Subempcar_id).ToList());
        }

        public ActionResult Asistencia_Personas(int? Subempcar_id, int? horario_id)
        {
            ViewBag.empresa = HttpContext.Session["Empresa"].ToString();

            var subempcar = db.subempresa_cargo.FirstOrDefault(s => s.Subempcar_id == Subempcar_id);


            ViewBag.car_id = subempcar.Car_Id;
            ViewBag.subemp_id = subempcar.Sub_Id;

            ViewBag.Subempcar_id = Subempcar_id;
            ViewBag.horario_id = horario_id;

            var contratos = db.contratos.Include(c => c.personas).Include(c => c.subempresas);
            var asistencias = db.asistencias.Include(a => a.contratos).Include(a => a.horario_laboral);

            ViewBag.contar = db.asistencias.Count(a => a.Hl_Id == horario_id);

            MultiplesClases multiple = new MultiplesClases
            {

                ObjEContrato = contratos.Where(c => c.Sub_Id == subempcar.Sub_Id && c.personas.Car_Id == subempcar.Car_Id).ToList(),
                ObjEAsistencia = asistencias.Where(a => a.Hl_Id == horario_id).ToList()

            };


            return View(multiple);
        }

        [HttpPost]
        public JsonResult Guardar_Asistencia(List<asistencias> lista_asistencia)
        {
            var status = false;

            if (lista_asistencia == null)
            {
                lista_asistencia = new List<asistencias>();
            }
            

            foreach (asistencias item in lista_asistencia)
            {
                if (item.asis_id > 0)
                {
                    var a = db.asistencias.Where(s => s.asis_id == item.asis_id).FirstOrDefault();

                    if (a != null)
                    {
                   
                        a.Con_Id = item.Con_Id;
                        a.Hl_Id = item.Hl_Id;
                        a.asis_estado = item.asis_estado;

                       
                    }

                }
                else
                {
                    db.asistencias.Add(item);

                }

                db.SaveChanges();
                status = true;



            }

            return new JsonResult { Data = new { status } };
        }


        // GET: Asistencias/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asistencias asistencias = db.asistencias.Find(id);
            if (asistencias == null)
            {
                return HttpNotFound();
            }
            return View(asistencias);
        }

        // GET: Asistencias/Create
        public ActionResult Create()
        {
            ViewBag.Con_Id = new SelectList(db.contratos, "Con_Id", "Per_Rut");
            ViewBag.Hl_Id = new SelectList(db.horario_laboral, "Hl_Id", "Hl_Titulo");
            return View();
        }

        // POST: Asistencias/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "asis_id,Con_Id,Hl_Id,asis_estado")] asistencias asistencias)
        {
            if (ModelState.IsValid)
            {
                db.asistencias.Add(asistencias);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Con_Id = new SelectList(db.contratos, "Con_Id", "Per_Rut", asistencias.Con_Id);
            ViewBag.Hl_Id = new SelectList(db.horario_laboral, "Hl_Id", "Hl_Titulo", asistencias.Hl_Id);
            return View(asistencias);
        }

        // GET: Asistencias/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asistencias asistencias = db.asistencias.Find(id);
            if (asistencias == null)
            {
                return HttpNotFound();
            }
            ViewBag.Con_Id = new SelectList(db.contratos, "Con_Id", "Per_Rut", asistencias.Con_Id);
            ViewBag.Hl_Id = new SelectList(db.horario_laboral, "Hl_Id", "Hl_Titulo", asistencias.Hl_Id);
            return View(asistencias);
        }

        // POST: Asistencias/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "asis_id,Con_Id,Hl_Id,asis_estado")] asistencias asistencias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asistencias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Con_Id = new SelectList(db.contratos, "Con_Id", "Per_Rut", asistencias.Con_Id);
            ViewBag.Hl_Id = new SelectList(db.horario_laboral, "Hl_Id", "Hl_Titulo", asistencias.Hl_Id);
            return View(asistencias);
        }

        // GET: Asistencias/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            asistencias asistencias = db.asistencias.Find(id);
            if (asistencias == null)
            {
                return HttpNotFound();
            }
            return View(asistencias);
        }

        // POST: Asistencias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            asistencias asistencias = db.asistencias.Find(id);
            db.asistencias.Remove(asistencias);
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
