using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitStop
{
    public partial class EditUser : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedInUserId"] == null)
            {
                Session["LoggedInUserId"] = 1;
            }


            if (!IsPostBack)
            {
                LoadUserProfile();
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT username FROM Students", con);
                ddUser.Items.Clear();
                try
                {
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ddUser.Items.Add(reader["username"].ToString());
                    }
                    con.Close();
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "Error: " + ex.Message;
                }
            }
        }

        private void LoadUserProfile()
        {
            int userId = Convert.ToInt32(Session["LoggedInUserId"]);
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "SELECT username, password, firstName, lastName, email, phoneNumber, avatarPath FROM Students WHERE Id = @StudentId";
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@StudentId", userId);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TBUsername.Text = reader["username"].ToString();
                                TBPassword.Text = reader["password"].ToString();
                                TBFirstName.Text = reader["firstName"].ToString();
                                TBLastName.Text = reader["lastName"].ToString();
                                TBEmailAddress.Text = reader["email"].ToString();
                                TBPhoneNum.Text = reader["phoneNumber"].ToString();

                                if (reader["avatarPath"] != DBNull.Value)
                                {
                                    imgAvatar.ImageUrl = reader["avatarPath"].ToString();
                                }
                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error: " + ex.Message;
                    }
                }
            }
        }

        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            int userId = Convert.ToInt32(Session["LoggedInUserId"]);
            string username = TBUsername.Text.Trim();
            string password = TBPassword.Text.Trim();
            string firstName = TBFirstName.Text.Trim();
            string lastName = TBLastName.Text.Trim();
            string email = TBEmailAddress.Text.Trim();
            string phoneNum = TBPhoneNum.Text.Trim();
            string role = ddRole.SelectedValue;
            string newAvatarPath = null;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNum))
            {
                lblStatus.Text = "Please fill in all required fields.";
                return;
            }

            try
            {
                if (fileUploadAvatar.HasFile)
                {
                    string folderPath = Server.MapPath("~/Uploads/Avatars/");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }


                    string extension = Path.GetExtension(fileUploadAvatar.FileName);
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string fileName = "avatar_" + userId + "_" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                        string savePath = Path.Combine(folderPath, fileName);
                        fileUploadAvatar.SaveAs(savePath);
                        newAvatarPath = "~/Uploads/Avatars/" + fileName;
                    }
                    else
                    {
                        lblStatus.Text = "Invalid file type. Please upload an image file.";
                        return;
                    }

                }

            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmdChecker = new SqlCommand("SELECT COUNT(*) FROM user WHERE username = @username", con);
                cmdChecker.Parameters.AddWithValue("@username", username);
                int count = Convert.ToInt32(cmdChecker.ExecuteScalar());

                if (count == 0) 
                {
                    SqlCommand cmdInsertUserPitStop = new SqlCommand("INSERT INTO UserPitStop (username,email,password, role) VALUES (@username, @email, @password, @role)", con);
                    cmdInsertUserPitStop.Parameters.AddWithValue("@username", username);
                    cmdInsertUserPitStop.Parameters.AddWithValue("@email", email);
                    cmdInsertUserPitStop.Parameters.AddWithValue("@password", password);
                    cmdInsertUserPitStop.Parameters.AddWithValue("@role", role);

                    SqlCommand cmdInsertRole = new SqlCommand();

                    if (newAvatarPath != null)
                    {
                        cmdInsertRole = new SqlCommand("INSERT INTO @Table (username, password, firstName,lastName,email,phoneNumber,avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber, @avatarPath)", con);
                        switch (role)
                        {
                            case "Admin":
                                cmdInsertRole = new SqlCommand("INSERT INTO Admins (username, password, firstName, lastName, email, phoneNumber, avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber, @avatarPath)", con);
                                break;
                            case "Student":
                                cmdInsertRole = new SqlCommand("INSERT INTO Students (username, password, firstName, lastName, email, phoneNumber, avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber, @avatarPath)", con);
                                break;
                            case "Advisor":
                                cmdInsertRole = new SqlCommand("INSERT INTO Advisors (username, password, firstName, lastName, email, phoneNumber, avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber, @avatarPath)", con);
                                break;
                        }
                        cmdInsertRole.Parameters.AddWithValue("@username", username);
                        cmdInsertRole.Parameters.AddWithValue("@password", password);
                        cmdInsertRole.Parameters.AddWithValue("@firstName", firstName);
                        cmdInsertRole.Parameters.AddWithValue("@lastName", lastName);
                        cmdInsertRole.Parameters.AddWithValue("@email", email);
                        cmdInsertRole.Parameters.AddWithValue("@phoneNumber", phoneNum);
                        cmdInsertRole.Parameters.AddWithValue("@avatarPath", newAvatarPath);
                    }
                    else
                    {
                        cmdInsertRole = new SqlCommand("INSERT INTO @Table (username, password, firstName,lastName,email,phoneNumber,avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber)", con);
                        switch (role)
                        {
                            case "Admin":
                                cmdInsertRole = new SqlCommand("INSERT INTO Admins (username, password, firstName, lastName, email, phoneNumber, avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber)", con);
                                break;
                            case "Student":
                                cmdInsertRole = new SqlCommand("INSERT INTO Students (username, password, firstName, lastName, email, phoneNumber, avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber)", con);
                                break;
                            case "Advisor":
                                cmdInsertRole = new SqlCommand("INSERT INTO Advisors (username, password, firstName, lastName, email, phoneNumber, avatarPath) VALUES (@username, @password, @firstName, @lastName, @email, @phoneNumber)", con);
                                break;
                        }
                        cmdInsertRole.Parameters.AddWithValue("@username", username);
                        cmdInsertRole.Parameters.AddWithValue("@password", password);
                        cmdInsertRole.Parameters.AddWithValue("@firstName", firstName);
                        cmdInsertRole.Parameters.AddWithValue("@lastName", lastName);
                        cmdInsertRole.Parameters.AddWithValue("@email", email);
                        cmdInsertRole.Parameters.AddWithValue("@phoneNumber", phoneNum);                 
                    }

                    try
                    {
                        con.Open();
                        cmdInsertUserPitStop.ExecuteNonQuery();
                        cmdInsertRole.ExecuteNonQuery();
                        con.Close();
                        lblStatus.Text = "New User created successfully.";
                        LoadUserProfile();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error: User cannot be created.\n" + ex.Message;
                    }



                }

                string sqlQuery;
                if (newAvatarPath != null)
                {
                    sqlQuery = "UPDATE Students SET username = @Username, password = @Password, firstName = @FirstName, lastName = @LastName, email = @Email, phoneNumber = @PhoneNum, avatarPath = @AvatarPath WHERE Id = @StudentId";
                }
                else
                {
                    sqlQuery = "UPDATE Students SET username = @Username, password = @Password, firstName = @FirstName, lastName = @LastName, email = @Email, phoneNumber = @PhoneNum WHERE Id = @StudentId";
                }
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PhoneNum", phoneNum);
                    if (newAvatarPath != null)
                    {
                        cmd.Parameters.AddWithValue("@AvatarPath", newAvatarPath);
                    }
                    cmd.Parameters.AddWithValue("@StudentId", userId);
                    try
                    {
                        con.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            lblStatus.Text = "Profile updated successfully.";
                        }
                        else
                        {
                            lblStatus.Text = "No changes were made.";
                        }
                        con.Close();
                        lblStatus.Text = "Profile updated successfully.";
                        LoadUserProfile();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error: " + ex.Message;
                    }
                }
            }
        }

        private void ddUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedUsername = TBUsername.Text;
            string role = Session["role"] as string;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery = "";
                switch (role)
                {
                    case "Admin":
                        sqlQuery = "SELECT username, password, firstName, lastName, email, phoneNumber, avatarPath FROM Admin WHERE username = @username";
                        break;
                    case "Advisor":
                        sqlQuery = "SELECT username, password, firstName, lastName, email, phoneNumber, avatarPath FROM Advisor WHERE username = @username";
                        break;
                    case "Student":
                        sqlQuery = "SELECT username, password, firstName, lastName, email, phoneNumber, avatarPath FROM Students WHERE username = @username";
                        break;
                    default:
                        lblStatus.Text = "User do not have a Role";
                        return;

                }
                
                using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                {
                    cmd.Parameters.AddWithValue("@username", selectedUsername);
                    try
                    {
                        con.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                TBUsername.Text = reader["username"].ToString();
                                TBPassword.Text = reader["password"].ToString();
                                TBFirstName.Text = reader["firstName"].ToString();
                                TBLastName.Text = reader["lastName"].ToString();
                                TBEmailAddress.Text = reader["email"].ToString();
                                TBPhoneNum.Text = reader["phoneNumber"].ToString();
                                ddRole.SelectedValue = role;

                                if (reader["avatarPath"] != DBNull.Value)
                                {
                                    imgAvatar.ImageUrl = reader["avatarPath"].ToString();
                                }
                            }
                        }
                        con.Close();
                    }
                    catch (Exception ex)
                    {
                        lblStatus.Text = "Error: " + ex.Message;
                    }
                }




            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string selectedUsername = ddUser.SelectedItem.Text;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmdChecker1 = new SqlCommand("SELECT COUNT(*) FROM UserPitStop WHERE username = @username", con);
                cmdChecker1.Parameters.AddWithValue("@username", selectedUsername);
                int count = Convert.ToInt32(cmdChecker1.ExecuteScalar());

                if (count > 0) 
                {
                    SqlCommand cmdChecker2 = new SqlCommand("SELECT role FROM UserPitStop WHERE username = @username", con);
                    cmdChecker2.Parameters.AddWithValue("@username", selectedUsername);
                    SqlDataReader reader = cmdChecker2.ExecuteReader();

                    string role = "";

                    while (reader.Read()) 
                    {
                        role = reader["role"].ToString();
                    }

                    switch (role)
                    {
                        case "Admin":
                            SqlCommand cmdDeleteAdmin = new SqlCommand("DELETE FROM Admin WHERE username = @username", con);
                            cmdDeleteAdmin.Parameters.AddWithValue("@username", selectedUsername);
                            cmdDeleteAdmin.ExecuteNonQuery();
                            break;
                        case "Advisor":
                            SqlCommand cmdDeleteAdvisor = new SqlCommand("DELETE FROM Advisor WHERE username = @username", con);
                            cmdDeleteAdvisor.Parameters.AddWithValue("@username", selectedUsername);
                            cmdDeleteAdvisor.ExecuteNonQuery();
                            break;
                        case "Student":
                            SqlCommand cmdDeleteStudent = new SqlCommand("DELETE FROM Students WHERE username = @username", con);
                            cmdDeleteStudent.Parameters.AddWithValue("@username", selectedUsername);
                            cmdDeleteStudent.ExecuteNonQuery();
                            break;
                        default:
                            lblStatus.Text = "User do not have a valid role.";
                            break;
                    }

                    SqlCommand cmdDeleteUserPitStop = new SqlCommand("DELETE FROM UserPitStop WHERE username = @username", con);
                    cmdDeleteUserPitStop.Parameters.AddWithValue("@username", selectedUsername);
                    cmdDeleteUserPitStop.ExecuteNonQuery();

                    lblStatus.Text = "User deleted successfully.";

                }
                else
                {
                    lblStatus.Text = "User does not exist in UserPitStop.";
                }

            }
        }
    }


}


