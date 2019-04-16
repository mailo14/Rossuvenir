using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using KvotaWeb.Models;
using KvotaWeb.Models.Items;
using KvotaWeb.ViewModels;

namespace KvotaWeb.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        
        public ActionResult Index()
        {
            kvotaEntities db = new kvotaEntities();
            IEnumerable<Zakaz> zakazi = db.Zakaz;
            ViewBag.zz = zakazi;
            return View();
        }

        public Dictionary<int, int> parentByCat = new Dictionary<int, int>() { { 30, 401 }, { 23, 403 }, { 24, 405 }, { 25, 407 }, { 26, 408 }, { 27, 410 }, { 28, 411 }, { 29, 412 }
            , { 201, 414}, { 205, 415}
           , { 211, 416}, { 212, 417}, { 213, 418}, { 214, 419}, { 215, 420}, { 216, 421}, { 217, 422}, { 218, 163}, { 219, 424}, { 269, 425}
       };

        #region Banner
        public ActionResult EditBanner(int id)
        {
            kvotaEntities db = new kvotaEntities();
            //catComboBox.ItemsSource = MainPage.;
            ViewBag.empty = Enumerable.Empty<SelectListItem>();
            ViewBag.vidi = new SelectList((from pp in db.Category where pp.parentId == 101 select pp),//new { Name = pp.id, Value = pp.tip }));
                 "id", "tip");
            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);
            if (li.bVid != null) ViewBag.mats = new SelectList(db.Category.Where(pp => pp.parentId == li.bVid), "id", "tip");
            else ViewBag.mats = new SelectList(new List<Category>(), "id", "tip");
            if (li.bVid != null) ViewBag.dpis = new SelectList(db.Category.Where(pp => pp.parentId == ((li.bVid == 2) ? 501 : 502)), "id", "tip");
            else ViewBag.dpis = new SelectList(new List<Category>(), "id", "tip");
            //    var nacens = new SelectList({ { "1", 1 }});

            return View(li);
        }
        public JsonResult fillBMats(int id) // its a GET, not a POST
        {
            kvotaEntities db = new kvotaEntities();
            var nullObj = new Category() { tip = "(не выбрано)" };
            List<Category> list = new List<Category>() { nullObj };
            list.AddRange(db.Category.Where(pp => pp.parentId == id));
            var data = list;
            // In reality you will do a database query based on the value of provinceId, but based on the code you have shown
            //var cities = new List<string>() { "City1", "City2", "City3" });
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult fillBdpis(int id) // its a GET, not a POST
        {
            kvotaEntities db = new kvotaEntities();
            var nullObj = new Category() { tip = "(не выбрано)" };
            List<Category> list = new List<Category>() { nullObj };
            if (id == 2) list.AddRange(db.Category.Where(pp => pp.parentId == 501));
            if (id == 3) list.AddRange(db.Category.Where(pp => pp.parentId == 502));
            var data = list;
            // In reality you will do a database query based on the value of provinceId, but based on the code you have shown
            //var cities = new List<string>() { "City1", "City2", "City3" });
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditBanner(ListItem li)
        {
            kvotaEntities db = new kvotaEntities();
            db.Entry(li).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        #endregion

        #region Poligrafiya
        public ActionResult EditPoligrafiya(int id)
        {
            kvotaEntities db = new kvotaEntities();
            ViewBag.razm = new SelectList((from pp in db.Category where pp.parentId == 201 select pp), "id", "tip");
            ViewBag.cvet = new SelectList((from pp in db.Category where pp.parentId == 202 select pp), "id", "tip");
            ViewBag.plotn = new SelectList((from pp in db.Category where pp.parentId == 203 select pp), "id", "tip");
            var lamins = db.Category.Where(pp => pp.parentId == 204).ToList();
            //lamins.Insert(0, null);
            ViewBag.lamin = new SelectList(lamins, "id", "tip");
            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            return View(li);
        }

        [HttpPost]
        public ActionResult EditPoligrafiya(ListItem li)
        {
            kvotaEntities db = new kvotaEntities();
            db.Entry(li).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        #endregion

        #region Нанесение логотипа
        public class Param
        {
            public int? vid;
            public int? param1;
            public int? param2;

            public Param(int? _vid, int? _param1, int? _param2) { vid = _vid; param1 = _param1; param2 = _param2; }
        }
        public ActionResult EditSuvenir(int id)
        {
            kvotaEntities db = new kvotaEntities();
            ViewBag.vidi = new SelectList((from pp in db.Category
                                           where pp.parentId == 301
                                           //  && pp.id != 27//деколь 
                                           select pp), "id", "tip");
            ViewBag.kolvoMest = new SelectList(new[] {
                new { Name = 1, Value ="1"},
                new { Name = 2, Value ="2"},
                new { Name = 3, Value = "3"},
                new { Name = 4, Value = "4"}
    }, "Name", "Value");
            Dictionary<int, string[]> labelByVid = new Dictionary<int, string[]>() {
           /* { 30, new string[]{ "   вид тиснения", null, "   площадь клише, кв.см","   клише уже изготовлено", null } },
            { 23, new string[]{ "   цвет основы", "   количество цветов", null, "   печать на синтетических тканях (сумки, плащевки, зонты)", "   печать более формата А4 или площади 600 кв. см." } },
            { 24 , new string[]{"   основа" , "   количество цветов", null, null, null } },
            { 25, new string[]{ "   основа", "   параметры", null, null, null } },
            { 26 , new string[]{ "   основа", "   цвет основы", null, "   печать на цилиндрической поверхности (кроме ручек)", null } },
            { 27, new string[]{ "   цветность", null, "   площадь логотипа, кв.см", null, null } },
            { 28, new string[]{"   техника" , null, "   общая площадь гравировки, кв.см", null, null } },
            { 29, new string[]{"   пакет", "   цвет", null, null, null } },*/
            { 30, new string[]{ "   материал:", "   формат:", null, "   клише уже изготовлено", "   тиснение с фольгой 1 цв." } },
            { 23, new string[]{ "   цвет основы:", "   количество цветов:", null, "   печать на синтетических тканях (сумки, плащевки, зонты)", "   печать более формата А4 или площади 600 кв. см." } },
            { 24 , new string[]{ "   основа:", "   количество цветов:", null, null, null } },
            { 25, new string[]{ "   основа:", "   размер:", null, null, null } },
            { 26 , new string[]{ "   основа:", "   размер запечатки:", null,null, null } },
            //{ 27, new string[]{ "   цветность", null, "   площадь логотипа, кв.см", null, null } },
            { 28, new string[]{null , null, "   общая площадь гравировки, кв.см:", null, null } },
            { 29, new string[]{ "   пакет:", "   цвет шелкографии:", null, null, null } },
            { 201, new string[]{ "   размер:", null, null, null, null } },
            { 205, new string[]{ "   размер:", null, null, "   печать на цветном", null } },
            };
            ViewData["askBetterPriceDivStyle"] = "display:none;";
            ViewData["labelByVid"] = labelByVid;//Json(labelByVid);


            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };
            List<Category> list;

            List<Param> pars = new List<Param>();
            pars.Add(null);
            pars.Add(new Param(li.vid1, li.param11, li.param12));
            pars.Add(new Param(li.vid2, li.param21, li.param22));
            pars.Add(new Param(li.vid3, li.param31, li.param32));
            pars.Add(new Param(li.vid4, li.param41, li.param42));

            bool existPockets = false;
            for (int i = 1; i <= 4; i++)
            {
                ViewData["paramDivStyle" + i + "2"] = "display:block;";
                ViewData["paramDivStyle" + i + "3"] = "display:block;";
                ViewData["paramBag" + i + "2"] = empty;
                if (pars[i].vid.HasValue && pars[i].vid > 0)
                {
                    int vidId = pars[i].vid.Value;
                    for (int j = 0; j < 5; j++) ViewData["paramLabel" + (i) + (j + 1)] = labelByVid[vidId][j];
                    int parent = parentByCat[vidId];
                    ViewData["paramBag" + i + "1"] = new SelectList((from pp in db.Category where pp.parentId == parent select pp), "id", "tip");
                    if (vidId == 28)//гравировкa
                    {
                        ViewData["paramDivStyle" + i + "1"] = "display:none;";
                        ViewData["paramDivStyle" + i + "2"] = "display:none;";
                        ViewData["paramDivStyle" + i + "3"] = "display:block;";
                        ViewData["paramDivStyle" + i + "4"] = "display:none;";
                        ViewData["paramDivStyle" + i + "5"] = "display:none;";
                    }
                    if (vidId == 29)//пакеты
                    {
                        existPockets = true;
                        list = new List<Category>() { }; list.AddRange(db.Category.Where(pp => pp.parentId == 413));
                        ViewData["paramBag" + i + "2"] = new SelectList(list, "id", "tip");
                        ViewData["paramDivStyle" + i + "3"] = "display:none;";
                    }
                    else
                    {
                        if (!new int[] { 28, 201, 205 }.Contains(vidId))
                        {
                            if (pars[i].param1.HasValue)
                            {
                                int catId = pars[i].param1.Value;
                                if (false)//+vidId == 25 && catId == 164 || vidId == 26 && catId != 172)
                                {
                                    ViewData["paramDivStyle" + i + "2"] = "display:none;";
                                }
                                else
                                {
                                    list = new List<Category>() { };
                                    list.AddRange(db.Category.Where(pp => pp.parentId == catId));
                                    ViewData["paramBag" + i + "2"] = new SelectList(list, "id", "tip");
                                }
                            }
                            ViewData["paramDivStyle" + i + "3"] = "display:none;";
                        }
                        else
                        {
                            ViewData["paramDivStyle" + i + "2"] = "display:none;";
                            if (vidId == 201 || vidId == 205) ViewData["paramDivStyle" + i + "3"] = "display:none;";
                        }
                    }


                    if (vidId == 30 || vidId == 23 || vidId == 205)//+&& pars[i].param1.HasValue && pars[i].param1!=158 )
                    {
                        ViewData["paramDivStyle" + i + "4"] = "display:block;";
                    }
                    else ViewData["paramDivStyle" + i + "4"] = "display:none;";

                    if (vidId == 30 || vidId == 23) ViewData["paramDivStyle" + i + "5"] = "display:block;";
                    else ViewData["paramDivStyle" + i + "5"] = "display:none;";
                }
                else
                {
                    ViewData["paramDivStyle" + i + "1"] = "display:none;";
                    ViewData["paramDivStyle" + i + "2"] = "display:none;";
                    ViewData["paramDivStyle" + i + "3"] = "display:none;";
                    ViewData["paramDivStyle" + i + "4"] = "display:none;";
                    ViewData["paramDivStyle" + i + "5"] = "display:none;";
                    ViewData["paramBag" + i + "1"] = empty;
                }
            }
            if (existPockets) ViewData["cebestDivStyle"] = "display:none;";//+hide price set 0

            return View(li);
        }
        public JsonResult fillParam2(int catId)
        {
            kvotaEntities db = new kvotaEntities();
            //if (new int[] { 30,27, 28, 29 }.Contains((t.Item1.SelectedItem as Category).id))
            /*if (cat.parentId == 407) t.Item4.ItemsSource = MainPage.db.Categories.Where(pp => pp.parentId == cat.id);
            if (cat.parentId == 403) t.Item4.ItemsSource = MainPage.db.Categories.Where(pp => pp.parentId == cat.id);
            if (cat.parentId == 405) t.Item4.ItemsSource = MainPage.db.Categories.Where(pp => pp.parentId == cat.id);
            if (cat.parentId == 408) t.Item4.ItemsSource = MainPage.db.Categories.Where();
            */

            var nullObj = new Category() { tip = "(не выбрано)" };
            var list = new List<Category>() { nullObj };
            list.AddRange(db.Category.Where(pp => pp.parentId == catId));
            return Json(list, JsonRequestBehavior.AllowGet);
            /*if (new int[] { 27, 28, 30 }.Contains((t.Item1.SelectedItem as Category).id))
                tuples[i].Item7.Focus();
            else
            {
                ComboBox nextCombo = t.Item4;
                if (t.Item4.ItemsSource == null && kolvoMestComboBox.SelectedIndex > 1) nextCombo = tuples[i].Item1;
                nextCombo.Focus();
                nextCombo.IsDropDownOpen = true;
            }*/
        }
        public JsonResult fillParam1(int catId)
        {
            kvotaEntities db = new kvotaEntities();
            var nullObj = new Category() { tip = "(не выбрано)" };
            List<Category> list = new List<Category>() { nullObj };
            var data = new List<KeyValuePair<string, IEnumerable<Category>>>();
            switch (catId)
            {
                case 30: //Тиснение

                    list = new List<Category>() { nullObj };
                    list.AddRange(db.Category.Where(pp => pp.parentId == 401));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   материал:", list));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   формат:", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   клише уже изготовлено", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   тиснение с фольгой 1 цв.", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 23: //Шелкография
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 403));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   цвет основы:", list));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   количество цветов:", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   печать на синтетических тканях (сумки, плащевки, зонты)", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   печать более формата А4 или площади 600 кв. см.", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 24: //Тампопечать
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 405));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   основа:", list));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   количество цветов:", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 25: //Сублимационная печать
                    //  list = new List<Category>() { nullObj };list.AddRange(db.Category.Where(pp => pp.parentId == 407));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 26: //УФ печать 
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 408));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   основа:", list));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   размер запечатки:", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));


                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 27: //Деколь
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 410));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   цветность:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   площадь логотипа, кв.см:", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 28: //Гравировка
                         //list = new List<Category>() { nullObj };list.AddRange(db.Category.Where(pp => pp.parentId == 411));
                         // data.Add(new KeyValuePair<string, IEnumerable<Category>>("   техника",list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   общая площадь гравировки, кв.см:", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;
                case 29: //Пакеты
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 412));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   пакет:", list));
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 413));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   цвет шелкографии:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;
                case 201: //Значки
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 414));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   размер:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;
                case 205: //dtg
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 415));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   размер:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));

                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   печать на цветном", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 211: //Квартальные календари
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    //list = new List<Category>() { nullObj };list.Add(new Category() { })
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить рекламное поле, шт:", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить индивидуальную сетку", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 212:// Ленты для бейджей(ланъярды)
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 417));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   основа:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   толщина:", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   премиум крепление", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   замок обрыва", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   межуровневый замок", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 213:// Открытки
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 418));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   вид:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить ушки", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить кальку 0+0", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить кальку 1+0", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить кальку 4+0", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 214:// Пакеты бумажные
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 419));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   размер:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 215:// Прочая полиграфия
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 420));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   вид:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 216:// Силиконовые браслеты
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 421));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   вид:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить ещё один цвет нанесения", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить колечко на браслет", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   сделать браслет светонакопительным", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   упаковать каждый браслет в отдельный пакетик", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   сегментировать браслет", null));
                    break;

                case 217:// Слэп браслеты
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 422));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   размер:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   добавить ещё один цвет нанесения", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   упаковать каждый браслет в отдельный пакетик", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   выполнить полноцветное нанесение", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 218:// Футболка с сублимацией
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 163));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   размер:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;

                case 219:// Шары с логотипом
                    list = new List<Category>() { nullObj }; list.AddRange(db.Category.Where(pp => pp.parentId == 424));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   параметры:", list));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   шар 'Металлик'", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("   палочка с розеткой", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    data.Add(new KeyValuePair<string, IEnumerable<Category>>("", null));
                    break;


            }
            //t.Item2.Focus());
            //t.Item2.IsDropDownOpen = true;

            /* bool existPocket = false;
             foreach (var tu in tuples)
                 if (tu.Item1.SelectedItem != null && (tu.Item1.SelectedItem as Category).id == 29) { existPocket = true; break; }
             if (existPocket) { sebestTextBlock.Visibility = sebestTextBox.Visibility = System.Windows.Visibility.Collapsed; sebestTextBox.Text = "0",list)); }
             else sebestTextBlock.Visibility = sebestTextBox.Visibility = System.Windows.Visibility.Visible;

             var data = db.Category.Where(pp => pp.parentId == id));*/
            // In reality you will do a database query based on the value of provinceId, but based on the code you have shown
            //var cities = new List<string>() { "City1", "City2", "City3" }));
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditSuvenir(ListItem li)
        {
            kvotaEntities db = new kvotaEntities();
            db.Entry(li).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        #endregion

        #region Сувениры в корне 
        /*public class Param
        {
            public int? vid;
            public int? param1;
            public int? param2;

            public Param(int? _vid, int? _param1, int? _param2) { vid = _vid; param1 = _param1; param2 = _param2; }
        }*/
        public ActionResult EditПакетыЗначкиСублимация(int id)
        {
            kvotaEntities db = new kvotaEntities();
            Dictionary<int, string[]> labelByVid = new Dictionary<int, string[]>() {
            { 25, new string[]{ null, null, null, null, null, null, null, null } },
            { 29, new string[]{ "   пакет:", "   цвет шелкографии:", null, null, null, null, null, null } },
            { 201, new string[]{ "   размер:", null, null, null, null, null, null, null } },

            { 211, new string[]{ null,  "   добавить рекламное поле, шт:", null,"   добавить индивидуальную сетку", null, null, null, null } },
            { 212, new string[]{ "   основа:", "   толщина:", null, "   премиум крепление","   замок обрыва","   межуровневый замок", null, null } },
            { 213, new string[]{ "   вид:", null, null,"   добавить ушки","   добавить кальку 0+0","   добавить кальку 1+0","   добавить кальку 4+0", null } },
            { 214, new string[]{ "   размер:", null, null, null, null, null, null, null } },
            { 215, new string[]{ "   вид:", null, null, null, null, null, null, null } },
            { 216, new string[]{ "   вид:", null, null, "   добавить ещё один цвет нанесения", "   добавить колечко на браслет", "   сделать браслет светонакопительным", "   упаковать каждый браслет в отдельный пакетик", "   сегментировать браслет" } },
            { 217, new string[]{ "   размер:", null, null, "   добавить ещё один цвет нанесения", "   упаковать каждый браслет в отдельный пакетик", "   выполнить полноцветное нанесение", null, null } },
            { 218, new string[]{ "   размер:", null, null, null, null, null, null, null } },
            { 219, new string[]{ "   параметры:", null, null, "   шар 'Металлик'", "   палочка с розеткой", null, null, null } },
            };

            ViewData["askBetterPriceDivStyle"] = "display:none;";


            ViewData["labelByVid"] = labelByVid;//Json(labelByVid);



            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };
            List<Category> list;

            List<Param> pars = new List<Param>();
            pars.Add(null);
            pars.Add(new Param(li.vid1, li.param11, li.param12));
            pars.Add(new Param(li.vid2, li.param21, li.param22));
            pars.Add(new Param(li.vid3, li.param31, li.param32));
            pars.Add(new Param(li.vid4, li.param41, li.param42));

            bool existPockets = false;
            for (int i = 1; i <= 1; i++)
            {
                ViewData["paramDivStyle" + i + "2"] = "display:block;";
                ViewData["paramDivStyle" + i + "3"] = "display:block;";
                ViewData["paramBag" + i + "2"] = empty;
                if (pars[i].vid.HasValue && pars[i].vid > 0)
                {
                    int vidId = pars[i].vid.Value;
                    for (int j = 0; j < 8; j++) ViewData["paramLabel" + (i) + (j + 1)] = labelByVid[vidId][j];
                    int parent = parentByCat[vidId];
                    if (vidId == 25 || vidId == 211)
                    {
                        ViewData["paramBag" + i + "1"] = empty;
                        ViewData["paramDivStyle" + i + "1"] = "display:none;";
                    }
                    else ViewData["paramBag" + i + "1"] = new SelectList((from pp in db.Category where pp.parentId == parent select pp), "id", "tip");
                    if (vidId == 29)//пакеты
                    {
                        existPockets = true;
                        list = new List<Category>() { }; list.AddRange(db.Category.Where(pp => pp.parentId == 413));
                        ViewData["paramBag" + i + "2"] = new SelectList(list, "id", "tip");
                        ViewData["paramDivStyle" + i + "3"] = "display:none;";
                    }
                    else
                    if (vidId == 212)
                    {
                        if (pars[i].param1.HasValue)
                        {
                            int catId = pars[i].param1.Value;
                            list = new List<Category>() { };
                            list.AddRange(db.Category.Where(pp => pp.parentId == catId));
                            ViewData["paramBag" + i + "2"] = new SelectList(list, "id", "tip");
                        }
                    }
                    else
                    if (vidId == 211)
                    {                        
                        var ilist=(new[] {
                            new { id = 0, tip = 0 },
                            new { id = 1, tip = 1 },
                            new { id = 2, tip = 2 },
                            new { id = 3, tip = 3 }
                        });
                            ViewData["paramBag" + i + "2"] = new SelectList(ilist, "id", "tip");                                               
                    }
                    else
                    {
                        ViewData["paramDivStyle" + i + "2"] = "display:none;";
                    }
                  ViewData["paramDivStyle" + i + "3"] = "display:none;";

                    int[] visibleChecks = new int[0];
                    switch (vidId)
                    {
                        case 211: visibleChecks = new[] { 4 }; break;
                        case 212: visibleChecks = new[] { 4, 5, 6 }; break;
                        case 213: visibleChecks = new[] { 4, 5, 6, 7 }; break;
                        case 216:
                            if (pars[i].param1.HasValue && pars[i].param1.Value==229)
                                visibleChecks = new[] { 4, 5, 6, 7 };
                            else visibleChecks = new[] { 4, 5, 6, 7, 8 }; break;
                        case 217:
                            if (pars[i].param1.HasValue && pars[i].param1.Value == 232)
                                visibleChecks = new[] { 4, 5};
                            else visibleChecks = new[] { 4, 5, 6 }; break;
                        case 219: visibleChecks = new[] { 4, 5 }; break;
                    }
                    for (int vc = 4; vc <= 8; vc++)
                        if (visibleChecks.Contains(vc))
                            ViewData["paramDivStyle" + i + vc.ToString()] = "display:block;";
                        else ViewData["paramDivStyle" + i + vc.ToString()] = "display:none;";


                }
                else
                {
                    for (int vc = 1; vc <= 8; vc++)
                        ViewData["paramDivStyle" + i + vc.ToString()] = "display:none;";
                    ViewData["paramBag" + i + "1"] = empty;
                }
            }
            if (existPockets) ViewData["cebestDivStyle"] = "display:none;";//+hide price set 0

            return View(li);
        }

        [HttpPost]
        public ActionResult EditПакетыЗначкиСублимация(ListItem li)
        {
            kvotaEntities db = new kvotaEntities();
            db.Entry(li).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        #endregion

        #region Футболки
        public ActionResult EditФутболки(int id)
        {
            kvotaEntities db = new kvotaEntities();
            ViewData["askBetterPriceDivStyle"] = "display:none;";
            
            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };
ViewData["paramBag11"] = new SelectList((from pp in db.Category where pp.parentId == 163 select pp), "id", "tip");
            ViewData["paramLabel11"] = "основа:";
            ViewData["paramBag12"] = ViewData["paramBag21"] = ViewData["paramBag22"] = ViewData["paramBag31"] = new SelectList((from pp in db.Category where pp.parentId == 423 select pp), "id", "tip");
            ViewData["paramLabel12"] = "размер нанесения 1:";
            ViewData["paramLabel21"] = "размер нанесения 2:";
            ViewData["paramLabel22"] = "размер нанесения 3:";
            ViewData["paramLabel31"] = "размер нанесения 4:";
            //              ViewData["paramDivStyle" + i + "2"] = "display:block;";
            List<Category> list;
            
            return View(li);
        }

        [HttpPost]
        public ActionResult EditФутболки(ListItem li)
        {
            kvotaEntities db = new kvotaEntities();
            db.Entry(li).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        #endregion

        #region Диски
        public ActionResult EditДиски(int id)
        {
            kvotaEntities db = new kvotaEntities();
            ViewData["askBetterPriceDivStyle"] = "display:none;";

            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };
            ViewData["paramLabel11"] = "тип диска:";
            ViewData["paramBag11"] = new SelectList((from pp in db.Category where pp.parentId == 425 select pp), "id", "tip");

            ViewData["paramLabel12"] = "параметры:";
            ViewData["paramBag12"] = new SelectList(new List<Category>(), "id", "tip");
            ViewData["paramLabel21"] = "упаковка:";
            ViewData["paramBag21"] = new SelectList(new List<Category>(), "id", "tip");
            var catId = li.param11 ?? 0;
            if (catId > 0)
            {
                if (catId == 270)
                    ViewData["paramBag12"] = new SelectList(
                   new[] {
                     new { id=272, tip= "диск, печать" },
                     new { id=273, tip= "диск, печать, запись" },
                     new { id=274, tip= "диск, печать, запись, лакировка" },
                   }
               , "id", "tip");
                else
                    ViewData["paramBag12"] = new SelectList(
                   new[] {
                     new { id=275, tip= "диск, печать" },
                     new { id=276, tip= "диск, печать, запись" },
                     new { id=277, tip= "диск, печать, лакировка" },
                     new { id=278, tip= "диск, печать, запись, лакировка" },
                   }
               , "id", "tip");

                if (catId == 270)
                    ViewData["paramBag21"] = new SelectList(
                   new[] {
                     new { id=4, tip= "Slim box черный тонкий" },
                     new { id=5, tip= "Slim box прозрачный" },
                     new { id=6, tip= "CD box прозрачный" },
                     new { id=7, tip= "CD box чёрный" },
                     new { id=8, tip= "CD box на 2 диска" },
                     new { id=9, tip= "Конверт" },
                     new { id=10, tip= "Пакетик для CD, DVD box" },
                   }
               , "id", "tip");
                else
                    ViewData["paramBag21"] = new SelectList(
                   new[] {
                     new { id=1, tip= "DVD box черный" },
                     new { id=2, tip= "DVD box прозрачный" },
                     new { id=3, tip= "DVD box (по размеру диска)" },
                     new { id=4, tip= "Slim box черный тонкий" },
                     new { id=5, tip= "Slim box прозрачный" },
                     new { id=9, tip= "Конверт" },
                     new { id=10, tip= "Пакетик для CD, DVD box" },
                   }
               , "id", "tip");
            }

            ViewData["paramLabel22"] = "раскладка:";
            ViewData["paramBag22"] = new SelectList(
                new[] {
                     new { id=1, tip= "Укладка диска в конверт" },
                     new { id=2, tip= "Укладка диска и полиграфии в Slim-box" },
                     new { id=3, tip= "Укладка диска и полиграфии в DVD-box" },
                }
            , "id", "tip");
            return View(li);
        }

        [HttpPost]
        public ActionResult EditДиски(ListItem li)
        {
            kvotaEntities db = new kvotaEntities();
            db.Entry(li).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        #endregion

        #region Блокноты
        public ActionResult EditБлокнот(int id)
        {
            kvotaEntities db = new kvotaEntities();
            var item = new Blocknot() { Tiraz = 30 };
            return View(item);

            ViewData["askBetterPriceDivStyle"] = "display:none;";

            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };
            ViewData["paramLabel11"] = "тип диска:";
            ViewData["paramBag11"] = new SelectList((from pp in db.Category where pp.parentId == 425 select pp), "id", "tip");

            ViewData["paramLabel12"] = "параметры:";
            ViewData["paramBag12"] = new SelectList(new List<Category>(), "id", "tip");
            ViewData["paramLabel21"] = "упаковка:";
            ViewData["paramBag21"] = new SelectList(new List<Category>(), "id", "tip");
            var catId = li.param11 ?? 0;
            if (catId > 0)
            {
                if (catId == 270)
                    ViewData["paramBag12"] = new SelectList(
                   new[] {
                     new { id=272, tip= "диск, печать" },
                     new { id=273, tip= "диск, печать, запись" },
                     new { id=274, tip= "диск, печать, запись, лакировка" },
                   }
               , "id", "tip");
                else
                    ViewData["paramBag12"] = new SelectList(
                   new[] {
                     new { id=275, tip= "диск, печать" },
                     new { id=276, tip= "диск, печать, запись" },
                     new { id=277, tip= "диск, печать, лакировка" },
                     new { id=278, tip= "диск, печать, запись, лакировка" },
                   }
               , "id", "tip");

                if (catId == 270)
                    ViewData["paramBag21"] = new SelectList(
                   new[] {
                     new { id=4, tip= "Slim box черный тонкий" },
                     new { id=5, tip= "Slim box прозрачный" },
                     new { id=6, tip= "CD box прозрачный" },
                     new { id=7, tip= "CD box чёрный" },
                     new { id=8, tip= "CD box на 2 диска" },
                     new { id=9, tip= "Конверт" },
                     new { id=10, tip= "Пакетик для CD, DVD box" },
                   }
               , "id", "tip");
                else
                    ViewData["paramBag21"] = new SelectList(
                   new[] {
                     new { id=1, tip= "DVD box черный" },
                     new { id=2, tip= "DVD box прозрачный" },
                     new { id=3, tip= "DVD box (по размеру диска)" },
                     new { id=4, tip= "Slim box черный тонкий" },
                     new { id=5, tip= "Slim box прозрачный" },
                     new { id=9, tip= "Конверт" },
                     new { id=10, tip= "Пакетик для CD, DVD box" },
                   }
               , "id", "tip");
            }

            ViewData["paramLabel22"] = "раскладка:";
            ViewData["paramBag22"] = new SelectList(
                new[] {
                     new { id=1, tip= "Укладка диска в конверт" },
                     new { id=2, tip= "Укладка диска и полиграфии в Slim-box" },
                     new { id=3, tip= "Укладка диска и полиграфии в DVD-box" },
                }
            , "id", "tip");
            return View(li);
        }

        [HttpPost]
        public ActionResult EditБлокнот(Blocknot li)
        {
            // db.Entry(li).State = System.Data.Entity.EntityState.Modified;
            // db.SaveChanges();
            return RedirectToAction("Edit", "Home");//, new { id = li.listId });
        }
        #endregion
        #region Блокноты
        public ActionResult EditЗначки2(int id)
        {
            kvotaEntities db = new kvotaEntities();
            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            var item =  Mapper.Map<SvetootrazatelNaklei>(li);//ItemBase.Create(li);
            ViewData = item.ViewData;
var vm =  Mapper.Map<SvetootrazatelOneValueVM>(item);//ItemBase.Create(li);
            return View(item.ViewName, vm);
        }
            public ActionResult EditЗначки(int id)
        {
            kvotaEntities db = new kvotaEntities();
            var li = db.ListItem.FirstOrDefault(pp => pp.id == id);
            var item = ItemBase.Create(li);
            ViewData = item.ViewData;
            return View(item.ViewName,item);

            ViewData["askBetterPriceDivStyle"] = "display:none;";

            //var li = db.ListItem.FirstOrDefault(pp => pp.id == id);

            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };
            ViewData["paramLabel11"] = "тип диска:";
            ViewData["paramBag11"] = new SelectList((from pp in db.Category where pp.parentId == 425 select pp), "id", "tip");

            ViewData["paramLabel12"] = "параметры:";
            ViewData["paramBag12"] = new SelectList(new List<Category>(), "id", "tip");
            ViewData["paramLabel21"] = "упаковка:";
            ViewData["paramBag21"] = new SelectList(new List<Category>(), "id", "tip");
            var catId = li.param11 ?? 0;
            if (catId > 0)
            {
                if (catId == 270)
                    ViewData["paramBag12"] = new SelectList(
                   new[] {
                     new { id=272, tip= "диск, печать" },
                     new { id=273, tip= "диск, печать, запись" },
                     new { id=274, tip= "диск, печать, запись, лакировка" },
                   }
               , "id", "tip");
                else
                    ViewData["paramBag12"] = new SelectList(
                   new[] {
                     new { id=275, tip= "диск, печать" },
                     new { id=276, tip= "диск, печать, запись" },
                     new { id=277, tip= "диск, печать, лакировка" },
                     new { id=278, tip= "диск, печать, запись, лакировка" },
                   }
               , "id", "tip");

                if (catId == 270)
                    ViewData["paramBag21"] = new SelectList(
                   new[] {
                     new { id=4, tip= "Slim box черный тонкий" },
                     new { id=5, tip= "Slim box прозрачный" },
                     new { id=6, tip= "CD box прозрачный" },
                     new { id=7, tip= "CD box чёрный" },
                     new { id=8, tip= "CD box на 2 диска" },
                     new { id=9, tip= "Конверт" },
                     new { id=10, tip= "Пакетик для CD, DVD box" },
                   }
               , "id", "tip");
                else
                    ViewData["paramBag21"] = new SelectList(
                   new[] {
                     new { id=1, tip= "DVD box черный" },
                     new { id=2, tip= "DVD box прозрачный" },
                     new { id=3, tip= "DVD box (по размеру диска)" },
                     new { id=4, tip= "Slim box черный тонкий" },
                     new { id=5, tip= "Slim box прозрачный" },
                     new { id=9, tip= "Конверт" },
                     new { id=10, tip= "Пакетик для CD, DVD box" },
                   }
               , "id", "tip");
            }

            ViewData["paramLabel22"] = "раскладка:";
            ViewData["paramBag22"] = new SelectList(
                new[] {
                     new { id=1, tip= "Укладка диска в конверт" },
                     new { id=2, tip= "Укладка диска и полиграфии в Slim-box" },
                     new { id=3, tip= "Укладка диска и полиграфии в DVD-box" },
                }
            , "id", "tip");
            return View(li);
        }

        [HttpPost]
        public ActionResult EditЗначки(Znachok item)
        {
            kvotaEntities db = new kvotaEntities();
            return null;
            var li = item.ToListItem();
             db.Entry(li).State = System.Data.Entity.EntityState.Modified;
             db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        #endregion
       /* [HttpPost]
        public JsonResult Recalc(ListItem li)
        {
            double sum = 0;
            // if (li.dopUslDost) sum += 400;
            //if (li.dopUslMaket) sum += 400;
            // db.Zakaz.
            //  var z = db.Zakaz.FirstOrDefault(pp => pp.id == id);
            //string.Format("{0} of {1}",sum/z.)
            switch (li.tipProd)
            {
                case 2:  reCalcPoligrafiya(li); break;
                case 3: reCalcBanner(li); break;
                case 1: reCalcSuvenir(li); break;
            }
            //   Calc.recalcLi(li);
            //   var data = new { total = li.total, totalLabel = li.totalLabel, askBetterPrice = li.askBetterPrice };
            var data = new { total = 10, totalLabel = "10", askBetterPrice = false};
            return Json(data);
        }*/

        public ActionResult Recalc(TotalResultsModel totals)
        {
            return PartialView("TotalResults", totals);
        }
        ItemBase GetModel<T>(FormCollection collection)
            where T:ItemBase,new()
        {
            var model = new T();
            TryUpdateModel(model, collection);
            return model;
        }

        [HttpPost]
        public ActionResult Recalc(FormCollection collection)
        {
            TipProds tipProd = (TipProds)Enum.Parse(typeof(TipProds), collection["TipProd"]) ; 
           // var tipProd = (TipProds)int.Parse(collection["TipProd"]);
            ItemBase model=null;
            switch (tipProd)
            {
                case TipProds.Znachok: var model0 = new Znachok(); TryUpdateModel(model0, collection); model = model0; break;
                case TipProds.Shelkografiya: var model1 = new Shelkografiya(); TryUpdateModel(model1, collection); model = model1; break;
                case TipProds.Tampopechat: var model2 = new Tampopechat(); TryUpdateModel(model2, collection); model = model2; break;
                case TipProds.PaketPvd: var model3 = new PaketPvd(); TryUpdateModel(model3, collection); model = model3; break;
                case TipProds.Tisnenie: var model4 = new Tisnenie(); TryUpdateModel(model4, collection); model = model4; break;
                case TipProds.DTG: var model5 = new DTG(); TryUpdateModel(model5, collection); model = model5; break;
                case TipProds.Gravirovka: var model6 = new Gravirovka(); TryUpdateModel(model6, collection); model = model6; break;
                case TipProds.UFkachestvo: var model7 = new UFkachestvo(); TryUpdateModel(model7, collection); model = model7; break;
                case TipProds.UFstandart: var model8 = new UFstandart(); TryUpdateModel(model8, collection); model = model8; break;
                case TipProds.Decol: var model9= new Decol(); TryUpdateModel(model9, collection); model = model9; break;
                case TipProds.BumajniiPaket: var model10= new BumajniiPaket(); TryUpdateModel(model10, collection); model = model10; break;
                case TipProds.Flag: model = GetModel<Flag>(collection);break; 
                case TipProds.FlagPobedi: model = GetModel<FlagPobedi>(collection);break;
                case TipProds.FlagNSO: model = GetModel<FlagNSO>(collection);break;
                case TipProds.Vimpel: model = GetModel<Vimpel>(collection);break;
                case TipProds.Skatert: model = GetModel<Skatert>(collection);break;
                case TipProds.Sharf: model = GetModel<Sharf>(collection);break;
                case TipProds.Platok: model = GetModel<Platok>(collection);break;
                case TipProds.ReklNakidka: model = GetModel<ReklNakidka>(collection);break;
                case TipProds.SportNomer: model = GetModel<SportNomer>(collection);break;
                case TipProds.Futbolka: model = GetModel<Futbolka>(collection);break;
                case TipProds.SlapChasi: model = GetModel<SlapChasi>(collection);break;
            }
          
                var li = model.ToListItem();
            SaveLi(li);
            //var sss = model.Calc();
            var totals = model.GetTotal();

            double sum = 0;
            // if (li.dopUslDost) sum += 400;
            //if (li.dopUslMaket) sum += 400;
            // db.Zakaz.
            //  var z = db.Zakaz.FirstOrDefault(pp => pp.id == id);
            //string.Format("{0} of {1}",sum/z.)
            /*switch (li.tipProd)
            {
                case 2:  reCalcPoligrafiya(li); break;
                case 3: reCalcBanner(li); break;
                case 1: reCalcSuvenir(li); break;
            }*/
            //   Calc.recalcLi(li);
            //   var data = new { total = li.total, totalLabel = li.totalLabel, askBetterPrice = li.askBetterPrice };
            return PartialView("TotalResults", totals);

            //var data = new { total = 10, totalLabel = string.Join(";",sss.Select(pp=>pp.Cena)), askBetterPrice = model.askBetterPrice};
            //return Json(data);
        }
        public void SaveLi(ListItem li)
        {
            kvotaEntities db = new kvotaEntities();
            db.Entry(li).State = System.Data.Entity.EntityState.Modified;

            var z = db.Zakaz.FirstOrDefault(pp => pp.id == li.listId);
            z.comment = string.Join(", ",
            db.ListItem.Where(pp => pp.listId == z.id).Select(pp => pp.tipProd).ToList()
            .Select(pp => ListItem.GetTipProdName((TipProds)pp)));
            db.SaveChanges();
        }
        [HttpPost]
        public ActionResult RecalcOld(ListItem li)
        {
            double sum = 0;
            // if (li.dopUslDost) sum += 400;
            //if (li.dopUslMaket) sum += 400;
            // db.Zakaz.
            //  var z = db.Zakaz.FirstOrDefault(pp => pp.id == id);
            //string.Format("{0} of {1}",sum/z.)
            /*switch (li.tipProd)
            {
                case 2:  reCalcPoligrafiya(li); break;
                case 3: reCalcBanner(li); break;
                case 1: reCalcSuvenir(li); break;
            }*/
               Calc.recalcLi(li);
            SaveLi(li);

            var data = new { total = li.total, totalLabel = li.totalLabel, askBetterPrice = li.askBetterPrice };
            return Json(data);
        }
    }
}