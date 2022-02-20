using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
    public partial class ProductCatalog
    {
        public static List<Country> GetCountries()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Country.ToList();
            }
        }
    }
}
