<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master" Title="ProductTypeMaster" CodeBehind="frmProductTypeMaster.aspx.cs"  Inherits="ProjectSmartCargoManager.frmProductTypeMaster" %>

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
        .style1
        {
            height: 29px;
        }
    </style>
  
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
  <div id="contentarea">
<div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red">
</asp:Label>
</div>
   <h1> 
    Product Type Master
         </h1>
           
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="100%" border="0">
<tr>
<td>
    Product Type *
</td>    
<td>
    <asp:TextBox ID="txtCommodityCode" runat="server" MaxLength="20"></asp:TextBox>
</td>
<td>
    Product Description
</td>    
<td>
    <asp:TextBox ID="txtProductDescription" runat="server"  MaxLength="50" Width="300px"></asp:TextBox>
</td>
<td>
    Active
    <asp:CheckBox ID="chkActive" runat="server" />
</td>
<td>Mail<asp:CheckBox ID="chkMail" runat="server" />
    </td>
    <td>
        <asp:Label ID="lblShipper" runat="server" Text="Known Shipper Validation"></asp:Label>
        <asp:CheckBox ID="chkShipper" runat="server" />
    </td>
</tr>
    <tr>
        <td class="style1">
            Priority</td>
        <td class="style1">
            <asp:TextBox ID="txtPriority" runat="server" Width="40px"></asp:TextBox>
           <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers" TargetControlID="txtPriority" />
            
        </td>
        <td class="style1">
            </td>
        <td class="style1">
            </td>
        <td class="style1">
            </td>
        <td class="style1">
            </td>
        <td class="style1">
            </td>
    </tr>
<tr>
            <td colspan="4">
              <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" onclick="btnSave_Click"  />
              &nbsp;<asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" onclick="btnClear_Click" />
              &nbsp;<asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click" />
            </td>
        
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
        <asp:TemplateField HeaderText="SerialNo" Visible="false">
        <ItemTemplate>
        <asp:Label runat="server" Text='<%#Eval("SerialNumber")%>' ID="lblSerialNo"></asp:Label>
        </ItemTemplate>
        </asp:TemplateField>
             <asp:TemplateField HeaderText="Product Type" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCommodityCode" runat="server" Text ='<%#Eval("ProductType")%>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Product Description" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblProductDescription" runat="server" Text ='<%#Eval("ProductDescription")%>'/>
             </ItemTemplate>
             </asp:TemplateField>             
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text ='<%# Eval("IsActive") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Mail" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblMail" runat="server" Text ='<%# Eval("IsMail") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
              <asp:TemplateField HeaderText="Known Shipper Validation" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblShipper" runat="server" Text ='<%# Eval("IsShipper") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
              
              <asp:TemplateField HeaderText="Priority" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblPriority" runat="server" Text ='<%# Eval("Priority") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
            <asp:ButtonField CommandName="Edit" Text="Edit">
            <ItemStyle Width="50px" />
            </asp:ButtonField>
          <asp:ButtonField CommandName="DeleteRecord" Text="Delete">
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
