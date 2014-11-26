<%@ Page Title="SpecialHandlingCodeMaster" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="SpecialHandlingCodeMaster.aspx.cs" Inherits="ProjectSmartCargoManager.SpecialHandlingCodeMaster" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function GetCode() {
        var origin = document.getElementById('<%= txtSpecialHandlingCode.ClientID%>');
        if (origin.value.length > 4) {
            origin.value = origin.value.substring(origin.value.length - 4);
            origin.value = origin.value.replace(')', '');
            
        }
    }
//    function ValidationCode() {
//        var lblvar = document.getElementById('<%=lblStatus.ClientID %>');
//        //__doPostBack(sender.get_element().name, "");
//        lblvar.innerHTML = "SHC Code is Already Exist";
//        alert(lblvar);
//        
//    }
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>

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
            height: 30px;
        }
    </style>
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
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
    <div id="contentarea">
    <div class="msg">
    <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
   <h1> 
            Special Handling Code Master
         </h1>
         
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="80%" border="0">
<tr>
<td>
    Code*
</td>
    
<td>
<asp:TextBox ID="txtSpecialHandlingCode" runat="server" MaxLength="3" Width="80px" Onchange="javascript:GetCode();" ></asp:TextBox>
<asp:AutoCompleteExtender ID="SHCCode" BehaviorID="SHCCode" runat="server" ServiceMethod="GetSHCCode"
CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="txtSpecialHandlingCode" MinimumPrefixLength="1"  >
</asp:AutoCompleteExtender>
</td>
<td>
      Description*
</td>
<td>
    <asp:TextBox ID="txtSpecialHandlingCodeDescription" runat="server"></asp:TextBox></td>
            
           <%-- <td class="style1">
                </td>
            <td class="style1">
                </td>
        
            <td class="style1">
            </td>--%>
           
        <td>
            <asp:Label ID="lblShipper" runat="server" Text="Known Shipper Validation"></asp:Label>
            <asp:CheckBox ID="chkShipper" runat="server" />
    </td>
           
        </tr>
        <tr>
        <td>
                Active</td>
            <td>
                <asp:CheckBox ID="chkActive" runat="server" />
            </td>
            <td>IsNOTOC</td>
            <td>
            <asp:DropDownList ID="ddlisnotoc" runat="server">
            <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
            <asp:ListItem Text="DGR" Value="DGR"></asp:ListItem>
            <asp:ListItem Text="Special Cagro" Value="Special Cargo"></asp:ListItem>
            </asp:DropDownList>
            </td>
        
        </tr>
   

    <tr>
        <td colspan="6">
            <asp:Button ID="btnSave" runat="server" CssClass="button" onclick="btnSave_Click" Text="Save" />
            <%--&nbsp;&nbsp;&nbsp;&nbsp;--%>
            <asp:Button ID="btnClear" runat="server" CssClass="button" onclick="btnClear_Click" Text="Clear" />
           <%-- &nbsp;&nbsp;&nbsp;&nbsp;--%>
            <asp:Button ID="btnList" runat="server" CssClass="button" onclick="btnList_Click" Text="List" />
        </td>
        <td>
            &nbsp;</td>
    </tr>
   

</table></div>
</asp:Panel>  
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat">
    <asp:GridView ID="grvSpecHandlingCodeList" runat="server" ShowFooter="false" 
    AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
    CellPadding="2" CellSpacing="3" PageSize="20" AllowPaging="True"
    AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" 
    HeaderStyle-CssClass="HeaderStyle"  PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
    SelectedRowStyle-CssClass="SelectedRowStyle" onrowcommand="grvSpecHandlingCodeList_RowCommand" 
    onrowediting="grvSpecHandlingCodeList_RowEditing" onpageindexchanging="grvSpecHandlingCodeList_PageIndexChanging"  Width="1000px" >
           
            <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
           
            <Columns>
            <asp:TemplateField HeaderText="Srno" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSrno" runat="server" Text = '<%# Eval("SerialNumber") %>'/>
                    </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Code">
                    <ItemTemplate>
                        <asp:Label ID="lblSpecHandlingCode" runat="server" Text = '<%# Eval("SpecialHandelingCode") %>'/>
                    </ItemTemplate>
              <ItemStyle Width="100px"/>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Description">
                    <ItemTemplate>
                        <asp:Label ID="lblDescription" runat="server" Text = '<%# Eval("Description") %>'/>
                    </ItemTemplate>
               <ItemStyle Wrap="true" Width="60%"/>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Status">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text = '<%# Eval("IsActive") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
                <asp:TemplateField HeaderText="Known Shipper Validation">
             <ItemTemplate>
             <asp:Label ID="lblShipper" runat="server" Text = '<%# Eval("IsShipper") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
                <asp:TemplateField HeaderText="isNoToc">
             <ItemTemplate>
             <asp:Label ID="lbldgr" runat="server" Text = '<%# Eval("isNoToc") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
            <asp:ButtonField CommandName="Edit" Text="Edit">
                                    <ItemStyle Width="50px" />
                                </asp:ButtonField>
            </Columns>
            <PagerStyle CssClass="PagerStyle" />
            <SelectedRowStyle CssClass="SelectedRowStyle" />
            <HeaderStyle CssClass="HeaderStyle" />
            <AlternatingRowStyle CssClass="AltRowStyle" />
            </asp:GridView>
    </div>
</asp:Panel>
  
  
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
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>


</asp:Content>
