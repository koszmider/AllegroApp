using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
    #region classes

    public class TokenReturnObject
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string scope { get; set; }
        public string jti { get; set; }
    }
    public class BuyNowPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class Input
    {
        public BuyNowPrice buyNowPrice { get; set; }
    }

    public class BuyNowPriceRootObject
    {
        public Input input { get; set; }
    }
    #region Promotion
    public class PromotionConfiguration
    {
        public string percentage { get; set; }
    }

    public class PromotionTrigger
    {
        public string forEachQuantity { get; set; }
        public string discountedNumber { get; set; }
    }

    public class PromotionSpecification
    {
        public string type { get; set; }
        public PromotionConfiguration configuration { get; set; }
        public PromotionTrigger trigger { get; set; }
    }

    public class PromotionBenefit
    {
        public PromotionSpecification specification { get; set; }
    }

    public class PromotionOffer
    {
        public string id { get; set; }
    }

    public class PromotionOfferCriteria
    {
        public string type { get; set; }
        public List<PromotionOffer> offers { get; set; }
    }

    public class PromotionRootObject
    {
        public List<PromotionBenefit> benefits { get; set; }
        public List<PromotionOfferCriteria> offerCriteria { get; set; }
    }
    public class PromotionListConfiguration
    {
        public string percentage { get; set; }
    }

    public class PromotionListTrigger
    {
        public int forEachQuantity { get; set; }
        public int discountedNumber { get; set; }
    }

    public class PromotionListSpecification
    {
        public string type { get; set; }
        public PromotionListConfiguration configuration { get; set; }
        public PromotionListTrigger trigger { get; set; }
    }

    public class PromotionListBenefit
    {
        public PromotionListSpecification specification { get; set; }
    }

    public class PromotionListOffer
    {
        public string id { get; set; }
        public object quantity { get; set; }
        public object promotionEntryPoint { get; set; }
    }

    public class PromotionListOfferCriteria
    {
        public string type { get; set; }
        public List<PromotionListOffer> offers { get; set; }
    }

    public class PromotionListPromotion
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public List<PromotionListBenefit> benefits { get; set; }
        public List<PromotionListOfferCriteria> offerCriteria { get; set; }
        public string status { get; set; }
    }

    public class PromotionListRootObject
    {
        public List<PromotionListPromotion> promotions { get; set; }
        public int totalCount { get; set; }
    }
    #endregion
    #region Promotion Created
    public class PromotionCreatedConfiguration
    {
        public string percentage { get; set; }
    }

    public class PromotionCreatedTrigger
    {
        public int forEachQuantity { get; set; }
        public int discountedNumber { get; set; }
    }

    public class PromotionCreatedSpecification
    {
        public string type { get; set; }
        public PromotionCreatedConfiguration configuration { get; set; }
        public PromotionCreatedTrigger trigger { get; set; }
    }

    public class PromotionCreatedBenefit
    {
        public PromotionCreatedSpecification specification { get; set; }
    }

    public class PromotionCreatedOffer
    {
        public string id { get; set; }
        public object quantity { get; set; }
        public object promotionEntryPoint { get; set; }
    }

    public class PromotionCreatedOfferCriteria
    {
        public string type { get; set; }
        public List<PromotionCreatedOffer> offers { get; set; }
    }

    public class PromotionCreatedRootObject
    {
        public string id { get; set; }
        public DateTime createdAt { get; set; }
        public List<PromotionCreatedBenefit> benefits { get; set; }
        public List<PromotionCreatedOfferCriteria> offerCriteria { get; set; }
        public string status { get; set; }
    }
    #endregion

    #region Item
    public class ItemCategory
    {
        public string id { get; set; }
    }

    public class ItemParameter
    {
        public string id { get; set; }
        public List<object> valuesIds { get; set; }
        public List<object> values { get; set; }
        public object rangeValue { get; set; }
    }

    public class ItemItem
    {
        public string type { get; set; }
        public string url { get; set; }
        public string content { get; set; }
    }

    public class ItemSection
    {
        public List<ItemItem> items { get; set; }
    }

    public class ItemDescription
    {
        public List<ItemSection> sections { get; set; }
    }

    public class ItemImage
    {
        public string url { get; set; }
    }

    public class ItemPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class ItemSellingMode
    {
        public string format { get; set; }
        public ItemPrice price { get; set; }
        public object startingPrice { get; set; }
        public object minimalPrice { get; set; }
    }

    public class ItemStock
    {
        public int available { get; set; }
        public string unit { get; set; }
    }

    public class ItemPublication
    {
        public object duration { get; set; }
        public string status { get; set; }
        public object startingAt { get; set; }
        public object endingAt { get; set; }
    }

    public class ItemDelivery
    {
        public object shippingRates { get; set; }
        public string handlingTime { get; set; }
        public string additionalInfo { get; set; }
        public object shipmentDate { get; set; }
    }

    public class ItemPayments
    {
        public string invoice { get; set; }
    }

    public class ItemImpliedWarranty
    {
        public string id { get; set; }
    }

    public class ItemReturnPolicy
    {
        public string id { get; set; }
    }

    public class ItemWarranty
    {
        public string id { get; set; }
    }

    public class ItemAfterSalesServices
    {
        public ItemImpliedWarranty impliedWarranty { get; set; }
        public ItemReturnPolicy returnPolicy { get; set; }
        public ItemWarranty warranty { get; set; }
    }

    public class ItemPromotion
    {
        public bool emphasized { get; set; }
        public bool bold { get; set; }
        public bool highlight { get; set; }
        public bool departmentPage { get; set; }
        public bool emphasizedHighlightBoldPackage { get; set; }
    }

    public class ItemLocation
    {
        public string countryCode { get; set; }
        public string province { get; set; }
        public string city { get; set; }
        public string postCode { get; set; }
    }

    public class ItemExternal
    {
        public string id { get; set; }
    }

    public class ItemValidation
    {
        public List<object> errors { get; set; }
        public DateTime validatedAt { get; set; }
    }

    public class ItemRootObject
    {
        public string id { get; set; }
        public string name { get; set; }
        public ItemCategory category { get; set; }
        public List<ItemParameter> parameters { get; set; }
        public string ean { get; set; }
        public ItemDescription description { get; set; }
        public object compatibilityList { get; set; }
        public List<ItemImage> images { get; set; }
        public ItemSellingMode sellingMode { get; set; }
        public ItemStock stock { get; set; }
        public ItemPublication publication { get; set; }
        public ItemDelivery delivery { get; set; }
        public ItemPayments payments { get; set; }
        public ItemAfterSalesServices afterSalesServices { get; set; }
        public object additionalServices { get; set; }
        public object sizeTable { get; set; }
        public ItemPromotion promotion { get; set; }
        public ItemLocation location { get; set; }
        public ItemExternal external { get; set; }
        public List<object> attachments { get; set; }
        public object contact { get; set; }
        public ItemValidation validation { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
    #endregion
    #region listing
    public class ListingSeller
    {
        public string id { get; set; }
        public bool company { get; set; }
        public bool superSeller { get; set; }
    }

    public class ListingPromotion
    {
        public bool emphasized { get; set; }
        public bool bold { get; set; }
        public bool highlight { get; set; }
    }

    public class ListingLowestPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class ListingDelivery
    {
        public bool availableForFree { get; set; }
        public ListingLowestPrice lowestPrice { get; set; }
    }

    public class ListingImage
    {
        public string url { get; set; }
    }

    public class ListingPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class ListingSellingMode
    {
        public string format { get; set; }
        public ListingPrice price { get; set; }
        public int popularity { get; set; }
    }

    public class ListingStock
    {
        public string unit { get; set; }
        public int available { get; set; }
    }

    public class ListingCategory
    {
        public string id { get; set; }
    }

    public class ListingRegular
    {
        public string id { get; set; }
        public string name { get; set; }
        public ListingSeller seller { get; set; }
        public ListingPromotion promotion { get; set; }
        public ListingDelivery delivery { get; set; }
        public List<ListingImage> images { get; set; }
        public ListingSellingMode sellingMode { get; set; }
        public ListingStock stock { get; set; }
        public ListingCategory category { get; set; }
    }

    public class ListingItems
    {
        public List<ListingRegular> promoted { get; set; }
        public List<ListingRegular> regular { get; set; }
    }

    public class ListingSearchMeta
    {
        public int availableCount { get; set; }
        public int totalCount { get; set; }
        public bool fallback { get; set; }
    }

    public class ListingSubcategory
    {
        public string id { get; set; }
        public string name { get; set; }
        public int count { get; set; }
    }

    public class ListingPath
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class ListingCategories
    {
        public List<ListingSubcategory> subcategories { get; set; }
        public List<ListingPath> path { get; set; }
    }

    public class ListingValue
    {
        public string value { get; set; }
        public string name { get; set; }
        public int count { get; set; }
        public bool selected { get; set; }
        public string idSuffix { get; set; }
    }

    public class ListingFilter
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public List<ListingValue> values { get; set; }
        public int? minValue { get; set; }
        public int? maxValue { get; set; }
    }

    public class ListingSort
    {
        public string value { get; set; }
        public string name { get; set; }
        public string order { get; set; }
        public bool selected { get; set; }
    }

    public class ListingRootObject
    {
        public ListingItems items { get; set; }
        public ListingSearchMeta searchMeta { get; set; }
        public ListingCategories categories { get; set; }
        public List<ListingFilter> filters { get; set; }
        public List<ListingSort> sort { get; set; }
    }
    #endregion
    #region SaleOffers
    public class SaleOfferCategory
    {
        public string id { get; set; }
    }

    public class SaleOfferPrimaryImage
    {
        public string url { get; set; }
    }

    public class SaleOfferPrice
    {
        public string amount { get; set; }
        public string currency { get; set; }
    }

    public class SaleOfferSellingMode
    {
        public string format { get; set; }
        public SaleOfferPrice price { get; set; }
        public object minimalPrice { get; set; }
        public object startingPrice { get; set; }
    }

    public class SaleOfferSaleInfo
    {
        public object currentPrice { get; set; }
        public int biddersCount { get; set; }
    }

    public class SaleOfferStats
    {
        public int watchersCount { get; set; }
        public int visitsCount { get; set; }
    }

    public class SaleOfferStock
    {
        public int available { get; set; }
        public int sold { get; set; }
    }

    public class SaleOfferPublication
    {
        public string status { get; set; }
        public object startingAt { get; set; }
        public DateTime? startedAt { get; set; }
        public object endingAt { get; set; }
        public DateTime? endedAt { get; set; }
    }

    public class SaleOfferWarranty
    {
        public string id { get; set; }
    }

    public class SaleOfferReturnPolicy
    {
        public string id { get; set; }
    }

    public class SaleOfferImpliedWarranty
    {
        public string id { get; set; }
    }

    public class SaleOfferAfterSalesServices
    {
        public SaleOfferWarranty warranty { get; set; }
        public SaleOfferReturnPolicy returnPolicy { get; set; }
        public SaleOfferImpliedWarranty impliedWarranty { get; set; }
    }

    public class SaleOfferExternal
    {
        public string id { get; set; }
    }

    public class SaleOfferShippingRates
    {
        public string id { get; set; }
    }

    public class SaleOfferDelivery
    {
        public SaleOfferShippingRates shippingRates { get; set; }
    }

    public class SaleOfferOffer
    {
        public string id { get; set; }
        public string name { get; set; }
        public SaleOfferCategory category { get; set; }
        public SaleOfferPrimaryImage primaryImage { get; set; }
        public SaleOfferSellingMode sellingMode { get; set; }
        public SaleOfferSaleInfo saleInfo { get; set; }
        public SaleOfferStats stats { get; set; }
        public SaleOfferStock stock { get; set; }
        public SaleOfferPublication publication { get; set; }
        public SaleOfferAfterSalesServices afterSalesServices { get; set; }
        public object additionalServices { get; set; }
        public SaleOfferExternal external { get; set; }
        public SaleOfferDelivery delivery { get; set; }
    }

    public class SaleOfferRootObject
    {
        public List<SaleOfferOffer> offers { get; set; }
        public int count { get; set; }
        public int totalCount { get; set; }
    }
    #endregion
    #region SaleOffer
    public class Offer
    {
        public class Category
        {
            public string id { get; set; }
        }

        public class Product
        {
            public Guid id { get; set; }
        }
        public class Options
        {
            public bool identifiesProduct { get; set; }
            public bool? isGTIN { get; set; }
        }
        public class Parameter
        {
            //public string id { get; set; }
            //public List<string> valuesIds { get; set; }
            //public List<string> values { get; set; }
            //public object rangeValue { get; set; }

            public string id { get; set; }
            public string name { get; set; }
            public List<string> valuesLabels { get; set; }
            public List<string> values { get; set; }
            public string unit { get; set; }
            public Options options { get; set; }
            public List<string> valuesIds { get; set; }
        }

        public class Item
        {
            public string type { get; set; }

            public string url { get; set; }
            public string content { get; set; }
        }

        public class Section
        {
            public List<Item> items { get; set; }
        }

        public class Description
        {
            public List<Section> sections { get; set; }
        }

        public class Image
        {
            public string url { get; set; }
        }

        public class Price
        {
            public string amount { get; set; }
            public string currency { get; set; }
        }

        public class SellingMode
        {
            public string format { get; set; }
            public Price price { get; set; }
            public object startingPrice { get; set; }
            public object minimalPrice { get; set; }

        }

        public class Tax
        {
            public decimal percentage { get; set; }

        }
        public class Stock
        {
            public int available { get; set; }
            public string unit { get; set; }
        }

        public class Publication
        {
            public object duration { get; set; }
            public string status { get; set; }
            public object startingAt { get; set; }
            public object endingAt { get; set; }
            public object endedBy { get; set; }
            public bool republish { get; set; }
        }

        public class ShippingRates
        {
            public string id { get; set; }
        }

        public class Delivery
        {
            public ShippingRates shippingRates { get; set; }
            public string handlingTime { get; set; }
            public string additionalInfo { get; set; }
            public string shipmentDate { get; set; }
        }

        public class Payments
        {
            public string invoice { get; set; }
            public Tax tax { get; set; }
        }
   
        public class ImpliedWarranty
        {
            public string id { get; set; }
        }

        public class ReturnPolicy
        {
            public string id { get; set; }
        }

        public class Warranty
        {
            public string id { get; set; }
        }

        public class AfterSalesServices
        {
            public ImpliedWarranty impliedWarranty { get; set; }
            public ReturnPolicy returnPolicy { get; set; }
            public Warranty warranty { get; set; }
        }

        public class Promotion
        {
            public bool emphasized { get; set; }
            public bool bold { get; set; }
            public bool highlight { get; set; }
            public bool departmentPage { get; set; }
            public bool emphasizedHighlightBoldPackage { get; set; }
        }

        public class Location
        {
            public string countryCode { get; set; }
            public string province { get; set; }
            public string city { get; set; }
            public string postCode { get; set; }
        }

        public class External
        {
            public string id { get; set; }
        }

        public class Validation
        {
            public List<object> errors { get; set; }
            public DateTime validatedAt { get; set; }
        }

        public class RootObject
        {
            public string id { get; set; }
            public string name { get; set; }
            public Category category { get; set; }
            public Product product { get; set; }
            public List<Parameter> parameters { get; set; }
            public string ean { get; set; }
            public Description description { get; set; }
            public object compatibilityList { get; set; }
            public object tecdocSpecification { get; set; }
            public List<Image> images { get; set; }
            public Tax tax { get; set; }
            public SellingMode sellingMode { get; set; }
            public Stock stock { get; set; }
            public Publication publication { get; set; }
            public Delivery delivery { get; set; }
            public Payments payments { get; set; }
            public AfterSalesServices afterSalesServices { get; set; }
            public object additionalServices { get; set; }
            public object sizeTable { get; set; }
            public Promotion promotion { get; set; }
            public Location location { get; set; }
            public External external { get; set; }
            public List<object> attachments { get; set; }
            public object contact { get; set; }
            public Validation validation { get; set; }
            public DateTime? createdAt { get; set; }
            public DateTime? updatedAt { get; set; }
        }
    }
    ////{
    ////    public class Category
    ////    {
    ////        public string id { get; set; }
    ////    }

    ////    public class Parameter
    ////    {
    ////        public string id { get; set; }
    ////        public List<string> valuesIds { get; set; }
    ////        public List<string> values { get; set; }
    ////        public RangeValue rangeValue { get; set; }
    ////    }
    ////    public class RangeValue
    ////    {
    ////        public string from { get; set; }
    ////        public string to { get; set; }
    ////    }
    ////    public class Item
    ////    {
    ////        public string type { get; set; }
    ////        public string url { get; set; }
    ////        public string content { get; set; }
    ////    }

    ////    public class Section
    ////    {
    ////        public List<Item> items { get; set; }
    ////    }

    ////    public class Description
    ////    {
    ////        public List<Section> sections { get; set; }
    ////    }

    ////    public class Image
    ////    {
    ////        public string url { get; set; }
    ////    }

    ////    public class Price
    ////    {
    ////        public string amount { get; set; }
    ////        public string currency { get; set; }
    ////    }

    ////    public class SellingMode
    ////    {
    ////        public string format { get; set; }
    ////        public Price price { get; set; }
    ////        public string startingPrice { get; set; }
    ////        public string minimalPrice { get; set; }
    ////    }

    ////    public class Stock
    ////    {
    ////        public int available { get; set; }
    ////        public string unit { get; set; }
    ////    }

    ////    public class Publication
    ////    {
    ////        public string duration { get; set; }
    ////        public string status { get; set; }
    ////        public string startingAt { get; set; }
    ////        public string endingAt { get; set; }
    ////        public string endedBy { get; set; }
    ////    }

    ////    public class Delivery
    ////    {
    ////        public ShippingRates shippingRates { get; set; }
    ////        public string handlingTime { get; set; }
    ////        public string additionalInfo { get; set; }
    ////        public string shipmentDate { get; set; }
    ////    }

    ////    public class ShippingRates
    ////    {
    ////        public string id { get ; set; }
    ////    }
    ////    public class Payments
    ////    {
    ////        public string invoice { get; set; }
    ////    }

    ////    public class ImpliedWarranty
    ////    {
    ////        public string id { get; set; }
    ////    }

    ////    public class ReturnPolicy
    ////    {
    ////        public string id { get; set; }
    ////    }

    ////    public class Warranty
    ////    {
    ////        public string id { get; set; }
    ////    }
    ////    public class External
    ////    {
    ////        public string id { get; set; }
    ////    }
    ////    public class Attachments
    ////    {
    ////        public string id { get; set; }
    ////    }

    ////    public class AfterSalesServices
    ////    {
    ////        public ImpliedWarranty impliedWarranty { get; set; }
    ////        public ReturnPolicy returnPolicy { get; set; }
    ////        public Warranty warranty { get; set; }
    ////    }

    ////    public class Promotion
    ////    {
    ////        public bool emphasized { get; set; }
    ////        public bool bold { get; set; }
    ////        public bool highlight { get; set; }
    ////        public bool departmentPage { get; set; }
    ////        public bool emphasizedHighlightBoldPackage { get; set; }
    ////    }

    ////    public class Location
    ////    {
    ////        public string countryCode { get; set; }
    ////        public string province { get; set; }
    ////        public string city { get; set; }
    ////        public string postCode { get; set; }
    ////    }
    ////    public class Error
    ////    {
    ////        public string code { get; set; }
    ////        public string details { get; set; }
    ////        public string message { get; set; }
    ////        public string path { get; set; }
    ////        public string userMessage { get; set; }
    ////    }
    ////    public class Validation
    ////    {
    ////        public List<Error> errors { get; set; }
    ////        public DateTime validatedAt { get; set; }
    ////    }

    ////    public class RootObject
    ////    {
    ////        public string id { get; set; }
    ////        public string name { get; set; }
    ////        public Category category { get; set; } 
    ////        public string product { get; set; }
    ////        public List<Parameter> parameters { get; set; }
    ////        public string ean { get; set; }
    ////        public Description description { get; set; }
    ////        //public object compatibilityList { get; set; }
    ////        public List<Image> images { get; set; }
    ////        public SellingMode sellingMode { get; set; }
    ////        public Stock stock { get; set; }
    ////        public Publication publication { get; set; }
    ////        public Delivery delivery { get; set; }
    ////        public Payments payments { get; set; }
    ////        public AfterSalesServices afterSalesServices { get; set; }
    ////        public string additionalServices { get; set; }
    ////        //public object sizeTable { get; set; }
    ////        public Promotion promotion { get; set; }
    ////        public Location location { get; set; }
    ////        public External external { get; set; }
    ////        public List<Attachments> attachments { get; set; }
    ////        //public object contact { get; set; }
    ////        public Validation validation { get; set; }
    ////        public DateTime? createdAt { get; set; }
    ////        public DateTime? updatedAt { get; set; }
    ////    }
    ////}
    #endregion
    #endregion

    public partial class AllegroRESTHelper
    {


        private static HttpWebRequest GetHttpWebRequest(string url, string method, long? itemId, long? userId)
        {
            string token = null;

            Dal.AllegroScan asc = new Dal.AllegroScan();
            if (itemId.HasValue)
            {
                Dal.AllegroItemUserView ai = asc.GetAllegroItemUser(itemId.Value);

                //if (ai == null)
                //    return null;
                token = ai.Token;
            }
            else
            {
                if (userId.HasValue)
                {

                    Dal.AllegroUser ai = asc.GetAllegroUser(userId.Value);

                    //if (ai == null)
                    //    return null;
                    token = ai.Token;
                }
                else

                {

                    Dal.AllegroUser ai = asc.GetAllegroUser((int)Dal.Helper.MyUsers.JacekStawicki);

                    //if (ai == null)
                    //    return null;
                    token = ai.Token;

                }
            }


            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(String.Format("https://api.allegro.pl{0}", url));
            request.Accept = "application/vnd.allegro.public.v1+json";
            request.ContentType = "application/vnd.allegro.public.v1+json";
            if (token != null)
                request.Headers.Add(HttpRequestHeader.Authorization, String.Format("Bearer {0}", token));
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-PL");
            //request.Headers.Add("Api-Key", au.ClientId);

            request.Method = method;

            //Dal.AllegroRestHelper.SetLog(url, method, itemId, userId);

            return request;
        }

        //public static void Test()
        //{
        //    AllegroRESTHelper.GetOffer(6749543339);
        //}
        public static void Me()
        {
            try
            {

                HttpWebRequest request = GetHttpWebRequest("/me", "GET", null, 0);


                string text = null;
                using (WebResponse webResponse = request.GetResponse())
                {
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    text = reader.ReadToEnd();
                }

                var json_serializer = new JavaScriptSerializer();
                DeliveryObject item = json_serializer.Deserialize<DeliveryObject>(text);

            }
            catch (Exception ex)
            {
                Bll.ErrorHandler.SendError(ex, "Błąd pobierania dostępnych metod dostaw");

            }
        }

    }
}
