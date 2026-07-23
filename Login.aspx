<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PitStop.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Login
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
        <div class="auth-card">
            <h1 class="auth-title">Welcome Back</h1>
            <p class="auth-subtitle">Log in to your PitStop account</p>

            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-input" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="Email is required"
                    CssClass="form-error"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <div class="form-group">
                <label for="txtPassword">Password</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-input" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ErrorMessage="Password is required"
                    CssClass="form-error"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

            <!-- TODO: [Teammate handling auth] - wire up Click event to validate credentials, set Session["UserRole"], redirect to Student/Dashboard.aspx or Admin/Dashboard.aspx -->
            <asp:Button ID="btnLogin" runat="server" Text="Log In" CssClass="btn-primary" OnClick="btnLogin_Click" />

            <asp:Label ID="lblLoginError" runat="server" CssClass="form-error" Visible="false"></asp:Label>

            <p class="auth-footer">Don't have an account? 
                <asp:LinkButton ID="lbCreateUser" runat="server" OnClick="lbCreateUser_Click">Register Here</asp:LinkButton>
            </p>
        </div>
    </div>
</asp:Content>
