<%@ Page Language="C#" AutoEventWireup="true" CodeFile="register.aspx.cs" Inherits="register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <title>Register</title>
    
    <link rel="stylesheet" href="login.css" />
    <link rel="stylesheet" href="main.css" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700" rel="stylesheet" />
    <link rel="stylesheet" href="register.css" />
    <script src="js/jquery.js"></script>
</head>
<body>

    

    <div class="navTitle">
        <img src="img/logoTest.png" class="logoImage"/>
    </div>

    <ul class="nav">
        <!--<li class="padding navsToHide" style="float:right"><a href="#about" onclick="modal('signupModal', true, '0.7s')">SIGN UP</a></li>-->
        <!--<li class="navsToHide" style="float:right"><a href="#" onclick="modal('loginModal', true, '0.7s')">LOGIN</a></li>-->
        <li class="padding navsToHide" style="float:right"><a href="register.aspx">SIGN UP</a></li>
        <li class="navsToHide" style="float:right"><a href="login.aspx">LOGIN</a></li>
        <li class="navsToHide" style="float:right"><a href="home.aspx">CONTACT</a></li>
        <li class="navsToHide" style="float:right"><a href="home.aspx">BENEFITS</a></li>
        <li class="navsToHide" style="float:right"><a href="home.aspx">FEATURES</a></li>
        <li class="navsToHide" style="float:right"><a href="home.aspx">ABOUT</a></li>
        <li class="navsToHide" style="float:right"><a href="home.aspx">HOME</a></li>
        <!--<li class="navsToHide" style="float:right"><a href="#" onclick="modal('employeeInfoModal', true, '0.7s'); checkDays()">TEST</a></li>-->
    </ul>

    <div style="float:right" class="responsiveIcon"><a href="#" onclick="responsiveIcon()">☰</a></div>

    <div class="responsiveNav">
        <ul class="nav-small">
            <li><a href="home.aspx">HOME</a></li>
            <li><a href="home.aspx#about">ABOUT</a></li>
            <li><a href="home.aspx">FEATURES</a></li>
            <li><a href="home.aspx">BENEFITS</a></li>
            <li><a href="home.aspx">CONTACT</a></li>
            <!--<li><a href="#" onclick="modal('loginModal', true, '0.7s')">LOGIN</a></li>
            <li><a href="#about" onclick="modal('signupModal', true, '0.7s')">SIGN UP</a></li>-->
            <li><a href="login.aspx">LOGIN</a></li>
            <!--<li><a href="register.aspx">SIGN UP</a></li>-->
        </ul>
    </div>

    <div class="entire-background">
        <div class="moveAll" style="background-color:transparent">
            <div class="heading" style="background-color:transparent">
                <h3>Register</h3>
            </div>
            
            <div class="row">
                <div class="cell cell-12 login-container">
                    <form name="login-form" class="login-form" runat="server">
                        <asp:DropDownList ID="cbo_registerAs" class="login-input email-input" runat="server" OnSelectedIndexChanged="cbo_registerAs_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem>Worker</asp:ListItem>
                            <asp:ListItem>Supervisor</asp:ListItem>
                            <asp:ListItem>Contractor</asp:ListItem>
                            <asp:ListItem>Monitor</asp:ListItem>
                        </asp:DropDownList>
                        <asp:TextBox ID="txt_firstName" class="login-input-half name-input" placeholder="First name" runat="server" CausesValidation="True" required="true"></asp:TextBox>
                        <asp:TextBox ID="txt_lastName" class="login-input-half name-input" placeholder="Last name" runat="server" CausesValidation="True" required="true"></asp:TextBox>
                        <asp:TextBox ID="txt_DOB" class="login-input password-input" placeholder="DOB" runat="server" TextMode="Date" required="true"></asp:TextBox>
                        <asp:TextBox ID="txt_email" class="login-input email-input" placeholder="Email" runat="server" TextMode="Email" ValidateRequestMode="Enabled" required="true"></asp:TextBox>
                        <asp:TextBox ID="txt_password" class="login-input password-input" placeholder="Password" runat="server" TextMode="Password" required="true"></asp:TextBox>
                        <asp:TextBox ID="txt_confirmPassword" class="login-input password-input" placeholder="Confirm Password" runat="server" TextMode="Password" required="true"></asp:TextBox>
                        <asp:TextBox ID="txt_passport" class="login-input password-input" placeholder="Passport Number" runat="server" required="true"></asp:TextBox>
                        <asp:TextBox ID="txt_code" class="login-input-half name-input" runat="server"></asp:TextBox>
                        <asp:Button ID="btn_submit" class="login-input submit-input" runat="server" Text="SUBMIT" OnClick="btn_submit_Click" />
                        <a href="Forgotpassword.aspx" style= "float:left" class="extra-actions">Forgot Password?</a>
                        <a href="login.aspx" style= "float:right" class="extra-actions">Login</a>
                    </form>
                </div>
            </div>
        </div>
    </div>
    
    <!-- The following script was added by Stephen on 26/10/2016 to validate that the two entered passwords should match -->
    <!-- If an ASP.NET alternative is prefered, a design will be required for a custom validation alert-->
    <script>
        var password = document.getElementById("txt_password"), confirm_password = document.getElementById("txt_confirmPassword");

        function validatePassword()
        {
            if (password.value != confirm_password.value)
            {
                confirm_password.setCustomValidity("Passwords Don't Match");
            }
            else
            {
                confirm_password.setCustomValidity('');
            }
        }

        password.onchange = validatePassword;
        confirm_password.onkeyup = validatePassword;
    </script>
</body>
</html>
