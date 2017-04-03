<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SupervisorProfile.aspx.cs" Inherits="Supervisor_SupervisorProfile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Profile Settings</title>

    <link rel="stylesheet" href="../login.css" />
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700" rel="stylesheet" />

</head>
<body>

    <div class="background-image"></div>

    <div class="entire-wrap">
        <div class="heading">
            <h3>Profile Settings</h3>
        </div>

        <div class="row">
            <div class="cell cell-12 login-container">
                <form id="form1" class="forgotPasswordForm" runat="server">

    <div class="formThings">
    
        <h3>First Name</h3>
        <asp:TextBox ID="txt_firstName" class="profileSettingsTextBox" runat="server" CausesValidation="True"></asp:TextBox>
        
        <h3>Middle Name</h3>
        <asp:TextBox ID="txt_middleName" class="profileSettingsTextBox" runat="server" CausesValidation="True"></asp:TextBox>
        
        <h3>Last Name</h3>
        <asp:TextBox ID="txt_lastName" class="profileSettingsTextBox" runat="server" CausesValidation="True"></asp:TextBox>
        
        <h3>Profile Picture</h3>
        <div class="imageUploadTexts">
            <asp:FileUpload ID="fup_picture" runat="server" />
            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="You can only upload JPG files." ControlToValidate="fup_picture" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|png|PNG)$"></asp:RegularExpressionValidator>
        </div>

        <h3>Email</h3>
        <asp:TextBox ID="txt_email" class="profileSettingsTextBox" runat="server" CausesValidation="True" TextMode="Email"></asp:TextBox>
        
        <h3>Address</h3>
        <asp:TextBox ID="txt_address1" class="profileSettingsTextBox" runat="server" CausesValidation="True"></asp:TextBox>
        <br />
        <asp:TextBox ID="txt_address2" class="profileSettingsTextBox" runat="server" CausesValidation="True"></asp:TextBox>
        
        <h3>City</h3>
        <asp:TextBox ID="txt_city" class="profileSettingsTextBox" runat="server" CausesValidation="True"></asp:TextBox>
        
        <h3>Region</h3>
        <asp:TextBox ID="txt_region" class="profileSettingsTextBox" runat="server" CausesValidation="True"></asp:TextBox>
        <%--This VISA section is only for workers. Also, there should be an option for New Zealand born individuals, or immigrants that don't require a Visa (like Australians)
        <br />
        <br />
        <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Visa Details"></asp:Label>
        <br />
        Visa Number<br />
        <asp:TextBox ID="txt_visaNumber" runat="server" CausesValidation="True"></asp:TextBox>
        <br />
        Registration Date<br />
        <asp:TextBox ID="txt_visaStartDate" runat="server" CausesValidation="True" TextMode="Date"></asp:TextBox>
        <br />
        Expiry Date<br />
        <asp:TextBox ID="txt_visaEndDate" runat="server" CausesValidation="True" TextMode="Date"></asp:TextBox>
        <br />
        Type<br />
        <asp:TextBox ID="txt_visaType" runat="server" CausesValidation="True"></asp:TextBox> --%>

        
        <asp:Button ID="btn_submit" runat="server" OnClick="btn_submit_Click" Text="Submit"/>
        <asp:Button ID="btn_cancel" runat="server" OnClick="btn_cancel_Click" Text="Cancel" />

        <!--I am pretty sure Date of Birth should be a thing. Maybe even have it on the register screen as well.-->
    
    </div>
    </form>
            </div>
        </div>
    </div>

</body>
</html>
