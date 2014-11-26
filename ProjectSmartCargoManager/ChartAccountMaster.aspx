<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="ChartAccountMaster.aspx.cs" Inherits="ProjectSmartCargoManager.ChartAccountMaster" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
    </style>
    
    
    
    <div id="contentarea">
   <h1> 
            Chart Accounts and Sub-Accounts
         </h1>
         
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
           
 
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="100%" border="0">
<tr>
<td>Account Id</td>
<td><asp:TextBox ID="txtAccountID" runat="server" Width="100px"></asp:TextBox></td>
<td>
    Account Name
</td>
<td>
                
                
                 <asp:TextBox ID="txtAccName" runat="server" Width="100px" 
                    style="text-align:left"></asp:TextBox> 
                    

   </td>

      <td>
             Account Description</td>
            <td>
                <asp:TextBox ID="txtAccDesc" runat="server" Width="100px" style="text-align:left" Visible="true"></asp:TextBox> 
                
                </td>
            
            <td>
               Parent Account
            </td>
            <td>
                <asp:DropDownList ID="ddlParentAcc" runat="server" ></asp:DropDownList>
                
            </td>
            
            <%--<td>Balancing Account</td>
    <td>
    <asp:DropDownList ID="ddlbalancing" runat="server" ></asp:DropDownList>
    </td>--%>
                
        </tr>
        <tr>
        <td>
        Cost Center
        </td>
        <td>
        <asp:DropDownList ID="ddlCostCenter" runat="server" ></asp:DropDownList>
        </td>
        <td>
        GL Account
        </td>
        <td>
        <asp:DropDownList ID="ddlGLAccount" runat="server" ></asp:DropDownList>
        </td>
        <td>
        SCMAcctField
        </td>
        <td>
       <asp:DropDownList ID="ddlSCMAcctField" runat="server"></asp:DropDownList>
        </td>
        <td>Account Category
        
        </td>
        <td>
        <asp:DropDownList ID="ddlAccCat" runat="server"></asp:DropDownList>
        </td>
        <td>
        <asp:CheckBox ID="chkactive" runat="server" Text="Active" />
        </td>
        
        <td>
            <asp:CheckBox ID="chkSystem" runat="server" Text="System" />
        </td>
        
        </tr>
        <tr>
        <td>
        Account Type
        </td>
        <td>
        <asp:DropDownList ID="ddlAccType" runat="server"></asp:DropDownList>
        </td>
        <td>Db Acc ID</td>
        <td>
            <asp:DropDownList ID="ddlDbAccId" runat="server">
            </asp:DropDownList>
        </td>
        <td>Cr Acc ID</td>
        <td>
            <asp:DropDownList ID="ddlCrAccId" runat="server">
            </asp:DropDownList>
        </td>
        <td>Ref Entity</td>
        <td>
            <asp:DropDownList ID="ddlRefEntity" runat="server">
                <asp:ListItem Text="Select"></asp:ListItem>
                <asp:ListItem Text="AGENT"></asp:ListItem>
                <asp:ListItem Text="INTERLINE"></asp:ListItem>
                <asp:ListItem Text="VENDOR"></asp:ListItem>
            </asp:DropDownList>
        </td>
        </tr>
        <tr>
         <td colspan="8" align="right">
        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" CausesValidation="false" OnClick="btnSave_Click"  />
        <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" CausesValidation="false" OnClick="btnList_Click"  />
        <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" CausesValidation="false" onclick="btnClear_Click"/>
        </td>
        </tr>
        </table>
 <table style="float:right;">
        <tr>
         
    <td>
        <%--<asp:Button ID="btnImoprt" runat="server" Text="Import" CssClass="button" 
            onclick="btnImoprt_Click" />--%>
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
             Visible="false"/>
    </td>
        </tr>
   
    </table>
   </div>
</asp:Panel>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%; overflow:auto;">
    <asp:GridView ID="grdChartAccount" runat="server" ShowFooter="false" Width="100%" 
     AutoGenerateColumns="False" CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
     AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
     PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
     SelectedRowStyle-CssClass="SelectedRowStyle" onpageindexchanging="grdChartAccount_PageIndexChanging" 
     OnRowCommand="grdChartAccount_RowCommand" OnRowEditing="grdChartAccount_RowEditing">
           
            <Columns>
            <asp:TemplateField HeaderText="" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lblAccountIdd" runat="server" Text = '<%# Eval("AccountID") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
            <asp:TemplateField HeaderText="Account ID" Visible="true">
             <ItemTemplate>
             <asp:Label ID="lblAccount" runat="server" Text = '<%# Eval("Account") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Account Name" Visible="true">
             <ItemTemplate>
             <asp:Label ID="lblAccountName" runat="server" Text = '<%# Eval("AccountName") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Account Description" Visible="true">
             <ItemTemplate>
             <asp:Label ID="lblAccountDescription" runat="server" Text = '<%# Eval("AccountDescription") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Parent Account" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblParentAccount" runat="server" Text = '<%# Eval("ParentAccount") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Cost Center" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblCostCenter" runat="server" Text = '<%# Eval("CostCenter") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="GLAccount" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblGLAccount" runat="server" Text = '<%# Eval("GLAccount") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="SCMAcct Field" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblSCMAcctField" runat="server" Text = '<%# Eval("SCMAcctField") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Acc. Category" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblAccCat" runat="server" Text = '<%# Eval("AccountCategory") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Acc. Type" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblAccType" runat="server" Text = '<%# Eval("AccountType") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
             <asp:TemplateField HeaderText="Updated By" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblUpdatedBy" runat="server" Text = '<%# Eval("UpdatedBy") %>'/>
             </ItemTemplate>
             </asp:TemplateField>   
             
             <asp:TemplateField HeaderText="Updated On" ItemStyle-HorizontalAlign="Center" Visible="true">
             <ItemTemplate>
             <asp:Label ID="lblUpdatedOn" runat="server" Text = '<%# Eval("UpdatedOn") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
            <asp:TemplateField HeaderText="Active" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblIsActive" runat="server" Text = '<%# Eval("IsActive") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="System" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblIsSys" runat="server" Text = '<%# Eval("isSystem") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Db Acc ID" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblDbAccountID" runat="server" Text = '<%# Eval("DbAccountID") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="CrAccountID" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblCrAccountID" runat="server" Text = '<%# Eval("CrAccountID") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="RefEntity" Visible="true" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblRefEntity" runat="server" Text = '<%# Eval("RefEntity") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
            
            <asp:ButtonField CommandName="Edit" Text="Edit">
            <ItemStyle Width="50px"/>
            </asp:ButtonField>
            
            <asp:ButtonField CommandName="DeleteRecord" Text="Delete">
            <ItemStyle Width="50px"/>
            </asp:ButtonField>
            
            
            </Columns>
             <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
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
</asp:Content>
