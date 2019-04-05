using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Znachok: ItemBase
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
            askBetterPrice = false;
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Razmer == null || Tiraz == null) continue;

                double cena ;
                if (TryGetPrice( i,Tiraz,Razmer,out cena) == false) continue;

                
                line.Cena = cena * Tiraz.Value;
            }
            return ret;

        }


        public Znachok():base( TipProds.Znachok, "EditЗначки")
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
    public enum TipProds:int { Znachok = 4, Shelkografiya = 23, Tampopechat = 24, PaketPvd = 29, Tisnenie = 30, DTG = 18, Gravirovka = 28 };
    public abstract class ItemBase
    {
        public int? ZakazId { get; set; }
        public string TotalLabel { get; set; }

        protected ItemBase(TipProds tipProd,string viewName)
        {
            TipProd = tipProd;
            ViewName = viewName;
        }

        public int Id { get; set; }
        public TipProds TipProd { get; set; }

        public bool askBetterPrice { get; set; }
        public ViewDataDictionary ViewData { get; set; }

        [Display(Name = "Тираж:")]
        public double? Tiraz { get; set; }
        public string ViewName { get; internal set; }

        public virtual ListItem ToListItem()
        {
            return new ListItem()
            {
                vid1 = (int)TipProd,
                tipProd = (int)TipProd,
                id = Id,
                tiraz = Tiraz,
                    listId = ZakazId,
        };
        }


        public bool TryGetPrice(Postavs firma, double? tiraz, int? catId, out double cena)
        {
            kvotaEntities db = new kvotaEntities();
            cena = 0;
            var minTiraz = (from p in db.Price where p.firma == (int)firma && p.catId == catId orderby p.tiraz select (int?)p.tiraz).FirstOrDefault();
            if (!minTiraz.HasValue) return false;

            var maxTirazi = (from p in db.Price where p.firma == (int)firma && p.catId == catId orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
            if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) askBetterPrice = true;
            if (tiraz < minTiraz)
            {
                cena = (from p in db.Price where p.firma == (int)firma && p.catId == catId && p.tiraz == minTiraz select p.cena).First();
                cena = cena * minTiraz.Value / tiraz.Value;
            }
            else
                cena = (from p in db.Price where p.firma == (int)firma && p.catId == catId && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();

            return true;
        }

        public abstract List<CalcLine> Calc();

        public static ItemBase Create(ListItem li)
        {
            switch ((TipProds)li.tipProd)
            {
                case TipProds.Znachok:
                    return new Znachok() {Id=li.id,ZakazId= li.listId, Tiraz = li.tiraz,  Razmer = li.param11 };
                case TipProds.Shelkografiya:
                    return new Shelkografiya() {Id=li.id,ZakazId= li.listId, Tiraz = li.tiraz,  Tcvet = li.param11,  KolichestvoTcvetov = li.param12
                    ,Sintetika=li.param14,FormatA3=li.param15};                    
  case TipProds.Tampopechat:
                    return new Tampopechat()
                    {
                        Id = li.id,
                        ZakazId = li.listId,
                        Tiraz = li.tiraz,
                        Osnova = li.param11,
                        KolichestvoTcvetov = li.param12
                    };
                case TipProds.PaketPvd:                    return new PaketPvd()
                    {
                        Id = li.id,
                        ZakazId = li.listId,
                        Tiraz = li.tiraz,
                        Paket= li.param11,
                        KolichestvoTcvetov = li.param12,
                        PoleZapechatki=li.param14
                    };   
                case TipProds.Tisnenie:                    return new Tisnenie()
                    {
                        Id = li.id,
                        ZakazId = li.listId,
                        Tiraz = li.tiraz,
                        Vid= li.param11,
                        Ploshad = li.param13,                        KlisheExists=li.param14
                    }; 
                case TipProds.DTG:
                    return new DTG() {Id=li.id,ZakazId= li.listId, Tiraz = li.tiraz,  Razmer = li.param11 };
                case TipProds.Gravirovka:                    return new Gravirovka()
                    {
                        Id = li.id,
                        ZakazId = li.listId,
                        Tiraz = li.tiraz,
                        Vid= li.param11,
                        Ploshad = li.param13,                        
                    }; 
                default:
                    return null;
            }
        }
    }
}