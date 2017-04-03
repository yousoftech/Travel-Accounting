<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Login</title>
    <link href="bootstrap/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="dist/css/style.min.css" rel="stylesheet" type="text/css" />
    <link href="plugins/iCheck/square/blue.css" rel="stylesheet" type="text/css" />
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
        <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
        <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
    <![endif]-->
    <link href="https://fonts.googleapis.com/css?family=Roboto:100,300,400,500,700" rel="stylesheet" />
    <style type="text/css">
        .login-logo, .register-logo {
            font-size: 35px;
            text-align: center;
            margin-bottom: 0 !important;
            font-weight: 300;
        }
    </style>
</head>
<body class="login-page">
    <div class="login-box">
        <div class="login-logo">
            <a href="">
                <img src="img/logoTest.png" class="center-block img-responsive" style="max-height: 80px;" /></a>
        </div>
        <!-- /.login-logo -->
        <div class="login-box-body">
            <p class="login-box-msg">Sign in to start your session</p>
            <form name="login-form" class="login-form" runat="server" style="background-color: transparent">
                <div class="form-group has-feedback">
                    <asp:DropDownList ID="cbo_loginAs" class="form-control" runat="server">
                        <asp:ListItem>Worker</asp:ListItem>
                        <asp:ListItem>Contractor</asp:ListItem>
                        <asp:ListItem>Supervisor</asp:ListItem>
                        <asp:ListItem>Monitor</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group has-feedback">
                    <asp:TextBox ID="txt_email" class="form-control" placeholder="Email" runat="server" ValidateRequestMode="Enabled" required="true"></asp:TextBox>
                    <span class="glyphicon glyphicon-envelope form-control-feedback"></span>
                </div>
                <div class="form-group has-feedback">
                    <asp:TextBox ID="txt_password" class="form-control" placeholder="Password" runat="server" TextMode="Password" required="true"></asp:TextBox>
                    <span class="glyphicon glyphicon-lock form-control-feedback"></span>
                </div>
                <div class="row">
                    <div class="col-xs-8">
                        <div class="checkbox icheck">
                            <label>
                                <input type="checkbox">
                                Remember Me
                            </label>
                        </div>
                    </div>
                    <!-- /.col -->
                    <div class="col-xs-4">
                        <asp:Button ID="btn_submit" class="btn btn-primary btn-block btn-flat" runat="server" Text="Sign In" OnClick="btn_submit_Click" />
                    </div>
                    <!-- /.col -->
                </div>
            </form>
            <!-- /.social-auth-links -->
            <a href="Forgotpassword.aspx">I forgot my password</a><br>
            <a href="register.aspx" class="text-center">Register a new membership</a>

        </div>
        <!-- /.login-box-body -->
    </div>






    <script src="plugins/jQuery/jQuery-2.1.4.min.js" type="text/javascript"></script>
    <script src="bootstrap/js/bootstrap.min.js" type="text/javascript"></script>
    <script src="plugins/iCheck/icheck.min.js" type="text/javascript"></script>
    <script>
        $(function () {
            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '20%' // optional
            });
        });
    </script>
</body>
</html>
