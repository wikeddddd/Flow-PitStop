using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitStop
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide previous errors
                lblLoginError.Visible = false;
                rfvEmail.Visible = false;
                rfvPassword.Visible = false;

                // Validate Email
                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    rfvEmail.Visible = true;
                    rfvEmail.ForeColor = System.Drawing.Color.Red;
                    rfvEmail.Text = "Email is required!";
                    return;
                }

                // Validate Password
                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    rfvPassword.Visible = true;
                    rfvPassword.ForeColor = System.Drawing.Color.Red;
                    rfvPassword.Text = "Password is required!";
                    return;
                }

                // Optional symbol validation
                string invalidSymbols = "#%^&*()+=[]{};:'\"\\|,<>";

                foreach (char c in txtEmail.Text)
                {
                    if (invalidSymbols.Contains(c))
                    {
                        rfvEmail.Visible = true;
                        rfvEmail.ForeColor = System.Drawing.Color.Red;
                        rfvEmail.Text = "Email contains invalid symbols!";
                        return;
                    }
                }

                using (SqlConnection con = new SqlConnection(
                    ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString))
                {
                    con.Open();

                    //---------------------------------------------------------
                    // Check Login
                    //---------------------------------------------------------
                    SqlCommand loginCmd = new SqlCommand(
                        @"SELECT email, role
                  FROM UserPitStop
                  WHERE email=@Email AND password=@Password", con);

                    loginCmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    loginCmd.Parameters.AddWithValue("@Password", txtPassword.Text);

                    string email = "";
                    string type = "";

                    using (SqlDataReader dr = loginCmd.ExecuteReader())
                    {
                        if (!dr.Read())
                        {
                            lblLoginError.Visible = true;
                            lblLoginError.ForeColor = System.Drawing.Color.Red;
                            lblLoginError.Text = "Email or Password is incorrect!";
                            return;
                        }

                        email = dr["email"].ToString();
                        type = dr["role"].ToString().Trim().ToLower();
                    }

                    //---------------------------------------------------------
                    // Determine table
                    //---------------------------------------------------------
                    string tableName = "";

                    switch (type)
                    {
                        case "admin":
                            tableName = "Admin";
                            break;

                        case "student":
                            tableName = "Students";
                            break;

                        case "advisor":
                            tableName = "Advisors";
                            break;

                        default:
                            lblLoginError.Visible = true;
                            lblLoginError.ForeColor = System.Drawing.Color.Red;
                            lblLoginError.Text = "Invalid user type!";
                            return;
                    }

                    //---------------------------------------------------------
                    // Get User Details
                    //---------------------------------------------------------
                    SqlCommand userCmd = new SqlCommand(
                        $"SELECT Id, username FROM {tableName} WHERE email=@Email", con);

                    userCmd.Parameters.AddWithValue("@Email", email);

                    string username = "";
                    string loginID = "";

                    using (SqlDataReader drUser = userCmd.ExecuteReader())
                    {
                        if (drUser.Read())
                        {
                            username = drUser["username"].ToString();
                            loginID = drUser["Id"].ToString();
                        }
                        else
                        {
                            lblLoginError.Visible = true;
                            lblLoginError.ForeColor = System.Drawing.Color.Red;
                            lblLoginError.Text = "User information not found.";
                            return;
                        }
                    }

                    //---------------------------------------------------------
                    // Save Session
                    //---------------------------------------------------------
                    Session["email"] = email;
                    Session["role"] = type;
                    Session["username"] = username;
                    Session["LoggedInUserID"] = loginID;

                    //---------------------------------------------------------
                    // Redirect
                    //---------------------------------------------------------
                    switch (type)
                    {
                        case "admin":
                            Response.Redirect("AdminDashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            break;

                        case "student":
                            Response.Redirect("StudentDashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            break;

                        case "advisor":
                            Response.Redirect("AdvisorDashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                lblLoginError.Visible = true;
                lblLoginError.ForeColor = System.Drawing.Color.Red;
                lblLoginError.Text = ex.Message;
            }
        }

        protected void lbCreateUser_Click(System.Object sender, System.EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
    
}