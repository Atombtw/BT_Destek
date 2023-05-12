using BtDestek.Models;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using PagedList;

namespace BtDestek.Controllers
{
    public class KullaniciController : Controller
    {
        GENMOT_MESSEntities1 db = new GENMOT_MESSEntities1();
        // GET: Kullanici
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(Tbl_Destek p)
        {
            if (p.Aciklama != null)
            {
                p.OkunduMu = true;
                p.TalepTarih = DateTime.Parse(DateTime.Now.ToString());
                p.Durum = "Henuz Isleme Alinmadi";
                db.Tbl_Destek.Add(p);
                db.SaveChanges();
                Session["Talep"] = "none";
                Session["Talep2"] = "block";
                return RedirectToAction("Index");
            }
            Session["Talep"] = "block";
            return RedirectToAction("Index");
        }

        public ActionResult Taleplerim(int sayfa = 1)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            int admin = Convert.ToInt32(Session["ID"]);
            var t = db.Tbl_Destek.Where(x => x.AdSoyad == admin && x.Durum != "Tamamlandi" && x.Durum != "Reddedildi").ToList().ToPagedList(sayfa, 5);

            if (Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Kullanici/Taleplerim.cshtml", t);
            }

            return View(t);
        }

        public ActionResult Bildirim()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            int admin = Convert.ToInt32(Session["ID"]);
            var t = db.Tbl_Destek.Where(x => x.OkunduMu == false && x.AdSoyad == admin).ToList();
            return View(t);
        }

        public ActionResult Onay(int id)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            int sayi = Convert.ToInt32(Session["Bildirim"]);
            var t = db.Tbl_Destek.Find(id);
            t.OkunduMu = true;
            db.SaveChanges();
            Session["Bildirim"] = (sayi - 1);
            return RedirectToAction("Bildirim");
        }


        [HttpGet]
        public ActionResult Tekrar(int id)
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            var t = db.Tbl_Destek.Find(id);
            return View(t);
        }

        [HttpPost]
        public ActionResult Tekrar(Islem model, Tbl_Destek p)
        {
            var t = db.Tbl_Destek.Find(p.ID);
            if (model.Islemler == "Onayla")
            {
                t.Aciklama = p.Aciklama;
                t.TalepSorunAciklama = null;
                t.TalepTarih = DateTime.Parse(DateTime.Now.ToString());
                t.BeklemeTarih = null;
                t.RedTarih = null;
                t.BitisTarih = null;
                t.OnayTarih = null;
                t.Durum = "Henuz Isleme Alinmadi";
                t.OkunduMu = true;
                t.TalepOnaylayan = null;
                int sayi = Convert.ToInt32(Session["Bildirim"]);
                Session["Bildirim"] = (sayi - 1);
                db.SaveChanges();
                return RedirectToAction("Taleplerim", "Kullanici");
            }
            if (model.Islemler == "Sil")
            {
                return RedirectToAction("Bildirim", "Kullanici");
            }
            return View();
        }

        public ActionResult Anaekran()
        {
            if (Session["kullanici"] == null)
            {
                return RedirectToAction("Login", "GirisYap");
            }
            return View();
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
        public ActionResult Detay(Islem model ,Tbl_Destek p)
        {
            var t = db.Tbl_Destek.Find(p.ID);
            if (model.Islemler == "Vazgec")
            {
                return RedirectToAction("Taleplerim", "Kullanici");
            }
            if (model.Islemler == "Sil")
            {
                t.BitisTarih = DateTime.Parse(DateTime.Now.ToString());
                t.OkunduMu = true;
                t.Durum = "İptal";
                t.TalepSorunAciklama = p.TalepSorunAciklama;
                db.SaveChanges();
            }
            return RedirectToAction("Taleplerim", "Kullanici");
        }
    }
}