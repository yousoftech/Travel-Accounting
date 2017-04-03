<%@ Page Title="" Language="C#" MasterPageFile="WorkerMaster.master" AutoEventWireup="true" CodeFile="WorkerHome.aspx.cs" Inherits="WorkerHome" %>

<asp:Content ContentPlaceHolderID="cph_mainSection" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-aqua"><i class="ion ion-ios-time-outline"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lbl_workStartToday" runat="server" Text="Today Work Start: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_startTimeToday" runat="server" Text="--:-- --"></asp:Label></span>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-red"><i class="ion ion-ios-time-outline"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lbl_workEndToday" runat="server" Text="Today Work End: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_endTimeToday" runat="server" Text="--:-- --"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <div class="clearfix visible-sm-block"></div>
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-green"><i class="ion ion-person"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lbl_contractorToday" runat="server" Text="Today's Contractor: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_contractorValueToday" runat="server" Text="None"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-yellow"><i class="ion ion-cash"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lbl_hourlyRateToday" runat="server" Text="Hourly Rate: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_hourlyRateValueToday" runat="server" Text="$X.XX"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
        </div>

        <div class="row">
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-aqua"><i class="ion ion-ios-time-outline"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lbl_workStartTomorrow" runat="server" Text="Tomorrow Work Start: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_startTimeTomorrow" runat="server" Text="--:-- --"></asp:Label></span>
                    </div>
                </div>
            </div>
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-red"><i class="ion ion-ios-time-outline"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lbl_workEndTomorrow" runat="server" Text="Tomorrow Work End: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_endTimeTomorrow" runat="server" Text="--:-- --"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <div class="clearfix visible-sm-block"></div>
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-green"><i class="ion ion-person"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lbl_contractorTomorrow" runat="server" Text="Tomorrow's Contractor: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_contractorValueTomorrow" runat="server" Text="None"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
            <div class="col-md-3 col-sm-6 col-xs-12">
                <div class="info-box">
                    <span class="info-box-icon bg-yellow"><i class="ion ion-cash"></i></span>
                    <div class="info-box-content">
                        <span class="info-box-text">
                            <asp:Label ID="lblhourlyRateTomorrow" runat="server" Text="Hourly Rate: "></asp:Label></span>
                        <span class="info-box-number">
                            <asp:Label ID="lbl_hourlyRateValueTomorrow" runat="server" Text="$X.XX"></asp:Label></span>
                    </div>
                    <!-- /.info-box-content -->
                </div>
                <!-- /.info-box -->
            </div>
        </div>

        <div class="row">

            <div class="col-sm-8">
                <div class="box">
                    <div class="box-header">
                        <div class="row">
                            <div class="col-sm-1">
                                Grower:
                            </div>
                            <div class="col-sm-3">
                                <asp:CheckBoxList ID="chk_workers" class="top-bar-worker-list" runat="server"></asp:CheckBoxList>
                                <asp:DropDownList ID="cbo_contractors" CssClass="form-control" runat="server"></asp:DropDownList>
                                <asp:SqlDataSource ID="sds_growers" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT tbl_grower.GrowersId, tbl_grower.FirstName, tbl_grower.LastName FROM tbl_grower INNER JOIN tbl_employees ON tbl_grower.growersid = tbl_employees.growersid where tbl_employees.workersid = @0"></asp:SqlDataSource>
                            </div>
                            <div class="col-sm-2">
                                Week Start Date:
                            </div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="cbo_weekStart" CssClass="form-control" runat="server" OnSelectedIndexChanged="cbo_weekStart_SelectedIndexChanged" AutoPostBack="True">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="box-body table-responsive no-padding">
                        <table class="table table-hover">
                            <tbody>
                                <tr>
                                    <td scope="col"></td>
                                    <th scope="col">Monday</th>
                                    <th scope="col">Tuesday</th>
                                    <th scope="col">Wednesday</th>
                                    <th scope="col">Thursday</th>
                                    <th scope="col">Friday</th>
                                    <th scope="col">Saturday</th>
                                    <th scope="col">Sunday</th>
                                </tr>
                                <tr>
                                    <td>Start Time</td>
                                    <td>
                                        <asp:TextBox ID="txt_mondayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_tuesdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_wednesdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_thursdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_fridayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_saturdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_sundayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>End Time</td>
                                    <td>
                                        <asp:TextBox ID="txt_mondayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_tuesdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_wednesdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_thursdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_fridayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_saturdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_sundayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox></td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>
                <a href="http://www.accuweather.com/en/nz/tauranga/246959/weather-forecast/246959" class="aw-widget-legal"></a>
                <div id="awcc1481679051332" class="aw-widget-current" data-locationkey="246959" data-unit="c" data-language="en-us" data-useip="false" data-uid="awcc1481679051332"></div>
                <script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>
            </div>
        </div>
    </section>

</asp:Content>
