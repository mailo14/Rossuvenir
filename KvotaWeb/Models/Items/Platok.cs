using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Platok : ItemBase
    {
        public override string Srok { get; set; } = "1-4 рабочих дня";
        public override string Description { get; set; } = "Платки из ткани -платочный шелк- (плотность 60гр/м2). Обработка по краю изделия - терморез";

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }

        [Display(Name = "оверлок")]
         public bool Overlok{ get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            rr.param14 = Overlok;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new Platok() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Razmer = li.param11,
                Overlok = li.param14
            };
        }
      
public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if ( Tiraz == null || Razmer == null) continue;

                kvotaEntities db = new kvotaEntities();
                decimal cena;
             if (TryGetPrice(i, Tiraz, Razmer, out cena) == false) continue;
            
                if (Overlok) cena += 30;

                 line.Cena = cena * (decimal)Tiraz.Value;
            }
            ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena;
            return ret;

        }


        public Platok():base( TipProds.Platok, "EditPlatok")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 434 select pp), "id", "tip");
        }
    }

}