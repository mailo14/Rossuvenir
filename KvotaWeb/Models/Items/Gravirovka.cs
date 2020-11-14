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
        public override string Srok { get; set; } = "от 2-х рабочих дней" + SrokPripiska;
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

        public static ItemBase CreateItem(ListItem li)
        {
            return new Gravirovka()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Vid = li.param11,
                Ploshad = li.param13,
            };
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Vid == null || Tiraz == null || Ploshad ==null) continue;

                var ploshad = Math.Max(2, Ploshad.Value);
                decimal? cena= GetCena(i, Vid.Value, (decimal)ploshad);
                if (cena == null) continue;

                decimal? cenaPriladki = GetCenaPriladki(i, Vid.Value);
                if (cenaPriladki == null) continue;

                line.Cena = cena.Value * (decimal)Tiraz.Value+ cenaPriladki.Value;
            }
            return ret;

        }

        private decimal? GetCenaPriladki(Postavs firma, int vid)
        {
            //if (firma == Postavs.РРЦ_1_5) return 450m;
            //if (firma == Postavs.ААА) return 350m;

            if (firma == Postavs.РРЦ_1_5)
            {
                var cena = GetCenaPriladki(Postavs.Плановая_СС, vid);
                if (cena.HasValue)
                    return 1.5m * cena.Value;
                else
                    return null;
            }
            if (firma == Postavs.Плановая_СС)
                return TryGetSingleParam(802);

            return null;
        }

        private decimal? GetCena(Postavs firma, int vid, decimal ploshad)
        {
            /*if (firma == Postavs.РРЦ_1_5)
            {
                if (vid == 66) return 7.5m * ploshad;
                if (vid == 67) return 11m * ploshad;
            }

            if (firma == Postavs.ААА)
            {
                if (vid == 66) return 7m * ploshad;
                if (vid == 67) return 10.5m * ploshad;
            }*/

            if (firma == Postavs.РРЦ_1_5)
            {
                var cena= GetCena(Postavs.Плановая_СС, vid, ploshad);
                if (cena.HasValue) return 1.5m * cena.Value;
                else return null;
            }

            if (firma == Postavs.Плановая_СС)
            {
                if (vid == 66) return TryGetSingleParam(800) * ploshad;
                if (vid == 67) return TryGetSingleParam(801) * ploshad;
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