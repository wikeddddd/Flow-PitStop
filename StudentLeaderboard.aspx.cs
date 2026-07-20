using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitStop
{
    public partial class StudentLeaderboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUserId"] == null)
            {
                Session["LoggedInUserId"] = 1;
            }
            if (!IsPostBack) {
                BindLeaderboardStandings();
            }
        }

        private void BindLeaderboardStandings()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                string sqlQuery = "SELECT TOP 10 s.Id, s.Name, g.totalXp, g.currentLevel, g.dailyStreak FROM Students s INNER JOIN Gamification g ON s.Id = g.Id ORDER BY g.totalXp DESC";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    try
                    {
                        con.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);
                            gvLeaderboard.DataSource = dt;
                            gvLeaderboard.DataBind();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

            protected void gvLeaderboard_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int position = e.Row.RowIndex + 1;
                Label lblPos = (Label)e.Row.FindControl("lblPosition");

                if (lblPos != null) {
                    if (position == 1)
                    {
                        lblPos.Text = "1st";

                    }
                    else if (position == 2)
                    {
                        lblPos.Text = "2nd";
                    }
                    else if (position == 3) {
                        lblPos.Text = "3rd";
                    }
                }

                }
            }
        }
    }
