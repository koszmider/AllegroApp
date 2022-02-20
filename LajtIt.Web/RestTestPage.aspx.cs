using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("2ff78df5-050d-4184-aa55-a27253e35c04")]
    public partial class RestTestPage : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var r = Bll.AllegroRESTHelper.UpdateCommandToSchedules("00010000111000");
        }
    }
}