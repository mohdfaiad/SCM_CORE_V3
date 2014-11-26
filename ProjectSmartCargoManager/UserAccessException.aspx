<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserAccessException.aspx.cs" Inherits="ProjectSmartCargoManager.UserAccessException" MasterPageFile="~/SmartCargoMaster.Master" %>

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
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
<h1>User Access Exception</h1>
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table cellpadding="2">
    <tr>
        <td>Role</td>
        <td>
            <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="true" onselectedindexchanged="ddlRole_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td>User</td>
        <td>
            <asp:DropDownList ID="ddlUser" runat="server" Width="100px">
            </asp:DropDownList>
        </td>
        <td>Module</td>
        <td>
            <asp:DropDownList Width="100px" ID="ddlModule" runat="server" AutoPostBack="true" onselectedindexchanged="ddlModule_SelectedIndexChanged">
            </asp:DropDownList>
        </td>
        <td>Page</td>
        <td>
            <asp:DropDownList Width="100px" ID="ddlPage" runat="server">
            </asp:DropDownList>
        </td>
        <td>Action</td>
        <td>
            <asp:TextBox ID="txtAction" runat="server"  Width="80px"></asp:TextBox>
        </td>
         <td>Status*</td>
        <td>
            <asp:DropDownList ID="ddlStatus" runat="server">
                <asp:ListItem Text="Select"></asp:ListItem>
                <asp:ListItem Text="Show"></asp:ListItem>
                <asp:ListItem Text="Hide"></asp:ListItem>
                <asp:ListItem Text="Disable"></asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:CheckBox ID="chkActive" runat="server" Text="Active" />
        </td>
    </tr>
    <tr>
    <td colspan="13">
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" onclick="btnSave_Click" />
            <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" onclick="btnList_Click" />
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" />
        </td>
    </tr>     
</table>
</div>
</asp:Panel>  
</div>
  
<div class="ltfloat">
    <asp:GridView ID="grdUserException" runat="server" Width="100%" 
     AutoGenerateColumns="false" PageSize="10" AllowPaging="true" AlternatingRowStyle-CssClass="AltRowStyle" 
     CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle" PagerStyle-CssClass="PagerStyle" 
     RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle" onpageindexchanging="grdUserException_PageIndexChanging" 
     onrowcommand="grdUserException_RowCommand" onrowediting="grdUserException_RowEditing">
     
        <Columns>
        
            <asp:TemplateField HeaderText="SrNo" Visible="false">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblSrNo" Text='<%#Eval("SerialNumber")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Role">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblRole" Text='<%#Eval("RoleName")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="User">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblUser" Text='<%#Eval("Users")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Module">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblModule" Text='<%#Eval("Module")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Page">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblPage" Text='<%#Eval("PageName")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblAction" Text='<%#Eval("ControlId")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblStatus" Text='<%#Eval("STATUS")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Active">
                <ItemTemplate>
                    <asp:Label runat="server" ID="lblActive" Text='<%#Eval("IsActive")%>' ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:ButtonField CommandName="Edit" Text="Edit" />
            
        </Columns>
     
    </asp:GridView>
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