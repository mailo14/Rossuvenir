﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Znachok: ItemBase
    {
         public int Id { get; set; }
        public int TipProd { get; set; } = 4;

        public int? ZakazId { get; set; } 

        [Display(Name = "Размер")]
         public int? Razmer { get; set; }

        public string TotalLabel { get; set; }
        
        
        public override ListItem ToListItem()
        {
            return new ListItem() {
                vid1=201,tipProd= TipProd,
                listId=ZakazId,
                id=Id,
                 param11= Razmer,  tiraz= Tiraz
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
                if (Razmer == 0 || Tiraz == 0) continue;

                double cena = 0;
                var minTiraz = (from p in db.Price where p.firma == (int)i && p.catId == Razmer orderby p.tiraz select (int?)p.tiraz).FirstOrDefault();
                if (!minTiraz.HasValue) continue;

                var maxTirazi = (from p in db.Price where p.firma == (int)i && p.catId == Razmer orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                if (Tiraz > 2 * maxTirazi[0] - maxTirazi[1]) askBetterPrice = true;
                if (Tiraz < minTiraz)
                {
                    cena = (from p in db.Price where p.firma == (int)i && p.catId == Razmer && p.tiraz == minTiraz select p.cena).First();
                    cena = cena * minTiraz.Value / Tiraz.Value;
                }
                else
                    cena = (from p in db.Price where p.firma == (int)i && p.catId == Razmer && p.tiraz <= Tiraz orderby p.tiraz descending select p.cena).First();
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

    public class CalcLine
    {
        public Postavs Postav { get; set; }
        public double? Cena { get; set; }
    }
    public enum Postavs:int { РРЦ_1_5=1, АртСувенир=2, ААА=3, Плановая_СС=10};
    public abstract class ItemBase
    {public bool askBetterPrice { get; set; }
        public ViewDataDictionary ViewData { get; set; }

        [Display(Name = "Тираж")]
        public double? Tiraz { get; set; }

        public abstract ListItem ToListItem();

        public abstract List<CalcLine> Calc();

        public static ItemBase Create(ListItem li)
        {
            switch (li.tipProd)
            {
                case 4:
                    return new Znachok() {Id=li.id,ZakazId= li.listId,  Razmer = li.param11, Tiraz = li.tiraz };
                default:
                    return null;
            }
        }
    }
}