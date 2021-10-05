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
        public override string Srok { get; set; } = "от 5-ти рабочих дней" + SrokPripiska;

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

        public static ItemBase CreateItem(ListItem li)
        {
            return new Tampopechat()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Osnova = li.param11,
                KolichestvoTcvetov = li.param12
            };
        }

        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();

            kvotaEntities db = new kvotaEntities();

            if (KolichestvoTcvetov != null && Tiraz != null)
                foreach (var firma in db.Firma)
                {
                    PriceDto cena;
                    if (TryGetPrice(firma.id, Tiraz, KolichestvoTcvetov, out cena) == false) continue;

                    var line = new CalcLine() { FirmaId = firma.id };
                    line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                    ret.Add(line);
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