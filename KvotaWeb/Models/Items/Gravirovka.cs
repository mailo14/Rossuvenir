using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Gravirovka : ItemBase
    {
        public override string Srok { get; set; } = "от 2-х рабочих дней" + SrokPripiska;
        [Display(Name = "Вид:")]
        public int? Vid { get; set; }

        [Display(Name = "Площадь, кв.см:")]
         public double? Ploshad { get; set; }

        [Display(Name = "изготовление образца")]
         public bool Obrazec { get; set; }

        [Display(Name = "чернение")]
         public bool Chernenie { get; set; }




        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
            rr.param13 = Ploshad;
            rr.param14 = Obrazec;
            rr.param15 = Chernenie;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Gravirovka()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                ParentId = li.parentId,

                Vid = li.param11,
                Ploshad = li.param13,
             Obrazec =li.param14,
             Chernenie= li.param15
        };
        }

        public override List<CalcLine> Calc()
        {
            var ret = new List<CalcLine>();

            kvotaEntities db = new kvotaEntities();
                            decimal nacenk;

            if (Vid != null && Tiraz != null && Ploshad != null)
                foreach (var firma in db.Firma)
                {
                    

                    var line = new CalcLine() { FirmaId = firma.id };

                    if (new int[] { 665, 666, 667 }.Contains(Vid.Value))
                    {
                        PriceDto cena = null;
                        if (new int[] { 665, 666 }.Contains(Vid.Value) && TryGetPrice(firma.id, Tiraz, Vid, out cena))
                        {
                            line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                        }
                        else if (Vid == 667)
                        {
                            if (TryGetPrice(firma.id, Tiraz, Vid, out cena))
                            {
                                line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                            }
                            else if (TryGetSingleParam(683, firma.id, out nacenk))
                            {
                                line.Cena = nacenk * (decimal)Ploshad * (decimal)Tiraz.Value;
                            }
                            else continue;
                        }
                            else continue;

                        if (Ploshad > 5)
                        {
                            var diff = (int)Ploshad - 5;
                            PriceDto cenaNacenk;
                            int paramId;
                            if (Vid == 665) paramId = 679;
                            else
                            if (Vid == 666) paramId = 681;
                            else paramId = 684;

                            if (TryGetSingleParam(paramId, firma.id, out nacenk))
                            {
                                line.Cena += nacenk * diff * (decimal)Tiraz.Value;
                            }
                            else
                            if (TryGetPrice(firma.id, Tiraz, 745, out cenaNacenk) == false)
                            {
                                line.Cena += cenaNacenk.Cena * diff * (decimal)Tiraz.Value;

                                if (TryGetSingleParam(686, firma.id, out nacenk))
                                {
                                    line.Cena += nacenk;
                                }
                                else continue;
                            }
                            else continue;
                        }
                    }
                    else if (Vid.Value == 669)
                    {
                        PriceDto cena = null;
                        if (TryGetPrice(firma.id, Tiraz, Vid, out cena))
                        {
                            line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                        }
                        else if (TryGetSingleParam(688, firma.id, out nacenk))
                        {
                            line.Cena = nacenk * (decimal)Ploshad * (decimal)Tiraz.Value;
                        }
                        else continue;
                    }
                    else if (Vid.Value == 670)
                    {
                        PriceDto cena = null;
                        int paramId = 0;
                        if (Ploshad < 10) paramId = 673;
                        else if (Ploshad < 20) paramId = 674;
                        else if (Ploshad < 50) paramId = 675;
                        else if (Ploshad < 100) paramId = 676;
                        else  paramId = 677;

                        if (TryGetPrice(firma.id, Tiraz, paramId, out cena))
                        {
                            line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                        }
                        else continue;
                    }
                    else
                    {
                        PriceDto cena = null;

                        if (TryGetPrice(firma.id, Tiraz, Vid, out cena))
                        {
                            line.Cena = cena.isAllTiraz ? cena.Cena : cena.Cena * (decimal)Tiraz.Value;
                            line.Cena *= (decimal)Ploshad;
                        }
                        else continue;
                    }

                    if (Obrazec)
                        {
                            int paramId;
                            if (Vid == 665) paramId = 678;
                            else
                            if (Vid == 666) paramId = 680;
                            else
                            if (Vid == 667)  paramId = 682;
                            else
                            if (Vid == 669)  paramId = 687;
                            else
                            //if (Vid == 670)
                            paramId = 691;
                            if (TryGetSingleParam(paramId, firma.id, out nacenk))
                            {
                                line.Cena += nacenk ;
                            }
                            else continue;
                        }
                        if (Chernenie)
                    {
                        if (TryGetSingleParam((Vid==667)?685:690, firma.id, out nacenk))
                        {
                            line.Cena *= (100.0m+nacenk)/100;
                        }
                        else continue;
                    }

                    ret.Add(line);
                }

            return ret;

        }

        private decimal? GetCenaPriladki(Postavs firma, int vid)
        {
            //if (firma == Postavs.РРЦ_1_5) return 450m;
            //if (firma == Postavs.ААА) return 350m;

            if (firma == Postavs.РРЦ_1_5)
            {
                var cena = GetCenaPriladki(Postavs.Плановая_СС, vid);
                if (cena.HasValue)
                    return 1.5m * cena.Value;
                else
                    return null;
            }
            if (firma == Postavs.Плановая_СС)
                return TryGetSingleParam(802);

            return null;
        }

        private decimal? GetCena(Postavs firma, int vid, decimal ploshad)
        {
            /*if (firma == Postavs.РРЦ_1_5)
            {
                if (vid == 66) return 7.5m * ploshad;
                if (vid == 67) return 11m * ploshad;
            }

            if (firma == Postavs.ААА)
            {
                if (vid == 66) return 7m * ploshad;
                if (vid == 67) return 10.5m * ploshad;
            }*/

            if (firma == Postavs.РРЦ_1_5)
            {
                var cena= GetCena(Postavs.Плановая_СС, vid, ploshad);
                if (cena.HasValue) return 1.5m * cena.Value;
                else return null;
            }

            if (firma == Postavs.Плановая_СС)
            {
                if (vid == 66) return TryGetSingleParam(800) * ploshad;
                if (vid == 67) return TryGetSingleParam(801) * ploshad;
            }
            return null;
        }
                 

        public Gravirovka(): base(TipProds.Gravirovka, "EditГравировка")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 411 select pp), "id", "tip");
            
        }
    }

}