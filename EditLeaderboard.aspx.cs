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

            if (Session["role"] != null && Session["role"].ToString() == "admin")
            {
                lnkTasks.Visible = false;
            }

            if (!IsPostBack) {
                BindLeaderboardStandings();
                BindDropdowns();
            }
        }

        private void BindDropdowns()
        {
            ddStudentID.Items.Clear();
            ddSchoolName.Items.Clear();
            ddStudentID.Items.Add(new ListItem("-- All --", ""));
            ddSchoolName.Items.Add(new ListItem("-- All --", ""));
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                con.Open();
                string query1 = "SELECT Id FROM Gamification";
                using (SqlCommand cmd = new SqlCommand(query1, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            ddStudentID.Items.Add(reader["Id"].ToString());
                    }
                }
                string query2 = "SELECT DISTINCT schoolName FROM Students WHERE schoolName IS NOT NULL AND schoolName <> ''";
                using (SqlCommand cmd = new SqlCommand(query2, con))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            ddSchoolName.Items.Add(reader["schoolName"].ToString());
                    }
                }
            }
        }

        private void BindLeaderboardStandings()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                string sqlQuery = @"SELECT TOP 10 s.StudentId as Id, s.firstName AS FirstName, s.schoolName AS SchoolName, 
                                           g.totalXp AS TotalXp, g.currentLevel AS CurrentLevel, g.dailyStreak AS DailyStreak 
                                    FROM Students s 
                                    INNER JOIN Gamification g ON s.StudentId = g.Id 
                                    ORDER BY g.totalXp DESC";
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
                        lblError.Text = "Error loading leaderboard: " + ex.Message;
                        lblError.Visible = true;
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
            string selectedStudentID = ddStudentID.SelectedValue;

            // Reset the other dropdown
            ddSchoolName.SelectedIndex = 0;

            if (string.IsNullOrEmpty(selectedStudentID))
            {
                BindLeaderboardStandings();
                return;
            }

            string query = @"SELECT s.StudentId as Id, s.firstName AS FirstName, s.schoolName AS SchoolName, 
                                     g.totalXp AS TotalXp, g.currentLevel AS CurrentLevel, g.dailyStreak AS DailyStreak 
                              FROM Students s INNER JOIN Gamification g ON s.StudentId = g.Id 
                              WHERE s.StudentId = @studentID 
                              ORDER BY g.totalXp DESC";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@studentID", selectedStudentID);
                        BindGrid(cmd);
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error filtering by student: " + ex.Message;
                    lblError.Visible = true;
                }
            }
        }

        protected void ddSchoolName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSchoolName = ddSchoolName.SelectedValue;

            // Reset the other dropdown
            ddStudentID.SelectedIndex = 0;

            if (string.IsNullOrEmpty(selectedSchoolName))
            {
                BindLeaderboardStandings();
                return;
            }

            string query = @"SELECT s.StudentId as Id, s.firstName AS FirstName, s.schoolName AS SchoolName, 
                                     g.totalXp AS TotalXp, g.currentLevel AS CurrentLevel, g.dailyStreak AS DailyStreak 
                              FROM Students s INNER JOIN Gamification g ON s.StudentId = g.Id 
                              WHERE s.schoolName = @schoolName 
                              ORDER BY g.totalXp DESC";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@schoolName", selectedSchoolName);
                        BindGrid(cmd);
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "Error filtering by school: " + ex.Message;
                    lblError.Visible = true;
                }
            }
        }

        private void BindGrid(SqlCommand cmd)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                adapter.Fill(dt);
                gvLeaderboard.DataSource = dt;
                gvLeaderboard.DataBind();
            }
        }

        protected void gvLeaderboard_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvLeaderboard.EditIndex = e.NewEditIndex;
            BindLeaderboardStandings();
        }

        protected void gvLeaderboard_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvLeaderboard.EditIndex = -1;
            BindLeaderboardStandings();
        }

        protected void gvLeaderboard_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int id = Convert.ToInt32(gvLeaderboard.DataKeys[e.RowIndex].Value);
            GridViewRow row = gvLeaderboard.Rows[e.RowIndex];

            string firstName   = ((TextBox)row.FindControl("txtFirstName")).Text.Trim();
            string schoolName  = ((TextBox)row.FindControl("txtSchoolName")).Text.Trim();
            string totalXp     = ((TextBox)row.FindControl("txtTotalXp")).Text.Trim();
            string currentLevel= ((TextBox)row.FindControl("txtCurrentLevel")).Text.Trim();
            string dailyStreak = ((TextBox)row.FindControl("txtDailyStreak")).Text.Trim();

            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "UPDATE Students SET firstName=@FirstName, schoolName=@SchoolName WHERE StudentId=@Id; " +
                        "UPDATE Gamification SET totalXp=@TotalXp, currentLevel=@CurrentLevel, dailyStreak=@DailyStreak WHERE StudentId=@Id", con))
                    {
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@SchoolName", schoolName);
                        cmd.Parameters.AddWithValue("@TotalXp", totalXp);
                        cmd.Parameters.AddWithValue("@CurrentLevel", currentLevel);
                        cmd.Parameters.AddWithValue("@DailyStreak", dailyStreak);
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                gvLeaderboard.EditIndex = -1;
                BindLeaderboardStandings();
                BindDropdowns();
                lblError.Text = "Updated successfully.";
                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Error updating: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
            }
        }

        protected void gvLeaderboard_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int id = Convert.ToInt32(gvLeaderboard.DataKeys[e.RowIndex].Value);
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(
                        "DELETE FROM Gamification WHERE StudentId=@Id; DELETE FROM Students WHERE StudentId=@Id", con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                BindLeaderboardStandings();
                BindDropdowns();
                lblError.Text = "Deleted successfully.";
                lblError.ForeColor = System.Drawing.Color.Green;
                lblError.Visible = true;
            }
            catch (Exception ex)
            {
                lblError.Text = "Error deleting: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
                lblError.Visible = true;
            }
        }
    }
}
