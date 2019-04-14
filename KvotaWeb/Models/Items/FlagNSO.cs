using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class FlagNSO : ItemBase
    {
        public override string Srok { get; set; } = "1-3 рабочих дня";
        public override string Description { get; set; } = "В наличии и под заказ. Размер 90х135. Материал Флажный шелк, обработка по периметру флага двойной строкой, карман слева 5см";

        [Display(Name = "Размер:")]
        public int? Razmer { get; set; } = 430;

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new FlagPobedi() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz,
                Razmer = li.param11            };
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

                line.Cena = cena * (decimal)Tiraz.Value;
            }
            ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena;
            return ret;
        }

        public FlagNSO():base( TipProds.FlagNSO, "EditFlagNSO")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
        }
    }

}