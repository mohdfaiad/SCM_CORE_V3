<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ShowULDList.aspx.cs" Inherits="ProjectSmartCargoManager.ShowULDList" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ULD</title>

    <script language="javascript" type="text/javascript">
        
    </script>

</head>
<body class="divback">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ToolkitScriptManager>
    <div style="width: 750px; padding-left: 20px;">
    <br />
           <asp:Label ID="LBLStatus" runat="server" Text="" Font-Bold="true" Font-Size="Small"></asp:Label>
        <br />
        
        <asp:UpdatePanel runat="server" ID="UPDimension">
            <ContentTemplate>
             
       <div style="overflow:auto;height:200px; width:750px; border:solid 1px #ccc;">
       <table>
       <tr>
       <td>ULD Type</td>
       <td>
       <asp:DropDownList runat="server" ID="ddlULDType"></asp:DropDownList>
       </td>
       <td>ULD Status</td>
       <td>
       <asp:DropDownList runat="server" ID="ddlULDStatus"></asp:DropDownList>
       </td>
       <td>ULD Use Status</td>
       <td>
       <asp:DropDownList runat="server" ID="ddlULDUseStatus"></asp:DropDownList>
       </td>
       <td>
       <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
               onclick="btnList_Click" />
       </td>
       </tr>
       </table>
       <asp:GridView ID="grdULDList" runat="server" AutoGenerateColumns="true" Width="100%">
      <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
      </asp:GridView>
      
                </div>
        <asp:GridView ID="grdULDSummaryList" runat="server" AutoGenerateColumns="true" Width="150px">
      <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
      </asp:GridView>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
            onclick="btnExport_Click">
        </asp:Button>
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClientClick="javascript:window.close();">
        </asp:Button>
    </div>
    </form>
</body>
</html>
