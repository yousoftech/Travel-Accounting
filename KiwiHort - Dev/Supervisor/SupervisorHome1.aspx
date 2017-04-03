<%@ Page Title="" Language="C#" MasterPageFile="~/Supervisor/SupervisorMaster.master" AutoEventWireup="true" CodeFile="SupervisorHome.aspx.cs" Inherits="SupervisorHome" %>
<asp:Content ID="Header" runat="server" contentplaceholderid="Header"> 
     <style>
    body { font: normal 100.01%/1.375 "Helvetica Neue",Helvetica,Arial,sans-serif; }
  </style>
  <link href="jquery.signaturepad.css" rel="stylesheet">
  <!--[if lt IE 9]><script src="../assets/flashcanvas.js"></script><![endif]-->
     <link href="jquery.signaturepad.css" rel="stylesheet">
  <!--[if lt IE 9]><script src="../assets/flashcanvas.js"></script><![endif]-->
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.1/jquery.min.js"></script>
    <script src="../js/jsign.js"></script>
        <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js"></script>
        <script type="text/javascript">


            function test() {
                var canvas = document.getElementById('thecanvas');
              
                    var image = document.getElementById("thecanvas").toDataURL("image/png");

                    image = image.replace('data:image/png;base64,', '');

                    $.ajax({
                        type: 'POST', url: 'SupervisorHome.aspx/UploadImage', data: '{ "imageData" : "' + image + '" }', contentType: 'application/json; charset=utf-8', dataType: 'json',
                        success: function (msg) {

                           

                        }


                    
                });
                  
            }
            function testend() {
                var canvas = document.getElementById('thecanvasend');

                var image = document.getElementById("thecanvasend").toDataURL("image/png");

                image = image.replace('data:image/png;base64,', '');

                $.ajax({
                    type: 'POST', url: 'SupervisorHome.aspx/UploadImage', data: '{ "imageData" : "' + image + '" }', contentType: 'application/json; charset=utf-8', dataType: 'json',
                    success: function (msg) {



                    }



                });

            }



        </script>

</asp:Content>
<asp:Content ContentPlaceHolderId="cph_mainSection" runat="server">

    <div class="singleStat-weather">
    <a href="http://www.accuweather.com/en/nz/tauranga/246959/weather-forecast/246959" class="aw-widget-legal">
                </a><div id="awcc1481679051332" class="aw-widget-current"  data-locationkey="246959" data-unit="c" data-language="en-us" data-useip="false" data-uid="awcc1481679051332"></div><script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>
                
    </div>
    <script type="text/javascript" src="http://oap.accuweather.com/launch.js"></script>

    <div class="worker-container">
        <div class="worker-time">
            <div class="worker-title">
                <asp:Label ID="lbl_workStartToday" runat="server" Text="Today Work Start: "></asp:Label>
            </div>
            <asp:Label ID="lbl_startTimeToday" runat="server" Text="--:-- --"></asp:Label>
        </div>
        <div class="worker-time">
            <div class="worker-title">
                <asp:Label ID="lbl_workEndToday" runat="server" Text="Today Work End: "></asp:Label>
            </div>
            <asp:Label ID="lbl_endTimeToday" runat="server" Text="--:-- --"></asp:Label>
        </div>
        <div class="worker-text">
            <div class="worker-title">
                <asp:Label ID="lbl_contractorToday" runat="server" Text="Today's Contractor: "></asp:Label>
            </div>
            <asp:Label ID="lbl_contractorValueToday" runat="server" Text="None"></asp:Label>
        </div>
        <div class="worker-text">
            <div class="worker-title">
                <asp:Label ID="lbl_hourlyRateToday" runat="server" Text="Hourly Rate: "></asp:Label>
            </div>
            <asp:Label ID="lbl_hourlyRateValueToday" runat="server" Text="$X.XX"></asp:Label>
        </div>
    </div>

    <div class="worker-container">
        <div class="worker-time">
            <div class="worker-title">
                <asp:Label ID="lbl_workStartTomorrow" runat="server" Text="Tomorrow Work Start: "></asp:Label>
            </div>
            <asp:Label ID="lbl_startTimeTomorrow" runat="server" Text="--:-- --"></asp:Label>
        </div>
        <div class="worker-time">
            <div class="worker-title">
                <asp:Label ID="lbl_workEndTomorrow" runat="server" Text="Tomorrow Work End: "></asp:Label>
            </div>
            <asp:Label ID="lbl_endTimeTomorrow" runat="server" Text="--:-- --"></asp:Label>
        </div>
        <div class="worker-text">
            <div class="worker-title">
                <asp:Label ID="lbl_contractorTomorrow" runat="server" Text="Tomorrow's Contractor: "></asp:Label>
                </div>
            <asp:Label ID="lbl_contractorValueTomorrow" runat="server" Text="None"></asp:Label>
        </div>
        <div class="worker-text">
            <div class="worker-title">
                <asp:Label ID="lblhourlyRateTomorrow" runat="server" Text="Hourly Rate: "></asp:Label>
            </div>
            <asp:Label ID="lbl_hourlyRateValueTomorrow" runat="server" Text="$X.XX"></asp:Label>
        </div>
    </div>

    <div id="jobPortalPlaceHolder">

        <div class="jobSearchBar">

        </div>

        <div class="singleJob">

        </div>
        <div class="singleJob">

        </div>
        <div class="singleJob">
             
        </div>

    </div>
    <div class="organiser-container">
        <div class="dashboard-top-bar">

            <div class="grower-label">
                <span class="top-bar-span-1">Grower:</span>
                <div class="grower-label-data">
                    <asp:CheckBoxList ID="chk_workers" class="top-bar-worker-list" runat="server"></asp:CheckBoxList>
                    <asp:DropDownList ID="cbo_contractors" runat="server"></asp:DropDownList>
                    <asp:SqlDataSource ID="sds_growers" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT tbl_grower.GrowersId, tbl_grower.FirstName, tbl_grower.LastName FROM tbl_grower INNER JOIN tbl_employees ON tbl_grower.growersid = tbl_employees.growersid where tbl_employees.workersid = @0"></asp:SqlDataSource>
                </div>
            </div>
            <div class="grower-selection">
                <span class="top-bar-span-2">Week Start Date:</span>
                <div class="grower-selection-data">
                    
                    <asp:DropDownList ID="cbo_weekStart" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbo_weekStart_SelectedIndexChanged">
                        
                    </asp:DropDownList>






















                </div>
            </div>

            <%--<asp:Button ID="btn_submit" class="top-bar-submit" runat="server" OnClick="btn_submit_Click" Text="View" />--%>
        </div>


        <div class="single-day-time" style="padding-top: 40px;">
            <span class="single-day-time-day">Monday</span>
            Start: <asp:TextBox ID="txt_mondayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
            End:
            <asp:TextBox ID="txt_mondayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
        </div>
        <div class="single-day-time">
            <span class="single-day-time-day">Tuesday</span>
            Start: <asp:TextBox ID="txt_tuesdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
            End:
            <asp:TextBox ID="txt_tuesdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>

        </div>
        <div class="single-day-time">
            <span class="single-day-time-day">Wednesday</span>
            Start: <asp:TextBox ID="txt_wednesdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
            End:
            <asp:TextBox ID="txt_wednesdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>

        </div>
        <div class="single-day-time">
            <span class="single-day-time-day">Thursday</span>
            Start: <asp:TextBox ID="txt_thursdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
            End:
            <asp:TextBox ID="txt_thursdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>

        </div>
        <div class="single-day-time">
            <span class="single-day-time-day">Friday</span>
            Start: <asp:TextBox ID="txt_fridayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
            End:
            <asp:TextBox ID="txt_fridayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>

        </div>
        <div class="single-day-time">
            <span class="single-day-time-day">Saturday</span>
            Start: <asp:TextBox ID="txt_saturdayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
            End:
            <asp:TextBox ID="txt_saturdayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>


        </div>
        <div class="single-day-time">
            <span class="single-day-time-day">Sunday</span>
            Start: <asp:TextBox ID="txt_sundayStart" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
            End:
            <asp:TextBox ID="txt_sundayEnd" runat="server" CausesValidation="True" CssClass="timeInput" TextMode="Time" Enabled="False" ReadOnly="True"></asp:TextBox>
   
        </div>
    </div>

</asp:Content>
