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
            // Auth guard — advisors only
            if (Session["role"] == null || Session["role"].ToString() != "advisor")
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                BindStudents();
                BindDeactivatedStudents();
            }
        }

        // ------------------------------------------------------------------
        // Load active students only (IsActive = 1)
        // ------------------------------------------------------------------
        private void BindStudents()
        {
            string connStr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = @"SELECT Id, username, firstName, lastName, schoolName, email, phoneNumber
                             FROM   Students
                             WHERE  IsActive = 1";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand     cmd = new SqlCommand(query, con);
                SqlDataAdapter da  = new SqlDataAdapter(cmd);
                DataTable      dt  = new DataTable();

                con.Open();
                da.Fill(dt);
                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }

        // ------------------------------------------------------------------
        // Load deactivated students (IsActive = 0)
        // ------------------------------------------------------------------
        private void BindDeactivatedStudents()
        {
            string connStr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query = @"SELECT Id, username, firstName, lastName, schoolName, email, phoneNumber
                             FROM   Students
                             WHERE  IsActive = 0";

            using (SqlConnection con = new SqlConnection(connStr))
            {
                SqlCommand     cmd = new SqlCommand(query, con);
                SqlDataAdapter da  = new SqlDataAdapter(cmd);
                DataTable      dt  = new DataTable();

                con.Open();
                da.Fill(dt);

                gvDeactivated.DataSource = dt;
                gvDeactivated.DataBind();

                // Hide the whole section if there are no deactivated students
                pnlDeactivated.Visible = dt.Rows.Count > 0;
            }
        }

        // ------------------------------------------------------------------
        // Edit / Cancel / Update (active grid)
        // ------------------------------------------------------------------
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

            GridViewRow row        = gvStudents.Rows[e.RowIndex];
            string firstName       = ((TextBox)row.Cells[2].Controls[0]).Text;
            string lastName        = ((TextBox)row.Cells[3].Controls[0]).Text;
            string schoolName      = ((TextBox)row.Cells[4].Controls[0]).Text;
            string email           = ((TextBox)row.Cells[5].Controls[0]).Text;
            string phoneNumber     = ((TextBox)row.Cells[6].Controls[0]).Text;

            string connStr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query   = @"UPDATE Students
                               SET    firstName   = @firstName,
                                      lastName    = @lastName,
                                      schoolName  = @schoolName,
                                      email       = @email,
                                      phoneNumber = @phoneNumber
                               WHERE  Id = @Id";

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@firstName",   firstName);
                    cmd.Parameters.AddWithValue("@lastName",    lastName);
                    cmd.Parameters.AddWithValue("@schoolName",  schoolName);
                    cmd.Parameters.AddWithValue("@email",       email);
                    cmd.Parameters.AddWithValue("@phoneNumber", phoneNumber);
                    cmd.Parameters.AddWithValue("@Id",          studentId);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMessage.Text     = "Student updated.";
                lblMessage.CssClass = "feedback-msg success";
            }
            catch (Exception ex)
            {
                lblMessage.Text     = "Error updating student: " + ex.Message;
                lblMessage.CssClass = "feedback-msg error";
            }

            gvStudents.EditIndex = -1;
            BindStudents();
        }

        // ------------------------------------------------------------------
        // Soft delete — sets IsActive = 0, no data is removed
        // ------------------------------------------------------------------
        protected void gvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int studentId = Convert.ToInt32(gvStudents.DataKeys[e.RowIndex].Value);

            string connStr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query   = "UPDATE Students SET IsActive = 0 WHERE Id = @Id";

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", studentId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMessage.Text     = "Student deactivated. Their tasks and records have been preserved.";
                lblMessage.CssClass = "feedback-msg success";
            }
            catch (Exception ex)
            {
                lblMessage.Text     = "Error deactivating student: " + ex.Message;
                lblMessage.CssClass = "feedback-msg error";
            }

            BindStudents();
            BindDeactivatedStudents();
        }

        // ------------------------------------------------------------------
        // Confirmation prompt on the Deactivate button
        // ------------------------------------------------------------------
        protected void gvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton deactivateBtn = e.Row.Cells[e.Row.Cells.Count - 1]
                                              .Controls.OfType<LinkButton>()
                                              .FirstOrDefault(b => b.CommandName == "Delete");

                if (deactivateBtn != null)
                {
                    deactivateBtn.Text            = "Deactivate";
                    deactivateBtn.OnClientClick   =
                        "return confirm('This will deactivate the student. " +
                        "Their tasks and XP records will be kept. You can restore them at any time.');";
                }
            }
        }

        // ------------------------------------------------------------------
        // Restore — sets IsActive = 1 on a deactivated student
        // ------------------------------------------------------------------
        protected void gvDeactivated_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Restore") return;

            int studentId = Convert.ToInt32(e.CommandArgument);

            string connStr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string query   = "UPDATE Students SET IsActive = 1 WHERE Id = @Id";

            try
            {
                using (SqlConnection con = new SqlConnection(connStr))
                {
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", studentId);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                lblMessage.Text     = "Student restored successfully.";
                lblMessage.CssClass = "feedback-msg success";
            }
            catch (Exception ex)
            {
                lblMessage.Text     = "Error restoring student: " + ex.Message;
                lblMessage.CssClass = "feedback-msg error";
            }

            BindStudents();
            BindDeactivatedStudents();
        }
    }
}
