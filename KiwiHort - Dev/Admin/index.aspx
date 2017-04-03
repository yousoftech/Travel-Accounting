<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/WorkerMaster.master" AutoEventWireup="true" CodeFile="index.aspx.cs" Inherits="Admin_index" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph_mainSection" Runat="Server">

    <div>  
        <h4>  
            Article for C#Corner  
        </h4>  
        <table>  
            <tr>  
                <td>  
                    Select File  
                </td>  
                <td>  
                    <asp:FileUpload ID="FlUploadcsv" runat="server" />  
                </td>  
                <td>  
                </td>  
                <td>  
                    <asp:Button ID="Button1" runat="server" Text="Upload" OnClick="Button1_Click" />  
                </td>  
            </tr>  
        </table>  

</asp:Content>

