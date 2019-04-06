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

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }
        
        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            return rr;
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            InnerMessageIds.Add(InnerMessages.AskBetterPrice);
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Razmer == null || Tiraz == null) continue;

                kvotaEntities db = new kvotaEntities();
                double cena = (from p in db.Price where p.firma == (int)i && p.catId == Razmer select p.cena).First();

                line.Cena = cena * Tiraz.Value;
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