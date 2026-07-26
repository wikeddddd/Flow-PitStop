using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace PitStop
{
    public partial class StudentDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUserId"] == null)
            {
                Session["LoggedInUserId"] = 1;
            }
            if (!IsPostBack) {
                InitializeDashboardSession();
            }
        }

        private void InitializeDashboardSession() 
        {
            if (Session["LoggedInUserId"] != null) 
            {
                int activeStudentId = Convert.ToInt32(Session["LoggedInUserId"]);
                if (Session["username"] != null && !string.IsNullOrEmpty(Session["username"].ToString()))
                {
                    lblUserProfile.Text = Session["username"].ToString();
                }
                else
                {
                    // Fallback to query student name/username from DB if session is empty
                    LoadStudentName(activeStudentId);
                }
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    string sqlQuery = "SELECT totalXp, currentLevel, dailyStreak FROM Gamification WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", activeStudentId);
                        try
                        {
                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    int xp = Convert.ToInt32(reader["totalXp"]);
                                    int level = Convert.ToInt32(reader["currentLevel"]);
                                    int streak = Convert.ToInt32(reader["dailyStreak"]);

                                    int levelThreshold = level * 1000; // Example threshold for leveling up

                                    lblCurrentRank.Text = level.ToString();
                                    lblStreak.Text = $"{streak} day streak";
                                    lblTotalPoints.Text = string.Format("{0:N0}", xp);
                                    litProgressTracker.Text = $"<progress value='{xp}' max='{levelThreshold}'></progress>";
                                    lblTotalPoints.Text = xp.ToString();
                                    lblCurrentRank.Text = level.ToString();
                                    lblStreak.Text = streak.ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Response.Write("Database error occurred: " + ex.Message);
                        }
                    }
                }
            
            }
        }   
        private void LoadStudentName(int studentId)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                string sqlQuery = "SELECT username, firstName FROM Students WHERE StudentId = @StudentId";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", studentId);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader["username"] != DBNull.Value ? reader["username"].ToString() : reader["firstName"].ToString();
                                lblUserProfile.Text = username;
                                Session["username"] = username; // Keep session synced
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        lblUserProfile.Text = "Student";
                    }
                }
            }
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            
        }
    }
}