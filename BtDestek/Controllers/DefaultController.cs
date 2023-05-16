using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BtDestek.Models;
using PagedList;

namespace BtDestek.Controllers
{
    public class DefaultController : Controller
    {
        GENMOT_MESSEntities1 db = new GENMOT_MESSEntities1();
        // GET: Default
        public ActionResult Index(int sayfa = 1)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            var t = db.Tbl_Destek.Where(x => x.Durum == "Henuz Isleme Alinmadi" || x.Durum == "Beklemede").ToList();
            return View(t);
        }

        [HttpGet]
        public ActionResult Detay(int id)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            var t = db.Tbl_Destek.Find(id);
            return View(t);
        }

        [HttpPost]
        public ActionResult Detay(Islem model, Tbl_Destek p)
        {
            if (model.Islemler == "Onayla")
            {
                var t = db.Tbl_Destek.Find(p.ID);
                t.OnayTarih = DateTime.Parse(DateTime.Now.ToString());
                t.Durum = "Islemde";
                t.TalepOnaylayan = Session["kullanici"].ToString();
                t.OkunduMu = true;
                db.SaveChanges();
            }
            if (model.Islemler == "Sil")
            {
                var t = db.Tbl_Destek.Find(p.ID);
                t.RedTarih = DateTime.Parse(DateTime.Now.ToString());
                t.Durum = "Reddedildi";
                t.TalepOnaylayan = Session["kullanici"].ToString();
                t.OkunduMu = false;
                db.SaveChanges();
            }
            if (model.Islemler == "Beklet")
            {
                var t = db.Tbl_Destek.Find(p.ID);
                t.BeklemeTarih = DateTime.Parse(DateTime.Now.ToString());
                t.Durum = "Beklemede";
                t.OkunduMu = true;
                db.SaveChanges();
            }
            if (model.Islemler == "Vazgec")
            {
                return RedirectToAction("Index", "Default");
            }
            return RedirectToAction("Index", "Default");
        }
        
        public ActionResult Onaylanan()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            var t = db.Tbl_Destek.Where(x => x.Durum == "Islemde" || x.Durum == "Beklemede").ToList();
            return View(t);
        }

        public ActionResult Reddedilen()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            var t = db.Tbl_Destek.Where(x => x.Durum == "Reddedildi").ToList();
            return View(t);
        }

        public ActionResult Tamamlanan()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            var t = db.Tbl_Destek.Where(x => x.Durum == "Tamamlandı").ToList();
            return View(t);
        }
    }
}