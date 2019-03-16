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
                case 1: return reCalcSuvenir(li); break;
             //   case 2: return reCalcPoligrafiya(li); break;
                case 3: return reCalcBanner(li); break;
                //case 4: case 6:  case 10:return reCalcSuvenir(li); break;
                default:return reCalcSuvenir(li); break;
            }
            return null;
        }
        /* private JsonResult toJson(ListItem li)
         {
             var data = new { total = li.total, totalLabel = li.totalLabel };
             return Json(data);
         }/**/
        private static double? reCalcSuvenir(ListItem li)
        {
            //MainPage.db.SubmitChanges();
            if (li.tiraz.HasValue)// && li.tiraz.HasValue && li.cebest.HasValue)
            {
                double tiraz = li.tiraz.Value;
                double sum = 0;
                if (li.vid1.HasValue )//&& (li.param11.HasValue || li.vid1==28) && (li.param12.HasValue || li.vid1 == 201 || li.vid1 == 205 || li.vid1 == 28) && (li.param13.HasValue || li.vid1 != 28))
                    sum += calcSuvenirEd(li,li.tiraz.Value, li.vid1.Value, li.param11?? 0, li.param12 ?? 0, li.param13, li.param14,li.param15,li.param24, li.param25, li.param34,
                        li.param21 ?? 0, li.param22 ?? 0, li.param31 ?? 0);

                if (li.vid2.HasValue )//&& (li.param21.HasValue || li.vid2==28) && (li.param22.HasValue || li.vid2 == 201 || li.vid2 == 205 || li.vid2 == 28) && (li.param23.HasValue || li.vid2 != 28))
                    sum += calcSuvenirEd(li,li.tiraz.Value, li.vid2.Value, li.param21?? 0, li.param22 ?? 0, li.param23, li.param24,li.param25);

                if (li.vid3.HasValue )//&& (li.param31.HasValue || li.vid3==28) && (li.param32.HasValue || li.vid3 == 201 || li.vid3 == 205 || li.vid3 == 28) && (li.param33.HasValue || li.vid3 != 28))
                    sum += calcSuvenirEd(li,li.tiraz.Value, li.vid3.Value, li.param31?? 0, li.param32 ?? 0, li.param33, li.param34,li.param35);

                if (li.vid4.HasValue )//&& (li.param41.HasValue || li.vid4==28) && (li.param42.HasValue || li.vid4 == 201 || li.vid4 == 205 || li.vid4 == 28) && (li.param43.HasValue || li.vid4 != 28))
                    sum += calcSuvenirEd(li,li.tiraz.Value, li.vid4.Value, li.param41?? 0, li.param42 ?? 0, li.param43, li.param44,li.param45);

                sum = AddDopsNacenk(sum, li.listId, li.tipProd);
                sum = Math.Ceiling(sum / tiraz) * tiraz;
                li.total = sum;
                li.totalLabel = string.Format("за ед.: {0:#0.00} р., всего: {1:#0.00} р.", sum / tiraz, sum);
            }
            else { li.total = null; li.totalLabel = "за ед.: - р., всего: - р."; }
            return li.total;
        }
        private static double calcSuvenirEd(ListItem li,double tiraz, int vid1, int param1, int param2, double? param3,bool param4, bool param5, bool param6=false, bool param7=false, bool param8=false,
            int param21=0, int param22=0, int param31=0)
        {
           // if (param1 == 0 || !new int[] { 27, 28, 30 }.Contains(vid1) && param2 == 0) return 0;            
            var db = new kvotaEntities();
            double ret = 0, cena;
            int minTiraz;
            int[] maxTirazi = new int[2];
            int param;
            switch (vid1)
            {
                case 30://Тиснение
                    if (param2== 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param2 select p.tiraz).Min();
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz;
if (param5==true)ret += cena * tiraz;//с фольгой
                    if (param4==false)//клише
                    {
                        var klishDiap = param2 - 179;
                        while (klishDiap >= 4) klishDiap -= 4;
                        switch (klishDiap)
                        {
                            case 0: ret += 1500;  break;
                            case 1: ret += 3000 ;  break;
                            case 2: ret +=  12000;  break;
                            case 3: ret += 24000 ;  break;
                        }

                    }
                    break;
                case 23: //Шелкография
                    if (param2 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param2 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param2 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;

                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz*((param4 && param1 != 158)?1.5:1)*(param5?1.5:1);
                    //if (param4 && param1 != 158) ret+= cena * tiraz*0.5;
                    //if (param5) ret+= cena * tiraz*0.2;
                    break;
                case 26: //УФ печать 
                    if (param1 == 0 || param1==172 && param2 ==0) return 0;
                    param = param1;
                    if (param2>0)param = param2;
 minTiraz = (from p in db.Price where p.catId == param select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;

                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                     ret += cena * tiraz;                    
                    break;
                case 24: //Тампопечать
                    if (param2 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param2 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param2 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz ;
                    break;
                case 25: //Сублимационная печать кружек
                    param = 164;
                    minTiraz = (from p in db.Price where p.catId == param select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz;
                    break;
                case 28: //Гравировка
                    if (param3.HasValue== false) return 0;
                    if (param3 < 2) param3 = 2;
                     ret += 450.0+12.75*param3.Value * tiraz;
                    break;

                case 29: //Пакеты
                    if (param1 == 0 || param2 == 0) return 0;
                    cena = (from p in db.Price where p.catId == param1 select p.cena).First();
                    ret += cena * tiraz;

                    minTiraz = (from p in db.Price where p.catId == param2 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param2 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz;
                    break;
                case 201: //значки
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz;
                    break;
                case 205: //dtg
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz*(param4 ? 2: 1);
                    break;

                case 211:// Квартальные календари                  
                    param = 416;
                    minTiraz = (from p in db.Price where p.catId == param select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    if (param2>0) cena += param2 * 135;
                    if (param4)
                    {
                        if (tiraz>=300) cena += 285;
                        else cena += 300;
                    }
                    ret += cena * tiraz;
                    break;
                case 212:// Ленты для бейджей(ланъярды)
                    if (param2 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param2 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param2 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;

                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    if (param4) cena += 12;
                    if (param5) cena += 12;
                    if (param6) cena += 12;
                    ret += tiraz *cena ;
                    break;
                case 213:// Открытки
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    if (param4) cena += 2;
                    if (param5) cena += 6;
                    if (param6)
                    {
                        if (tiraz>=300) cena += 9.8;
                        else if (tiraz >= 200) cena += 11.3;
                        else if (tiraz >= 100) cena += 12;
                        else cena += 12.8;
                    }
                    if (param7) cena += 9.8;
                    ret += cena * tiraz;
                    break;
                case 214:// Пакеты бумажные
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz;
                    break;
                case 215://   Прочая полиграфия
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    ret += cena * tiraz;
                    break;

                case 216://  Силиконовые браслеты
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    if (param1 == 229)
                    {
                        if (param4) cena += 7;
                        if (param5) cena += 7;
                        if (param6) cena += 4;
                        if (param7) cena += 4;
                    }
                    else
                    {
                        if (param4) cena += 6;
                        if (param5) cena += 6;
                        if (param6) cena += 4;
                        if (param7) cena += 4;
                        if (param8) cena += 6;
                    }
                    ret += cena * tiraz;
                    break;

                case 217://   Слэп браслеты
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                 
                        if (param4) cena += 12;
                        if (param5) cena += 4;
                    if (param1 == 231)
                    {
                        if (param6) cena += 25;
                    }
                    ret += cena * tiraz;
                    break;
                case 218: //Футболка с сублимацией
                    if (param1 == 0 
                       // || (param2 + param21+param22+param31)==0
                        ) return 0;
                    if (param1 == 165) cena = 320;
                    else cena = 270;

                    List<int> sizes = new List<int>() { param2 , param21 , param22 , param31 };
                    sizes.RemoveAll(pp => pp == 0);
                    foreach (var si in sizes)
                    {
                        double cenaEd = 0;
                        param = si;
                        minTiraz = (from p in db.Price where p.catId == param select p.tiraz).Min();
                        if (li.askBetterPrice == false)
                        {
                            maxTirazi = (from p in db.Price where p.catId == param orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                            if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                        }
                        if (tiraz < minTiraz)
                        {
                            cenaEd = (from p in db.Price where p.catId == param && p.tiraz == minTiraz select p.cena).First();
                            cenaEd = cenaEd * minTiraz / tiraz;
                        }
                        else
                            cenaEd = (from p in db.Price where p.catId == param && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                        cena += cenaEd;
                    }
                    ret += cena * tiraz;
                    break;
                case 219://  Шары с логотипом
                    if (param1 == 0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param1 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param1 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param1 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();

                    if (param4) cena += 1.5;
                    if (param5) cena += 4;
                    ret += cena * tiraz;
                    break;
                case 269://  Диски
                    if (param1 == 0 || param2==0) return 0;
                    minTiraz = (from p in db.Price where p.catId == param2 select p.tiraz).Min();
                    maxTirazi = (from p in db.Price where p.catId == param2 orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                    if (tiraz > 2 * maxTirazi[0] - maxTirazi[1]) li.askBetterPrice = true;
                    if (tiraz < minTiraz)
                    {
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz == minTiraz select p.cena).First();
                        cena = cena * minTiraz / tiraz;
                    }
                    else
                        cena = (from p in db.Price where p.catId == param2 && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();
                    switch (param21)
                    {
                        case 1:case 3:case 4: cena += 26 ; break;
                        case 2: cena += 32 ; break;
                        case 5: cena += 28 ; break;
                        case 6: cena +=  36; break;
                        case 7: cena +=  32; break;
                        case 8: cena +=  42; break;
                        case 9: case 10:cena +=  6; break;
                    }
 switch (param22)
                    {
                        case 1: cena += 4; break;
                        case 2: cena += 6; break;
                        case 3: cena += 10; break;
                    }

                    ret += cena * tiraz;
                    break;

            }
            return ret;
        }


        private static double? reCalcPoligrafiya(ListItem li)
        {
            if (li.pRazm.HasValue && li.pCvet.HasValue && li.pPlotn.HasValue && li.tiraz.HasValue)
            {
                var db = new kvotaEntities();
                double tiraz = li.tiraz.Value,
                    razm = li.pRazm.Value;
                double sum = 0;

                double cena;
                cena = (from p in db.Price where p.catId == razm && p.tiraz == li.pCvet.Value select p.cena).First();
                sum += cena * tiraz;
                cena = (from p in db.Price where p.catId == razm && p.tiraz == li.pPlotn.Value select p.cena).First();
                sum += cena * tiraz;
                if (li.pLamin.HasValue && li.pLamin > 0)
                {
                    cena = (from p in db.Price where p.catId == razm && p.tiraz == li.pLamin.Value select p.cena).First();
                    sum += cena * tiraz;
                }
                if (li.pBigov.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 700 select p.cena).First();
                    sum += cena * li.pBigov.Value * li.tiraz.Value;
                }
                if (li.pDirk.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 701 select p.cena).First();
                    sum += cena * li.pDirk.Value * li.tiraz.Value;
                }
                sum = sum * 1.07;

                sum = AddDopsNacenk(sum, li.listId,li.tipProd);
                sum = Math.Ceiling(sum / tiraz) * tiraz;
                li.total = sum;
                li.totalLabel = string.Format("за ед.: {0:#0.00} р., всего: {1:#0.00} р.", sum / tiraz, sum);
            }
            else { li.total = null; li.totalLabel = "за ед.: - р., всего: - р."; }
            return li.total;
        }

        private static double? reCalcBanner(ListItem li)
        {
            if (li.bVid>0 && li.bMat>0 && li.bDpi>0 && li.tiraz>0)
            {
                var db = new kvotaEntities();
                double tiraz = li.tiraz.Value;
                double sum = 0;

                double cena;

                if (tiraz < 1) tiraz = 1;
                cena = (from p in db.Price where p.catId == li.bMat && p.tiraz==li.bDpi select p.cena).First();
                sum += cena * tiraz;

                if (li.bPost1.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 600 select p.cena).First();
                    sum += cena * li.bPost1.Value;
                }
                if (li.bPost2.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 601 select p.cena).First();
                    sum += cena * li.bPost2.Value;
                }
                if (li.bPost3.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 602 select p.cena).First();
                    sum += cena * li.bPost3.Value;
                }
                if (li.bPost4.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 603 select p.cena).First();
                    sum += cena * li.bPost4.Value;
                }
                if (li.bPost5.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 604 select p.cena).First();
                    sum += cena * li.bPost5.Value;
                }
                if (li.bPost6.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 605 select p.cena).First();
                    sum += cena * li.bPost6.Value;
                }
                if (li.bPost7.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 606 select p.cena).First();
                    sum += cena * li.bPost7.Value;
                }
                if (li.bPost8.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 607 select p.cena).First();
                    sum += cena * li.bPost8.Value;
                }
                if (li.bPost9.HasValue)
                {
                    cena = (from p in db.Price where p.catId == 608 select p.cena).First();
                    sum += cena * li.bPost9.Value;
                }

                sum = AddDopsNacenk(sum, li.listId,li.tipProd);
                sum = Math.Ceiling(sum / tiraz) * tiraz;
                li.total = sum;
                li.totalLabel = string.Format("за ед.: {0:#0.00} р., всего: {1:#0.00} р.", sum / tiraz, sum);
            }
            else { li.total = null; li.totalLabel = "за ед.: - р., всего: - р."; }
            //var data = new { total = li.total, totalLabel = li.totalLabel };
            //return Json(data);
            return li.total;
        }

        public static double AddDopsNacenk(double sum, int? listId,int tipProd)
        {
            var db = new kvotaEntities();
            var z = db.Zakaz.First(pp => pp.id == listId);
            //+доп.услуги+доп.траты)*наценка
            //if (z.dopUslDost) sum += 400;
            //if (z.dopUslMaket) sum += 400;
            if (z.dopTrat.HasValue) sum += z.dopTrat.Value;

            switch (z.nacenTip)
            {
                case 1://стандарт
                    if (tipProd == 3) sum *= 1.4;
                    else sum *= 1.0;
                    break;
                /*case 2:
                    if (tipProd == 1) sum *= 1.2;
                    else sum *= 1.6;
                    break;*/
                case 3://своя
                    if (z.nacenValue.HasValue) sum *= z.nacenValue.Value; break;
            }
            return sum;
        }
    }
}