using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class KontrBraslet : ItemBase
    {
        public override string Srok
        {
            get
            {
                var catId = Vid;
                var descr = "";
                switch (catId)
                {
                    case 559:
                        descr = "от 30 мин.";
                        break;
                    case 560:
                        descr = "2-4 рабочих дня";
                        break;
                    case 561:
                        descr = "12–14 календарных дней";
                        break;
                }
                return descr + SrokPripiska;
            }
            set { }
        }

        public override string Description
        {
            get
            {
                var catId = Vid;
                var descr = "";
                switch (catId)
                {
                    case 559:
                        descr = "Бумажные контрольные браслеты — одновременно простой и экономный способ контроля посетителей.Одноразовые клубные браслеты — невероятно эффективный и недорогой инструмент для идентификации посетителей на любом мероприятии. Производство: ОАЭ. Эконом-печать выполняется только в чёрном цвете! Цветная печать возможна только при тиражах до 2500 шт. ";
                        break;
                    case 560:
                        descr = "Виниловые контрольные браслеты — одноразовые браслеты категории «премиум». Пластиковые или виниловые контроль браслеты — невероятно эффективный инструмент для идентификации посетителей.";
                        break;
                    case 561:
                        descr = "Браслеты для контроля посетителей из ткани без преувеличения можно назвать полноценным промо-продуктом. Возможность нанесения полноцветного изображения или логотипа превращает эти браслеты в яркий и памятный сувенир для гостей мероприятия, а надежный пластиковый или металлический замок полностью исключает возможность их повторного использования и передачи вторым лицам. Сублимация - материал сатин, вышивка - материал полиэстер+хлопок+нейлон";
                        break;
                }
                return descr;
            }
            set { }
        }

        public override string PicturePath
        {
            get
            {
                return GetExistImageUrl(this.GetType(), Vid, null);
            }
            set { }
        }

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

                if (value == 559 || value == 561)
                {
                    ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
                    ViewData["NanesenieDivStyle"] = "display:block;";                    
                }
                else
                {
                    ViewData["params2"] = empty;
                    ViewData["NanesenieDivStyle"] = "display:none;";
                }

                if (value == 560)
                {
                    ViewData["NanestiLogoDivStyle"] = "display:block;";
                    ViewData["PechatFormaDivStyle"] = "display:block;";
                    ViewData["SmenaTcvetaDivStyle"] = "display:block;";
                }
                else
                {
                    ViewData["NanestiLogoDivStyle"] = "display:none;";
                    ViewData["PechatFormaDivStyle"] = "display:none;";
                    ViewData["SmenaTcvetaDivStyle"] = "display:none;";
                }                    
            }
        }


        [Display(Name = "Вид нанесения:")]
        public int? Nanesenie { get; set; }

        [Display(Name = "нанести логотип")]
         public bool NanestiLogo { get; set; }

        [Display(Name = "печатная форма")]
         public bool PechatForma { get; set; }

        [Display(Name = "смена цвета")]
         public bool SmenaTcveta { get; set; }


        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
 rr.param12 = Nanesenie;

            rr.param14 = NanestiLogo;
            rr.param15 = PechatForma;

            rr.param24 = SmenaTcveta;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new KontrBraslet()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,

                Vid=li.param11,
  Nanesenie = li.param12,

            NanestiLogo= li.param14 ,
                PechatForma=li.param15 ,

             SmenaTcveta= li.param24 ,                
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
                    if (Vid== null || Tiraz == null ||  Vid== 561 && Nanesenie==null ) continue;

                decimal cena;

                if (TryGetPrice(i, Tiraz, Vid == 561?Nanesenie: Vid, out cena) == false) continue;

                    decimal dops = 0, allTirazDops = 0;
                    if (Vid==559 && Nanesenie.HasValue)
                    {
                        if (Nanesenie == 575) dops += TryGetSingleParam(806);
                        else if (Nanesenie == 576) dops += TryGetSingleParam(807);
                        else if (Nanesenie == 577) dops += TryGetSingleParam(808);
                    }
                    if (Vid == 560)
                    {
                        if (NanestiLogo)
                            dops += TryGetSingleParam(919);
                        if (PechatForma) allTirazDops += TryGetSingleParam(920);
                        if (SmenaTcveta) allTirazDops += TryGetSingleParam(921);
                    }

                cena += dops;
                line.Cena = cena * (decimal)Tiraz.Value+ allTirazDops;
            }
        }
        var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena=1.5m*pCena;

            return ret;
        }

        public KontrBraslet(): base(TipProds.KontrBraslet, "EditKontrBraslet")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 438 select pp), "id", "tip");
        }
    }

}