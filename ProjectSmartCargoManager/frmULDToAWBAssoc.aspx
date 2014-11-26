<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmULDToAWBAssoc.aspx.cs" Inherits="ProjectSmartCargoManager.frmULDToAWBAssoc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link href="style/style.css" rel="stylesheet" type="text/css" />   
    
     <script type="text/javascript">

        function DoneClick(button) {
            window.opener.cllsa();
            window.close();
        }

        function DoneClickforBooking(button) {            
            window.close();
        }
        
    </script> 
     
</head>
<body style="background: url()">
    <form id="form1" runat="server">
    <div style="overflow:scroll;  height:500px" >
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
   
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
<table style="margin-top:50px;">

<tr>
<td>
    <asp:GridView ID="gvAWBULDAssoc" runat="server" CssClass="grdrowfont" AutoGenerateColumns="false">
    <Columns>
   
    <asp:TemplateField HeaderText="AWBNo">
    <ItemTemplate>
   <asp:Label ID="lblAWB" runat="server" Text='<%# Eval("AWB") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Total Pcs">
    <ItemTemplate>
   <asp:Label ID="lblTotPcs" runat="server" Text='<%# Eval("TotPcs") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
      <asp:TemplateField HeaderText="Total Wgt">
    <ItemTemplate>
   <asp:Label ID="lblTotWgt" runat="server" Text='<%# Eval("TotWgt") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
      <asp:TemplateField HeaderText="Pcs to Assign">
    <ItemTemplate>
   <asp:Label ID="lblPcsToAss" runat="server" Text='<%# Eval("PcsToAssign") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
      <asp:TemplateField HeaderText="Wgt To Assign">
    <ItemTemplate>
   <asp:Label ID="lblWgttoAss" runat="server" Text='<%# Eval("WgttoAssign") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Origin">
    <ItemTemplate>
   <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Destination">
    <ItemTemplate>
   <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("Destination") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Flight Number">
    <ItemTemplate>
   <asp:Label ID="lblFlightNumber" runat="server" Text='<%# Eval("FlightNumber") %>'></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Flight Date">
    <ItemTemplate>
   <asp:Label ID="lblFlightDate" runat="server" Text='<%# Eval("FliightDate","{0:MM/dd/yyyy}") %>' DataFormatString="{0:MM/dd/yyyy}"></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
    
    </Columns>
        <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
          <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
            <FooterStyle CssClass="grdrowfont"></FooterStyle>
            <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
            <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
</td>
</tr>
</table>
<table>
<tr>
<td>
    <asp:GridView ID="GVAssULD" runat="server" AutoGenerateColumns="false" CssClass="grdrowfont"
        ShowFooter="True" >
            <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
    <Columns>
    <asp:TemplateField>
    <ItemTemplate>
  <asp:CheckBox ID="chkSelect" runat="server" />
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="SrNo" Visible="false">
    <ItemTemplate>
   <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("SrNo") %>'  Width="100px" ></asp:Label> 
    </ItemTemplate>
    </asp:TemplateField>
   
    <asp:TemplateField HeaderText="ULD #">
    <ItemTemplate>
   <asp:TextBox ID="lblULDNo" runat="server" Text='<%# Eval("ULD") %>'  Width="100px" ></asp:TextBox> 
   <asp:AutoCompleteExtender ID="ULDNoExtender" runat="server" ServiceMethod="GetULDs"
                                            CompletionInterval="0" EnableCaching="false" CompletionSetCount="10" TargetControlID="lblULDNo"
                                            MinimumPrefixLength="1">
   </asp:AutoCompleteExtender>
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="AWB #">
    <ItemTemplate>
   <%--<asp:TextBox ID="lblAWBNo" runat="server" Text='<%# Eval("AWB") %>' Width="100px"></asp:TextBox> --%>
      <asp:DropDownList ID="lblAWBNo" runat="server" DataSource="<%# fillDropinAssoc() %>" DataTextField="AWB" AutoPostBack="true"
       DataValueField="AWB" SelectedValue='<%# Eval("AWB").ToString() %>' OnSelectedIndexChanged="ddlAWB_getAvailData">
       </asp:DropDownList>
    </ItemTemplate>
    </asp:TemplateField>
    
    <asp:TemplateField HeaderText="Type">
    <ItemTemplate>
   <asp:TextBox ID="lblType" runat="server" Text='<%# Eval("Type") %>' Width="100px"></asp:TextBox> 
    </ItemTemplate>
    </asp:TemplateField>
    
      <asp:TemplateField HeaderText="Pos in Flight">
    <ItemTemplate>
    <asp:TextBox ID="lblPosInFlt" runat="server" Text='<%# Eval("PosInFlight") %>'  Width="100px"></asp:TextBox> 
    </ItemTemplate>
    </asp:TemplateField>
    
      <asp:TemplateField HeaderText="Pieces">
    <ItemTemplate>
    <asp:TextBox ID="lblAssULDPcs" runat="server" Text='<%# Eval("Pcs") %>' Width="100px" MaxLength="5" ></asp:TextBox> 
    <asp:FilteredTextBoxExtender ID="lblAssULDPcs_FilteredTextBoxExtender"  
        runat="server" Enabled="True" TargetControlID="lblAssULDPcs" 
        ValidChars="0123456789">
    </asp:FilteredTextBoxExtender>
    </ItemTemplate>
    </asp:TemplateField>
    
      <asp:TemplateField HeaderText="Weight">
    <ItemTemplate>
    <asp:TextBox ID="lblAssULDWgt" runat="server" Text='<%# Eval("Wgt") %>' Width="100px" MaxLength="8"></asp:TextBox> 
    <asp:FilteredTextBoxExtender ID="lblAssULDWgt_FilteredTextBoxExtender"  
        runat="server" Enabled="True" TargetControlID="lblAssULDWgt" 
        ValidChars=".0123456789">
    </asp:FilteredTextBoxExtender>
    </ItemTemplate>
    </asp:TemplateField>
    
      <asp:TemplateField HeaderText="Flight #" >
    <ItemTemplate>
    <asp:TextBox ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' Width="100px" Enabled="false"></asp:TextBox> 
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="Flight Date">
    <ItemTemplate>
    <asp:TextBox ID="lblFlightDate" runat="server" Text='<%# Eval("FlightDate","{0:MM/dd/yyyy}") %>'  Width="100px" Enabled="false" DataFormatString="{0:MM/dd/yyyy}"></asp:TextBox> 
    </ItemTemplate>
  
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="ULD Origin">
    <ItemTemplate>
    <asp:TextBox ID="lblULDOrigin" runat="server" Text='<%# Eval("ULDOrigin") %>' Width="100px" Enabled="false"></asp:TextBox> 
    </ItemTemplate>
    </asp:TemplateField>
    
     <asp:TemplateField HeaderText="ULD Dest">
    <ItemTemplate>
    <asp:TextBox ID="lblULDDest" runat="server" Text='<%# Eval("ULDDest") %>' Width="100px" Enabled="false"></asp:TextBox> 
    </ItemTemplate>
      <FooterTemplate>
    <asp:Button ID="btnAdd" runat="server" Text="Add"  CssClass="button" OnClick="addNewRow_Click"/>
    <%--<asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="button"/>--%>
    
    </FooterTemplate>
      
    <FooterStyle HorizontalAlign="Right" />
    </asp:TemplateField>
    
    </Columns>
      <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
          <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
            <FooterStyle CssClass="grdrowfont"></FooterStyle>
            <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
            <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
</td>
</tr>

<tr>
<td>
<asp:Button id="btnSave" runat="server" Text="Save" CssClass="button" 
        onclick="btnSave_Click" />
</td>
</tr>
</table>

 
    </div>
   
 </form>
</body>
</html>