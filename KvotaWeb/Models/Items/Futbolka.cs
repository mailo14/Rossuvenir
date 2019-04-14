using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Futbolka : ItemBase
    {
        public override string Srok { get; set; } = "от 1 часа до 3-5 рабочих дней";
        public override string Description { get; set; } = "Цены указаны до А4 формата, без учета стоимости носителя. При нанесении изображения большого формата, цена рассчитывается индивидуально. Тираж менее 20 шт.и более 1000 шт.рассчитывается индивидуально· В стоимость не включена цена носителя.";

        [Display(Name = "Вид/основа:")]
         public int? Osnova { get; set; }

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
            rr.param11 = Osnova;
            rr.param12 = Material;
            rr.param14 = Dvustoronnii;
            rr.param13 = SvoiRazmerH;
            rr.param23 = SvoiRazmerL;
            rr.param24 = Dorabotka;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new Futbolka() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Osnova = li.param11,
                Material =li.param12??0, Dvustoronnii=li.param14,
                SvoiRazmerH=li.param13, SvoiRazmerL = li.param23,
                Dorabotka=li.param24
            };
        }
      
public override List<CalcLine> Calc()

        {
            if (Tiraz <20  && Tiraz >= 1000) InnerMessageIds.Add(InnerMessages.AskBetterPrice); ;

            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if ( Tiraz == null || Osnova == null ) continue;

                kvotaEntities db = new kvotaEntities();
                decimal cena;
                if (Osnova != null && SvoiRazmerH==null && SvoiRazmerL==null)
                {
                    if (TryGetPrice(i, Tiraz, Osnova, out cena) == false) continue;
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

                 line.Cena = cena *(1m+nacenk)* (decimal)Tiraz.Value
                    +(Dorabotka?150m:0);
            }
            ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena;
            return ret;

        }


        public Futbolka():base( TipProds.Futbolka, "EditFutbolka")
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