<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DCMList.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.DCMList" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
     
    </style>
    <script language="javascript" type="text/javascript">
        
    </script>
 </asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
  <div id="contentarea">
  
    <h1>       
        DCM Listing
    </h1> 
        <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        </p>
  <div> 
  
  <table width="98%">
   <tr>
    <td>
     DCM Number
    </td>
    <td>
        <asp:TextBox ID="txtDCMNumber" runat="server" Width="140px"></asp:TextBox>
    </td>
    <td>
     From Date *
    </td>
    <td>
        <asp:TextBox ID="txtDCMFrom" runat="server" Width="80px"></asp:TextBox>
        <asp:ImageButton ID="btnDCMFrom" runat="server" ImageUrl="~/Images/calendar_2.png"
           ImageAlign="AbsMiddle" />
        <asp:CalendarExtender ID="CEDCMFrom" Format="dd/MM/yyyy" TargetControlID="txtDCMFrom"
           PopupButtonID="btnDCMFrom" runat="server" PopupPosition="BottomLeft">
        </asp:CalendarExtender>
    </td>
    <td>
     To Date *
    </td>
    <td>
     <asp:TextBox ID="txtDCMTo" runat="server" Width="80px"></asp:TextBox>
     <asp:ImageButton ID="btnDCMTo" runat="server" ImageUrl="~/Images/calendar_2.png"
        ImageAlign="AbsMiddle" />
     <asp:CalendarExtender ID="CEDCMTo" Format="dd/MM/yyyy" TargetControlID="txtDCMTo"
        PopupButtonID="btnDCMTo" runat="server" PopupPosition="BottomLeft">
     </asp:CalendarExtender>
    </td>
    <td>
     DCM Type
    </td>
    <td>
        <asp:DropDownList ID="ddlDCMType" runat="server" Width="80px" >
         <asp:ListItem Text="All" Value=""></asp:ListItem>
          <asp:ListItem Text="Credit" Value="Credit"></asp:ListItem>
          <asp:ListItem Text="Debit" Value="Debit"></asp:ListItem>
        </asp:DropDownList>
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
<asp:GridView ID="grdDCMList" Width="80%" runat="server"
             AutoGenerateColumns="False" >
 <AlternatingRowStyle CssClass="trcolor">
 </AlternatingRowStyle>
            <Columns>
                <asp:TemplateField HeaderText="Agent Code" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="DCM Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblDCMNumber" runat="server" Text='<%# Eval("DCMNumber") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Invoice Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("InvoiceNumber") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="AWB Number" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblInvoiceNumber" runat="server" Text='<%# Eval("AWBNumber") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Amount" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("DCMAmount") %>' ></asp:Label >
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Type" ItemStyle-HorizontalAlign="Center" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                       <asp:Label ID="lblType" runat="server" Text='<%# Eval("DCMType") %>' ></asp:Label >
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
                        <asp:Button ID="btnPrint" Visible="false" runat="server" 
                            Text="Print Listing" CssClass="button" />
                    </td>
                    
                    <td><asp:HiddenField ID="hf1" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf2" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf3" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf4" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hf5" runat="server" Value="" /></td>
                    <td><asp:HiddenField ID="hfInvoiceNos" runat="server" Value="" /></td>
                </tr>
            </table>
            </div>
            <br />
 </div>
  </asp:Content> 

