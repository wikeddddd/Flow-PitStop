using System;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace PitStop
{
    public partial class ManageStudents : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStudents();
            }
        }

        private void BindStudents()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = "SELECT Id, username, firstName, lastName, schoolName, email, phoneNumber FROM Students";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                connection.Open();
                adapter.Fill(dt);
                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }

        protected void gvStudents_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvStudents.EditIndex = e.NewEditIndex;
            BindStudents();
        }

        protected void gvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvStudents.EditIndex = -1;
            BindStudents();
        }

        protected void gvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);

            GridViewRow row = gvStudents.Rows[e.RowIndex];
            string firstName = ((TextBox)row.Cells[2].Controls[0]).Text;
            string lastName = ((TextBox)row.Cells[3].Controls[0]).Text;
            string schoolName = ((TextBox)row.Cells[4].Controls[0]).Text;
            string email = ((TextBox)row.Cells[5].Controls[0]).Text;
            string phoneNumber = ((TextBox)row.Cells[6].Controls[0]).Text;

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = "UPDATE Students SET firstName = @firstName, lastName = @lastName, " +
                           "schoolName = @schoolName, email = @email, phoneNumber = @phoneNumber WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@firstName", firstName);
                command.Parameters.AddWithValue("@lastName", lastName);
                command.Parameters.AddWithValue("@schoolName", schoolName);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                command.Parameters.AddWithValue("@Id", studentId);

                connection.Open();
                command.ExecuteNonQuery();
            }

            gvStudents.EditIndex = -1;
            BindStudents();
        }

        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);

            string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = "DELETE FROM Students WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", studentId);
                connection.Open();
                command.ExecuteNonQuery();
            }

            BindStudents();
        }

        protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deleteButton = e.Row.Cells[e.Row.Cells.Count - 1].Controls.OfType<LinkButton>()
                    .FirstOrDefault(b => b.CommandName == "Delete");

                if (deleteButton != null)
                {
                    deleteButton.OnClientClick = "return confirm('Deleting a student also removes their Tasks and Gamification records if foreign keys cascade. Continue?');";
                }
            }
        }
    }
}