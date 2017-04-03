<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Admin_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="admin.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" SelectCommand="GetWorker" SelectCommandType="StoredProcedure" UpdateCommand="UpdateVisa" UpdateCommandType="StoredProcedure">
            <UpdateParameters>
                <asp:Parameter Name="WorkersId" Type="String" />
                <asp:Parameter Name="Type" Type="String" />
            </UpdateParameters>
         

        </asp:SqlDataSource>
        <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />
        <br />
        <asp:GridView ID="GridView1" runat="server" CssClass="mydatagrid" PagerStyle-CssClass="pager"
 HeaderStyle-CssClass="header" AllowPaging="True"  RowStyle-CssClass="rows" AutoGenerateColumns="False" DataKeyNames="WorkersId" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="FirstName" HeaderText="FirstName" SortExpression="FirstName"  />
                <asp:BoundField DataField="WorkersId" HeaderText="WorkersId"  SortExpression="WorkersId" />
                <asp:BoundField DataField="LastName" HeaderText="LastName" SortExpression="LastName"  />
                <asp:BoundField DataField="email" HeaderText="email" SortExpression="email"  />
                <asp:ImageField  DataImageUrlField="Picture" HeaderText="Picture" SortExpression="Picture"   ControlStyle-CssClass="grideview_profilepic">
<ControlStyle CssClass="grideview_profilepic"></ControlStyle>
                </asp:ImageField>
                <asp:BoundField DataField="Visanumber" HeaderText="Visanumber" SortExpression="Visanumber" />
                <asp:BoundField DataField="StartDate" HeaderText="StartDate" SortExpression="StartDate" DataFormatString="{0:d}" />
                <asp:BoundField DataField="ExpireDate" HeaderText="ExpireDate" SortExpression="ExpireDate" DataFormatString="{0:d}" />
                <asp:CheckBoxField DataField="Validity" HeaderText="Validity" SortExpression="Validity" />
                <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
            </Columns>
            <HeaderStyle CssClass="header" />
            <PagerStyle CssClass="pager" />
            <RowStyle CssClass="rows" />
        </asp:GridView>
    </form>
</body>
</html>
