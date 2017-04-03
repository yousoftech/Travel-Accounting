<%@ Page Title="" Language="C#" MasterPageFile="~/Monitor/MonitorMaster.master" AutoEventWireup="true" CodeFile="MonitorHome.aspx.cs" Inherits="Monitor_MonitorHome" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%--Piechart content starts here  <<<<AJAXCONTROL TOOLKIT>>>>--%>

<%--Piechart content ends here--%>


<asp:Content ID="cph_mainSection" ContentPlaceHolderID="cph_mainSection" runat="server">
    <style type="text/css">
        div#cph_mainSection_PieChart2__ParentDiv, div#cph_mainSection_PieChart1__ParentDiv {
            border: transparent;
        }

            div#cph_mainSection_PieChart2__ParentDiv svg, div#cph_mainSection_PieChart1__ParentDiv svg {
                margin-left: -83px;
                /*margin-top: -70px;*/
            }
    </style>
    <script>moveIcon(0);</script>
    <div class="content">
        <div class="row">
            <div class="col-sm-4">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Number of contractor Vs Number of farms</h3>
                    </div>
                    <div class="box-body" style="max-height: 430px;">
                        <asp:PieChart ID="PieChart2" runat="server" ChartHeight="550px" ChartWidth="550px"
                            ChartType="Column" ChartTitleColor="#0E426C" BorderStyle="None" EnableTheming="False" ForeColor="#FF9900" BorderColor="#33CC33">
                        </asp:PieChart>

                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Recent Payment</h3>
                    </div>
                    <div class="box-body" style="min-height: 430px;">
                        <asp:GridView ID="gvmonitorbudgetRP" runat="server" AutoGenerateColumns="true" DataSourceID="SqlDataSource_gvmbrecentpayment" CellPadding="4" GridLines="None" AllowPaging="True" AllowSorting="True" CssClass="table table-hover">
                            <%-- <Columns>
                           
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category"></asp:BoundField>
                            <asp:BoundField DataField="Payment" HeaderText="Payment" SortExpression="Payment"></asp:BoundField>
                              <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date"></asp:BoundField>
                           
                        </Columns>--%>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource_gvmbrecentpayment" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT CONCAT(tbl_grower.FirstName, tbl_grower.LastName) as Name, tbl_job_mcat.CatName as Category, '$'+CAST(tbl_Budget.Amount as VARCHAR(15)) Payment, tbl_Budget.TimeStamp as Date  FROM [tbl_Budget] INNER JOIN tbl_grower ON tbl_Budget.GrowerID=tbl_grower.GrowersId INNER JOIN tbl_job_mcat ON tbl_Budget.McatID=tbl_job_mcat.JobmCatID where tbl_Budget.MonitorID=@Mid ">
                            <SelectParameters>
                                <asp:SessionParameter Name="Mid" SessionField="Id" ConvertEmptyStringToNull="true" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Weather Forcast</h3>
                    </div>
                    <div class="box-body" style="min-height: 430px;">
                        <!-- weather widget start -->
                        <div id="m-booked-bl-simple-week-vertical-11301">
                            <a href="http://www.booked.net/weather/auckland-18049" class="booked-wzs-160-275 weather-customize" style="background-color: #137AE9; width: 160px;" id="width1">
                                <div class="booked-wzs-160-275_in">
                                    <div class="booked-wzs-160-275-data">
                                        <div class="booked-wzs-160-275-left-img wrz-18"></div>
                                        <div class="booked-wzs-160-275-right">
                                            <div class="booked-wzs-day-deck">
                                                <div class="booked-wzs-day-val">
                                                    <div class="booked-wzs-day-number"><span class="plus">+</span>19</div>
                                                    <div class="booked-wzs-day-dergee">
                                                        <div class="booked-wzs-day-dergee-val">&deg;</div>
                                                        <div class="booked-wzs-day-dergee-name">C</div>
                                                    </div>
                                                </div>
                                                <div class="booked-wzs-day">
                                                    <div class="booked-wzs-day-d"><span class="plus">+</span>21&deg;</div>
                                                    <div class="booked-wzs-day-n"><span class="plus">+</span>19&deg;</div>
                                                </div>
                                            </div>
                                            <div class="booked-wzs-160-275-info">
                                                <div class="booked-wzs-160-275-city">Auckland</div>
                                                <div class="booked-wzs-160-275-date">Wednesday, 22</div>
                                            </div>
                                        </div>
                                    </div>
                                    <table cellpadding="0" cellspacing="0" class="booked-wzs-table-160">
                                        <tr>
                                            <td class="week-day"><span class="week-day-txt">Thursday</span></td>
                                            <td class="week-day-ico">
                                                <div class="wrz-sml wrzs-03"></div>
                                            </td>
                                            <td class="week-day-val"><span class="plus">+</span>21&deg;</td>
                                            <td class="week-day-val"><span class="plus">+</span>17&deg;</td>
                                        </tr>
                                        <tr>
                                            <td class="week-day"><span class="week-day-txt">Friday</span></td>
                                            <td class="week-day-ico">
                                                <div class="wrz-sml wrzs-18"></div>
                                            </td>
                                            <td class="week-day-val"><span class="plus">+</span>21&deg;</td>
                                            <td class="week-day-val"><span class="plus">+</span>17&deg;</td>
                                        </tr>
                                        <tr>
                                            <td class="week-day"><span class="week-day-txt">Saturday</span></td>
                                            <td class="week-day-ico">
                                                <div class="wrz-sml wrzs-18"></div>
                                            </td>
                                            <td class="week-day-val"><span class="plus">+</span>19&deg;</td>
                                            <td class="week-day-val"><span class="plus">+</span>12&deg;</td>
                                        </tr>
                                        <tr>
                                            <td class="week-day"><span class="week-day-txt">Sunday</span></td>
                                            <td class="week-day-ico">
                                                <div class="wrz-sml wrzs-18"></div>
                                            </td>
                                            <td class="week-day-val"><span class="plus">+</span>20&deg;</td>
                                            <td class="week-day-val"><span class="plus">+</span>13&deg;</td>
                                        </tr>
                                        <tr>
                                            <td class="week-day"><span class="week-day-txt">Monday</span></td>
                                            <td class="week-day-ico">
                                                <div class="wrz-sml wrzs-18"></div>
                                            </td>
                                            <td class="week-day-val"><span class="plus">+</span>21&deg;</td>
                                            <td class="week-day-val"><span class="plus">+</span>16&deg;</td>
                                        </tr>
                                        <tr>
                                            <td class="week-day"><span class="week-day-txt">Tuesday</span></td>
                                            <td class="week-day-ico">
                                                <div class="wrz-sml wrzs-18"></div>
                                            </td>
                                            <td class="week-day-val"><span class="plus">+</span>20&deg;</td>
                                            <td class="week-day-val"><span class="plus">+</span>18&deg;</td>
                                        </tr>
                                    </table>
                                    <div class="booked-wzs-center"><span class="booked-wzs-bottom-l">See 7-Day Forecast</span> </div>
                                </div>
                            </a>
                        </div>
                        <script type="text/javascript"> var css_file = document.createElement("link"); css_file.setAttribute("rel", "stylesheet"); css_file.setAttribute("type", "text/css"); css_file.setAttribute("href", 'https://s.bookcdn.com/css/w/booked-wzs-widget-160x275.css?v=0.0.1'); document.getElementsByTagName("head")[0].appendChild(css_file); function setWidgetData(data) { if (typeof (data) != 'undefined' && data.results.length > 0) { for (var i = 0; i < data.results.length; ++i) { var objMainBlock = document.getElementById('m-booked-bl-simple-week-vertical-11301'); if (objMainBlock !== null) { var copyBlock = document.getElementById('m-bookew-weather-copy-' + data.results[i].widget_type); objMainBlock.innerHTML = data.results[i].html_code; if (copyBlock !== null) objMainBlock.appendChild(copyBlock); } } } else { alert('data=undefined||data.results is empty'); } } </script>
                        <script type="text/javascript" charset="UTF-8" src="https://widgets.booked.net/weather/info?action=get_weather_info&ver=5&cityID=18049&type=4&scode=2&ltid=3457&domid=w209&anc_id=84088&cmetric=1&wlangID=1&color=060a0f&wwidth=160&header_color=ffffff&text_color=333333&link_color=08488D&border_form=1&footer_color=ffffff&footer_text_color=333333&transparent=0"></script>
                        <!-- weather widget end -->
                    </div>
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="row">
            <div class="col-sm-4">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Budget</h3>
                        <asp:DropDownList ID="ddlSelectFarmForBudgetPie" runat="server" OnSelectedIndexChanged="ddlSelectFarmForBudgetPie_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    </div>
                    <div class="box-body" style="max-height: 430px;">

                        <asp:PieChart ID="PieChart1" runat="server" ChartHeight="550px" ChartWidth="550px"
                            ChartType="Column" ChartTitleColor="#0E426C" BorderStyle="None" EnableTheming="False">
                        </asp:PieChart>
                    </div>
                </div>
            </div>
            <div class="col-sm-6">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Recent Payment</h3>
                    </div>
                    <div class="box-body" style="min-height: 430px;">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" DataSourceID="SqlDataSource1" CellPadding="4" GridLines="None" AllowPaging="True" AllowSorting="True" CssClass="table table-hover">
                            <%-- <Columns>
                           
                            <asp:BoundField DataField="Name" HeaderText="Name" SortExpression="Name"></asp:BoundField>
                            <asp:BoundField DataField="Category" HeaderText="Category" SortExpression="Category"></asp:BoundField>
                            <asp:BoundField DataField="Payment" HeaderText="Payment" SortExpression="Payment"></asp:BoundField>
                              <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date"></asp:BoundField>
                           
                        </Columns>--%>
                        </asp:GridView>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT CONCAT(tbl_grower.FirstName, tbl_grower.LastName) as Name, tbl_job_mcat.CatName as Category, '$'+CAST(tbl_Budget.Amount as VARCHAR(15)) Payment, tbl_Budget.TimeStamp as Date  FROM [tbl_Budget] INNER JOIN tbl_grower ON tbl_Budget.GrowerID=tbl_grower.GrowersId INNER JOIN tbl_job_mcat ON tbl_Budget.McatID=tbl_job_mcat.JobmCatID where tbl_Budget.MonitorID=@Mid ">
                            <SelectParameters>
                                <asp:SessionParameter Name="Mid" SessionField="Id" ConvertEmptyStringToNull="true" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </div>
            <div class="col-sm-2">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Contractor</h3>
                    </div>
                    <div class="box-body" style="min-height: 430px; max-height: 430px; overflow-y: auto;">
                        <%--Add list view here--%>
                        <asp:ListView ID="LVContractors" runat="server" DataSourceID="SqlDataSource2" ItemPlaceholderID="ContentID">

                            <ItemTemplate>


                                <asp:Label ID="Name" runat="server"
                                    Text='<%#Eval("Name") %>' />
                                <br />
                                <br />

                            </ItemTemplate>

                        </asp:ListView>
                        <%--Listview ends Here--%>


                        <%-- <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:KiwihortData %>'
                        SelectCommand="select tbl_Duty.GrowerID, COUNT(*), CONCAT(tbl_Grower.FirstName,tbl_Grower.LastName) AS Name from tbl_Duty  INNER JOIN tbl_Grower ON tbl_Duty.GrowerID=tbl_Grower.GrowerID GROUP BY tbl_Duty.GrowerID, tbl_Grower.FirstName,tbl_Grower.LastName  HAVING COUNT(*)>1 "></asp:SqlDataSource>
                        --%>
                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="select ( rTRIM([dbo].[tbl_grower].[FirstName] )+[dbo].[tbl_grower].[LastName]) as Name from [dbo].[tbl_grower] inner join 

[dbo].[tbl_employees] on [dbo].[tbl_employees].[workersid]=[dbo].[tbl_grower].[GrowersId]

where [dbo].[tbl_employees].[growersid]=@Mid">
                            <SelectParameters>
                                <asp:SessionParameter Name="Mid" SessionField="Id" ConvertEmptyStringToNull="true" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
