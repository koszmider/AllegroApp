using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Serialization;

namespace LajtIt.Web
{
    [Developer("6593d7bc-11a3-45f2-9d0c-2f13f6ac29b3")]
    public partial class GzipPage : LajtitPage
    {
        public class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            List<Dal.ProductCatalogView> products = pch.GetProductCatalogForPosnet();
            Bll.PosnetHelper.POSClientData data = new Bll.PosnetHelper.POSClientData();
            
            Bll.PosnetHelper.PLUcodes codes = new Bll.PosnetHelper.PLUcodes();
            data.PLUcodes = codes;
            codes.Item = new List<Bll.PosnetHelper.Item>();
             

            foreach(Dal.ProductCatalogView product in products)
            {
                Bll.PosnetHelper.Item item = new Bll.PosnetHelper.Item()
                {
                    Id_unique = product.PosnetUniqueId.ToString(),
                    Name = String.Format("{0} {1} {2}", product.PosnetUniqueId,   product.SupplierName, product.Code)  ,
                    MinMagazyn = "0",
                    Typ = "0",
                    Ptu = "0",
                    Price = "0",
                    PackNr = "0",
                    JmNr = "0",
                    Plu_formatIlosci = "3",
                    ConstPrice = "False",
                    Rabat = "0",
                    WielopakIlosc = "1",
                    PLUNr = "1",
                    Notatnik = "False",
                    KodKreskowy = ""


                };
                codes.Item.Add(item);

            }
            string xml = Serialize(data);

            GzipIt(xml);
        }
        public static string Serialize(object dataToSerialize)
        {
            if (dataToSerialize == null) return null;

            using (StringWriter stringwriter = new Utf8StringWriter())
            {
                var serializer = new XmlSerializer(dataToSerialize.GetType());
                serializer.Serialize(stringwriter, dataToSerialize);
                return stringwriter.ToString();
            }
        }
        private void GzipIt(string p)
        {
      
            // Convert 10000 character string to byte array.
            byte[] text = Encoding.ASCII.GetBytes(p);

            // Use compress method.
            byte[] compress = Compress(text);

            // Write compressed data.
            File.WriteAllBytes(MapPath("/REVO_1.nps"), compress);
        }

        public static byte[] Compress(byte[] raw)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory,
                    CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
    }
}