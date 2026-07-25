<%@ Register TagPrefix="uc" TagName="AdvisorNav" Src="~/AdvisorNav.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="AdvisorDashboard.aspx.cs" Inherits="PitStop.AdvisorDashboard" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Advisor Dashboard</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <uc:AdvisorNav runat="server" />

    <header class="dashboard-header">
        <div class="user-profile-tag">
            Welcome back, <strong><asp:Label ID="lblAdvisorName" runat="server" Text="Advisor"></asp:Label></strong>
        </div>
    </header>

    <div class="advisor-card">
        <h3>Team Overview</h3>
        <p>Total students: <strong><asp:Label ID="lblStudentCount" runat="server" Text="0"></asp:Label></strong></p>
    </div>

    <div class="advisor-card">
        <h3>Recently Registered Students</h3>
        <asp:GridView ID="gvRecentStudents" runat="server"
            AutoGenerateColumns="false"
            CssClass="table-data"
            GridLines="None">
            <Columns>
                <asp:BoundField DataField="username"  HeaderText="Username" />
                <asp:BoundField DataField="firstName" HeaderText="First Name" />
                <asp:BoundField DataField="lastName"  HeaderText="Last Name" />
                <asp:BoundField DataField="email"     HeaderText="Email" />
            </Columns>
            <EmptyDataTemplate>No students registered yet.</EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>