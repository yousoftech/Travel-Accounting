<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Monitor/MonitorMaster.master" CodeFile="MonitorProfile.aspx.cs" Inherits="Monitor_MonitorProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" runat="Server">
    <style type="text/css">
        .box-body input {
            margin-bottom: 15px;
        }

        .fileContainer {
            overflow: hidden;
            position: relative;
        }

            .fileContainer [type=file] {
                cursor: inherit;
                display: block;
                filter: alpha(opacity=0);
                min-height: 100%;
                min-width: 100%;
                opacity: 0;
                position: absolute;
                right: 0;
                text-align: right;
                top: 0;
            }


        .fileContainer {
            float: left;
            background: -webkit-linear-gradient(top, #f9f9f9, #e3e3e3);
            border: 1px solid #999;
            border-radius: 3px;
            padding: 5px 8px;
            outline: none;
            white-space: nowrap;
            -webkit-user-select: none;
            cursor: pointer;
            text-shadow: 1px 1px #fff;
            font-weight: 700;
            font-size: 10pt;
            margin-bottom: 18px;
        }

            .fileContainer [type=file] {
                cursor: pointer;
            }

        }
    </style>
    <section class="content">
        <div class="row">
            <div class="col-sm-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Profile Setting</h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>First Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_firstName" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Middle Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_middleName" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Last Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_lastName" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Email</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_email" class="form-control" runat="server" CausesValidation="True" TextMode="Email"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Address 1</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_address1" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Address 2</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_address2" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>City</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_city" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Region</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_region" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Post Code</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="txt_postcode" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-5 col-sm-2">
                                        <label>Profile Picture</label>
                                    </div>
                                    <div class="col-xs-5 col-sm-3">
                                        <label class="fileContainer">
                                            Select File
    <asp:FileUpload ID="fup_picture" runat="server" />
                                        </label>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="You can only upload JPG files." ControlToValidate="fup_picture" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|png|PNG)$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-xs-12 col-sm-3">
                                        <asp:Button ID="btn_submit" runat="server" class="btn btn-success" OnClick="btn_submit_Click" Text="Submit" />
                                        <asp:Button ID="btn_cancel" class="btn btn-danger" runat="server" OnClick="btn_cancel_Click" Text="Cancel" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <%--        txt_code starts here--%>
        <%--<div class="row">
            <div class="col-sm-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <div class="row">
                            <div class="col-lg-2">
                                <h3 class="box-title">Code for your Workers</h3>
                            </div>
                            <div class="col-lg-3">
                                <asp:TextBox ReadOnly="true" ID="txt_code" class="form-control" runat="server" CausesValidation="True" onfocus="this.select();" AutoPostBack="True" OnTextChanged="txt_firstName_TextChanged"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
        <%--        txt_code ends here--%>

        <%--        Add/update farms starts here      --%>

        <div class="row" id="form-2">
            <div class="col-sm-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <div class="row">
                            <div class="col-sm-3">
                                <h3 class="box-title">View/Update Farm Details</h3>
                            </div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="cbo_selectFarm" CssClass="form-control" runat="server" DataSourceID="SqlDataSource1" DataTextField="Farm_Name" DataValueField="FarmId">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT * FROM tbl_farms"></asp:SqlDataSource>

                            </div>
                            <div class="col-sm-3">
                                <asp:Button ID="btnEditFarm" runat="server" Text="Edit" CssClass="btn btn-default" OnClick="Button1_Click" />
                            </div>
                            <div class="col-sm-2">
                            </div>
                            <div class="col-sm-2">
                            </div>
                        </div>

                    </div>
                    <div class="box-body table-responsive no-padding">
                        <asp:GridView ID="GridView1" CssClass="table table-hover" runat="server" AutoGenerateColumns="False" DataSourceID="sds_farms">
                            <Columns>
                                <asp:BoundField DataField="Farm_Name" HeaderText="Farm_Name" SortExpression="Farm_Name" />
                                <asp:BoundField DataField="Address1" HeaderText="Address1" SortExpression="Address1" />
                                <asp:BoundField DataField="Address2" HeaderText="Address2" SortExpression="Address2" />
                                <asp:BoundField DataField="City" HeaderText="City" SortExpression="City" />
                                <asp:BoundField DataField="Region" HeaderText="Region" SortExpression="Region" />
                                <asp:BoundField DataField="postcode" HeaderText="postcode" SortExpression="postcode" />
                            </Columns>
                        </asp:GridView>
                        <asp:SqlDataSource ID="sds_farms" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="select [dbo].[tbl_farms].[Farm_Name] , [dbo].[tbl_address].[Address1],[dbo].[tbl_address].[Address2],[dbo].[tbl_address].[City],[dbo].[tbl_address].[Region], [dbo].[tbl_address].[postcode] from [dbo].[tbl_address] 

inner join [dbo].[tbl_farms] on [dbo].[tbl_farms].[FarmId]=[dbo].[tbl_address].[AddressId]"></asp:SqlDataSource>
                    </div>
                </div>
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Add Farm Details</h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-xs-5 col-sm-2">
                                <label>Farm Name</label>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <asp:TextBox ID="txt_farmName" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <label>Farm Address</label>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <asp:TextBox ID="txt_addressFarm" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <label>Farm Address2</label>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <asp:TextBox ID="txt_address2Farm" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <label>City</label>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <asp:TextBox ID="txt_cityFarm" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <label>Region</label>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <asp:TextBox ID="txt_regionFarm" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <label>Post Code</label>
                            </div>
                            <div class="col-xs-5 col-sm-2">
                                <asp:TextBox ID="txt_postcodeFarm" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                            </div>
                            <div class="col-xs-7 col-sm-2 center-block">
                                <asp:Button ID="btn_submitFarm" Style="min-width: 160px;" runat="server" Text="Submit Farm Details" class="btn btn-primary" OnClick="btn_submitFarm_Click" />
                            </div>
                            <div class="col-xs-7 col-sm-2 center-block">
                                <asp:Button ID="Button2" runat="server" Style="min-width: 160px;" OnClick="btn_cancel_Click" class="btn btn-danger" Text="Return without Saving" />
                            </div>
                            <%--  <div class="col-xs-7 col-sm-2 center-block">
                                <asp:Button ID="btn_code" runat="server" OnClick="btn_code_Click" class="btn btn-success" Text="Generate Code" />
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--        Add/update farms ends here--%>


        <%--        Add contractor starts here--%>

        <%--        <div class="row">
            <div class="col-sm-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <h3 class="box-title">Add Contractor Details</h3>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>First Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox1" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Middle Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox2" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Last Name</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox3" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Email</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox4" class="form-control" runat="server" CausesValidation="True" TextMode="Email"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Address 1</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox5" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Address 2</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox6" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <div class="row">
                                    <div class="col-xs-6 col-sm-6">
                                        <label>City</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox7" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Region</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox8" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <label>Post Code</label>
                                    </div>
                                    <div class="col-xs-6 col-sm-6">
                                        <asp:TextBox ID="TextBox9" class="form-control" runat="server" CausesValidation="True"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="row">
                                    <div class="col-xs-5 col-sm-2">
                                        <label>Profile Picture</label>
                                    </div>
                                    <div class="col-xs-5 col-sm-2">
                                        <label class="fileContainer">
                                            Select File
    <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </label>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="You can only upload JPG files." ControlToValidate="fup_picture" ValidationExpression="^.*\.(jpg|JPG|jpeg|JPEG|png|PNG)$"></asp:RegularExpressionValidator>
                                    </div>
                                    <div class="col-xs-3 col-sm-2">
                                        <asp:Button ID="btn_submitecontractor" runat="server" class="btn btn-danger" OnClick="btn_submit_Click" Text="Update Profile Settings" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>

        <%--       Add contractor ends here--%>

        <%--        View/Update Block code starts here--%>

        <div class="box box-danger">
            <div class="box-header with-border">
                <div class="row">
                    <div class="col-sm-3">
                        <h3 class="box-title">View/Update Block Details</h3>
                    </div>
                    <div class="col-sm-3">
                        <asp:DropDownList ID="DropDownList2" CssClass="form-control" runat="server" DataSourceID="SqlDataSource2" DataTextField="Block_Name" DataValueField="Block_Name">
                        </asp:DropDownList>
                        <%-- <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="SELECT * FROM tbl_blocks"></asp:SqlDataSource>--%>


                        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT [Block_Name] FROM [tbl_blocks]"></asp:SqlDataSource>
                        <br />

                    </div>
                    <%-- <div class="col-lg-2">
                        <asp:TextBox ID="txtblock" CssClass="form-control" runat="server"></asp:TextBox><br />
                    </div>--%>
                    <div class="col-sm-3">
                        <asp:Button ID="btneditblock" runat="server" Text="Edit" CssClass="btn btn-default" OnClick="btneditblock_Click" />
                    </div>
                    <div class="col-sm-2">
                    </div>
                    <div class="col-sm-2">
                    </div>
                </div>

            </div>
            <div class="box-body table-responsive no-padding">
                <asp:GridView ID="GridView3" CssClass="table table-hover" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSourceEditBlock" OnRowEditing="GridView1_RowEditing" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ID">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ID" runat="server" Text='<%#Eval("BlockId") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--  <asp:BoundField DataField="BlockId" HeaderText="BlockId" ReadOnly="True" InsertVisible="False" SortExpression="BlockId"></asp:BoundField>--%>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lbl_blockName" runat="server" Text='<%#Eval("Block_Name") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_blockName" runat="server" Text='<%#Eval("Block_Name") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="Block_Name" HeaderText="Block_Name" SortExpression="Block_Name"></asp:BoundField>--%>
                       <%-- <asp:BoundField DataField="FarmId" HeaderText="FarmId" SortExpression="FarmId" ReadOnly="true"></asp:BoundField>--%>
                        <asp:TemplateField HeaderText="City">
                            <ItemTemplate>
                                <asp:Label ID="lbl_farmid" runat="server" Text='<%#Eval("FarmId") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_farmid" runat="server" Text='<%#Eval("FarmId") %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

                <asp:SqlDataSource runat="server" ID="SqlDataSourceEditBlock" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT * FROM [tbl_blocks]" UpdateCommand="Update tbl_blocks set Block_Name='block9',FarmId='farmid123456' where BlockId='99999999'" ></asp:SqlDataSource>
            </div>
        </div>

        <%--        View/Update Block Code Ends Here--%>



        <%--Add Block starts here--%>
        <div class="row">
            <div class="col-sm-12">
                <div class="box box-danger">
                    <div class="box-header with-border">
                        <div class="row">
                            <div class="col-lg-3">
                                <h3 class="box-title">Add Block Details</h3>
                            </div>
                            <div class="col-lg-2">

                                <asp:TextBox ID="txtblockname" runat="server" CssClass="form-control"></asp:TextBox><br />
                                <%-- <asp:DropDownList ID="DropDownList1" CssClass="form-control" runat="server" DataSourceID="SqlDataSource3" DataTextField="Block_Name" DataValueField="Block_Name">
                                </asp:DropDownList>
                            
                                <asp:SqlDataSource runat="server" ID="SqlDataSource3" ConnectionString='<%$ ConnectionStrings:KiwihortData %>' SelectCommand="SELECT [Block_Name] FROM [tbl_blocks]"></asp:SqlDataSource>
                                <br />--%>
                            </div>
                            <div class="col-lg-2">
                                <asp:TextBox ID="txtfarmid" CssClass="form-control" runat="server"></asp:TextBox><br />
                            </div>
                            <div class="col-lg-2">
                                <asp:Button ID="btnAddBlock" runat="server" class="btn btn-warning" Text="Add Block" OnClick="Button2_Click" />
                            </div>

                        </div>

                    </div>


                </div>
            </div>
        </div>

        <%--Addd block ends here--%>


        <%--View/Update Block Details--%>



        <%--View/Update Block Details--%>
    </section>
</asp:Content>
