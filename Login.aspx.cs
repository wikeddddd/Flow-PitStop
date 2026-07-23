using System;
using System.Collections.Generic;
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

        protected System.Void btnLogin_Click(System.Object sender, System.EventArgs e)
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

                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();

                //SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM userTable WHERE email = '" + txtEmail.Text + "' and Password = '" + txtPassword.Text + "'", con);

                int count = Convert.ToInt32(cmd.ExecuteScalar().ToString());

                if (count > 0)
                {
                    //SqlCommand cmdType = new SqlCommand("SELECT fname, email, usertype FROM userTable WHERE email = '" + txtEmail.Text + "'", con);

                    SqlDataReader dr = cmdType.ExecuteReader();

                    string type = "";
                    string name = "";
                    string email = "";


                    while (dr.Read())
                    {
                        type = dr["usertype"].ToString().Trim();
                        name = dr["fname"].ToString().Trim();
                        email = dr["email"].ToString().Trim();

                    }

                    Session["firstName"] = name;
                    Session["email"] = email;
                    Session["role"] = type;

                    if (type == "admin")
                    {
                        Response.Redirect("adminDashboard.aspx");
                    }
                    else if (type == "student")
                    {
                        Response.Redirect("StudentDashboard.aspx"); // Kena cari directory
                    }
                    else if (type == "advisor")
                    {
                        Response.Redirect("advisorDashboard.aspx"); // Kena cari directory
                    }

                }
                else
                {
                    errorMsg.Visible = true;
                    errorMsg.ForeColor = System.Drawing.Color.Red;
                    errorMsg.Text = "Username and password mismatch!";
                    return;
                }
            }
            catch (Exception ex)
            {
                errorMsg.Visible = true;
                errorMsg.ForeColor = System.Drawing.Color.Red;
                errorMsg.Text = "Error: " + ex.Message;
            }
        }

        protected System.Void lbCreateUser_Click(System.Object sender, System.EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
    
}