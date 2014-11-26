<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rptOverviewAWB.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.rptOverviewAWB" %>

<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2"
    Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
 
 
    <style type="text/css">
        .style1
        {
        }
        .style2
        {
            height: 27px;
        }
    </style>
 
 
 
</asp:Content>

 <asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
     <div id="contentarea">
   
    
    <%--<h1><img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
    
       <h1>AWB Movement Overview</h1>
          <div class="botline">
            <table width="100%">
                <tr>
                    <td>
                        Country</td>
                    <td>
                        <asp:TextBox ID="txtCountry" runat="server" Width="100px" 
                            ToolTip="Country code Ex- IN"></asp:TextBox>
                         <%--  <asp:TextBoxWatermarkExtender ID="txtCountry_TextBoxWatermarkExtender"  WatermarkText="User Name" 
                    runat="server" TargetControlID="txtUserName">
                </asp:TextBoxWatermarkExtender>--%>
                    </td>
                    <td >
                        Region</td>
                    <td >
                        <asp:TextBox ID="txtRegion" runat="server" 
                            Width="100px"></asp:TextBox>
                    </td>
                    <td >
                        Origin
                    </td>
                    <td>
                        <asp:TextBox ID="txtAutoSource" runat="server" Width="100px"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoSource_AutoCompleteExtender" runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoSource">
               </asp:AutoCompleteExtender>
                    </td>
                    
                    <td>
                        Destination</td>
                    <td>
                        <asp:TextBox ID="txtAutoDest" runat="server" Width="100px"></asp:TextBox>
                         <asp:AutoCompleteExtender ID="txtAutoDest_AutoCompleteExtender" 
                            runat="server"  ServiceMethod="GetStation" MinimumPrefixLength="1"  
                            Enabled="True" ServicePath="~/Home.aspx"  
               EnableCaching="true"  TargetControlID="txtAutoDest">
               </asp:AutoCompleteExtender>
                        </td>
                    <td class="style1">
                        Flight #</td>
                    <td>
              <asp:TextBox ID="txtFlightNo" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
               
                <%--<tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
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
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>--%>
                
                <tr>
                    <td>
                        Date From</td>
                    <td>
              <asp:TextBox ID="txtFlightFromdate" runat="server"  Width="100px" 
                            ToolTip="Please enter date format: dd/MM/yyyy"></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightFromdate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightFromdate">
              </asp:CalendarExtender>
                    </td>
                    <td >
                        Date To</td>
                    <td >
                      
              <asp:TextBox ID="txtFlightToDate" runat="server" Width="100px" 
                          ToolTip="Please enter valid date format: dd/MM/yyyy" 
                 ></asp:TextBox>
              <asp:CalendarExtender ID="txtFlightToDate_CalendarExtender" runat="server"  Format="dd/MM/yyyy"
                  Enabled="True" TargetControlID="txtFlightToDate">
              </asp:CalendarExtender>
                    </td>
                    <td >
                        Based On :</td>
                    <td>
                        <asp:DropDownList ID="ddldateselection" runat="server" ValidationGroup="list">
                        <asp:ListItem>Execution Date</asp:ListItem>
                        <asp:ListItem>Flight Date</asp:ListItem>
              </asp:DropDownList>
              <asp:RequiredFieldValidator ID="Rqrdvalidator" runat="server" ControlToValidate="ddldateselection" Text="*"></asp:RequiredFieldValidator>
                    </td>
                    
                    <td>
                        AWB #</td>
                    <td>
                    <asp:TextBox ID="txtAWBPrefix" runat="server" Width="40px" MaxLength="3"></asp:TextBox>
                        <asp:TextBox ID="txtAWBNo" runat="server" Width="100px"></asp:TextBox>
                        </td>
                    <td class="style1">
                        &nbsp;</td>
                    <td>
                          &nbsp;</td>
                    <td>
                        &nbsp;</td>
                    <td>
                        <asp:DropDownList ID="ddlOrigin" runat="server" 
                            
                            AutoPostBack="True" Visible="False">
                        </asp:DropDownList>
                        </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style2">
                        &nbsp;</td>
                    <td class="style2">
                        </td>
                    <td class="style2" >
                      
                        </td>
                    <td class="style2" >
                      
                          <asp:DropDownList ID="ddlStatus" runat="server" Visible="False">
                          <asp:ListItem Value ="All"></asp:ListItem>
                         <asp:ListItem Value ="Complete"></asp:ListItem>
                         <asp:ListItem Value="InComplete"></asp:ListItem>
                         
                        </asp:DropDownList>
                    </td>
                    <td class="style2" >
                        </td>
                    <td class="style2">
                        </td>
                    <td class="style2">
                        </td>
                    <td class="style2">
                        </td>
                    <td class="style2">
                        
    
                                            
                        
                                            
    
                    </td>
                    <td class="style2">
                        
                                                                    
                        
                    </td>
                    <td class="style2">
                        
                                            </td>
                    <td class="style2">
                        
                        <asp:DropDownList ID="ddlDestination" runat="server" AutoPostBack="True" 
                         
                            Visible="False">
                        </asp:DropDownList>
                        
                    </td>
                    <td class="style2">
                        </td>
                </tr>
                <tr>
                <td colspan="8">
                <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" ValidationGroup="list" />
                <asp:Button ID="btnExport" runat="server" Text="Export" Visible="false"
                            CssClass="button" onclick="btnExport_Click" />
                <asp:Button ID="btnclear" runat="server" Text="Clear" 
                            CssClass="button" onclick="btnclear_Click" />
                </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label></td>
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
           </table>
    
   
    
    </div>
    
    <table width="100%">
        <tr>
        <td>
            
            <dd:WebReportViewer ID="rptViewerAWBMovement" runat="server" Height="500px" Width="1000px" />
            
                    </td>  
            
            
</tr>
   </table>
         
         </div>
    
  
    </asp:Content>

