using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Dal
{
   public  class PaczkomatyHelper
    {
       public void SetPaczkomaty(List<Paczkomaty> paczkomaty, string actingUser)
       {
           using (LajtitDB ctx = new LajtitDB())
           {
               ctx.Paczkomaty.DeleteAllOnSubmit(ctx.Paczkomaty);
               //foreach (Paczkomaty p in paczkomaty)
               //{
               //    try
               //    {
               //        ctx.Paczkomaties.InsertOnSubmit(p);
                       ctx.Paczkomaty.InsertAllOnSubmit(paczkomaty);
                       ctx.SubmitChanges();
                   //}
                   //catch (Exception ex)
                   //{
                   //    throw ex;
                   //}
               //}
           }
       }

        public List<PaczkomatyView> GetPaczkomaty()
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.PaczkomatyView.OrderBy(x => x.Name).ToList();
            }
        }
        public List<PaczkomatyView> GetPaczkomaty(string searchText)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.PaczkomatyView
                    .Where(x=>x.Name.Contains(searchText) || x.Description.Contains(searchText))
                    .OrderBy(x => x.Name).ToList();
            }
        }
    }
}
