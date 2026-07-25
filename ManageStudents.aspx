<%@ Register TagPrefix="uc" TagName="AdvisorNav" Src="~/AdvisorNav.ascx" %>
<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="ManageStudents.aspx.cs" Inherits="PitStop.ManageStudents" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Manage Students</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <uc:AdvisorNav runat="server" />

    <div class="advisor-card">
        <asp:Label ID="lblMessage" runat="server" CssClass="feedback-msg"></asp:Label>

        <h3>Registered Students</h3>
        <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="false"
            CssClass="table-data" DataKeyNames="Id"
            OnRowEditing="gvStudents_RowEditing"
            OnRowCancelingEdit="gvStudents_RowCancelingEdit"
            OnRowUpdating="gvStudents_RowUpdating"
            OnRowDeleting="gvStudents_RowDeleting"
            OnRowDataBound="gvStudents_RowDataBound">
            <Columns>
                <asp:BoundField DataField="Id"          HeaderText="ID"         ReadOnly="true" />
                <asp:BoundField DataField="username"    HeaderText="Username"   ReadOnly="true" />
                <asp:BoundField DataField="firstName"   HeaderText="First Name" />
                <asp:BoundField DataField="lastName"    HeaderText="Last Name" />
                <asp:BoundField DataField="schoolName"  HeaderText="School" />
                <asp:BoundField DataField="email"       HeaderText="Email" />
                <asp:BoundField DataField="phoneNumber" HeaderText="Phone" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" DeleteText="Deactivate" />
            </Columns>
            <EmptyDataTemplate>No active students.</EmptyDataTemplate>
        </asp:GridView>
    </div>

    <%-- Only rendered when there is at least one deactivated student --%>
    <asp:Panel ID="pnlDeactivated" runat="server" Visible="false">
        <div class="advisor-card">
            <h3>Deactivated Students</h3>
            <p class="advisor-note">These students have been deactivated. Their tasks and XP records are still intact.</p>
            <asp:GridView ID="gvDeactivated" runat="server" AutoGenerateColumns="false"
                CssClass="table-data" DataKeyNames="Id"
                OnRowCommand="gvDeactivated_RowCommand">
                <Columns>
                    <asp:BoundField DataField="Id"          HeaderText="ID"         ReadOnly="true" />
                    <asp:BoundField DataField="username"    HeaderText="Username"   ReadOnly="true" />
                    <asp:BoundField DataField="firstName"   HeaderText="First Name" ReadOnly="true" />
                    <asp:BoundField DataField="lastName"    HeaderText="Last Name"  ReadOnly="true" />
                    <asp:BoundField DataField="schoolName"  HeaderText="School"     ReadOnly="true" />
                    <asp:BoundField DataField="email"       HeaderText="Email"      ReadOnly="true" />
                    <asp:BoundField DataField="phoneNumber" HeaderText="Phone"      ReadOnly="true" />
                    <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton runat="server"
                                CommandName="Restore"
                                CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn-restore"
                                OnClientClick="return confirm('Restore this student to active status?');">
                                Restore
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>No deactivated students.</EmptyDataTemplate>
            </asp:GridView>
        </div>
    </asp:Panel>

</asp:Content>
