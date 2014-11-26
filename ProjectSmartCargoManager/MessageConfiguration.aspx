<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="MessageConfiguration.aspx.cs" Inherits="ProjectSmartCargoManager.MessageConfiguration" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
<div id="contentarea">
    <h1> 
        
                     
                      <img alt="Message Configuration" src="Images/txtmessageconfi.png" /></h1>
                      
                      <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
        </p>
                      <br />
     <div class="divback">
     
     
      <table class="style1">
                          <tr>
                              <td>
                                  <asp:Label ID="lblOrigin" runat="server" Text="Origin"></asp:Label>
                              </td>
                              <td>
                                  <asp:TextBox ID="txtOrigin" runat="server"></asp:TextBox>
                              </td>
                              <td>
                                  <asp:Label ID="lblDestination" runat="server" Text="Destination"></asp:Label>
                              </td>
                              <td>
                                  <asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>
                              </td>
                              <td>
                                  <asp:Label ID="lblFlightNo" runat="server" Text="Flight No."></asp:Label>
                              </td>
                              <td>
                                  <asp:TextBox ID="txtFltNo" runat="server"></asp:TextBox>
                                 <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                      ControlToValidate="txtFltNo" ErrorMessage="*"></asp:RequiredFieldValidator>--%>
                                  <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                                      onclick="btnList_Click" />
                                  <asp:Button ID="btClear" runat="server" CssClass="button" 
                                      onclick="btClear_Click" Text="Clear" />
                              </td>
                          </tr>
                      </table>
                    <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">
                               Configuration</legend>

                      <table class="style1">
                          <tr>
                              <th>
                                  <asp:Label ID="lblMessageType" runat="server" Text="MessageType"></asp:Label>
                             </th>
                              <th>
                                  <asp:Label ID="lblVersion" runat="server" Text="Version"></asp:Label>
                              </th>
                              <th>
                                  <asp:Label ID="lblMailId" runat="server" Text="Email ID (multiple ID seperated by comma)"></asp:Label>
                              </th>
                          </tr>
                          <tr align="center">
                              <td>
                                  <asp:Label ID="lblFFM" runat="server" Text="FFM"></asp:Label>
                              </td>
                              <td>
                                  8</td>
                              <td>
                                  <asp:TextBox ID="txtEmailFFM" runat="server" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                  <asp:RegularExpressionValidator ID="FFMRegularExpressionValidator" 
                                      runat="server" ControlToValidate="txtEmailFFM" ErrorMessage="*" 
                                      ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" ValidationGroup="valConf"></asp:RegularExpressionValidator>
                              </td>
                          </tr>
                          <tr align="center">
                              <td>
                                  <asp:Label ID="lblFTX" runat="server" Text="FTX"></asp:Label>
                              </td>
                              <td>
                                  2</td>
                              <td>
                                  <asp:TextBox ID="txtEmailFTX" runat="server" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                  <asp:RegularExpressionValidator ID="FTXRegularExpressionValidator" 
                                      runat="server" ControlToValidate="txtEmailFTX" ErrorMessage="*" 
                                      ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" ValidationGroup="valConf"></asp:RegularExpressionValidator>
                              </td>
                          </tr>
                          <tr align="center">
                              <td>
                                  <asp:Label ID="lblFSU" runat="server" Text="FSU"></asp:Label>
                              </td>
                              <td>
                                  12</td>
                              <td>
                                  <asp:TextBox ID="txtEmailFSU" runat="server" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                  <asp:RegularExpressionValidator ID="FSURegularExpressionValidator" 
                                      runat="server" ControlToValidate="txtEmailFSU" ErrorMessage="*" 
                                      ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" ValidationGroup="valConf"></asp:RegularExpressionValidator>
                              </td>
                          </tr>
                          <tr align="right">
                              <td colspan="3">
                                  <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" 
                                      onclick="btnSave_Click" ValidationGroup="valConf" />
                              </td>
                          </tr>
                          <tr>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                          </tr>
                      </table>
                      </fieldset>
                              
                      
     </div>
     </div>
</asp:Content>
