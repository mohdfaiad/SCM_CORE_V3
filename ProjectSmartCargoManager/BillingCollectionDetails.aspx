<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillingCollectionDetails.aspx.cs" Inherits="ProjectSmartCargoManager.BillingCollectionDetails" MasterPageFile="~/SmartCargoMaster.Master" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
     
    </style>
    <script language="javascript" type="text/javascript">
    
        function DoPostBackWithRowIndex(rowIndex) {
            if (document.getElementById('<%=HdnSelectedRowIndex.ClientID%>') != null) {
                document.getElementById('<%=HdnSelectedRowIndex.ClientID%>').value = rowIndex;
            }
            return true;
        }
        
        

    </script>
 </asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>       
            <img src="Images/txt_billing.png" />
    </h1> 
        <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        </p>
  <div> 
  
  <table width=" 80%">
   <tr>
    <td>
     Agent Code 
    </td>
    
    <td>
        <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentCode_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
    <td>
     Agent Name
    </td>
    <td>
        <asp:DropDownList ID="ddlAgentName" runat="server" Width="170px" 
            AutoPostBack="True" onselectedindexchanged="ddlAgentName_SelectedIndexChanged" >
        </asp:DropDownList>
    </td>
    <td>
     Bill Type
    </td>
    <td>
        <asp:DropDownList ID="ddlBillType" runat="server" Width="120px" >
         <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
          <asp:ListItem Text="Fortnightly" Value="Fortnightly"></asp:ListItem>
          <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
        </asp:DropDownList>
    </td>
    <td></td>
    </tr>
    <tr>
    <td>
     From Date *
    </td>
    <td>
        <asp:TextBox ID="txtInvoiceFrom" runat="server" Width="80px"></asp:TextBox>
        <asp:ImageButton ID="btnInvoiceFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
           ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEInvoiceFrom" Format="dd/MM/yyyy" TargetControlID="txtInvoiceFrom"
           PopupButtonID="btnInvoiceFrom" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender>
    </td>
    <td>
     To Date *
    </td>
    <td>
     <asp:TextBox ID="txtInvoiceTo" runat="server" Width="80px"></asp:TextBox>
     <asp:ImageButton ID="btnInvoiceTo" runat="server" ImageUrl="~/Images/calendar_2.png"
        ImageAlign="AbsMiddle" />
     <asp:CalendarExtender ID="CEInvoiceTo" Format="dd/MM/yyyy" TargetControlID="txtInvoiceTo"
        PopupButtonID="btnInvoiceTo" runat="server" PopupPosition="BottomLeft">
     </asp:CalendarExtender>
    </td>
     <td>
     Origin
    </td>
    <td>
        <asp:TextBox ID="txtOrigin" runat="server" Width="90px"></asp:TextBox>
        <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
                 TargetControlID="txtOrigin">
               </asp:AutoCompleteExtender>
    </td>
        
    <td>
        <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
            onclick="btnList_Click" />
    </td>

   </tr>
  </table>  
  
  </div> 
  <br />

<div id="divPrint" class="divback">
<asp:GridView ID="grdInvoiceList" Width="80%" runat="server"
         AutoGenerateColumns="False" AllowPaging="True" PageSize="20" 
        onrowdatabound="grdInvoiceList_RowDataBound" >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
               
                 <asp:TemplateField HeaderText="Agent Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentName" runat="server" Text='<%# Eval("AgentName") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:ButtonField HeaderText="Invoice No." ItemStyle-HorizontalAlign="Center" CommandName="InvoiceClick" DataTextField="InvoiceNumber"/>
                
                <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true" Visible ="false">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("InvoiceNumber") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# Eval("InvoiceAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Collected Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:TextBox ID="txtCollectedAmount" runat="server" Text='<%# Eval("CollectedAmount") %>' OnTextChanged="TxtCollAmtChanged"
                            AutoPostBack="true"></asp:TextBox >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Pending Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblPendingAmount" runat="server" Text='<%# Eval("PendingAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
        </Columns> 
                <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                <HeaderStyle CssClass="titlecolr"></HeaderStyle>
                <RowStyle CssClass="grdrowfont"></RowStyle>
</asp:GridView>

</div>   
  <div id="fotbut">
            <table>
                <tr>
                    
                    <td>
                        <asp:Button ID="btnUpdate" Visible="false" runat="server" 
                            Text="Update" CssClass="button" onclick="btnUpdate_Click" />
                    </td>&nbsp;
                    <td>
                        <asp:Button ID="btnExport" Visible="false" runat="server" 
                            Text="Export" CssClass="button" onclick="btnExport_Click" />
                    </td>
                    <td><asp:HiddenField ID="HdnSelectedRowIndex" runat="server" Value="" /></td>
                </tr>
            </table>
            </div>
            <br />
 </div>
  </asp:Content> 
