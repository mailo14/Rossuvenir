using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Tampopechat : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней";

        int? _Osnova = null;
        [Display(Name = "Основа:")]
         public int? Osnova { get { return _Osnova; } set {
                if (value != _Osnova)
                {
                    _Osnova = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null) ViewData["params2"] = empty;
                else ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
            } }

        [Display(Name = "Количество цветов:")]
         public int? KolichestvoTcvetov { get; set; }


        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Osnova;
            rr.param12 = KolichestvoTcvetov;
            return rr;
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (KolichestvoTcvetov == null || Tiraz == null) continue;

                decimal cena;
                if (TryGetPrice(i, Tiraz, KolichestvoTcvetov, out cena) == false) continue;

                line.Cena = cena * (decimal)Tiraz.Value;
            }
            return ret;

        }

        public Tampopechat(): base(TipProds.Tampopechat, "EditТампопечать")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 405 select pp), "id", "tip");
            
        }
    }

}