using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll
{
    public class ProductCatalogCalculator1
    {
        #region priv
        private const decimal VAT = 0.23M;
        private decimal purchasePriceNetto;
        private decimal purchaseRealPriceNetto;
        private decimal? fixedSellPriceNetto;
        private decimal rebate;
        private decimal margin;
        private decimal sellPriceCalculatedNetto;
        private decimal sellPriceNetto;
        private decimal allegroShopCostNetto;
        //private decimal allegroCostNetto;
        private decimal incomeNetto;
        #endregion

        public decimal PurchasePriceNetto { get { return purchasePriceNetto; } }
        public decimal PurchaseRealPriceNetto { get { return purchaseRealPriceNetto; } }
        public decimal? FixedSellPriceNetto { get { return fixedSellPriceNetto; } }
        public decimal SellPriceCalculatedNetto { get { return sellPriceCalculatedNetto; } }
        public decimal SellPriceNetto { get { return sellPriceNetto; } }
        public decimal AllegroShopCostNetto { get { return allegroShopCostNetto; } }
        //public decimal AllegroCostNetto { get { return allegroCostNetto; } }
        public decimal IncomeNetto { get { return incomeNetto; } }
        public decimal IncomeRate { get { return sellPriceNetto == 0 ? 0 : incomeNetto * 100M / sellPriceNetto; } }
        public decimal IncomeRate2 { get { return sellPriceNetto == 0 ? 0 : incomeNetto * 100M / (purchasePriceNetto + allegroShopCostNetto); } }

        private decimal VATRate { get { return 1 + VAT; } }
        public decimal PurchasePriceBrutto { get { return purchasePriceNetto * VATRate; } }
        public decimal PurchaseRealPriceBrutto { get { return purchaseRealPriceNetto * VATRate; } }
        public decimal? FixedSellPriceBrutto { get { return fixedSellPriceNetto * VATRate; } }
        public decimal SellPriceCalculatedBrutto { get { return sellPriceCalculatedNetto * VATRate; } }
        public decimal SellPriceBrutto { get { return sellPriceNetto * VATRate; } }
        public decimal AllegroShopCostBrutto { get { return allegroShopCostNetto * VATRate; } }
        //public decimal AllegroCostBrutto { get { return allegroCostNetto * VATRate; } }
        public decimal IncomeBrutto { get { return incomeNetto * VATRate; } }
        public bool IsPriceFixed { get { return fixedSellPriceNetto.HasValue; } }

        //public ProductCatalogCalculator(int productCatalogId)
        //{
        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    Dal.ProductCatalog pc = oh.GetProductCatalog(productCatalogId);

        //    decimal _rebate = pc.Supplier.Rebate;
        //    decimal _margin = pc.Supplier.Margin;

        //    if (pc.Rebate.HasValue)
        //        _rebate = pc.Rebate.Value;
        //    if (pc.Margin.HasValue)
        //        _margin = pc.Margin.Value;

        //    Calculate(pc.PurchasePrice, pc.AllegroPrice, _rebate, _margin);

        //}
        public ProductCatalogCalculator1(decimal _purchasePriceNetto, decimal? _fixedSellPriceNetto, decimal? _rebate, decimal? _margin)
        {
            Calculate(_purchasePriceNetto, _fixedSellPriceNetto, _rebate??0, _margin??0);
        }

        private void Calculate(decimal _purchasePriceNetto, decimal? _fixedSellPriceNetto, decimal _rebate, decimal _margin)
        {
            this.purchasePriceNetto = _purchasePriceNetto;
            this.rebate = _rebate;
            this.margin = _margin;
            this.fixedSellPriceNetto = _fixedSellPriceNetto;

            this.purchaseRealPriceNetto = purchasePriceNetto * (1 - rebate);
            this.sellPriceCalculatedNetto = purchaseRealPriceNetto * (1 + margin);

            if (this.fixedSellPriceNetto.HasValue)
                this.sellPriceNetto = this.fixedSellPriceNetto.Value;
            else
                this.sellPriceNetto = this.sellPriceCalculatedNetto;

            this.allegroShopCostNetto = CalculateShopCostNetto(this.sellPriceNetto * (1 + VAT));
            this.incomeNetto = this.sellPriceNetto - this.purchaseRealPriceNetto - this.allegroShopCostNetto;

        }
        #region statyczne metody

        public static decimal CalculateShopCostNetto(decimal brutto)
        {
            decimal total = 0;

            if (brutto <= 5000)
                total = (brutto - 1000) * 0.03M + 63;
            else
                total = (brutto - 5000) * 0.01M + 183;

            if (brutto <= 1000)
                total = (brutto - 100) * 0.06M + 9;
            if (brutto <= 100)
                total = brutto * 0.09M;


            return total / (1+VAT);
        }
        #endregion

        public static decimal BruttoValue2(decimal nettoValue)
        {
           
             return nettoValue * (1 + VAT);
        }

        public static decimal NettoValue(decimal bruttoValue)
        {

            return bruttoValue / VAT;
        }
        public decimal ChangePrice(decimal brutto, int? priceAddType, int? priceAddValueType, decimal? priceValue)
        {
            decimal result = brutto;
     
            if (priceAddType == 0 ||
                !priceAddType.HasValue ||
                !priceAddValueType.HasValue ||
                !priceValue.HasValue)
            { return result; }

            if (priceAddType == 1)
            {
                if (priceAddValueType == 1)
                    result += priceValue.Value;
                else
                    result = result * (1 + priceValue.Value / 100);
            }
            if (priceAddType == 2)
            {
                if (priceAddValueType == 1)
                    result -= priceValue.Value;
                else
                    result = result * (1 - priceValue.Value / 100);
            }


            return result;
        }
    }
}
