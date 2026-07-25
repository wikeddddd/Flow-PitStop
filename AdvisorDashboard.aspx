<%@ Register TagPrefix="uc" TagName="AdvisorNav" Src="~/AdvisorNav.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="AdvisorDashboard.aspx.cs" Inherits="PitStop.AdvisorDashboard" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Advisor Dashboard</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <uc:AdvisorNav runat="server" />
    <h3>Welcome back</h3>
    <p>Use the tabs above to manage tasks and students.</p>
</asp:Content>