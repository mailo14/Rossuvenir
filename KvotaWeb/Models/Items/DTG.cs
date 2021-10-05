using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class DTG : ItemBase
    {
        public override string Srok { get; set; } = "от 3-х рабочих дней" + SrokPripiska;

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
            return new DTG()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Razmer = li.param11
            };
        }

        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();

            kvotaEntities db = new kvotaEntities();

            if (Razmer != null && Tiraz != null)
                foreach (var firma in db.Firma)
                {
                    PriceDto cena;
                    if (TryGetPrice(firma.id, Tiraz, Razmer, out  cena) == false) continue;

                    var line = new CalcLine() { FirmaId = firma.id };
                    line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                    ret.Add(line);
                }

            return ret;
        }

        public DTG():base( TipProds.DTG, "EditDTG")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 415 select pp), "id", "tip");
        }
    }

}