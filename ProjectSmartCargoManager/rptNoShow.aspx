<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="rptNoShow.aspx.cs" Inherits="ProjectSmartCargoManager.rptNoShow" %>
<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="contentarea">
   
    
    <h1>
            <%--<img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
       <h1>
        No Show AWB
       </h1>
          <div class="botline">
            <table border="0" cellpadding="3" cellspacing="3" width="95%">
                                <tr>
                                    <td>
                                        Origin
                                    </td>
                                <td colspan="3">
                                        
                                        
                                        <asp:DropDownList ID="ddlSource" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                        
                                        </td>
                                    
                                    <td>
                                        Destination
                                    </td>
                                    <td>
                                       
                                        <asp:DropDownList ID="ddlDest" runat="server" AutoPostBack="false">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        Comm Code</td>
                                    <td>
                                        <asp:TextBox ID="txtCommodityCode" TabIndex="35" runat="server" Width="100px"
                                            CssClass="styleUpper" onchange="return GetCommodityCode(this);">
                                        </asp:TextBox>
                                        <asp:AutoCompleteExtender ID="ACECommCode" BehaviorID="ACECommCode" runat="server"
                                            ServiceMethod="GetCommodityCodesWithName" CompletionInterval="0" EnableCaching="false"
                                            CompletionSetCount="10" TargetControlID="txtCommodityCode" MinimumPrefixLength="1"
                                            OnClientPopulated="onCommListPopulated">
                                        </asp:AutoCompleteExtender>
                                    </td>
                                    
                                </tr>
                                
                                <tr>
                                <td>
                                        From Date</td>
                                <td colspan="3">
                                        
                                       
                                        <asp:TextBox ID="txtAWBFromDt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender2" runat="server" PopupButtonID="imgAWBFromDt"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtAWBFromDt">
                                        </asp:CalendarExtender>
                                        <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                    </td>
                                    
                                    <td>
                                        To Date</td>
                                    <td>
                                        <asp:TextBox ID="txtAWBToDt" runat="server" Width="85px"></asp:TextBox>
                                        <asp:CalendarExtender ID="CalendarExtender3" runat="server" PopupButtonID="imgAWBToDt"
                                            Enabled="True" Format="dd/MM/yyyy" TargetControlID="txtAWBToDt">
                                        </asp:CalendarExtender>
                                        <asp:ImageButton ID="imgAWBToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="LBLAWBStatus" runat="server" Text="Status"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DDLStatus" runat="server"  Width="150px" Visible="false">
                                            <asp:ListItem Text="Booked" Value="B"></asp:ListItem>
                                            <asp:ListItem Text="Executed" Value="E"></asp:ListItem>
                                            <asp:ListItem Text="Reopen" Value="R"></asp:ListItem>
                                            <asp:ListItem Text="Void" Value="V"></asp:ListItem>
                                            
                                            <asp:ListItem Text="ALL" Value="A" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    
                                    
                                    
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Button ID="btnList" runat="server" CssClass="button" OnClick="btnList_Click"
                                            OnClientClick="callShow();" Text="List" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btnClear" runat="server" CssClass="button"
                                            Text="Clear" />
                                    </td>
                                    
                                </tr>
                            </table>
    
   
    
    </div>
    
    <table width="100%">
        <tr>
        <td>
            
            
            <dd:WebReportViewer ID="rptNoShowAWB" runat="server" Height="500px" Width="1000px" />
            
                    </td>  
            
            
</tr>
   </table>
         
         </div>
</asp:Content>
