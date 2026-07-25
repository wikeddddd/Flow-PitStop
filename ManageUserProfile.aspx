

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="ManageUserProfile.aspx.cs" Inherits="PitStop.ManageUserProfile" %>



<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Manage User Profile</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
        <div class="profile-card">
            <h2>Manage Profile</h2>
            <asp:Image ID="imgAvatar" runat="server" />
            <asp:Panel ID="pnlNav" runat="server">
                <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/StudentDashboard.aspx">Dashboard</asp:LinkButton>
                <asp:LinkButton ID="LinkButton2" runat="server" PostBackUrl="~/PitWallTasks.aspx">Pit Wall Tasks</asp:LinkButton>
                <asp:LinkButton ID="lbLeaderboard" runat="server" PostBackUrl="~/StudentLeaderboard.aspx">Leaderboard</asp:LinkButton>
            </asp:Panel>
            <asp:ValidationSummary ID="vsProfile" runat="server" ValidationGroup="Profile" HeaderText="Please correct these errors" />
        </div>
        <div class="form-group">
            <label>Update Profile Picture</label>
            <asp:FileUpload ID="fileUploadAvatar" runat="server" />
            <asp:RegularExpressionValidator 
    ID="validateAvatar" 
    runat="server" 
    ErrorMessage="Please upload a valid image file (JPEG, PNG, GIF)" 
    ValidationExpression="^.*(?i:\.jpe?g|\.png|\.gif)$" 
    ControlToValidate="fileUploadAvatar" 
    ValidationGroup="Profile">
</asp:RegularExpressionValidator>

        </div>
         <div class="form-group">
            <label>Username:</label>
            <asp:TextBox ID="TBUsername" runat="server"></asp:TextBox>
             <asp:RequiredFieldValidator ID="validateUsername" runat="server" ErrorMessage="Username is required" ControlToValidate="TBUsername" ValidationGroup="Profile"></asp:RequiredFieldValidator>
        </div>
         <div class="form-group">
             <label>Password:</label>
            <asp:TextBox ID="TBPassword" runat="server" TextMode="Password"></asp:TextBox>
             <asp:RequiredFieldValidator ID="validatePassword" runat="server" ErrorMessage="Password is required" ControlToValidate="TBPassword" ValidationGroup="Profile"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <label>First Name:
            </label>
            <asp:TextBox ID="TBFirstName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="validateFirstName" runat="server" ErrorMessage="First name is required" ControlToValidate="TBFirstName" ValidationGroup="Profile"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <label>Last Name:</label>
            <asp:TextBox ID="TBLastName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="validateLastName" runat="server" ErrorMessage="Last name is required" ControlToValidate="TBLastName" ValidationGroup="Profile"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <label>Email Address:
            </label>
            <asp:TextBox ID="TBEmailAddress" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="validateEmailAddress" runat="server" ErrorMessage="Email address is required" ControlToValidate="TBEmailAddress" ValidationGroup="Profile"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
            <label>Phone Number:
            </label>
            <asp:TextBox ID="TBPhoneNum" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="validatePhoneNum" runat="server" ErrorMessage="Phone number is required" ControlToValidate="TBPhoneNum" ValidationGroup="Profile"></asp:RequiredFieldValidator>
        </div>
        <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
        <asp:Button ID="btnSaveProfile" runat="server" OnClick="btnSaveProfile_Click" Text="Button" ValidationGroup="Profile" />


</asp:Content>