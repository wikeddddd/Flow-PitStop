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
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlRole_SelectedIndexChanged()
        {

        }

        protected void btnRegister_Click()
        {
            try
            {
                if (txtUsername.Text == string.Empty)
                {
                    rfvUsername.Visible = true;
                    rfvUsername.ForeColor = System.Drawing.Color.Red;
                    rfvUsername.Text = "Username is required!";
                    return;
                }

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

                if (txtConfirmPassword.Text == string.Empty) {

                    cvPassword.Visible = true;
                    cvPassword.ForeColor = System.Drawing.Color.Red;
                    cvPassword.Text = "Confirm password is required!";
                    return;
                }


                String invalidSymbols = "#%^&*()_+=[]{};:'\"\\|,<>";
                foreach (char c in txtUsername.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        rfvUsername.Visible = true;
                        rfvUsername.ForeColor = System.Drawing.Color.Red;
                        rfvUsername.Text = "Username cannot contain prohibited symbols!";
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
                foreach (char c in txtConfirmPassword.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        cvPassword.Visible = true;
                        cvPassword.ForeColor = System.Drawing.Color.Red;
                        cvPassword.Text = "Confirm password cannot contain prohibited symbols!";
                        return;
                    }
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    cvPassword.Visible = true;
                    cvPassword.ForeColor = System.Drawing.Color.Red;
                    cvPassword.Text = "Passwords do not match!";
                    return;
                }

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();

                SqlCommand checkCMD1= new SqlCommand("SELECT COUNT(*) FROM UserPitStop WHERE email ='" + txtEmail.Text + "' ", con);
                int usernameUsed = (int)checkCMD1.ExecuteScalar();

                SqlCommand checkCMD2 = new SqlCommand("SELECT COUNT(*) FROM UserPitStop WHERE username ='" + txtUsername.Text + "' ", con);
                int emailUsed = (int)checkCMD2.ExecuteScalar();

                if (usernameUsed > 0)
                {
                    lblRegisterError.Text = "Username already exists. Please choose a different username.";
                    return;
                }
                else if (emailUsed > 0)
                {
                    lblRegisterError.Text = "Email already exists. Please choose a different email.";
                    return;
                }
                else
                {
                    SqlCommand cmdUser = new SqlCommand("INSERT INTO UserPitStop (username, password ,email,role) VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + txtEmail.Text + "', '" + ddlRole.SelectedValue + "')", con);
                    cmdUser.ExecuteNonQuery();
                    string role = ddlRole.SelectedValue.ToString();
                    switch (role)
                    {
                        case "Admin":
                           SqlCommand cmdRole = new SqlCommand("INSERT INTO Admin (username,password,email) VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + txtEmail.Text + "')", con);
                           cmdRole.ExecuteNonQuery();
                           break;
                        case "Advisor":
                            SqlCommand cmdRole2 = new SqlCommand("INSERT INTO Advisor (username,password,email) VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + txtEmail.Text + "')", con);
                            cmdRole2.ExecuteNonQuery();
                            break;
                        case "Student":
                            SqlCommand cmdRole3 = new SqlCommand("INSERT INTO Students (username,password,email) VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + txtEmail.Text + "')", con);
                            cmdRole3.ExecuteNonQuery();
                            break;
                        default:
                            lblRegisterError.Text = "Invalid role selected.";
                            return;
                    }
                    
                }
                con.Close();

            }
            catch (Exception ex)
            {
                lblRegisterError.ForeColor = System.Drawing.Color.Red;
                lblRegisterError.Text = "An error occurred: " + ex.Message;

            }
        }

        protected void lbLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        
    }
}