using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NPOI.SS.Formula.Functions;

namespace LajtIt.Bll
{
    public partial class AllegroScan
    {
        public void AllegroUpdateProducts()
        {
            // Set a variable to the My Documents path.
            string mydocpath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            FileInfo file = new FileInfo(Path.Combine(mydocpath, "ProductIds.txt"));
            if (file.Exists)
            {
                using (StreamReader inputFile = new StreamReader(Path.Combine(mydocpath, "ProductIds.txt")))
                {
                    string text = inputFile.ReadToEnd();
                    List<string> strProductIdList = text.Split('.').ToList();
                    foreach (string sid in strProductIdList)
                    {
                        if (sid.Length > 0)
                        {
                            int ProductCatalogId = (int)Decimal.Parse(sid, System.Globalization.CultureInfo.InvariantCulture);
                            Bll.ProductFileImportHelper.UpdateAllegro(ProductCatalogId);
                        }
                    }
                }

                file.Delete();
            }

        }
        //        public void CreateAuctions()
        //        {
        //            Bll.AllegroHelper.GetVersionKey(Dal.Helper.MyUsers.JacekStawicki.ToString());
        //            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
        //                        Bll.AllegroGoalHelper agh = new Bll.AllegroGoalHelper();
        //                        AllegroHelper ah = new AllegroHelper(); 
        //            List<Dal.AllegroGoalSchedule> schedules = allegroScan.GetSchedules();


        //            foreach (Dal.AllegroGoalSchedule schedule in schedules)
        //            {
        //                int nextScheduleId = allegroScan.GetSchedulesNextId();
        //                long itemId = schedule.AllegroGoal.ItemId;
        //                long newItemId;
        //                int status = ah.SetShopItem(Dal.Helper.MyUsers.JacekStawicki.ToString(), itemId, nextScheduleId, out newItemId);

        //                switch (status)
        //                {
        //                    case -1:  NotifyAboutScheduleError(schedule); break;
        //                    default:

        //                            Dal.AllegroGoalItem allegroGoalItem = new Dal.AllegroGoalItem()
        //                            {
        //                                GoalId = schedule.GoalId,
        //                                InsertDate = DateTime.Now,
        //                                ItemId = newItemId,
        //                                GoalItemId = nextScheduleId
        //                            };

        //                            allegroScan.SetScheduleGoalItem(allegroGoalItem);

        //                        break;
        //                }
        //            }
        //        }

        //        private void NotifyAboutScheduleError(Dal.AllegroGoalSchedule schedule)
        //        {
        //                EmailSender es = new EmailSender();

        //                es.SendEmail(new Dto.Email()
        //                {
        //                    Body = String.Format(@"Proces automatycznego wystawiania aukcji naptokał problem<br>
        //                    Nazwa celu: {0}<br>
        //                    Czas: {1}:{2}<br>
        //                    Numer przedmiotu: {3}", schedule.AllegroGoal.Name,
        //                        schedule.Hour.Hours, schedule.Hour.Minutes, schedule.AllegroGoal.ItemId),
        //                    FromEmail = Dal.Helper.DevEmail,
        //                    Subject = "Problem z wystawieniem przedmiotu",
        //                    ToEmail = Dal.Helper.DevEmail,
        //                    ToName = "Administrator"

        //                });
        //        }


        //public void ChangeOffer(long itemId)
        //{
        //    AllegroHelper ah = new AllegroHelper();
        //    ah.SetChangeOffer( itemId);
        //}



        //public void VerifyAllegroItemBatch()
        //{

        //    Dal.OrderHelper oh = new Dal.OrderHelper();
        //    int[] availableStatuses = new int[] { (int)Dal.Helper.ProductAllegroItemBatchStatus.Verifying, (int)Dal.Helper.ProductAllegroItemBatchStatus.VerifyingAndCreating };
        //    Dal.ProductCatalogAllegroItemBatch batch = oh.GetProductCatalogAllegroItemBatchToProcess(availableStatuses);

        //    if (batch == null)
        //        return;
        //    Bll.AllegroHelper ah = new Bll.AllegroHelper();

        //    ah.VerifyOffers(batch);

        //}

        //public void SetAllegroParcelNumber(string v)
        //{
        //    Bll.AllegroHelper ah = new Bll.AllegroHelper();
        //    ah.SetAllegroParcelNumber();
        //}

        public void SetAllegroUpdateItems(string v)
        {
            Bll.AllegroRESTHelper.GetSellerOffers(500, 0, null);
        }
        public void SetAllegroUpdateOfferProduct(string v)
        {
            Bll.AllegroRESTHelper.GetSellerOffers(500, 0, true);
        }    
        public void GetAllegroProductsFromOffers(string v)
        {
            Bll.AllegroRESTHelper.GetAllegroProductsFromOffers();
        }

        //public void CreateAllegroItems()
        //{

        //    Dal.OrderHelper oh = new Dal.OrderHelper();

        //    int[] availableStatuses = new int[] { (int)Dal.Helper.ProductAllegroItemBatchStatus.Creating };

        //    Dal.ProductCatalogAllegroItemBatch batch = oh.GetProductCatalogAllegroItemBatchToProcess(availableStatuses);

        //    if (batch == null)
        //        return;
        //    Bll.AllegroHelper ah = new Bll.AllegroHelper();

        //    ah.CreateOffers(batch.BatchId);
        //}

        //public void UpdateAllegroItems()
        //{
        //    Bll.AllegroHelper ah = new Bll.AllegroHelper();

        //    try
        //    {
        //        ah.UpdateAllegroItems();
        //    }
        //    catch (Exception ex)
        //    {
        //        Bll.ErrorHandler.SendError(ex, "UpdateAllegroItems");

        //    }
        //}
        public void SetEnv()
        {

            Dal.Helper.SetEnv();

        }




        //public void SetAllegroPromotions(string v)
        //{
        //    Bll.AllegroRESTHelper.SetPromotions();
        //}

        public void SetAllegroPromotionsDelete(string v)
        {
            //Bll.AllegroRESTHelper.SetPromotionsDelete();
        }
        //public void SetAllegroPromotionsDeleteAll(string v)
        //{
        //    Bll.AllegroRESTHelper.SetPromotionsDeleteAll();
        //}
        public void SetAllegroTokenRefresh(string v)
        {
            Bll.AllegroRESTHelper.GetRefreshToken();
        }

        public void GetCeneoOrders(string v)
        {
            Bll.CeneoApiHelper.GetOrders();
        }

        public void GetCeneoBids(string v)
        {
            Bll.CeneoApiHelper cah = new CeneoApiHelper();
            cah.GetCeneoProducts(Dal.Helper.Shop.Ceneo, Dal.Helper.Shop.Lajtitpl);
        }
        public void SetCeneoBids(string v)
        {
            Bll.CeneoApiHelper cah = new CeneoApiHelper();
            cah.SetCeneoBids(Dal.Helper.Shop.Ceneo, Dal.Helper.Shop.Lajtitpl);
        }
        public void GetCeneoCategories(string v)
        {
             

            Bll.CeneoApiHelper.Categories cah = new CeneoApiHelper.Categories();
            cah.LoadData<Bll.CeneoApiHelper.Categories.ArrayOfCategory>();
        }
        public void GetShopProducers(string v)
        {
            Bll.ShopUpdateHelper.ClickShop cah = new Bll.ShopUpdateHelper.ClickShop();
            cah.GetProducers(Dal.Helper.Shop.Lajtitpl);
        }

        //public void AltavolaImport()
        //{
        //    LajtIt.Bll.AltavolaHelper ah = new AltavolaHelper();
        //    ah.LoadData();
        //}


        public void AltavolaImportCeneo()
        {
            LajtIt.Bll.AltavolaHelper ah = new AltavolaHelper();
            ah.SupplierId = 15;
            ah.LoadData<CeneoHelper.Offers>();


        }
        public void RedluxImport()
        {
            LajtIt.Bll.RedluxHelper ah = new RedluxHelper();
            ah.SupplierId = 25;
            ah.LoadData<RedluxHelper.SHOP>();


        }

        public void StepIntoDesignImport()
        {
            LajtIt.Bll.StepIntoDesignHelper ah = new StepIntoDesignHelper();
            ah.SupplierId = 47;
            ah.LoadData<StepIntoDesignHelper.Offer>();


        }
        public void LampexImport()
        {
            LajtIt.Bll.LampexHelper ah = new LampexHelper();
            ah.SupplierId = 33;
            ah.LoadData<Bll.LampexHelper.Offers>();
        }

        public void CleoniImport()
        {
            LajtIt.Bll.CleoniHelper ah = new CleoniHelper();
         
            ah.LoadData();
        }
        public void SolluxImport()
        {
            LajtIt.Bll.SolluxHelper ah = new SolluxHelper();
            ah.SupplierId = 31;
            ah.LoadData<Bll.SolluxHelper.Document>();
        }

        public void SetShopConfiguration()
        {
            Bll.Integrations.ShopHelper.SetShopConfiguration(1076);
        }

        public void CeneoFileCopy()
        {
            Bll.CeneoHelper ch = new Bll.CeneoHelper();
            ch.GetCeneoFile();
        }

        //public void AltavolaImportStatus()
        //{
        //    LajtIt.Bll.AltavolaHelper ah = new AltavolaHelper();
        //    ah.UpdateStatus();
        //}

        public void NordluxImport()
        {
            LajtIt.Bll.NordluxHelper ah = new NordluxHelper();
            ah.SupplierId = 61;
            ah.ReadMailbox();
        }

        public void MaxligthImport()
        {
            LajtIt.Bll.MaxlightHelper ah = new MaxlightHelper();
            ah.SupplierId = 2;
            ah.LoadData<Bll.CeneoHelper.Offers>();
        }

        public void LightPrestigeImport()
        {
            LajtIt.Bll.LightPrestigeHelper ah = new LightPrestigeHelper();
            ah.SupplierId = 55;
            ah.UpdatePurchasePrice = true;
            ah.LoadData<Bll.LightPrestigeHelper.Document>();
        }
        public void AuhilonImport()
        {
            LajtIt.Bll.AuhilonHelper ah = new AuhilonHelper();
            ah.SupplierId = 37;
            ah.GetFile();
        }
        public void ArgonImport()
        {
            LajtIt.Bll.ArgonHelper ah = new ArgonHelper();
            ah.SupplierId = 7;
            ah.GetFile();
        }
        public void CosmoLightImport()
        {
            LajtIt.Bll.CosmoLightHelper clh = new CosmoLightHelper();
            clh.SupplierId = 92;
            clh.GetFile();
        }
        public void HubschImport()
        {
            LajtIt.Bll.HubschHelper ah = new HubschHelper();
            ah.SupplierId = 78;
            ah.GetFile();
        }
        public void SetInpostTracking()
        {
            Bll.InpostHelper ih = new InpostHelper();
            ih.GetTracking();
        }

        public void FollowObjects()
        {
            Bll.ProductCatalogHelper pch = new ProductCatalogHelper();
            pch.FollowObjects();
        }

        public void ZumalineImport()
        {
            LajtIt.Bll.ZumalineHelper ah = new ZumalineHelper();
            ah.SupplierId = 9;
            //ah.LoadData<Bll.ZumalineHelper.Products>();
            ah.GetFile();

            //ah.ReadMailbox();
        }
        public void ZumalinePNLDImport()
        {
            //LajtIt.Bll.ZumalineHelper ah = new ZumalineHelper();
            //ah.SupplierId = 9;
          

            //ah.ReadMailbox();
        }
        public void PaulmannImport()
        {
            LajtIt.Bll.PaulmannHelper ah = new PaulmannHelper();
            ah.SupplierId = 84;
          

            ah.ReadMailbox();
        }
        public void CandelluxImport()
        {
            LajtIt.Bll.CandelluxHelper ah = new CandelluxHelper();
            ah.SupplierId = 50;
            ah.GetFile();
        }
        public void ElsteadImport()
        {
            LajtIt.Bll.ElsteadHelper ah = new ElsteadHelper();
            ah.SupplierId = 16;
            ah.GetFile();
        }
        public void ApetiImport()
        {
            LajtIt.Bll.CandelluxHelper ah = new CandelluxHelper();
            ah.SupplierId = 54;
            //ah.LoadData<Bll.ZumalineHelper.Products>();
            ah.GetFile();
        }
        public void RLImport()
        {
            LajtIt.Bll.TrioRLHelper ah = new TrioRLHelper();
            ah.SupplierId = 74;
            //ah.LoadData<Bll.ZumalineHelper.Products>();
            ah.GetFile();
        }
        public void TrioImport()
        {
            LajtIt.Bll.TrioRLHelper ah = new TrioRLHelper();
            ah.SupplierId = 70;
            //ah.LoadData<Bll.ZumalineHelper.Products>();
            ah.GetFile();
        }

        public void LucideImport()
        {
            LajtIt.Bll.LucideHelper ah = new LucideHelper();
            ah.SupplierId = 72;
            ah.GetFile();
        }

        public void SuMaImport()
        {
            LajtIt.Bll.SuMaHelper ah = new SuMaHelper();
            ah.SupplierId = 90;
            ah.GetFile();
        }
        public void LutecImport()
        {
            LajtIt.Bll.LutecHelper ah = new LutecHelper();
            ah.SupplierId = 73;
            ah.GetFile();
        }
        public void GlasbergImport()
        {
            LajtIt.Bll.GlasbergHelper ah = new GlasbergHelper();
            ah.SupplierId = 86;
            ah.GetFile();
        }
        public void NovaLuceImport()
        {
            LajtIt.Bll.NovaLuceHelper ah = new NovaLuceHelper();
            ah.SupplierId = 80;
            ah.GetFile();
        }
        public void BrilliantImport()
        {
            LajtIt.Bll.BrilliantHelper ah = new BrilliantHelper();
            ah.SupplierId = 71;
            ah.GetFile();
        }

        public void AZzardoImport()
        {
            LajtIt.Bll.AzzardoHelper ah = new AzzardoHelper();
            ah.SupplierId = 8;
            ah.LoadData<Bll.AzzardoHelper.Towary>();
        }
        public void MarkslojdImport()
        {
            LajtIt.Bll.MarkslojdHelper ah = new MarkslojdHelper();
            ah.LoadData();
        }      
        public void EgloImport()
        {
            LajtIt.Bll.EgloHelper ah = new EgloHelper();
            ah.LoadData();
        }
        public void RabaluxImport()
        {
            LajtIt.Bll.RabaluxHelper ah = new RabaluxHelper();
            ah.LoadData();
        }
        public void PolnedImport()
        {
            LajtIt.Bll.PolnedHelper ah = new PolnedHelper();
            ah.LoadData();
        }
        public void SompexImport()
        {
            LajtIt.Bll.SompexHelper ah = new SompexHelper();
            ah.LoadData();
        }
        public void PoluxImport()
        {
            LajtIt.Bll.PoluxHelper ah = new PoluxHelper();
            ah.SupplierId = 52;
            ah.LoadData<Bll.PoluxHelper.Urlset>();
        }
        public void NowodvorskiImport()
        {
            LajtIt.Bll.NowodvorskiHelper ah = new NowodvorskiHelper();
            ah.LoadData();
        }

        public void MaytoniImport()
        {
            Bll.MaytoniHelper mh = new MaytoniHelper();
            mh.GetFile();
        }
        public void ItaluxImport()
        {
            Bll.ItaluxHelper mh = new ItaluxHelper();
            mh.LoadData();
        }
        public void TKImport()
        {
            Bll.TKHelper mh = new TKHelper();
            mh.LoadData();
        }
        public void MilagroImport()
        {
            Bll.MilagroHelper mh = new Bll.MilagroHelper();
            mh.GetFile();


        }

        //public void CeneoFile()
        //{
        //    Bll.CeneoHelper ch = new CeneoHelper();
        //    ch.GenerateXMLFile();
        //}


        //public void HomebookFile()
        //{
        //    Bll.EmagHelper ch = new EmagHelper();
        //    ch.GenerateXMLFileHomebook("homebook", Dal.Helper.Shop.Homebook);
        //}

        public void SetProductNames(string userName)
        {

            Mixer.SetProductNames(true);
        }
        public void SetProductDescriptions(string userName)
        {

            Mixer.SetProductDescriptions(true);
        }
        public void Tls()
        {
            var test_servers = new Dictionary<string, string>();
            test_servers["SSL 2"] = "https://www.ssllabs.com:10200";
            test_servers["SSL 3"] = "https://www.ssllabs.com:10300";
            test_servers["TLS 1.0"] = "https://www.ssllabs.com:10301";
            test_servers["TLS 1.1"] = "https://www.ssllabs.com:10302";
            test_servers["TLS 1.2"] = "https://www.ssllabs.com:10303";

            var supported = new Func<string, bool>(url =>
            {
                try { return new System.Net.Http.HttpClient().GetAsync(url).Result.IsSuccessStatusCode; }
                catch { return false; }
            });
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var supported_protocols = test_servers.Where(server => supported(server.Value));


            // Set a variable to the My Documents path.
            string mydocpath =
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(mydocpath, "WriteLines.txt")))
            {
                outputFile.WriteLine(string.Join(", ", supported_protocols.Select(x => x.Key)));
            }



        }

        public void ConvertImages()
        {
            string dir = String.Format(ConfigurationManager.AppSettings[String.Format("ProductCatalogImagesDirectory_{0}", Dal.Helper.Env.ToString())], "");
            Bll.Helper.ToJPG(dir);
        }
 

        public void CheckMisssingImages()
        {
            Bll.ProductCatalogHelper pch = new ProductCatalogHelper();
            pch.CheckMisssingImages();
        }

        public void CheckMisssingImagesInDb()
        {
            Bll.ProductCatalogHelper pch = new ProductCatalogHelper();
            pch.CheckFilesNotIdDb();
        }
        //public void ShopRefreshImages()
        //{
        //    Bll.ShopUpdateHelper.ClickShop cs = new ShopUpdateHelper.ClickShop();
        //        cs.ShopRefreshImages(Dal.Helper.Shop.Lajtitpl);
        //}

        public void ShopUpdateOrderStatus()
        {
            Bll.ShopHelper sh = new ShopHelper();
            sh.SetOrderStatuses(Dal.Helper.Shop.Lajtitpl);
        }

        public void TEST()
        {
            Dal.ProductCatalogShopAttribute shopAttribute = Dal.DbHelper.ProductCatalog.GetProductCatalogShopAttributeByAttributeGroupId(Dal.Helper.Shop.Lajtitpl, 7);
            Bll.ShopRestHelper.AttributeGroups.UpdateAttribute(Dal.Helper.Shop.Lajtitpl, shopAttribute);
        }

        public void Ftp()
        {
            Bll.FtpHelper ftp = new FtpHelper();
            ftp.UploadProductCatalogImages();
        }

        public void FtpSupplierId(int id)
        {
            Bll.FtpHelper ftp = new FtpHelper();
            ftp.UploadProductCatalogImagesBySupplierId(id);
        }

        public void FtpList()
        {
            Bll.FtpHelper ftp = new FtpHelper();
            ftp.ClearFtpFiles();
        }

        public void LajtitUpdateStock()
        {
            Dal.ProductCatalogHelper pch = new Dal.ProductCatalogHelper();
            pch.SetProductCatalogUpdateStock();
        }

        public void KingHomeImport()
        {
            Bll.KingHomeHelper kh = new KingHomeHelper();
            kh.SupplierId = 66;
            kh.LoadData<Bll.KingHomeHelper.Produkty>();
        }

   

        //public void SetShopUpdateByCatalogBatch(string actingUser)
        //{
        //    Bll.ShopHelper sh = new ShopHelper();

        //    sh.UpdateByCatalogBatch(Dal.Helper.ShopType.ClickShop, Dal.Helper.UpdateScheduleType.OnlineShopBatch, actingUser);
        //}
        //public void SetShopUpdateByCatalogSingle(string actingUser)
        //{
        //    Bll.ShopHelper sh = new ShopHelper();
        //    sh.SetShopUpdateByCatalogSingle(actingUser);

        //}


        //public void SetAllegroUpdateByCatalog(string actingUser)
        //{
        //    Bll.ShopHelper sh = new ShopHelper(false);
        //    sh.SetAllegroUpdateByCatalog(actingUser);


        //}

        public void SetImportProductImages(string actingUser, int supplierId, string dir)
        {
            Bll.ProductFileImportHelper ph = new ProductFileImportHelper();
            ph.SetImportProductImages(supplierId, dir);
        }
        public void SetImportProductImagesAdHoc(string actingUser, int supplierId, string dir)
        {
            Bll.ProductFileImportHelper ph = new ProductFileImportHelper();
            ph.SetImportProductImagesAdHoc(supplierId, dir);
        }
        public void SetImportProductImagesByCatalog(string actingUser, int supplierId, string dir)
        {
            Bll.ProductFileImportHelper ph = new ProductFileImportHelper();
            ph.ImagesByCodeByCatalog(supplierId, dir);
        }
        public void SetImportProductImagesByCatalogAdd(string actingUser, int supplierId, string dir)
        {
            Bll.ProductFileImportHelper ph = new ProductFileImportHelper();
            ph.ImagesByCodeByCatalogAdd(supplierId, dir);
        }

        public void SetImportProducts(string actingUser)
        {
            Bll.ProductFileImportHelper ph = new ProductFileImportHelper();
            ph.SetImportProducts();
        }
        public void SetImportProductsCheck(string actingUser)
        {
            Bll.ProductFileImportHelper ph = new ProductFileImportHelper();
            ph.SetImportProductsCheck();
        }

      

        //public void GetFormFields()
        //{
        //    Bll.AllegroHelper ah = new Bll.AllegroHelper();

        //    ah.GetFormFields();
        //}

        public void SetBid(string actingUser)
        {
            Bll.BidHelper bid = new Bll.BidHelper();

            bid.Run(actingUser);

        }

        public void GetShopOrders(string actingUser)
        {
           // Bll.ShopHelper sh = new ShopHelper();
            //sh.GetOrders(Dal.Helper.Shop.Lajtitpl, actingUser);

            ShopRestHelper.Orders.GetOrders(Dal.Helper.Shop.Lajtitpl);
            ShopRestHelper.Orders.GetOrderPayments(Dal.Helper.Shop.Lajtitpl);

            //sh.GetOrderPayments(Dal.Helper.Shop.Lajtitpl, actingUser);
        }

        //public void SetShopAttributes()
        //{
        //    Bll.ShopHelper sh = new ShopHelper();

        //    //sh.SetAttributeDefinitions();
        //    sh.SetAttributesToShopProduct(); 
        //}
        //public void SetShopAttributesToCategory()
        //{
        //    Bll.ShopHelper sh = new ShopHelper();
             
        //    sh.SetProductCategoriesFromAttributes(Dal.Helper.Shop.Lajtitpl);
        //}
        //public void SetShopProductsToCatalog()
        //{
        //    Bll.ShopHelper sh = new ShopHelper();

        //    sh.GetCategories();
        //    //sh.AssignShopProductToProductCatalog(); 
        //}
        public void SetPaczkomaty(string actingUser)
        {
            Bll.PaczkomatyHelper p = new PaczkomatyHelper();
            p.SetPaczkomaty(actingUser);

        }
 

        public  void SetAllegroAction(string actiongUser)
        {
            Bll.AllegroHelper ah = new Bll.AllegroHelper();
              

            ah.SetAllegroAction();  
        }


        public void SetRest(string p)
        {
            int[] pIds = new int[] { 1563,1346, 116062 };
            //List<Dal.ProductCatalogShopUpdateSchedule> schedules = new List<Dal.ProductCatalogShopUpdateSchedule>();

            //foreach (int pId in pIds)
            //{

            //    schedules.Add(new Dal.ProductCatalogShopUpdateSchedule()
            //    {
            //        ProductCatalogId = pId,
            //        ShopColumnTypeId = (int)Dal.Helper.ShopColumnType.Category
            //    });
            //    schedules.Add(new Dal.ProductCatalogShopUpdateSchedule()
            //    {
            //        ProductCatalogId = pId,
            //        ShopColumnTypeId = (int)Dal.Helper.ShopColumnType.Price
            //    });
            //    schedules.Add(new Dal.ProductCatalogShopUpdateSchedule()
            //    {
            //        ProductCatalogId = pId,
            //        ShopColumnTypeId = (int)Dal.Helper.ShopColumnType.Ean
            //    });
            //}

            //Bll.ShopRestHelper.Products.SetProductsUpdate(Dal.Helper.Shop.Lajtitpl, schedules, pIds);
            Bll.ShopRestHelper.Orders.GetOrders(Dal.Helper.Shop.Lajtitpl);

        }

        public void GetShopCategories()
        { 
            List<Dal.Shop> shops = Dal.DbHelper.Shop.GetShops(Dal.Helper.ShopEngineType.Shoper).Where(x => x.IsActive).ToList();
                ;

            foreach (Dal.Shop shop in shops)
            {
                Dal.Helper.Shop s = (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), shop.ShopId);
                Bll.ShopRestHelper.Categories.GetCategories(s);
            }

        }
        public void SetShopProductsUpdate(string v, Dal.Helper.Shop shop)
        {
            Bll.ShopHelper sh = new ShopHelper();

            sh.SetShopProductsUpdate(shop);
        }

        //public void SetShopProductCreate()
        //{
        //    Bll.ShopHelper sh = new ShopHelper();

        //    sh.CreateUpdateProducts();
        //}
 
        public void SetShopoptions()
        {
            Bll.ShopHelper sh = new ShopHelper();

            sh.SetShopoptions();
        }
        public void SetShopMainPage()
        {
            Bll.ShopHelper sh = new ShopHelper();

            sh.SetShopMainPage(Dal.Helper.Shop.Lajtitpl);
        }

        public void SetAllegroTags(string v)
        {
            AllegroRESTHelper.Tags.GetTags();
        }
    }
}

