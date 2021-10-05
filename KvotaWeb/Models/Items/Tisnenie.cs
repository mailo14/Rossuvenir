using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Tisnenie : ItemBase
    {
        public override string Srok { get; set; } = "от 5-ти рабочих дней" + SrokPripiska;
int? _Vid = null;
        [Display(Name = "Вид:")] 
        public int? Vid
        {
            get { return _Vid; }
            set
            {
                if (value != _Vid)
                {
                    _Vid = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null) ViewData["params2"] = empty;
                else ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
            }
        }

        [Display(Name = "Материал изделий:")]
        public int? Material { get; set; }

        [Display(Name = "Вид клише:")]
        public int? VidKlishe { get; set; }

        [Display(Name = "Площадь, кв.см:")]
         public double? Ploshad { get; set; }

        [Display(Name = "клише уже изготовлено")]
         public bool KlisheExists { get; set; }


        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
               rr.param12 = Material;
            rr.param13 = Ploshad;
            rr.param14 = KlisheExists;
            rr.param21 = VidKlishe;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Tisnenie()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Vid = li.param11,
               Material= li.param12,
                Ploshad = li.param13,
                KlisheExists = li.param14,
                VidKlishe=li.param21

        };
        }

        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();

            kvotaEntities db = new kvotaEntities();

            if (Material != null && Tiraz != null && (KlisheExists || Ploshad!=null && VidKlishe!=null))
                foreach (var firma in db.Firma)
                {
                    PriceDto cena;
                    if (TryGetPrice(firma.id, Tiraz, Material, out cena) == false) continue;

                    var line = new CalcLine() { FirmaId = firma.id };
                    line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;

                    if (!KlisheExists)
                    {
                        decimal cenaKlishe = 0;
                        if (TryGetSingleParam(VidKlishe.Value, firma.id, out cenaKlishe))
                        {
                            line.Cena += cenaKlishe;
                        }
                        else continue;
                    }

                    ret.Add(line);
                }

            return ret;


        }

        private decimal GetCenaKlishe(Postavs firma, double ploshad)
        {
            /*if (firma==Postavs.РРЦ_1_5 || firma == Postavs.ААА)
            {
                if (ploshad < 31) return 850;
                if (ploshad < 41) return 1000;
                if (ploshad <= 50) return 1300;
                return 1300+ Math.Ceiling((decimal)ploshad -50)*18;
            }
            if (firma == Postavs.АртСувенир)
             return 1000;*/
            if (firma == Postavs.Плановая_СС || firma == Postavs.РРЦ_1_5)
            {
                var cena = 0m;
                if (ploshad < 31) cena= TryGetSingleParam(804);
               else cena=   TryGetSingleParam(805);

                if (firma == Postavs.РРЦ_1_5)
                    cena *= 1.5m;
                return cena;
            }
            return 0;
        }

        public Tisnenie(): base(TipProds.Tisnenie, "EditТиснение")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 401 select pp), "id", "tip");
            ViewData["params3"] = new SelectList((from pp in db.Category where pp.id==696 || pp.id == 698  select pp), "id", "tip");
            
        }
    }

}