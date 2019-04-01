using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KvotaWeb.Models.Items
{
    public class Blocknot
    {
         public int Id { get; set; }

        [Display(Name = "Тираж")]
        public int Tiraz { get; set; }

         public int Format { get; set; }
        public int Pechat { get; set; }
        public int Plotnost { get; set; }
        public bool PostPechat { get; set; }
        public string TotalLabel { get; set; }
        
    }
}