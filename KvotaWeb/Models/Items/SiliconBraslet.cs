using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class SiliconBraslet : ItemBase
    {        
        public override string Srok
        {
            get
            {
                var catId = Vid;
                var descr = "";
                if (catId == 578)
                {
                    if (RazmerForma==592 || RazmerForma == 593)
                    descr = "2–4 рабочих дня";
                    else descr = "12–14 календарных дней";
                }
                else if (catId == 580 || catId == 581 || catId == 582 || catId == 585 || catId == 587)
                    descr = "2–4 рабочих дня";
                else if (catId == 579 || catId == 583 || catId == 584)
                    descr = "12–14 календарных дней";
                else if (catId == 586 || catId == 588)
                    descr = "в наличии на складе";
                else if (catId == 589)
                    descr = "5–7 календарных дней";
                else if (catId == 590 || catId == 591)
                    descr = "21–25 дней";
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
                if (catId == 578)
                    descr = "Нанесение логотипа превращает силиконовые браслеты в крутой, эффективный промо продукт. Браслеты с данным типом нанесения мы можем изготовить в самые кратчайшие сроки. Логотип наносится по всей окружности браслета специальной прорезиненной краской. ";
                else if (catId == 579)
                    descr = "Отличительной особенностью таких браслетов является сложная технология нанесения, при которой вдавленный/выпуклый логотип вручную прокрашивается специальной краской. Такие браслеты обладают высоким качеством и индивидуальностью. Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                else if (catId == 580)
                    descr = "Уникальное предложение нашей компании — единственного отечественного производителя силиконовых браслетов. Комбинируйте любые два, три или даже четыре цвета, создавая по-настоящему особенное изделие! Огромный потенциал для творчества и индивидуализации. Позвольте себе максимум стиля и выразительности, решайтесь на беспрецедентность! Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                else if (catId == 581)
                    descr = "Двухцветные силиконовые браслеты — это уникальное предложение от нашей компании. Браслеты такого типа смотрятся невероятно эффектно и стильно, и по праву могут считаться премиальными. Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                else if (catId == 582)
                    descr = "Идентификационные браслеты передают значимую информацию о своём обладателе (имя, адрес, номера телефонов и т.д.) Они производятся из качественных, долговечных и безопасных материалов и предназначены для постоянного ношения. Основная часть браслета выполнена из 100% экологически чистого силикона, а металлическая вставка выполнена из нержавеющей стали (316L). Нанесение осуществляется на металлическую часть методом гравировки. Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                else if (catId == 583)
                    descr = "Отличительной особенностью таких браслетов является сложная технология нанесения, при которой выпуклый логотип вручную прокрашивается специальной краской.Такие браслеты обладают высоким качеством и индивидуальностью.Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.E156";
                else if (catId == 584)
                    descr = "Для данного вида нанесения мы изготавливаем специальную форму и отливаем по ней браслеты. Силиконовые браслеты на руку с таким видом нанесения невероятно практичны. Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                else if (catId == 585)
                    descr = "Эта модель силиконового браслета с логотипом предусматривает также область со специальным покрытием, где вы можете самостоятельно сделать любую нужную надпись. Возможности применения такого изделия не ограничены, — они станут отличным решением для проведения всевозможных конкурсов, лотерей и других массовых мероприятий. Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                else if (catId == 586)
                    descr = "Силиконовые браслеты без нанесения различных цветов всегда в наличии на наших складах в большом количестве. ";
                else if (catId == 587)
                    descr = "";
                else if (catId == 588)
                    descr = "Эта модель силиконового браслета с логотипом предусматривает также область со специальным покрытием, где вы можете самостоятельно сделать любую нужную надпись. Такое изделие — находка для фитнесс-клубов, так как позволяет вам подписать номер шкафчика с личными вещами посетителей. ";
                else if (catId == 589)
                    descr = "Браслеты для ключей изготовлены из экологически чистого силикона, а специальная, прорезиненная краска исключает возможность стирания логотипа или нумерации. Информацию по креплениям для ключей уточняйте, пожалуйста, у менеджера.";
                else if (catId == 590)
                    descr = "Силиконовые R-fid браслеты — современный и удобный способ идентификации, как массовых, так и персональных пропускных систем. Корпус браслетов изготовлен из высококачественного силикона и надежно защищает чип от воздействия воды и температуры. Стоимость изделия с чипом EM-marine. Мы можем изготовить и другие чипы: Mifare, Mifare 1к, Mifare 4к. Бесплатная доставка: по Москве(в пределах МКАД) при заказе от 5000 ₽; до терминалов ТК в областных центрах России при заказе от 10000 ₽.";
                else if (catId == 591)
                    descr = "Rfid - браслеты — удобный способ идентификации посетителей, отлично подходит для массовых и персональных пропускных систем.Эта модель выглядит как украшение, использовать которое очень удобно благодаря наличию металлической застёжки. Аксессуар выполнен их высококачественного бархатистого силикона и прекрасно защищает чип от воздействия неблагоприятных факторов среды и механических повреждений. Стоимость изделия с чипом EM-marine. Мы можем изготовить и другие чипы: Mifare, Mifare 1к, Mifare 4к.";

                return descr;
            }
            set { }
        }

        public override string PicturePath
        {
            get
            {
                return GetExistImageUrl(this.GetType(),Vid,RazmerForma);
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

                if (Vid != null && new int[] { 578, 579, 582, 583, 584 }.Contains(Vid.Value) )
                {
                    ViewData["RazmerFormaDivStyle"] = "display:block;";
                    ViewData["params2"] = new SelectList((from pp in db.Category where pp.parentId == value select pp), "id", "tip");
                }
                else
                {
                    ViewData["RazmerFormaDivStyle"] = "display:none;";
                    ViewData["params2"] = empty;
                }


                    if (Vid != null && new int[] { 578, 579, 580, 581, 583, 585 }.Contains(Vid.Value))
                    ViewData["DopTcvetaDivStyle"] = "display:block;";
                else ViewData["DopTcvetaDivStyle"] = "display:none;";

                if (Vid != null && new int[] { 578, 580, 581, 585, 586, 588 }.Contains(Vid.Value))
                    ViewData["AddKolcoDivStyle"] = "display:block;";
                else ViewData["AddKolcoDivStyle"] = "display:none;";

                if (Vid != null && new int[] { 578, 579, 582, 583, 584 }.Contains(Vid.Value))
                    ViewData["SvetonakopDivStyle"] = "display:block;";
                else ViewData["SvetonakopDivStyle"] = "display:none;";

                if (Vid != null && new int[] { 578, 579, 580, 581, 583, 584, 585, 586, 587, 588, 589 }.Contains(Vid.Value))
                    ViewData["UpakovatDivStyle"] = "display:block;";
                else ViewData["UpakovatDivStyle"] = "display:none;";

                if (Vid != null  && new int[] { 578, 579, 583, 584 }.Contains(Vid.Value))
                    ViewData["SvirlDivStyle"] = "display:block;";
                else ViewData["SvirlDivStyle"] = "display:none;";

                if (Vid != null  && new int[] { 578, 580, 581, 585 }.Contains(Vid.Value))
                    ViewData["SmenaTcvetaDivStyle"] = "display:block;";
                else ViewData["SmenaTcvetaDivStyle"] = "display:none;";
                if (Vid != null  && new int[] { 578, 579, 583, 584 }.Contains(Vid.Value))
                    ViewData["SmenaRazmeraDivStyle"] = "display:block;";
                else ViewData["SmenaRazmeraDivStyle"] = "display:none;";

                if (Vid != null  && new int[] { 582, 590, 591 }.Contains(Vid.Value))
                    ViewData["PersonalDivStyle"] = "display:block;";
                else ViewData["PersonalDivStyle"] = "display:none;";

                if (Vid != null && new int[] { 588, 589, 590, 591 }.Contains(Vid.Value))
                    ViewData["LogoDivStyle"] = "display:block;";
                else ViewData["LogoDivStyle"] = "display:none;";

                if (Vid != null && new int[] { 589, 590, 591 }.Contains(Vid.Value))
                    ViewData["AddNumDivStyle"] = "display:block;";
                else ViewData["AddNumDivStyle"] = "display:none;";

                if (Vid == 589)
                {
                    ViewData["AddKrepDivStyle"] = "display:block;";
                    ViewData["AddZastezDivStyle"] = "display:block;";
                }
                else
                {
                    ViewData["AddKrepDivStyle"] = "display:none;";
                    ViewData["AddZastezDivStyle"] = "display:none;";
                }

                if (value == null)
                    ViewData["DescrDivStyle"] = "display:none;";              
            }
        }
        [Display(Name = "Размер:")]
        public int? RazmerForma { get; set; }

        [Display(Name = "Количество доп.цветов нанесения:")]
        public double? DopTcveta { get; set; }

        [Display(Name = "добавить колечко на браслет")]
         public bool AddKolco { get; set; }


        [Display(Name = "сделать браслет светонакопительным")]
        public bool Svetonakop { get; set; }

        [Display(Name = "упаковать каждый браслет в отдельный пакетик")]
        public bool Upakovat { get; set; }

        [Display(Name = "сегментировать браслет / «cвирл»")]
         public bool Svirl { get; set; }

        [Display(Name = "смена цвета")]
        public bool SmenaTcveta { get; set; }

        [Display(Name = "смена размера")]
        public bool SmenaRazmera { get; set; }

        [Display(Name = "персонализировать")]
         public bool Personal { get; set; }

        [Display(Name = "нанести логотип на браслет")]
        public bool Logo { get; set; }

        [Display(Name = "добавить нумерацию на браслете")]
        public bool AddNum { get; set; }

        [Display(Name = "добавить запасное крепление")]
        public bool AddKrep { get; set; }

        [Display(Name = "добавить запасную застёжку")]
        public bool AddZastez { get; set; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Vid;
            rr.param12 = RazmerForma;
            rr.param13 = DopTcveta;

            rr.param14 = AddKolco;
            rr.param15 = Svetonakop;

            rr.param24 = Upakovat;
            rr.param25 = Svirl;

            rr.param34 = SmenaTcveta;
            rr.param35 = SmenaRazmera;

            rr.param44 = Personal;
            rr.param45 = Logo;

            rr.param54 = AddNum;
            rr.param55 = AddKrep;

            rr.param64 = AddZastez;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new SiliconBraslet()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,

                Vid = li.param11,
                RazmerForma= li.param12,
                DopTcveta = li.param13,

                AddKolco = li.param14,
                Svetonakop = li.param15,

                Upakovat = li.param24,
                Svirl = li.param25,

                SmenaTcveta = li.param34,
                SmenaRazmera = li.param35,

                Personal = li.param44,
                Logo = li.param45,

                AddNum = li.param54,
                AddKrep = li.param55,

                AddZastez = li.param64,
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
                    if (Vid == null || Tiraz == null 
                        || new int[] { 578, 579, 582, 583, 584 }.Contains(Vid.Value) && RazmerForma==null) continue;

                decimal cena;

                    
                    if (TryGetPrice(i, Tiraz,  (new int[] { 578,579,582,583,584}.Contains(Vid.Value))?RazmerForma:Vid, out cena) == false) continue;

                    decimal dops = 0, allTirazDops = 0;
                    if (DopTcveta != null && new int[] { 578, 579, 580, 581, 583, 585 }.Contains(Vid.Value))
                        dops += (decimal)DopTcveta.Value*4;
                    if (AddKolco && new int[] { 578, 580, 581, 585, 586, 588 }.Contains(Vid.Value))
                        dops += (Vid==588)?2:4;
                    if (Svetonakop && new int[] { 578, 579, 582, 583, 584 }.Contains(Vid.Value))
                        dops += 2;
                    if (Upakovat && new int[] { 578, 579, 580, 581, 583, 584, 585, 586, 587, 588, 589 }.Contains(Vid.Value))
                    {
                        if (Vid == 581) dops += 0;
                        else if (Vid == 588) dops += 4;
                        else dops += 2;
                    }
                    if (Svirl && new int[] { 578, 579, 583, 584}.Contains(Vid.Value))
                        dops += 4;
                    if (SmenaTcveta && new int[] { 578, 580, 581,585}.Contains(Vid.Value))
                        allTirazDops += 200;
                    if (SmenaRazmera && new int[] { 578, 579, 583, 584 }.Contains(Vid.Value))
                        allTirazDops += (Vid==578)?300:1000; 

                    if (Personal && new int[] { 582,590,591}.Contains(Vid.Value))
                        dops += (Vid==582)?40:0;
                    if (Logo && new int[] { 588, 589, 590, 591 }.Contains(Vid.Value))
                    {

                        if (Vid == 588) dops += 4;
                        else if (Vid == 589) dops += 10;
                        else dops += 20;
                    }
                    if (AddNum && new int[] { 589, 590, 591 }.Contains(Vid.Value))
                        dops += 20;
                    if (AddKrep && Vid== 589)
                        dops += 15;
                    if (AddZastez && Vid == 589)
                        dops += 15;

                        cena += dops;
                    line.Cena = cena * (decimal)Tiraz.Value + allTirazDops;
                }
            }
            var pCena = ret.First(pp => pp.Postav == Postavs.Плановая_СС).Cena; if (pCena.HasValue) ret.First(pp => pp.Postav == Postavs.РРЦ_1_5).Cena = 1.5m * pCena;

            return ret;
        }

        public SiliconBraslet(): base(TipProds.SiliconBraslet, "EditSiliconBraslet")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 440 select pp), "id", "tip");
        }
    }

}