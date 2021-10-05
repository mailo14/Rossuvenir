﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KvotaWeb.Models;
using WebMatrix.WebData;
using System.Web.Security;
using KvotaWeb.Models.Items;

namespace KvotaWeb.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        
        kvotaEntities db = new kvotaEntities();
        public ActionResult Index()
        {
            IEnumerable<Zakaz> zakazi = db.Zakaz.OrderByDescending(pp => pp.dat).Take(15);//.Take(3);
            ViewBag.zz = zakazi;
            return View();
        }
        public ActionResult Edit(int id)
        {
            ViewBag.nacenks = new SelectList(new[] {
                //new { Name = "0", Value =" "},
               // null,
                new { Name = "1", Value ="стандарт"},
               // new { Name = "2", Value ="жд"},
                new { Name = "3", Value = "своя"},
    }, "Name", "Value");
            var z = db.Zakaz.FirstOrDefault(pp => pp.id == id);
            ViewBag.listItems = db.ListItem.Where(pp => pp.listId == z.id);
        //    var nacens = new SelectList({ { "1", 1 }});
        if (z.nacenTip!=3) ViewData["nacenValueDivStyle"] = "display:none;";
            string s = "";double sum=0;
            foreach (ListItem li in ViewBag.listItems)
            {
                sum += li.total??0;
                if (s.Length > 0) s += ", ";
                s += li.TipProdName;
            }
            z.comment =(s!="")? s:"Новый заказ";
            z.total = sum;/**/
            db.SaveChanges();

            return View(z);
        }
        [HttpPost]
        public ActionResult Edit(Zakaz z)
        {
             db.Entry(z).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            // db.Zakaz.
            //  var z = db.Zakaz.FirstOrDefault(pp => pp.id == id);
            return RedirectToAction("Index");
        }

        public ActionResult Add(int userId)
        {
            
               var z = new Zakaz() { nacenTip = 1, dat = DateTime.Now, comment = "Новый заказ", dopUslDost = true, dopUslMaket = true,
               userId = WebSecurity.GetUserId(User.Identity.Name), userName = User.Identity.Name
               };
            db.Zakaz.Add(z);
            db.SaveChanges();
            return RedirectToAction("Edit", "Home", new { id = z.id });
        }
        [HttpPost]
        public ActionResult Add(Zakaz z)
        {
            db.Entry(z).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            // db.Zakaz.
            //  var z = db.Zakaz.FirstOrDefault(pp => pp.id == id);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            var z = db.Zakaz.Find(id);
            db.Zakaz.Remove(z);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AddLi(int zId, TipProds tipProd)
        {
            var li = new ListItem();
            if (new TipProds[] { TipProds.DTG, TipProds.Gravirovka, TipProds.Decol, TipProds.Tampopechat, TipProds.Tisnenie, TipProds.UFkachestvo, TipProds.UFstandart, TipProds.Shelkografiya }
                .Contains(tipProd))
            {
                li.tipProd = (int)TipProds.MultiSuvenir;
                li.listId = zId;
                db.ListItem.Add(li);
                db.SaveChanges();

                var subLi = new ListItem();
                subLi.tipProd = (int)tipProd;
                subLi.parentId = li.id;
                db.ListItem.Add(subLi);
                db.SaveChanges();

            return EditLi(li.id, TipProds.MultiSuvenir);
            }
            else
            {
                //int tipProd = SelectedIndex++;
                li.tipProd = (int)tipProd;
                if (li.tipProd == 3) li.bDpi = 144;

                li.listId = zId;

                db.ListItem.Add(li);
                db.SaveChanges();

            return EditLi(li.id, tipProd);
            }
            /*switch (tipProd)
            {
                // case 4: li.vid1 = 201;break;
                case 4:    
            db.ListItem.Add(li);
            db.SaveChanges();
                   // var ii=ItemBase.Create(li);
                    return RedirectToAction("EditЗначки", "Product", new { id = li.id });

                    break;
                case 23:  case 24: case 29:case 30:case 18:case 28:case 31:case 32:
                case 33:    case 9:
                case 34:
                    db.ListItem.Add(li);
            db.SaveChanges();
                   // var ii=ItemBase.Create(li);
                    return RedirectToAction("EditЗначки", "Product", new { id = li.id });

                    break;
                case 6: li.vid1 = 25; break;
                case 10: li.vid1 = 29; break;

                case 5: li.vid1 = 211; li.param12 = 0; break;
                case 7: li.vid1 = 212; break;
                case 8: li.vid1 = 213; break;
                case 11: li.vid1 = 215; break;
                case 12: li.vid1 = 216; break;
                case 13: li.vid1 = 217; break;
                case 14: li.vid1 = 218; break;
                case 15: li.vid1 = 219; break;
                case 16: li.vid1 = 269; break;
                case 17: return RedirectToAction("EditБлокнот", "Product", new { id = 0 });
            }            
            db.ListItem.Add(li);
            db.SaveChanges();

            int id = li.id;
            return EditLi(id, tipProd);*/
            /* if (tipProd == 1) return RedirectToAction("EditSuvenir", "Product", new { id = id });  
             if (tipProd == 2) return RedirectToAction("EditPoligrafiya", "Product", new { id = id });
             return RedirectToAction("EditBanner", "Product", new { id = id });*/
        }
        /*public RedirectToRouteResult EditProductRedirectAction(int liId, tipProd)
        {
            if (tipProd == 1) return RedirectToAction("EditSuvenir", "Product", new { id = id });  
            if (tipProd == 2) return RedirectToAction("EditPoligrafiya", "Product", new { id = id });
            return RedirectToAction("EditBanner", "Product", new { id = id });
        }*/
        public ActionResult EditLi(int id ,TipProds tipProd)
        {              
            switch (tipProd)
            {
                //case TipProds.MultiSuvenir:return View("EditMultiSuvenir",new MultiSuvenir());
                //case TipProds.SvetootrazatelNaklei: return  RedirectToAction("EditЗначки2", "Product", new { id = id });
                case TipProds.Banner: return RedirectToAction("EditBanner", "Product", new { id = id });
                //case 4:case 23:case 24:case 29:case 30:case 18:case 28: case 31: case 32:  case 33:case 9: case 34:
                default:
                    return RedirectToAction("EditЗначки", "Product", new { id = id });
             /*   case 1:return RedirectToAction("EditSuvenir", "Product", new { id = id });
             //   case 2: return RedirectToAction("EditPoligrafiya", "Product", new { id = id });
               
                case 14: return RedirectToAction("EditФутболки", "Product", new { id = id });
                case 16: return RedirectToAction("EditДиски", "Product", new { id = id });
                //              case 4: case 6: case 10:
                default: return RedirectToAction("EditПакетыЗначкиСублимация", "Product", new { id = id });
               */
  
  //              default:return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DeleteLi(int id)
        {
            var li = db.ListItem.Find(id);
            db.ListItem.Remove(li);
            db.SaveChanges();
           return RedirectToAction("Edit", "Home", new { id = li.listId });
        }
        public ActionResult fillLitable(int zId)
        {
            var listItems = db.ListItem.Where(pp => pp.listId == zId);
            ViewBag.listItemsP = listItems;
            return PartialView();
        }
        [HttpPost]
        public JsonResult refillTotals(Zakaz z) 
        {
            db.Entry(z).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            reCalc(z, true);
            var lis = db.ListItem.Where(pp => pp.listId == z.id).AsEnumerable();

            var data = new { total = lis.Sum(pp=>pp.total)??0 };
            return Json(data);

            //var view = PartialView("fillLitable", lis);
            //view.ViewBag.listItems = lis;
            return null;
        }

        [HttpPost]
        public ActionResult Recalc(Zakaz z)
        {
            db.Entry(z).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            reCalc(z, true);
            return RedirectToAction("Edit",new { id=z.id});

            double sum = 0;
            if (z.dopUslDost) sum += 400;
            if (z.dopUslMaket) sum += 400;
            // db.Zakaz.
            //  var z = db.Zakaz.FirstOrDefault(pp => pp.id == id);
            //string.Format("{0} of {1}",sum/z.)
            z.total = sum;
            return PartialView(sum);
        }

        private void reCalc(Zakaz z,bool recalcItems)
        {
            double sum = 0;
            var lis = db.ListItem.Where(pp => pp.listId == z.id);
            foreach (var li in lis)
            {
                if (recalcItems) Calc.recalcLi(li);
                if (li.total.HasValue) sum += li.total.Value;
            }
            z.total = sum;
            db.SaveChanges();
        }


    }

   
}
namespace KvotaWeb.Models
{
    public partial class ListItem
    {
        public string TipProdName
        {
            get
            {
                return GetTipProdName((TipProds)tipProd,id,false);
            }
        }
        public static string GetTipProdName(TipProds tipProd,int id=0,bool addIstochnikCen=true)
        {
            string name;
            switch (tipProd)
            {
                case TipProds.Znachok: name= "Закатные значки";break;
                case TipProds.Shelkografiya: name= "Шелкография";break;
                case TipProds.Tampopechat: name= "Тампопечать";break;
                case TipProds.PaketPvd: name= "Пакеты ПВД";break;
                case TipProds.Tisnenie: name= "Тиснение";break;
                case TipProds.DTG: name= "DTG";break;
                case TipProds.Gravirovka: name= "Гравировка";break;
                case TipProds.UFkachestvo: name= "УФ (качество)";break;
                case TipProds.UFstandart: name= "УФ (стандарт)";break;
                case TipProds.Decol: name= "Деколь";break;
                case TipProds.BumajniiPaket: name= "Пакеты бумажные";break;
                case TipProds.Flag: name= "Флаги и растяжки";break;
                case TipProds.FlagPobedi: name= "Флаги Победы (Копия Знамени Победы)";break;
                case TipProds.FlagNSO: name= "Флаги Новосибирской области и Новосибирска";break;
                case TipProds.Vimpel: name= "Вымпелы";break;
                case TipProds.Skatert: name= "Скатерти";break;
                case TipProds.Sharf: name= "Шарфы";break;
                case TipProds.Platok: name= "Платки, косынки";break;
                case TipProds.ReklNakidka: name= "Рекламные накидки";break;
                case TipProds.SportNomer: name= "Спортивные номера";break;
                case TipProds.Futbolka: name= "Футболки";break;
                case TipProds.SlapChasi: name= "Силиконовые слэп часы";break;
                case TipProds.Svetootrazatel: name= "Светоотражатели";break;
                case TipProds.KontrBraslet: name= "Контрольные браслеты";break;
                case TipProds.SlapBraslet: name= "Слэп браслеты";break;
                case TipProds.Lenta: name= "Ленты для бейджей (ланъярды)";break;
                case TipProds.SiliconBraslet: name= "Силиконовые браслеты";break;
                case TipProds.MultiSuvenir:
                    kvotaEntities db = new kvotaEntities();
                    string ret= string.Join(@"/",
                        db.ListItem.Where(pp => pp.parentId == id).Select(pp => pp.tipProd).ToList()
                        .Select(pp => ListItem.GetTipProdName((TipProds)pp,id,addIstochnikCen)));
                    if (ret != "") return ret;
                        return "Сувениры";


                case TipProds.Banner:  name= "Баннеры и ПВХ";break;
                    /*
                case 1: return "Нанесение логотипа";
                
                case 5: return "Квартальные календари";
                case 6: return "Кружки с сублимацией";
                case 7: return "Ленты для бейджей (ланъярды)";
                case 8: return "Открытки ";
                case 11: return "Прочая полиграфия";
                case 12: return "Силиконовые браслеты";
                case 13: return "Слэп браслеты";
                case 14: return "Футболка с сублимацией";
                case 15: return "Шары с логотипом";
                case 16: return "Запись дисков";*/
                default: return null;
            }
            /*if (addIstochnikCen)
            {
                var tipProdInfo = new kvotaEntities().Product.FirstOrDefault(pp => pp.id == (int)tipProd);
                if (tipProdInfo != null && tipProdInfo.istochnikCen != "")
                    name += $" ({tipProdInfo.istochnikCen})";
            }*/
            return name;
        }
        public bool askBetterPrice = false;
    }
    public partial class Rabotnik
    {
        public string Role
        {
            get
            {
                if (uroven == 1) return "Администратор";
                return "Менеджер";
            }
        }

    }
}