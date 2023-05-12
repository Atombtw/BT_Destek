using BtDestek.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace BtDestek.Controllers
{
    public class GirisYapController : Controller
    {
        GENMOT_MESSEntities1 db = new GENMOT_MESSEntities1();
        // GET: GirisYap
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Tbl_Admin t, Tbl_Personel t2)
        {
            var deger = db.Tbl_Admin.FirstOrDefault(x => x.Mail == t.Mail && x.Sifre == t.Sifre);
            var deger2 = db.Tbl_Personel.FirstOrDefault(x => x.Mail == t2.Mail && x.Sifre == t2.Sifre);
            if (deger != null)
            {
                Session.Add("kullanici", deger.Mail);
                FormsAuthentication.SetAuthCookie(deger.Mail, false);
                return RedirectToAction("Index", "Default");
            }
            else if (deger2 != null)
            {
                Session.Add("ID", deger2.ID);
                int id = Convert.ToInt32(Session["ID"]);
                var bildirim = db.Tbl_Destek.Count(x => x.AdSoyad == id && x.OkunduMu == false);
                Session.Add("Bildirim", bildirim);
                Session.Add("Talep", "none");
                Session.Add("Talep2", "none");
                Session.Add("kullanici", deger2.AdSoyad);
                FormsAuthentication.SetAuthCookie(deger2.Mail, false);

                return RedirectToAction("Anaekran", "Kullanici");
            }
            else
            {
                Session.Add("Bilgi", "Bilgilerinizi Kontrol Edip Tekrar Deneyiniz.");
                return View();
            }
        }
    }
}