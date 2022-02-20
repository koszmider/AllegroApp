using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class InventoryHelper
    {
        public void SetInventory(Dal.Inventory i)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Inventory.InsertOnSubmit(i);
                ctx.SubmitChanges();
            }

        }
        public List<InventoryView> GetInventory()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.InventoryView.ToList();
            }
        }
        public List<InventorySummaryResult> GetInventorySummary(int year)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.InventorySummary(year).ToList();
            }
        }

        public void SetInventoryDelete(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Inventory i = ctx.Inventory.Where(x => x.Id == id).FirstOrDefault();

                ctx.Inventory.DeleteOnSubmit(i);
                ctx.SubmitChanges();
            }
        }
    }
}
