using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Configuration;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace LajtIt.Dal
{

    public class LajtitDB : AllegroDBLinqSetDataContext
    {
        public LajtitDB()
            : base(
                ConfigurationManager.ConnectionStrings[String.Format("LajtitDB_{0}", Environment.UserName)].ConnectionString)
        {
            this.CommandTimeout = 3600;

        }
    }
    public class LajtitViewsDB : LajtitViewsDBDataContext
    {
        public LajtitViewsDB()
            : base(
                ConfigurationManager.ConnectionStrings[String.Format("LajtitDB_{0}", Environment.UserName)].ConnectionString)
        {
            this.CommandTimeout = 3600;

        }
    }
    public class LajtitAllegroDB : AllegroDBDataContext
    {
        public LajtitAllegroDB()
            : base(
                ConfigurationManager.ConnectionStrings[String.Format("LajtitAllegroDB_{0}", Environment.UserName)].ConnectionString)
        {
            this.CommandTimeout = 3600;

        }
    }
    public class LajtitHelperDB : HelperDBLinqSetDataContext
    {
        public LajtitHelperDB()
            : base(
                ConfigurationManager.ConnectionStrings[String.Format("LajtitHelperDB_{0}", Environment.UserName)].ConnectionString)
        {
            this.CommandTimeout = 3600;

        }
    }


    public class FileImportValidator
    {
        private List<string> validationErrors = new List<string>();
        public int FileDataId { get; set; }
        public bool ValidationResult { get { return validationErrors.Count == 0; } }
        public string FieldName { get; set; }
        public List<string> ValidationErrors { get { return validationErrors; } }

        public void AddError(string error)
        {
            validationErrors.Add(error);
        }
    }

    public static class Helper
    {
        public static string StaticLajtitUrl = "static.lajtit.pl";
        public static string ToUrlFriendlyString(this string value)
        {
            value = (value ?? "").Trim().ToLower();

            var url = new StringBuilder();

            foreach (char ch in value)
            {
                switch (ch)
                {
                    case ' ':
                        url.Append('-');
                        break;
                    default:
                        url.Append(Regex.Replace(ch.ToString(), @"[^A-Za-z0-9'()\*\\+_~\:\/\?\-\.,;=#\[\]@!$&]", ""));
                        break;
                }
            }

            return url.ToString();
        }
        public static decimal RoundValue(SupplierRoundPriceType type, decimal value)
        {
            if (value < 10)
                return value;

            switch(type)
            {
                case SupplierRoundPriceType.None: return value;
                case SupplierRoundPriceType.DownToInteger: return Math.Floor(value);
            }
            return value;
        }
        public const int DefaultParcelWeight = 2;
        public enum ShippingServiceMode
        {
            Courier = 1,
            Point = 2,
            Showroom = 3,
            ExternalShipping = 4,
            CourierInPost = 5
        }
        public enum OrderShippingStatus
        {
            Deleted = 0,
            Temporary = 1,
            Generated = 2,
            ReadyToCreate = 3
        }
        public enum ShippingServiceType
        {
            ForOrder = 1,
            ForComplaint = 2
        }
        public enum OrderReceiptStatus
        {
            Error = 0,
            New = 1,
            OK = 2
        }
        public enum ProductCatalogGroupFamilyType
        {
            Family = 1,
            Aletnative = 2
        }
        public enum OrderPaymentType
        {
            PayU23 = 1, //23 m-home
            Cash0 = 4,
            eCard23 = 6,
            Ing23 = 13,
            Cash23 = 18,
            CC23 = 19,
            Przelewy24 = 22
        }
        public enum SupplierRoundPriceType
        {
            None = 0,
            DownToInteger = 1

        }
        public enum OrderPaymentAccoutingType
        {
            Evidence=1,
            Invoice=2,
            CashRegister=3
        }
        public enum PromotionConditionType
        {
            PriceFrom = 1,
            PriceTo = 2,
            Promotion = 3,
            PromotionRate = 4,
            Availability=5,
            Outlet = 6,
            SupplierCountry = 7
        }
        public class ShopCeneo
        {
            public Shop Shop { get; set; }
            public Shop CeneoShop { get; set; }
            public int ProductCatalogId { get; set; }
            public string ShopProductId { get; set; }
            public string CeneoProductId { get; set; }
            public decimal CeneoClickRate  { get; set; }
            public decimal CeneoMaxBid   { get; set; }
            public decimal CeneoClickRateWithMaxBid { get; set; }
        }
      
        public enum Shop
        {
            Lajtitpl = 1,
            JacekStawicki = 2,
            CzerwoneJablko = 3,
            Oswietlenie_Lodz = 4,
            sklep_italux = 5,
            Ceneo = 6,
            eMag = 7,
            Homebook=8,
            GG=9,
            Mail=10,
            Telefon=11,
            OLX=12,
            Showroom=13,
            Czat = 14,
            PGE = 15,
            maytoni_sklep=16,
            Favi = 17,
            FB= 20,
            Morele = 21,
            Empik = 22,
            OswietlenieTechniczne= 23,
            Erli = 24,
            Lampy_Maytoni = 25,
            Clipperon = 26,
            Allegro_www_lajtit_pl = 28

        }
        public enum ShopType
        {
            Undefined = 0,
            Allegro = 1,
            ShoperLajtitPl = 2,
            Ceneo = 3,
            eMag = 4,
            Homebook = 5,
            ShoperOswietlenieTechnicznePl = 9,
            Erli = 10,
            Clipperon = 11
        }  public enum ComplaintStatus
        {
            New = 1,
            Closed=4
        }
        public enum ShopEngineType
        {
            Undefined = 0,
            Allegro = 1,
            Shoper= 2,
            Ceneo = 3,
            eMag = 4,
            Homebook = 5,
            Erli = 9
        }
        public enum ShopUpdateStatus
        {
            New = 1,
            Processed = 2
        }
        public enum SystemRole
        {
            Admin = 1,
            Manager = 2,
            Seller = 3,
            Wahrehouse = 4,
            Backend = 5,
            Marketing = 6,
            Customer = 7,
            Accouting = 8

        }
        public enum ShopColumnType
        {
            All = 0,//Wszystko	
            Delivery = 1,//Cennik dostawy	
            Quantity = 2,//Ilość	
            Price = 3,//Cena	
            Name = 4,//Tytuł 
            Description = 5,//Opis	
            Images = 6,//Zdjęcia
            Attributes = 7,//Parametry	
            Status = 8,//Status
            Category = 9,//Kategoria
            Ean = 10,//Ean
            PricePromo = 11,//Cena	
            Related = 12,//Produkty powiązane
        }
        public enum UpdateScheduleType
        {
            //All = 0,
            /// <summary>
            /// aktualizacje możliwe do wykonania hurtowo
            /// </summary>
            OnlineShopBatch = 1,
            /// <summary>
            /// aktualizacje wykonywane pojedynczo
            /// </summary>
            OnlineShopSingle = 2
        }
        ////public enum Source
        ////{
        ////    Allegro = 1,
        ////    OnlineShop = 2,
        ////    Showroom = 8,
        ////    Ceneo = 9,
        ////    eMag = 11,
        ////    Homebook = 12

        ////}
        public enum TableName
        {
            ProductCatalog 
        }
        public enum AllegroFieldType
        {
            Int = 1,
            Float = 3
        }
        public enum ProductType
        {
            RegularProduct = 1,
            ComponentProduct = 2,
            ComboProduct = 3
        }
        //public enum AllegroItemUpdateStatus
        //{
        //    NotScheduled = -1,
        //    Error = 0,
        //    OK = 1,
        //    Verifying = 2,
        //    Pause = 3
        //}
        public enum ProductCatalogUpdateStatus
        { 
            Deleted = 0,
            New = 1,
            Completed = 2,
            Error = 3
        }
        public enum CostType
        {
            ImportCost = 16
        }
        public class Amount
        {
            public decimal VATRate { get; set; }
            public decimal VAT { get; set; }
            public decimal Netto { get; set; }
            public decimal Brutto { get; set; }

        }
        public static EnvirotmentEnum Env = EnvirotmentEnum.Dev;

        public enum EnvirotmentEnum
        {
            Dev,
            Prod
        }

        // public const long MyID = 678165;
        public const string MyEmail = "kontakt@lajtit.pl";
        public const string MyManagerEmail = "magda@lajtit.pl";
        public const string DevEmail = "jacek@lajtit.pl";
        public const string ErrorEmail = "bledy@lajtit.pl";
        public const string BackendEmail = "backend@lajtit.pl";

        public const decimal VAT = 0.23M;
        public static void SetEnv()
        {

            if ( Environment.UserName == "lajtit_www_user"
                || Environment.UserName == "wwwlajtit"
                || Environment.UserName.ToLower() == "administrator")
                Dal.Helper.Env = Dal.Helper.EnvirotmentEnum.Prod;

        }

        public enum MyUsers
        {
            JacekStawicki = 678165,
            CzerwoneJablko = 28277822,
            Oswietlenie_Lodz = 44282528,
            sklep_italux = 55501013
        }
        public static long GetUserId(string userName)
        {
            return (long)(MyUsers)System.Enum.Parse(typeof(MyUsers), userName);
            //return 0;
        }

        public static string GetUserName(long userId)
        {
            return Enum.GetName(typeof(MyUsers), userId);
        }

        public static long[] GetMyIds()
        {
            return Enum.GetNames(typeof(Dal.Helper.MyUsers)).Select(x => (long)(int)Enum.Parse(typeof(Dal.Helper.MyUsers), x)).ToArray();  
              
        }

        public static string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["LajtitDB_" + Environment.UserName].ConnectionString;
        }
        public static string DateTimeToStringWithDays(DateTime data)
        {
            TimeSpan days = DateTime.Now - data;
            return String.Format("{0:yy-MM-dd HH:mm} ({1} dni)", data, days.Days);
        }
        public static long DateTimeToUnixTime(DateTime d)
        {

            DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
            TimeSpan span = (d.ToLocalTime() - epoch);
            return (long)span.TotalSeconds;
        }
        public static string Description(this Enum enumValue)
        {
            var enumType = enumValue.GetType();
            var field = enumType.GetField(enumValue.ToString());
            var attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length == 0
                ? enumValue.ToString()
                : ((DescriptionAttribute)attributes[0]).Description;
        }
        public static void CopyProperties(this object source, object destination)
        {
            // If any this null throw an exception
            if (source == null || destination == null)
                throw new Exception("Source or/and Destination Objects are null");
            // Getting the Types of the objects
            Type typeDest = destination.GetType();
            Type typeSrc = source.GetType();

            // Iterate the Properties of the source instance and  
            // populate them from their desination counterparts  
            PropertyInfo[] srcProps = typeSrc.GetProperties();
            foreach (PropertyInfo srcProp in srcProps)
            {
                if (!srcProp.CanRead)
                {
                    continue;
                }
                PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
                if (targetProperty == null)
                {
                    continue;
                }
                if (!targetProperty.CanWrite)
                {
                    continue;
                }
                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    continue;
                }
                if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType))
                {
                    continue;
                }
                // Passed all tests, lets set the value
                targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
            }
        }
 
    
 
        public enum EmailTemplate
        {
            AllegroComment = 1,
            AllegroNewItems = 2
        }
        public enum ShippingCompany
        {
            Dpd = 1,
            PocztaPolska = 2,
            OdbiorOsobisty = 3,
            InPost = 4,
            TBA = 5,
            UPS=7,
            GLS=8,
            DHL =10,
            FedEX=11,
            InPostKurier=20
        }
        public enum Weekdays
        {
            Poniedzialek = 1,
            Wtorek = 2,
            Sroda = 3,
            Czwartek = 4,
            Piatek = 5,
            Sobota = 6,
            Niedziela = 7

        }
        public enum ShippingType
        {
            DpdCourier = 3,
            DpdCourierCOD = 4,
            SelfPaymentByDelivery = 5,
            SelfDelivery = 6,
            InpostLocker = 9,
            InpostLockerCOD = 10,
            International=13
        }
        public enum Warehouse
        {
            Przewodnia = 1,
            Pabianicka = 2,
            MagazynZewnetrzny = 3,
            Ekspozycja = 4
        }
        public enum OrderStatus
        {
            Temporary = -1,
            Deleted = 0,
            New = 1,
            Cancelled = 2,
            Sent = 3,
            ReadyToSend = 4,
            ClientContact = 5,
            Complaint = 6,
            Exported = 7,
            Comment = 8,
            WaitingForClient = 9,
            WaitingForPayment = 10,
            WaitingForProduct = 11,
            WaitingForDelivery = 12,
            ReadyToPickupByClient = 13,
            ReturnRecieved = 14,
            ReturnPayment = 15,
            CustomerNotification = 16,
            WaitingForAcceptance = 17
        }
        public enum OrderProductStatus
        {
            Deleted = 0,
            New = 1,
            Completed = 2,
            Ready = 3,
            Ordered = 4,
            Comment = 8
        }
        public enum FileImportStatus
        {
            Deleted = 0,
            New = 1,
            Processing = 2,
            Ok = 3,
            Error = 4,
            Imported = 5,
            ReadyToImport = 6
        }

        public class Formatowanie
        {
            private static string zero = "zero";
            private static string[] jednosci = { "", " jeden ", " dwa ", " trzy ",
        " cztery ", " pięć ", " sześć ", " siedem ", " osiem ", " dziewięć " };
            private static string[] dziesiatki = { "", " dziesięć ", " dwadzieścia ",
        " trzydzieści ", " czterdzieści ", " pięćdziesiąt ",
        " sześćdziesiąt ", " siedemdziesiąt ", " osiemdziesiąt ",
        " dziewięćdziesiąt "};
            private static string[] nascie = { "dziesięć", " jedenaście ", " dwanaście ",
        " trzynaście ", " czternaście ", " piętnaście ", " szesnaście ",
        " siedemnaście ", " osiemnaście ", " dziewiętnaście "};
            private static string[] setki = { "", " sto ", " dwieście ", " trzysta ",
        " czterysta ", " pięćset ", " sześćset ",
        " siedemset ", " osiemset ", " dziewięćset " };
            private static string[] tysiacePojedyncze = { "", " tysiąc ", " tysiące ",
        " tysiące ", " tysiące ", " tysięcy ", " tysięcy ",
        " tysięcy ", " tysięcy ", " tysięcy " };
            private static string tysiaceNascie = " tysięcy ";
            private static string[] tysiaceMnogie = {" tysięcy ", " tysięcy ", " tysiące ",
        " tysiące ", " tysiące ", " tysięcy ", " tysięcy ",
        " tysięcy ", " tysięcy ", " tysięcy "};
            public static string LiczbaSlownie(string languageCode, decimal liczba)
            {
                switch (languageCode)
                {
                    case "pl":
                        return LiczbaSlownieBase(liczba).Replace(" ", " ").Trim();
                    //case "en":
                    //    return LiczbaSlownieBase(liczba).Replace(" ", " ").Trim();
                }
                return "";
            }
            private static string LiczbaSlownieBase(decimal liczba)
            {
                StringBuilder sb = new StringBuilder();
                //0-999
                int wartosc = (int)liczba;
                if (wartosc == 0)
                    return zero;
                int jednosc = wartosc % 10;
                int para = wartosc % 100;
                int set = (wartosc % 1000) / 100;
                if (para > 10 && para < 20)
                    sb.Insert(0, nascie[jednosc]);
                else
                {
                    sb.Insert(0, jednosci[jednosc]);
                    sb.Insert(0, dziesiatki[para / 10]);
                }
                sb.Insert(0, setki[set]);

                //1000-999999
                wartosc = wartosc / 1000;
                jednosc = wartosc % 10;
                para = wartosc % 100;
                set = (wartosc % 1000) / 100;
                if ((wartosc % 1000) / 10 == 0)
                {
                    sb.Insert(0, tysiacePojedyncze[jednosc]);
                    if (jednosc > 1)
                        sb.Insert(0, jednosci[jednosc]);
                    sb.Append(String.Format(" zł {0}/100", PoPrzecinku(liczba)));
                    return sb.ToString();
                }
                if (para >= 10 && para < 20)
                {
                    sb.Insert(0, tysiaceNascie);
                    sb.Insert(0, nascie[para % 10]);
                }
                else
                {
                    sb.Insert(0, tysiaceMnogie[jednosc]);
                    sb.Insert(0, jednosci[jednosc]);
                    sb.Insert(0, dziesiatki[para / 10]);
                }
                sb.Insert(0, setki[set]);
                sb.Append(String.Format(" zł {0}/100", PoPrzecinku(liczba)));
                return sb.ToString();
            }

            private static int PoPrzecinku(decimal liczba)
            {
                return (int)((liczba - (int)liczba) * 100);
            }
        }


        public static int DefaultCompanyId { get { return 78; }}

        public static string GetDayOfWeek(int dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case 1: return "poniedziałek"; 
                case 2: return "wtorek"; 
                case 3: return "środa"; 
                case 4: return "czwartek"; 
                case 5: return "piątek"; 
                case 6: return "sobota"; 
                case 7: return "niedziela";  
                default: return "";
            }
        }
    }
}
