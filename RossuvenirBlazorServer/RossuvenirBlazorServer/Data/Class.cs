using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RossuvenirBlazorServer.Data
{

    public enum TipProds : int
    {DTG = 18, Gravirovka = 28,
        Znachok = 4, Shelkografiya = 23, Tampopechat = 24, PaketPvd = 29, Tisnenie = 30,  UFkachestvo = 31
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

        protected ItemBase(TipProds tipProd)//, string viewName)
        {
            TipProd = tipProd;
            // ViewName = viewName;
        }
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public TipProds TipProd { get; set; }
    }
    public class Dtg : ItemBase
    {
        public Dtg() : base(TipProds.DTG)
        {

        }
    }
    public class Gravirovka : ItemBase
    {
        public int Tiraz{get;set;}
        public Gravirovka() : base(TipProds.Gravirovka)
        {

        }
    }
}
