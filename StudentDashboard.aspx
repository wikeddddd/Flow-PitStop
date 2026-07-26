<%@ Page Language="C#" MasterPageFile="~/MemberSection.Master" AutoEventWireup="true" CodeBehind="StudentDashboard.aspx.cs" Inherits="PitStop.StudentDashboard" %>


<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">


    <title>Student Dashboard</title>
</asp:Content>


<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
        <header class="dashboard-header">
            <div class="user-profile-tag">
                Welcome back, <strong><asp:Label ID="lblUserProfile" runat="server" Text="User Profile"></asp:Label></strong>
            </div>
        </header>
        <div>
        </div>
        <main class="content-workspace">
            <h1 class="section-headline">Driver Performance</h1>

            <div class="dashboard-card">
                <asp:Label ID="Label1" runat="server" Text="Current Rank:"></asp:Label>
                <asp:Label ID="lblCurrentRank" runat="server" Text="N/A"></asp:Label>
                <asp:Label ID="Label3" runat="server" Text="Streak:"></asp:Label>
                <asp:Label ID="lblStreak" runat="server" Text="N/A"></asp:Label>
                <asp:Label ID="Label4" runat="server" Text="Total Points:"></asp:Label>
                <asp:Label ID="lblTotalPoints" runat="server" Text="N/A"></asp:Label>
                <span>Engine Efficiency Level</span>
                <asp:Label ID="lblEngineEfficiency" runat="server" Text="0 / 0 XP"></asp:Label>
                <asp:Literal ID="litProgressTracker" runat="server"></asp:Literal>
            </div>
        </main>

</asp:Content>

