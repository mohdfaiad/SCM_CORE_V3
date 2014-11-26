<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/SmartCargoMaster.Master" CodeBehind="AutoReportsConfig.aspx.cs" Inherits="ProjectSmartCargoManager.AutoReportsConfig" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

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
                  z-index:1001;
                  -moz-opacity:0.8;
                  opacity:0.8;
                  filter:alpha(opacity=80);
            }
            .white_content 
            {
                margin:0 auto;
                  display: none;
                  position: absolute;
                  top: 30%;
                  left: 35%;
                  width: 30%;
                  height: 45%;
                  padding: 16px;
                  border: 16px solid #ccdce3;
                  background-color: white;
                  z-index:1002;
                  overflow: auto;
            
            }
    </style>
   
        <style>
.ajax__calendar .ajax__calendar_invalid .ajax__calendar_day 
{
    background-color:gray;
    color:White; 
    text-decoration:none; 
    cursor:default;
}
</style>
<script type ="text/javascript">
    function ViewPanel() {
        document.getElementById('light').style.display = 'block';
        document.getElementById('fade').style.display = 'block';
    }
    function HidePanel() {
        document.getElementById('light').style.display = 'none';
        document.getElementById('fade').style.display = 'none';
    }

    function ViewPanelSplit() {
        document.getElementById('Lightsplit').style.display = 'block';
        document.getElementById('fadesplit').style.display = 'block';
    }
    function HidePanelSplit() {
        document.getElementById('Lightsplit').style.display = 'none';
        document.getElementById('fadesplit').style.display = 'none';
    }
</script>
 </asp:Content>
    
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server"  EnableViewState="true">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <div id="contentarea">
                        <div>
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
                        </div>    
                        <div class="botline">
                        <table>
                        <tr>
                        <td>Report Name : <asp:TextBox ID="txtReportName" runat="server" Width="120px" MaxLength="4"></asp:TextBox>
                        </td>
                        <td>
                        &nbsp;&nbsp;From Date : <asp:TextBox ID="txtFromDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                            <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtFromDate" PopupButtonID="imgFromDate">
                            </asp:CalendarExtender>
                        </td>
                        <td>
                        &nbsp;&nbsp;To Date : <asp:TextBox ID="txtToDate" runat="server" Width="85px" MaxLength="10"></asp:TextBox>
                            <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
                                Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtToDate" PopupButtonID="imgToDate">
                            </asp:CalendarExtender>
                        </td>
                        </tr>
                        </table>

                            &nbsp;&nbsp;
                    <asp:Button ID="BtnList" runat="server" Text="List" ValidationGroup="btnList" 
                                CssClass="button" onclick="BtnList_Click" />
                    <asp:Button ID="BtnClear" runat="server" Text="Clear"  
                      CssClass="button" onclick="BtnClear_Click" />
                       
                        </div>
                        
                        <div style="float:left;">
                                     <asp:Panel ID="pnlReportList" runat="server" ScrollBars="Auto" Height="200px" 
                                             style="margin-top:20px"
                                             BorderStyle="Solid" BorderWidth="1px" Width="800px">
                                            <asp:GridView ID="grdReportList"  
                                                 runat="server" CellPadding="3" CellSpacing="2" AutoGenerateColumns="False" style="z-index: 1">
                                                 <Columns>
                                                     <asp:TemplateField>
                                                         <ItemTemplate>
                                                             <asp:CheckBox ID="chkReportList" runat="server" />
                                                         </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="SrNo" Visible="False">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSrNo" Text='<%# Eval("SrNo") %>' runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="ReportName">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtReportName" Text='<%# Eval("ReportName") %>' runat="server" Width="140px"></asp:TextBox>
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="To Email">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtToEmail" Text='<%# Eval("ToEmail") %>' runat="server" Width="200px" ></asp:TextBox>                                                     
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="From Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtFromDate" Text='<%# Eval("FromDate") %>' runat="server" Width="80px"></asp:TextBox>   
                                                            <asp:CalendarExtender ID="txtFromDate_CalendarExtender" runat="server" Enabled="True" 
                                                            TargetControlID="txtFromDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                                                  
                                                        </ItemTemplate>
                                                     </asp:TemplateField>     
                                                     <asp:TemplateField HeaderText="To Date">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtToDate" Text='<%# Eval("ToDate") %>' runat="server" Width="80px"></asp:TextBox> 
                                                            <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" Enabled="True" 
                                                            TargetControlID="txtToDate" Format="dd/MM/yyyy" ></asp:CalendarExtender>                                                    
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="SP Name">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtSPName" Text='<%# Eval("SPName") %>' runat="server" Width="80px"></asp:TextBox>                                                     
                                                        </ItemTemplate>
                                                     </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Frequency">
                                                        <ItemTemplate>
                                                            <asp:DropDownList ID="ddlReportFrequency" runat="server">
                                                             <asp:ListItem Text="Daily" Value="Daily"></asp:ListItem>
                                                             <asp:ListItem Text="Weekly" Value="Weekly"></asp:ListItem>
                                                             <asp:ListItem Text="Fortnightly" Value="Fortnightly"></asp:ListItem>
                                                             <asp:ListItem Text="Monthly" Value="Monthly"></asp:ListItem>
                                                             <asp:ListItem Text="Yearly" Value="Yearly"></asp:ListItem>
                                                         </asp:DropDownList>                                                    
                                                        </ItemTemplate>
                                                     </asp:TemplateField>     
                                                     <asp:TemplateField HeaderText="Is Active">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="chkisactive" GroupName="grpchkisactive" runat="server" />
                                                        </ItemTemplate>
                                                     </asp:TemplateField> 
                                                 </Columns>
                                                 <HeaderStyle CssClass="titlecolr"/>
                                                 <AlternatingRowStyle CssClass="trcolor" Wrap="False" />
                                                 <EditRowStyle CssClass="grdrowfont" />
                                                 <RowStyle CssClass="grdrowfont" Wrap="False" HorizontalAlign="Center" />
                                                 <FooterStyle CssClass="grdrowfont"/>
                                             </asp:GridView>        
                                     </asp:Panel>
                                     <asp:Button ID="btnAdd" runat="server" Text="Add" Visible = "true" 
                                        CssClass="button" onclick="btnAdd_Click">
                                     </asp:Button>
                                     <asp:Button ID="btnSave" runat="server" Text="Save" Visible = "true" 
                                        CssClass="button" onclick="btnSave_Click">
                                     </asp:Button>
                                     </div> 
                                     
         </div>
        

		
		
</asp:Content>

