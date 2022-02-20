using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.SqlClient;

namespace LajtIt.Dal
{
    public class TimeTrackerHelper
    {

        public List<Dal.TimeTrackerLocation> GetLocations()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.TimeTrackerLocation.ToList();
            }
        }
        public List<Dal.TimeTrackerView> GetTimeTracker(DateTime date, string userName)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.TimeTrackerView
                    .Where(x => x.InsertUser == userName && date.Year == x.InsertDate.Year && date.Month == x.InsertDate.Month && date.Day == x.InsertDate.Day)
                    .OrderBy(x => x.InsertDate)
                    .ToList();
            }
        }

        public void SetTimeTrackerUpdate(TimeTracker tt)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                TimeTracker ttUpdate = ctx.TimeTracker
                    .Where(x => x.Id == tt.Id)
                    .FirstOrDefault();

                ttUpdate.Comment = tt.Comment;
                ttUpdate.WorkingHours = tt.WorkingHours;
                ttUpdate.LocationTypeId = tt.LocationTypeId;

                ctx.SubmitChanges();
            }
        }

        public List<Dal.TimeTrackerMonthView> GetTimeTrackerReport(DateTime value, string userName)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.TimeTrackerMonthView
                    .Where(x => x.InsertUser == userName)// && x.Year == value.Year && x.Month == value.Month)
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .ToList();
            }
        }

        public void SetTimeTracker(TimeTracker tt)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                ctx.TimeTracker.InsertOnSubmit(tt);
                ctx.SubmitChanges();
            }
        }

        public bool CheckMonthHours(string userName, decimal workingHours, int? id)
        {
            using (LajtitViewsDB ctxv = new LajtitViewsDB())
            { 
            using (LajtitDB ctx = new LajtitDB())
            {
                decimal h = 0;
                if (id.HasValue)
                    h = ctx.TimeTracker.Where(x => x.InsertUser == userName && x.Id == id.Value).Select(x => x.WorkingHours).FirstOrDefault();

                TimeTrackerMonthView month = ctxv.TimeTrackerMonthView
                    .Where(x => x.InsertUser == userName && x.Year == DateTime.Now.Year && x.Month == DateTime.Now.Month)
                    .FirstOrDefault();
                AdminUser au = ctx.AdminUser.Where(x => x.UserName == userName).FirstOrDefault();
                if (au == null || !au.MaxHoursPerMonth.HasValue)
                    return true;

                if (month != null)
                    return month.WorkingHours.Value - h + workingHours <= au.MaxHoursPerMonth.Value;
                else
                    return workingHours <= au.MaxHoursPerMonth.Value;

            }
        }
        }

        public bool CheckDayHours(string userName, decimal workingHours, int? id)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                decimal h = 0;
                if (id.HasValue)
                    h = ctx.TimeTracker.Where(x => x.InsertUser == userName && x.Id == id.Value).Select(x => x.WorkingHours).FirstOrDefault();

                List<TimeTracker> time = ctx.TimeTracker
                    .Where(x => x.InsertUser == userName && x.InsertDate.Date == DateTime.Now.Date)
                    .ToList();
                decimal dayHours = 0;
                if (time.Count() > 0)
                    dayHours = time.Sum(x => x.WorkingHours);

                AdminUser au = ctx.AdminUser.Where(x => x.UserName == userName).FirstOrDefault();
                if (au == null || !au.MaxHoursPerDay.HasValue)
                    return true;

                return dayHours - h + workingHours <= au.MaxHoursPerDay.Value;

            }
        }

        public List<Dal.UserLog> GetTimeTrackerStats(DateTime date, string userName)
        {

            using (LajtitHelperDB ctx = new LajtitHelperDB())
            {
                return  ctx.UserLogs.Where(x => x.InsertDate.Date == date.Date && x.InsertUser == userName).ToList();
            }
        }
    }
}