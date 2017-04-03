<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Activation.aspx.cs" Inherits="Activation" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta http-equiv="X-UA-Compatible" content="IE=edge"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <title></title>
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700" rel="stylesheet" />
    <link rel="stylesheet" href="404.css" />
</head>
<body>
    

   <div class="error">
        <div class="error-body">

            <!--<img src="img/sadKiwi.png" class="sad-kiwi" />-->
            <h4 id="activationMessage" class="error-subheading" ><asp:Literal ID="ltMessage" runat="server" /></h4>

            <form id="form1" runat="server">
                    <div class="activationMessageContainer">
                        
                    </div>
                </form>

            <p>
                <small><asp:Literal ID="ltSubmessage" runat="server" /></small>
            </p>

            <div class="seperator-with-text">
                <span class="seperator-text">
                    OR
                </span>
            </div>

            <p>
                <a href="Home.aspx" class="return-button">Return to homepage</a>
            </p>

        </div>
        <div class="error-footer">
            <p>
                <small>© Kiwihort 2016</small>
            </p>
        </div>
    </div>

    <script>
			function goBack()
			{
			    window.location.href = "login.aspx";
				//window.history.back();
			}
    </script>




</body>
</html>
