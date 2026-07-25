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
            if (Session["username"] == null)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    string sqlQuery = "SELECT TOP 1 Id, username, role FROM UserPitStop ORDER BY Id ASC";
                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        try
                        {
                            con.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    Session["username"] = reader["username"].ToString();
                                    Session["role"] = reader["role"].ToString();
                                    Session["LoggedInUserId"] = Convert.ToInt32(reader["Id"]);
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


            if (!IsPostBack)
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SELECT username FROM Students UNION SELECT username FROM Advisors UNION SELECT username FROM Admin", con);
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
        }



        protected void btnSaveProfile_Click(object sender, EventArgs e)
        {
            string username = TBUsername.Text.Trim();
            string password = TBPassword.Text.Trim();
            string firstName = TBFirstName.Text.Trim();
            string lastName = TBLastName.Text.Trim();
            string email = TBEmailAddress.Text.Trim();
            string phoneNum = TBPhoneNum.Text.Trim();
            string role = ddRole.SelectedValue;
            string newAvatarPath = null;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNum))
            {
                lblStatus.Text = "Please fill in all required fields.";
                return;
            }

            // ── resolve IDs ──────────────────────────────────────────────
            int userId = 0;
            int userToTableId = 0;
            string roleTable = "";

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    SqlCommand cmdGetUserId = new SqlCommand("SELECT Id FROM UserPitStop WHERE username = @username", con);
                    cmdGetUserId.Parameters.AddWithValue("@username", username);
                    object userIdObj = cmdGetUserId.ExecuteScalar();
                    if (userIdObj != null) userId = Convert.ToInt32(userIdObj);

                    SqlCommand cmdGetRole = new SqlCommand("SELECT role FROM UserPitStop WHERE username = @username", con);
                    cmdGetRole.Parameters.AddWithValue("@username", username);
                    object roleObj = cmdGetRole.ExecuteScalar();
                    if (roleObj != null) roleTable = roleObj.ToString();
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error resolving user: " + ex.Message;
                return;
            }

            string tableForRole = "";
            switch (roleTable)
            {
                case "Admin":    tableForRole = "Admin";    break;
                case "Student":  tableForRole = "Students"; break;
                case "Advisor":  tableForRole = "Advisors"; break;
                default:
                    lblStatus.Text = "User does not have a valid role.";
                    return;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    SqlCommand cmdGetTableId = new SqlCommand($"SELECT Id FROM {tableForRole} WHERE username = @username", con);
                    cmdGetTableId.Parameters.AddWithValue("@username", username);
                    object tableIdObj = cmdGetTableId.ExecuteScalar();
                    if (tableIdObj != null) userToTableId = Convert.ToInt32(tableIdObj);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error resolving role table ID: " + ex.Message;
                return;
            }

            // ── avatar upload ─────────────────────────────────────────────
            try
            {
                if (fileUploadAvatar.HasFile)
                {
                    string folderPath = Server.MapPath("~/Uploads/Avatars/");
                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    string extension = Path.GetExtension(fileUploadAvatar.FileName).ToLower();
                    if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
                    {
                        string fileName = "avatar_" + userId + "_" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                        fileUploadAvatar.SaveAs(Path.Combine(folderPath, fileName));
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
                lblStatus.Text = "Error uploading avatar: " + ex.Message;
                return;
            }

            // ── update role table ─────────────────────────────────────────
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();

                    string sqlQuery = newAvatarPath != null
                        ? $"UPDATE {tableForRole} SET username=@Username, password=@Password, firstName=@FirstName, lastName=@LastName, email=@Email, phoneNumber=@PhoneNum, avatarPath=@AvatarPath WHERE Id=@UserId"
                        : $"UPDATE {tableForRole} SET username=@Username, password=@Password, firstName=@FirstName, lastName=@LastName, email=@Email, phoneNumber=@PhoneNum WHERE Id=@UserId";

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        cmd.Parameters.AddWithValue("@LastName", lastName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@PhoneNum", phoneNum);
                        if (newAvatarPath != null) cmd.Parameters.AddWithValue("@AvatarPath", newAvatarPath);
                        cmd.Parameters.AddWithValue("@UserId", userToTableId);

                        int rows = cmd.ExecuteNonQuery();
                        lblStatus.Text = rows > 0 ? "Profile updated successfully." : "No changes were made.";
                    }

                    // also sync email/password in UserPitStop
                    using (SqlCommand cmdSync = new SqlCommand("UPDATE UserPitStop SET email=@Email, password=@Password WHERE Id=@UserId", con))
                    {
                        cmdSync.Parameters.AddWithValue("@Email", email);
                        cmdSync.Parameters.AddWithValue("@Password", password);
                        cmdSync.Parameters.AddWithValue("@UserId", userId);
                        cmdSync.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error saving profile: " + ex.Message;
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
            string Tables = "";
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
                            SqlCommand cmdDeleteGamification = new SqlCommand("DELETE FROM Gamification WHERE Id = (SELECT Id FROM Students WHERE username = @username)", con);
                            cmdDeleteGamification.Parameters.AddWithValue("@username", selectedUsername);
                            cmdDeleteGamification.ExecuteNonQuery();
                            SqlCommand cmdDeleteTask = new SqlCommand("DELETE FROM Task WHERE studentId = (SELECT Id FROM Students WHERE username = @username)", con);
                            cmdDeleteTask.Parameters.AddWithValue("@username", selectedUsername);
                            cmdDeleteTask.ExecuteNonQuery();
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

        protected void ddUser_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (ddUser.SelectedItem == null) return;

            string selectedUsername = ddUser.SelectedItem.Text;

            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open(); 

                    
                    SqlCommand cmdGetRole = new SqlCommand("SELECT role FROM UserPitStop WHERE username = @username", con);
                    cmdGetRole.Parameters.AddWithValue("@username", selectedUsername);
                    object roleObj = cmdGetRole.ExecuteScalar();

                    if (roleObj == null)
                    {
                        lblStatus.Text = "User role not found.";
                        return;
                    }

                    string givenRole = roleObj.ToString();
                    string tableName;

                    switch (givenRole)
                    {
                        case "Admin":
                            tableName = "Admin";
                            break;
                        case "Student":
                            tableName = "Students";
                            break;
                        case "Advisor":
                            tableName = "Advisors";
                            break;
                        default:
                            lblStatus.Text = "User does not have a valid role.";
                            return;
                    }
                    

                    if (tableName == null)
                    {
                        lblStatus.Text = "User does not have a valid role.";
                        return;
                    }

                    
                    SqlCommand cmdFetch = new SqlCommand($"SELECT username, password, firstName, lastName, email, phoneNumber FROM {tableName} WHERE username = @username", con);
                    cmdFetch.Parameters.AddWithValue("@username", selectedUsername);

                    using (SqlDataReader reader = cmdFetch.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            TBUsername.Text = reader["username"].ToString();
                            TBPassword.Text = reader["password"].ToString();
                            TBFirstName.Text = reader["firstName"].ToString();
                            TBLastName.Text = reader["lastName"].ToString();
                            TBEmailAddress.Text = reader["email"].ToString();
                            TBPhoneNum.Text = reader["phoneNumber"].ToString();

                            // Set the role dropdown
                            if (ddRole.Items.FindByValue(givenRole) != null)
                            {
                                ddRole.SelectedValue = givenRole;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
            }
        }
    }
}


