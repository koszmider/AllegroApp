

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public partial class AllegroRESTHelper
    {
        public class Variants
        {
            public class Offer
            {
                public string id { get; set; }
                public string colorPattern { get; set; }
            }

            public class Parameter
            {
                public string id { get; set; }
            }

            public class RootObject
            {
                public string name { get; set; }
                public List<Offer> offers { get; set; }
                public List<Parameter> parameters { get; set; }
            }


            public static void UpdateVariants()
            {

                Dal.ProductCatalogGroupHelper pch = new ProductCatalogGroupHelper();
                List<Dal.ProductCatalogGroupFamilyAllegro> families = pch.GetGroupFamilyForAllegro();//.Where(x=>x.FamilyId==162).ToList();

                foreach (Dal.ProductCatalogGroupFamilyAllegro family in families)
                {
                    Console.WriteLine(String.Format("FamilyID {0}, UserID {1}, CategoryId: {2}", family.FamilyId, family.UserId, family.AllegroCategoryId));
                    try
                    {
                        long[] itemIds = pch.GetProductCatalogsByProductFamilyForAllegro(family.FamilyId, family.UserId, family.AllegroCategoryId.Value);
                        Console.WriteLine(String.Format("FamilyID {0}, UserID {1}, ITems count {2}", family.FamilyId, family.UserId, itemIds.Count()));
                        if (itemIds.Count() > 1)
                        {
                            bool createNew = family.SetId.HasValue == false;
                            Guid setId = Guid.NewGuid();
                            if (!createNew)
                                setId = family.SetId.Value;

                            if (CreateVariant(family.UserId, itemIds, setId))
                                pch.SetProductCatalogsByProductFamily(family.Id, createNew, setId);
                        }
                        else
                        {
                            if (family.SetId.HasValue)
                            {
                                if (DeleteVariant(family.SetId.Value, family.UserId))
                                    pch.SetProductCatalogsByProductFamilyDelete(family.Id);
                            }
                            else

                                pch.SetProductCatalogsByProductFamily(family.Id, false, Guid.NewGuid());
                        }
                    }
                    catch (Exception ex)
                    {

                        Bll.ErrorHandler.SendError(ex, String.Format(@"Aktualizacja wariantów. FamilyID {0}, UserID {1}, CategoryId: {2}
                        ", family.FamilyId, family.UserId, family.AllegroCategoryId));
                    }
                }
            }

            public static void DeleteVariants()
            {

                Dal.ProductCatalogGroupHelper pch = new ProductCatalogGroupHelper();
                List<Dal.ProductCatalogGroupFamilyAllegro> families = pch.GetGroupFamilyForAllegroCreated();//.Where(x=>x.FamilyId==162).ToList();

                foreach (Dal.ProductCatalogGroupFamilyAllegro family in families)
                {
                    Console.WriteLine(String.Format("FamilyID {0}, UserID {1}, CategoryId: {2}, setId {3}", family.FamilyId, family.UserId, family.AllegroCategoryId, family.SetId));
                    try
                    {
                        System.Threading.Thread.Sleep(1000);
                        if (DeleteVariant(family.SetId.Value, family.UserId))
                            pch.SetProductCatalogsByProductFamilyClear(family.Id);

                    }
                    catch (Exception ex)
                    {

                        Bll.ErrorHandler.SendError(ex, String.Format(@"Usuwanie wariantów. FamilyID {0}, UserID {1}, CategoryId: {2}
                        ", family.FamilyId, family.UserId, family.AllegroCategoryId));
                    }
                }
            }

            private static bool DeleteVariant(Guid setId, long userId)
            {
                try
                {
                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-variants/{0}", setId), "DELETE", null, userId);
                    

                    WebResponse webResponse = request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();

                    //Object response = FromJson(text);

                    Console.WriteLine("Wariant usunięty: {1} {0}", setId, userId);
                    return true;

                }
                catch (WebException ex)
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse == null)
                        {
                            Bll.ErrorHandler.SendError(ex, ex.Message); 
                        }
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();

                            Console.WriteLine("Error code: {0}", text);


                        }
                    }
                    Console.WriteLine("Error code: {0}", ex.Message);
                    return false;

                }
            }

            private static bool CreateVariant(long userId, long[] itemIds, Guid setId)
            {
                try
                {
                    HttpWebRequest request = GetHttpWebRequest(String.Format("/sale/offer-variants/{0}", setId), "PUT", null, userId);
                    List<Offer> offers = new List<Offer>();
                    offers.AddRange(itemIds.Select(x => new Offer() { id = x.ToString(), colorPattern = x.ToString() }).ToList());

                    List<Parameter> parameters = new List<Parameter>();
                    parameters.Add(new Parameter() { id = "color/pattern" });
                    RootObject variant = new RootObject();
                    variant.name = setId.ToString();
                    variant.offers = offers;
                    variant.parameters = parameters;


                    Stream dataStream = request.GetRequestStream();


                    string jsonEncodedParams = Bll.RESTHelper.ToJson(variant);
                    System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                    byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();



                    WebResponse webResponse = request.GetResponse();
                    Stream responseStream = webResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(responseStream);
                    string text = reader.ReadToEnd();

                    Object response = Bll.RESTHelper.FromJson(text);

                    return true;

                }
                catch (WebException ex)
                {
                    using (WebResponse response = ex.Response)
                    {
                        HttpWebResponse httpResponse = (HttpWebResponse)response;
                        if (httpResponse == null)
                        {
                            Bll.ErrorHandler.SendError(ex, ex.Message);


                        }
                        Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                        using (Stream data = response.GetResponseStream())
                        using (var reader = new StreamReader(data))
                        {
                            string text = reader.ReadToEnd();

                            //Bll.ErrorHandler.SendError(ex, String.Format("Błąd publikowania ofert userId {0}, commandId {1}, {2}", "", "", text));
                            Bll.ErrorHandler.LogError(ex, String.Format("Błąd publikowania ofert userId {0}, commandId {1}, {2}", "", "", text));

                        }
                    }
                    return false;


                }

            }

        }
    }
}