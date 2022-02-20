

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;
using static LajtIt.Bll.AllegroRESTHelper.DraftError;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
        public class PublicOffer
        {
            public class Publication
            {
                public string status { get; set; }
                public object duration { get; set; }
                public object endedBy { get; set; }
                public object endingAt { get; set; }
                public object startingAt { get; set; }
                public bool republish { get; set; }
            }

            public class Product
            {
                public object id { get; set; }
                public Publication publication { get; set; }
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

            public class Payments
            {
                public string invoice { get; set; }
            }

            public class Price
            {
                public double amount { get; set; }
                public string currency { get; set; }
            }

            public class SellingMode
            {
                public string format { get; set; }
                public Price price { get; set; }
                public object startingPrice { get; set; }
                public object minimalPrice { get; set; }
            }

            public class Stock
            {
                public int available { get; set; }
                public string unit { get; set; }
            }

            public class Location
            {
                public string countryCode { get; set; }
                public string province { get; set; }
                public string city { get; set; }
                public string postCode { get; set; }
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

            public class Validation
            {
                public List<object> errors { get; set; }
                public List<object> warnings { get; set; }
                public DateTime validatedAt { get; set; }
            }

            public class External
            {
                public string id { get; set; }
            }

            public class Category
            {
                public string id { get; set; }
            }

            public class Tax
            {
                public string percentage { get; set; }
            }

            public class Discounts
            {
                public object wholesalePriceList { get; set; }
            }

            public class Root
            {
                public string id { get; set; }
                public string name { get; set; }
                public Product product { get; set; }
                public AfterSalesServices afterSalesServices { get; set; }
                public Payments payments { get; set; }
                public SellingMode sellingMode { get; set; }
                public Stock stock { get; set; }
                public Location location { get; set; }
                public Delivery delivery { get; set; }
                public Publication publication { get; set; }
                public Description description { get; set; }
                public Validation validation { get; set; }
                public DateTime createdAt { get; set; }
                public DateTime updatedAt { get; set; }
                public List<string> images { get; set; }
                public External external { get; set; }
                public Category category { get; set; }
                public Tax tax { get; set; }
                public object sizeTable { get; set; }
                public Discounts discounts { get; set; }
            }


            public static string GetOfferProduct(long itemId)
            {
                string text = null;
                try
                {
                    HttpWebRequest request =
       GetHttpWebRequest(String.Format("/sale/product-offers/{0}", itemId), "GET", itemId, null);
                    request.ContentType = "application/vnd.allegro.beta.v2+json";
                    request.Accept = "application/vnd.allegro.beta.v2+json";


                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }


                    var json_serializer = new JavaScriptSerializer();
                    Root allegroProducts = json_serializer.Deserialize<Root>(text);

                    return "";
                }
                catch (WebException ex)
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse == null)
                        { 
                            return null ;

                        } 
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string t = reader.ReadToEnd();

                            return null;
                        }
                    }
                }
                catch (Exception ex)
                { 
                   // ErrorHandler.SendError(ex, String.Format("UpdateOffer błąd. ItemId {0}", item.ItemId));
                    return null;
                }
            }
        }

        public class Products
        {
            // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
            public class ProductSearchClass
            {
                public class Similar
                {
                    public string id { get; set; }
                }

                public class Category
                {
                    public string id { get; set; }
                    public List<Similar> similar { get; set; }
                }

                public class Options
                {
                    public bool identifiesProduct { get; set; }
                }

                public class Parameter
                {
                    public string id { get; set; }
                    public string name { get; set; }
                    public List<string> valuesLabels { get; set; }
                    public List<string> values { get; set; }
                    public string unit { get; set; }
                    public Options options { get; set; }
                    public List<string> valuesIds { get; set; }
                }

                public class Image
                {
                    public string url { get; set; }
                }

                public class Product
                {
                    public Guid id { get; set; }
                    public string name { get; set; }
                    public Category category { get; set; }
                    public List<Parameter> parameters { get; set; }
                    public List<Image> images { get; set; }
                    public object description { get; set; }
                }

                public class Subcategory
                {
                    public string id { get; set; }
                    public string name { get; set; }
                    public int count { get; set; }
                }

                public class Path
                {
                    public string id { get; set; }
                    public string name { get; set; }
                }

                public class Categories
                {
                    public List<Subcategory> subcategories { get; set; }
                    public List<Path> path { get; set; }
                }

                public class Root
                {
                    public List<Product> products { get; set; }
                    public Categories categories { get; set; }
                    public List<object> filters { get; set; }
                    public object nextPage { get; set; }
                }

            }

            public class ProductClass
            {
                public class Similar
                {
                    public string id { get; set; }
                }

                public class Category
                {
                    public string id { get; set; }
                    public List<Similar> similar { get; set; }
                }

                public class Options
                {
                    public bool identifiesProduct { get; set; }
                    public bool? isGTIN { get; set; }
                }

                public class Parameter
                {
                    public string id { get; set; }
                    public string name { get; set; }
                    public List<string> valuesLabels { get; set; }
                    public List<string> values { get; set; }
                    public string unit { get; set; }
                    public Options options { get; set; }
                    public List<string> valuesIds { get; set; }
                }

                public class Image
                {
                    public string url { get; set; }
                }

                public class OfferRequirements
                {
                    public List<object> parameters { get; set; }
                }

                public class Root
                {
                    public string id { get; set; }
                    public string name { get; set; }
                    public Category category { get; set; }
                    public List<Parameter> parameters { get; set; }
                    public List<Image> images { get; set; }
                    public OfferRequirements offerRequirements { get; set; }
                    public object compatibilityList { get; set; }
                    public object tecdocSpecification { get; set; }
                    public object description { get; set; }
                }
            }
            public static void GetProductsUpdate()
            {
                List<Dal.AllegroProduct> products = Dal.DbHelper.AllegroHelper.GetAllegroProductsToUpdate(1000);


                SetAllegroProducts(products);
            }
            public static void GetProductsNew()
            {
                List<Dal.AllegroProduct> products = Dal.DbHelper.AllegroHelper.GetAllegroProductsToCheck(1000);

                SetAllegroProducts(products);
            }

            private static void SetAllegroProducts(List<AllegroProduct> products)
            {
                foreach (var product in products)
                {
                    var text = GetAllegroProduct(product);

                    if (text != null)
                    {
                        var json_serializer = new JavaScriptSerializer();
                        ProductSearchClass.Root allegroProducts = json_serializer.Deserialize<ProductSearchClass.Root>(text);

                        foreach(ProductSearchClass.Product allegroProduct in allegroProducts.products)
                        {
                            Dal.DbHelper.AllegroHelper.SetAllegroProduct(product.Ean, allegroProduct.id, text, null);
                        }
                    }
                }
            }

            private static string GetAllegroProduct(AllegroProduct product)
            {
                string text = null;
                try
                {


                    HttpWebRequest request = 
                        GetHttpWebRequest(String.Format("/sale/products?ean={0}", product.Ean), "GET", null, (long)Dal.Helper.MyUsers.CzerwoneJablko);



                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }
                    return text;

                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania produktu. Ean: {0} ", product.Ean));

                    return null;
                }
            }
            public static List<Offer.Parameter> GetParametersFromProduct(Guid id)
            {
                try
                {


                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/products/{0}", id), "GET", null, (int)Dal.Helper.MyUsers.CzerwoneJablko);

                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    ProductClass.Root product = json_serializer.Deserialize<ProductClass.Root>(text);


                    return product.parameters.Select(x => new Offer.Parameter()
                    {
                        id=x.id,
                        name=x.name,
                        //options=x.options,
                        unit=x.unit,
                        values=x.values,
                        valuesIds=x.valuesIds,
                        valuesLabels=x.valuesLabels
                    }).ToList();

                }
                catch (Exception ex)
                {
                    Bll.ErrorHandler.SendError(ex, String.Format("Błąd pobierania informacji o produkcie. Allegro.ProductId: {0}", id));

                }
                return new List<Offer.Parameter>();
            }
        }

    }
}