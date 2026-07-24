<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="AdvisorDashboard.aspx.cs" Inherits="PitStop.AdvisorDashboard" EnableEventValidation="false" %>

<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Advisor Dashboard</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <div>
        <h3>Add New Task</h3>
        <asp:HiddenField ID="hfSelectedTaskId" runat="server" />
        <table>
            <tr>
                <td>Student ID:</td>
                <td>
                    <asp:TextBox ID="txtStudentId" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvStudentId" runat="server" 
                        ControlToValidate="txtStudentId" ErrorMessage="Student ID is required" 
                        ForeColor="Red" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Title:</td>
                <td>
                    <asp:TextBox ID="txtTitle" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvTitle" runat="server" 
                        ControlToValidate="txtTitle" ErrorMessage="Title is required" 
                        ForeColor="Red" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>Description:</td>
                <td>
                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="3"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>XP Reward:</td>
                <td>
                    <asp:TextBox ID="txtXPReward" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvXP" runat="server" 
                        ControlToValidate="txtXPReward" ErrorMessage="XP Reward is required" 
                        ForeColor="Red" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvXP" runat="server" 
                        ControlToValidate="txtXPReward" MinimumValue="1" MaximumValue="1000" 
                        Type="Integer" ErrorMessage="XP must be between 1-1000" 
                        ForeColor="Red" Display="Dynamic">
                    </asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td>Due Date:</td>
                <td>
                    <asp:TextBox ID="txtDueDate" runat="server" TextMode="Date"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvDueDate" runat="server" 
                        ControlToValidate="txtDueDate" ErrorMessage="Due date is required" 
                        ForeColor="Red" Display="Dynamic">
                    </asp:RequiredFieldValidator>
                </td>
            </tr>
        </table>
        <asp:Button ID="btnAddTask" runat="server" Text="Add Task" OnClick="btnAddTask_Click"/>
        <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="btnApprove_Click"/>
        <br /><br />

        <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="false"
            CssClass="table-data" DataKeyNames="TaskId"
            OnRowEditing="gvTasks_RowEditing"
            OnRowCancelingEdit="gvTasks_RowCancelingEdit"
            OnRowUpdating="gvTasks_RowUpdating" 
            OnRowDeleting="gvTasks_RowDeleting"
            OnRowDataBound="gvTasks_RowDataBound"
            OnSelectedIndexChanged="gvTasks_SelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="TaskId" HeaderText="Task ID" ReadOnly="true" />
                <asp:BoundField DataField="title" HeaderText="Title" />
                <asp:BoundField DataField="description" HeaderText="Description" />
                <asp:BoundField DataField="xpReward" HeaderText="XP Reward" />
                <asp:BoundField DataField="dueDate" HeaderText="Due Date" DataFormatString="{0:d}" />
                <asp:BoundField DataField="status" HeaderText="Status" />
                <asp:BoundField DataField="StudentName" HeaderText="Student" ReadOnly="true" />
                <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>