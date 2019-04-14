using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Flag : ItemBase
    {
        public override string Srok { get; set; } = "1-3 рабочих дня";
        public override string Description { get; set; } = "В стоимость включена обработка флага двойной строкой, карман, петли, завязки.По желанию заказчика допольнительно можно сделать глухой карман, петлю в карман снизу + 0руб. Двухсторонний сшивной флаг - из двух слоев запечатанной ткани, с тканевой прослойкой внутри. Подробнее:  sgrafika.ru Люверсы и лента крепления включена в стоимость флага";

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }

        [Display(Name = "Материал:")]
         public int? Material { get; set; }

        [Display(Name = "двусторонний")]
         public bool Dvustoronnii { get; set; }
        
        [Display(Name = "Свой размер: высота, cм:")]
         public double? SvoiRazmerH { get; set; }

        [Display(Name = "Свой размер: ширина, cм:")]
        public double? SvoiRazmerL { get; set; }

        [Display(Name = "доработка дизайна")]
         public bool Dorabotka { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            rr.param12 = Material;
            rr.param14 = Dvustoronnii;
            rr.param13 = SvoiRazmerH;
            rr.param23 = SvoiRazmerL;
            rr.param24 = Dorabotka;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new Flag() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Razmer = li.param11,
                Material =li.param12??0, Dvustoronnii=li.param14,
                SvoiRazmerH=li.param13, SvoiRazmerL = li.param23,
                Dorabotka=li.param24
            };
        }
      
public override List<CalcLine> Calc()

        {
            if (Tiraz >= 801) InnerMessageIds.Add(InnerMessages.AskBetterPrice); ;

            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if ( Tiraz == null || Razmer == null  && (SvoiRazmerH == null || SvoiRazmerL == null)) continue;

                kvotaEntities db = new kvotaEntities();
                decimal cena;
                if (Razmer != null && SvoiRazmerH==null && SvoiRazmerL==null)
                {
                    if (TryGetPrice(i, Tiraz, Razmer, out cena) == false) continue;
                }
                else
                {
                    if( SvoiRazmerH == null || SvoiRazmerL == null) continue;
                    cena = (decimal)SvoiRazmerH.Value * (decimal)SvoiRazmerL.Value / 100 / 100 * (Tiraz > 3 ? 500 : 530);                     
                }
                decimal nacenk=0;
                if (Material == 1) nacenk += 0.15m;
                else if (Material == 2) nacenk += 0.20m;
                if (Dvustoronnii) nacenk += 1.20m;

                 line.Cena = cena *(1m+nacenk)* (decimal)Tiraz.Value;
            }
            return ret;

        }


        public Flag():base( TipProds.Flag, "EditFlag")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 428 select pp), "id", "tip");

            ViewData["params2"] = new SelectList(new[] {
                new { id = 0, tip= "флажный шелк Премиум 80гр" },
                new { id = 1, tip= "флажная сетка 110гр" },
                new { id = 2, tip= "атлас, габардин" },
            }, "id", "tip");
        }
    }

}