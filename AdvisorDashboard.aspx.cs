using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PitStop
{
    public partial class AdvisorDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Auth guard — only advisors may access this page
            if (Session["role"] == null || Session["role"].ToString() != "advisor")
            {
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
                return;
            }

            if (!IsPostBack)
            {
                BindDashboard();
            }
        }

        private void BindDashboard()
        {
            // Greet the logged-in advisor
            if (Session["username"] != null)
                lblAdvisorName.Text = Session["username"].ToString();

            string connStr = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Total students
                SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM Students", con);
                lblStudentCount.Text = cmdCount.ExecuteScalar().ToString();

                // Last 5 registered students
                SqlCommand cmdStudents = new SqlCommand(
                    "SELECT TOP 5 username, firstName, lastName, email FROM Students ORDER BY StudentId DESC", con);

                SqlDataAdapter da = new SqlDataAdapter(cmdStudents);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvRecentStudents.DataSource = dt;
                gvRecentStudents.DataBind();
            }
        }
    }
}