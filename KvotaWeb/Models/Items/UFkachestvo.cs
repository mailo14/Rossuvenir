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
        public override string Srok { get; set; } = "от 2-х рабочих дней";

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
                        ViewData["IndividPersParamDivStyle"] = "display:none;";
                        ViewData["RazmerZapechatkiParamDivStyle"] = "display:none;";
                }
                else
                {
                    if (value == 172)
                    {
                        ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");

                        ViewData["RazmerZapechatkiParamDivStyle"] = "display:block;";
                        ViewData["IndividPersParamDivStyle"] = "display:none;";
                    }
                    else
                    {ViewData["params2"] =empty;

                        ViewData["RazmerZapechatkiParamDivStyle"] = "display:none;";
                        ViewData["IndividPersParamDivStyle"] = "display:block;";
                    }
                }
            } }

        [Display(Name = "Размер запечатки:")]
         public int? RazmerZapechatki { get; set; }

        [Display(Name = "индивидуальная персонализация")]
         public bool IndividPers{ get; set; }
        
        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Izdelie;
            rr.param12 = RazmerZapechatki;
            rr.param14 = IndividPers;
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
                IndividPers = li.param14
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
                    || (Izdelie==172 && RazmerZapechatki == null)
                    ) continue;

                decimal cena;
                if (Izdelie == 172)
                {
                    if (Tiraz >= 200)
                    {
                        InnerMessageIds.Add(InnerMessages.IndividPrice);
                        continue;
                    }
                    if (TryGetPrice(i, Tiraz, RazmerZapechatki, out cena) == false) continue;
                }
                else
                {
                    if (TryGetPrice(i, Tiraz, Izdelie, out cena) == false) continue;
                    cena = cena * (IndividPers ? 1.3m : 1m);
                }
                if (new int[]{ 170,171,172}.Contains(Izdelie.Value) && Tiraz<10)
                    line.Cena = cena ;
                else
                    line.Cena = cena * (decimal)Tiraz.Value;
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