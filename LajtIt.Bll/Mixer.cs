using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LajtIt.Dal;

namespace LajtIt.Bll
{
    public class Mixer
    {
        public enum TagType
        {
            Square = 1,
            Angle = 2,
            Curly = 4
        }
        public static string GetProductName(int shopId, int productCatalogId)
        {

            return GetProductName(shopId, productCatalogId, false);
        }
        public static string GetProductName(int shopId, int productCatalogId, bool createNew)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(productCatalogId);

            Dal.ProductCatalogShopFnResult shop = pch.GetProductCatalogShops(productCatalogId)
                .Where(x => x.ShopId == shopId).FirstOrDefault();

            if (shop != null && !String.IsNullOrEmpty(shop.ShopProductName) && shop.IsNameLocked == true)
                return shop.ShopProductName;


            if (shop != null && !String.IsNullOrEmpty(shop.ShopProductName) && createNew == false)
                return shop.ShopProductName;

            string template = shop.ShopTemplate;

            if (!String.IsNullOrEmpty(shop.Template))
                template = shop.Template;

            return GetProductName(productCatalogId, pch, pc, shop, template);
        }
        internal static void SetProductNames(int productCatalogId, List<int> shopIds, bool createNew)
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            foreach (int shopId in shopIds)
            {
                string name = GetProductName(shopId, productCatalogId, createNew);

                pch.ProductCatalogShopProductName(productCatalogId, shopId, name);
            }
        }
        internal static void SetProductNames(int productCatalogId, List<int> shopIds)
        {
            SetProductNames(productCatalogId, shopIds, true);
        }
        public static string GetProductName(string template, int productCatalogId)
        {

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(productCatalogId);

            Dal.ProductCatalogShopFnResult shop = pch.GetProductCatalogShops(productCatalogId).Where(x => x.ShopId == (int)Dal.Helper.Shop.Lajtitpl).FirstOrDefault();



            return GetProductName(productCatalogId, pch, pc, shop, template);
        }
        private static string GetProductName(int productCatalogId, Dal.ProductCatalogHelper pch, ProductCatalog pc, ProductCatalogShopFnResult shop, string template)
        {
            template = GetOptions(template);


            string[] tags = GetTags(TagType.Square, template);

            string productName = template;

            int? sex = null;
            switch (shop.SexTypeId)
            {
                case 1: sex = 1; break;
                case 0: sex = 0; break;
                case 2: sex = 2; break;
                default: sex = Bll.Helper.GetRandomNumber(1, 2); break;
            }




            foreach (string tag in tags)
            {
                string t = tag.Replace("[", "").Replace("]", "");
                Dal.ProductCatalogAttribute attribute = pch.GetProductCatalogAttributeGroup(productCatalogId, t);

                if (attribute != null)
                {
                    string friendlyName = "";

                    switch (sex.Value)
                    {
                        case 1: friendlyName = attribute.FriendlyNameM; break;
                        case 0: friendlyName = attribute.FriendlyNameF; break;
                        case 2: friendlyName = attribute.FriendlyNameN; break;
                    }
                    if (String.IsNullOrEmpty(friendlyName))
                        friendlyName = "";

                    friendlyName = GetRandom(friendlyName);

                    productName = productName.Replace(tag, friendlyName + " ");
                }
                else
                {
                    Dal.ProductCatalogAttributeToProduct att = pch.GetProductCatalogAttributeValue(productCatalogId, t);


                    if (att != null)
                    {
                        switch (att.ProductCatalogAttribute.AttributeTypeId)
                        {
                            case 1: productName = productName.Replace(tag, GetFieldValue(att, att.DecimalValue)); break;
                            case 2: productName = productName.Replace(tag, GetFieldValue(att, att.StringValue)); break;
                            default: productName = productName.Replace(tag, String.Format("{0} ", "")); break;
                        }
                    }
                    else
                    {
                        switch (t)
                        {
                            case "PRODUCENT": productName = productName.Replace(tag, pc.Supplier.Name + " "); break;
                            case "LINIA": productName = productName.Replace(tag, GetLine(pc) + " "); break;
                            case "EAN": productName = productName.Replace(tag, pc.Ean + " "); break;
                            case "KOD": productName = productName.Replace(tag, pc.Code + " "); break;
                            case "KOD2": productName = productName.Replace(tag, pc.Code2 + " "); break;
                            default: productName = productName.Replace(tag, ""); break;
                        }


                    }
                }
            }
            string p = productName.Replace("  ", " ").TrimEnd().Trim();
            if (String.IsNullOrEmpty(p))
                return p;
            else
                return p.First().ToString().ToUpper() + p.Substring(1);
        }


        public static string GetDescription(Dal.Helper.Shop shop, int productCatalogId)
        {

            Dal.ShopHelper sh = new Dal.ShopHelper();
            Dal.MixerHelper m = new MixerHelper();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            StringBuilder sb = new StringBuilder();

            #region Product

            Dal.ProductCatalogShopFnResult shopProduct = pch.GetProductCatalogShops(productCatalogId).Where(x => x.ShopId == (int)shop).FirstOrDefault();
            int? sex = null;

            switch (shopProduct.SexTypeId)
            {
                case 1: sex = 1; break;
                case 0: sex = 0; break;
                case 2: sex = 2; break;
                default: sex = Bll.Helper.GetRandomNumber(1, 2); break;
            }
            Dal.ProductCatalog pc = Dal.DbHelper.ProductCatalog.GetProductCatalog(productCatalogId);
            #endregion

            // pobiera jednen losowy opis dla każdej z grup atrybutów
            List<Dal.ProductCatalogMixerAttributeGroupRandomFnResult> mixerGroups = m.GetProductCatalogMixerAttributeGroupRandom(productCatalogId, shop);

            // pobranie grup ułożonych w odpowiedniej kolejności
            List<Dal.ProductCatalogAttributeGroupsForShopResult> attributesForShop = sh.GetProductCatalogAttributeGroupsForShop(shop, productCatalogId);

            var groups = attributesForShop.OrderBy(x => x.Order).Select(x => new
            {
                x.AttributeGroupId,
                AttributeGroupName = x.GroupName
                //x.AttributeGroupTypeId

            }).Distinct().ToList();




            foreach (Dal.ProductCatalogMixerAttributeGroupRandomFnResult mixerGroup in mixerGroups)
            {
                //ProductCatalogMixerAttributeGroupRandomFnResult mixerGroup = mixerGroups.Where(x => x.AttributeGroupId == g.AttributeGroupId).FirstOrDefault();

                if (mixerGroup == null || (mixerGroup.TemplateF == null && mixerGroup.TemplateM == null && mixerGroup.AttributeGroupTypeId != 3))
                    continue;
                string groupDesc = null;

                switch (mixerGroup.AttributeGroupTypeId)
                {
                    case 1:
                    case 2:
                        string desc1 = "";// sex.Value ? mixerGroup.TemplateM : mixerGroup.TemplateF;

                        switch (sex.Value)
                        {
                            case 1: desc1 = mixerGroup.TemplateM; break;
                            case 0: desc1 = mixerGroup.TemplateF; break;
                            case 2: desc1 = mixerGroup.TemplateN; break;
                        }


                        groupDesc = NewMethod(pc, desc1, sex);
                        if (groupDesc != null)
                            sb.AppendLine(String.Format("<p>{0}</p>", groupDesc));
                        break;
                    case 3:
                        // pobierz atrybuty dla tej grupy

                        List<Dal.ProductCatalogAttribute> attributes = Dal.DbHelper.ProductCatalog.GetProductCatalogAttributes(mixerGroup.AttributeGroupId);
                        // iteruj po każdym, wybierz losowo szablon

                        foreach (Dal.ProductCatalogAttribute attribute in attributes)
                        {
                            Dal.ProductCatalogMixerAttributeGroup mag = pch.GetProductCatalogMixerAttributeGroup(mixerGroup.AttributeGroupId, attribute.AttributeId);

                            if (mag == null)
                                continue;

                            string desc = ""; //sex.Value ? mag.TemplateM : mag.TemplateF;
                            switch (sex.Value)
                            {
                                case 1: desc = attribute.FriendlyDescriptionM; break;
                                case 0: desc = attribute.FriendlyDescriptionF; break;
                                case 2: desc = attribute.FriendlyDescriptionN; break;
                            }



                            groupDesc = NewMethod(pc, desc, sex);
                            if (groupDesc != null)
                                sb.AppendLine(String.Format("<p>{0}</p>", groupDesc));


                        }


                        break;
                }




            }



            //sb.AppendLine(name);
            return sb.ToString();
        }

        private static string NewMethod(ProductCatalog pc, string groupDesc, int? sexTypeId)
        {

            //if (String.IsNullOrEmpty(groupDesc))
            //    return "";

            #region statyczny opis

            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();

            string[] tags = GetTags(TagType.Curly, groupDesc);

            foreach (string tag in tags)
            {
                string tag1 = tag.Replace("{", "").Replace("}", "");

                string t = GetRandom(tag1);

                groupDesc = groupDesc.Replace(tag, t);

            }
            if (!String.IsNullOrEmpty(groupDesc))
                groupDesc = groupDesc.First().ToString().ToUpper() + groupDesc.Substring(1);

            #endregion


            #region dynamiczne tagi 


            tags = GetTags(TagType.Square, groupDesc);

            foreach (string tag in tags)
            {
                string tag1 = tag.Replace("[", "").Replace("]", "");


                Dal.ProductCatalogAttribute attribute = pch.GetProductCatalogAttributeGroup(pc.ProductCatalogId, tag1);

                if (attribute != null)
                {
                    string friendlyName = "";

                    switch (sexTypeId.Value)
                    {
                        case 1: friendlyName = attribute.FriendlyDescriptionM; break;
                        case 0: friendlyName = attribute.FriendlyDescriptionF; break;
                        case 2: friendlyName = attribute.FriendlyDescriptionN; break;
                    }
                    if (String.IsNullOrEmpty(friendlyName))
                        friendlyName = "";

                    friendlyName = GetRandom(friendlyName);

                    groupDesc = groupDesc.Replace(tag, friendlyName);
                }
                else
                {
                    Dal.ProductCatalogAttributeToProduct att = pch.GetProductCatalogAttributeValue(pc.ProductCatalogId, tag1);


                    if (att != null)
                    {
                        switch (att.ProductCatalogAttribute.AttributeTypeId)
                        {
                            case 1: groupDesc = groupDesc.Replace(tag, GetFieldValue(att, att.DecimalValue)); break;
                            case 2: groupDesc = groupDesc.Replace(tag, GetFieldValue(att, att.StringValue)); break;
                            default: groupDesc = groupDesc.Replace(tag, String.Format("{0} ", "")); break;
                        }
                    }
                    else
                    {
                        switch (tag1)
                        {
                            case "PRODUCENT": groupDesc = groupDesc.Replace(tag, pc.Supplier.Name); break;
                            case "LINIA": groupDesc = groupDesc.Replace(tag, GetLine(pc)); break;
                            case "EAN": groupDesc = groupDesc.Replace(tag, pc.Ean); break;
                            case "KOD": groupDesc = groupDesc.Replace(tag, pc.Code); break;
                            case "KOD2": groupDesc = groupDesc.Replace(tag, pc.Code2); break;
                                ; default: groupDesc = null; break;//  groupDesc.Replace(tag, ""); break;
                        }


                    }
                }


                //groupDesc = groupDesc.Replace(tag, t);

            }
            if (!String.IsNullOrEmpty(groupDesc))
                groupDesc = groupDesc.First().ToString().ToUpper() + groupDesc.Substring(1);
            if (!String.IsNullOrEmpty(groupDesc) && groupDesc.EndsWith(" ."))
                groupDesc = groupDesc.Replace(" .", ".");

            #endregion
            return groupDesc;
        }

        private static string GetFieldValue(ProductCatalogAttributeToProduct att, object v)
        {

            if (att.ProductCatalogAttribute.FieldTemplate == null)
                return String.Format("{0} ", v);
            else
                return String.Format(att.ProductCatalogAttribute.FieldTemplate + " ", v);
        }

        private static string GetLine(ProductCatalog pc)
        {
            Dal.ProductCatalogGroupHelper pchg = new ProductCatalogGroupHelper();

            var g = pchg.GetProductCatalogLine(pc.ProductCatalogId);

            if (g == null)
                return "";
            else
                return g.GroupName;
        }

        private static string GetOptions(string template)
        {
            string pattern = @"\(([^)]*)\)";// (<div.*>)(.*)(<\\/div>)";

            MatchCollection matches = Regex.Matches(template, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Console.WriteLine("Matches found: {0}", matches.Count);

            if (matches.Count > 0)
                foreach (Match m in matches)
                {
                    Console.WriteLine("Inner DIV: {0}", m.Groups[0]);

                    string[] tags = GetTags(TagType.Square, m.Groups[0].Value.Replace("(", "").Replace(")", ""));//.Cast<Group>().Select(x => x.Value.Replace("(", "").Replace(")", "")).ToArray());

                    template = template.Replace(m.Groups[0].Value, tags.OrderBy(x => Guid.NewGuid()).FirstOrDefault());
                }
            //var a= matches.Cast<Match>().Select(x => x.Value).ToArray();

            return template;
        }

        private static string GetRandom(string friendlyName)
        {
            if (friendlyName == null)
                return null;

            string[] friendlyNames = friendlyName.Split('|');

            return friendlyNames.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }

        private static string[] GetTags(TagType tagType, string template)
        {
            if (template == null)
                return new string[0];

            string pattern = @"\[([^]]*)\]";// (<div.*>)(.*)(<\\/div>)";


            switch (tagType)
            {
                case TagType.Curly:
                    pattern = @"\{([^}]*)\}"; break;
            }


            MatchCollection matches = Regex.Matches(template, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Console.WriteLine("Matches found: {0}", matches.Count);

            if (matches.Count > 0)
                foreach (Match m in matches)
                    Console.WriteLine("Inner DIV: {0}", m.Groups[0]);

            return matches.Cast<Match>().Select(x => x.Value).ToArray();
        }

        internal static void SetProductNames(bool onlyActive)
        {
            List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops().Where(x => x.IsActive && x.CanExportProducts).ToList();
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            List<Dal.ProductCatalog> products = pch.GetProductCatalog(onlyActive).ToList();

            foreach (Dal.Shop shop in shops)
            {
                foreach (Dal.ProductCatalog product in products)
                {
                    string name = GetProductName(shop.ShopId, product.ProductCatalogId);

                    pch.SetProductCatalogName(shop.ShopId, product.ProductCatalogId, name);
                }
            }
        }
        internal static void SetProductDescriptions(bool onlyActive)
        {

            List<Dal.ProductCatalogMissingDescriptions> productsWithoutDescription =
                Dal.DbHelper.ProductCatalog.GetProductsWithoutDescription();

            foreach (var product in productsWithoutDescription)
            {
                Dal.Helper.Shop shop = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), product.ShopId);



                Dal.ProductCatalogShopProduct psp = new ProductCatalogShopProduct()
                {
                    ShopId = product.ShopId,
                    ProductCatalogId = product.ProductCatalogId,
                    LongDescription = GetDescription(shop, product.ProductCatalogId)
                };

                Dal.DbHelper.ProductCatalog.SetProductCatalogShopProductDescription(psp);
            }
        }
    }
}
