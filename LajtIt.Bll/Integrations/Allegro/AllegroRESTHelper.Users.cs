

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using LajtIt.Dal;

namespace LajtIt.Bll
{

    public partial class AllegroRESTHelper
    {
        public class Users
        {
            public class Recommended
            {
                public int unique { get; set; }
                public int total { get; set; }
            }

            public class NotRecommended
            {
                public int unique { get; set; }
                public int total { get; set; }
            }

            public class AverageRates
            {
                public double deliveryCost { get; set; }
                public double service { get; set; }
                public double description { get; set; }
            }

            public class RootUserRating
            {
                public Recommended recommended { get; set; }
                public NotRecommended notRecommended { get; set; }
                public string recommendedPercentage { get; set; }
                public AverageRates averageRates { get; set; }
            }

            public static RootUserRating GetUserRatingSummary(long userId)
            {
                try
                {

                    HttpWebRequest request = GetHttpWebRequest(String.Format("/users/{0}/ratings-summary", userId), "GET", null, userId);


                    string text = null;
                    using (WebResponse webResponse = request.GetResponse())
                    {
                        Stream responseStream = webResponse.GetResponseStream();
                        StreamReader reader = new StreamReader(responseStream);
                        text = reader.ReadToEnd();
                    }

                    var json_serializer = new JavaScriptSerializer();
                    RootUserRating rating = json_serializer.Deserialize<RootUserRating>(text);

                    return rating;


                }
                catch (Exception ex)
                {
                    return null;
                    //Bll.ErrorHandler.SendError(ex, String.Format("Nieudana próba publikacji ofert. CommandId: {0}<br><br>{1}", commandID, ex.Message));
                }
            }
        }
    }
}