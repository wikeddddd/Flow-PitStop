using System;

namespace PitStop
{
    public partial class MemberSection : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string role = Session["role"]?.ToString();
            string userName = Session["username"]?.ToString();

            if (!string.IsNullOrEmpty(userName))
            {
                lblUserInitials.Text = GetInitials(userName);
            }

            navDashboard.Visible = false;

            if (role == "admin")
            {
                dashboardLink.Attributes["href"] = ResolveUrl("~/AdminDashboard.aspx");
                phAdminNav.Visible = true;
            }
            else if (role == "advisor")
            {
                dashboardLink.Attributes["href"] = ResolveUrl("~/AdvisorDashboard.aspx");
                phAdvisorNav.Visible = true;
            }
            else
            {
                dashboardLink.Attributes["href"] = ResolveUrl("~/StudentDashboard.aspx");
                phStudentNav.Visible = true;
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
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}