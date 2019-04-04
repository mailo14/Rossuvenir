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
         public int Id { get; set; }
        public int TipProd { get; set; } = 4;

        public int? ZakazId { get; set; } 

        [Display(Name = "Цвет основы")]
         public int? Tcvet { get; set; }

        [Display(Name = "Количество цветов")]
         public int? KolichestvoTcvetov { get; set; }

        [Display(Name = "печать на синтетических тканях (сумки, плащевки, зонты)")]
         public bool Sintetika { get; set; }

        [Display(Name = "печать более формата А4 или площади 600 кв. см.")]
        public bool FormatA3 { get; set; }

        public string TotalLabel { get; set; }
        
        
        public override ListItem ToListItem()
        {
            return new ListItem() {
                vid1=201,tipProd= TipProd,
                listId=ZakazId,
                id=Id,
                // param11= Razmer,
                tiraz = Tiraz
            };
        }

        public override List<CalcLine> Calc()

        {
            kvotaEntities db = new kvotaEntities();
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (KolichestvoTcvetov == 0 || Tiraz == 0) continue;

                double cena = 0;
                var minTiraz = (from p in db.Price where p.firma == (int)i && p.catId == KolichestvoTcvetov orderby p.tiraz select (int?)p.tiraz).FirstOrDefault();
                if (!minTiraz.HasValue) continue;

                var maxTirazi = (from p in db.Price where p.firma == (int)i && p.catId == KolichestvoTcvetov orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                if (Tiraz > 2 * maxTirazi[0] - maxTirazi[1]) askBetterPrice = true;
                if (Tiraz < minTiraz)
                {
                    cena = (from p in db.Price where p.firma == (int)i && p.catId == KolichestvoTcvetov && p.tiraz == minTiraz select p.cena).First();
                    cena = cena * minTiraz.Value / Tiraz.Value;
                }
                else
                    cena = (from p in db.Price where p.firma == (int)i && p.catId == KolichestvoTcvetov && p.tiraz <= Tiraz orderby p.tiraz descending select p.cena).First();
                line.Cena = cena * Tiraz.Value;
            }
            return ret;

        }

        public Znachok()
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 414 select pp), "id", "tip");
        }
    }

}