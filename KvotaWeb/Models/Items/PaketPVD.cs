using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class PaketPvd : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней";

        [Display(Name = "Пакет:")]
        public int? Paket { get; set; }

        [Display(Name = "Цвет шелкографии (1 сторона):")]
        public int? KolichestvoTcvetov1 { get; set; }

        [Display(Name = "Цвет шелкографии (2 сторона):")]
        public int? KolichestvoTcvetov2 { get; set; }

        [Display(Name = "поле запечатки более 30%")]
        public bool PoleZapechatki { get; set; }
        

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Paket;
            rr.param12 = KolichestvoTcvetov1;
            rr.param21 = KolichestvoTcvetov2;
            rr.param14 = PoleZapechatki;
            return rr;
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (KolichestvoTcvetov1 == null || Paket == null || Tiraz == null) continue;

                decimal cena;
                if (TryGetPrice(i, Tiraz, KolichestvoTcvetov1, out cena) == false) continue;

                if (KolichestvoTcvetov2 != null)
                {
                    decimal cena2;
                    if (TryGetPrice(i, Tiraz, KolichestvoTcvetov2, out cena2) == false) continue;
                    cena += cena2;
                }

                kvotaEntities db = new kvotaEntities();
                
                double cenaPaketa = (from p in db.Price where p.firma == (int)i && p.catId == Paket select p.cena).First();



                cena = cena * (PoleZapechatki? 1.25m : 1m) + (decimal)cenaPaketa;
                line.Cena = cena * (decimal)Tiraz.Value;
            }
            return ret;

        }

        public PaketPvd(): base(TipProds.PaketPvd, "EditПакетыПВД")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 412 select pp), "id", "tip");
            ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == 413 select pp), "id", "tip");
           // ViewData["params3"] = new SelectList((from pp in db.Category where pp.parentId == 413 select pp), "id", "tip");            
        }
    }

}