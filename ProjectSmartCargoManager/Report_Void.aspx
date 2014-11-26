<%@ Page  Title="Void AWB Report" Language="C#" AutoEventWireup="true" CodeBehind="Report_Void.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.Report_Void" %>

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
     <asp:ToolkitScriptManager ID="TSM0" runat="server" >
    </asp:ToolkitScriptManager >
    

     <div id="contentarea">
   <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
</div>
    <%--<h1><img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
    
       <h1>Void AWB Report</h1>
       <%--<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label>--%>
          <div class="botline">
            <table width="80%">
                <tr>
                    <td>
                        Origin</td>
                    <td>
                   <%-- <asp:TextBox ID="txtAutoSource" runat="server" Width="100px" ToolTip="City Code Ex-BOM" ></asp:TextBox>
                        <asp:AutoCompleteExtender ID="txtAutoSource_AutoCompleteExtender" 
                            runat="server"  ServicePath="~/Report_Void.aspx" ServiceMethod="GetStation" 
                            TargetControlID="txtAutoSource"  MinimumPrefixLength="1" EnableCaching="true" Enabled="true">
                        </asp:AutoCompleteExtender>--%>
                       <asp:DropDownList ID="ddlOrg" runat="server" Width="70px">
         </asp:DropDownList>  
                    </td>
                    <td >AWB #
                        </td>
                    <td >
                    <asp:TextBox ID="txtAWBPrefix" runat="server" Width=45></asp:TextBox>
                    <asp:TextBox ID="txtAWBNo" runat="server" Width="100px"></asp:TextBox>                        
                    </td>
                    <td >From Date
                        
                    </td>
                    <td>
               <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="74px" ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate" PopupButtonID="imgFrmDt">
              </asp:CalendarExtender>
                        <asp:ImageButton ID="imgFrmDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                       
                    </td>
                    
                    <td>To Date
                        </td>
                    <td>
               <asp:TextBox ID="txtFlightToDate" runat="server" Width="74px" ToolTip="Please enter valid date format: dd/MM/yyyy" ></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate" PopupButtonID="imgToDt">
              </asp:CalendarExtender>
                        <asp:ImageButton ID="imgToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" CausesValidation="False" />
                        </td>
                    <td class="style1">
                        </td>
                    <td>
              <asp:TextBox ID="txtFlightNo" runat="server" Width="100px" Visible="false" Enabled="false"></asp:TextBox>
                        </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                        </td>
                    <td>
                    
              
                    </td>
                    <td >
                        </td>
                    <td >
                    <asp:TextBox ID="txtAutoDest" runat="server" Width="100px" Visible="false"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoDest_AutoCompleteExtender" 
                            runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  
                            Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoDest">
               </asp:AutoCompleteExtender>
                      
              
                    </td>
                    <td >
                        &nbsp;</td>
                    <td>
                    <%--<asp:DropDownList ID="ddlFlight" runat="server" 
                            onselectedindexchanged="ddlFlight_SelectedIndexChanged">
              </asp:DropDownList>--%>
                        <asp:DropDownList ID="ddlAirCraftType" runat="server" Visible="False">
              </asp:DropDownList>
                    </td>
                    
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                          &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                       
                        </td>
                    <td>
                        &nbsp;</td>
                </tr>
                
                
           </table>
    <asp:TextBox ID="txtCountry" runat="server" Width="100px" 
                            ToolTip="Country code Ex- IN" ontextchanged="txtCountry_TextChanged" Visible="false"></asp:TextBox>
                            
                            <asp:TextBox ID="txtRegion" runat="server" 
                            Width="100px" Visible="false"></asp:TextBox>
                            
                             <asp:DropDownList ID="ddlOrigin" runat="server" 
                            
                            AutoPostBack="True" Visible="False">
                        </asp:DropDownList>
                            
                              <asp:DropDownList ID="ddlStatus" runat="server" Visible="False">
                          <asp:ListItem Value ="All"></asp:ListItem>
                         <asp:ListItem Value ="Complete"></asp:ListItem>
                         <asp:ListItem Value="InComplete"></asp:ListItem>
                         
                        </asp:DropDownList>
                            
                            
                            <asp:DropDownList ID="ddlDestination" runat="server" AutoPostBack="True" 
                         
                            Visible="False">
                        </asp:DropDownList>
                            <br />
    <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                        
                                            
                        
                                            <asp:Button ID="btnclear" runat="server" Text="Clear" 
                            CssClass="button" onclick="btnclear_Click" 
                                             />
    
                                            
                        
                                            <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" 
                                             />
    
    </div>
    
  
        <div style=" border: thin solid #000000; float: left; ">
            
            <rsweb:ReportViewer ID="ReportViewer1" runat="server"  
                Width="1024px">
            </rsweb:ReportViewer>
         
            
            <dd:WebReportViewer ID="rptViewerAWBMovement" runat="server" Height="500px" Width="1000px" /></div>
            

         
         </div>
    
  
    </asp:Content>

