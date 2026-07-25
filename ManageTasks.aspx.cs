using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace PitStop
{
    public partial class ManageTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["role"] == null)
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            string role = Session["role"].ToString();

            if (role != "advisor" && role != "admin")
            {
                Response.Redirect("~/StudentDashboard.aspx");
                return;
            }

            if (role == "admin")
            {
                advisorNav.Visible = false;
            }

            if (!IsPostBack)
            {
                BindTasks();
            }
        }

        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = "INSERT INTO Tasks (StudentId, title, description, xpReward, dueDate, status, AdvisorId) " +
                           "VALUES (@Id, @title, @description, @xpReward, @dueDate, @status, @advisorId)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Id", int.Parse(txtStudentId.Text));
                    command.Parameters.AddWithValue("@title", txtTitle.Text);
                    command.Parameters.AddWithValue("@description", txtDescription.Text);
                    command.Parameters.AddWithValue("@xpReward", int.Parse(txtXPReward.Text));
                    command.Parameters.AddWithValue("@dueDate", DateTime.Parse(txtDueDate.Text));
                    command.Parameters.AddWithValue("@status", "Pending");
                    command.Parameters.AddWithValue("@advisorId", Session["LoggedInUserID"]);


                    connection.Open();
                    command.ExecuteNonQuery();
                }

                txtTitle.Text = "";
                txtDescription.Text = "";
                txtXPReward.Text = "";
                txtDueDate.Text = "";
                txtStudentId.Text = "";
                lblMessage.Text = "Task added successfully!";
                lblMessage.CssClass = "feedback-msg success";
            }
            catch (FormatException)
            {
                lblMessage.Text = "Student ID and XP Reward must be numbers, and Due Date must be valid.";
                lblMessage.CssClass = "feedback-msg error";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error adding task: " + ex.Message;
                lblMessage.CssClass = "feedback-msg error";
            }

            BindTasks();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfSelectedTaskId.Value))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Please select a task from the table first!');", true);
                return;
            }

            int taskId = int.Parse(hfSelectedTaskId.Value);
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            // 1. First, check if the task is already approved so XP isn't awarded twice
            string selectQuery = "SELECT StudentId, xpReward, status FROM Tasks WHERE TaskId = @TaskId";

            // 2. Update task status
            string updateTaskQuery = "UPDATE Tasks SET status = 'Approved' WHERE TaskId = @TaskId";

            // 3. Increment student's total XP (adjust TotalXP to match your column name in Students table)
            string updateStudentXpQuery = "UPDATE Gamification SET totalXp = ISNULL(totalXp, 0) + @XpReward WHERE Id = @StudentId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    int studentId = 0;
                    int xpReward = 0;
                    string currentStatus = "";

                    // Fetch Task Details
                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, connection))
                    {
                        selectCmd.Parameters.AddWithValue("@TaskId", taskId);
                        using (SqlDataReader reader = selectCmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                studentId = Convert.ToInt32(reader["StudentId"]);
                                xpReward = Convert.ToInt32(reader["xpReward"]);
                                currentStatus = reader["status"].ToString();
                            }
                        }
                    }

                    // Guard clause: Prevent re-approving and duplicating XP rewards
                    if (currentStatus == "Approved")
                    {
                        lblMessage.Text = "This task has already been approved.";
                        lblMessage.CssClass = "feedback-msg error";
                        return;
                    }

                    // Execute Updates inside a Transaction
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // Approve Task
                            using (SqlCommand taskCmd = new SqlCommand(updateTaskQuery, connection, transaction))
                            {
                                taskCmd.Parameters.AddWithValue("@TaskId", taskId);
                                taskCmd.ExecuteNonQuery();
                            }

                            // Add XP to Student
                            using (SqlCommand studentCmd = new SqlCommand(updateStudentXpQuery, connection, transaction))
                            {
                                studentCmd.Parameters.AddWithValue("@StudentId", studentId);
                                studentCmd.Parameters.AddWithValue("@XpReward", xpReward);
                                studentCmd.ExecuteNonQuery();
                            }

                            // Commit changes
                            transaction.Commit();

                            lblMessage.Text = $"Task approved successfully! {xpReward} XP awarded to student.";
                            lblMessage.CssClass = "feedback-msg success";
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw; // Rethatch to trigger outer catch
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error approving task: " + ex.Message;
                lblMessage.CssClass = "feedback-msg error";
            }

            BindTasks();
        }

        private void BindTasks()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = @"SELECT t.TaskId, t.StudentId, t.title, t.description, t.xpReward, t.dueDate, t.status, t.AdvisorId,
                             (st.firstName + ' ' + st.lastName) AS StudentName
                             FROM Tasks t
                             JOIN Students st ON t.StudentId = st.StudentId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                adapter.Fill(dt);
                gvTasks.DataSource = dt;
                gvTasks.DataBind();
            }
        }

        protected void gvTasks_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvTasks.EditIndex = e.NewEditIndex;
            BindTasks();
        }

        protected void gvTasks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvTasks.EditIndex = -1;
            BindTasks();
        }

        protected void gvTasks_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int taskId = Convert.ToInt32(gvTasks.DataKeys[e.RowIndex].Values["TaskId"]);

            GridViewRow row = gvTasks.Rows[e.RowIndex];
            string title = ((TextBox)row.Cells[1].Controls[0]).Text;
            string description = ((TextBox)row.Cells[2].Controls[0]).Text;
            string status = ((TextBox)row.Cells[5].Controls[0]).Text;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            try
            {
                int xpReward = int.Parse(((TextBox)row.Cells[3].Controls[0]).Text);
                DateTime dueDate = DateTime.Parse(((TextBox)row.Cells[4].Controls[0]).Text);

                string query = "UPDATE Tasks SET title = @title, description = @description, " +
                               "xpReward = @xpReward, dueDate = @dueDate, status = @status WHERE TaskId = @TaskId";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@xpReward", xpReward);
                    command.Parameters.AddWithValue("@dueDate", dueDate);
                    command.Parameters.AddWithValue("@status", status);
                    command.Parameters.AddWithValue("@TaskId", taskId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
                lblMessage.Text = "Task updated.";
                lblMessage.CssClass = "feedback-msg success";
            }
            catch (FormatException)
            {
                lblMessage.Text = "XP Reward must be a number and Due Date must be a valid date.";
                lblMessage.CssClass = "feedback-msg error";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error updating task: " + ex.Message;
                lblMessage.CssClass = "feedback-msg error";
            }

            gvTasks.EditIndex = -1;
            BindTasks();
        }

        protected void gvTasks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int taskId = Convert.ToInt32(gvTasks.DataKeys[e.RowIndex].Values["TaskId"]);

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TaskId", taskId);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                lblMessage.Text = "Task deleted.";
                lblMessage.CssClass = "feedback-msg success";
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error deleting task: " + ex.Message;
                lblMessage.CssClass = "feedback-msg error";
            }

            BindTasks();
        }

        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvTasks.UniqueID, "Select$" + e.Row.RowIndex);

                for (int i = 0; i < e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvTasks, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[i].Style["cursor"] = "pointer";
                }

                LinkButton deleteButton = e.Row.Cells[e.Row.Cells.Count - 1].Controls.OfType<LinkButton>()
                    .FirstOrDefault(b => b.CommandName == "Delete");

                if (deleteButton != null)
                {
                    deleteButton.OnClientClick = "return confirm('Are you sure you want to delete this task?');";
                }

                string status = DataBinder.Eval(e.Row.DataItem, "status").ToString();
                TableCell statusCell = e.Row.Cells[5];
                string cssClass = status == "Approved" ? "status-approved" : status == "Rejected" ? "status-rejected" : "status-pending";
                statusCell.Text = $"<span class='{cssClass}'>{status}</span>";
            }
        }

        protected void gvTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow row = gvTasks.SelectedRow;

            hfSelectedTaskId.Value = gvTasks.SelectedDataKey["TaskId"].ToString();

            if (gvTasks.SelectedDataKey["StudentId"] != null)
            {
                txtStudentId.Text = gvTasks.SelectedDataKey["StudentId"].ToString();
            }

            txtTitle.Text = HttpUtility.HtmlDecode(row.Cells[1].Text);
            txtDescription.Text = HttpUtility.HtmlDecode(row.Cells[2].Text);
            txtXPReward.Text = HttpUtility.HtmlDecode(row.Cells[3].Text);

            if (DateTime.TryParse(row.Cells[4].Text, out DateTime parsedDate))
            {
                txtDueDate.Text = parsedDate.ToString("yyyy-MM-dd");
            }
        }
    }
}