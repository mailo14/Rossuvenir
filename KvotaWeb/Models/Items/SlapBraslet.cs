using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class SlapBraslet : ItemBase
    {        
        public override string Srok
        {
            get
            {
                var catId = Vid;
                var descr = "";
                switch (catId)
                {
                    case 553:
                    case 554:
                        descr = "2-4 рабочих дня";
                        break;
                    case 555:
                        descr = "12–14 календарных дней";
                        break;
                }
                return descr;
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
                    case 553:
                        descr = "Светоотражающий слэп браслет с логотипом — это не только модный, но ещё и практичный аксессуар. Выполняя функцию светоотражателя, такой браслет обеспечивает видимость в тёмное время суток. Мы можем изготовить светоотражающие (самофиксирующиеся) slap-браслеты необходимого вам размерана заказ. ";
                        break;
                    case 554:
                        descr = "Силиконовый слеп браслет с логотипом — броский и запоминающийся промо продукт, а также великолепный сувенир на любом мероприятии. Главное преимущество таких браслетов, конечно же, универсальность размера, они великолепно подходят как взрослым, так и детям.";
                        break;
                    case 555:
                        descr = "Тканевые слеп браслеты — абсолютно новый, восхитительный промо продукт, разработанный нашей кампанией. В данном изделии стальная пластина с обеих сторон покрывается тканью сатин с удивительно гладкой и шелковистой текстурой.А сублимационная печать позволяет нанести на тканевые слэп браслеты абсолютно любое графическое изображение. Нанесение и упаковка включены в стоимость изготовления тиража. ";
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

                if (value == 553)
                {
                    ViewData["UpakovatDivStyle"] = "display:none;";
                }
                else
                {
                    ViewData["UpakovatDivStyle"] = "display:block;";
                }
                if (value == 553 || value == 554)
                {
                    ViewData["OneColorDiv"] = "display:block;";
                    ViewData["DopTcvetaDiv"] = "display:block;";
                    ViewData["UpakovatDiv"] = "display:block;";
                }
                else
                {
                    ViewData["OneColorDiv"] = "display:none;";
                    ViewData["DopTcvetaDiv"] = "display:none;";
                    ViewData["UpakovatDiv"] = "display:none;";
                }
                if (value == 553)
                {
                    ViewData["PolnocvetDiv"] = "display:block;";
                }
                else
                {
                    ViewData["PolnocvetDiv"] = "display:none;";
                }
                if (value == 554)
                {
                    ViewData["PechatFormaDiv"] = "display:block;";
                    ViewData["SmenaTcvetaDiv"] = "display:block;";
                }
                else
                {
                    ViewData["PechatFormaDiv"] = "display:none;";
                    ViewData["SmenaTcvetaDiv"] = "display:none;";
                }

                if (value == null)
                    ViewData["DescrDivStyle"] = "display:none;";              
            }
        }

        public override string PicturePath
        {
            get
            {
                return GetExistImageUrl(this.GetType(), Vid, null);
            }
            set { }
        }

        [Display(Name = "нанести логотип")]
         public bool OneColor { get; set; }

        [Display(Name = "Количество доп.цветов нанесения:")]
         public double? DopTcveta { get; set; }

        [Display(Name = "выполнить полноцветное нанесение")]
         public bool Polnocvet { get; set; }

        [Display(Name = "упаковать каждый браслет в отдельный пакетик")]
         public bool Upakovat { get; set; }

        [Display(Name = "печатная форма")]
        public bool PechatForma { get; set; }

        [Display(Name = "смена цвета")]
        public bool SmenaTcveta { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
            rr.param13 = DopTcveta;

            rr.param14 = OneColor;
            rr.param15 = Polnocvet;

            rr.param24 = Upakovat;

            rr.param25 = PechatForma;
            rr.param34 = SmenaTcveta;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new SlapBraslet()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,

                Vid=li.param11,
            DopTcveta= li.param13,

            OneColor= li.param14 ,
                Polnocvet=li.param15 ,

             Upakovat= li.param24 ,
                 PechatForma= li.param25,
                SmenaTcveta=li.param34  ,
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
                    if (Vid== null || Tiraz == null) continue;

                decimal cena;

                if (TryGetPrice(i, Tiraz,  Vid, out cena) == false) continue;
                
                decimal dops = 0, allTirazDops = 0;
                if ((Vid==553 || Vid == 554) &&  OneColor) dops +=  7 ;
                if ((Vid==553 || Vid == 554) && DopTcveta!=null) dops += (decimal)DopTcveta.Value*7;
                if (Vid == 553 && Polnocvet) dops += 15;
                    if ((Vid == 553 || Vid == 554) && Upakovat) dops += 2;
                    if (Vid == 554)
                    {
                        if (PechatForma) allTirazDops += 300;
                        if (SmenaTcveta) allTirazDops += 200;
                    }

                        cena += dops;
                    line.Cena = cena * (decimal)Tiraz.Value + allTirazDops;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena = 1.5m * pCena;

            return ret;
        }

        public SlapBraslet(): base(TipProds.SlapBraslet, "EditSlapBraslet")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 421 select pp), "id", "tip");
        }
    }

}