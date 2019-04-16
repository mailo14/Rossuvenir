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

                if (value == 558)
                {
                    ViewData["Chip16DivStyle"] = "display:block;";
                }
                else
                {
                    ViewData["Chip16DivStyle"] = "display:none;";
                }
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


        [Display(Name = "Размер/форма:")]
        public int? KolichestvoTcvetov { get; set; }

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
 rr.param12 = KolichestvoTcvetov;
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
  KolichestvoTcvetov = li.param12,
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
                if (Vid== null || Tiraz == null ) continue;

                decimal cena;

                if (TryGetPrice(i, Tiraz, Vid, out cena) == false) continue;
                
                decimal nanecenie = 0;
                if (Shelkografiya)
                {
                    if (Tiraz >= 100)
                        nanecenie += 10;
                    else nanecenie += 1000.0m / (decimal)Tiraz.Value;
                }
                    if (Lazer) nanecenie += 20;
                if (Dvustoronnii) nanecenie *= 2;

                if (Personal) nanecenie += 40;
                if (Vid==558 && Chip16) nanecenie += 25;


                 cena += nanecenie;
                line.Cena = cena * (decimal)Tiraz.Value;
            }
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