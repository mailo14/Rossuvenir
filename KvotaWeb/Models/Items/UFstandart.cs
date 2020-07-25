using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class UFstandart : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней";

        public override string Description { get; set; } = "Если размер нанесения превышает 50 кв.см., тогда к стоимости добавляется 0,5 руб. за каждый кв.см.при печати без подложки и 0,6 руб.за каждый кв.см.при печати с белой подложкой"
 +Environment.NewLine+"При печати на цилиндрической поверхности +30% (бутылки, термокружки и т.д.)"
 + Environment.NewLine + "Каждое дополнительное место печати рассчитывается как печать на отдельном предмете.";

        int? _Izdelie = null;
        [Display(Name = "Изделие:")]
         public int? Izdelie { get { return _Izdelie; } set {
                if (value != _Izdelie)
                {
                    _Izdelie = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null) ViewData["params2"] = empty;
                else
                    ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");                
            } }

        [Display(Name = "Цвет:")]
         public int? Tcvet { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Izdelie;
            rr.param12 = Tcvet;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new UFstandart()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Izdelie = li.param11,
                Tcvet = li.param12
            };
        }
        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Izdelie== null || Tiraz == null
                    || Tcvet == null                    ) continue;

                decimal cena;
               
                    if (TryGetPrice(i, Tiraz, Tcvet, out cena) == false) continue;
                    line.Cena = cena * (decimal)Tiraz.Value;
            }
            return ret;
        }

        public UFstandart(): base(TipProds.UFstandart, "EditУФстандарт")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 426 select pp), "id", "tip");            
        }
    }

}