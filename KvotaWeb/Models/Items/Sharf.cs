using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Sharf : ItemBase
    {
        public override string Srok { get; set; } = "1-4 рабочих дня";
        public override string Description { get; set; } = "По умолчанию изготовление на ткани искусственный шелк (таффета)";

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }

        [Display(Name = "на тканях мокрый атлас, габардин")]
        public bool Atlas { get; set; }

        [Display(Name = "внутренняя тканевая прослойка")]
        public bool VnutrProsloika { get; set; }

        [Display(Name = "бахрома по краям")]
         public bool Bohroma { get; set; }
     
        [Display(Name = "обстрочка одинарной строкой")]
         public bool Obstrochka { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            rr.param14 = Atlas;
            rr.param15 = VnutrProsloika;
            rr.param24 = Bohroma;
            rr.param25 = Obstrochka;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new Sharf() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Razmer = li.param11,
                Atlas = li.param14,
                VnutrProsloika = li.param15,
                Bohroma = li.param24,
                Obstrochka = li.param25
            };
        }
      
public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if ( Tiraz == null || Razmer == null  ) continue;

                kvotaEntities db = new kvotaEntities();
                decimal cena;
                  if (TryGetPrice(i, Tiraz, Razmer, out cena) == false) continue;
                
                if (Atlas) cena = cena * (1 + 0.2m);
                if (VnutrProsloika) cena += 30;
                if (Bohroma) cena += 30;
                if (Obstrochka) cena += 25;

                line.Cena = cena * (decimal)Tiraz.Value;
            }
            ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena;
            return ret;
        }


        public Sharf():base( TipProds.Sharf, "EditSharf")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 433 select pp), "id", "tip");
        }
    }

}