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
    public partial class AdvisorDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            { 
                BindTasks();
                
            }
               
        }

        protected void btnAddTask_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = "INSERT INTO Tasks (Id, title, description, xpReward, dueDate, status) " +
                           "VALUES (@Id, @title, @description, @xpReward, @dueDate, @status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", int.Parse(txtStudentId.Text)); // which student this task is assigned to
                command.Parameters.AddWithValue("@title", txtTitle.Text);
                command.Parameters.AddWithValue("@description", txtDescription.Text);
                command.Parameters.AddWithValue("@xpReward", int.Parse(txtXPReward.Text));
                command.Parameters.AddWithValue("@dueDate", DateTime.Parse(txtDueDate.Text));
                command.Parameters.AddWithValue("@status", "Pending");

                connection.Open();
                command.ExecuteNonQuery();
            }

            txtTitle.Text = "";
            txtDescription.Text = "";
            txtXPReward.Text = "";
            txtDueDate.Text = "";
            txtStudentId.Text = "";
            BindTasks();
        }
        private void BindTasks()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = @"SELECT t.TaskId, t.title, t.description, t.xpReward, t.dueDate, t.status,
                             (st.firstName + ' ' + st.lastName) AS StudentName
                             FROM Tasks t
                             JOIN Students st ON t.Id = st.Id";

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
            int taskId = Convert.ToInt32(gvTasks.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvTasks.Rows[e.RowIndex];
            string title = ((TextBox)row.Cells[1].Controls[0]).Text;
            string description = ((TextBox)row.Cells[2].Controls[0]).Text;
            int xpReward = int.Parse(((TextBox)row.Cells[3].Controls[0]).Text);
            DateTime dueDate = DateTime.Parse(((TextBox)row.Cells[4].Controls[0]).Text);
            string status = ((TextBox)row.Cells[5].Controls[0]).Text;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
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

            gvTasks.EditIndex = -1;
            BindTasks();
        }

        protected void gvTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // 1. Force ASP.NET to render the __doPostBack script on the page
                Page.ClientScript.RegisterForEventValidation(gvTasks.UniqueID, "Select$" + e.Row.RowIndex);

                // 2. Attach click event to cells (excluding the command buttons cell so edit/delete still work)
                for (int i = 0; i < e.Row.Cells.Count - 1; i++)
                {
                    e.Row.Cells[i].Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gvTasks, "Select$" + e.Row.RowIndex);
                    e.Row.Cells[i].Style["cursor"] = "pointer";
                }

                // 3. Keep delete confirmation prompt intact
                LinkButton deleteButton = e.Row.Cells[e.Row.Cells.Count - 1].Controls.OfType<LinkButton>()
                    .FirstOrDefault(b => b.CommandName == "Delete");

                System.Web.UI.WebControls.LinkButton deleteButton = e.Row.Cells[6].Controls.OfType<System.Web.UI.WebControls.LinkButton>().FirstOrDefault(b => b.CommandName == "Delete");

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

        protected void gvTasks_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int taskId = Convert.ToInt32(gvTasks.DataKeys[e.RowIndex].Value);

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TaskId", taskId);
                connection.Open();
                command.ExecuteNonQuery();
            }

            BindTasks();
        }

    }


}