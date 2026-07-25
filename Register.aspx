<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="PitStop.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Regsiter
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="auth-container">
        <div class="auth-card">
            <h1 class="auth-title">Create Account</h1>
            <p class="auth-subtitle" id="subCreateAccount">Join PitStop and start tracking progress</p>

            <div class="form-group">
                <label for="txtUsername">Username</label>
                <asp:TextBox ID="txtUsername" runat="server" CssClass="form-input" OnTextChanged="txtUsername_TextChanged"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvUsername" runat="server"
                    ControlToValidate="txtUsername"
                    ErrorMessage="Username is required"
                    CssClass="form-error"
                    Display="Dynamic">
                </asp:RequiredFieldValidator>
            </div>

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

            <div class="form-group">
                <label for="txtConfirmPassword">Confirm Password</label>
                <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-input" TextMode="Password"></asp:TextBox>
                <asp:CompareValidator ID="cvPassword" runat="server"
                    ControlToValidate="txtConfirmPassword"
                    ControlToCompare="txtPassword"
                    ErrorMessage="Passwords do not match"
                    CssClass="form-error"
                    Display="Dynamic">
                </asp:CompareValidator>
            </div>

            <div class="form-group">
                <label for="ddlRole">I am a...</label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-input" OnSelectedIndexChanged="ddlRole_SelectedIndexChanged">
                    <asp:ListItem Text="Student" Value="Student"></asp:ListItem>
                    <asp:ListItem Text="Advisor" Value="Advisor"></asp:ListItem>
                </asp:DropDownList>
            </div>

            <!-- TODO: [Teammate handling auth] - wire up Click event to insert new user into Users table (hash password!), then redirect to Login.aspx -->
            <asp:Button ID="btnRegister" runat="server" Text="Create Account" CssClass="btn-primary" OnClick="btnRegister_Click" />

            <asp:Label ID="lblRegisterError" runat="server" CssClass="form-error" Visible="false"></asp:Label>

            <p class="auth-footer">Already have an account? <a href="~/Login.aspx" runat="server">
                <asp:LinkButton ID="lbLogin" runat="server" OnClick="lbLogin_Click" CausesValidation="False">Log In</asp:LinkButton>
                </a></p>
        </div>
    </div>
</asp:Content>

