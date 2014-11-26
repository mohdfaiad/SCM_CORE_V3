<%@ Page Title="Account Category Master" Language="C#" AutoEventWireup="true" CodeBehind="AccountCategoryMaster.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.AccountCategoryMaster" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script>
    function RadioCheck(rb) {
        var gv = document.getElementById("<%=grvCategoryList.ClientID%>");
        var rbs = gv.getElementsByTagName("input");

        var row = rb.parentNode.parentNode;
        for (var i = 0; i < rbs.length; i++) {
            if (rbs[i].type == "radio") {
                if (rbs[i].checked && rbs[i] != rb) {
                    rbs[i].checked = false;
                    break;
                }

            }
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
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
  
  <div id="contentarea">
   <h1> 
           Account Category Master
         </h1>
         
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
            
 <div class="botline">
<asp:Panel ID="pnlNew" runat="server">

<table width="80%" border="0" cellpadding="3" cellspacing="3">
<tr>
<td>Category ID*
</td>
<td>
    <asp:TextBox ID="txtCategoryID" runat="server"></asp:TextBox>
</td>
<td>
    Category Name.
</td>
<td>
     <asp:TextBox ID="txtCategoryName" runat="server" MaxLength="200"></asp:TextBox>
</td>
<td>
Active
</td>
<td>
    <asp:CheckBox ID="chkActive" runat="server" 
        />
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
<asp:GridView ID="grvCategoryList" runat="server" Width="100%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="15" AllowPaging="True" 
        onrowcommand="grvCategoryList_RowCommand" 
        OnPageIndexChanging="grvCategoryList_PageIndexChanging"
         >
           
            <Columns>
             <asp:TemplateField HeaderText="" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:RadioButton ID="rdbchkRow" runat="server" onclick="javascript:RadioCheck(this)" GroupName="gr1"/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Category ID" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtCategoryID" runat="server" Text = '<%# Eval("CategoryID") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                  <asp:TemplateField HeaderText="Category Name" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtCategoryName" runat="server" Text = '<%# Eval("CategoryName") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                  <asp:TemplateField HeaderText="Category Description" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:TextBox ID="grdtxtCategoryDescription" runat="server" Text = '<%# Eval("CategoryDescription") %>'/>
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
             <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="button" Visible ="false" 
              CausesValidation="false" OnClick="btnEdit_Click" />
            
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

