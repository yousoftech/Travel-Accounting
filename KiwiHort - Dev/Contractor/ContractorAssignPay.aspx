<%@ Page Title="" Language="C#" MasterPageFile="~/Contractor/ContractorMaster.master" AutoEventWireup="true" CodeFile="ContractorAssignPay.aspx.cs" Inherits="Contractor_ContractorAssignPay" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" runat="Server">
    <div class="content">
        <div class="row">
            <div class="col-lg-12">
                <div class="box">
                    <div class="box-header">
                        <div class="row">
                            <div class="col-lg-2">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="">Title:</label>
                                        <asp:DropDownList ID="cbo_worker" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbo_worker_SelectedIndexChanged" Style="max-width: 94%;" CssClass="form-control"></asp:DropDownList>
                                        <br />
                                    </div>
                                </div>
                            </div>
                            <div class="col-lg-3">
                                <div class="form-inline">
                                    <div class="form-group">
                                        <label for="">Price $</label>
                                        <asp:TextBox ID="txt_pay"
                                            CssClass="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-2">
                                <asp:Button ID="btn_submit" Style="max-width: 90%; margin: 0 auto;" class="btn btn-primary btn-block" runat="server" Text="Submit" OnClick="btn_submit_Click" />
                                <br />
                            </div>
                        </div>
                        <div class="box-body table-responsive">
                            <div>
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" DataSourceID="SqlDataSource1">
                                </asp:GridView>
                            </div>
                        </div>
                        <asp:RangeValidator MinimumValue="15.75" MaximumValue="50" ID="RangeValidator1" runat="server" ErrorMessage="Worker pay can't be less than $12.20 or greater than $50.00" ControlToValidate="txt_pay"></asp:RangeValidator>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT RTRIM(firstname) + ' ' + RTRIM(lastname) AS name, '$' + CAST(CAST(payrate AS MONEY) AS VARCHAR(MAX)) AS payrate FROM tbl_worker"></asp:SqlDataSource>

                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <div class="box">
                    <div class="box-header">
                        <div class="row">
                            <div class="col-sm-2">
                                Search Filters
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="cbo_day" class="dropdown dropdown-day" CssClass="form-control" runat="server" AutoPostBack="true"></asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-sm-2">
                                <asp:DropDownList ID="cbo_farm" class="dropdown dropdown-farm" CssClass="form-control" runat="server"></asp:DropDownList>
                                <br />
                            </div>
                            <div class="col-sm-1">
                                <asp:Button ID="btn_excel" class="btn btn-primary pull-right" runat="server" Text="Submit" OnClick="btn_excel_Click" />
                            </div>
                            <div class="col-sm-3">
                                <asp:Button ID="Button1" OnClick="Button1_Click" class="btn btn-primary pull-right" runat="server" Text="Download Import File For FlexTime" />
                                <asp:SqlDataSource ID="SqlDataSource2" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT [dbo].[tbl_worker].[FirstName] + [dbo].[tbl_worker].[LastName] as [Employee] ,d1.[Day] as [Date],[dbo].[tbl_farms].[Farm_Name] as [Work],[dbo].[tbl_job_cat].[CatName] as [Category],
b1.[Block_Name] as [Job Summary],
[dbo].[tbl_Attendance].[Start_time] as [Start Time],
[dbo].[tbl_Attendance].[End_time] as [Finish Time],
[dbo].[tbl_Attendance].[Total_hours] as [Duration],
[dbo].[tbl_Attendance].[breaktime] as [Break],
[dbo].[tbl_Attendance].[note] as [Description]
FROM [dbo].[tbl_Attendance] 
inner join [dbo].[tbl_blocks] as b1 on b1.[BlockId]=[dbo].[tbl_Attendance].[blockid]
inner join [dbo].[tbl_job_cat] on [dbo].[tbl_job_cat].[JobCatID]=[dbo].[tbl_Attendance].[job_cat]
INNER JOIN tbl_duty as d1 on tbl_attendance.rosterid = d1.rosterid 
INNER JOIN tbl_worker on tbl_worker.workersid = d1.workerid 
INNER JOIN tbl_shift as s1 on d1.shiftid = s1.shiftid  
Inner Join tbl_farms on tbl_farms.FarmId=s1.farmId

where 
d1.day = @1 and d1.[GrowerID]= @2 and s1.farmid =@3 and tbl_attendance.end_time is not null"
                                    runat="server">
                                    <SelectParameters>
                                        <asp:Parameter Name="1" />
                                        <asp:Parameter DefaultValue="" Name="2" />
                                        <asp:Parameter DefaultValue="" Name="3" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                                <br />
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="box-body table-responsive no-padding">
                        <div>
                            <asp:GridView CssClass="table table-hover" ID="dgd_workers" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2">
                                <Columns>
                                    <asp:BoundField DataField="Employee" HeaderText="Employee" ReadOnly="True" SortExpression="Employee" />
                                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" />
                                    <asp:BoundField DataField="Work" HeaderText="Work" SortExpression="Work" />
                                    <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category" />
                                    <asp:BoundField DataField="Job Summary" HeaderText="Job Summary" SortExpression="Job Summary" />
                                    <asp:BoundField DataField="Start Time" HeaderText="Start Time" SortExpression="Start Time" />
                                    <asp:BoundField DataField="Finish Time" HeaderText="Finish Time" SortExpression="Finish Time" />
                                    <asp:BoundField DataField="Duration" HeaderText="Duration" SortExpression="Duration" />
                                    <asp:BoundField DataField="Break" HeaderText="Break" SortExpression="Break" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    <div class="box-footer clearfix">
                    </div>
                </div>
            </div>
        </div>
        <%--    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT RTRIM(firstname) + ' ' + RTRIM(lastname) AS name, '$' + CAST(CAST(payrate AS MONEY) AS VARCHAR(MAX)) AS payrate FROM tbl_worker"></asp:SqlDataSource>--%>
    </div>
</asp:Content>

