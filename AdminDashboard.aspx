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
        <main class="content-workspace">

            <div class="dashboard-card" style="height: 28px">
            </div>
        </main>

</asp:Content>

