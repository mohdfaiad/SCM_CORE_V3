<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingAWBFileUpload.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.BillingAWBFileUpload" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<style type="text/css">
    </style>
    <script language="javascript" type="text/javascript">

        function callexport() {


            window.open('showBillingExcel.aspx', 'Send');
        }

        function getPath() {
            alert(document.getElementById(HidPath).value);
        }
    </script>
 </asp:Content> 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>       
            <img src="Images/txt_billing.png" />
    </h1> 
   <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        </p>
  <div  class="botline"> 
  <table width=" 50%">
   <tr>
    <td>
        Select File to upload
    </td>
    <td>
        <asp:FileUpload ID="FileExcelUpload" runat="server" />
    </td>
    <td>
        <asp:Button ID="btnUpload" runat="server" CssClass="button" Text="Upload" 
            onclick="btnUpload_Click" OnClientClick="document.getElementById('HidPath').value = FileExcelUpload.PostedFile.FileName" />
    </td>
    <td>
        <asp:Button ID="btnMilestone" runat="server" CssClass="button" Text="Milestone" Visible="false" 
            onclick="btnMilestone_Click"/></td>
   </tr>
  </table>  
  </div> 
  <div id="fotbut">
            <table style="width: 930px;">
                <tr>
                    <td>
                        <asp:HiddenField ID="HidPath" runat="server" />
                    </td>
                </tr>
            </table>
            </div>
  </div>
</asp:Content> 