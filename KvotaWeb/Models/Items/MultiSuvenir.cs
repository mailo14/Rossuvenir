using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class MultiSuvenir
    {
        public List<ItemBase> Items { get; set; } = new List<ItemBase>() {/* new Znachok(), */new FlagNSO(), new FlagNSO() };


        [Display(Name = "Тираж:")]
        public double? Tiraz { get; set; }

    }

}