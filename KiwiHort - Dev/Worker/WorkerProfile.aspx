<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Worker/WorkerMaster.master" CodeFile="WorkerProfile.aspx.cs" Inherits="Worker_WorkerProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" runat="Server">
    <style type="text/css">
        .box-body input {
            margin-bottom: 15px;
        }

        .fileContainer {
            overflow: hidden;
            position: relative;
        }

            .fileContainer [type=file] {
                cursor: inherit;
                display: block;
                filter: alpha(opacity=0);
                min-height: 100%;
                min-width: 100%;
                opacity: 0;
                position: absolute;
                right: 0;
                text-align: right;
                top: 0;
            }


        .fileContainer {
            float: left;
            background: -webkit-linear-gradient(top, #f9f9f9, #e3e3e3);
            border: 1px solid #999;
            border-radius: 3px;
            padding: 5px 8px;
            outline: none;
            white-space: nowrap;
            -webkit-user-select: none;
            cursor: pointer;
            text-shadow: 1px 1px #fff;
            font-weight: 700;
            font-size: 10pt;
            margin-bottom: 18px;
        }

            .fileContainer [type=file] {
                cursor: pointer;
            }

        }
    </style>
    <section class="content">
        <div class="row">
            <div class="col-sm-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Profile Setting</h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>First Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_firstName" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Middle Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_middleName" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Last Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_lastName" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Email</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_email" class="form-control" runat="server" CausesValidation="True" TextMode="Email"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Address 1</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_address1" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Address 2</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_address2" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>City</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_city" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Region</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_region" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Post Code</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_postcode" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-5 col-sm-2">
                                        <label>Profile Picture</label>
                                    </div>
                                    <div class="col-xs-5 col-sm-3">
                                        <label class="fileContainer">
                                            Select File
    <asp:FileUpload ID="fup_picture" runat="server" />
                                        </label>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="You can only upload JPG files." ControlToValidate="fup_picture" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|png|PNG)$"></asp:RegularExpressionValidator>
                                    </div>
                                        <label class="fileContainer">
                                            Select File
    <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </label>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="You can only upload JPG files." ControlToValidate="FileUpload1" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|png|PNG)$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-xs-12 col-sm-3">
                                        <asp:Button ID="btn_submit" runat="server" class="btn btn-success" OnClick="btn_submit_Click" Text="Submit" />
                                        <asp:Button ID="btn_cancel" class="btn btn-danger" runat="server" OnClick="btn_cancel_Click" Text="Return without Saving" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
</asp:Content>
