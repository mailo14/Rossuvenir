using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Svetootrazatel : ItemBase
    {
        public override string Srok { get; set; }= "2–4 рабочих дня";

        public override string Description
        {
            get
            {
                var catId = Vid;
                var descr = "";
                switch (catId)
                {
                    case 566:
                        descr = "Светоотражающие наклейки совсем лёгкие и очень просты в применении. Они крепятся к поверхностям различной природы за счёт клейкой основы, которая надёжно фиксирует светоотражающий элемент на коробках, тетрадных обложках, велосипеде... Яркие и примечательные наклейки, днём не отличимые от интересного аксессуара, обеспечат безопасность на дорогах в тёмное время суток. Особенно такие наклейки нравятся детям!"
               + "Фасование наклеек: на подложке, по 8 шт."
               + "Возможно изготовление светоотражающих наклеек различной формы и размера под заказ.";
                        break;
                    case 567:
                        descr = "Светоотражающие слэп браслеты огибают запястье по хлопку и универсальны для любого размера руки. Они прочны и удобны для пешеходов, а поверхность светоотражателя без труда очищается влажной губкой. Наша компания занимается изготовлением качественных фликеров с нанесением, которые с удовольствием будет носить каждый."
                            +"Возможно изготовление слеп браслетов различной длины под заказ.";
                        break;
                    case 568:
                        descr = "Светоотражающие брелоки выполнены из двусторонней световозвращающей плёнки, с возможностью нанесения логотипа с двух сторон. Брелки надёжно крепятся цепочкой к сумкам, ранцам и рюкзакам, поэтому они всегда остаются на виду и в подвижном состоянии."
                            +"Возможно изготовление брелоков любой формы и размеров под заказ.";
                        break;
                    case 569:
                        descr = "Компактные светоотражающие значки надежно крепятся на любой вид одежды с помощью металлической булавки. Яркий дизайн таких значков привлекает к себе внимание и повышает характеристики видимости пешехода в темное время суток. Приобретая светоотражатели с нанесением вашей символики, вы не просто делаете подарок, но еще и выражаете свою заботу о людях.";
                        break;
                }
                return descr;
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
                if (value == 568 || value == 569)
                {
                    ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
                    ViewData["RazmerFormaDivStyle"] = "display:block;";                    
                }
                else
                {
                    ViewData["params2"] = empty;
                    ViewData["RazmerFormaDivStyle"] = "display:none;";
                }

                if (value == 566)
                {
                    ViewData["UpakovatDivStyle"] = "display:none;";
                }
                else
                {
                    ViewData["UpakovatDivStyle"] = "display:block;";
                }
                if (value == null)
                    ViewData["DescrDivStyle"] = "display:none;";
                    
                /*switch (value)
                {
                    case 556:
                        Description = "Новинка на российском рынке, практичный и необычный аксессуар, слэп -часы давно пользуются признанием за рубежом.Обладатели вещицы отмечают удобство использования: довольно одного лёгкого удара о запястье, — и часы обернулись вокруг руки точно по размеру. Замечательной идеей будет преподнесение такого практичного подарка вашим сотрудникам или партнёрам; наверняка вы вскоре заметите их пунктуальность!"
                            + Environment.NewLine + "**— на каждом изделии своё изображение или имя."
                            + Environment.NewLine + "Гарантия на механизм(без батарейки): 6 месяцев. Страна производства: Китай.Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                        break;
                    case 557:
                        Description = "Необычная и интересная задумка — использовать slap-браслет вместо стандартного ремешка как в обычных наручных часах. Такие часы выглядят очень современно, а пользоваться ими проще простого — достаточно легкого удара о запястье, чтобы часы оказались на руке. Эта модель особенно примечательна тем, что предусматривает возможность комбинирования различных цветов — продуманные гармоничные сочетания цветов ремешка и обрамления циферблата радуют всегда радуют глаз."
                    + Environment.NewLine + "**— на каждом изделии своё изображение или имя."
                    + Environment.NewLine + "Гарантия на механизм(без батарейки): 6 месяцев. Страна производства: Китай.";
                        break;
                    case 558:
                        Description = "Три самостоятельных продукта в одном изделии — вот что действительно здорово! Стильный и забавный слэп браслет, часы и даже флеш-накопитель, ловко спрятанный в конструкцию слэп ремешка, сочетаются в одной вещице. Такой сувенир понравится, кому угодно, коллеги и клиенты непременно оценят вашу изобретательность и творческий подход. Эта модель — уникальная разработка нашей команды. По умолчанию изделия с объёмом памяти 8 ГБ."
                            + Environment.NewLine + "**— на каждом изделии своё изображение или имя."
                            + Environment.NewLine + "Гарантия на механизм(без батарейки): 6 месяцев. Страна производства: Китай.";
                        break;
                }*/
            }
        }

        public override string PicturePath
        {
            get
            {
                return GetExistImageUrl(this.GetType(), Vid, RazmerForma);
            }
            set { }
        }

        [Display(Name = "Размер/форма:")]
        public int? RazmerForma { get; set; }

        [Display(Name = "нанести логотип в один цвет")]
         public bool OneColor { get; set; }

        [Display(Name = "Количество доп.цветов нанесения:")]
         public double? DopTcveta { get; set; }

        [Display(Name = "выполнить полноцветное нанесение")]
         public bool Polnocvet { get; set; }

        [Display(Name = "упаковать каждый браслет в отдельный пакетик")]
         public bool Upakovat { get; set; }


        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
 rr.param12 = RazmerForma;
            rr.param13 = DopTcveta;

            rr.param14 = OneColor;
            rr.param15 = Polnocvet;

            rr.param24 = Upakovat;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Svetootrazatel()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,

                Vid=li.param11,
  RazmerForma = li.param12,
            DopTcveta= li.param13,

            OneColor= li.param14 ,
                Polnocvet=li.param15 ,

             Upakovat= li.param24 ,                
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
                    if (Vid== null || Tiraz == null ||  ((Vid== 568 || Vid== 569) && RazmerForma==null )) continue;

                decimal cena;

                if (TryGetPrice(i, Tiraz, (Vid == 568 || Vid == 569)?RazmerForma: Vid, out cena) == false) continue;
                
                decimal dops = 0;
                if (OneColor) dops += (Vid == 567) ? 7 : 5;
                if (DopTcveta!=null) dops += (decimal)DopTcveta.Value*( (Vid == 567) ? 7 : 5);
                if (Polnocvet) dops += 15;
                if (Upakovat && Vid!= 566) dops += 2;

                cena += dops;
                line.Cena = cena * (decimal)Tiraz.Value;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena = 1.5m * pCena;

            return ret;
        }

        public Svetootrazatel(): base(TipProds.Svetootrazatel, "EditSvetootrazatel")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 439 select pp), "id", "tip");
        }
    }

}