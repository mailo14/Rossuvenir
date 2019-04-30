using KvotaWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KvotaWeb
{
    public class Calc
    {

        //public static kvotaEntities db = new kvotaEntities();
        public static double? recalcLi(ListItem li)
        {
            li.askBetterPrice = false;
            switch (li.tipProd)
            {
                //case 1: return reCalcSuvenir(li); break;
             //   case 2: return reCalcPoligrafiya(li); break;
                case 3: return reCalcBanner(li); break;
                //case 4: case 6:  case 10:return reCalcSuvenir(li); break;
               // default:return reCalcSuvenir(li); break;
            }
            return null;
        }


        private static double? reCalcBanner(ListItem li)
        {
            if (li.bVid>0 && li.bMat>0 && li.bDpi>0 && li.tiraz>0)
            {
                var db = new kvotaEntities();
               decimal  tiraz = (decimal)li.tiraz.Value;
                decimal sum = 0;

                decimal cena;

                if (tiraz < 1) tiraz = 1;
                cena = (decimal)(from p in db.Price where p.catId == li.bMat && p.tiraz==li.bDpi select p.cena).First();
                sum += cena * tiraz;

                if (li.bPost1.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 510 select p.cena).First();
                    sum += cena * li.bPost1.Value;
                }
                if (li.bPost2.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 511 select p.cena).First();
                    sum += cena * li.bPost2.Value;
                }
                if (li.bPost3.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 512 select p.cena).First();
                    sum += cena * li.bPost3.Value;
                }
                if (li.bPost4.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 513 select p.cena).First();
                    sum += cena * li.bPost4.Value;
                }
                if (li.bPost5.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 514 select p.cena).First();
                    sum += cena * li.bPost5.Value;
                }
                if (li.bPost6.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 515 select p.cena).First();
                    sum += cena * li.bPost6.Value;
                }
                if (li.bPost7.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 516 select p.cena).First();
                    sum += cena * li.bPost7.Value;
                }
                if (li.bPost8.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 517 select p.cena).First();
                    sum += cena * li.bPost8.Value;
                }
                if (li.bPost9.HasValue)
                {
                    cena = (decimal)(from p in db.Price where p.catId == 518 select p.cena).First();
                    sum += cena * li.bPost9.Value;
                }

                sum = AddDopsNacenk(sum, li.listId,li.tipProd);
                sum = Math.Ceiling(sum / tiraz) * tiraz;
                li.total = (double)sum;
                li.totalLabel = string.Format("за ед.: {0:#0.00} р., всего: {1:#0.00} р.", sum / tiraz, sum);
            }
            else { li.total = null; li.totalLabel = "за ед.: - р., всего: - р."; }
            //var data = new { total = li.total, totalLabel = li.totalLabel };
            //return Json(data);
            return li.total;
        }

        public static decimal AddDopsNacenk(decimal sum, int? listId,int tipProd)
        {
            var db = new kvotaEntities();
            var z = db.Zakaz.First(pp => pp.id == listId);
            //+доп.услуги+доп.траты)*наценка
            //if (z.dopUslDost) sum += 400;
            //if (z.dopUslMaket) sum += 400;
            if (z.dopTrat.HasValue) sum += (decimal)z.dopTrat.Value;

            switch (z.nacenTip)
            {
                case 1://стандарт
                    if (tipProd == 3) sum *= 1.4m;
                    else sum *= 1.0m;
                    break;
                /*case 2:
                    if (tipProd == 1) sum *= 1.2;
                    else sum *= 1.6;
                    break;*/
                case 3://своя
                    if (z.nacenValue.HasValue) sum *= (decimal)z.nacenValue.Value; break;
            }
            return sum;
        }
    }
}