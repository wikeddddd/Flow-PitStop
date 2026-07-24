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

        protected void btnLogin_Click(System.Object sender, System.EventArgs e)
        {
            try
            {
                if (txtEmail.Text == string.Empty)
                {
                    rfvEmail.Visible = true;
                    rfvEmail.ForeColor = System.Drawing.Color.Red;
                    rfvEmail.Text = "Email is required!";
                    return;
                }

                if (txtPassword.Text == string.Empty)
                {
                    rfvPassword.Visible = true;
                    rfvPassword.ForeColor = System.Drawing.Color.Red;
                    rfvPassword.Text = "Password is required!";
                    return;
                }

                String invalidSymbols = "#%^&*()_+=-[]{};:'\"\\|,.<>";

                foreach (char c in txtEmail.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        rfvEmail.Visible = true;
                        rfvEmail.ForeColor = System.Drawing.Color.Red;
                        rfvEmail.Text = "Email cannot contain prohibited symbols!";
                        return;
                    }
                }

                foreach (char c in txtPassword.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        rfvPassword.Visible = true;
                        rfvPassword.ForeColor = System.Drawing.Color.Red;
                        rfvPassword.Text = "Password cannot contain prohibited symbols!";
                        return;
                    }
                }

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();

                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM UserPitStop WHERE email = '" + txtEmail.Text + "' and Password = '" + txtPassword.Text + "'", con);

                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (count > 0)
                {
                    SqlCommand cmdType = new SqlCommand("SELECT email, usertype FROM UserPitStop WHERE email = '" + txtEmail.Text + "'", con);

                    SqlDataReader dr = cmdType.ExecuteReader();

                    string type = "";
                    string email = "";
                    string username = "";
                    string LoginID = "";

                    while (dr.Read())
                    {
                        type = dr["usertype"].ToString().Trim();
                        email = dr["email"].ToString().Trim();

                    }

                    SqlCommand cmdData = new SqlCommand("SELECT * FROM @Table WHERE email = '" + txtEmail.Text + "'", con);
                    switch (type)
                    {
                        case "admin":
                            cmdData.Parameters.AddWithValue("@Table", "Admin");
                            break;
                        case "student":
                            cmdData.Parameters.AddWithValue("@Table", "Students");
                            break;
                        case "advisor":
                            cmdData.Parameters.AddWithValue("@Table", "Advisors");
                            break;
                        default:
                            lblLoginError.Visible = true;
                            lblLoginError.ForeColor = System.Drawing.Color.Red;
                            lblLoginError.Text = "Invalid user type!";
                            break;
                    }
                    SqlDataReader drData = cmdData.ExecuteReader();

                    while (drData.Read())
                    {
                        username = drData["username"].ToString().Trim();
                        LoginID = drData["id"].ToString().Trim();
                    }

                    Session["email"] = email;
                    Session["role"] = type;
                    Session["username"] = username;
                    Session["LoggedInUserID"] = LoginID;

                    if (type == "admin")
                    {
                        Response.Redirect("AdminDashboard.aspx");
                    }
                    else if (type == "student")
                    {
                        Response.Redirect("StudentDashboard.aspx"); 
                    }
                    else if (type == "advisor")
                    {
                        Response.Redirect("AdvisorDashboard.aspx");
                    }

                }
                else
                {
                    lblLoginError.Visible = true;
                    lblLoginError.ForeColor = System.Drawing.Color.Red;
                    lblLoginError.Text = "Username and password mismatch!";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblLoginError.Visible = true;
                lblLoginError.ForeColor = System.Drawing.Color.Red;
                lblLoginError.Text = "Error: " + ex.Message;
            }
        }

        protected void lbCreateUser_Click(System.Object sender, System.EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
    
}