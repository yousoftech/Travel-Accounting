<%@ Page Title="" Language="C#" MasterPageFile="ContractorMaster.master" AutoEventWireup="true" CodeFile="ContractorHome.aspx.cs" Inherits="ContractorHome" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ContentPlaceHolderID="cph_mainSection" runat="server">
    <style type="text/css">
        .direct-chat-messages {
            min-height: 290px;
        }

        .percentage-1 {
            position: absolute;
            top: 41%;
            left: 24%;
            text-align: center;
            font-size: 22px;
            width: 118px;
            word-wrap: break-word;
        }

        .workingSingleItem img {
            width: 30px;
            float: left;
            margin-right: 10px;
        }

        .workingSingleItem h3 {
            font-size: 15px;
            line-height: 30px;
            margin: 0;
            padding: 0;
        }

        .workingSingleItem {
            margin-bottom: 5px;
        }
    </style>
    <div class="row">
        <div class="col-sm-6">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="info-box">
                        <span class="info-box-icon bg-aqua"><i class="ion ion-calendar"></i></span>
                        <div class="info-box-content">
                            <span class="info-box-text">Workers Employed in
                        <asp:Label ID="lbl_month1" runat="server"></asp:Label></span>
                            <span class="info-box-number">
                                <asp:Label ID="lbl_monthVal1" runat="server">XX</asp:Label></span>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="info-box">
                        <span class="info-box-icon bg-red"><i class="ion ion-calendar"></i></span>
                        <div class="info-box-content">
                            <span class="info-box-text">Workers Employed in
                        <asp:Label ID="lbl_month2" runat="server"></asp:Label></span>
                            <span class="info-box-number">
                                <asp:Label ID="lbl_monthVal2" runat="server">XX</asp:Label></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="info-box">
                        <span class="info-box-icon bg-green"><i class="ion ion-calendar"></i></span>
                        <div class="info-box-content">
                            <span class="info-box-text">Workers Employed in
                        <asp:Label ID="lbl_month3" runat="server"></asp:Label></span>
                            <span class="info-box-number">
                                <asp:Label ID="lbl_monthVal3" runat="server">XX</asp:Label></span>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <div class="info-box">
                        <span class="info-box-icon bg-yellow"><i class="ion ion-calendar"></i></span>
                        <div class="info-box-content">
                            <span class="info-box-text">Workers Employed This Month</span>
                            <span class="info-box-number">
                                <asp:Label ID="lbl_monthVal4" runat="server">XX</asp:Label></span>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-sm-12">
                    <a href="http://www.accuweather.com/en/nz/tauranga/246959/weather-forecast/246959" class="aw-widget-legal"></a>
                    <div id="awcc1481679051332" class="aw-widget-current" data-locationkey="246959" data-unit="c" data-language="en-us" data-useip="false" data-uid="awcc1481679051332"></div>
                    <script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="row">
                <div class="col-md-6">
                    <div class="box box-primary direct-chat direct-chat-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="lbl_workerTodayTitle" runat="server" Text="Working for you today"></asp:Label></h3>
                        </div>
                        <div class="box-body">
                            <div class="direct-chat-messages">
                                <asp:Label ID="lbl_workerTodayContent" runat="server" Text="lbl_workerTodayContent"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="box box-primary direct-chat direct-chat-primary">
                        <div class="box-header with-border">
                            <h3 class="box-title">
                                <asp:Label ID="lbl_workerTomorrowTitle" runat="server" Text="Working for you tomorrow"></asp:Label></h3>
                        </div>
                        <div class="box-body">

                            <div class="direct-chat-messages">
                                <div class="direct-chat-msg">
                                    <%--<img class="direct-chat-img" src="dist/img/user1-128x128.jpg" alt="Message User Image" />--%>
                                    <%--<div class="direct-chat-text">
                                        <span class="direct-chat-name pull-left">Alexander Pierce</span>
                                    </div>--%>
                                    <asp:Label ID="lbl_workerTomorrowContent" runat="server" Text="lbl_workerTomorrowContent"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="box">
                <div class="box-header with-border">
                    <h3 class="box-title">Monthly Recap Report</h3>
                </div>
                <div class="box-body">
                    <div class="dev">
                        <asp:Chart ID="Chart2" runat="server" BackColor="Transparent" Width="600px" BorderlineColor="" DataSourceID="SqlDataSource1" Palette="None" TextAntiAliasingQuality="Normal" AntiAliasing="Graphics">
                            <Series>
                                <asp:Series Name="Workers Paid" BorderWidth="2" ChartType="Line" Color="137, 181, 61" Legend="Legend1" XValueMember="month" YValueMembers="workers paid" Font="Roboto Light, 8.25pt"></asp:Series>
                                <asp:Series ChartArea="ChartArea1" ChartType="Line" Legend="Legend1" Name="Budget Recieved" XValueMember="month" YValueMembers="budget recieved" Font="Roboto Light, 8.25pt">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BackColor="Transparent"></asp:ChartArea>
                            </ChartAreas>
                            <Legends>
                                <asp:Legend Name="Legend1" BackColor="Transparent" Font="Roboto Light, 9.75pt" IsTextAutoFit="False">
                                </asp:Legend>
                            </Legends>
                            <BorderSkin BackSecondaryColor="White" BorderColor="Maroon" BorderDashStyle="Solid" BorderWidth="5" />
                        </asp:Chart>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-6">
            <div class="box">
                <div class="box-header with-border">
                    <h3 class="box-title">Remaining Budget</h3>
                    <div class="box-tools pull-right">
                        For this month
                    </div>
                </div>
                <div class="box-body">
                    <div class="dev">
                        <asp:Chart ID="Chart1" Width="600" runat="server" DataSourceID="SqlDataSource2" PaletteCustomColors="203, 203, 203; 137, 181, 61" BackColor="Transparent" BackImageTransparentColor="Transparent" BorderlineColor="Transparent" Palette="None" PageColor="Transparent" AntiAliasing="Graphics">
                            <Series>
                                <asp:Series Name="Series1" ChartType="Doughnut" XValueMember="grouper" YValueMembers="S" CustomProperties="DoughnutRadius=10, PieLabelStyle=Disabled, PieStartAngle=270" IsValueShownAsLabel="False" Legend="Legend1">
                                </asp:Series>
                            </Series>
                            <ChartAreas>
                                <asp:ChartArea Name="ChartArea1" BackColor="Transparent"></asp:ChartArea>
                            </ChartAreas>
                            <Legends>
                                <asp:Legend BackColor="Transparent" Name="Legend1" Alignment="Center" DockedToChartArea="ChartArea1" Font="Roboto Light, 9.75pt" IsDockedInsideChartArea="False" IsTextAutoFit="False">
                                </asp:Legend>
                            </Legends>
                            <BorderSkin BackColor="Transparent" PageColor="Transparent" />
                        </asp:Chart>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT SUM(Amount) FROM tbl_budget WHERE MONTH(TimeStamp) = 1"></asp:SqlDataSource>
                        <div class="percentage-1">
                            <asp:Label ID="lbl_percentage2" runat="server" ForeColor="#89B53D" Text="X%"></asp:Label><br />
                            <asp:Label ID="lbl_amount2" runat="server" ForeColor="#CBCBCB" Text="$x"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="ViewBudgetAndWorkerPay" SelectCommandType="StoredProcedure"></asp:SqlDataSource>
</asp:Content>
