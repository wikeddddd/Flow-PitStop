<%@ Page Language="C#" MasterPageFile="~/MemberSection.Master" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="PitStop.AdminDashboard" %>


<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">


    <title>Admin Dashboard</title>
</asp:Content>


<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
        <header class="dashboard-header">
            <div class="user-profile-tag">
                Welcome back, <strong><asp:Label ID="lblUserProfile" runat="server" Text="User Profile"></asp:Label></strong>
            </div>
        </header>
        <div>
            &nbsp;</div>
        <nav class="sidebar-navigation">
        
            <asp:LinkButton ID="lbUserProfile" runat="server" PostBackUrl="~/ManageUserProfile.aspx" OnClick="lbUserProfile_Click">User Profile</asp:LinkButton>
        
            <asp:LinkButton ID="lbEditUser" runat="server" PostBackUrl="~/EditUser.aspx" OnClick="lbEditUser_Click">Manage User</asp:LinkButton>
            <asp:LinkButton ID="lbEditPitWall" runat="server" PostBackUrl="~/AdvisorDashboard.aspx" OnClick="lbEditPitWall_Click">Pit Wall Tasks</asp:LinkButton>
        
            <asp:LinkButton ID="lbRacerLeaderboard" runat="server" OnClick="lbRacerLeaderboard_Click" PostBackUrl="~/EditLeaderboard.aspx">Racer Leaderboard</asp:LinkButton>
        
        </nav>
        <main class="content-workspace">

            <div class="dashboard-card" style="height: 28px">
            </div>
        </main>

</asp:Content>

