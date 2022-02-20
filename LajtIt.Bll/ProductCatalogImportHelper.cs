using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public class ProductCatalogImportHelper
    {
        public List<ProductCatalogImport> BindImports()
        {
            Dal.ProductCatalogImportHelper h = new Dal.ProductCatalogImportHelper();
            return h.GetImports();
        }
    }
}
