using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Data.Linq.SqlClient;

namespace LajtIt.Dal
{
    public class TaskTrackerHelper
    {

        public List<Dal.TaskTrackerStatus> GetStatuses()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.TaskTrackerStatus.ToList();
            }
        }


        public List<Dal.TaskTrackerStatusHistoryView> GetTaskStatuseHistory(int taskId)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                return ctx.TaskTrackerStatusHistoryView.Where(x => x.TaskId == taskId).OrderByDescending(x => x.InsertDate).ToList();
            }
        }

        public Dal.TaskTracker GetTask(int taskId)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.TaskTracker.Where(x => x.TaskId == taskId).FirstOrDefault();
            }
        }
        public List<Dal.TaskTrackerView> GetTasks(string userName)
        {
            using (LajtitViewsDB ctx = new LajtitViewsDB())
            {
                if (userName == null)
                    return ctx.TaskTrackerView.OrderByDescending(x => x.Priority).ToList();
                else
                    return ctx.TaskTrackerView.Where(x => x.UserAssigned.Contains(userName)).OrderByDescending(x => x.Priority).ToList();
            }
        }

        public int GetTaskWaitingCount(string userName)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] statusesId = new int[] { 1, 2 };
                return ctx.TaskTracker.Where(x => x.UserAssigned == userName && statusesId.Contains(x.TaskTrackerStatusId)).Count();
            }
        }
        public static string TextToHtml(string text)
        {
            //text = HttpUtility.HtmlEncode(text);
            text = text.Replace("\r\n", "\r");
            text = text.Replace("\n", "\r");
            text = text.Replace("\r", "<br>\r\n");
            text = text.Replace("  ", " &nbsp;");
            return text;
        }

        public int SetTaskUpdate(TaskTracker tt, Dal.TaskTrackerStatusHistory ttsh)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int result = 0;
                try
                {
                    StringBuilder sb = new StringBuilder();

                    if (tt.TaskId != 0)
                    {
                        Dal.TaskTracker ttToUpdate = ctx.TaskTracker.Where(x => x.TaskId == tt.TaskId).FirstOrDefault();

                        if (ttToUpdate.Comment != tt.Comment)
                            sb.AppendLine(String.Format("<b>Zmiana pola Opis. Było</b><br><br>{0}<br>", TextToHtml(ttToUpdate.Comment)));
                        if (ttToUpdate.FinishDate != tt.FinishDate)
                            sb.AppendLine(String.Format("<b>Zmiana data zakończenia. Było </b>{0:yyyy/MM/dd}<br>", ttToUpdate.FinishDate));
                        if (ttToUpdate.TaskTrackerStatusId != tt.TaskTrackerStatusId)
                        {
                            sb.AppendLine(String.Format("<b>Zmiana pola status. Było </b>{0}<br>", ttToUpdate.TaskTrackerStatus.Name));
                            ttToUpdate.TaskTrackerStatus = ctx.TaskTrackerStatus.Where(x => x.TaskTrackerStatusId == tt.TaskTrackerStatusId).FirstOrDefault();
                        }
                        if (ttToUpdate.Title != tt.Title)
                            sb.AppendLine(String.Format("<b>Zmiana pola tytuł. Było </b>{0}<br>", ttToUpdate.Title));
                        if (ttToUpdate.UserAssigned != tt.UserAssigned)
                            sb.AppendLine(String.Format("<b>Zmiana przypisanego użytkownika. Było </b>{0}<br>", ttToUpdate.UserAssigned));
                        if (ttToUpdate.WorkingHours != tt.WorkingHours)
                            sb.AppendLine(String.Format("<b>Zmiana liczby godzin. Było </b>{0}<br>", ttToUpdate.WorkingHours));
                        if (ttToUpdate.IsRecurringTask != tt.IsRecurringTask)
                            sb.AppendLine(String.Format("<b>Zmiana zadanie cykliczne Było </b>{0}<br>", ttToUpdate.IsRecurringTask));

                        if (ttsh.Comment != "")
                            ttsh.Comment = String.Format("<b>Komenarz do zmiany statusu:</b><br>{0}<br><br><b>Odnotowne zmiany:</b><br>{1}", TextToHtml(ttsh.Comment), sb.ToString());
                        else
                            if (sb.Length > 0)
                            ttsh.Comment = String.Format("<b>Odnotowne zmiany:</b><br>{0}", sb.ToString());

                        ttToUpdate.TaskTypeId = tt.TaskTypeId;
                        ttToUpdate.Comment = tt.Comment;
                        ttToUpdate.FinishDate = tt.FinishDate;
                        ttToUpdate.Title = tt.Title;
                        ttToUpdate.UserAssigned = tt.UserAssigned;
                        ttToUpdate.WorkingHours = tt.WorkingHours;
                        ttToUpdate.Priority = tt.Priority;
                        ttToUpdate.IsRecurringTask = tt.IsRecurringTask;
                    }

                    else
                    {
                        tt.InsertDate = DateTime.Now;
                        tt.InsertUser = ttsh.InsertUser;
                        ttsh.TaskTracker = tt;
                        ctx.TaskTracker.InsertOnSubmit(tt);

                    }

                    ctx.TaskTrackerStatusHistory.InsertOnSubmit(ttsh);
                    ctx.SubmitChanges();
                    result = tt.TaskId;
                }
                catch (Exception ex)
                {
                    Dal.ErrorHandler.LogError(ex, String.Format("Zapis zadania {0}", tt.TaskId));
                }
                return result;
            }
        }

        public List<TaskTrackerType> GetTaskTypes()
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                return ctx.TaskTrackerType.ToList();
            }
            }

        public void SetTaskPriority(Dictionary<int, int> priorities)
        {
            using (LajtitDB ctx = new LajtitDB())
            {
                int[] taskIds = priorities.Keys.ToArray();

                List<Dal.TaskTracker> tasks = ctx.TaskTracker.Where(x => taskIds.Contains(x.TaskId)).ToList();

                foreach (Dal.TaskTracker tt in tasks)
                    tt.Priority = priorities[tt.TaskId];

                ctx.SubmitChanges();
            }
        }
    }
}