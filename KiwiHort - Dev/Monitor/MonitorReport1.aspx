<%@ Page Title="" Language="C#" MasterPageFile="~/Monitor/MonitorMaster.master" AutoEventWireup="true" CodeFile="MonitorReport1.aspx.cs" Inherits="Monitor_MonitorReport1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" runat="Server">


    <div class="row">
        <div class="col-md-12">
            <div class="nav-tabs-custom">
                <ul id="tab" class="nav nav-tabs">
                    <li class="active">
                        <asp:LinkButton ID="LinkButton2" OnClick="Button1_Click" runat="server">Farm Summary</asp:LinkButton></li>

                    <li class="active">
                        <asp:LinkButton ID="LinkButton3" OnClick="Button2_Click" runat="server">Work Summary</asp:LinkButton></li>
                    <li class="active">
                        <asp:LinkButton ID="LinkButton1" OnClick="btn_break_Click" runat="server"> Contractor Summary</asp:LinkButton></li>

                    <%-- <li class="active">
                        <asp:LinkButton ID="LinkButton4" OnClick="btn_break_Click" runat="server">Work Summary</asp:LinkButton></li>
                    <li class="active">
                        <asp:LinkButton ID="LinkButton5" OnClick="Button2_Click" runat="server"> Contractor Summary</asp:LinkButton></li>--%>
                </ul>

                <div class="tab-content">
                    <asp:MultiView ID="Mv1" runat="server" OnActiveViewChanged="MultiView1_ActiveViewChanged">
                        <div class="tab-pane active" id="tab_1">
                            <asp:View ID="View1" runat="server">
                                <h3 class="title-bar">Farm Summary</h3>
                                <div id="startTimesContent" class="times-content" runat="server">
                                    <div class="table-responsive">
                                        Select Farm:
                                        <asp:DropDownList ID="ddlselectfarm" runat="server" AutoPostBack="true"></asp:DropDownList>
                                        Select Block:
                                        <asp:DropDownList ID="ddlselectblock" runat="server"></asp:DropDownList>
                                        Select Contractor:
                                        <asp:DropDownList ID="ddlcontractor" runat="server"></asp:DropDownList>
                                        Select Supervisor:
                                        <asp:DropDownList ID="ddlsupervisor" runat="server"></asp:DropDownList>
                                        Start Date:<asp:TextBox ID="txtstartdate" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender ID="CEstartdate" runat="server" TargetControlID="txtstartdate"></asp:CalendarExtender>
                                        End Date:<asp:TextBox ID="txtenddate" runat="server"></asp:TextBox>
                                        <asp:CalendarExtender ID="CEenddate" runat="server" TargetControlID="txtenddate"></asp:CalendarExtender>
                                        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

                                        <br />
                                        <asp:Button ID="btn_search" runat="server" Text="Search" CssClass="btn btn-primary pull-right" />

                                    </div>
                                    <div>


                                      
                                    </div>
                                </div>
                            </asp:View>
                        </div>
                        <div class="tab-pane" id="tab_2">
                            <asp:View ID="View2" runat="server">
                                <span class="title-bar">Work Summary</span>
                                <div id="endTimesContent" class="times-content" runat="server">
                                    <div class="table-responsive">
                                    </div>

                                </div>


                            </asp:View>
                        </div>
                        <div class="tab-pane" id="tab_3">
                            <asp:View ID="View3" runat="server">
                                <h3 class="title-bar">Contractor Summary</h3>
                                <div class="table-responsive">
                                </div>

                            </asp:View>
                        </div>
                    </asp:MultiView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

