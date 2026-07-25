<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvisorNav.ascx.cs" Inherits="PitStop.AdvisorNav" %>

<div class="advisor-tabs">
    <a href="~/AdvisorDashboard.aspx" runat="server" id="lnkDashboard" class="tab-link">Dashboard</a>
    <a href="~/ManageTasks.aspx" runat="server" id="lnkTasks" class="tab-link">Manage Tasks</a>
    <a href="~/ManageStudents.aspx" runat="server" id="lnkStudents" class="tab-link">Manage Students</a>
    <a href="~/ManageAdvisorProfile.aspx" runat="server" id="lnkProfile" class="tab-link">Manage Profile</a>

</div>  