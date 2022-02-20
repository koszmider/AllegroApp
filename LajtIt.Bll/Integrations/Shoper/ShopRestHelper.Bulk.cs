using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace LajtIt.Bll
{
	public partial class ShopRestHelper
	{
		public class Bulk
		{
			public class BulkObject
			{
				public string id { get; set; }
				public string path { get; set; }
				public string method { get; set; }

				public object body { get; set; }
			}

            //public class Body
            //{
            //    public string error { get; set; }
            //    public string error_description { get; set; }
            //}
            public class Item
            {
                public int code { get; set; }
                public object body { get; set; }
                public string id { get; set; }
            }

            public class BulkResult
            {
                bool _errors;
                public bool errors
                {
                    get { return items.Where(x => x.code != 200).Count() > 0; }
                    set
                    {
                        _errors = value;
                    }
                }
                public List<Item> items { get; set; }

                internal void Append(BulkResult bulkResult)
                {
                    if (items == null)
                        items = new List<Item>();

                    items.AddRange(bulkResult.items);                    
                }
            }


            public static BulkResult Sent(Dal.Helper.Shop shop, List<BulkObject>  objects)
            {
                try
                {
                    if (objects.Count() == 0)
                        return new BulkResult() { errors = false, items = new List<Item>() };

                    BulkResult result = new BulkResult();

                    int partsCount = (objects.Count() / 25);

                    for (int i = 0; i < partsCount + 1; i++)
                    {
                        BulkObject[] objectParts = objects.Skip(i * 25).Take(25).ToArray();

                        if (objectParts.Length == 0)
                            continue;

                        HttpWebRequest request = ShopRestHelper.GetHttpWebRequest(shop, "/webapi/rest/bulk", "POST");

                        Stream dataStream = request.GetRequestStream();
                        string jsonEncodedParams = Bll.RESTHelper.ToJson(objectParts);
                        System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();


                        byte[] byteArray = encoding.GetBytes(jsonEncodedParams);

                        dataStream.Write(byteArray, 0, byteArray.Length);
                        dataStream.Close();


                        string text = GetTextFromWebResponse(request);


                        var json_serializer = new JavaScriptSerializer();

                        var r = json_serializer.Deserialize<BulkResult>(text);

                        result.Append(r);

                    }


                   
                    return result;
                    /*
                     {"errors":false,"items":[
                    {"code":200,"body":1,"id":"product_PcId-1563_PId-502"},
                    {"code":200,"body":1,"id":"product_PcId-1346_PId-203"},
                    {"code":200,"body":79542,"id":"product_PcId-79039_PId-"}]}

*/
                }

                catch (WebException ex)
                {
                    return ProcessException(ex, shop); 
                }
            }

            public static string BulkResultToString(BulkResult result)
            {
                StringBuilder sb = new StringBuilder();
                if (result.items != null)
                {

                    string header = "<table border=1><tr style='background-color:silver'><td colspan='3'>Przetworzono: {0}, Liczba błędów: {1}</td><tr style='background-color:gray'><td>Id</td><td>Kod</td><td>Błąd</td></tr>";

                    sb.AppendLine(String.Format(header, result.items.Count(), result.items.Where(x => x.code != 200).Count()));

                    foreach (Item item in result.items)
                    {

                        string row = "<tr><td>{0}</td><td>{1}</td><td>{2} - {3}</td></tr>";

                        if (item.body != null && item.body is Dictionary<string, object>)
                        {
                            Dictionary<string, object> ib = item.body as Dictionary<string, object>;
                            sb.AppendLine(String.Format(row, item.id, item.code, ib["error"], ib["error_description"]));
                        }
                        else
                            sb.AppendLine(String.Format(row, item.id, item.code, "", ""));

                    }
                    sb.AppendLine("</table>");
                }
                return sb.ToString();
            }
        }
	}
}
