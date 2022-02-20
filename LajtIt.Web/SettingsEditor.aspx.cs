using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LajtIt.Web
{
    [Developer("619e36b4-ead9-4151-b110-9a3de1600368")]
    public partial class SettingsEditor : LajtitPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindSettings();
        }

        private void BindSettings()
        {
            Dal.SettingsHelper sh = new Dal.SettingsHelper();
            lsbxSettings.DataSource = sh.GetSettings();
            lsbxSettings.DataBind();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {

            pnSettings.Visible = txbCode.Enabled = txbDecimal.Enabled = txbInt.Enabled = txbString.Enabled = true;
            txbCode.Text = txbDecimal.Text = txbInt.Text = txbName.Text = txbString.Text = "";
            ViewState["SettingId"] = null;

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            Dal.Settings setting = new Dal.Settings()
            {
                Code = txbCode.Text.Trim(),
                DecimalValue = GetDecimalValue(),
                IntValue = GetIntValue(),
                StringValue = GetStringValue(),
                Name = txbName.Text.Trim()
            };

            if (ViewState["SettingId"] != null)
                setting.Id = Convert.ToInt32(ViewState["SettingId"]);
            Dal.SettingsHelper sh = new Dal.SettingsHelper();

            int settingId = sh.SetSetting(setting);

            DisplayMessage("Zapisano");
            pnSettings.Visible = false;
            BindSettings();
            lsbxSettings.SelectedValue = settingId.ToString();
            LoadSettings();
        }

        private string GetStringValue()
        {
            if (String.IsNullOrEmpty(txbString.Text.Trim()))
                return null;
            else
                return txbString.Text.Trim();
        }

        private int? GetIntValue()
        {
            int i = 0;
            if (Int32.TryParse(txbInt.Text.Trim(), out i))
                return i;
            else
                return null;
        }

        private decimal? GetDecimalValue()
        {
            decimal i = 0;
            if (Decimal.TryParse(txbDecimal.Text.Trim(), out i))
                return i;
            else
                return null;
        }

        protected void lsbxSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void LoadSettings()
        {
            Clear();
            pnSettings.Visible = true;
            int settingId = Convert.ToInt32(lsbxSettings.SelectedValue);
            Dal.SettingsHelper sh = new Dal.SettingsHelper();

            Dal.Settings setting = sh.GetSetting(settingId);
            ViewState["SettingId"] = settingId;

            txbCode.Text = setting.Code.ToUpper();
            txbCode.Enabled = false;
            if (setting.DecimalValue.HasValue)
            {
                txbDecimal.Enabled = true;
                txbDecimal.Text = setting.DecimalValue.ToString();
            }
            else
                txbDecimal.Enabled = false;
            if (setting.IntValue.HasValue)
            {
                txbInt.Enabled = true;
                txbInt.Text = setting.IntValue.ToString();
            }
            else
                txbInt.Enabled = false;
            if (setting.StringValue != null)
            {
                txbString.Enabled = true;
                txbString.Text = setting.StringValue;
            }
            else
                txbString.Enabled = false;
            txbName.Text = setting.Name;
        }
    }
}