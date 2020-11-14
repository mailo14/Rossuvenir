using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Lenta : ItemBase
    {        
        public override string Srok
        {
            get
            {
                var catId = Vid;
                var descr = "";
                if (catId == 532)
                    descr = "в наличии на складе";
                else if (catId == 533)
                    descr = "4–7 календарных дней(для лент 25 мм: 14–17 календарных дней)";
                else if (catId == 534)
                    descr = "14–17 календарных дней";
                else if (catId == 535)
                    descr = "8–12 календарных дней";
                else if (catId == 536)
                    descr = "8–12 календарных дней";
                else if (catId == 537)
                    descr = "в наличии на складе";
                return descr+" (+2 рабочих дня на проверку и сборку)";
            }
            set { }
        }

        public override string Description
        {
            get
            {
                var catId = Vid;
                var descr = "";
                if (catId == 532)
                    descr = "Ленты для бейджей изготовлены из качественного материала — нейлон. Мы всегда храним на складе запас готовой продукции, и предлагаем широкую цветовую гамму, среди которой вы без труда найдёте фирменный цвет своей компании. Всё это даёт возможность получить необходимые вам ленты для бейджей в самые кратчайшие сроки. ";
                else if (catId == 533)
                    descr = "Ленты для бейджей с логотипом сублимация — ленты с двусторонней полноцветной печатью. При данном виде нанесения возможен перенос любого изображения, даже с самыми мелкими деталями. Материал ленты изготовлен из самого высококачественного сырья. Цвета получаются яркими и насыщенными, а краска не стирается.";
                else if (catId == 534)
                    descr = "Ленты(ланьярды) для бейджей с логотипом, выполненные с использованием шелкографического метода нанесения изображений и надписей, пользуются большой популярностью и распространены повсеместно.Изображение получается очень отчётливым и точно соответствует корпоративному цвету вашей организации: ведь цвет нанесения смешивается специально для каждого конкретного случая. Cтандартные крепления бесплатны";
                else if (catId == 535)
                    descr = "Жаккардовые ленты для бейджей получаются при сложном тканом сплетении, в результате которого изображение или логотип буквально вплетаются в ленту с помощью специального ткацкого станка. В классическом варианте используется два различных цвета нитей, при сплетении которых на обратной стороне ленты получается инверсия рисунка или логотипа. Cтандартные крепления бесплатны";
                else if (catId == 536)
                    descr = "Разноцветные шнурки для бейджей очень легко подобрать под фирменный цвет и тематику мероприятия. Мы всегда храним на складе запас готовой продукции, это даёт возможность получить шнурки для бейджей, необходимые для вашего мероприятия в самые кратчайшие сроки. Cтандартные крепления бесплатны. Шнурки для бейджей всегда в наличии на наших складах в большом количестве";
                else if (catId == 537)
                    descr = "Отличительной особенностью таких ретракторов является большой и прочный карабин с металлической основой. Элегантные и прочные — такие держатели для пропуска по праву считаются премиальными. * — при тиражах 100 шт. и более.";

                return descr;
            }
            set { }
        }

        public override string PicturePath
        {
            get
            {
                return GetExistImageUrl(this.GetType(), Vid, RazmerForma);
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

                if (Vid != null && Vid != 532)
                {
                    ViewData["RazmerFormaDivStyle"] = "display:block;";
                    ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
                }
                else
                {
                    ViewData["RazmerFormaDivStyle"] = "display:none;";
                    ViewData["params2"] = empty;
                }


                    if (Vid != 532 && Vid != 537)
                {
                    ViewData["PremiumDivStyle"] = "display:block;";
                    ViewData["ZamokObrivDivStyle"] = "display:block;";
                    ViewData["MezurZamokDivStyle"] = "display:block;";
                }
                else
                {
                    ViewData["PremiumDivStyle"] = "display:none;";
                    ViewData["ZamokObrivDivStyle"] = "display:none;";
                    ViewData["MezurZamokDivStyle"] = "display:none;";
                }
                if (Vid != 537)
                    ViewData["KarmashekDivStyle"] = "display:block;";
                else 
                    ViewData["KarmashekDivStyle"] = "display:none;";

                if (Vid == 533)
                    ViewData["LogoObrDivStyle"] = "display:block;";
                else 
                    ViewData["LogoObrDivStyle"] = "display:none;";
                if (Vid == 537)
                {
                    ViewData["LogoObyomDivStyle"] = "display:block;";
                    ViewData["KrepKolcoDivStyle"] = "display:block;";
                }
                else
                {
                    ViewData["LogoObyomDivStyle"] = "display:none;";
                    ViewData["KrepKolcoDivStyle"] = "display:none;";
                }


                if (value == null)
                    ViewData["DescrDivStyle"] = "display:none;";              
            }
        }
        
        [Display(Name = "Размер/форма:")]
        public int? RazmerForma { get; set; } 

        [Display(Name = "премиум крепление")]
         public bool Premium { get; set; }

        [Display(Name = "замок обрыва")]
         public bool ZamokObriv { get; set; }

        [Display(Name = "межуровневый замок")]
         public bool MezurZamok { get; set; }

        [Display(Name = "кармашек для бейджа")]
        public bool Karmashek { get; set; }

        [Display(Name = "печать логотипа на обратной стороне")]
        public bool LogoObr { get; set; }

        [Display(Name = "нанести логотип (объёмная наклейка)*")]
        public bool LogoObyom { get; set; }

        [Display(Name = "поменять крепление на кольцо")]
        public bool KrepKolco { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
            rr.param12 = RazmerForma;

            rr.param14 = Premium;
            rr.param15 = ZamokObriv;

            rr.param24 = MezurZamok;
            rr.param25 = Karmashek;

            rr.param34 = LogoObr;
            rr.param35 = LogoObyom;

            rr.param44 = KrepKolco;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Lenta()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,

                Vid=li.param11,
                RazmerForma= li.param12 ,

             Premium=li.param14 ,
            ZamokObriv=li.param15 , 

             MezurZamok=li.param24 ,
             Karmashek=li.param25 ,

            LogoObr=li.param34 , 
             LogoObyom=li.param35 ,

             KrepKolco=li.param44 ,
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
                    if (Vid != 532 && Vid == null || Tiraz == null) continue;

                    decimal cena;

                    if (TryGetPrice(i, Tiraz, (Vid == 532) ? 532 : RazmerForma, out cena) == false) continue;

                    decimal dops = 0, allTirazDops = 0;
                    switch (Vid)
                    {
                        case 532:
                            if (Karmashek) dops += TryGetSingleParam(924);
                            break;
                        case 533:
                            if (Premium) dops += TryGetSingleParam(809);
                            if (ZamokObriv) dops += TryGetSingleParam(810);
                            if (MezurZamok) dops += TryGetSingleParam(811);
                            if (Karmashek) dops += TryGetSingleParam(812);
                            if (LogoObr) dops += TryGetSingleParam(813);
                            break;
                        case 534:
                            if (Premium) dops += TryGetSingleParam(814);
                            if (ZamokObriv) dops += TryGetSingleParam(815);
                            if (MezurZamok) dops += TryGetSingleParam(816);
                            if (Karmashek) dops += TryGetSingleParam(817);
                            break;
                        case 535:
                            if (Premium) dops += TryGetSingleParam(818);
                            if (ZamokObriv) dops += TryGetSingleParam(819);
                            if (MezurZamok) dops += TryGetSingleParam(820);
                            if (Karmashek) dops += TryGetSingleParam(821);
                            break;
                        case 536:
                            if (Premium) dops += TryGetSingleParam(822);
                            if (ZamokObriv) dops += TryGetSingleParam(823);
                            if (MezurZamok) dops += TryGetSingleParam(824);
                            if (Karmashek) dops += TryGetSingleParam(825);
                            break;
                        case 537:
                            if (LogoObyom) dops += TryGetSingleParam(826);
                            if (KrepKolco) dops += TryGetSingleParam(827);
                            break;
                    }
                    cena += dops;
                    line.Cena = cena * (decimal)Tiraz.Value + allTirazDops;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena = 1.5m * pCena;

            return ret;
        }

        public Lenta(): base(TipProds.Lenta, "EditLenta")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 417 select pp), "id", "tip");
        }
    }

}