<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmMessageConfiguration.aspx.cs" Inherits="ProjectSmartCargoManager.FrmMessageConfiguration" MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style1
        {
            width: 100%;
        }
        </style>
    <script language="javascript" type="text/javascript">

        function GetAirportORGCode() {
            var TXTORGClobject = '<%=txtOrigin.ClientID %>';
            var value = document.getElementById('<%=txtOrigin.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTORGClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetAirportDestCode() {
            var TXTDestClobject = '<%=txtDestination.ClientID %>';
            var value = document.getElementById('<%=txtDestination.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTDestClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
        function GetTransitDestCode() {
            var TXTDestClobject = '<%=txtTransitDest.ClientID %>';
            var value = document.getElementById('<%=txtTransitDest.ClientID %>').value;
            window.open('ListMultipleSelect.aspx?Parent=MaintainRatesORG&level=A&param=Origin&TargetTXT=' + TXTDestClobject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        }
            function GetMessageCode() {

                var TxtClientObject = '<%= txtMessageType.ClientID %>';
                var value = document.getElementById('<%= txtMessageType.ClientID %>').value;
                window.open('ListMultipleSelect.aspx?Parent=FrmMessageConfiguration&param=MsgType&TargetTXT=' + TxtClientObject + '&Values=' + value, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
            }
    </script>
    <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%> <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%> <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%> <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%> <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%> <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%>
<script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>
	<script type="text/javascript">


	    $(function() {
	    $('#<%=  ddlMsgCommType.ClientID %>').change(function() {
	    if (this.value == -1) {
	        
	        $('#<%=  txtEmailFFM.ClientID %>,#<%=  lblEmail.ClientID %> ').css({ display: 'block' });
	        $('#<%=  txtSitaID.ClientID %>,#<%=  lblSitaId.ClientID %> ').css({ display: 'block' });
	        $('#<%=  txtFTPID.ClientID %>,#<%=  lblFTPURL.ClientID %> ').css({ display: 'block' });
	        $('#<%=  txtFTPUserName.ClientID %>,#<%=  lblFTPUserName.ClientID %> ').css({ display: 'block' });
	        $('#<%=  txtFTPPassword.ClientID %>,#<%=  lblFTPPass.ClientID %> ').css({ display: 'block' });
	    }
	            if (this.value == 0) {
	                
	                $('#<%=  txtEmailFFM.ClientID %>,#<%=  lblEmail.ClientID %> ').css({ display: 'none' });
	                $('#<%=  txtSitaID.ClientID %>,#<%=  lblSitaId.ClientID %> ').css({ display: 'block' });
                    $('#<%=  txtFTPID.ClientID %>,#<%=  lblFTPURL.ClientID %> ').css({ display: 'none' });
                    $('#<%=  txtFTPUserName.ClientID %>,#<%=  lblFTPUserName.ClientID %> ').css({ display: 'none' });
                    $('#<%=  txtFTPPassword.ClientID %>,#<%=  lblFTPPass.ClientID %> ').css({ display: 'none' });
                }
                if (this.value == 1) {
                    
                    $('#<%=  txtEmailFFM.ClientID %>,#<%=  lblEmail.ClientID %> ').css({ display: 'block' });
                    $('#<%=  txtSitaID.ClientID %>,#<%=  lblSitaId.ClientID %> ').css({ display: 'none' });
                    $('#<%=  txtFTPID.ClientID %>,#<%=  lblFTPURL.ClientID %> ').css({ display: 'none' });
                    $('#<%=  txtFTPUserName.ClientID %>,#<%=  lblFTPUserName.ClientID %> ').css({ display: 'none' });
                    $('#<%=  txtFTPPassword.ClientID %>,#<%=  lblFTPPass.ClientID %> ').css({ display: 'none' });
                }
                if (this.value == 2) {
                    
                    $('#<%=  txtEmailFFM.ClientID %>,#<%=  lblEmail.ClientID %> ').css({ display: 'none' });
                    $('#<%=  txtSitaID.ClientID %>,#<%=  lblSitaId.ClientID %> ').css({ display: 'none' });
                    $('#<%=  txtFTPID.ClientID %>,#<%=  lblFTPURL.ClientID %> ').css({ display: 'block' });
                    $('#<%=  txtFTPUserName.ClientID %>,#<%=  lblFTPUserName.ClientID %> ').css({ display: 'block' });
                    $('#<%=  txtFTPPassword.ClientID %>,#<%=  lblFTPPass.ClientID %> ').css({ display: 'block' });
                }


	            
	        });
	    });
	</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
</asp:ToolkitScriptManager>
<div id="contentarea">
    <h1> 
        
                     
                      <img src="Images/txtmessageconfi.png" alt="Message Configuration" /></h1>
                      <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
               ></asp:Label>
        </p>
                      <br />
     <div class="divback">
     <asp:UpdatePanel ID="UPFourth" runat="server">
        <ContentTemplate>
     <table>
     <tr>
     <td> Partner Type</td>
     <td>
      <asp:DropDownList ID="ddlPartnerType" runat="server" Width="120px"  AutoPostBack="true"
              onselectedindexchanged="ddlPartnerType_SelectedIndexChanged">
          <asp:ListItem Selected="True" Text="Select"></asp:ListItem>
           <asp:ListItem Text="Agent"></asp:ListItem>
           <asp:ListItem Text="Airline"></asp:ListItem>
           <asp:ListItem Text="GHA"></asp:ListItem>
          </asp:DropDownList>
     </td>
     </tr>
     <tr>
     <td>Partner Code</td>
     <td>
      <asp:DropDownList ID="ddlPartnerCode" runat="server" Width="120px">
           </asp:DropDownList>
           <%--<asp:TextBox ID="txtPartnerCode" runat="server" ></asp:TextBox>
           <asp:TextBoxWatermarkExtender ID="txtPartnerCode_TextBoxWatermarkExtender" 
             runat="server" TargetControlID="txtPartnerCode" WatermarkText="Partner Code.." >
           </asp:TextBoxWatermarkExtender>
            <asp:AutoCompleteExtender ID="txtPartnerCode_AutoCompleteExtender" runat="server" 
             TargetControlID="txtPartnerCode" ServiceMethod="GetPartner" MinimumPrefixLength="1"  Enabled="True"  
             EnableCaching="true">
            </asp:AutoCompleteExtender>--%>
     </td>
     </tr>
     <tr>
     <td>Message Type:</td>
    <td align="center">
      <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
              <table>
                 <tr>
                 <%--<td>
                 <asp:CheckBox ID="chkSelectAll" runat="server" Text="Select All" 
                    AutoPostBack="True" OnClick="javascript:SelectAllStations(this);" 
                    meta:resource1key="chkSelectAllResource1" />
                 </td>--%>
                 
                 <%--<td><asp:CheckBoxList ID="chkListStation" runat="server"  
                        meta:resourcekey="chkListStationResource1" RepeatDirection="Horizontal">
                        </asp:CheckBoxList>
                 </td>--%>
                 
                 <td>
                 <asp:TextBox ID = "txtMessageType" runat="server"></asp:TextBox> 
                 <asp:ImageButton ID="ImageButton6" runat="server" ImageUrl="~/Images/list_bullets.png"
                 ImageAlign="AbsMiddle" OnClientClick="javascript:GetMessageCode();return false;" />
                 </td>
                 </tr>
                 </table> 
                </div>
        </ContentTemplate>
      </asp:UpdatePanel>
   </td>
     </tr>
     </table>
      </ContentTemplate>
     </asp:UpdatePanel>
      </br>
      
      
      <fieldset>  <legend style=" font-weight:bold;  color:#999999; padding:5px;" xml:lang="">
                               Configuration</legend>
<table class="style1">
                          <tr>
                              <td>
                                  <asp:Label ID="lblOrigin" runat="server" Text="Origin"></asp:Label>
                                  
                              </td>
                              <td>
                                  <asp:TextBox ID="txtOrigin" runat="server"></asp:TextBox>
                                 <%--  <asp:TextBoxWatermarkExtender ID="txtOrigin_TextBoxWatermarkExtender" WatermarkText="Origin Code" runat="server" TargetControlID="txtOrigin">
                                   </asp:TextBoxWatermarkExtender>
                                --%> <%--  <asp:AutoCompleteExtender ID="txtOrigin_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtOrigin">
                                   </asp:AutoCompleteExtender>

--%>
                                                 <asp:ImageButton runat="server" 
                                      OnClientClick="javascript:GetAirportORGCode();return false;" 
                                      ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png" ID="imgOrigin"></asp:ImageButton>


                              </td>
                              <td>
                                  <asp:Label ID="lblDestination" runat="server" Text="Destination"></asp:Label>
                              </td>
                              <td>
                                  <asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>
                                  <%--<asp:TextBoxWatermarkExtender ID="txtDestination_TextBoxWatermarkExtender" WatermarkText="Destination Code" runat="server" TargetControlID="txtDestination">
                                   </asp:TextBoxWatermarkExtender>
                                  --%><%-- <asp:AutoCompleteExtender ID="txtDestination_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtDestination">
                                   </asp:AutoCompleteExtender>--%>


                                                 <asp:ImageButton runat="server" 
                                      OnClientClick="javascript:GetAirportDestCode();return false;" 
                                      ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png" ID="imgDest" 
                                    ></asp:ImageButton>


                              </td>
                              <td>
                               Transit Destination
                              </td>
                              <td>
                               <asp:TextBox ID="txtTransitDest" runat="server"></asp:TextBox>
                              <%-- <asp:TextBoxWatermarkExtender ID="txtTransitDest_TextBoxWatermarkExtender" WatermarkText="Transit Destination Code" runat="server" TargetControlID="txtTransitDest">
                                   </asp:TextBoxWatermarkExtender>
                              --%>    <%-- <asp:AutoCompleteExtender ID="txtTransitDest_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" TargetControlID="txtTransitDest">
                                   </asp:AutoCompleteExtender>--%>


                                                 <asp:ImageButton runat="server" 
                                      OnClientClick="javascript:GetTransitDestCode();return false;" 
                                      ImageAlign="AbsMiddle" ImageUrl="~/Images/list_bullets.png" ID="imgTransitDest" 
                                     ></asp:ImageButton>


                              </td>
                              </tr>
                              <tr>
                            
                              <td>
                                  <asp:Label ID="lblFlightNo" runat="server" Text="Flight No."></asp:Label>
                              </td>
                              <td>
                                  <asp:TextBox ID="txtFltNo" runat="server"></asp:TextBox>
                               <%--   <asp:TextBoxWatermarkExtender ID="txtFltNo_TextBoxWatermarkExtender" WatermarkText="Enter Flight Number" runat="server" TargetControlID="txtFltNo">
                                   </asp:TextBoxWatermarkExtender>
                                --%>   <asp:AutoCompleteExtender ID="txtFltNo_AutoCompleteExtender" runat="server" 
                                        TargetControlID="txtFltNo" ServiceMethod="GetFlight" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/ArrivalReassign.aspx">
                                   </asp:AutoCompleteExtender>
                              </td>
                              <td>
                               Message Comm Type
                              </td>  
                              <td>
                              <%--<select name="dllSelect" id="dllSelect" onchange="call()" style="width:120px;">
                                <option>All</option>
                                <option>SITA</option>
                                <option>Email</option>
                                <option>FTP</option>
                              </select>--%>
                                 <asp:DropDownList ID="ddlMsgCommType" runat="server" Width="120px" AutoPostBack="True" onchange="return FilterStatus()" AppendDataBoundItems="true">
                                 <asp:ListItem Selected="True" Text="All" Value="-1"></asp:ListItem>
                                 <asp:ListItem Text="SITA" Value="0"></asp:ListItem>                                 
                                 <asp:ListItem Text="Email" Value="1"></asp:ListItem>
                                 <asp:ListItem Text="FTP" Value="2"></asp:ListItem>
                                  </asp:DropDownList>
                                  </td>  
                              <td>
                                  Auto Generate</td>
                              <td>
                                  <asp:CheckBox ID="chkAutoGenerate" runat="server" />
                                  </td>
                              
                          </tr>
                              
                              <tr>
                            
                              <td>
                                  Messaging Start With</td>
                              <td>
                                  <asp:TextBox ID="txtMsgStart" runat="server"></asp:TextBox>
                              </td>
                              <td>
                                  Messaging END With</td>  
                              <td>
                                  <asp:TextBox ID="txtMsgEnd" runat="server"></asp:TextBox>
                                  </td>  
                              <td>
                                  &nbsp;</td>
                              <td>
                                  &nbsp;</td>
                              
                          </tr>
                              
                      </table>
                      <table class="style1">
                          <%--<tr>
                              <th>
                                  <asp:Label ID="lblMessageType" runat="server" Text="MessageType"></asp:Label>
                             </th>
                              <th>
                                  <asp:Label ID="lblVersion" runat="server" Text="Version"></asp:Label>
                              </th>
                              <th>
                                  <asp:Label ID="lblMailId" runat="server" Text="Email ID (multiple ID seperated by comma)"></asp:Label>
                              </th>
                          </tr>--%>
                          <tr>
                              <td width="80px">
                               
                                  <asp:Label ID="lblEmail" runat="server" Text="Email Id"></asp:Label>
                              </td>
                              <td colspan="3">
                                  <asp:TextBox ID="txtEmailFFM" runat="server" TextMode="MultiLine" Width="80%"></asp:TextBox>
                                 <%-- <asp:RegularExpressionValidator ID="FFMRegularExpressionValidator" 
                                      runat="server" ControlToValidate="txtEmailFFM" ErrorMessage="*" 
                                      ValidationExpression="((\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*([,])*)*" ValidationGroup="valConf"></asp:RegularExpressionValidator>--%>
                              </td>
                          </tr>
                          <%--<tr align="center">
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
                          </tr>--%>
                          <tr>
                              <td width="80px">
                              
                                  <asp:Label ID="lblSitaId" runat="server" Text="Sita ID"></asp:Label>
                              </td>
                              <td colspan="3"><asp:TextBox ID="txtSitaID" runat="server" TextMode="MultiLine" 
                                      Width="80%"></asp:TextBox>
                                  &nbsp;</td>
                          </tr>
                          <tr>
                              <td width="80px">
                                  <asp:Label ID="lblFTPURL" runat="server" Text="FTP URL"></asp:Label>
                              </td>
                                  <td><asp:TextBox ID="txtFTPID" runat="server" ></asp:TextBox></td>
                              <td>
                                  <asp:Label ID="lblFTPUserName" runat="server" Text="FTP User Name"></asp:Label>
                              </td>
                                  <td>
                                  <asp:TextBox ID="txtFTPUserName" runat="server"></asp:TextBox></td>
                                  
                              <td>
                                  <asp:Label ID="lblFTPPass" runat="server" Text="FTP Password"></asp:Label>
                              </td>
                                  <td>
                                  <asp:TextBox ID="txtFTPPassword" runat="server"></asp:TextBox></td>
                                  
                              <td>
                                  &nbsp;</td>
                                  
                          </tr>
                          <tr align="right">
                              <td colspan="4">
                                  &nbsp;</td>
                              <td>
                              </td>
                              <td>
                                  <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" 
                                    ValidationGroup="valConf" onclick="btnSave_Click" />
                              </td>
                          </tr>
                          </table>
                      </fieldset>
                              
                      
     </div>
     </div>
</asp:Content>
