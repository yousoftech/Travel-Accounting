<%@ Page Title="" Language="C#" MasterPageFile="~/Supervisor/SupervisorMaster.master" AutoEventWireup="true" CodeFile="SupervisorMessage.aspx.cs" Inherits="SupervisorMessage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" Runat="Server">
        <asp:Button ID="Compose" runat="server" class="messageButtons" OnClick="Button1_Click" Text="Compose" />
        <asp:Button ID="Button2" runat="server" class="messageButtons" OnClick="Button2_Click" Text="Inbox" />
        <asp:Button ID="Button3" runat="server" class="messageButtons" OnClick="Button3_Click" Text="Sent" />
    <div>
    
        <asp:MultiView ID="MultiView1" runat="server">


            <script>

                hideIcon();

            </script>



             <asp:View ID="Tab2" runat="server">
                 <span class="title-bar">Compose Message</span>

                <div class="message-box-container">
                   
                    <asp:TextBox ID="txtto" runat="server" TextMode="Email" placeholder="To"></asp:TextBox>
                    
                    <asp:TextBox ID="txtsubject" runat="server" placeholder="Add a subject"></asp:TextBox>

                    <asp:TextBox ID="txtbody" runat="server"  TextMode="MultiLine" placeholder="Add a message"></asp:TextBox>
                    
                    <asp:Button ID="Button4" runat="server" OnClick="Button4_Click" Text="Send" />
                 </div>

                 
                


             </asp:View>






            <asp:View ID="View1" runat="server">
                <span class="title-bar">Inbox</span>
                <div class="inbox-sent-content">
                    <asp:GridView ID="GridView1" runat="server" DataSourceID="SqlDataSource1" ShowHeader="False"></asp:GridView>
                </div>
                
             </asp:View>















            <asp:View ID="View2" runat="server">
                <span class="title-bar">Sent</span>
                <div class="inbox-sent-content">
                    <asp:GridView ID="GridView2" runat="server" DataSourceID="SqlDataSource2" ShowHeader="False"></asp:GridView>
                </div>
             </asp:View>













       
        </asp:MultiView>
   
  
    </div>
        
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" ProviderName="<%$ ConnectionStrings:KiwihortData.ProviderName %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:KiwihortData %>" ProviderName="<%$ ConnectionStrings:KiwihortData.ProviderName %>"></asp:SqlDataSource>
</asp:Content>

