
<%@ Page  Title="Capacity vs Utilized Report" Language="C#" AutoEventWireup="true" CodeBehind="rptFlightUtilization.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptFlightUtilization" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 
 
    <style type="text/css">
        .style1
        {
width: 44px;
}
.style2
{
        }
        .style3
        {
            width: 10px;
        }
    </style>
 
 
 
</asp:Content>

 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
     <div id="contentarea">
   <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
</div>
    <%--<h1><img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
       <h1>
           Capacity vs Utilized Report</h1>
          <div >
            <table width="100%">
                <tr>
                    <td class="style2">
                        Date From </td>
                    <td>
                        
                         <%--  <asp:TextBoxWatermarkExtender ID="txtCountry_TextBoxWatermarkExtender"  WatermarkText="User Name" 
                    runat="server" TargetControlID="txtUserName">
                </asp:TextBoxWatermarkExtender>--%>
                    <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
                             <asp:ImageButton ID="imgAWBFromDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />     
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server" PopupButtonID="imgAWBFromDt"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate">
              </asp:CalendarExtender></td>
                    <td >
                        Date To</td>
                    <td  colspan="8">
                        <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
                 <asp:ImageButton ID="imgAWBToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server" PopupButtonID="imgAWBToDt" Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate">
              </asp:CalendarExtender>
                    </td>
                    <td >
                       
                    </td>
                    <td>
                        
                         <%--<asp:AutoCompleteExtender ID="txtAutoSource_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoSource">
               </asp:AutoCompleteExtender>--%>
                    </td>
                    
                    <td>
                        </td>
                    <td>
                        <%--
                         <asp:AutoCompleteExtender ID="txtAutoDest_AutoCompleteExtender" 
                            runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  
                            Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoDest">
               </asp:AutoCompleteExtender>--%>
                        </td>
                    <td class="style1">
                       </td>
                    <td class="style3">
                        </td>
                    <td>
                        &nbsp;</td>
                    <td class="style3">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        Country</td>
                    <td>
                    <asp:DropDownList ID="ddlCountry" runat="server"  Width ="100"
                            ></asp:DropDownList>
              
                      
                    </td>
                    <td >
                        Region</td>
                    <td ><asp:DropDownList ID="ddlRegion" runat="server"></asp:DropDownList>
                      
              
                    </td>
                    <td >
                       Origin
                    </td>
                    <td>
                    <%--<asp:DropDownList ID="ddlFlight" runat="server" 
                            onselectedindexchanged="ddlFlight_SelectedIndexChanged">
              </asp:DropDownList>--%>
                        
                    <asp:DropDownList ID="ddlOrigin" runat="server" Width="100" 
                            
                            AutoPostBack="True" Visible="true">
                        </asp:DropDownList></td>
                    
                    <td>
                        Destination</td>
                    <td>
                        <asp:DropDownList ID="ddlDestination" runat="server"  Width ="100" AutoPostBack="True" 
                         
                            Visible="true">
                        </asp:DropDownList></td>
                    <td class="style1">
                        &nbsp;</td>
                    <td class="style3">
                          &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="style3">
                        
                        </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        Status</td>
                    <td>
                          <asp:DropDownList ID="ddlStatus" runat="server">
                          <asp:ListItem Value ="All"></asp:ListItem>
                         <asp:ListItem Value ="ACTIVE"></asp:ListItem>
                         <asp:ListItem Value="CANCELLED"></asp:ListItem>
                         <asp:ListItem Value="DRAFT"></asp:ListItem>
                        </asp:DropDownList>
                        </td>
                    <td >
                      
                        <asp:CheckBox ID="chkDomestic" runat="server" AutoPostBack="True" 
                            Checked="True" Text="Domestic" ValidationGroup="A" />
                    </td>
                    <td >
                      
                        <asp:CheckBox ID="chkInternational" runat="server" Checked="True" 
                            Text="International" ValidationGroup="A" />
                    </td>
                    <td >
                         AirCraftType</td>
                    <td>
                        <asp:DropDownList ID="ddlAirCraftType" runat="server">
              </asp:DropDownList></td>
                    <td>
                         Flight #</td>
                    <td>
                        <asp:DropDownList ID="ddlFlightPrefix" runat="server" 
                            onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged" 
                            AutoPostBack="True"></asp:DropDownList>
              <asp:DropDownList ID="ddlFlightNumber" runat="server" ></asp:DropDownList></td>
                    <td class="style1">
                        
                    </td>
                    <td class="style3">
                        
                    </td>
                    <td>
                        &nbsp;</td>
                    <td class="style3">
                        
                        
                        
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2" colspan="13">
                        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                <asp:Button ID="btnclear" runat="server" Text="Clear" CssClass="button" onclick="btnclear_Click" Visible="true" />
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" onclick="btnExport_Click" /></td>
                </td>
                </tr>
                <tr>
                <td >
                
                
                <td>
                </td>
                
                </tr>
                <tr>
                    <td colspan="8">
                       <%-- <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label>--%>
             </td>
                    <td class="style1">
                        &nbsp;</td>
                    <td class="style3">
                        
                                            &nbsp;</td>
                    <td>
                        
                                            &nbsp;</td>
                    <td class="style3">
                        
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
           </table>
    
   
    
    </div>
   <div style=" border: thin solid #000000; float: left">
   
           <%-- <dd:WebReportViewer ID="rptViewerShowScedule" runat="server" Height="500px" Width="1000px" />--%>
            <rsweb:ReportViewer ID="RptFlightUtilizationViewer" runat="server" Width="1022px">
            </rsweb:ReportViewer>
         </div>
         </div>
    </asp:Content>
