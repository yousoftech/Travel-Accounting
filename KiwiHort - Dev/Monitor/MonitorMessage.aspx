<%@ Page Title="" Language="C#" MasterPageFile="~/Monitor/MonitorMaster.master" AutoEventWireup="true" CodeFile="MonitorMessage.aspx.cs" Inherits="Monitor_MonitorMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" runat="Server">
    <style type="text/css">
        @media (max-width:768px) {

            table.table.table-hover tr td {
                width: inherit;
            }
        }

        table.table.table-hover tr td {
            width: 25% !important;
            word-wrap: break-word;
            word-break: break-all;
        }
    </style>
    <div class="content">
        <div class="row">
            <div class="col-sm-4">
                <asp:LinkButton ID="Compose" runat="server" CssClass="btn btn-block btn-success" OnClick="Button1_Click"><i class="ion ion-ios-navigate-outline"></i>&nbsp;&nbsp;Compose</asp:LinkButton>
            </div>
            <div class="col-sm-4">
                <asp:LinkButton ID="Button2" CssClass="btn btn-block btn-warning" runat="server" OnClick="Button2_Click"><i class="fa fa-inbox"></i>&nbsp;&nbsp;Inbox</asp:LinkButton>
            </div>
            <div class="col-sm-4">
                <asp:LinkButton ID="Button3" CssClass="btn btn-block btn-danger" runat="server" OnClick="Button3_Click"><i class="fa fa-envelope-o"></i>&nbsp;&nbsp;Sent</asp:LinkButton>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-12">
                <asp:MultiView ID="MultiView1" runat="server">
                    <asp:View ID="Tab2" runat="server">
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Compose Message</h3>
                            </div>
                            <div class="box-body">
                                <div class="box-body">
                                    <div class="form-group">
                                        <asp:TextBox ID="txtto" runat="server" TextMode="Email" CssClass="form-control" placeholder="To"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtsubject" runat="server" CssClass="form-control" placeholder="Add a subject"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <asp:TextBox ID="txtbody" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="Add a message"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer">
                                <div class="pull-right">
                                    <asp:Button ID="Button4" Text="Send" runat="server" OnClick="Button4_Click" type="submit" class="btn btn-primary"></asp:Button>

                                </div>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="View1" runat="server">
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Inbox</h3>
                            </div>
                            <div class="box-body table-responsive no-padding">
                                <asp:GridView ID="GridView1" CssClass="table table-hover" runat="server" DataSourceID="SqlDataSource1"></asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                    <asp:View ID="View2" runat="server">
                        <div class="box">
                            <div class="box-header">
                                <h3 class="box-title">Sent</h3>
                            </div>
                            <div class="box-body table-responsive no-padding">
                                <asp:GridView ID="GridView2" CssClass="table table-hover" runat="server" DataSourceID="SqlDataSource2"></asp:GridView>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" ProviderName="<%$ConnectionStrings:KiwihortData.ProviderName %>"></asp:SqlDataSource>
                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" ProviderName="<%$ ConnectionStrings:KiwihortData.ProviderName %>"></asp:SqlDataSource>
            </div>
        </div>
    </div>
</asp:Content>
