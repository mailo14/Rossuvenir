using KvotaWeb.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.ViewModels
{
    public class SvetootrazatelOneValueVM
    {
        public ItemBase Source { get; set; }
        public string Srok { get; set; }
        public int? ZakazId { get; set; }
        public string TotalLabel { get; set; }

        public int Id { get; set; }
        public TipProds TipProd { get; set; }

        public ViewDataDictionary ViewData { get; set; }
        public int? Vid { get; set; }

        public double? Tiraz { get; set; }
        public  string Description { get; set; }
    }
}