using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace LajtIt.Web.Controls
{
    public partial class AllegroSellMatrixControl : LajtitControl
    {
        #region Properties
        public DateTime EndDate
        {
            get
            {
                return calEndDate.SelectedDate.Value;
            }
            set
            {
                calEndDate.SelectedDate = value;
            }
        }
        public int NumberOfWeeks
        {
            get
            {
                return Convert.ToInt32(ViewState["NumberOfWeeks"]);
            }
            set
            {
                ViewState["NumberOfWeeks"] = value;
            }
        }
        public bool? IsPromoted
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsPromoted"]);
            }
            set
            {
                ViewState["IsPromoted"] = value;
            }
        }
        public long? UserId
        {
            get
            {
                return Convert.ToInt64(ViewState["UserId"]);
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }
        #endregion
        private List<Dal.AllegroSellMatrixResult> results;
        private Dal.AllegroSellMatrixResult resultsDays;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!calEndDate.SelectedDate.HasValue)
            {
                if (!Page.IsPostBack)
                {
                    calEndDate.SelectedDate = DateTime.Now.AddDays(-7);

                }
            }
            if (Page.IsPostBack)
            {
                calEndDate.SelectedDate = DateTime.Parse(txbEndDate.Text);
            }

        }

        public void BindMatrix()
        {
            Bll.AllegroStatHelper ash = new Bll.AllegroStatHelper();
            DateTime endDate =  calEndDate.SelectedDate.Value; 
            int numberOfWeeks = Convert.ToInt32(txbNumberOfWeeks.Text.Trim());
            bool? isPromoted = null;
            long? userId = null;

            switch (ddlIsPromoted.SelectedIndex)
            {
                case 1: isPromoted = true; break;
                case 2: isPromoted = false; break;
            }
            if (ddlSeller.SelectedIndex != 0)
                userId = 0;

            results = ash.GetAllegroSellMatrix(endDate, numberOfWeeks, isPromoted, userId);
            resultsDays = GetResultsByDays();
            gvAllegroSellMatrix.DataSource = results;
            gvAllegroSellMatrix.DataBind();

        }

        private Dal.AllegroSellMatrixResult GetResultsByDays()
        {
            Dal.AllegroSellMatrixResult days = new Dal.AllegroSellMatrixResult()
            {
                Monday = results.Sum(x => x.Monday.Value),
                Tuesday = results.Sum(x => x.Tuesday.Value),
                Wednesday = results.Sum(x => x.Wednesday.Value),
                Thursday = results.Sum(x => x.Thursday.Value),
                Friday = results.Sum(x => x.Friday.Value),
                Saturday = results.Sum(x => x.Saturday.Value),
                Sunday = results.Sum(x => x.Sunday.Value),
                TotalByHour = results.Sum(x => x.TotalByHour)
            };
            results.Add(days);

         
            return days;
        }
        protected void btnShow_Click(object sender, EventArgs e)
        {
            BindMatrix();
        }
        protected void gvAllegroSellMatrix_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            int max;
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int total = resultsDays.TotalByHour.Value;

                e.Row.Cells[0].Text = String.Format("{0:0.00}%", (resultsDays.Monday * 100.00 / total));
                e.Row.Cells[1].Text = String.Format("{0:0.00}%", (resultsDays.Tuesday * 100.00 / total));
                e.Row.Cells[2].Text = String.Format("{0:0.00}%", (resultsDays.Wednesday * 100.00 / total));
                e.Row.Cells[3].Text = String.Format("{0:0.00}%", (resultsDays.Thursday * 100.00 / total));
                e.Row.Cells[4].Text = String.Format("{0:0.00}%", (resultsDays.Friday * 100.00 / total));
                e.Row.Cells[5].Text = String.Format("{0:0.00}%", (resultsDays.Saturday * 100.00 / total));
                e.Row.Cells[6].Text = String.Format("{0:0.00}%", (resultsDays.Sunday * 100.00 / total));
                int[] a = new int[]{
                    resultsDays.Monday.Value ,
                    resultsDays.Tuesday.Value ,
                    resultsDays.Wednesday.Value,
                    resultsDays.Thursday.Value, 
                    resultsDays.Friday.Value ,
                    resultsDays.Saturday.Value ,
                    resultsDays.Sunday.Value  
                };
                max = a.Max();
                e.Row.Cells[0].BackColor = GetColor(0, max,resultsDays.Monday.Value);
                e.Row.Cells[1].BackColor = GetColor(0, max,resultsDays.Tuesday.Value);
                e.Row.Cells[2].BackColor = GetColor(0, max, resultsDays.Wednesday.Value);
                e.Row.Cells[3].BackColor = GetColor(0, max, resultsDays.Thursday.Value);
                e.Row.Cells[4].BackColor = GetColor(0, max,resultsDays.Friday.Value);
                e.Row.Cells[5].BackColor = GetColor(0, max, resultsDays.Saturday.Value);
                e.Row.Cells[6].BackColor = GetColor(0, max,resultsDays.Sunday.Value); 

            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Dal.AllegroSellMatrixResult hour = e.Row.DataItem as Dal.AllegroSellMatrixResult;

                if (!hour.Hour.HasValue)
                {
                    e.Row.Style.Add("font-weight", "bold");
                }
                else
                {
                      max = results.Where(x=>x.Hour!=null).Max(x => x.TotalByHour.Value);
                     double value = hour.TotalByHour.Value * 100.00 / resultsDays.TotalByHour.Value;
                    e.Row.Cells[8].Text = String.Format("{0:0.00}%", value);
                    e.Row.Cells[8].BackColor = GetColor(0, max, hour.TotalByHour.Value);
                }


                if (e.Row.RowIndex < 24)
                {
                    max = results.Where(x => x.Hour != null).Max(x => x.MaxNumber.Value);
                    for (int c = 0; c < 7; c++)
                    {
                        int number = Convert.ToInt32(e.Row.Cells[c].Text);
                        e.Row.Cells[c].BackColor = GetColor(0, max, number);
                        e.Row.Cells[c].ForeColor = Color.Silver;
                    }

                }
            }
        }
        //public Color GetBlendedColor(int percentage)
        //{
        //    if (percentage < 50)
        //        return Interpolate(Color.Red, Color.Yellow, percentage / 50.0);
        //    return Interpolate(Color.Yellow, Color.Green, (percentage - 50) / 50.0);
        //}

        //private Color Interpolate(Color color1, Color color2, double fraction)
        //{
        //    double r = Interpolate(color1.R, color2.R, fraction);
        //    double g = Interpolate(color1.G, color2.G, fraction);
        //    double b = Interpolate(color1.B, color2.B, fraction);
        //    return Color.FromArgb((int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
        //}

        //private double Interpolate(double d1, double d2, double fraction)
        //{
        //    return d1 + (d1 - d2) * fraction;
        //}
        Color GetColor(Int32 rangeStart /*Complete Red*/, Int32 rangeEnd /*Complete Green*/, Int32 actualValue)
        {
            if (rangeStart >= rangeEnd) return Color.Black;

            Int32 max = rangeEnd - rangeStart; // make the scale start from 0
            Int32 value = actualValue - rangeStart; // adjust the value accordingly

            Int32 green = (255 * value) / max; // calculate green (the closer the value is to max, the greener it gets)
            Int32 red = 255 - green; // set red as inverse of green

            return Color.FromArgb(red, green, 0);
        }
    }
}