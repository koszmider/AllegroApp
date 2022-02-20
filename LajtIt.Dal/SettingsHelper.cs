using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Dal
{
    public class SettingsHelper
    {
        public List<Settings> GetSettings()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Settings.ToList();
            }
        }

        public Settings GetSetting(int settingId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Settings.Where(x => x.Id == settingId).FirstOrDefault();
            }
        }

        public int SetSetting(Settings setting)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                if (setting.Id == 0)
                    ctx.Settings.InsertOnSubmit(setting);
                else
                {
                    Settings settingToUpdate = ctx.Settings.Where(x => x.Id == setting.Id).FirstOrDefault();
                    settingToUpdate.DecimalValue = setting.DecimalValue;
                    settingToUpdate.IntValue = setting.IntValue;
                    settingToUpdate.Name = setting.Name;
                    settingToUpdate.StringValue = setting.StringValue;


                }
                ctx.SubmitChanges();
                return setting.Id;
            }
        }

        public Settings GetSetting(string code)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.Settings.Where(x => x.Code == code).FirstOrDefault();
            }
        }
    }
}
