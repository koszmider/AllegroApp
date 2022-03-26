using LajtIt.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace LajtIt.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine(Environment.UserName);
            //Console.WriteLine("C - pobierz kategorie");
            //Console.WriteLine("U - pobierz aukcje użytkowników");
            //Console.WriteLine();

            if (args.Length == 0)
            {
                LajtIt.Bll.ErrorHandler.LogError(new Exception("Empty parameters list"), "Program.cs");
                return;
            }
            string key = args[0];// Console.ReadLine().ToUpper();

            if (Bll.Helper.BatchProcessingIsLock && args.Where(x=>x=="NOLOCK").Count()==0)
            {
                return;
            }



            LajtIt.Bll.AllegroScan allegroScan = new LajtIt.Bll.AllegroScan();

 
            allegroScan.SetEnv();

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.MaxServicePointIdleTime = 0;
            try
            {
                switch (key)
                {
                    //case "ORDERS_RECEIPT": // ok2
                    //    Bll.OrderHelper.SetOrdersReceipt(); 
                    //    break;
                    #region Allegro


                    //case "COSTS":// ok2
                    //    allegroScan.SetAllegroCosts();
                    //    break;
                    //case "PROCESS_END": // ok2
                    //    allegroScan.GetCategories();
                    //    allegroScan.ProcessEndJournal("Auto");
                    //    break;               
                    //case "PROCESS_ME": // ok2
                    //    allegroScan.GetMyJournal();
                    //    allegroScan.GetMyJournalDeals();
                    //    allegroScan.ProcessMyJournal("Auto");
                    //    break;

                    case "CHECK_UPDATES":
                        allegroScan.ManageUpdates();
                        break;

                    case "CHECK_PROMOS":
                        allegroScan.ManagePromotions();
                        break;

                    case "GENERATE_SALES_FILE":
                        Bll.SalesFileHelper.GenerateSellFileReport();
                        break;

                    case "ALLEGRO_UPDATE_PRODUCTS":
                        allegroScan.AllegroUpdateProducts();
                        break;

                    case "ALLEGRO_VAT": // ok2
                        Bll.AllegroRESTHelper.Batch.UpdateVatResult();
                        Bll.AllegroRESTHelper.Batch.UpdateVat();
                        break;
                    case "ALLEGRO_PAYMENTS": // ok2
                        Bll.AllegroRESTHelper.Payments.GetPayments();
                        break;
                    case "ALLEGRO_BADGES": // ok2
                        Bll.AllegroRESTHelper.Badges.SetBadges();
                        break;
                    case "ALLEGRO_BADGES_STATUS": // ok2
                        Bll.AllegroRESTHelper.Badges.GetBadgeCampaignApplications();
                        break;
                    case "ALLEGRO_DISPUTES": // ok2
                        Bll.AllegroRESTHelper.Disputes.GetDisputes();
                        break;
                    case "ALLEGRO_EMAIL_RESPONDER": // ok2
                        Bll.AllegroEmailResponder.ReadMailbox();
                        break;
                    case "ALLEGRO_CATEGORIES": // ok2
                        Bll.AllegroRESTHelper.Categories.GetCategories();
                        break;
                    case "ALLEGRO_ACTIONS": // ok2
                        allegroScan.SetAllegroAction("Auto");
                        break;
                    case "ALLEGRO_UPDATE_BY_CATALOG": // ok2
                        Bll.ShopUpdateHelper.ProcessClickShopSingle(Dal.Helper.ShopType.Allegro);
                        break;
                    case "ALLEGRO_TOKEN_REFRESH": // ok2
                        allegroScan.SetAllegroTokenRefresh("Auto");
                        break;
                    case "ALLEGRO_TAGS": // ok2
                        allegroScan.SetAllegroTags("Auto");
                        break;
                    //case "ALLEGRO_SET_PROMOTIONS": // ok
                    //    allegroScan.SetAllegroPromotions("Auto");
                    //    break;
                    //case "ALLEGRO_DELETE_PROMOTIONS": // ok
                    //    allegroScan.SetAllegroPromotionsDelete("Auto");
                    //    break;
                    //case "ALLEGRO_DELETE_ALL_PROMOTIONS": // ok
                    //    allegroScan.SetAllegroPromotionsDeleteAll("Auto");
                    //    break;
                    case "ALLEGRO_UPDATE_ITEMS": // ok
                        allegroScan.SetAllegroUpdateItems("Auto");
                        break;
                    case "ALLEGRO_UPDATE_OFFER_PRODUCT": // ok
                        allegroScan.SetAllegroUpdateOfferProduct("Auto");
                        allegroScan.GetAllegroProductsFromOffers("Auto");
                        break;

                    case "ALLEGRO_DELETE_DRAFTS": // ok
                        Bll.AllegroRESTHelper.DeleteDraft();
                        break;
                    case "ALLEGRO_GET_PRODUCTS_FROM_OFFERS": // ok
                        Bll.AllegroRESTHelper.GetProductsFromOffers();
                        break;
                    //case "ALLEGRO_GET_PRODUCTS_NEW": // ok
                    //    Bll.AllegroRESTHelper.Products.GetProductsNew();
                    //    break;
                    //case "ALLEGRO_GET_PRODUCTS_UPDATE": // ok
                    //    Bll.AllegroRESTHelper.Products.GetProductsUpdate();
                    //    break;
                    //case "ALLEGRO_OFFERS_RESTORE": // ok
                    //    Bll.AllegroRESTHelper.SetOffersRestore();
                    //    break;




                    //case "ALLEGRO_PARCEL_NUMBER": // ok
                    //    allegroScan.SetAllegroParcelNumber("Auto");
                    //    break;
                    case "ALLEGRO_CREATE_OFFER_DRAFT": // ok
                        Bll.AllegroRESTHelper.CreateDraft();
                        Bll.AllegroRESTHelper.ReActivate();
                        break;
                    case "ALLEGRO_UPDATE_OFFER_DRAFT": // ok
                        Bll.AllegroRESTHelper.UpdateDraft();
                        break;
                    //case "ALLEGRO_CREATE_IMAGES": // ok
                    //    Bll.AllegroRESTHelper.CreateImages();
                    //    break;
                    case "ALLEGRO_PUBLISH_OFFERS": // ok
                        Bll.AllegroRESTHelper.PublishOffers();
                        break;
                    case "ALLEGRO_PUBLISH_OFFERS_CHECK": // ok
                        Bll.AllegroRESTHelper.PublishOffersCheck();
                        break;
                    case "ALLEGRO_PUBLISH_OFFERS_DETAILS": // ok
                        Bll.AllegroRESTHelper.PublishOffersDetails();
                        break;
                    case "ALLEGRO_UPDATE_OFFER": // ok2
                        Bll.AllegroRESTHelper.UpdateOffer();
                        break;
                    case "ALLEGRO_FEES": // ok2
                        Bll.AllegroRESTHelper.GetFees();
                        break;
                    case "ALLEGRO_VARIANTS": // ok2
                        Bll.AllegroRESTHelper.Variants.UpdateVariants();
                        break;
                    case "ALLEGRO_VARIANTS_DELETE": // ok2
                        Bll.AllegroRESTHelper.Variants.DeleteVariants();
                        break;
                    case "ALLEGRO_TEST": // ok2
                        break;
                    case "ALLEGRO_MISSING_OFFERS": // ok2
                        Bll.AllegroRESTHelper.GetMissingOffers();
                        break;
                    //case "ALLEGRO_OFFERS_CHECK_STATUS": // ok2
                    //    Bll.AllegroRESTHelper.GetOffersStatus();
                    //    break;
                    //case "ALLEGRO_OFFERS_GET_JSON": // ok2
                    //    Bll.AllegroRESTHelper.GetOffersJson();B
                    //    break;
                    case "ALLEGRO_ORDERS": // ok2
                        Bll.AllegroRESTHelper.Orders.GetOrders();
                        Bll.AllegroRESTHelper.Orders.Process();
                        Bll.AllegroRESTHelper.Orders.SetShipment();
                        break;  
                    case "ALLEGRO_BILLING": // ok2
                        Bll.AllegroRESTHelper.Billing.GetBillingTypes();
                        Bll.AllegroRESTHelper.Billing.GetBilling();
                        break;
                    case "ALLEGRO_TEST1": // ok2
                        //Bll.AllegroRESTHelper.GetOffer(8088295637);
                        break;

                    #endregion

                    case "SHOP_ORDER_SENT": // ok2
                        Bll.ShopHelper.SetOrderAsSent(Dal.Helper.Shop.Morele); 
                        break;
                    #region Sklep lajtit.pl
                    case "SHOP": // ok2
                        allegroScan.GetShopOrders("Auto");
                        break;
                    case "SHOP_PRODUCERS": // ok2
                        allegroScan.GetShopProducers("Auto");
                        break;

                    //case "SHOP_PRODUCTS_TO_CATALOG": // ok2
                    //    allegroScan.SetShopProductsToCatalog();
                    //    break;
                    case "SHOP_CATEGORY_TREE": // ok2
                        allegroScan.GetShopCategories();
                        break;
                    case "SHOP_CONFIGURATION": // ok2
                        allegroScan.SetShopConfiguration();
                        break;
                    //case "SHOP_ATTRIBUTES_TO_CATEGORY": // ok2
                    //   // allegroScan.SetShopAttributesToCategory();
                    //    break;


                    case "SHOP_PROMOTIONS_DELETE":
                        Bll.ShopRestHelper.SpecialOffers.SetPromotionsDelete( Dal.Helper.Shop.Lajtitpl);
                        break;
                    case "SHOP_UPDATE_CLICK_SHOP_SINGLE":
                        Bll.ShopUpdateHelper.ProcessClickShopSingle(Dal.Helper.ShopType.ShoperLajtitPl);
                        break;
                    case "SHOP_ERLI_UPDATE_CLICK_SHOP_SINGLE": 
                        Bll.ShopUpdateHelper.ProcessClickShopSingle(Dal.Helper.ShopType.Erli);
                        break;
                    case "SHOP_UPDATE_CLICK_SHOP_BATCH":
                        Bll.ShopUpdateHelper.ProcessClickShopBatch(Dal.Helper.ShopType.ShoperLajtitPl);
                        break;
                    case "SHOP_CLIPPERON_UPDATE_CLICK_SHOP_BATCH":
                        Bll.ShopUpdateHelper.ProcessClickShopBatch(Dal.Helper.ShopType.Clipperon);
                        break;
                    case "SHOP_UPDATE_CLICK_SHOP_BATCH1":
                        Bll.ShopUpdateHelper.ProcessClickShopBatch(Dal.Helper.ShopType.ShoperOswietlenieTechnicznePl);
                        break;


                    //case "SHOP_OPTIONS": // ok
                    //    allegroScan.SetShopoptions();
                    //    break;
                    case "SHOP_MAIN_PAGE": // ok
                        allegroScan.SetShopMainPage();
                        break;
                    ////case "SHOP_UPDATE_BY_CATALOG_BATCH": // ok2
                    ////    //allegroScan.SetShopUpdateByCatalogBatch("Auto");
                    ////    break;
                    ////case "SHOP_UPDATE_BY_CATALOG_SINGLE": // ok2
                    ////    //allegroScan.SetShopUpdateByCatalogSingle("Auto");
                    ////    break;
                    case "SHOP_SET_PRODUCTS_ORDER": // ok2
                        allegroScan.SetShopProductsUpdate("Auto", Dal.Helper.Shop.Lajtitpl);
                        break;
                    case "SHOP_UPDATE_STATUS": // ok2 
                        allegroScan.ShopUpdateOrderStatus();
                        break;
                    case "SHOP_SYNC": // ok2 
                        ConsoleHelper.ShopSync();
                        break;
                    case "SHOP_TEST": // ok2 
                        ConsoleHelper.ShopTest();
                        break;
                    case "SHOP_CATEGORY_MANAGER": // ok2 
                        Bll.ShopHelper.SetShopCategories(); break;


                    #endregion

                    #region Clipperon

                    case "CLIPPERON_PRODUCTS": // ok
                        LajtIt.Bll.ClipperonRestHelper.Products.GetProducts(Dal.Helper.Shop.Clipperon);
                        //LajtIt.Bll.ClipperonRestHelper.Products.SetProducts(Dal.Helper.Shop.Clipperon);
                        break;

                    case "CLIPPERON_ORDERS": // ok

                      //LajtIt.Bll.ClipperonRestHelper.Orders.RestoreOrder(Dal.Helper.Shop.Clipperon);
                        LajtIt.Bll.ClipperonRestHelper.Orders.GetOrders(Dal.Helper.Shop.Clipperon);
                        LajtIt.Bll.ClipperonRestHelper.Orders.SetOrderStatus(Dal.Helper.Shop.Clipperon);
                        //LajtIt.Bll.ClipperonRestHelper.Products.SetProducts(Dal.Helper.Shop.Clipperon);
                        break;

                    #endregion
                    #region Inne
                    case "CENEO_ZAMOWIENIA": // ok
                        allegroScan.GetCeneoOrders("Auto");
                        break;
                    case "CENEO_BIDS": // ok 
                        allegroScan.GetCeneoBids("Auto");
                        break;
                    case "CENEO_BIDS_SET": // ok
                        allegroScan.SetCeneoBids("Auto"); 
                        break;
                    case "CENEO_CATEGORY_TREE": // ok
                        allegroScan.GetCeneoCategories("Auto");
                        break;
                    case "SYSTEM_SHOP_NAMES": // ok
                        allegroScan.SetProductNames("Auto");
                        break;
                    case "SYSTEM_SHOP_DESCRIPTIONS": // ok
                        allegroScan.SetProductDescriptions("Auto");
                        break;

                    #endregion
                    #region Inne
                    case "PACZKOMATY": // ok
                        allegroScan.SetPaczkomaty("Auto");
                        break;
                    case "IMPORT_PRODUCTS": // ok2
                        allegroScan.SetImportProducts("Auto");
                        break;
                    case "IMPORT_CHECK_PRODUCTS": // ok2
                        allegroScan.SetImportProductsCheck("Auto");
                        break;
                    case "FTP": // ok2 
                        allegroScan.Ftp();
                        break;
                    case "FTP_SUPPLIER_ID": // ok2 
                        allegroScan.FtpSupplierId(Convert.ToInt32(args[1]));
                        break;
                    case "FTP_LIST": // ok2 
                        allegroScan.FtpList();
                        break;

                    //case "CENEO_FILE": // ok2 
                    //    allegroScan.CeneoFile();
                    //    break;
                    //case "EMAG_FILE": // ok2 
                    //    allegroScan.EmagFile();
                    //    break;
                    //case "HOMEBOOK_FILE": // ok2 
                    //    allegroScan.HomebookFile();
                    //    break;
                    //case "EMAG_CATEGORY_TREE": // ok2 
                    //    Bll.EmagRESTHelper.GetCategory(); ;
                    //    break;
                    //case "EMAG_ORDERS": // ok2 
                    //    Bll.EmagRESTHelper.GetOrders(); ;
                    //    break;
                    //case "EMAG_PRODUCTS": // ok2 
                    //    Bll.EmagRESTHelper.SetProducts(); ;
                    //    break;
                    case "EMPIK_ORDERS": // ok2 
                        Bll.EmpikRESTHelper.Orders.GetOrders(); ;
                        break;
                    case "EMPIK_OFFERS": // ok2 
                                         // var o = Bll.EmpikRESTHelper.Offers.Upload("https://marketplace.empik.com/api/offers/imports");

                        Bll.EmpikRESTHelper.Offers.GetOffers(Dal.Helper.Shop.Empik);

                        Bll.EmpikRESTHelper.Offers.GetFile(Dal.Helper.Shop.Empik);
                        break;
                    case "ERLI_UPDATE_CLICK_SHOP_SINGLE":
                        Bll.ShopUpdateHelper.ProcessClickShopSingle(Dal.Helper.ShopType.Erli);
                        break;
                    case "ERLI_ORDERS": // ok2  
                        Bll.ErliRESTHelper.Orders.GetOrders(Dal.Helper.Shop.Erli);
                        Bll.ErliRESTHelper.Orders.GetOrderPayments(Dal.Helper.Shop.Erli);
                        Bll.ErliRESTHelper.Orders.SetOrderStatus(Dal.Helper.Shop.Erli);
                        break;

                    case "MORELE_REGISTER_TOKEN":
                        Bll.MoreleRESTHelper.GetToken();
                        break;

                    case "MORELE_REFRESH_TOKEN":
                        Bll.MoreleRESTHelper.GetRT();
                        break;

                    case "MORELE_ORDERS": // ok2  
                        //Bll.MoreleRESTHelper.GetToken();
                        //Bll.MoreleRESTHelper.GetRefreshToken();
                        Bll.MoreleRESTHelper.Orders.GetOrders(); ;
                        Bll.ShopHelper.SetOrderAsSent(Dal.Helper.Shop.Morele);
                        break;
                    //case "CENEO_FILE_COPY": // ok2 
                    //    allegroScan.CeneoFileCopy();
                    //    break;
                    case "LAJTIT_IMAGES":
                        ConsoleHelper.LajtitplSetImages(252257);
                        break;
                    case "LAJTIT_TEST": // ok 2
                        allegroScan.TEST();
                        break;
                    case "LAJTIT_UPDATE_STOCK": // ok 2
                        allegroScan.LajtitUpdateStock();
                        break;

                    case "LAJTIT_FOLLOW_OBJECTS": // ok 2
                        allegroScan.FollowObjects();
                        break;

                    //case "INPOST_GET_TRACKING": // ok 2
                    //    allegroScan.SetInpostTracking();
                    //    break;
                    case "LAJTIT_SET_TRACKING": // ok 2
                        Bll.OrderHelper.SetShippingNumbers();
                        break;
                 
                    case "LAJTIT_ORDERS_TO_FINISH": // ok 2
                        Bll.OrderHelper.GetOrdersToComplete();
                        break;

                    #endregion


                    #region Integracje produktów
                    case "STEP_INTO_DESIGN_IMPORT": // ok 
                        allegroScan.StepIntoDesignImport();
                        break;
                    case "ALTAVOLA_IMPORT": // ok 
                        allegroScan.AltavolaImportCeneo();
                        break;
                    case "REDLUX_IMPORT": // ok 
                        allegroScan.RedluxImport();
                        break;
                    case "LAMPEX_IMPORT": // ok2 
                        allegroScan.LampexImport();
                        break;
                    case "CLEONI_IMPORT": // ok2 
                        allegroScan.CleoniImport();
                        break;

                    //case "ALTAVOLA_IMPORT_STATUS": // ok 
                    //    allegroScan.AltavolaImportStatus();
                    //    break;
                    case "SOLLUX_IMPORT": // ok 2
                        allegroScan.SolluxImport();
                        break;
                    case "LIGHTPRESTIGE_IMPORT": // ok 2
                        allegroScan.LightPrestigeImport();
                        break;
                    case "NORDLUX_IMPORT": // ok2 
                        allegroScan.NordluxImport();
                        break;
                    case "MAXLIGHT_IMPORT": // ok2 
                        allegroScan.MaxligthImport();
                        break;
                    case "AUHILON_IMPORT": // ok2 
                        allegroScan.AuhilonImport();
                        break;
                    case "ARGON_IMPORT": // ok2 
                        allegroScan.ArgonImport();
                        break;
                    case "COSMOLIGHT_IMPORT": // ok2 
                        allegroScan.CosmoLightImport();
                        break;
                    case "HUBSCH_IMPORT": // ok2 
                        allegroScan.HubschImport();
                        break;
                    case "ZUMALINE_IMPORT": // ok2 
                        allegroScan.ZumalineImport();
                        break;
                    case "PN_LD_IMPORT": // ok2 
                        allegroScan.ZumalinePNLDImport();
                        break;
                    case "PAULMANN_IMPORT": // ok2 
                        allegroScan.PaulmannImport();
                        break;
                    case "CANDELLUX_IMPORT": // ok2 
                        allegroScan.CandelluxImport();
                        break;
                    case "ELSTEAD_IMPORT": // ok2 
                        allegroScan.ElsteadImport();
                        break;
                    case "LUCIDE_IMPORT": // ok2 
                        allegroScan.LucideImport();
                        break;
                    case "SUMA_IMPORT": // ok2 
                        allegroScan.SuMaImport();
                        break;
                    case "LUTEC_IMPORT": // ok2 
                        allegroScan.LutecImport();
                        break;
                    case "GLASBERG_IMPORT": // ok2 
                        allegroScan.GlasbergImport();
                        break;
                    case "NOVALUCE_IMPORT": // ok2 
                        allegroScan.NovaLuceImport();
                        break;
                    case "TRIO_IMPORT": // ok2 
                        allegroScan.TrioImport();
                        break;
                    case "RL_IMPORT": // ok2 
                        allegroScan.RLImport();
                        break; 
                    case "BRILLIANT_IMPORT": // ok2 
                        allegroScan.BrilliantImport();
                        break;
                    case "APETI_IMPORT": // ok2 
                        allegroScan.ApetiImport();
                        break;
                    case "AZZARDO_IMPORT": // ok2 
                        allegroScan.AZzardoImport();
                        break;
                    case "MAYTONI_IMPORT": // ok2 
                        allegroScan.MaytoniImport();
                        break;
                    case "ITALUX_IMPORT": // ok2 
                        allegroScan.ItaluxImport();
                        break;
                    case "TK_IMPORT": // ok2 
                        allegroScan.TKImport();
                        break;
                    case "MARKSLOJD_IMPORT": // ok2
                        allegroScan.MarkslojdImport();
                        break;
                    case "EGLO_IMPORT": // ok2
                        allegroScan.EgloImport();
                        break;
                    case "RABALUX_IMPORT": // ok2 
                        allegroScan.RabaluxImport();
                        break;
                    case "POLNED_IMPORT": // ok2 
                        allegroScan.PolnedImport();
                        break;
                    case "NOWODVORSKI_IMPORT": // ok2 
                        allegroScan.NowodvorskiImport();
                        break;
                    case "MILAGRO_IMPORT": // ok2 
                        allegroScan.MilagroImport();
                        break;
                    case "SOMPEX_IMPORT": // ok2 
                        allegroScan.SompexImport();
                        break;
                    case "POLUX_IMPORT": // ok2 
                        allegroScan.PoluxImport();
                        break;
                    case "KING_HOME_IMPORT": // ok2 
                        allegroScan.KingHomeImport();
                        break;

                    #endregion


                    #region tmp
                    case "SYSTEM_EXPORT_FILES_CENEO":
                        ShopExportFileHelper.ExportFile(Dal.Helper.Shop.Lajtitpl, Dal.Helper.Shop.Ceneo); break;
                    case "SYSTEM_EXPORT_FILES_MORELE":
                        ShopExportFileHelper.ExportFile(Dal.Helper.Shop.Lajtitpl, Dal.Helper.Shop.Morele); break;
                    case "SYSTEM_EXPORT_FILES_EMPIK":
                        ShopExportFileHelper.ExportFile(Dal.Helper.Shop.Lajtitpl, Dal.Helper.Shop.Empik); break;
                    case "SYSTEM_EXPORT_FILES_FB":
                        ShopExportFileHelper.ExportFile(Dal.Helper.Shop.Lajtitpl, Dal.Helper.Shop.FB); break;
                    case "SYSTEM_EXPORT_FILES_PGE":
                        ShopExportFileHelper.ExportFile(Dal.Helper.Shop.Lajtitpl, Dal.Helper.Shop.PGE); break;
                    case "REST": // ok
                        allegroScan.SetRest("Auto");
                        break;
                    case "IMPORT_IMAGES": // ok
                        int supplierId = Convert.ToInt32(args[1]);
                        string dir = args[2];
                        allegroScan.SetImportProductImages("Auto", supplierId, dir);
                        break;
                    case "IMPORT_IMAGES_ADHOC": // ok 
                        allegroScan.SetImportProductImagesAdHoc("Auto", Convert.ToInt32(args[1]), args[2]);
                        break;
                    case "IMPORT_IMAGES_BY_CODE_NOT_EXISTING": // ok 
                        allegroScan.SetImportProductImagesByCodeAddNotExisting("Auto", Convert.ToInt32(args[1]), args[2]);
                        break;
                    case "IMPORT_IMAGES_BY_CATALOG": // ok
                        int s = Convert.ToInt32(args[1]);
                        string d = args[2];
                        allegroScan.SetImportProductImagesByCatalog("Auto", s, d);
                        break;
                    case "IMPORT_IMAGES_BY_CATALOG_ADD": // ok
                        allegroScan.SetImportProductImagesByCatalogAdd("Auto", Convert.ToInt32(args[1]), args[2]);
                        break;
                    case "TEST_TLS": // ok 
                        allegroScan.Tls();
                        break;
                    case "SHOP_REFRESH_IMAGES": // ok 
                        //allegroScan.ShopRefreshImages();
                        break;
                    case "IMAGES_MISSING":
                        allegroScan.CheckMisssingImages(); break;
                    case "IMAGES_NOT_IN_DB":
                        allegroScan.CheckMisssingImagesInDb(); break;
                    case "IMAGES_UPDATE_SIZE":
                        ConsoleHelper.CheckImagesSize(); break;
                    case "IMAGES_THUMBS_CREATE":
                        ConsoleHelper.ImageThumbsCreate(); break;

                    case "SEND_ERRORS":
                        EmailSender.SendErrors();  
                        EmailSender.AllegroSendErrors(); break;
                    case "PNG_TO_JPG":
                        allegroScan.ConvertImages(); break;
                    case "TEST1":
                        Bll.AllegroRESTHelper.Me(); break;
                        //Bll.ShopRestHelper.Products.GetProductFromShop(Dal.Helper.Shop.Lajtitpl, "116709", 141318); break;


                    case "TEST_DEL_SHOP_ATT_GR":
                        Bll.ClipperonRestHelper.Orders.GetOrderFromClipperon(Dal.Helper.Shop.Clipperon, "303-7500903-2227531");
                        //Bll.ShopRestHelper.AttributeGroups.UpdateAttribute2(Dal.Helper.Shop.Lajtitpl); 
                        break;
                    #endregion 

                    default:
                        LajtIt.Bll.ErrorHandler.LogError(new Exception("Parameter not found"), "Program.cs" );
                        break;
                }
            }
            catch (Exception ex)
            {
                
                LogError(ex, key);
                throw ex;
                //throw new Exception("Main", ex);
            } 
        }

        private static void LogError(Exception ex, string key)
        {
            LajtIt.Bll.ErrorHandler.SendError(ex, key);
            LajtIt.Bll.ErrorHandler.LogError(ex, "Program.cs - " + key);
            if (ex.InnerException != null)
                LogError(ex.InnerException, key);
        }

        private static void ProcessShipment(List<string> list)
        {
            foreach (string s in list)
                System.Console.Write(s);

            System.Console.ReadKey();
        }
    }
}
