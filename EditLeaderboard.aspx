<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="EditLeaderboard.aspx.cs" Inherits="PitStop.EditLeaderboard" %>


<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Driver Standings</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
    <div class="dashboard-container">


    
        <nav class="sidebar-nav">
            <asp:LinkButton ID="lnkTasks" runat="server" PostBackUrl="~/StudentDashboard.aspx">Dashboard</asp:LinkButton>
        </nav>
     </div>
        <main class="content-workspace">
            <h1 class="section-headline">Driver Standings</h1>
            <div class="dashboard-card">
                <asp:GridView ID="gvLeaderboard" runat="server" AutoGenerateColumns="False" DataKeyNames="Id" CssClass="grid-view" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="gvLeaderboard_SelectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" InsertVisible="False" ReadOnly="True" SortExpression="Id"/>
                        <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName" />
                        <asp:BoundField DataField="SchoolName" HeaderText="SchoolName" ItemStyle-Font-Bold="true" SortExpression="SchoolName"/>
                        <asp:BoundField DataField="TotalXp" HeaderText="TotalXp" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" ItemStyle-Font-Bold="true" SortExpression="TotalXp" />
                        <asp:BoundField DataField="CurrentLevel" HeaderText="CurrentLevel" SortExpression="CurrentLevel" />
                        <asp:BoundField DataField="DailyStreak" HeaderText="DailyStreak" SortExpression="DailyStreak" />
                        <asp:CommandField ShowEditButton="true" ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:connectionString %>" 
                    SelectCommand="SELECT u.Id, u.FirstName, u.SchoolName, g.TotalXp, g.CurrentLevel, g.DailyStreak 
                                 FROM Gamification g
                                 INNER JOIN Students u ON g.Id = u.Id
                                 ORDER BY g.TotalXp DESC, g.DailyStreak DESC"
                    UpdateCommand="UPDATE Students SET FirstName = @FirstName, SchoolName = @SchoolName WHERE Id = @Id; UPDATE Gamification SET TotalXp = @TotalXp, CurrentLevel = @CurrentLevel, DailyStreak = @DailyStreak WHERE Id = @Id;"
    
                    DeleteCommand="DELETE FROM Gamification WHERE Id = @Id; DELETE FROM Students WHERE Id = @Id;"></asp:SqlDataSource>
            </div>
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Student ID"></asp:Label>
                <asp:DropDownList ID="ddStudentID" runat="server" OnSelectedIndexChanged="ddStudentID_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div>
                <asp:Label ID="Label2" runat="server" Text="School Name"></asp:Label>
                <asp:DropDownList ID="ddSchoolName" runat="server" OnSelectedIndexChanged="ddSchoolName_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
            <div>

                <asp:Label ID="lblError" runat="server" ForeColor="Red" Text="Label" Visible="False"></asp:Label>

            </div>
   
</asp:Content>
