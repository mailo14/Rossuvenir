//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KvotaWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Zakaz
    {
        public int id { get; set; }
        public Nullable<int> num { get; set; }
        public string comment { get; set; }
        public Nullable<double> total { get; set; }
        public bool dopUslDost { get; set; }
        public bool dopUslMaket { get; set; }
        public Nullable<int> nacenTip { get; set; }
        public Nullable<double> nacenValue { get; set; }
        public Nullable<double> dopTrat { get; set; }
        public int userId { get; set; }
        public System.DateTime dat { get; set; }
        public string userName { get; set; }
    }
}
