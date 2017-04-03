<%@ Page Title="" Language="C#" MasterPageFile="ContractorMaster.master" AutoEventWireup="true" CodeFile="ContractorOrganiser.aspx.cs" Inherits="ContractorOrganiser" %>

<asp:Content ContentPlaceHolderID="cph_mainSection" runat="server">
    <div class="row">
        <div class="col-lg-3">
            <div class="box">
                <div class="box-header">
                    Worker Assignment
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <label>Workers:</label>

                    <asp:CheckBoxList ID="chk_workers" runat="server">
                    </asp:CheckBoxList>
                    <asp:SqlDataSource ID="sds_workers" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT [tbl_worker].[FirstName], [tbl_worker].[LastName], [tbl_worker].[WorkersId], [tbl_login].[type] FROM [tbl_worker] INNER JOIN [tbl_login] ON [tbl_login].[Id]=[tbl_worker].[WorkersId] ORDER BY [tbl_worker].[FirstName]"></asp:SqlDataSource>
                </div>
            </div>
            <div class="box">
                <div class="box-header">
                    Supervisor Assignment
                </div>
                <!-- /.box-header -->
                <div class="box-body">
                    <asp:DropDownList ID="cbo_workerAssignDay" runat="server" CssClass="form-control" OnSelectedIndexChanged="cbo_workerAssignDay_SelectedIndexChanged" AutoPostBack="true">
                    </asp:DropDownList><br />
                    <asp:DropDownList ID="cbo_workerAssignFarm" CssClass="form-control" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cbo_workerAssignFarm_SelectedIndexChanged">
                    </asp:DropDownList><br />
                    <asp:DropDownList ID="cbo_workerAssignSupervisor" CssClass="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbo_workerAssignSupervisor_SelectedIndexChanged">
                        <asp:ListItem>Select Supervisor</asp:ListItem>
                    </asp:DropDownList><br />
                    <asp:CheckBoxList ID="chk_workersForSupervisors" Style="min-height: 88px; overflow-y: auto;"
                        CssClass="form-control" runat="server">
                    </asp:CheckBoxList><br />
                    <asp:SqlDataSource ID="sds_workersForSupervisors" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT [tbl_worker].[FirstName], [tbl_worker].[LastName], [tbl_worker].[WorkersId], [tbl_login].[type] FROM [tbl_worker] INNER JOIN [tbl_login] ON [tbl_login].[Id]=[tbl_worker].[WorkersId] INNER JOIN [tbl_duty] ON [tbl_Duty].[workerId]=[tbl_worker].[WorkersId] INNER JOIN [tbl_Shift] ON [tbl_Shift].[ShiftID]=[tbl_duty].[ShiftId]"></asp:SqlDataSource>

                    <asp:Button ID="btn_workerAssign" runat="server" CssClass="btn btn-primary pull-right" Text="Assign" OnClick="btn_workerAssign_Click" />
                </div>
            </div>
        </div>
        <div class="col-sm-9">
            <div class="box">
                <div class="box-header">
                    <div class="row">
                        <div class="col-sm-3">
                            <h3 class="box-title">Week Start Date:</h3>
                        </div>
                        <div class="col-sm-3">
                            <div class="input-group date" style="width: 150px;">
                                <div class="input-group-addon"><i class="fa fa-calendar"></i></div>
                                <asp:DropDownList ID="cbo_weekStart" CssClass="form-control pull-right" runat="server" AutoPostBack="True" OnSelectedIndexChanged="cbo_weekStart_SelectedIndexChanged"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <asp:Button ID="btn_submit" class="btn btn-primary pull-right" runat="server" OnClick="btn_submit_Click" Text="Submit" />
                        </div>
                    </div>
                </div>
                <!-- /.box-header -->
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
                                    <asp:TextBox ID="txt_mondayStart" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox>
                                    <td>
                                        <asp:TextBox ID="txt_tuesdayStart" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_wednesdayStart" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_thursdayStart" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_fridayStart" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_saturdayStart" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_sundayStart" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>End Time</td>
                                <td>
                                    <asp:TextBox ID="txt_mondayEnd" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox>
                                    <td>
                                        <asp:TextBox ID="txt_tuesdayEnd" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_wednesdayEnd" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_thursdayEnd" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_fridayEnd" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_saturdayEnd" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                                    <td>
                                        <asp:TextBox ID="txt_sundayEnd" runat="server" CausesValidation="True" CssClass="form-control" TextMode="Time"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Farm</td>
                                <td>
                                    <asp:DropDownList ID="cbo_mondayFarm" class="farm-name" runat="server" CssClass="form-control" CausesValidation="true"></asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="cbo_tuesdayFarm" class="farm-name" runat="server" CssClass="form-control" CausesValidation="true"></asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="cbo_wednesdayFarm" class="farm-name" runat="server" CssClass="form-control" CausesValidation="true"></asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="cbo_thursdayFarm" class="farm-name" runat="server" CssClass="form-control" CausesValidation="true"></asp:DropDownList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT[ShiftID],[Shiftstarttime],[ShiftendTime],[TotalTime],[Farm_Name] FROM tbl_Shift inner join[dbo].[tbl_farms] on[dbo].[tbl_farms].[FarmId]=[dbo].[tbl_Shift].[farmId] WHERE ShiftID = @0">
                                        <SelectParameters>
                                            <asp:Parameter Name="0" />
                                        </SelectParameters>
                                    </asp:SqlDataSource>
                                </td>
                                <td>
                                    <asp:DropDownList ID="cbo_fridayFarm" class="farm-name" runat="server" CssClass="form-control" CausesValidation="true"></asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="cbo_saturdayFarm" class="farm-name" runat="server" CssClass="form-control" CausesValidation="true"></asp:DropDownList></td>
                                <td>
                                    <asp:DropDownList ID="cbo_sundayFarm" class="farm-name" runat="server" CssClass="form-control" CausesValidation="true"></asp:DropDownList></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="box">
                <div class="box-header">
                    <div class="row">
                        <div class="col-sm-2">
                            <h3 class="box-title">Worker Viewing</h3>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="cbo_weekStart2" CssClass="form-control" runat="server" AutoPostBack="false" OnSelectedIndexChanged="cbo_weekStart2_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-3">
                            <asp:DropDownList ID="cbo_farmWorker" CssClass="form-control" runat="server" AutoPostBack="false" OnSelectedIndexChanged="cbo_weekStart2_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-sm-2">
                            <asp:TextBox ID="txt_search" runat="server" CssClass="form-control" placeholder="Worker Name"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button ID="btn_search" runat="server" Text="Search" OnClick="btn_search_Click" CssClass="btn btn-primary pull-right" />
                        </div>
                    </div>
                </div>
                <!-- /.box-header -->
                <div class="box-body table-responsive no-padding">
                    <asp:GridView ID="dgd_organiserWorkers" runat="server" AutoGenerateSelectButton="True" OnSelectedIndexChanged="dgd_organiserWorkers_SelectedIndexChanged" CssClass="table table-hover">
                    </asp:GridView>
                </div>
                <div class="box-footer clearfix">
                    <asp:Label ID="lbl_noContent" runat="server" Visible="false" Text="Your search has returned no results"></asp:Label>
                </div>
            </div>
        </div>
    </div>
    <asp:MultiView ID="MultiView1" runat="server">
    </asp:MultiView>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:HiddenField ID="hdf_flag" runat="server" />
</asp:Content>
