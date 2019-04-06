using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Gravirovka : ItemBase
    {
        [Display(Name = "Вид:")]
        public int? Vid { get; set; }

        [Display(Name = "Площадь, кв.см:")]
         public double? Ploshad { get; set; }




        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
            rr.param13 = Ploshad;
            return rr;
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            InnerMessageIds.Add(InnerMessages.AskBetterPrice);
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Vid == null || Tiraz == null || Ploshad ==null) continue;

                var ploshad = Math.Max(2.5, Ploshad.Value);
                double? cena= GetCena(i, Vid.Value, ploshad);
                if (cena == null) continue;

                double? cenaPriladki = GetCenaPriladki(i, Vid.Value);
                if (cenaPriladki == null) continue;

                line.Cena = cena.Value * Tiraz.Value+ cenaPriladki.Value;
            }
            return ret;

        }

        private double? GetCenaPriladki(Postavs firma, int vid)
        {
            if (firma == Postavs.РРЦ_1_5) return 450;
            if (firma == Postavs.ААА) return 350;
            if (firma == Postavs.Плановая_СС) return 300;
            return null;
        }

        private double? GetCena(Postavs firma, int vid, double ploshad)
        {
            if (firma == Postavs.РРЦ_1_5)
            {
                if (vid == 66) return 7.5 * ploshad;
                if (vid == 67) return 11 * ploshad;
            }

            if (firma == Postavs.ААА)
            {
                if (vid == 66) return 7 * ploshad;
                if (vid == 67) return 10.5 * ploshad;
            }

            if (firma == Postavs.Плановая_СС)
            {
                if (vid == 66) return 5 * ploshad;
                if (vid == 67) return 7 * ploshad;
            }
            return null;
        }
                 

        public Gravirovka(): base(TipProds.Gravirovka, "EditГравировка")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 411 select pp), "id", "tip");
            
        }
    }

}