using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Shelkografiya : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней" + SrokPripiska;

        int? _Tcvet = null;
        [Display(Name = "Цвет основы:")]
         public int? Tcvet { get { return _Tcvet; } set {
                if (value != _Tcvet)
                {
                    _Tcvet = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null) ViewData["params2"] = empty;
                else ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
            } }

        [Display(Name = "Количество цветов:")]
         public int? KolichestvoTcvetov { get; set; }

        [Display(Name = "печать на синтетике, сумках, зонтах, рукавах")]
         public bool Sintetika { get; set; }

        //[Display(Name = "печать более формата А4 или площади 600 кв. см.")]
        // public bool FormatA3 { get; set; }

        [Display(Name = "Площадь печати")]
        public int? Ploshad { get; set; }


        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Tcvet;
            rr.param12 = KolichestvoTcvetov;
            rr.param14 = Sintetika;
            rr.param21 = Ploshad;
            return rr;
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (KolichestvoTcvetov == null || Tiraz == null) continue;

                decimal cena;
                if (TryGetPrice(i, Tiraz, KolichestvoTcvetov, out cena) == false) continue;

                //decimal addNacenk
                cena = cena * (Sintetika ? 1.5m : 1m) ;
                if (Ploshad == 0) cena = 0.9m * cena;
                else if (Ploshad == 2) cena = 1.2m * cena;
                line.Cena = cena * (decimal)Tiraz.Value;
            }
            return ret;

        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Shelkografiya()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Tcvet = li.param11,
                KolichestvoTcvetov = li.param12
                    ,
                Sintetika = li.param14,
                Ploshad = li.param21??1
            };
        }

        public Shelkografiya(): base(TipProds.Shelkografiya, "EditШелкография")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 403 select pp), "id", "tip");
            ViewData["params3"] = new SelectList(new[] {
                new { id = 0, tip= "менее формата А4" },
                new { id = 1, tip= "формат А4" },
                new { id = 2, tip= "более формата А4 или площади 600 кв. см." },
            }, "id", "tip");
        }
    }

}