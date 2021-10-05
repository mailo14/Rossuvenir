using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class PaketPvd : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней" + SrokPripiska;

        [Display(Name = "Пакет:")]
        public int? Paket { get; set; }

        [Display(Name = "Цвет шелкографии (1 сторона):")]
        public int? KolichestvoTcvetov1 { get; set; }

        [Display(Name = "Цвет шелкографии (2 сторона):")]
        public int? KolichestvoTcvetov2 { get; set; }

        [Display(Name = "поле запечатки более 30%")]
        public bool PoleZapechatki { get; set; }
        

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Paket;
            rr.param12 = KolichestvoTcvetov1;
            rr.param21 = KolichestvoTcvetov2;
            rr.param14 = PoleZapechatki;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new PaketPvd()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                Paket = li.param11,
                KolichestvoTcvetov1 = li.param12,
                KolichestvoTcvetov2 = li.param21,
                PoleZapechatki = li.param14
            };
        }

        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();

            kvotaEntities db = new kvotaEntities();

            if (KolichestvoTcvetov1 != null && Paket != null && Tiraz != null)
                foreach (var firma in db.Firma)
                {
                    var line = new CalcLine() { FirmaId = firma.id };
                    PriceDto cena;
                    decimal paramVal;
                    if (TryGetSingleParam(Paket.Value,firma.id,  out paramVal) == false) continue;
                    line.Cena = paramVal * (decimal)Tiraz.Value;

                    if (TryGetPrice(firma.id, Tiraz, KolichestvoTcvetov1, out cena) == false) continue;

                    line.Cena += cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;

                    if (KolichestvoTcvetov2.HasValue)
                    {
                        if (TryGetPrice(firma.id, Tiraz, KolichestvoTcvetov2, out cena) == false) continue;
                        line.Cena += cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                    }

                    if (PoleZapechatki)
                    {
                        TryGetSingleParam(705,firma.id,  out paramVal);
                        line.Cena *= paramVal;
                    }

                    ret.Add(line);
                }

            return ret;


        }

        public PaketPvd(): base(TipProds.PaketPvd, "EditПакетыПВД")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 412 select pp), "id", "tip");
            ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == 413 select pp), "id", "tip");
           // ViewData["params3"] = new SelectList((from pp in db.Category where pp.parentId == 413 select pp), "id", "tip");            
        }
    }

}