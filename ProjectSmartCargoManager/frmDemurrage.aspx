<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmDemurrage.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmDemurrage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div id="contentarea">
    <h1>ULD Demurrage</h1>
    
    <div id="singlecol">
    
 <div>
     <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True"></asp:Label>
<h2 style="width:400px;"> 
<asp:RadioButtonList ID="tblDemType" runat="server" RepeatDirection="Horizontal" 
        RepeatLayout="Flow">
    <asp:ListItem Selected="True">Internal</asp:ListItem>
    <asp:ListItem>External</asp:ListItem>
    </asp:RadioButtonList>
  </h2>
  
<table>
<tr>
<td>
From Date
</td>
<td>
    Warehouse
</td>
<td>
ULD Type
</td>
<td>
ULD No.
</td>
<td>
ULD Owner
</td>
<td>
Demurrage Status
</td>
<td>
    &nbsp;
    Agent Code</td>
<td>
    &nbsp;</td>
    <td>
    </td>
</tr>
<tr>
<td>
    <asp:TextBox ID="txtFrom" runat="server" CssClass="inputbgmed" Width="100px"></asp:TextBox>
</td>
<td>
    <asp:TextBox ID="txtWH" runat="server" CssClass="inputbgmed" Width="100px"></asp:TextBox>
</td>
<td>
    <asp:TextBox ID="txtULDType" runat="server" CssClass="inputbgmed" Width="100px"></asp:TextBox>
</td>
<td align ="center">
    <asp:TextBox ID="txtULDNo" runat="server" CssClass="inputbgmed" Width="100px"></asp:TextBox>
</td>
<td>
    <asp:TextBox ID="txtULDOwner" runat="server" CssClass="inputbgmed" 
        Width="100px"></asp:TextBox>
</td>
<td>
    <asp:DropDownList ID="ddlStatus" runat="server">
    </asp:DropDownList>
</td>
<td>
        &nbsp;
        <asp:DropDownList ID="ddlAgentCode" runat="server">
    </asp:DropDownList>
</td>
<td>
    <asp:Button ID="btnSearch" runat="server" onclick = "btnSearch_Click" CssClass="button" Text ="List Demurrage"/>
    <%--<asp:ImageButton ID="btnSearch" runat="server" Height="20px" 
        ImageUrl="~/images/search.png" Width="20px" onclick="btnSearch_Click" />--%>
</td>
</tr>
</table>

</div>

<%--<div style = "overflow:scroll;width:100%;" >--%>

<h2> Result</h2>
 <asp:GridView ID="gvUCR" runat="server" AutoGenerateColumns="False" 
              Width="100%" ShowFooter="false" HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle"  CellPadding="2"
            AlternatingRowStyle-CssClass="AltRowStyle" 
             PagerStyle-CssClass="PagerStyle"                            >
<RowStyle CssClass="RowStyle" HorizontalAlign="Center"></RowStyle>
<EmptyDataTemplate>
No Record Available
</EmptyDataTemplate>
                                   <Columns>
                                   <asp:TemplateField >    
                                   <ItemTemplate>    
                                   <asp:CheckBox ID="lblchk" runat="server"></asp:CheckBox>
                                   </ItemTemplate>
                                   </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="SrNo" Visible="false">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblSrNo" runat="server" Text='<%# Eval("SrNo") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="UCR">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblUCR" runat="server" Text='<%# Eval("UCRNo") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="ULD No.">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblULDNo" runat="server" Text='<%# Eval("ULDNO") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Qty">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Possessor">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Days">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblNoDays" runat="server" Text='<%# Eval("NoofDays") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Station">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblWH" runat="server" Text='<%# Eval("WareHouse") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="SubLoc">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblSubLoc" runat="server" Text='<%# Eval("SubWareHouse") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>   
                                   <asp:TemplateField HeaderText="Source" Visible="false">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblSource" runat="server" Text='<%# Eval("Source") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>   
                                   <asp:TemplateField HeaderText="Status">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Start Date">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblStrtDt" runat="server" Text='<%# Eval("StartDate") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="End Date">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("EndDate") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="ULDStatus">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblULDStatus" runat="server" Text='<%# Eval("ULDStatus") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>   
                                   <asp:TemplateField HeaderText="Dem Charge">    
                                   <ItemTemplate>    
                                   <asp:TextBox ID="lblCharges" runat="server" Text='<%# Eval("Charge") %>' Width="70px"></asp:TextBox>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Currency">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblCurrency" runat="server" text='<%# Eval("Currency") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>                                   
                                   <asp:TemplateField HeaderText="Rate">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblRates" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Comment">    
                                   <ItemTemplate>    
                                   <asp:TextBox ID="lblComment" runat="server" Text='<%# Eval("Comment") %>' TextMode="MultiLine" Width="150px"></asp:TextBox>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="Possessor" Visible="false">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblPossessor" runat="server" Text='<%# Eval("Processor") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="isInternal" Visible="false">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblInternal" runat="server" Text='<%# Eval("isInternal") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="isCustomer" Visible="false">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblisCustomer" runat="server" Text='<%# Eval("isCustomer") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   
                                    <asp:TemplateField HeaderText="LocationType" Visible="false">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblLocationType" runat="server" Text='<%# Eval("LocationType") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LocationID" Visible="false">    
                                   <ItemTemplate>    
                                   <asp:Label ID="lblLocationID" runat="server" Text='<%# Eval("LocationID") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   </Columns>

<PagerStyle CssClass="PagerStyle"></PagerStyle>

<HeaderStyle CssClass="HeaderStyle"></HeaderStyle>

<AlternatingRowStyle CssClass="AltRowStyle"></AlternatingRowStyle>
                                   </asp:GridView>
                                   <br />
                                        
                               <br />
                                                                             
<%--</div>--%>
<asp:Button ID="btnUpdtDem" runat="server" 
                                                         CssClass="button"  
        Text="Update Demurrage" onclick="btnUpdtDem_Click" />
<%--<div>
<br />
<h2> External Demurrage </h2>
<asp:GridView ID="gvExternalDem" runat="server" AutoGenerateColumns="False" 
              Width="100%" ShowFooter="false">
                                  <Columns>
                                  <asp:TemplateField HeaderText="ULDNo">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblULDNo" runat="server" Text = '<%# Eval("ULDNo") %>' ></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField> 
                                   <asp:TemplateField HeaderText="Gateway">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblGateway" runat="server" Text = '<%# Eval("Gateway") %>' ></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                    <asp:TemplateField HeaderText="LocationDt">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblLocDt" runat="server" Text = '<%# Eval("LocationDt") %>'></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField>
                                   <asp:TemplateField HeaderText="LocationSrc">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblLocSrc" runat="server" Text = '<%# Eval("LocationSrc") %>' ></asp:Label>
                                   </ItemTemplate>
                                   </asp:TemplateField> 
                                   </Columns>
</asp:GridView>

</div>--%>
</div></div>
    </asp:Content>
