using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PitStop
{
    public partial class AdvisorNav : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);

            if (currentPage == "AdvisorDashboard.aspx")
            {
                lnkDashboard.Attributes["class"] += "active";
            }
            else if (currentPage == "ManageTasks.aspx")
            {
                lnkTasks.Attributes["class"] += "active";
            }
            else if (currentPage == "ManageStudents.aspx")
            {
                lnkStudents.Attributes["class"] += "active";
            }
        }
    }
}