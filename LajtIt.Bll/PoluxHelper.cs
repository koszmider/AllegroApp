using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks; 
using System.Xml.Serialization;

namespace LajtIt.Bll
{
    public class PoluxHelper : ImportData, IImportData
    {
        [XmlRoot(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public class Url
        {
            [XmlElement(ElementName = "loc", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public string Loc { get; set; }
            [XmlElement(ElementName = "priority", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public string Priority { get; set; }
            [XmlElement(ElementName = "changefreq", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public string Changefreq { get; set; }
            [XmlElement(ElementName = "lastmod", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public string Lastmod { get; set; }
        }

        [XmlRoot(ElementName = "urlset", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
        public class Urlset
        {
            [XmlElement(ElementName = "url", Namespace = "http://www.sitemaps.org/schemas/sitemap/0.9")]
            public List<Url> Url { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
            [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Xsi { get; set; }
            [XmlAttribute(AttributeName = "image", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Image { get; set; }
            [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
            public string SchemaLocation { get; set; }
        }

        Dictionary<string, string> polux = new Dictionary<string, string>();

        public new void LoadData<T>()
        {
            T data = base.LoadData<T>();
            ProcessData(data);
            base.PostLoadProcess();
        }

        public void ProcessData<T>(T obj)
        {
            try
            {
                Bll.PoluxHelper.Urlset pm = obj as Bll.PoluxHelper.Urlset;


                foreach(Bll.PoluxHelper.Url url in pm.Url)//.Skip(200).Take(200))
                {

                    GetWebPage(url.Loc);

                }

                Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

                pch.SetProductCatalogStatusPolux(SupplierId, polux); 
            }
            catch (Exception ex)
            {

                Bll.ErrorHandler.SendError(ex, "Polux przetwarzanie pliku");
            }
        }

        private void GetWebPage(string url)
        {
            string html = string.Empty; 

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "C# console client";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    //string pattern = @"<p id=\""KodEan\""([^>]*)>(.*)<\/p>";
                    string pattern = @"<strong itemprop=\""gtin13\"">(.*)</strong>";

                    // Regex regex = new Regex();
                    Match m = Regex.Match(html, pattern);
                    string ean = m.Groups[1].ToString();


                    pattern = @"<span>Dostępność:</span> <strong>(.*)</strong>";
                    string pattern2 = @"<div id=""PrzyciskKupowania"" >";

                    bool chwil = html.Contains(pattern2); // jeśli to jest tzn że można kupić
                   
                    

                    Match ma = Regex.Match(html, pattern);
                    string avail = ma.Groups[1].ToString();

                    if (!chwil)
                        avail = "Niedostępny";

                    if (!String.IsNullOrEmpty(ean) && !String.IsNullOrEmpty(avail))
                        if (!polux.ContainsKey(ean))
                            polux.Add(ean, avail);
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
