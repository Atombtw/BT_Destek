using BtDestek.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BtDestek.Controllers
{
    public class MyController : Controller
    {
        GENMOT_MESSEntities1 db = new GENMOT_MESSEntities1();
        
        // GET: My
        public ActionResult Index()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            string admin = Session["kullanici"].ToString();
            var t = db.Tbl_Destek.Where(x => x.Durum == "Islemde" || x.Durum == "Beklemede" && x.TalepOnaylayan == admin).ToList();
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
            var t = db.Tbl_Destek.Find(p.ID);
            if (model.Islemler == "Tamamlandı")
            {
                t.BitisTarih = DateTime.Parse(DateTime.Now.ToString());
                t.OkunduMu = false;
                t.Durum = "Tamamlandı";
                t.TalepSorunAciklama = p.TalepSorunAciklama;
            }
            if (model.Islemler == "Guncelle")
            {
                t.OkunduMu = true;
                t.TalepSorunAciklama = p.TalepSorunAciklama;
            }
            if (model.Islemler == "Sil")
            {
                t.RedTarih = DateTime.Parse(DateTime.Now.ToString());
                t.OkunduMu = false;
                t.TalepSorunAciklama = p.TalepSorunAciklama;
                t.Durum = "Reddedildi";
            }
            if (model.Islemler == "Beklemede")
            {
                t.BeklemeTarih = DateTime.Parse(DateTime.Now.ToString());
                t.OkunduMu = true;
                t.TalepSorunAciklama = p.TalepSorunAciklama;
                t.Durum = "Beklemede";
            }
            if (model.Islemler == "Vazgec")
            {
                return RedirectToAction("Index", "My");
            }
            db.SaveChanges();
            return RedirectToAction("Index", "My");
        }
    }
}