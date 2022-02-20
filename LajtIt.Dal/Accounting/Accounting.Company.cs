using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal.DbHelper
{
      public partial class Accounting
    {
        public static Company GetCompany(int companyId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Company.Where(x => x.CompanyId == companyId).FirstOrDefault();
            }
        }

        public static List<Company> GetCompanies(string query, int limit)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Company.Where(x => x.Name.Contains(query) || x.BankName.Contains(query)).Take(limit).ToList();
            }
        }
        public static List<Dal.CashRegister> GetCashRegisters()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.CashRegister.ToList();
            }
        }
        public static CashRegister GetCashRegister(int id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.CashRegister.Where(x => x.Id == id).FirstOrDefault();
            }
        }
    }
}
