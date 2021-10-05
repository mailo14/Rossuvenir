using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Shelkografiya : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней" + SrokPripiska;

        int? _Tcvet = null;
        [Display(Name = "Цвет основы:")]
         public int? Tcvet { get { return _Tcvet; } set {
                if (value != _Tcvet)
                {
                    _Tcvet = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null) ViewData["params2"] = empty;
                else ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
            } }

        [Display(Name = "Количество цветов:")]
         public int? KolichestvoTcvetov { get; set; }

        [Display(Name = "термотрансфер")]
         public bool Termotransfer { get; set; }

        //[Display(Name = "печать более формата А4 или площади 600 кв. см.")]
        // public bool FormatA3 { get; set; }

        [Display(Name = "Площадь печати")]
        public int? Ploshad { get; set; }


        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Tcvet;
            rr.param12 = KolichestvoTcvetov;
            rr.param14 = Termotransfer;
            rr.param21 = Ploshad;
            return rr;
        }

        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();

            kvotaEntities db = new kvotaEntities();

            if (KolichestvoTcvetov != null && Tiraz != null && Ploshad != null)
                foreach (var firma in db.Firma)
                {
                    PriceDto cena;
                    if (TryGetPrice(firma.id, Tiraz, KolichestvoTcvetov, out cena) == false) continue;

                    var line = new CalcLine() { FirmaId = firma.id };
                    line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;

                    line.Cena *= (Termotransfer ? 1.5m : 1m);

                    decimal nacenk;
                    if (TryGetSingleParam(Ploshad.Value, firma.id, out nacenk))
                        line.Cena *= nacenk;
                    else continue;

                    ret.Add(line);
                }

            return ret;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Shelkografiya()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Tcvet = li.param11,
                KolichestvoTcvetov = li.param12
                    ,
                Termotransfer = li.param14,
                Ploshad = li.param21
            };
        }

        public Shelkografiya(): base(TipProds.Shelkografiya, "EditШелкография")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 35/*403*/ select pp), "id", "tip");
            ViewData["params3"] = new SelectList((from pp in db.Category where pp.parentId == 660/*403*/ select pp), "id", "tip");
        }
    }

}