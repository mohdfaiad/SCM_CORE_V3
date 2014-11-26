
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptDailyFlightSchedule.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptDailyFlightSchedule" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 
 
    <style type="text/css">
        .style1
        {
        }
    </style>
 
 
 
</asp:Content>
 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
                   
     <div id="contentarea">
     <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
</div>
        <h1>
            Active Flight Report</h1>
       
          <div class="botline">
     <table width="100%">
                <tr>
                    <td>
                        Country</td>
                    <td>
                    <asp:DropDownList ID="ddlCountry" runat="server" width="130"></asp:DropDownList>
                        <%--<asp:TextBox ID="txtCountry" runat="server" Width="100px" 
                            ToolTip="Country code Ex- IN"></asp:TextBox>--%>
                         <%--  <asp:TextBoxWatermarkExtender ID="txtCountry_TextBoxWatermarkExtender"  WatermarkText="User Name" 
                    runat="server" TargetControlID="txtUserName">
                </asp:TextBoxWatermarkExtender>--%>
                    </td>
                    <td >
                        Region</td>
                    <td ><asp:DropDownList ID="ddlRegion" runat="server" Width="130"></asp:DropDownList>
                        <%--<asp:TextBox ID="txtRegion" runat="server" 
                            Width="100px"></asp:TextBox>--%>
                    </td>
                    <td >
                        Origin
                    </td>
                    <td>
                    <asp:DropDownList ID="ddlOrigin" runat="server" width="130" Visible="true">
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txtAutoSource" runat="server" Width="100px"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoSource_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoSource">
               </asp:AutoCompleteExtender>--%>
                    </td>
                    
                    <td>
                        Destination</td>
                    <td> 
                        <asp:DropDownList ID="ddlDestination" runat="server" Width="130" 
                         
                            Visible="True">
                        </asp:DropDownList>
                        <%--<asp:TextBox ID="txtAutoDest" runat="server" Width="100px"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoDest_AutoCompleteExtender" 
                            runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  
                            Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoDest">
               </asp:AutoCompleteExtender>--%>
                        </td>
                    <td class="style1">
                      </td>
                    <td>
                    
              <%--<asp:TextBox ID="txtFlightNo" runat="server" Width="100px"></asp:TextBox>--%>
                        </td>
                </tr>
                <tr>
                    <td>
                        Date From</td>
                    <td>
              <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate" PopupButtonID="imgFrmDt">
              </asp:CalendarExtender>
                    
                    <asp:ImageButton ID="imgFrmDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                    
                    </td>
                    <td >
                        Date To</td>
                    <td >
                      
              <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate" PopupButtonID="imgToDt">
              </asp:CalendarExtender>
                    
                    <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                    </td>
                    <td >
                        Aircraft Type</td>
                    <td>
                    <%--<asp:DropDownList ID="ddlFlight" runat="server" 
                            onselectedindexchanged="ddlFlight_SelectedIndexChanged">
              </asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlAirCraftType" runat="server">
              </asp:DropDownList>
                    </td>
                    
                    <td>
                          Flight #</td>
                    <td>
                         <asp:DropDownList ID="ddlFlightPrefix" runat="server" Width="45" 
                             onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged"></asp:DropDownList>
                    <asp:DropDownList ID="ddlFlightNumber" runat="server" Width="100"></asp:DropDownList></td>
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                          &nbsp;</td>
                </tr>
                <tr>
                    <td>
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
                      
                        &nbsp;</td>
                    <td >
                      
                          &nbsp;</td>
                    <td >
                    
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        
                    </td>
                    <td>
                        
                    </td>
                    <td>
                        
    
                                            
                        
                                            
    
                    </td>
                    <td>
                        
                                                                    
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButton ID="rbActiveSchedule" runat="server" Checked="True" GroupName="A" 
                            Text="Active Schedule" /></td>
                    <td>
                      
                                                   <asp:RadioButton ID="rbMasterSchedule" runat="server" GroupName="A" 
                            Text="Master Schedule" /></td>
                    <td >
                      
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td >
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        
                        &nbsp;</td>
                    <td>
                        
                        &nbsp;</td>
                    <td>
                        
    
                                            
                        
                                            
    
                        &nbsp;</td>
                    <td>
                        
                                                                    
                        
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="10">
                        <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                         &nbsp;
                <asp:Button ID="btnclear" runat="server" Text="Clear" 
                            CssClass="button" onclick="btnclear_Click" /> &nbsp;
                         <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" />
                    </td>   
                </tr>
                <tr>
                <td>
                
                
                       
                </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <%--<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label>--%>
             </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        
                                            &nbsp;</td>
                </tr>
           </table>
    
   
    
    </div>
  
        <div style=" border: thin solid #000000; float: left;">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" 
                Width="1024px">
            </rsweb:ReportViewer>
          
           <%--<dd:WebReportViewer ID="rptViewerShowScedule" runat="server" Height="500px" Width="1000px" />--%>
            <rsweb:ReportViewer ID="ReportViewer2" runat="server"  Width="1024px">
            </rsweb:ReportViewer>
            </div>
            
        
      
         
         </div>
    
  
    </asp:Content>
