<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="PitWallTasks.aspx.cs" Inherits="PitStop.PitWallTasks" %>



<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Pit Wall Tasks</title>
</asp:Content>


<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">

        <header class="dashboard-header">
            <h1>Pit Wall Tasks</h1>
        </header>

        <nav class="sidebar-nav">

            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/StudentDashboard.aspx">Dashboard</asp:LinkButton>
            <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/StudentLeaderboard.aspx">Leaderboard</asp:LinkButton>
            <asp:LinkButton ID="LinkButton3" runat="server" PostBackUrl="~/ManageUserProfile.aspx">UserProfile</asp:LinkButton>

        </nav>

        <main class="content-workspace">
            <div class="grid-layout">
                <div class="task-card">
                    <h3>File Submission Terminal</h3>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                    <div class="submission-panel">
                        
                        <asp:Label ID="Label3" runat="server" Text="Select Active Pending Assignment: "></asp:Label>
                        <asp:DropDownList ID="ddlPendingTasks" runat="server" DataTextField="title" DataValueField="TaskId"></asp:DropDownList>
                        
                        
                        <asp:Label ID="Label1" runat="server" Text="Upload Document: "></asp:Label>
                        <asp:FileUpload ID="fileTaskUpload" runat="server"/>
                        <asp:RequiredFieldValidator ID="validateFileUpload" runat="server" ErrorMessage="File upload is required" ControlToValidate="fileTaskUpload"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="reValidateFileUpload" runat="server" ErrorMessage="Please upload a valid document file (PDF, DOC, DOCX)" ValidationExpression="\.pdf|\.doc|\.docx$" ControlToValidate="fileTaskUpload"></asp:RegularExpressionValidator>
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CausesValidation="false"/>
                        <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
                    </div>
                </div>
                <div class="task-card">
                    <h3>Complete Duty Log</h3>
                    <asp:GridView ID="gvTasks" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                        <Columns>
                            <asp:BoundField DataField="title" HeaderText="title" SortExpression="title" />
                            <asp:BoundField DataField="description" HeaderText="description" SortExpression="description" />
                            <asp:BoundField DataField="xpReward" HeaderText="xpReward" SortExpression="xpReward" />
                            <asp:BoundField DataField="status" HeaderText="status" SortExpression="status" />
                            <asp:BoundField DataField="dueDate" HeaderText="dueDate" SortExpression="dueDate" />
                            <asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        
        </main>

</asp:Content>