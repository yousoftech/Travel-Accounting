<%@ Page Title="" Language="C#" MasterPageFile="~/Monitor/MonitorMaster.master" AutoEventWireup="true" CodeFile="MonitorBudget.aspx.cs" Inherits="Monitor_MonitorBudget" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="cph_mainSection" ContentPlaceHolderID="cph_mainSection" runat="server">
    <style type="text/css">
        .table {
            margin-bottom: 0px;
        }
    </style>
    <section class="content">
        <div class="row">
            <div class="col-sm-4">
                <div class="box box-danger" style="min-height: 425px;">
                    <div class="box-header with-border" style="min-height: 50px;">
                        <h1 class="box-title" style="font-size: 24px;">Budgetting</h1>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-xs-6">
                                <label>Farm</label>
                                <asp:DropDownList ID="dl_Farm" runat="server" CssClass="form-control" ></asp:DropDownList>
                               <%-- <asp:SqlDataSource runat="server" ID="SqlDataSource_MonitorBudget_ddlfarm" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT [Farm_Name] FROM [tbl_farms] where GrowerID=@Monitorid">
                                    <SelectParameters>
                                        <asp:SessionParameter Name="Monitorid" SessionField="Id" ConvertEmptyStringToNull="true" />
                                    </SelectParameters>
                                </asp:SqlDataSource>--%>
                                <br />
                            </div>
                            <div class="col-xs-6">
                                <label>Contractor</label>
                                <asp:DropDownList ID="dl_Contractor" runat="server" CssClass="form-control" ></asp:DropDownList>
                               <%-- <asp:SqlDataSource runat="server" ID="SqlDataSource_MonitorBudget_ddlcontractor" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT CONCAT( [FirstName], [LastName] ) as FirstName FROM [tbl_grower]"></asp:SqlDataSource>--%>
                                <br />
                            </div>
                            <div class="col-xs-6">
                                <label>Main Category</label>
                                <asp:DropDownList ID="dl_Mcat" runat="server" CssClass="form-control" OnSelectedIndexChanged="dl_Mcat_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-xs-6">
                                <label>Sub Category</label>
                                <asp:DropDownList ID="dl_Cat" runat="server" CssClass="form-control" ></asp:DropDownList>
                               <%-- <asp:SqlDataSource runat="server" ID="SqlDataSource_MonitorBudget_subcat" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT [CatName] FROM [tbl_job_cat]"></asp:SqlDataSource>--%>
                                <br />
                            </div>
                            <div class="col-xs-6">
                                <label>Amount $</label>
                                <asp:TextBox ID="txt_amount" runat="server" CssClass="form-control"></asp:TextBox>
                                <br />
                            </div>
                            <div class="col-xs-6">
                                <label>Note</label>
                                <asp:TextBox ID="txt_note" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                                <br />
                            </div>
                            <div class="col-xs-6">
                                <br>
                            </div>
                            <div class="col-xs-6">
                                <br>
                            </div>
                            <div class="col-xs-12 text-center">
                                <br />
                                <asp:Button ID="btn_Submit" Style="min-width: 150px; margin-bottom: 5px;" class="btn btn-primary" runat="server" Text="Submit" OnClick="btn_Submit_Click" />
                                <asp:Button ID="btn_cancel" Style="min-width: 150px; margin-bottom: 5px;" class="btn btn-danger" runat="server" Text="Cancel" />
                            </div>
                        </div>
                    </div>
                    <!-- /.box-body -->
                </div>
            </div>
            <div class="col-sm-8">
                <div class="box">
                    <div class="box-header">
                        <h2 class="box-title">Table Title</h2>
                        <div class="box-tools">
                            <div class="input-group input-group-sm" style="width: 150px;">
                                <input type="text" name="table_search" class="form-control pull-right" placeholder="Search">
                                <div class="input-group-btn">
                                    <button type="submit" class="btn btn-default"><i class="fa fa-search"></i></button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body table-responsive no-padding">
                        <asp:GridView ID="gv_budget" runat="server" CssClass="table table-hover" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" PageSize="5">
                            <Columns>
                                <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                                <asp:BoundField DataField="Grower" HeaderText="Grower" SortExpression="Grower" />
                                <asp:BoundField DataField="Farm Name" HeaderText="Farm Name" SortExpression="Farm Name" />
                                <asp:BoundField DataField="Main Job Category" HeaderText="Main Job Category" SortExpression="Main Job Category" />
                                <asp:BoundField DataField="Job Subcategory" HeaderText="Job Subcategory" SortExpression="Job Subcategory" />
                                <asp:BoundField DataField="Notes" HeaderText="Notes" SortExpression="Notes" />
                                <asp:BoundField DataField="Timestamp" HeaderText="Timestamp" SortExpression="Timestamp" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT * FROM [tbl_Budget]"></asp:SqlDataSource>

                    </div>
                </div>
            </div>
        </div>
        <!-- /.row -->
    </section>
</asp:Content>
