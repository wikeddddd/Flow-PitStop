

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MemberSection.Master" CodeBehind="EditUser.aspx.cs" Inherits="PitStop.EditUser" %>



<asp:Content ID="ContentTitle" ContentPlaceHolderID="TitleContent" runat="server">
    <title>Manage User Profile</title>
</asp:Content>

<asp:Content ID="ContentMain" ContentPlaceHolderID="MemberMainContent" runat="server">
        <div class="profile-card">
            <h2>Edit User</h2>
            <asp:Image ID="imgAvatar" runat="server" CssClass="profile-avatar" />
            <asp:LinkButton ID="lbDashboard" runat="server" PostBackUrl="~/AdminDashboard.aspx" Visible="false">Dashboard</asp:LinkButton>
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

             <br />

        </div>
        <div class="form-group">
            <asp:Label ID="Label1" runat="server" Text="Choose a User :"></asp:Label>

             <asp:DropDownList ID="ddUser" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddUser_SelectedIndexChanged1">
            </asp:DropDownList>
        </div>
         <div class="form-group">
            <label>Username:<asp:TextBox ID="TBUsername" runat="server"></asp:TextBox>
             </label>
             &nbsp;<asp:RequiredFieldValidator ID="validateUsername" runat="server" ErrorMessage="Username is required" ControlToValidate="TBUsername" ValidationGroup="Profile"></asp:RequiredFieldValidator>
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
        <div class="form-group"> 
            <asp:Label ID="Label2" runat="server" Text="Role"></asp:Label>
            <asp:DropDownList ID="ddRole" runat="server">
                <asp:ListItem Value="Admin">Admin</asp:ListItem>
                <asp:ListItem Value="Advisor">Advisor</asp:ListItem>
                <asp:ListItem Value="Student">Student</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            <asp:Label ID="lblStatus" runat="server" Text="Label"></asp:Label>
        
        </div>
        <div class="form-group">
            <asp:Button ID="btnSaveProfile" runat="server" OnClick="btnSaveProfile_Click" Text="Save" ValidationGroup="Profile" />


            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" Text="Delete" />
        </div>



</asp:Content>