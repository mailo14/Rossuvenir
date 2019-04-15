using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class BumajniiPaket : ItemBase
    {
        public override string Srok { get; set; } = null;
        public override string Description { get; set; } = "Мелованная бумага 170 гр., печать 4+0, глянцевая ламинация 1+0, установка люверсов, ручек, укрепление дна картоном";

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }
        
        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new BumajniiPaket() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Razmer = li.param11 };
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
                    if (Razmer == null || Tiraz == null) continue;

                    kvotaEntities db = new kvotaEntities();
                    decimal cena;
                    if (TryGetPrice(i, Tiraz, Razmer, out cena) == false) continue;

                    line.Cena = cena * (decimal)Tiraz.Value;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena = 1.5m * pCena;
            return ret;
        }


        public BumajniiPaket():base( TipProds.BumajniiPaket, "EditBumajniiPaket")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 419 select pp), "id", "tip");
        }
    }

}