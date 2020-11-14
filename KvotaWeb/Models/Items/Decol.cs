using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Decol : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней" + SrokPripiska;

        int? _Vid = null;
        [Display(Name = "Вид посуды:")]
         public int? Vid { get { return _Vid; } set {
                if (value != _Vid)
                {
                    _Vid = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null)
                {
                    ViewData["params2"] = empty;
                        ViewData["DiametrParamDivStyle"] = "display:none;";
                }
                else
                {
                    ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");

                    if (value == 307)
                        ViewData["DiametrParamDivStyle"] = "display:none;";
                    else ViewData["DiametrParamDivStyle"] = "display:block;";
                }
                } }

        [Display(Name = "Количество цветов:")]
         public int? KolichestvoTcvetov { get; set; }


        [Display(Name = "Площадь, кв.см:")]
        public double? Ploshad { get; set; }

        [Display(Name = "Диаметр основы")]
        public int? Diametr { get; set; }

        [Display(Name = "печать золотой краской")]
         public bool Zolotoi { get; set; }
        

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
            rr.param12 = KolichestvoTcvetov;
            rr.param13 = Ploshad;
            rr.param14 = Zolotoi;
            rr.param21 = Diametr;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Decol()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Vid = li.param11,
                KolichestvoTcvetov = li.param12,
                Ploshad=li.param13,
           Zolotoi= li.param14,
            Diametr= li.param21
            };
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Vid == null|| KolichestvoTcvetov == null || Tiraz == null || Ploshad==null || Vid == 308 && Diametr==null) continue;

                decimal cena;
                if (TryGetPrice(i, Tiraz, KolichestvoTcvetov, out cena) == false) continue;

                double koef = 1;
                if (Vid == 307)
                {
                    if (Ploshad >= 51) koef = 1.1;
                    else if (Ploshad >= 101) koef = 1.2;
                    else if (Ploshad >= 151) koef = 1.4;
                }
                else
                {
                    if (Diametr == 319) koef = 0.8;
                    else if (Diametr == 321) koef = 1.1;
                }
                if (Zolotoi) cena += (decimal)Ploshad.Value * TryGetSingleParam(803);
                cena = cena * (decimal)koef;
                line.Cena = cena * (decimal)Tiraz.Value;
            }
            return ret;

        }

        public Decol(): base(TipProds.Decol, "EditДеколь")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 410 select pp), "id", "tip");
            ViewData["params21"] = new SelectList((from pp in db.Category where pp.parentId == 427 select pp), "id", "tip");
        
        }
    }

}