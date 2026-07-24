using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace PitStop
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack) {
                InitializeDashboardSession();
            }
        }

        private void InitializeDashboardSession() 
        {
            if (Session["username"] != null) 
            {
               lblUserProfile.Text = Session["username"].ToString();
            }
        }

        protected void lbRacerLeaderboard_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditLeaderboard.aspx");
        }

        protected void lbUserProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageUserProfile.aspx");
        }

        protected void lbEditUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditUser.aspx");
        }

        protected void lbEditPitWall_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdvisorDashboard.aspx");
        }
    }
}