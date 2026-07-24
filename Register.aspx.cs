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

        

        protected void lbLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                // Hide previous messages
                lblRegisterError.Text = "";
                lblRegisterError.Visible = false;

                rfvUsername.Visible = false;
                rfvEmail.Visible = false;
                rfvPassword.Visible = false;
                cvPassword.Visible = false;

                // =============================
                // Validation
                // =============================

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    rfvUsername.Visible = true;
                    rfvUsername.ForeColor = System.Drawing.Color.Red;
                    rfvUsername.Text = "Username is required!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtEmail.Text))
                {
                    rfvEmail.Visible = true;
                    rfvEmail.ForeColor = System.Drawing.Color.Red;
                    rfvEmail.Text = "Email is required!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Text))
                {
                    rfvPassword.Visible = true;
                    rfvPassword.ForeColor = System.Drawing.Color.Red;
                    rfvPassword.Text = "Password is required!";
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtConfirmPassword.Text))
                {
                    cvPassword.Visible = true;
                    cvPassword.ForeColor = System.Drawing.Color.Red;
                    cvPassword.Text = "Confirm Password is required!";
                    return;
                }

                if (txtPassword.Text != txtConfirmPassword.Text)
                {
                    cvPassword.Visible = true;
                    cvPassword.ForeColor = System.Drawing.Color.Red;
                    cvPassword.Text = "Passwords do not match!";
                    return;
                }

                string invalidSymbols = "#%^&*()+=[]{};:'\"\\|,<>";

                foreach (char c in txtUsername.Text)
                {
                    if (invalidSymbols.Contains(c))
                    {
                        rfvUsername.Visible = true;
                        rfvUsername.ForeColor = System.Drawing.Color.Red;
                        rfvUsername.Text = "Username contains prohibited symbols!";
                        return;
                    }
                }

                foreach (char c in txtEmail.Text)
                {
                    if (invalidSymbols.Contains(c))
                    {
                        rfvEmail.Visible = true;
                        rfvEmail.ForeColor = System.Drawing.Color.Red;
                        rfvEmail.Text = "Email contains prohibited symbols!";
                        return;
                    }
                }

                foreach (char c in txtPassword.Text)
                {
                    if (invalidSymbols.Contains(c))
                    {
                        rfvPassword.Visible = true;
                        rfvPassword.ForeColor = System.Drawing.Color.Red;
                        rfvPassword.Text = "Password contains prohibited symbols!";
                        return;
                    }
                }

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString))
                {
                    con.Open();

                    // =============================
                    // Check Email
                    // =============================

                    SqlCommand checkEmail = new SqlCommand(
                        "SELECT COUNT(*) FROM UserPitStop WHERE email=@Email", con);

                    checkEmail.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    int emailUsed = Convert.ToInt32(checkEmail.ExecuteScalar());

                    if (emailUsed > 0)
                    {
                        lblRegisterError.Visible = true;
                        lblRegisterError.ForeColor = System.Drawing.Color.Red;
                        lblRegisterError.Text = "Email already exists.";
                        return;
                    }

                    // =============================
                    // Check Username
                    // =============================

                    SqlCommand checkUsername = new SqlCommand(
                        "SELECT COUNT(*) FROM UserPitStop WHERE username=@Username", con);

                    checkUsername.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());

                    int usernameUsed = Convert.ToInt32(checkUsername.ExecuteScalar());

                    if (usernameUsed > 0)
                    {
                        lblRegisterError.Visible = true;
                        lblRegisterError.ForeColor = System.Drawing.Color.Red;
                        lblRegisterError.Text = "Username already exists.";
                        return;
                    }

                    // =============================
                    // Insert into UserPitStop
                    // =============================

                    SqlCommand cmdUser = new SqlCommand(
                        @"INSERT INTO UserPitStop
                (username,password,email,role)
                VALUES
                (@Username,@Password,@Email,@UserType)", con);

                    cmdUser.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmdUser.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmdUser.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                    cmdUser.Parameters.AddWithValue("@UserType", ddlRole.SelectedValue);

                    cmdUser.ExecuteNonQuery();

                    // =============================
                    // Insert into role table
                    // =============================

                    string tableName = "";

                    switch (ddlRole.SelectedValue.ToLower())
                    {
                        case "admin":
                            tableName = "Admin";
                            break;

                        case "advisor":
                            tableName = "Advisor";
                            break;

                        case "student":
                            tableName = "Students";
                            SqlCommand cmdGame = new SqlCommand("INSERT INTO Gamification (Id) VALUES (@Id)", con);
                            cmdGame.Parameters.AddWithValue("@Id", txtUsername.Text.Trim());
                            cmdGame.ExecuteNonQuery();
                            break;

                        default:
                            lblRegisterError.Visible = true;
                            lblRegisterError.ForeColor = System.Drawing.Color.Red;
                            lblRegisterError.Text = "Invalid role selected.";
                            return;
                    }

                    SqlCommand cmdRole = new SqlCommand(
                        $"INSERT INTO {tableName} (username,password,email) VALUES (@Username,@Password,@Email)", con);

                    cmdRole.Parameters.AddWithValue("@Username", txtUsername.Text.Trim());
                    cmdRole.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmdRole.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());

                    cmdRole.ExecuteNonQuery();
                }

                // =============================
                // Success
                // =============================

                Response.Redirect("Login.aspx");
            }
            catch (Exception ex)
            {
                lblRegisterError.Visible = true;
                lblRegisterError.ForeColor = System.Drawing.Color.Red;
                lblRegisterError.Text = ex.Message;
            }
        }
    }
}