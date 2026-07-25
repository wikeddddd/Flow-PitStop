using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace PitStop
{
    public partial class PitWallTasks : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["LoggedInUserId"] == null)
                {
                    Session["LoggedInUserId"] = 1;

                }

                if (!IsPostBack)
                {
                    refreshPage();
                }
            } catch(Exception ex) { lblStatus.Text = "Error: " + ex.Message; }
            
        }

        private void refreshPage()
        { 
            LoadTasksGrid();
            LoadPendingTasksDDL();
        }

        private void LoadTasksGrid()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT t.StudentId, t.title, t.description, t.xpReward, t.status, t.dueDate FROM Tasks t INNER JOIN Students st ON t.StudentId = st.StudentId WHERE st.StudentId = @StudentId";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", Convert.ToInt32(Session["LoggedInUserId"]));
                    try
                    {
                        con.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            System.Data.DataTable dt = new System.Data.DataTable();
                            adapter.Fill(dt);
                            gvTasks.DataSource = dt;
                            gvTasks.DataBind();
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error loading tasks: " + ex.Message;
                    }
                }
            }
        }

        private void LoadPendingTasksDDL()
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT t.TaskId, t.StudentId, t.title, t.description, t.xpReward, t.status, t.dueDate FROM Tasks t INNER JOIN Students st ON t.StudentId = st.StudentId WHERE st.StudentId = @StudentId";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", Convert.ToInt32(Session["LoggedInUserId"]));
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            ddlPendingTasks.Items.Clear();
                            ddlPendingTasks.Items.Add(new ListItem("-- Select a Task --", ""));
                            while (reader.Read())
                            {
                                ddlPendingTasks.Items.Add(new ListItem(reader["title"].ToString(), reader["TaskId"].ToString()));
                            }
                        }
                        if (ddlPendingTasks.Items.Count <= 1)
                        {
                            if (ddlPendingTasks.Items.Count == 1)
                            {
                                ddlPendingTasks.Items[0].Text = "No pending tasks available";
                            }
                            btnSubmit.Enabled = false;
                        }
                        else
                        {
                            btnSubmit.Enabled = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error loading pending tasks: " + ex.Message;
                    }
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ddlPendingTasks.SelectedValue == "0" || !fileTaskUpload.HasFile)
            {
                lblStatus.Text = "Please select a task and upload a file.";
                return;
            }

            try
            {
                int targetTaskId = Convert.ToInt32(ddlPendingTasks.SelectedValue);

                string uploadPath = Server.MapPath("~/Uploads/");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                string fileName = Guid.NewGuid().ToString().Substring(0,8) + "_" + Path.GetFileName(fileTaskUpload.FileName);
                string filePath = Path.Combine(uploadPath, fileName);
                fileTaskUpload.SaveAs(filePath);
                
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "UPDATE Tasks SET status = 'Submitted', filePath = @filePath WHERE TaskId = @taskId";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@taskId", targetTaskId);
                        cmd.Parameters.AddWithValue("@filePath", filePath);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                lblStatus.Text = "Task submitted successfully!";
                refreshPage();
            }
            catch (SqlException ex)
            {
                lblStatus.Text = "Database Error: " + ex.Message;
            }
        }
    }
}