using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll
{
    /*
    public class AllegroGoalHelper
    {
        public bool VerifyItemId(string userName, long itemId)
        {
            bool result = true;
            Dal.AllegroScan allegroScan = new Dal.AllegroScan();
            Dal.AllegroItem ai = allegroScan.GetAllegroItem(itemId);

            if (ai == null)
            {
                LajtIt.Bll.AllegroScan s = new AllegroScan();
                AllegroHelper.GetVersionKey(userName);
                s.SetAuction(userName, itemId);
                ai = allegroScan.GetAllegroItem(itemId);
                if (ai == null)
                    return false;
            } 

            return result;
        }
        public bool GoalAdd(Dal.AllegroGoal goal)
        {
            bool result = true;

            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            agh.GoalAdd(goal);

            return result;
        }

        public bool GoalEdit(Dal.AllegroGoal goal)
        {
            bool result = true;

            Dal.AllegroGoalHelper agh = new Dal.AllegroGoalHelper();
            agh.GoalEdit(goal);

            return result;
        }



        public List<Dal.AllegroGoalSchedule> GetGoalSchedules(int goalId)
        {
            throw new NotImplementedException();
        }
    }
    */
}
