using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Dal;

namespace LajtIt.Web.Controls
{
    public partial class ShopProducerControl : LajtitControl
    {
      
        private Dal.Helper.Shop Shop
        {
            get
            {
                return (Dal.Helper.Shop)Enum.ToObject(typeof(Dal.Helper.Shop), Int32.Parse(hfShopId.Value.ToString()));


            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindForm();
        }


        public void BindForm()
        {
          
        }

        protected void lbtnProducerCreate_Click(object sender, EventArgs e)
        {
            // Bll.ShopUpdateHelper.ClickShop cs = new Bll.ShopUpdateHelper.ClickShop();

            Bll.ShopRestHelper.Producers.GetProducers(Shop);

            //int id = cs.SetProducer(Shop, txbProducerNew.Text.Trim());
            string name = txbProducerNew.Text.Trim();
            int id = Bll.ShopRestHelper.Producers.SetProducer(Shop, name);

            if (id == 0)
                DisplayMessage("Błąd tworzenia producenta");
            else
            {

                Dal.ShopHelper sh = new Dal.ShopHelper();
                Dal.ShopProducer producer = new ShopProducer()
                {
                    Name = name,
                    ShopProducerId = id,
                    InsertDate = DateTime.Now,
                    IsActive = true,
                    ShopId = (int)Shop
                };

                sh.SetShopProducer(Shop, producer);
                DisplayMessage("Nowy producent został utworzony. Wyszukaj go i przypisz do dostawcy");

            }
        }

        internal int? GetProducerId()
        {
            if (!String.IsNullOrEmpty(hfProducerId.Value))
                return Int32.Parse(hfProducerId.Value);
            else
                return null;
        }

        internal void SetProducerId(int supplierId, Helper.Shop shop, int? producerId)
        {
            Dal.OrderHelper oh = new OrderHelper();
            
            Dal.Supplier supplier = oh.GetSupplier(supplierId);
            hfShopId.Value = ((int)shop).ToString();

            if (producerId.HasValue)
            {
                Dal.ShopHelper sh = new ShopHelper();
                Dal.ShopProducer sp = sh.GetShopProducers((int)shop).Where(x => x.ShopProducerId == producerId.Value).FirstOrDefault();
                hfProducerId.Value = producerId.ToString();
                txbProducer.Text = sp.Name;
            }
            else
                hfProducerId.Value = "0";


        }

        internal void SetShopId(int shopId)
        {
            hfShopId.Value = shopId.ToString() ;
        }
    }
}