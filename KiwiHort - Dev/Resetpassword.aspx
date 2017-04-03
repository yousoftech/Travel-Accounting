<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Resetpassword.aspx.cs" Inherits="Resetpassword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Reset Password</title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700" rel="stylesheet" />
    <link rel="stylesheet" href="login.css" />
</head>
<body>

    <div class="background-image"></div>
    
    <div class="entire-wrap">
        <div class="heading">
            <h3>Reset Password</h3>
        </div>

        <div class="row">
            <div class="cell cell-12 login-container">
                <form id="form1" class="resetPasswordForm" runat="server">
                    <asp:TextBox ID="txtpwd" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>    
                    <asp:TextBox ID="txtcofrmpwd" runat="server" placeholder="Confirm password" TextMode="Password"></asp:TextBox>
                    
                    <asp:Button ID="btnsubmit" runat="server" Text="SUBMIT" OnClick="btnsubmit_Click" />

                    <asp:CompareValidator ID="CompareValidatorPassword" runat="server" ControlToCompare="txtpwd" ControlToValidate="txtcofrmpwd" ErrorMessage="Password does not match"></asp:CompareValidator>
                </form>
            </div>
        </div>
    </div>

</body>
</html>
