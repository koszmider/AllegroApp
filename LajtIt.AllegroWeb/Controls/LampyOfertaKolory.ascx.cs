using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.AllegroWeb.Controls
{
    public partial class LampyOfertaKolory : System.Web.UI.UserControl
    {
        public string Model;
        protected void Page_Load(object sender, EventArgs e)
        {


            switch (Request.QueryString["kolor"])
            {
                case "zielony": pGreen.Visible = true;
                    SetGreenImages(); ;
                    break;
                case "czarny": pBlack.Visible = true;
                    SetBlackImages();
                    break;
            }

            if (!pGreen.Visible)
                return;
        }
        private void SetBlackImages()
        {
            IPageColor page = this.Page as IPageColor;

            switch (Model)
            {
                case "surya":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_surya_bialo-czarna.jpg"
                    , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_surya_bialo-czarna2.jpg");
                    break;
                case "spiral":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_spiral_bialo-czarna.jpg"
                    , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_spiral_czarna.jpg");
                    break;
                case "kronos":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_kronos_bialo-czarna.jpg"
                    , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_kronos_czarna.jpg");
                    break;
                case "cosmo":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modna_lampa_cosmo_czarna.jpg");
                    break;
                case "tromle_l":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_l_bialo-czarna.jpg"
                        , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_l_czarna.jpg"
                        );
                    break;
                case "tromle_xl":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_xl_bialo-czarna.jpg"
                        , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_xl_czarna.jpg"
                        );
                    break;
                case "estrella":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modna_lampa_Estrella_II_bialo_czarna.jpg"
                        , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modna_lampa_Estrella_II_czarna.jpg"
                        );
                    break;

            }
        }
        private void SetGreenImages()
        {
            IPageColor page = this.Page as IPageColor;

            switch (Model)
            {
                case "kronos":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_kronos_bialo-zielona.jpg"
                    , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_lampa_wiszaca_kronos_zielona.jpg");
                    break;
                case "cosmo":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modna_lampa_cosmo_zielona.jpg");
                    break;
                case "tromle_l":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_m_bialo-zielona.jpg"
                        , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_l_zielona.jpg"
                        );
                    break;
                case "tromle_xl":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_xl_bialo-zielona.jpg"
                        , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modne_lampy_tromle_xl_zielona.jpg"
                        );
                    break;
                case "estrella":
                    page.SetPageColor(@"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modna_lampa_Estrella_II_bialo_zielona.jpg"
                        , @"http://www.lajtit.pl/public/assets/allegro/kolory/www.lajtit.pl_modna_lampa_Estrella_II_zielona.jpg"
                        );
                    break;

            }
        }
    }
}