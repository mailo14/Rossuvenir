using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KvotaWeb.Models.Items
{
    public class MultiSuvenir : ItemBase
    {
        public List<ItemBase> Items { get; set; }//= new List<ItemBase>() {/* new Znachok(), */new FlagNSO(), new FlagNSO() };
        
        public override string Srok { get; set ; }

        public override ListItem ToListItem()
        {
            var rr = base.ToListItem();
            return rr;
        }

        public static ItemBase CreateItem(ListItem li)
        {
            var ret= new MultiSuvenir()
            {
                Id = li.id,
                ZakazId = li.listId,
                Tiraz = li.tiraz,
                Items= new kvotaEntities().ListItem.Where(pp=>pp.parentId==li.id).ToList().Select(pp=>ItemBase.Create(pp)).ToList()
            };

            return ret;
        }

        public override List<CalcLine> Calc()
        {
            kvotaEntities db = new kvotaEntities();
            var subs = db.ListItem.Where(pp => pp.parentId == Id).ToList();
            var subCalcs = new List<List<CalcLine>>();
            foreach (var sub in subs)
            {
                
                   var item = ItemBase.Create(sub);
                item.InnerMessageIds.Clear();
                subCalcs.Add(item.Calc(Tiraz));
                foreach (var im in item.InnerMessageIds) InnerMessageIds.Add(im); 
            }

            var ret = new List<CalcLine>();
            int k = -1;
            foreach (Postavs i in Enum.GetValues(typeof(Postavs)))
            {
                k++;
                var line = new CalcLine() { Postav = i };
                ret.Add(line);
                var srez = subCalcs.Select(pp => pp[k]).ToArray();
                if (srez.All(pp => pp.Cena != null))
                    line.Cena = srez.Sum(pp => pp.Cena);
            }
            return ret;
        }

        public MultiSuvenir() : base(TipProds.MultiSuvenir, "EditMultiSuvenir")
        {
            var empty = new SelectList(new List<Category>(), "id", "tip"); //Enumerable.Empty<SelectListItem>();
            var nullObj = new Category() { tip = "(не выбрано)" };

            kvotaEntities db = new kvotaEntities();
            ViewData = new ViewDataDictionary();
            
        }
    }

}