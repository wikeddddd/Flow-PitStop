<%@ Register TagPrefix="uc" TagName="AdvisorNav" Src="~/AdvisorNav.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="ManageStudents.aspx.cs" Inherits="PitStop.ManageStudents" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Manage Students</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <uc:AdvisorNav runat="server" />
    <div class="advisor-card">
        <h3>Registered Students</h3>
        <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="false"
            CssClass="table-data" DataKeyNames="Id"
            OnRowEditing="gvStudents_RowEditing"
            OnRowCancelingEdit="gvStudents_RowCancelingEdit"
            OnRowUpdating="gvStudents_RowUpdating"
            OnRowDeleting="gvStudents_RowDeleting"
            OnRowDataBound="gvStudents_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="username" HeaderText="Username" ReadOnly="true" />
                <asp:BoundField DataField="firstName" HeaderText="First Name" />
                <asp:BoundField DataField="lastName" HeaderText="Last Name" />
                <asp:BoundField DataField="schoolName" HeaderText="School" />
                <asp:BoundField DataField="email" HeaderText="Email" />
                <asp:BoundField DataField="phoneNumber" HeaderText="Phone" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
            <EmptyDataTemplate>No students registered yet.</EmptyDataTemplate>
        </asp:GridView>
    </div>
</asp:Content>