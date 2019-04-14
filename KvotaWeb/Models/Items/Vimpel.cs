using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Vimpel : ItemBase
    {
        public override string Srok { get; set; } = "1-5 рабочих дня";
        public override string Description { get; set; } = "Материал атлас, мокрый щёлк. Виды: с жесткой вставкой внутри, обшитые шнуром + кисть на клине, или мягкие. На одностороннем вымпеле оборотная сторона белая";

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }
        
        [Display(Name = "мягкий")]
         public bool Myagkii { get; set; }
        
        [Display(Name = "запечатка с двух сторон")]
         public bool Zapechatka { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            rr.param14 = Myagkii;
            rr.param24 = Zapechatka;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new Vimpel() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Razmer = li.param11,
               Myagkii=li.param14,
                Zapechatka = li.param24
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

                decimal nacenk=0;
                if (Myagkii) nacenk += 0.1m;
                if (Zapechatka ) nacenk += 0.5m;                

                 line.Cena = cena *(1m+nacenk)* (decimal)Tiraz.Value;
            }
            ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena;
            return ret;

        }


        public Vimpel():base( TipProds.Vimpel, "EditVimpel")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 431 select pp), "id", "tip");

        }
    }

}