using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Tisnenie : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней";

        [Display(Name = "Вид:")]
        public int? Vid { get; set; }

        [Display(Name = "Площадь, кв.см:")]
         public double? Ploshad { get; set; }

        [Display(Name = "клише уже изготовлено")]
         public bool KlisheExists { get; set; }




        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
            rr.param13 = Ploshad;
            rr.param14 = KlisheExists;
            return rr;
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Vid == null || Tiraz == null || !KlisheExists && Ploshad==null) continue;

                double cena;
                if (TryGetPrice(i, Tiraz, Vid, out cena) == false) continue;

                double cenaKlishe = 0;
                if (!KlisheExists) cenaKlishe = GetCenaKlishe(i, Ploshad.Value);
                line.Cena = cena * Tiraz.Value+ cenaKlishe;
            }
            return ret;

        }

        private double GetCenaKlishe(Postavs firma, double ploshad)
        {
            if (firma==Postavs.РРЦ_1_5 || firma == Postavs.ААА)
            {
                if (ploshad < 31) return 850;
                if (ploshad < 41) return 1000;
                if (ploshad <= 50) return 1300;
                return 1300+Math.Ceiling(ploshad-50)*18;
            }
            if (firma == Postavs.АртСувенир)
             return 1000;
            if (firma == Postavs.Плановая_СС)
            {
                if (ploshad < 31) return 1080;
                return 1400;
            }
            return 0;
        }

        public Tisnenie(): base(TipProds.Tisnenie, "EditТиснение")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 401 select pp), "id", "tip");
            
        }
    }

}