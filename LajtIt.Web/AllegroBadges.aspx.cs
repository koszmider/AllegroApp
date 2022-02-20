using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LajtIt.Bll;
using System.IO;
using System.Drawing;
using System.Net;

namespace LajtIt.Web
{
    [Developer("5867EA6C-DA7E-4999-A645-0426B52E57C5")]
    public partial class AllegroBadges : LajtitPage
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                BindBadges();
            }
        }

        private void BindBadges()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            gvBadges.DataSource = Bll.AllegroRESTHelper.Badges.GetBadges()
                .Where(x=>x.publication.to.HasValue && x.publication.to.Value >DateTime.Now.AddDays(-1))
                .OrderBy(x => x.publication.from)
                .ToList() ;
            gvBadges.DataBind();
        }
 
    }
}