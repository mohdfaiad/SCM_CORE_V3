<%@ Page Title="Message Details" Language="C#" AutoEventWireup="true" CodeBehind="MessagePopUp.aspx.cs" Inherits="ProjectSmartCargoManager.MessagePopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callclose() {
            document.getElementById('msgLight_New').style.display = 'none';
            document.getElementById('msgFade_New').style.display = 'none';
        }

        function HidePanelSplit() {
            document.getElementById('Lightsplit').style.display = 'none';
            document.getElementById('fadesplit').style.display = 'none';
        }
    </script>
    
</head>
<body style="background:#FFFFFF;">
    <form id="form1" runat="server">
    <div style="margin:6px;"> 
     <asp:Label ID="LblMsgCnt" runat="server" Text="Message Content" Font-Bold="True" 
            Font-Size="Medium"></asp:Label>
    </div>
    
    <div>
       <asp:Label ID="lblStatus" runat="server" Text="" Font-Bold="True"></asp:Label>
    </div>
    
    <br />
    <asp:Label ID="lblEmailId" runat="server" Text="Email Id" Font-Bold="True"></asp:Label>
    <div style="margin:6px;">
      <asp:TextBox ID="txtEmailId" runat="server" TextMode="MultiLine" 
      ReadOnly="true" Width="365px" Height="40px" BorderColor="Gray" style="OVERFLOW:auto" ></asp:TextBox>
    </div>
    
    <asp:Label ID="lblMsgbody" runat="server" Text="Message Body" Font-Bold="True"></asp:Label>
    
    <div style="margin:6px;">
    <asp:TextBox ID="txtMessageBody" runat="server" TextMode="MultiLine" ReadOnly="true"
     Width="365px" Height="300px" BorderColor="Gray" style="OVERFLOW:auto"></asp:TextBox>
     
    </div>
    
    <div style="margin:6px;">
   <%-- <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClientClick="javascript:SaveDescription();" />--%>
     <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button" 
            onclick="btnDelete_Click" />
     <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="button" />
     <asp:Button ID="btnSendViaSITA" runat="server" Text="SendViaSITA" CssClass="button" />
     <asp:Button ID="btnClose" runat="server" Text="Close" CssClass="button" />--%>
     
     <table width = "30%">
    <tr>
    <td>
       <asp:Button ID="btnMsgDelete" runat="server" class="button" Text="Delete" Visible="false"
            OnClick="btnDelete_Click" CausesValidation="False"/>                

    </td>
    <td>
        <asp:Button ID="btnProcess" runat="server" class="button" Text="Re-Send" Visible="false"
             CausesValidation="False" onclick="btnProcess_Click"/>

    </td>
    <td>
       <asp:Button ID="btnEdit" runat="server" class="button" Text="Edit" 
             CausesValidation="False" onclick="btnEdit_Click"/>
    </td>
     <td>
        <asp:Button ID="btnSitaUpload" CssClass="button" runat="server" 
             Text="Send via SITA" onclick="btnSitaUpload_Click"/>
    </td>
    <td>
       <asp:Button ID="btnClose" runat="server" class="button" Text="Close" 
             CausesValidation="False" onclick="btnClose_Click"/>

    </td>
    
    </tr>
    </table>
    
    </div>
    </form>
</body>
</html>
