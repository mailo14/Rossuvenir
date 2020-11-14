﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class Znachok: ItemBase
    {

        [Display(Name = "Размер:")]
         public int? Razmer { get; set; }
        public override string Srok { get; set; } = "от 3-х рабочих дней" + SrokPripiska;

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            rr.param11 = Razmer;
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            return new Znachok() { Id = li.id, ZakazId = li.listId, Tiraz = li.tiraz, Razmer = li.param11 };
        }

        public override List<CalcLine> Calc()

        {
            var ret = new List<CalcLine>();
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                if (Razmer == null || Tiraz == null) continue;

                decimal cena;
                if (TryGetPrice( i,Tiraz,Razmer,out cena) == false) continue;

                
                line.Cena = cena * (decimal)Tiraz.Value;
            }
            return ret;

        }


        public Znachok():base( TipProds.Znachok, "EditЗначки")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            ViewData["params1"] = new SelectList((from pp in db.Category where pp.parentId == 414 select pp), "id", "tip");
        }
    }

    public class CalcLine
    {
        public Postavs Postav { get; set; }
        public decimal? Cena { get; set; }
    }
    public class TotalCalcLine
    {
        public string Postav { get; set; }
        public string EdCena { get; set; }
        public string Cena { get; set; }
        public string Marza { get; set; }
    }
  public class TotalResultsModel
    {
        public List<TotalCalcLine> Lines { get; set; }
        public string Message { get; set; }
    }

    public enum Postavs:int { РРЦ_1_5=1, АртСувенир=2, ААА=3, Плановая_СС=10};
    public enum TipProds : int
    {
        Znachok = 4, Shelkografiya = 23, Tampopechat = 24, PaketPvd = 29, Tisnenie = 30, DTG = 18, Gravirovka = 28, UFkachestvo = 31
            , UFstandart = 32, Decol = 33, BumajniiPaket = 9, Flag = 34
            , Banner = 3,
        FlagPobedi = 35,
        FlagNSO = 36,
        Vimpel = 37,
        Skatert = 38,
        Sharf = 39,
        Platok = 40,
        ReklNakidka = 41,
        SportNomer = 42,
        Futbolka = 43,
        SlapChasi = 44,
        Svetootrazatel = 45,
        SvetootrazatelNaklei = 46,
        KontrBraslet = 47,
        SlapBraslet = 48,
        Lenta = 49,
        SiliconBraslet = 50,
        MultiSuvenir = 51
    };
    public abstract class ItemBase
    {
        public const string SrokPripiska = " (+2 рабочих дня на проверку и сборку)";
        public int? ZakazId { get; set; }
        public string TotalLabel { get; set; }

        protected ItemBase(TipProds tipProd,string viewName)
        {            
               TipProd = tipProd;
            ViewName = viewName;
        }

        public int Id { get; set; }
        public int? ParentId { get; set; }
        public TipProds TipProd { get; set; }

        public ViewDataDictionary ViewData { get; set; }

        [Display(Name = "Тираж:")]
        public double? Tiraz { get; set; }
        public string ViewName { get; set; }
        public abstract string Srok { get; set; }
        public virtual string Description { get; set; }
        public virtual string PicturePath { get; set; } = "";

        public virtual ListItem ToListItem()
        {
            return new ListItem()
            {
                vid1 = (int)TipProd,
                tipProd = (int)TipProd,
                id = Id,
                tiraz = Tiraz,
                    listId = ZakazId,
                parentId= ParentId
            };
        }

        public string GetExistImageUrl(Type type, int? param1, int? param2)
        {
            string imageUrl;
            if (param1.HasValue)
            {
                string dir = $@"/Images/products/{type.Name}/";
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath(imageUrl = dir + param1 + ".png"))
                    || param2.HasValue && System.IO.File.Exists(HttpContext.Current.Server.MapPath(imageUrl = dir + param2 + ".png")))
                    return imageUrl;
            }
            return "";
        }

        public bool TryGetPrice(Postavs firma, double? dTiraz, int? catId, out decimal cena)
        {
            cena = 0;
            var tiraz = (decimal)dTiraz;
            if (tiraz == 0) return false;

            if (firma == Postavs.Плановая_СС || firma == Postavs.РРЦ_1_5)
            {
                bool is1_5 = false; if (firma == Postavs.РРЦ_1_5) { is1_5 = true; firma = Postavs.Плановая_СС; }
                kvotaEntities db = new kvotaEntities();
                var minTiraz = (from p in db.Price where p.firma == (int)firma && p.catId == catId orderby p.tiraz select (int?)p.tiraz).FirstOrDefault();
                if (!minTiraz.HasValue) return false;

                var maxTirazi = (from p in db.Price where p.firma == (int)firma && p.catId == catId orderby p.tiraz descending select p.tiraz).Take(2).ToArray();
                if (maxTirazi.Length == 2 && tiraz > 2 * maxTirazi[0] - maxTirazi[1]) InnerMessageIds.Add(InnerMessages.AskBetterPrice);
                if (tiraz < minTiraz)
                {
                    cena = (decimal)(from p in db.Price where p.firma == (int)firma && p.catId == catId && p.tiraz == minTiraz select p.cena).First();
                    cena = cena * minTiraz.Value / tiraz;
                }
                else
                    cena = (decimal)(from p in db.Price where p.firma == (int)firma && p.catId == catId && p.tiraz <= tiraz orderby p.tiraz descending select p.cena).First();

                if (is1_5) cena *= 1.5m;
                return true;
            }
            return false;
        }

        public decimal TryGetSingleParam(int paramId,Postavs firma=Postavs.Плановая_СС)
        {
            kvotaEntities db = new kvotaEntities();
            return (decimal)(from p in db.Price
                    where p.firma == (int)firma && p.catId == paramId
                    select p.cena).Single();
        }

        public enum InnerMessages
        {
            AskBetterPrice,
            IndividPrice
        }
        public HashSet<InnerMessages> InnerMessageIds = new HashSet<InnerMessages>();
        public string Message
        {
            get
            {
                return
                    string.Join(Environment.NewLine,
                    InnerMessageIds.Select(pp =>
                    {
                        switch (pp)
                        {
                            case InnerMessages.AskBetterPrice: return @"*Для получения лучшей цены обратитесь в Отдел реализации";
                            case InnerMessages.IndividPrice: return @"*Расчет цены индивидуально по макету";
                            default: return null;
                        };
                    }
                ));
            }
        }
        public abstract List<CalcLine> Calc();
        public List<CalcLine> Calc(double? tiraz)
        {
            Tiraz = tiraz;
            return Calc();
        }
        public  List<CalcLine> CalcWithNacenkaAndRounded()
        {
var lines = Calc();
            var nacenk = GetNacenk(ZakazId, (int)TipProd);

            foreach (var line in lines)
            if (line.Cena!=null) line.Cena= Math.Ceiling(line.Cena.Value*nacenk / (decimal)Tiraz.Value) * (decimal)Tiraz.Value;
            return lines;
        }
        public decimal GetNacenk(int? listId, int tipProd)
            {
                var db = new kvotaEntities();
            if (listId.HasValue)
            {//костыль
                var z = db.Zakaz.First(pp => pp.id == listId);
                //+доп.услуги+доп.траты)*наценка
                //if (z.dopUslDost) sum += 400;
                //if (z.dopUslMaket) sum += 400;
                //if (z.dopTrat.HasValue) sum += z.dopTrat.Value;
                switch (z.nacenTip)
                {
                    case 1://стандарт
                        if (tipProd == 3) return 1.4m;
                        break;
                    //    else return 1.0;
                    case 3://своя
                        if (z.nacenValue.HasValue) return (decimal)z.nacenValue.Value;
                        break;
                }
            }
                return 1;
            }
        public  TotalResultsModel GetTotal()
        {
            InnerMessageIds.Clear();
            //List<CalcLine> initlines,double tiraz
            var lines = CalcWithNacenkaAndRounded();//initlines.ToList();
            var baseLine = lines.First(pp => pp.Postav == Postavs.Плановая_СС);

            var ret = new TotalResultsModel()
            {
                Lines = new List<TotalCalcLine>(),
                Message = Message
            };
            foreach (var line in lines)
            {
                var tLine = new TotalCalcLine();
                ret.Lines.Add(tLine);
                switch (line.Postav)
                {
                    case Postavs.РРЦ_1_5:
                        tLine.Postav = "РРЦ 1,5";break;
                    case Postavs.АртСувенир:
                        tLine.Postav = "АртСУВЕНИР"; break;
                    case Postavs.ААА:
                        tLine.Postav = "ААА"; break;
                    case Postavs.Плановая_СС:
                        tLine.Postav = "Плановая СС"; break;
                }
                if (line.Cena.HasValue)
                {
                    tLine.EdCena = (line.Cena.Value / (decimal)Tiraz.Value).ToString("f2");
                    tLine.Cena = line.Cena.Value.ToString("f2");
                    if (line != baseLine && baseLine.Cena!=null && baseLine.Cena != 0)
                        tLine.Marza = ((line.Cena - baseLine.Cena) / line.Cena * 100).Value.ToString("f2") + @"%";
                    else tLine.Marza = "-";
                }
                else tLine.EdCena = tLine.Cena = tLine.Marza = "-";
            }
            return ret;
        }
        public static ItemBase Create(ListItem li)
        {
            switch ((TipProds)li.tipProd)
            {
                case TipProds.Znachok:
                    return Znachok.CreateItem(li);

                case TipProds.Shelkografiya:
                    return Shelkografiya.CreateItem(li);                                 
  case TipProds.Tampopechat:
                    return Tampopechat.CreateItem(li);
                case TipProds.PaketPvd:
                    return PaketPvd.CreateItem(li);
                case TipProds.Tisnenie:
                    return Tisnenie.CreateItem(li);
                case TipProds.DTG:
                    return DTG.CreateItem(li);
                case TipProds.Gravirovka:
                    return Gravirovka.CreateItem(li);
                case TipProds.UFkachestvo:
                    return UFkachestvo.CreateItem(li);
                case TipProds.UFstandart:
                    return UFstandart.CreateItem(li);
                case TipProds.Decol:
                    return Decol.CreateItem(li);

                case TipProds.BumajniiPaket:
                    return BumajniiPaket.CreateItem(li);
                case TipProds.Flag:
                    return Flag.CreateItem(li);
                case TipProds.FlagPobedi:
                    return FlagPobedi.CreateItem(li);
                case TipProds.FlagNSO:
                    return FlagNSO.CreateItem(li);
                case TipProds.Vimpel:
                    return Vimpel.CreateItem(li);
                case TipProds.Skatert:
                    return Skatert.CreateItem(li);
                case TipProds.Sharf:
                    return Sharf.CreateItem(li);
                case TipProds.Platok:
                    return Platok.CreateItem(li);
                case TipProds.ReklNakidka:
                    return ReklNakidka.CreateItem(li);
                case TipProds.SportNomer:
                    return SportNomer.CreateItem(li);
                case TipProds.Futbolka:
                    return Futbolka.CreateItem(li);
                case TipProds.SlapChasi:
                    return SlapChasi.CreateItem(li);
                case TipProds.Svetootrazatel:
                    return Svetootrazatel.CreateItem(li);
                case TipProds.KontrBraslet:
                    return KontrBraslet.CreateItem(li);
                case TipProds.SlapBraslet:
                    return SlapBraslet.CreateItem(li);
                case TipProds.Lenta:
                    return Lenta.CreateItem(li);
                case TipProds.SiliconBraslet:
                    return SiliconBraslet.CreateItem(li);
                case TipProds.MultiSuvenir:
                    return MultiSuvenir.CreateItem(li);

                default:
                    return null;
                 /*   var tipProd = (TipProds)li.tipProd;
                    var tipProdName = Enum.GetName(typeof(TipProds), li.tipProd);
                    var tt = GetType(tipProdName);
                    var mm = tt.GetMethod("CreateItem");
                    var rr=mm.Invoke(null, new object[] { li }) ;
                    return GetType(tipProdName).GetMethod("CreateItem").Invoke(null,new object[] { li }) as ItemBase;*/
            }
        }

        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;
            foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null)
                    return type;
            }
            return null;
        }
        public object GetInstance(string strFullyQualifiedName)
        {
            Type t = Type.GetType(strFullyQualifiedName);
            return Activator.CreateInstance(t);
        }
    }
}