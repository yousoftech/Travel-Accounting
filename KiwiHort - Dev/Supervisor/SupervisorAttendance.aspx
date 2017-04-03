<%@ Page Title="" Language="C#" MasterPageFile="~/Supervisor/SupervisorMaster.master" AutoEventWireup="true" CodeFile="SupervisorAttendance.aspx.cs" Inherits="Supervisor_SupervisorAttendance" %>

<asp:Content ID="Header" runat="server" ContentPlaceHolderID="Header">
    <%--<link href="jquery.signaturepad.css" rel="stylesheet" />--%>
    <!--[if lt IE 9]><script src="../assets/flashcanvas.js"></script><![endif]-->
    <%--<link href="jquery.signaturepad.css" rel="stylesheet" />--%>
    <!--[if lt IE 9]><script src="../assets/flashcanvas.js"></script><![endif]-->
    <script src="../js/workerDashboard.js" type="text/javascript"></script>
    <%--<script src="../js/jsign.js"></script>--%>

    <script type="text/javascript">
        moveIcon(1);
        function test() {
            var canvas = document.getElementById('thecanvas');
            var image = document.getElementById("thecanvas").toDataURL("image/png");
            image = image.replace('data:image/png;base64,', '');
            $.ajax({
                type: 'POST', url: 'SupervisorAttendance.aspx/UploadImage', data: '{ "imageData" : "' + image + '" }', contentType: 'application/json; charset=utf-8', dataType: 'json',
                success: function (msg) {
                }
            });
        }
        function testend() {
            var canvas = document.getElementById('thecanvasend');
            var image = document.getElementById("thecanvasend").toDataURL("image/png");
            image = image.replace('data:image/png;base64,', '');
            $.ajax({
                type: 'POST', url: 'SupervisorAttendance.aspx/UploadImage', data: '{ "imageData" : "' + image + '" }', contentType: 'application/json; charset=utf-8', dataType: 'json',
                success: function (msg) {
                }
            });
        }
        $(function () {
            $("#test1").click(function () {
                var canvas = document.getElementById('thecanvas');
                var context = canvas.getContext('2d');
                context.clearRect(0, 0, canvas.width, canvas.height);
            });
        });
    </script>
    <style type="text/css">
        table.table.table-hover tr td img {
            width: 30px;
            height: 30px;
            display: block;
            background: #dcdcdc;
        }

        .nav-tabs-custom li.active a:hover {
            background: #00a65a !important;
            cursor: pointer !important;
            color: #fff !important;
        }

        @media (max-width:768px) {
            #tab li.active {
                width: 100%;
            }

            table.table.table-hover tr td {
                width: inherit;
            }
        }

        table.table.table-hover tr td {
            width: 16%;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="cph_mainSection" runat="server">
    <%--  <asp:Button ID="Button1" runat="server" Text="Check Start Times" OnClientClick="return Drawdot()" OnClick="Button1_Click" class="big-button"/>--%>
    <%--<asp:LinkButton ID="LinkButton2" class="big-button" runat="server" Style="padding: 7px; padding-right: 20px; padding-left: 20px;" OnClick="Button1_Click">Check Start Times</asp:LinkButton>--%>
    <%--<asp:LinkButton ID="LinkButton1" class="big-button" runat="server" Style="padding: 7px; padding-right: 20px; padding-left: 20px;" OnClick="Button2_Click">Check Finish Times</asp:LinkButton>--%>
    <%--<asp:LinkButton ID="LinkButton3" class="big-button" runat="server" Style="padding: 7px; padding-right: 20px; padding-left: 20px;" OnClick="btn_break_Click">Add break</asp:LinkButton>--%>
    <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>--%>
    <%--    <asp:Button ID="Button2" runat="server" Text="Check Finish Times" OnClick="Button2_Click"  OnClientClick="return Drawdot()" class="big-button" />--%>
    <div class="content">
        <div class="row">
            <div class="col-md-12">
                <div class="nav-tabs-custom">
                    <ul id="tab" class="nav nav-tabs">
                        <li class="active">
                            <asp:LinkButton ID="LinkButton2" OnClick="Button1_Click" runat="server">Check Start Times</asp:LinkButton></li>
                        <li class="active">
                            <asp:LinkButton ID="LinkButton3" OnClick="btn_break_Click" runat="server">Add break</asp:LinkButton></li>
                        <li class="active">
                            <asp:LinkButton ID="LinkButton1" OnClick="Button2_Click" runat="server">Check Finish Times</asp:LinkButton></li>
                        <li style="margin: 5px;">
                            <asp:CheckBox ID="chkphase" Text="Worker On Phase" AutoPostBack="true" OnCheckedChanged="chkphase_CheckedChanged" runat="server" />
                        </li>
                        <li style="margin: 5px;">
                            <div class="form-inline">
                                <div class="form-group">
                                    <label for="">Bin Rate: </label>
                                    <asp:TextBox ID="txtrate" CssClass="form-control" runat="server" Text="0" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li style="margin: 5px;">
                            <div class="form-inline">
                                <div class="form-group">
                                    <label for="">Number of Bin:  </label>
                                    <asp:TextBox ID="txtbin" runat="server" CssClass="form-control" Text="0" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                    </ul>

                    <div class="tab-content">
                        <asp:MultiView ID="Mv1" runat="server" OnActiveViewChanged="MultiView1_ActiveViewChanged">
                            <div class="tab-pane active" id="tab_1">
                                <asp:View ID="View1" runat="server">
                                    <h3 class="title-bar">Start Times</h3>
                                    <div id="startTimesContent" class="times-content" runat="server">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GridView3" CssClass="table table-hover" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="RosterID,ShiftID">
                                                <Columns>
                                                    <asp:ImageField DataImageUrlField="Picture" HeaderText="Image" ReadOnly="true"></asp:ImageField>
                                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                                                    <asp:BoundField DataField="Shiftstarttime" HeaderText="Start Time" SortExpression="Shiftstarttime" />
                                                    <asp:TemplateField HeaderText="Alt Start Time">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" Style="float: left; margin-right: 5px;" runat="server" />
                                                            <asp:TextBox ID="txt_time" CssClass="form-control" Style="width: 120px;" runat="server" TextMode="Time"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Farm Block">
                                                        <ItemTemplate>
                                                            <asp:DropDownList Style="width: 100px" ID="cbo_block" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Main Cat">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="cbo_mcat" Style="width: 200px" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="cbo_mcat_SelectedIndexChanged" runat="server"></asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Sub Cat">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="cbo_scat" Style="width: 200px" CssClass="form-control" runat="server"></asp:DropDownList>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT tbl_Duty.RosterID, tbl_shift.ShiftID, [tbl_worker].[Picture], [tbl_worker].[FirstName], [tbl_worker].[LastName], tbl_Shift.Shiftstarttime, tbl_Shift.TotalTime FROM  tbl_Shift INNER JOIN tbl_Duty
                    ON tbl_Shift.ShiftID=tbl_Duty.ShiftID
                    INNER JOIN [dbo].[tbl_worker] on [tbl_worker].[WorkersId]=[tbl_Duty].[WorkerID]
                    WHERE tbl_Duty.Day = @wday AND tbl_Duty.SupervisorId = @name">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="" Name="wday" />
                                                    <asp:Parameter Name="name" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                        <%--<asp:Button ID="btn_startSubmit" class="big-button" runat="server" Text="Button" OnClick="test" />--%>

                                        <%--           <input type="button" id="btn_startSubmit"   class="big-button" name="btnSave" value="Save the canvas to server"  />--%>
                                        <%--</div>--%>
                                        <div id="startTimesEmpty" class="empty-text" runat="server">
                                            <h1>All worker times have been completed.</h1>
                                        </div>
                                        <div class="drawIt">
                                            <h4>Sign Here</h4>
                                        </div>
                                        <%--<ul class="sigNav">
                                            <li class="drawIt"><%--<a href="#draw-it">Sign Here</a>--%></li>
                                            <%--<li class="clearButton"><a href="#clear" id="test1">Clear</a></li>--%>
                                        <%--   <asp:Button ID="Button4" runat="server" Text="Submit" OnClientClick ="return test2()" class="clearButton" />--%>

                                        <div class="sig sigWrapper">
                                            <div class="typed"></div>
                                            <canvas class="pad" width="220" height="100" id="thecanvas" style="background-color: white; border: 1px solid #dcdcdc;"></canvas>
                                            <input type="hidden" name="output" class="output" />
                                        </div>
                                        <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>

                                        <%--       <input type="button" id="btnSave" name="btnSave" value="Save the canvas to server"  />
           
                                        --%>
                                        <div class="sigNav">
                                            <div class="clearButton col-sm-1 no-padding">
                                                <a href="#clear" style="min-width: 105px;" class="btn btn-danger" id="test1">Clear</a>
                                            </div>
                                            <div class="col-sm-1">
                                                <asp:Button Style="min-width: 105px;" ID="btn_startSubmit" runat="server" Text="Submit" CssClass="btn btn-success" OnClientClick="return test()" OnClick="btn_startSubmit_Click" class="big-button" />
                                            </div>
                                            <div class="clearfix"></div>
                                        </div>
                                    </div>
                                </asp:View>
                            </div>
                            <div class="tab-pane" id="tab_2">
                                <asp:View ID="View2" runat="server">
                                    <span class="title-bar">Finish Times</span>
                                    <div id="endTimesContent" class="times-content" runat="server">
                                        <div class="table-responsive">
                                            <asp:GridView ID="GridView4" CssClass="table table-hover" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource2" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" DataKeyNames="RosterID">
                                                <Columns>
                                                    <asp:ImageField DataImageUrlField="Picture" HeaderText="Image" ReadOnly="true"></asp:ImageField>
                                                    <asp:BoundField DataField="FirstName" HeaderText="First Name" SortExpression="FirstName" />
                                                    <asp:BoundField DataField="LastName" HeaderText="Last Name" SortExpression="LastName" />
                                                    <asp:BoundField DataField="Shiftendtime" HeaderText="End Time" SortExpression="Shiftendtime" />
                                                    <asp:TemplateField HeaderText="Attendance">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkRow" Style="float: left; margin-right: 5px;" runat="server" />

                                                            <asp:TextBox ID="txt_time" runat="server" Style="width: 100px;" TextMode="Time" CssClass="form-control" OnTextChanged="txt_break_TextChanged"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Note">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txt_break" Style="min-width: 100px" runat="server" CssClass="form-control" TextMode="SingleLine" Max="200"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT tbl_Duty.RosterID, [tbl_worker].[Picture], [tbl_worker].[FirstName], [tbl_worker].[LastName], tbl_Shift.Shiftendtime, tbl_Shift.TotalTime FROM  tbl_Shift INNER JOIN tbl_Duty 
ON tbl_Shift.ShiftID=tbl_Duty.ShiftID
INNER JOIN [dbo].[tbl_worker] on [tbl_worker].[WorkersId]=[tbl_Duty].[WorkerID]
INNER JOIN [dbo].[tbl_Attendance] on  [dbo].[tbl_Attendance].[RosterID]=[dbo].[tbl_Duty].[RosterID]
 WHERE  [dbo].[tbl_Duty].[supervisorID]=@name And [dbo].[tbl_Duty].[Day]= @wday  And [dbo].[tbl_Attendance].[Start_time] is not null  order by rosterId ASC">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="" Name="wday" />
                                                    <asp:Parameter Name="name" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                        </div>
                                        <div id="endTimesEmpty" class="empty-text" runat="server">
                                            <h3>All worker times have been completed.</h3>
                                        </div>
                                    </div>
                                    <div class="drawIt">
                                        <h4>Sign Here</h4>
                                    </div>
                                    <div class="sig sigWrapper">
                                        <div class="typed"></div>
                                        <canvas class="pad" width="220" height="100" id="thecanvasend" style="background-color: white; border: 1px solid #dcdcdc;"></canvas>
                                        <input type="hidden" name="output" class="output" />
                                    </div>
                                    <%--<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>--%>

                                    <%--       <input type="button" id="btnSave" name="btnSave" value="Save the canvas to server"  />
           
                                    --%>
                                    <div class="sigNav">
                                        <div class="clearButton col-sm-1 no-padding">
                                            <a href="#clear" style="min-width: 105px;" class="btn btn-danger" id="test1">Clear</a>
                                        </div>
                                        <div class="col-sm-1">
                                            <asp:Button ID="Button1" Style="min-width: 105px;" runat="server" Text="Submit" CssClass="btn btn-success" OnClientClick="return test()" OnClick="Button3_Click" class="big-button" />
                                        </div>
                                        <div class="clearfix"></div>
                                    </div>
                                </asp:View>
                            </div>
                            <div class="tab-pane" id="tab_3">
                                <asp:View ID="View3" runat="server">
                                    <h3 class="title-bar">Add break</h3>
                                    <div class="table-responsive">
                                        <asp:GridView runat="server" CssClass="table table-hover" AutoGenerateColumns="False" DataSourceID="SqlDataSource3" ID="Gridview40" OnRowCommand="Gridview40_RowCommand">
                                            <Columns>
                                                <asp:ImageField HeaderText="Image" ReadOnly="true"></asp:ImageField>
                                                <asp:BoundField DataField="FirstName" ReadOnly="true" HeaderText="FirstName" SortExpression="FirstName" />
                                                <asp:BoundField DataField="LastName" HeaderText="LastName" ReadOnly="true" SortExpression="LastName" />
                                                <asp:TemplateField HeaderText="Leave Time">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_breakStart" Text='<%# Bind("starttime") %>' Style="width: 150px;" CssClass="form-control" runat="server" TextMode="time" OnTextChanged="txt_breakStart_TextChanged" AutoPostBack="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Return Time">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txt_breakEnd" Text='<%# Bind("endtime") %>' Style="width: 150px;" runat="server" CssClass="form-control" TextMode="time" leavwe="==" OnTextChanged="txt_breakEnd_TextChanged" AutoPostBack="false"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField HeaderText="Paid Break">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkpaid" runat="server" CssClass="form-control"></asp:CheckBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:HiddenField Value='<%# Eval("rosterid") %>' ID="rosterid" runat="server" />
                                                        <asp:Button ID="btnstart" CssClass="btn btn-default" runat="server" Text="Start" CommandName="Start" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />
                                                        <asp:Button ID="btnend" CssClass="btn btn-default" runat="server" Text="End" CommandName="End" CommandArgument="<%# ((GridViewRow) Container).RowIndex %>" />

                                                    </ItemTemplate>

                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand=" SELECT   [tbl_duty].[rosterid], [dbo].[tbl_worker].[FirstName],[dbo].[tbl_worker].[LastName], tbl_tempBreak.starttime AS starttime, tbl_tempbreak.endtime AS endtime FROM	[dbo].[tbl_worker]	Inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[WorkerID]=[dbo].[tbl_worker].[WorkersId]						inner join [dbo].[tbl_Attendance] on [dbo].[tbl_Attendance].[RosterID]=[dbo].[tbl_Duty].[RosterID]							INNER JOIN [dbo].[tbl_tempBreak] ON [dbo].[tbl_tempBreak].rosterid = [dbo].[tbl_Duty].rosterid WHERE	tbl_duty.supervisorid = @0 and		tbl_duty.day = @1 and		[dbo].[tbl_Attendance].[Start_time] is not null and		[dbo].[tbl_Attendance].[End_time] is null UNION  SELECT  [tbl_duty].[rosterid], [dbo].[tbl_worker].[FirstName],[dbo].[tbl_worker].[LastName], '' AS starttime, '' AS endtime FROM	[dbo].[tbl_worker]	Inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[WorkerID]=[dbo].[tbl_worker].[WorkersId]							inner join [dbo].[tbl_Attendance] on [dbo].[tbl_Attendance].[RosterID]=[dbo].[tbl_Duty].[RosterID]WHERE	tbl_duty.supervisorid = @0 and		tbl_duty.day = @1 and		[dbo].[tbl_Attendance].[Start_time] is not null and		[dbo].[tbl_Attendance].[End_time] is null and		[tbl_duty].[rosterid] not in (SELECT   [tbl_duty].[rosterid]  FROM	[dbo].[tbl_worker]	Inner join [dbo].[tbl_Duty] on [dbo].[tbl_Duty].[WorkerID]=[dbo].[tbl_worker].[WorkersId]							inner join [dbo].[tbl_Attendance] on [dbo].[tbl_Attendance].[RosterID]=[dbo].[tbl_Duty].[RosterID]							INNER JOIN [dbo].[tbl_tempBreak] ON [dbo].[tbl_tempBreak].rosterid = [dbo].[tbl_Duty].rosterid WHERE	tbl_duty.supervisorid = @0 and		tbl_duty.day = @1 and		[dbo].[tbl_Attendance].[Start_time] is not null and		[dbo].[tbl_Attendance].[End_time] is null)		">
                                            <SelectParameters>
                                                <asp:Parameter DefaultValue="null" Name="0" />
                                                <asp:Parameter DefaultValue="null" Name="1" />
                                            </SelectParameters>
                                        </asp:SqlDataSource>
                                    </div>
                                    <asp:Label runat="server" Text="Label" ID="lbl_showcell"></asp:Label>
                                </asp:View>
                            </div>
                        </asp:MultiView>
                    </div>
                </div>
            </div>
        </div>

        <script src="../js/jquery.signaturepad.js"></script>
        <script>
            $(document).ready(function () {
                $('.sigPad').signaturePad({ drawOnly: true });
            });
            moveIcon(1);
        </script>
        <script src="../js/json2.min.js"></script>
        <image id="embedImage"></image>
</asp:Content>



