using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Skatert : ItemBase
    {
        public override string Srok { get; set; } = "1-3 рабочих дня";
        public override string Description { get; set; } = "В стоимость включена обработка одинарной строкой или оверлок. По умолчанию изготовление на ткани искусственный шелк (таффета)";

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }

        [Display(Name = "на тканях мокрый щёлк, атлас, габардин")]
         public bool Mokrii { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            rr.param14 = Mokrii;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new Skatert() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Razmer = li.param11,
                Mokrii = li.param14
            };
        }
      
public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (i == Postavs.Плановая_СС)
                {
                    if (Tiraz == null || Razmer == null) continue;

                    kvotaEntities db = new kvotaEntities();
                    decimal cena;
                    if (TryGetPrice(i, Tiraz, Razmer, out cena) == false) continue;

                    decimal nacenk = 0;
                    if (Mokrii) nacenk += 0.2m;

                    line.Cena = cena * (1m + nacenk) * (decimal)Tiraz.Value;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*pCena;
            return ret;

        }


        public Skatert():base( TipProds.Skatert, "EditSkatert")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 432 select pp), "id", "tip");

        }
    }

}