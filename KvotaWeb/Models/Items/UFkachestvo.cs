using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class UFkachestvo : ItemBase
    {
        public override string Srok { get; set; } = "от 2-х рабочих дней" + SrokPripiska;

        int? _Izdelie = null;
        [Display(Name = "Изделие:")]
         public int? Izdelie { get { return _Izdelie; } set {
                if (value != _Izdelie)
                {
                    _Izdelie = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null)
                {
                    ViewData["params2"] = empty;
                        ViewData["RazmerZapechatkiParamDivStyle"] = "display:none;";
                }
                else
                {
                    if (value !=641)//== 172 || value == 701)
                    {
                        ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");

                        ViewData["RazmerZapechatkiParamDivStyle"] = "display:block;";
                    }
                    else
                    {ViewData["params2"] =empty;

                        ViewData["RazmerZapechatkiParamDivStyle"] = "display:none;";
                    }

                }
                if (value == 702)
                {ViewData["paramDivStyle5"] = "display:block";
                    ViewData["paramDivStyle6"] = "display:block";
                    ViewData["paramDivStyle7"] = "display:block";
                }
                else
                {
                    ViewData["paramDivStyle5"] = "display:none";
                    ViewData["paramDivStyle6"] = "display:none";
                    ViewData["paramDivStyle7"] = "display:none";
                }
            } }

        [Display(Name = "Размер запечатки/вид/цвет:")]
         public int? RazmerZapechatki { get; set; }

        [Display(Name = "двухсторонняя")]
        public bool Dvustoronnya { get; set; }

        [Display(Name = "заливка более 50% площади")]
        public bool Zalivka50 { get; set; }

        [Display(Name = "печать 'с подъемом'")]
        public bool SPodyomom { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Izdelie;
            rr.param12 = RazmerZapechatki;
            rr.param15 = Dvustoronnya;
            rr.param24 = Zalivka50;
            rr.param25 = SPodyomom;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new UFkachestvo()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Izdelie = li.param11,
                RazmerZapechatki = li.param12,

                Dvustoronnya = li.param15,
                Zalivka50 = li.param24,
                SPodyomom = li.param25
            };
        }
        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();

            kvotaEntities db = new kvotaEntities();

            if (Izdelie != null && (Izdelie != 641 || Izdelie ==641 && RazmerZapechatki==null) && Tiraz != null )
                foreach (var firma in db.Firma)
                {
                    PriceDto cena;
                    var param = (Izdelie ==641) ? Izdelie : RazmerZapechatki;
                    if (TryGetPrice(firma.id, Tiraz, param, out cena) == false) continue;

                    var line = new CalcLine() { FirmaId = firma.id };
                    line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;

                    if (Zalivka50) line.Cena *= 1.3m;
                    if (SPodyomom) line.Cena *= 1.3m;
                    if (Dvustoronnya) line.Cena *= 2m;
                    
                    ret.Add(line);
                }

            return ret;
        }

        public UFkachestvo(): base(TipProds.UFkachestvo, "EditУФкачество")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 408 select pp), "id", "tip");
          
            
        }

        
    }

}