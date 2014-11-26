<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="JournalEntrySummary.aspx.cs" Inherits="ProjectSmartCargoManager.JournalEntrySummary" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
<div id="contentarea">
   <h1> 
            Journal Entries Summary
         </h1>
         
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
           
 
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="100%" border="0">
<tr>
<td>From Date</td>
<td><asp:TextBox ID="txtfrmdt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" PopupButtonID="ImageButton1"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtfrmdt">
                                        </asp:CalendarExtender></td>
<td>
   To Date 
</td>
<td>   
       <asp:TextBox ID="txttodt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:ImageButton ID="ImageButton2" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="ImageButton2"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txttodt">
                                        </asp:CalendarExtender>    
   </td>
        <td>
        Chart of Account ID
        
        </td>
        <td>
        <asp:DropDownList ID="ddlChartAccID" runat="server" ></asp:DropDownList>
        </td>
             <td colspan="2">
             <asp:Button ID="btnList" runat="server" CssClass="button" 
             Text="List" CausesValidation="false" OnClick="btnList_Click"  />
        
        <asp:Button ID="btnClear" runat="server" CssClass="button" 
             Text="Clear" CausesValidation="false" onclick="btnClear_Click"/>
             <asp:Button ID="btnExport" runat="server" CssClass="button" 
             Text="Export" CausesValidation="false" onclick="btnExport_Click"/>
             </td>
        </tr>
        </table>
 
   </div>
</asp:Panel>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat" style="width:100%; overflow:auto;">
    <asp:GridView ID="grdJournalAccountSummary" runat="server" ShowFooter="false" Width="100%" 
 AutoGenerateColumns="False" CellPadding="2" 
 CellSpacing="3" PageSize="10" AllowPaging="True" 
 AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
        SelectedRowStyle-CssClass="SelectedRowStyle" 
        >
           
            <Columns>
            <asp:ButtonField CommandName="View" Text="View" Visible="false">
                                        <ItemStyle Width="50px" />
                                    </asp:ButtonField>
             
           
             <asp:TemplateField HeaderText="Chart of Acc ID" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblChartAccountID" runat="server" Text = '<%# Eval("ChartofAccountID") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            
            
            <asp:TemplateField HeaderText="DbSCMAcctValue" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblDbSCMAcctValue" runat="server" Text = '<%# Eval("DbSCMAcctValue") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            
            
            
            
            <asp:TemplateField HeaderText="Db/Cr Acc Type" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblCrSCMAcctValue" runat="server" Text = '<%# Eval("DbCrAccountType") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
            
            
            
             
             
             
             
            
            </Columns>
            
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
</asp:Panel>
  </div>
</asp:Content>
