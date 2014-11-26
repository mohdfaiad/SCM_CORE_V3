<%@ Page Title="CommodityMaster" Language="C#" AutoEventWireup="true"  MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="CommodityMaster.aspx.cs" Inherits="ProjectSmartCargoManager.CommodityMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function Validate() {

        var CommCode = document.getElementById("<%=txtCommodityCode.ClientID%>").value;
        var CommName = document.getElementById("<%= txtCommodityName.ClientID%>").value;

        if (CommCode == "" || CommName == "") {
            alert('Fields marked with * are mandatory');
            return false;
        }
        
    }
     </script>
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
            height: 31px;
        }
        .style2
        {
            width: 96px;
        }
        .style3
        {
            height: 31px;
            width: 96px;
        }
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
  <div id="contentarea">
   <h1> 
            Commodity Master
         </h1>
         
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
            
 <div class="botline">
<asp:Panel ID="pnlNew" runat="server">

<table width="100%" border="0" cellpadding="3" cellspacing="3">
<tr>
<td>
Commodity Code*
</td>
    
<td>
<asp:TextBox ID="txtCommodityCode" runat="server"></asp:TextBox>
</td>
<td>
Commodity Name*
</td>
<td>
    <asp:TextBox ID="txtCommodityName" runat="server"></asp:TextBox>
    </td>
<td>
Commodity Desc. 
</td>
<td>
     <asp:TextBox ID="txtCommodityDesc" runat="server" TextMode="MultiLine" 
         Height="26px"></asp:TextBox>
</td>

<td>
Active
</td>

<td class="style2">
    <asp:CheckBox ID="chkActive" runat="server" />
</td>


</tr>

   
        <tr>
        <td class="style1">Commodity Category</td>
        <td class="style1">
            <asp:DropDownList ID="ddlCommCategory" runat="server">
            </asp:DropDownList>
        </td>
        
        <td>
            <asp:Label ID="lblisnotoc" runat="server" Text="IsNoTOC"></asp:Label>
            </td>
            <td><asp:DropDownList ID="ddlisnotoc" runat="server">
            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
               <asp:ListItem Text="DGR" Value="DGR"></asp:ListItem>
                  <asp:ListItem Text="Special Cargo" Value="Special Cargo"></asp:ListItem>
            </asp:DropDownList>
            </td>
            <td>Priority</td>
            <td>
                <asp:TextBox ID="txtPriority" runat="server"></asp:TextBox>
                 <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="True" FilterType="Numbers" TargetControlID="txtPriority" ValidChars="0123456789"></asp:FilteredTextBoxExtender>
            </td>
            <td class="style1">SHC Code</td>
        <td class="style1">
            <asp:DropDownList ID="ddlshccode" runat="server">
            </asp:DropDownList>
        </td>
        </tr>
        <tr>
            <td colspan="6">
                <asp:Label ID="lblShipper" runat="server" Text="Known Shipper Validation"></asp:Label>
                <asp:CheckBox ID="chkShipper" runat="server" />
            </td>
            
            
        </tr>
    

    <tr>
        <td colspan="8">
            <asp:Button ID="btnSave" runat="server" CssClass="button" 
                onclick="btnSave_Click" OnClientClick="javascript:return Validate();" 
                Text="Save" />
            <asp:Button ID="btnClear" runat="server" CssClass="button" 
                onclick="btnClear_Click" Text="Clear" />
            <asp:Button ID="btnList" runat="server" CssClass="button" 
                onclick="btnList_Click" Text="List" />
        </td>
    </tr>
    

</table>
</asp:Panel>  </div>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%">
    <asp:GridView ID="grvCommodityList" runat="server" ShowFooter="false" Width="100%" AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
     CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" onrowcommand="grvCommodityList_RowCommand" 
     onpageindexchanging="grvCommodityList_PageIndexChanging" onrowediting="grvCommodityList_RowEditing"
     AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
           
            <Columns>
            <asp:TemplateField HeaderText="Sr No" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSrNo" runat="server" Text = '<%# Eval("SerialNumber") %>'/>
                    </ItemTemplate>
                    <%--<HeaderStyle Wrap="True"></HeaderStyle>--%>
             </asp:TemplateField>
                    
             <asp:TemplateField HeaderText="Commodity Code" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblCommodityCode" runat="server" Text = '<%# Eval("CommodityCode") %>'/>
                    </ItemTemplate>
            <%--<HeaderStyle Wrap="True"></HeaderStyle>--%>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Commodity Name" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCommodityName" runat="server" Text = '<%# Eval("CommodityName") %>'/>
             </ItemTemplate>
            <%--<HeaderStyle Wrap="True"></HeaderStyle>--%>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Commodity Description" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCommDesc" runat="server" Text = '<%# Eval("Description") %>'/>
             </ItemTemplate>
             <%--<HeaderStyle Wrap="True"></HeaderStyle>--%>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Commodity Category" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblCommCategory" runat="server" Text = '<%# Eval("CommCategory") %>'/>
             </ItemTemplate>
             <%--<HeaderStyle Wrap="True"></HeaderStyle>--%>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text = '<%# Eval("IsActive") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                <asp:TemplateField HeaderText="Known Shipper Validation" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblShipper" runat="server" Text = '<%# Eval("IsShipper") %>' Width="80px"/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="IsNoToc" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblnotoc" runat="server" Text = '<%# Eval("IsNoTOc") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
              <asp:TemplateField HeaderText="Priority" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblpriority" runat="server" Text = '<%# Eval("Priority") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
              <asp:TemplateField HeaderText="SHC Code" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblSHCcode" runat="server" Text = '<%# Eval("SHCCode") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
            <asp:ButtonField CommandName="Edit" Text="Edit">
            <ItemStyle Width="50px" />
            </asp:ButtonField>
            <asp:ButtonField CommandName="DeleteRecord" Text="Delete">
            <ItemStyle Width="50px" />
            </asp:ButtonField>
            
            </Columns>
            <%--<HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>--%>
    </asp:GridView></div>
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
