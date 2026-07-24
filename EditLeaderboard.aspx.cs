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
    public partial class EditLeaderboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUserId"] == null)
            {
                Session["LoggedInUserId"] = 1;
            }
            if (!IsPostBack) {
                BindLeaderboardStandings();
                ddStudentID.Items.Clear();
                ddSchoolName.Items.Clear();
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    con.Open();
                    string query1 = "SELECT Id FROM Gamification";
                    using (SqlCommand cmd = new SqlCommand(query1, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ddStudentID.Items.Add(reader["Id"].ToString());
                                }
                            }
                        
                        }

                    }
                    string query2 = "SELECT DISTINCT schoolName FROM Students";
                    using (SqlCommand cmd = new SqlCommand(query2, con))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ddSchoolName.Items.Add(reader["schoolName"].ToString());
                                }
                            }
                        }
                    }
                    con.Close();
                }
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

        protected void gvLeaderboard_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddStudentID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSchoolName.SelectedItem != null)
            {
                string selectedSchoolName = ddSchoolName.SelectedItem.Text;
                string selectedStudentID = ddStudentID.SelectedItem.Text;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    try
                    {
                        con.Open();
                        string query = "SELECT s.Id, s.Name, g.totalXp, g.currentLevel, g.dailyStreak FROM Students s INNER JOIN Gamification g ON s.Id = g.Id WHERE s.schoolName = @schoolName,s.Id = @studentID ORDER BY g.totalXp DESC";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@schoolName", selectedSchoolName);
                            cmd.Parameters.AddWithValue("@studentID", selectedStudentID);
                            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                            {
                                System.Data.DataTable dt = new System.Data.DataTable();
                                adapter.Fill(dt);
                                gvLeaderboard.DataSource = dt;
                                gvLeaderboard.DataBind();
                            }
                        }
                        con.Close();
                    } catch 
                    {
                        lblError.Text = "An error occurred while fetching data. Please try again.";
                        lblError.Visible = true;
                    }

                }
            }
            else
            {
                string selectedStudentID = ddStudentID.SelectedItem.Text;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    con.Open();
                    string query = "SELECT * FROM Gamification WHERE Id = @studentID";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@studentID", selectedStudentID);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);
                            gvLeaderboard.DataSource = dt;
                            gvLeaderboard.DataBind();
                        }
                    }
                    con.Close();
                }
            }
        }

        protected void ddSchoolName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddStudentID.SelectedItem != null)
            {
                string selectedSchoolName = ddSchoolName.SelectedItem.Text;
                string selectedStudentID = ddStudentID.SelectedItem.Text;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    con.Open();
                    string query = "SELECT s.Id, s.Name, g.totalXp, g.currentLevel, g.dailyStreak FROM Students s INNER JOIN Gamification g ON s.Id = g.Id WHERE s.schoolName = @schoolName AND s.Id = @studentID ORDER BY g.totalXp DESC"; ;
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@schoolName", selectedSchoolName);
                        cmd.Parameters.AddWithValue("@studentID", selectedStudentID);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);
                            gvLeaderboard.DataSource = dt;
                            gvLeaderboard.DataBind();
                        }
                    }
                    con.Close();
                }
            }
            else
            {
                string selectedSchoolName = ddSchoolName.SelectedItem.Text;
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    con.Open();
                    string query = "SELECT s.Id, s.Name, g.totalXp, g.currentLevel, g.dailyStreak FROM Students s INNER JOIN Gamification g ON s.Id = g.Id WHERE s.schoolName = @schoolName ORDER BY g.totalXp DESC"; ;
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@schoolName", selectedSchoolName);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);
                            gvLeaderboard.DataSource = dt;
                            gvLeaderboard.DataBind();
                        }
                    }
                    con.Close();
                }
            }
        }
    }
    }
