using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Futbolka : ItemBase
    {
        public override string Srok { get; set; } = "от 1 часа до 3-5 рабочих дней" + SrokPripiska;
        public override string Description { get; set; } = "Цены указаны до А4 формата, без учета стоимости носителя. При нанесении изображения большого формата, цена рассчитывается индивидуально. Тираж менее 20 шт.и более 1000 шт.рассчитывается индивидуально· В стоимость не включена цена носителя.";

            int? _Osnova = null;
        [Display(Name = "Вид/основа:")]
        public int? Osnova
        {
            get { return _Osnova; }
            set
            {
                if (value != _Osnova)
                {
                    _Osnova = value;
                }
                var empty = new SelectList(new List<Category>(), "id", "tip");

                kvotaEntities db = new kvotaEntities();
                if (value == null)
                {
                    ViewData["params2"] = empty;
                }
                else
                {
                    ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
                }
            }
        }

        [Display(Name = "Количество цветов:")]
         public int? Tcvet { get; set; }

        [Display(Name = "Количество доп.цветов:")]
         public double? DopTcveta { get; set; }        

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Osnova;
            rr.param12 = Tcvet;
            rr.param13 = DopTcveta;
            return rr;
        }
        public static ItemBase CreateItem(ListItem li)
        {
            return new Futbolka() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Osnova = li.param11,
                Tcvet =li.param12,                 DopTcveta = li.param13
            };
        }
      
public override List<CalcLine> Calc()

        {

            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (i == Postavs.Плановая_СС)
                {
                    if (Tiraz == null || Osnova == null || Tcvet == null) continue;
                    if (Tiraz < 20 || Tiraz >= 1000) { InnerMessageIds.Add(InnerMessages.AskBetterPrice); continue; }
                    kvotaEntities db = new kvotaEntities();
                    decimal cena;

                    if (TryGetPrice(i, Tiraz, Tcvet, out cena) == false) continue;
                    if (DopTcveta != null)
                    {
                        int param = (Osnova == 385) ? 392 : 398;
                        decimal dopCena;
                        TryGetPrice(i, Tiraz, param, out dopCena);
                        cena += dopCena * (decimal)DopTcveta.Value;
                    }

                    line.Cena = cena * (decimal)Tiraz.Value;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*pCena;
            return ret;

        }


        public Futbolka():base( TipProds.Futbolka, "EditFutbolka")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 437 select pp), "id", "tip");

            ViewData["params2"] = new SelectList(new[] {
                new { id = 0, tip= "флажный шелк Премиум 80гр" },
                new { id = 1, tip= "флажная сетка 110гр" },
                new { id = 2, tip= "атлас, габардин" },
            }, "id", "tip");
        }
    }

}