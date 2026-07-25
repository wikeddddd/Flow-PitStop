<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="EditLeaderboard.aspx.cs" Inherits="PitStop.EditLeaderboard" %>


<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Driver Standings</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <div class="dashboard-container">


    
    <asp:Panel ID="pnlNav" runat="server">
        <nav class="sidebar-nav">
            <asp:LinkButton ID="lnkTasks" runat="server" PostBackUrl="~/StudentDashboard.aspx">Dashboard</asp:LinkButton>
        </nav>
    </asp:Panel>
        <main class="content-workspace">
            <h1 class="section-headline">Driver Standings</h1>
            <div class="dashboard-card">
                <asp:GridView ID="gvLeaderboard" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="grid-view"
                    OnRowDataBound="gvLeaderboard_RowDataBound"
                    OnRowEditing="gvLeaderboard_RowEditing"
                    OnRowUpdating="gvLeaderboard_RowUpdating"
                    OnRowCancelingEdit="gvLeaderboard_RowCancelingEdit"
                    OnRowDeleting="gvLeaderboard_RowDeleting">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id"/>
                        <asp:TemplateField HeaderText="FirstName" SortExpression="FirstName">
                            <ItemTemplate><asp:Label ID="lblFirstName" runat="server" Text='<%# Eval("FirstName") %>'></asp:Label></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtFirstName" runat="server" Text='<%# Bind("FirstName") %>'></asp:TextBox></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SchoolName" SortExpression="SchoolName">
                            <ItemTemplate><asp:Label ID="lblSchoolName" runat="server" Text='<%# Eval("SchoolName") %>'></asp:Label></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtSchoolName" runat="server" Text='<%# Bind("SchoolName") %>'></asp:TextBox></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TotalXp" SortExpression="TotalXp">
                            <ItemTemplate><asp:Label ID="lblTotalXp" runat="server" Text='<%# Eval("TotalXp") %>'></asp:Label></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtTotalXp" runat="server" Text='<%# Bind("TotalXp") %>'></asp:TextBox></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CurrentLevel" SortExpression="CurrentLevel">
                            <ItemTemplate><asp:Label ID="lblCurrentLevel" runat="server" Text='<%# Eval("CurrentLevel") %>'></asp:Label></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtCurrentLevel" runat="server" Text='<%# Bind("CurrentLevel") %>'></asp:TextBox></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="DailyStreak" SortExpression="DailyStreak">
                            <ItemTemplate><asp:Label ID="lblDailyStreak" runat="server" Text='<%# Eval("DailyStreak") %>'></asp:Label></ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtDailyStreak" runat="server" Text='<%# Bind("DailyStreak") %>'></asp:TextBox></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>

            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Student ID"></asp:Label>
                <asp:DropDownList ID="ddStudentID" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddStudentID_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div>
                <asp:Label ID="Label2" runat="server" Text="School Name"></asp:Label>
                <asp:DropDownList ID="ddSchoolName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddSchoolName_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div>

                <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>

            </div>
   
</asp:Content>
