using System;

namespace PitStop
{
    public partial class MemberSection : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string role = Session["UserRole"]?.ToString();
            string userName = Session["UserName"]?.ToString();

            if (!string.IsNullOrEmpty(userName))
            {
                lblUserInitials.Text = GetInitials(userName);
            }

            // Point Dashboard link to the correct role's dashboard
            if (role == "Admin")
            {
                dashboardLink.Attributes["href"] = "~/Admin/Dashboard.aspx";
            }
            else
            {
                dashboardLink.Attributes["href"] = "~/Student/Dashboard.aspx";
            }
        }

        private string GetInitials(string name)
        {
            string[] parts = name.Split(' ');
            string initials = "";
            foreach (string p in parts)
            {
                if (p.Length > 0) initials += p[0];
                if (initials.Length >= 2) break;
            }
            return initials.ToUpper();
        }
    }
}