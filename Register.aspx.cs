using System;
using System.Collections.Generic;
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

        protected System.Void ddlRole_SelectedIndexChanged()
        {

        }

        protected System.Void btnRegister_Click()
        {
            try
            {
                if (txtFullName.Text == string.Empty)
                {
                    FullNameRequiredFieldValidator.Visible = true;
                    FullNameRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                    FullNameRequiredFieldValidator.Text = "Full name is required!";
                    return;
                }

                if (txtEmail.Text == string.Empty)
                {
                    EmailRequiredFieldValidator.Visible = true;
                    EmailRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                    EmailRequiredFieldValidator.Text = "Email is required!";
                    return;
                }

                if (txtPassword.Text == string.Empty)
                {
                    PasswordRequiredFieldValidator.Visible = true;
                    PasswordRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                    PasswordRequiredFieldValidator.Text = "Password is required!";
                    return;
                }

                if (txtConfirmPassword.Text == string.Empty) {

                    ConfirmPasswordRequiredFieldValidator.Visible = true;
                    ConfirmPasswordRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                    ConfirmPasswordRequiredFieldValidator.Text = "Confirm password is required!";
                    return;
                }


                String invalidSymbols = "#%^&*()_+=[]{};:'\"\\|,<>";
                foreach (char c in txtFullName.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        FullNameRequiredFieldValidator.Visible = true;
                        FullNameRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                        FullNameRequiredFieldValidator.Text = "Full name cannot contain prohibited symbols!";
                        return;
                    }
                }
                foreach (char c in txtPassword.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        PasswordRequiredFieldValidator.Visible = true;
                        PasswordRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                        PasswordRequiredFieldValidator.Text = "Password cannot contain prohibited symbols!";
                        return;
                    }
                }
                foreach (char c in txtEmail.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        EmailRequiredFieldValidator.Visible = true;
                        EmailRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                        EmailRequiredFieldValidator.Text = "Email cannot contain prohibited symbols!";
                        return;
                    }
                }
                foreach (char c in txtConfirmPassword.Text)
                {
                    if (invalidSymbols.Contains(c.ToString()))
                    {
                        ConfirmPasswordRequiredFieldValidator.Visible = true;
                        ConfirmPasswordRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                        ConfirmPasswordRequiredFieldValidator.Text = "Confirm password cannot contain prohibited symbols!";
                        return;
                    }
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    ConfirmPasswordRequiredFieldValidator.Visible = true;
                    ConfirmPasswordRequiredFieldValidator.ForeColor = System.Drawing.Color.Red;
                    ConfirmPasswordRequiredFieldValidator.Text = "Passwords do not match!";
                    return;
                }

                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                con.Open();

                //SqlCommand checkCMD = new SqlCommand("SELECT COUNT(*) FROM userTable WHERE email ='" + txtEmail.Text + "' ", con);
                int userExists = (int)checkCMD.ExecuteScalar();

                if (userExists > 0)
                {
                    errorMsg.Text = "Account already exists. Please choose a different email.";
                    return;
                }
                else
                {
                   // SqlCommand cmd = new SqlCommand("INSERT INTO userTable (username, Password ,firstName,lastName,email,phoneNumber,userType) VALUES ('" + txtUsername.Text + "', '" + txtPassword.Text + "', '" + txtFirstName.Text + "', '" + txtLastName.Text + "', '" + txtEmail.Text + "', '" + txtPhoneNumber.Text + "', '" + lbUserRole.SelectedValue + "')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    errorMsg.ForeColor = System.Drawing.Color.Green;
                    errorMsg.Text = "User created successfully!";
                }
                con.Close();

            }
            catch (Exception ex)
            {
                errorMsg.ForeColor = System.Drawing.Color.Red;
                errorMsg.Text = "An error occurred: " + ex.Message;

            }
        }

        protected System.Void lbLogin_Click(System.Object sender, System.EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        
    }
}