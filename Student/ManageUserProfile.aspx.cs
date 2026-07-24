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
    public partial class ManageUserProfile : System.Web.UI.Page
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
                
            } catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string sqlQuery;
                if (newAvatarPath != null)
                    
                {
                    sqlQuery = "UPDATE Students SET username = @Username, password = @Password, firstName = @FirstName, lastName = @LastName, email = @Email, phoneNumber = @PhoneNum, avatarPath = @AvatarPath WHERE Id = @StudentId";
                }
                else { 
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
                    if (newAvatarPath != null) {
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
    }
}