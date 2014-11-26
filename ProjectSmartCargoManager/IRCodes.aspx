<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master"  Title="Irregularity Codes" CodeBehind="IRCodes.aspx.cs" Inherits="ProjectSmartCargoManager.IRCodes" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 </asp:Content>
 
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 
  <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  
    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
        function check() {
            var ddlvalue = document.getElementById('<%=ddlExpImp.ClientID%>').value;
            if (ddlvalue == 'All') {
                alert('Select Export or Import');
                return false;
            }
            else
                return true;
        }
     </script>
    
    <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 1000px;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .black_overlaymsg
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: White;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_contentmsg
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 5%;
            height: 5%;
            padding: 16px;
            background-color: Transparent;
            z-index: 1002;
        }
        .style1
        {
            height: 36px;
        }
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
  <div id="contentarea">
   <h1> 
         Irregularity Codes
         </h1>
         
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
          
 
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="100%" border="0">
<tr>
<td class="style1">
Irregularity Code*
</td>
    
<td class="style1">
<asp:TextBox ID="txtIRCode" runat="server"></asp:TextBox>
</td>
<td class="style1">
Description*
</td>
<td class="style1">
<asp:TextBox ID="txtIRDes" runat="server"></asp:TextBox></td>

<td class="style1">
Export-Import 
</td>
<td class="style1">
    <asp:DropDownList ID="ddlExpImp" runat="server">
    <asp:ListItem Text="Select" Value="All"></asp:ListItem>
    <asp:ListItem Text="Export" Value="E"></asp:ListItem>
    <asp:ListItem Text="Import" Value="I"></asp:ListItem>
    </asp:DropDownList> 
</td>

<td class="style1">
Active
</td>

<td class="style1">
    <asp:CheckBox ID="chkActive" runat="server" />
</td>
<td class="style1">
</td>
            <td class="style1">
                
            </td>
        </tr>
    </caption>

    <tr>
        <td colspan="9">
            <asp:Button ID="btnSave" runat="server" CssClass="button" 
                onclick="btnSave_Click" OnClientClick="javascript:return check()" Text="Save" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnClear" runat="server" CssClass="button" 
                onclick="btnClear_Click" Text="Clear" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnList" runat="server" CssClass="button" 
                onclick="btnList_Click" Text="List" />
        </td>
        <td>
            &nbsp;</td>
    </tr>

</table></div>   
</asp:Panel>  
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%">
<asp:GridView ID="grvCommodityList" runat="server" ShowFooter="false" Width="80%" AutoGenerateColumns="False"
CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" onrowcommand="grvCommodityList_RowCommand" 
onpageindexchanging="grvCommodityList_PageIndexChanging" onrowediting="grvCommodityList_RowEditing"
AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
           
            <Columns>
             <asp:TemplateField HeaderText="Serial No" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSerialNo" runat="server" Text = '<%# Eval("SerialNumber") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Irregularity Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblIRCode" runat="server" Text = '<%# Eval("IrregularityCode") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Irregularity Description" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblIRCDesc" runat="server" Text = '<%# Eval("IrregularityDescrp") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStat" runat="server" Text = '<%# Eval("IsActive") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Exp-Imp" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblExpImp" runat="server" Text = '<%# Eval("ExpImp") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
            <asp:ButtonField CommandName="Edit" Text="Edit">
            <ItemStyle Width="50px" />
            </asp:ButtonField>
            </Columns>
    </asp:GridView>
    
    </div>
</asp:Panel>
  </div>
  
  <div id="msglight" class="white_contentmsg">
        <table>
            <tr>
                <td width="5%" align="center">
                    <br />
                    <img src="Images/loading.gif" />
                    <br />
                    <asp:Label ID="msgshow" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="msgfade" class="black_overlaymsg">
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
  
</asp:Content>
