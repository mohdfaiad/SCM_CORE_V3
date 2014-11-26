<%@ Page Title="RegionMaster" Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="RegionMaster.aspx.cs" Inherits="ProjectSmartCargoManager.RegionMaster" %>

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
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
    <div id="contentarea">
    <div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
   <h1>Region Master</h1>
            
 <div class="botline">
<asp:Panel ID="pnlNew" runat="server">
<table width="100%" border="0" cellpadding="3" cellspacing="3">
<tr>
<td>
Region Code*
</td>
    
<td>
<asp:TextBox ID="txtRegionCode" runat="server"></asp:TextBox></td>
<td>
Region Name*
</td>
<td>
    <asp:TextBox ID="txtRegionName" runat="server"></asp:TextBox>
    </td>
    
<td>
Country Code*
</td>
<td>
    <asp:DropDownList ID="ddlCountryCode" runat="server" AutoPostBack="false" onselectedindexchanged="ddlCountryCode_SelectedIndexChanged">
    </asp:DropDownList>
</td>
<%--<td>
Country Name
</td>--%>
<td>
 <asp:TextBox ID="txtCountryName" runat="server" Enabled="False" Visible="false"></asp:TextBox>
</td>

<td>
Active
</td>
<td>
    <asp:CheckBox ID="chkActive" runat="server" />
</td>
</tr>
<tr>
 <td colspan="10">
  <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" onclick="btnSave_Click" />
  <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" onclick="btnClear_Click"/>
  <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click" />
  <asp:Button ID="btnExport" runat="server" CssClass="button" Text="Export" onclick="btnExport_Click" />
</td>
</tr>
</table>
</asp:Panel>  
</div>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%">

<asp:GridView ID="grvRegionList" runat="server" ShowFooter="false" Width="80%" AutoGenerateColumns="False" CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
 onpageindexchanging="grvRegionList_PageIndexChanging" onrowcommand="grvRegionList_RowCommand" onrowediting="grvRegionList_RowEditing" 
 AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
           
            <Columns>
             <asp:TemplateField HeaderText="Region Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRegionCode" runat="server" Text = '<%# Eval("RegionCode") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Region Name" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblRegionName" runat="server" Text = '<%# Eval("RegionName") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Country Code" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCoutryCode" runat="server" Text = '<%# Eval("CountryCode") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
              <asp:TemplateField HeaderText="Country Name" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCoutryName" runat="server" Text = '<%# Eval("CountryName") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text = '<%# Eval("IsActive") %>'/>
             </ItemTemplate>
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
    <Triggers>
    <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    </asp:UpdatePanel>
    
</asp:Content>
