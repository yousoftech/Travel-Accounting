<%@ Page Title="" Language="C#" MasterPageFile="~/Contractor/ContractorMaster.master" AutoEventWireup="true" CodeFile="ContractorReport.aspx.cs" Inherits="Contractor_ContractorReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" runat="Server">
    <div class="content">
        <div class="row">
            <div class="col-sm-12">
                <div class="box">
                    <div class="box-header">
                        <div class="row">
                            <div class="col-sm-3">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="">Search Filter</label>
                                        <asp:DropDownList ID="cbo_day" class="dropdown dropdown-day" CssClass="form-control" runat="server" OnSelectedIndexChanged="cbo_day_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="cbo_week" class="dropdown dropdown-week" CssClass="form-control" runat="server" OnSelectedIndexChanged="cbo_week_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="cbo_farm" class="dropdown dropdown-farm" CssClass="form-control" runat="server"></asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <asp:CheckBox ID="chkphase" runat="server" Text="Phase Report" />

                                <br />
                            </div>
                            <div class="col-sm-3">
                                <asp:Button ID="btn_submit" class="btn btn-primary pull-left" runat="server" Text="Get Report" OnClick="btn_submit_Click" />

                                <asp:Button ID="btn_excel" class="btn btn-primary pull-right" runat="server" Text="Download Excel File" OnClick="btn_excel_Click" />

                            </div>
                        </div>
                    </div>
                    <div class="box-body table-responsive no-padding">
                        <div>
                            <asp:GridView CssClass="table table-hover" ID="dgd_workers" runat="server" DataSourceID="SqlDataSource1"></asp:GridView>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand=""></asp:SqlDataSource>
                        </div>
                    </div>
                    <div class="box-footer clearfix">
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
