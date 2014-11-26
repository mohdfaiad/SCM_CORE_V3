<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="CartMaster.aspx.cs" Inherits="ProjectSmartCargoManager.CartMaster" %>
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
   <h1>Cart Master</h1>
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="90%" border="0">
<tr>
<td>
Cart#
</td>
<td>
<asp:TextBox ID="txtCartNo" runat="server" Width="75px"></asp:TextBox>
</td>

<td>
Cart Description
</td>
<td>
<asp:TextBox ID="txtCartDesc" runat="server" Width="95px"></asp:TextBox>
</td>
<td>
Station Code
</td>
<td>
<asp:DropDownList ID="ddlStnCode" runat="server">
</asp:DropDownList>
</td>
<td>
Location
</td>
<td>
    <asp:TextBox ID="txtLocation" runat="server" Width="80px"></asp:TextBox>
</td>
<td>
Active
</td>
<td>
<asp:CheckBox ID="chkActive" runat="server" />
</td>
<td>

</td>
</tr>
<tr>
<td colspan="8">
<asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" 
        onclick="btnSave_Click" />
<asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
        onclick="btnClear_Click"/>
<asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
        onclick="btnList_Click" />
<asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
        onclick="btnExport_Click" />
</td>
</tr>
</table>
</div>

</asp:Panel>  
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%">
<asp:GridView ID="grdCartList" runat="server" ShowFooter="false" Width="80%" 
AutoGenerateColumns="False" CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
        SelectedRowStyle-CssClass="SelectedRowStyle" 
        onpageindexchanging="grdCartList_PageIndexChanging" 
        onrowcommand="grdCartList_RowCommand" onrowediting="grdCartList_RowEditing">
           
            <Columns>
            
             <asp:TemplateField HeaderText="SrNo" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lblSrNo" runat="server" Text = '<%# Eval("SerialNumber") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
            
             <asp:TemplateField HeaderText="Cart No">
             <ItemTemplate>
             <asp:Label ID="lblCartNo" runat="server" Text = '<%# Eval("CartNumber") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Cart Description">
             <ItemTemplate>
             <asp:Label ID="lblCartDesc" runat="server" Text = '<%# Eval("CartDescription") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Station">
             <ItemTemplate>
             <asp:Label ID="lblStn" runat="server" Text = '<%# Eval("Station") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Location">
             <ItemTemplate>
             <asp:Label ID="lblLocation" runat="server" Text = '<%# Eval("Location") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Active" ItemStyle-Width="32px" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
                 <asp:CheckBox ID="chkIsAct" runat="server" Enabled="false" Checked='<%#Eval("Isactive")%>' />
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:ButtonField CommandName="Edit" Text="Edit" />
             
            </Columns>
    </asp:GridView>
    </div>
    <div class="ltfloat">
        <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="button" Visible="false" 
            onclick="btnUpdate_Click" />
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
