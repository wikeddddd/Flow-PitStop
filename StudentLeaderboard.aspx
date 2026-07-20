<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="StudentLeaderboard.aspx.cs" Inherits="PitStop.StudentLeaderboard" %>


<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Driver Standings</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <div class="dashboard-container">


    
        <nav class="sidebar-nav">
            <asp:LinkButton ID="lnkTasks" runat="server" PostBackUrl="~/StudentDashboard.aspx">Dashboard</asp:LinkButton>
            <asp:LinkButton ID="lnkLeaderboard" runat="server" PostBackUrl="~/PitWallTasks.aspx">Pit Wall Tasks</asp:LinkButton>
            <asp:LinkButton ID="lnkProfile" runat="server" PostBackUrl="~/ManageUserProfile.aspx">User Profile</asp:LinkButton>
        </nav>
     </div>
        <main class="content-workspace">
            <h1 class="section-headline">Driver Standings</h1>
            <div class="dashboard-card">
                <asp:GridView ID="gvLeaderboard" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="grid-view" DataSourceID="SqlDataSource1">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id"/>
                        <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" />
                        <asp:BoundField DataField="SchoolName" HeaderText="SchoolName" ItemStyle-Font-Bold="true" SortExpression="SchoolName"/>
                        <asp:BoundField DataField="TotalXp" HeaderText="TotalXp" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" SortExpression="TotalXp" />
                        <asp:BoundField DataField="CurrentLevel" HeaderText="CurrentLevel" SortExpression="CurrentLevel" />
                        <asp:BoundField DataField="DailyStreak" HeaderText="DailyStreak" SortExpression="DailyStreak" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommand="SELECT u.Id, u.FirstName, u.SchoolName, g.TotalXp, g.CurrentLevel, g.DailyStreak 
                                 FROM Gamification g
                                 INNER JOIN Students u ON g.Id = u.Id
                                 ORDER BY g.TotalXp DESC, g.DailyStreak DESC"></asp:SqlDataSource>
            </div>
   
</asp:Content>
