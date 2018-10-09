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
    public class Horario_laboralController : Controller
    {
        private radixEntities db = new radixEntities();

        public ActionResult Horario(int? Subempcar_id)
        {


            string emp_nom = HttpContext.Session["Empresa"].ToString();
            ViewBag.empresa = emp_nom;
            ViewBag.Subempcar_id = Subempcar_id;

            return View();
        }
        public ActionResult Horario_Personas(int? sub_Id, int? car_id)
        {
            var subempresa_cargo = db.subempresa_cargo.FirstOrDefault(s => s.Sub_Id == sub_Id && s.Car_Id == car_id);

            ViewBag.Subempcar_id =subempresa_cargo.Subempcar_id;

            string emp_nom = HttpContext.Session["Empresa"].ToString();
            ViewBag.empresa = emp_nom;
            return View();
        }


        public JsonResult GetEvents(int? Subempcar_id)
        {

            db.Configuration.ProxyCreationEnabled = false;
            List<horario_laboral> events = db.horario_laboral.Where(h => h.Subempcar_id == Subempcar_id).ToList();
            //var events = db.horario_laboral.Where(h => h.Subempcar_id == Subempcar_id).ToList();

            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }


        // GET: Horario_laboral
        public ActionResult Index(int? Subempcar_id)
        {
            var horario_laboral = db.horario_laboral;
            return View(horario_laboral.Where(hl=>hl.Subempcar_id== Subempcar_id).ToList());
        }

        [HttpPost]
        public JsonResult SaveEvent(horario_laboral e, int? Subempcar_id)
        {
            var status = false;

            if (e.Hl_Id > 0)
            {
                //Update
                var v = db.horario_laboral.Where(a => a.Hl_Id == e.Hl_Id).FirstOrDefault();
                if (v != null)
                {
                    v.Hl_Titulo = e.Hl_Titulo;
                    v.Hl_Inicio = e.Hl_Inicio;
                    v.Hl_Termino = e.Hl_Termino;
                    v.Hl_Descripcion = e.Hl_Descripcion;
                    v.Hl_TodoDia = e.Hl_TodoDia;
                    v.Hl_ColorTema = e.Hl_ColorTema;
                }
            }
            else
            {
                e.Subempcar_id = Subempcar_id;
                db.horario_laboral.Add(e);
            }
            db.SaveChanges();
            status = true;

            return new JsonResult { Data = new { status } };
        }


        [HttpPost]
        public JsonResult DeleteEvent(int? Hl_Id)
        {
            var status = false;

            var v = db.horario_laboral.Where(a => a.Hl_Id == Hl_Id).FirstOrDefault();

            if (v != null)
            {
                db.horario_laboral.Remove(v);
                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status } };
        }
    }
}
