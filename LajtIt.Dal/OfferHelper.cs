using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LajtIt.Dal
{
    public class OfferHelper
    {
        public int SetOfferNew(string offerName, string userName)
        {
            Dal.Offer offer = GetNewOffer();
            offer.Name = offerName;
            offer.InsertUser = userName;

            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.Offer.InsertOnSubmit(offer);
                ctx.SubmitChanges();

                return offer.OfferId;
            }
        }

        public OfferVersion GetOfferVersion(int offerVersionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<OfferVersion>(x => x.Offer);

                ctx.LoadOptions = dlo;
                return ctx.OfferVersion.Where(x => x.OfferVersionId == offerVersionId).FirstOrDefault();
            }
        }

        public List<Offer> GetOffers(string searchText, int[] statuses)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                DataLoadOptions dlo = new DataLoadOptions();
                dlo.LoadWith<Offer>(x => x.OfferStatus);

                ctx.LoadOptions = dlo;
                var r = ctx.Offer.AsQueryable();

                if (searchText != null)
                    r = r.Where(x =>
                    x.Phone.Contains(searchText)
                    || x.Name.Contains(searchText)
                    || x.Email.Contains(searchText));
                if (statuses.Length > 0)
                    r = r.Where(x => statuses.Contains(x.OfferStatusId));

                return r.OrderByDescending(x => x.InsertDate).ToList();
            }
        }

        public OfferVersion GetOfferVersionByNumber(string versionId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OfferVersion.Where(x => "1908"+x.OfferVersionId.ToString() == versionId).FirstOrDefault();

            }
        }

        public List<OfferStatus> GetOfferStatuses()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OfferStatus.ToList();
            }
        }

        public Offer GetOffer(int offerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Offer.Where(x => x.OfferId == offerId).FirstOrDefault();
            }
        }

        private Offer GetNewOffer()
        {
            return new Offer()
            {
                InsertDate = DateTime.Now,
                OfferStatusId = 1,
                ShowCode = false,
                ShowSupplier = false
            };
        }

        public void SetOffer(Offer offer, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.Offer offerToUpdate = ctx.Offer.Where(x => x.OfferId == offer.OfferId).FirstOrDefault();

                offerToUpdate.ContactName = offer.ContactName;
                offerToUpdate.Email = offer.Email;
                offerToUpdate.Name = offer.Name;
                offerToUpdate.OfferStatusId = offer.OfferStatusId;
                offerToUpdate.Phone = offer.Phone;
                offerToUpdate.ShowCode = offer.ShowCode;
                offerToUpdate.ShowSupplier = offer.ShowSupplier;

                ctx.SubmitChanges();

            }
        }

        public int SetOfferProduct(int offerId, int? offerVersionId, int productCatalogId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.ProductCatalog pc = ctx.ProductCatalog.Where(x => x.ProductCatalogId == productCatalogId).FirstOrDefault();

                OfferProduct op = new OfferProduct()
                {
                    Price = null,
                    ProductCatalogId = productCatalogId,
                    Quantity = 1,
                    Rebate = 0,
                    Name = null
                };
                OfferVersion ov = null;
                if (offerVersionId == null)
                {
                    ov = new OfferVersion()
                    {
                        InsertDate = DateTime.Now,
                        InsertUser = userName,
                        IsLocked = false,
                        OfferId = offerId,
                        LastUpdateDate = DateTime.Now,
                        UpdateUser = userName
                    };
                    op.OfferVersion = ov;
                }
                else
                    op.OfferVersionId = offerVersionId.Value;

                ctx.OfferProduct.InsertOnSubmit(op);
                ctx.SubmitChanges();
                return offerVersionId ?? ov.OfferVersionId;
            }
        }

        public List<Dal.OfferVersion> GetOfferVersions(int offerId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.OfferVersion.Where(x => x.OfferId == offerId).
                    OrderByDescending(x => x.OfferVersionId)
                    .ToList();
            }
        }

        public List<OfferProductsView> GetOfferProducts(int offerVersionId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.OfferProductsView.Where(x => x.OfferVersionId == offerVersionId).OrderBy(x=>x.Id).ToList();
            }
            }

        public void SetOfferProductDelete(int offerProductId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                Dal.OfferProduct op = ctx.OfferProduct.Where(x => x.Id == offerProductId).FirstOrDefault();
                ctx.OfferProduct.DeleteOnSubmit(op);
                ctx.SubmitChanges();
            }
        }

        public void SetOfferProducts(int offerVersionId, List<OfferProduct> offerProducts, bool lockOffer, string fileName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                List<OfferProduct> productsToUpdate = ctx.OfferProduct.Where(x => x.OfferVersionId == offerVersionId).ToList();

                foreach(Dal.OfferProduct productToUpdate in productsToUpdate)
                {
                    Dal.OfferProduct product = offerProducts.Where(x => x.Id == productToUpdate.Id).FirstOrDefault();

                    productToUpdate.Name= product.Name;
                    productToUpdate.Price= product.Price;
                    productToUpdate.Quantity= product.Quantity;
                    productToUpdate.Rebate = product.Rebate;
                    productToUpdate.Comment = product.Comment;
                }

                Dal.OfferVersion ov = ctx.OfferVersion.Where(x => x.OfferVersionId == offerVersionId).FirstOrDefault();
                ov.IsLocked = lockOffer;
                ov.FileName = fileName;

                ctx.SubmitChanges();
            }
        }

        public void SetOfferVersion(int offerId, int? offerVersionId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                OfferVersion ov = new OfferVersion()
                {
                    OfferId = offerId,
                    InsertDate = DateTime.Now,
                    InsertUser = userName,
                    IsLocked = false,
                    LastUpdateDate = DateTime.Now,
                    UpdateUser = userName
                };


                if (offerVersionId != null)
                {
                    List<OfferProduct> products = ctx.OfferProduct.Where(x => x.OfferVersionId == offerVersionId.Value).ToList()
                        .Select(x => new OfferProduct()
                        {
                            Name = x.Name,
                            OfferVersion = ov,
                            Price = x.Price,
                            ProductCatalogId = x.ProductCatalogId,
                            Quantity = x.Quantity,
                            Rebate = x.Rebate,
                            Comment = x.Comment

                        }).ToList();
                    ctx.OfferProduct.InsertAllOnSubmit(products);
                }
                else
                    ctx.OfferVersion.InsertOnSubmit(ov);

                ctx.SubmitChanges();


            }
        }

        public int SetOrderFromOffer(int offerId, int? offerVersionId, int shopId, string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {

                Dal.OrderHelper oh = new Dal.OrderHelper();



                OfferVersion ov = GetOfferVersion(offerVersionId.Value);

                Dal.Order newOrder = new Dal.Order()
                {
                    OrderStatusId = -1,
                    InsertDate = DateTime.Now,
                  
                    Email = ov.Offer.Email??Helper.MyEmail,
            
                    ShipmentFirstName = null,
                    ShipmentLastName = null,
                    ShipmentAddress = null,
                    ShipmentPostcode = null,
                    ShipmentCity = null,
                    ShipmentCompanyName = ov.Offer.ContactName,
                    Phone = ov.Offer.Phone??"",
                    Phone2 = null,
                    //ShippintTypeId = (int)Helper.ShippingType.SelfDelivery,
                    ExternalUserId = 0,
                    AmountPaid = 0,
                    ShopId = shopId==0? (int)Helper.Shop.Showroom: shopId,
                    ShippingCostVAT = 0.23M,
                    ParActive = true,
                    CompanyId = Dal.Helper.DefaultCompanyId,
                    ShippingAmountCurrency=0,
                    ShippingCurrencyRate=1, 
                    ShipmentCountryCode="PL",
                    ShippingCurrencyCode="PLN"
                };


                int orderId = oh.SetOrderNew(newOrder);


                List<OfferProductsView> products = GetOfferProducts(offerVersionId.Value);

                List<OrderProduct> orderProdcuts = new List<OrderProduct>();
                foreach(OfferProductsView op in products)
                {
                    orderProdcuts.Add(
                        new OrderProduct()
                        {
                            OrderId = orderId,
                            OrderProductStatusId = (int)Helper.OrderProductStatus.New,
                            Price = op.OfferPrice.Value,
                            Rebate = op.Rebate,
                            ProductCatalogId = op.ProductCatalogId,
                            ProductTypeId = (int)Helper.ProductType.RegularProduct, 
                            VAT = newOrder.ShippingCostVAT,
                            Quantity = op.Quantity,
                            ExternalProductId = 0,
                            LastUpdateDate = DateTime.Now,
                            LastUpdateReason = "Dodano produkt z oferty",
                            ProductName= op.ProductName,
                            Comment = op.Comment,
                            CurrencyRate=1,
                            PriceCurrency=op.OfferPrice.Value
                        }
                        );

                }

                oh.SetOrderProducts(orderProdcuts);

                OrderStatusHistory osh = new OrderStatusHistory()
                {
                    Comment = String.Format("Utworzono na podstawie <a href='/offer.aspx?id={0}&v={1}'>oferty {0}</a>", offerId, offerVersionId),
                    InsertDate = DateTime.Now,
                    InsertUser = userName,
                    OrderId = orderId,
                    OrderStatusId = (int)Helper.OrderProductStatus.New

                };

                oh.SetOrderStatus(osh, null);
                 osh = new OrderStatusHistory()
                {
                    Comment = String.Format("", offerId, offerVersionId),
                    InsertDate = DateTime.Now,
                    InsertUser = userName,
                    OrderId = orderId,
                    OrderStatusId = (int)Helper.OrderProductStatus.New

                };

                oh.SetOrderStatus(osh, null);

                return orderId;

            }
        }
    }
}
