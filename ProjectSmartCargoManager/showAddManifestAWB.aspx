<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showAddManifestAWB.aspx.cs" Inherits="ProjectSmartCargoManager.showAddManifestAWB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    
<script language="javascript" type="text/javascript">

function PassValues()
{
    window.opener.document.forms(0).submit();
    self.close();
}

//function close() {
//    alert("Test");
//    window.opener.location.href ='localhost:64813/frmExportManifest.aspx?fromChild=1';
//    self.close();
//}

function callexport() {

  //  alert("Test");
    window.opener.location.href = '//frmExportManifest.aspx?fromChild=1';
    self.close();
}
</script>
    
     <style type="text/css">
        .divback
        {
            background: url(images/divback.png) repeat-x scroll left bottom;
            border: 1px solid #d2cfca;
            border-radius: 6px;
            padding: 10px;
            margin: 0px;
            width: 546px;
            height: 76px;
        }
        .divgrd
        {
            overflow: scroll;
        }
        .titlecolr
        {
            background: #36a3f8;
            color: #ffffff;
            line-height: 20px;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 14px;
        }
        .button
        {
            background: url(images/buton.png) repeat-x;
            border-radius: 6px;
            color: #FFFFFF;
            font-weight: bold;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 13px;
        }
        .button:hover
        {
            background: url(images/butin.png) repeat-x;
            border-radius: 6px;
            color: #FFFFFF;
            font-weight: bold;
            font-family: Calibri, Arial, Helvetica, sans-serif;
            font-size: 13px;
        }
        .buttonSearch
        {
            background: none;
        }
        .botline
        {
            border-bottom: 1px solid #a9acb0;
            padding-bottom: 6px;
            float: left;
            width: 718px;
            padding-top: 6px;
        }
         </style>

   <%-- <script type="text/javascript">

        function CloseWindow(selectedval,id) {

            opener.document.getElementById('<%= Request["TargetTXT"] %>').value = '' + selectedval;
            opener.document.getElementById('<%= Request["Hid"] %>').value = '' + id;
            window.close();
        }  
    
    </script>--%>
    
</head>
<body>
    <form id="form1" runat="server">
     <br />
    <div style="font-size: medium">
        AWBs</div>
    <br />
    <div class="divgrd" style="width: 355px; height: 260px">
        <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Auto"  >
        <asp:GridView ID="grdAWBs" runat="server" AutoGenerateColumns="False"
            ShowFooter="True" Width="82%" Height="64%">
            <Columns>
                <asp:TemplateField HeaderText="AWBNo">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtAWBno" runat="server"  Width="80px" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Pieces">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtPCS" runat="server" MaxLength="4" Width="55px"  AutoPostBack="True" OnTextChanged="txtPcs_TextChanged"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Weight">
                 
                    <ItemTemplate>
                        <asp:TextBox ID="txtweight" runat="server"  MaxLength="4" Enabled="false"
                            Width="55px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AvlPCS">
                   
                    <ItemTemplate>
                        <asp:TextBox ID="txtAvlPCS" runat="server" Width="55px" Enabled="false" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="AvlWgt">
                    
                    <ItemTemplate>
                        <asp:TextBox ID="txtAwlWeight" runat="server" Width="55px" Enabled="false"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                
                
            </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
            <AlternatingRowStyle HorizontalAlign="Center" />
        </asp:GridView>
        
        
        <table width="100%">
        <tr>
        <td>
         <asp:Button ID="btnAddManifest" runat="server" Text="Add To Manifest" CssClass="button" OnClick="btnAddManifest_Click">
        </asp:Button>
       
<%--            <Button ID="Button1"  OnClientClick=="javascript:PassValues();" Text="Button" />
--%>          
                  <asp:Button ID="btnShowEAWB" runat="server" Text="Click Me" CssClass="button" OnClientClick="callexport();"
            OnClick="btnShowEAWB_Click" />
        </td>
            <td>
             <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>
        </td>
        
        </tr>
        <tr>
        <td colspan="2">
            <asp:Label ID="LBLStatus" runat="server"></asp:Label>
        </td>
        </tr>
        </table>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
