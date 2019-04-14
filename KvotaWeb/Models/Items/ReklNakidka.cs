using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class ReklNakidka : ItemBase
    {
        public override string Srok { get; set; } = "1-3 рабочих дня";
        public override string Description { get; set; } = "Размер 60х70, 50х70. По умолчанию материал - искусственный шелк(таффета). Полный сублимационный окрас ткани с двух сторон изделия. Обработка: обшивка косой бейкой, завязки по бокам";

        [Display(Name = "Размер:")]
        public int? Razmer { get; set; } = 435;

        [Display(Name = "изготовление на габардине")]
        public bool Gabardin { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new ReklNakidka() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz,
                Razmer = li.param11            };
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

                    line.Cena = cena * (Gabardin ? 1.2m : 1m) * (decimal)Tiraz.Value;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*pCena;
            return ret;
        }

        public ReklNakidka():base( TipProds.ReklNakidka, "EditReklNakidka")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); 
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
        }
    }

}