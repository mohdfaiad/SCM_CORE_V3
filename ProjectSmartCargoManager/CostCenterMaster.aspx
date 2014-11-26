<%@ Page Title="Cost Center Master" Language="C#" AutoEventWireup="true" CodeBehind="CostCenterMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.CostCenterMaster" %>

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
  <asp:Label ID="lblStatus" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
  </div>
   <h1> 
           Cost Center Master
         </h1>
         
                
            
 <div class="botline">
<asp:Panel ID="pnlNew" runat="server">

<table width="80%" border="0" cellpadding="3" cellspacing="3">
<tr>
<td>
    Cost Center Name*
</td>
<td>
    <asp:TextBox ID="txtCostCenterName" runat="server"></asp:TextBox>
</td>
<td>
    Cost Center Desc.
</td>
<td>
     <asp:TextBox ID="txtCostCenterDescription" runat="server" MaxLength="200"></asp:TextBox>
</td>
<td>
Active
</td>
<td>
    <asp:CheckBox ID="chkActive" runat="server" 
        oncheckedchanged="chkActive_CheckedChanged" />
</td>
<td align="right">
    &nbsp;</td>
</tr>
    <tr>
        <td colspan="7">
            <asp:Button ID="btnList" runat="server" CssClass="button" 
                onclick="btnList_Click" Text="List" />
            <asp:Button ID="btnClear" runat="server" CssClass="button" 
                onclick="btnClear_Click" Text="Clear" />
        </td>
    </tr>
</table>
</asp:Panel>
</div>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%">
<asp:GridView ID="grvCostCenterList" runat="server" ShowFooter="false" Width="100%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
        onrowcommand="grvCostCenterList_RowCommand" 
        onpageindexchanging="grvCostCenterList_PageIndexChanging" >
           
            <Columns>
             <asp:TemplateField HeaderText="" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkRow" runat="server"/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Cost Center ID" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtCostCenterID" runat="server" Text = '<%# Eval("CostCenterID") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                  <asp:TemplateField HeaderText="Cost Center Name" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtCostCenterName" runat="server" Text = '<%# Eval("CostCenterName") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                  <asp:TemplateField HeaderText="Cost Center Desc" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtCostCenterDescription" runat="server" Text = '<%# Eval("CostCenterDescription") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                  <asp:TemplateField HeaderText="Updated By" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtUpdatedBy" runat="server" Text = '<%# Eval("UpdatedBy") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Updated On" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtUpdatedOn" runat="server" Text = '<%# Eval("UpdatedOn") %>'/>
                    </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="IsActive" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:CheckBox ID="chkActive" runat="server" Checked='<%#((bool)Eval("IsActive"))%>'/>
                    </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
</div>
<asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="button" 
             CausesValidation="false" onclick="btnAdd_Click" />
             <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
             CausesValidation="false" onclick="btnSave_Click" />
             <asp:Button ID="btnDel" runat="server" Text="Delete" CssClass="button" 
             CausesValidation="false" onclick="btnDel_Click" />
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

