<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmCTM.aspx.cs" Inherits="ProjectSmartCargoManager.frmCTM" MasterPageFile="~/SmartCargoMaster.Master" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<script language="javascript" type="text/javascript">

    function popup() {

        var Fltdt = document.getElementById("<%= txtFDate.ClientID %>").value;
        var Fltcd = document.getElementById("<%= txtFromPre.ClientID %>").value;

        var Fltid = document.getElementById("<%= txtFromFlight.ClientID %>").value;
        var fltno = Fltcd + Fltid;


        window.open('UCRPopup.aspx?Type=New' + '&Mode=M' + '&FlightNo=' + fltno + '&FlightDate=' + Fltdt + '&pg=Ctm', '', 'left=0,top=0,width=1000,height=1000,toolbar=0,resizable=0');
    }

</script>   
    <br />
    <br />
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
     <div id="contentarea">
     <asp:Label ID="lblMsg" runat="server" ForeColor="Red" Font-Size="Large" Font-Bold="true"></asp:Label>
      <h1>
         <asp:Label ID="lblTitle" runat="server" Text="CTM [Transfer Manifest]"></asp:Label>
             </h1>
     <div style="margin-top:15px;">
     <table width="100%">
     <tr>
     <td style="width:25%;">CTM Ref&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtCTMRef" runat="server" Width="150px"></asp:TextBox></td>
     <td style="width:20%;">CTM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlCTM" runat="server">
         <asp:ListItem Selected="True">Select</asp:ListItem>
         <asp:ListItem>Inbound</asp:ListItem>
         <asp:ListItem>Outbound</asp:ListItem>
         </asp:DropDownList></td>
     <td style="width:22%;">Creation Date&nbsp;&nbsp; <asp:TextBox ID="txtCreateDate" runat="server" Width="80px"></asp:TextBox>
         <asp:CalendarExtender ID="txtCreateDate_CalendarExtender" runat="server" 
             Enabled="True" TargetControlID="txtCreateDate">
         </asp:CalendarExtender>
         </td>
      <td style="width:20%;">AWB #&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtAWBPre" runat="server" 
             Width="30px"></asp:TextBox>
     <asp:TextBox ID="txtAWBNo" runat="server" Width="70px"></asp:TextBox>
         </td>
     <td style="width:10%;">Airport&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="lblAirport" runat="server"></asp:Label></td>
     </tr>
     <tr>
     <td>From Carrier&nbsp;&nbsp; <asp:TextBox ID="txtFromPre" runat="server" Width="50px"></asp:TextBox>
          <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtFromPre" runat="server" ControlToValidate="txtFromPre"  ErrorMessage="*"></asp:RequiredFieldValidator>

     <asp:TextBox ID="txtFromFlight" runat="server" Width="70px"></asp:TextBox>
     <asp:RequiredFieldValidator ID="txtFromFlight_RequiredFieldValidator" runat="server" ControlToValidate="txtFromFlight"  ErrorMessage="*"></asp:RequiredFieldValidator>
     </td>
     <td>
     Flight Date&nbsp;&nbsp; <asp:TextBox ID="txtFDate" runat="server"  Width="80px"></asp:TextBox>
         <asp:CalendarExtender ID="txtFDate_CalendarExtender" runat="server" 
             Enabled="True" TargetControlID="txtFDate">
         </asp:CalendarExtender>
         <asp:RequiredFieldValidator ID="txtFDate_RequiredFieldValidator" runat="server" ControlToValidate="txtFDate"  ErrorMessage="*"></asp:RequiredFieldValidator>
     </td>
     <td >
     To Carrier&nbsp;&nbsp;<asp:TextBox ID="txtToPre" runat="server" Width="50px"></asp:TextBox>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator_txtToPre" runat="server" ControlToValidate="txtToPre" ValidationGroup="save" ErrorMessage="*"></asp:RequiredFieldValidator>
     <asp:TextBox ID="txtToFlight" runat="server" Width="70px"></asp:TextBox>
     <asp:RequiredFieldValidator ID="txtToFlight_RequiredFieldValidator" runat="server" ControlToValidate="txtToFlight" ValidationGroup="save" ErrorMessage="*"></asp:RequiredFieldValidator>
     </td>
     <td>
     Flight Date&nbsp;&nbsp; <asp:TextBox ID="txtTDate" runat="server"  Width="80px"></asp:TextBox>
         <asp:CalendarExtender ID="txtTDate_CalendarExtender" runat="server" 
             Enabled="True" TargetControlID="txtTDate">
         </asp:CalendarExtender>
         <asp:RequiredFieldValidator ID="txtTDate_RequiredFieldValidator" runat="server" ControlToValidate="txtTDate" ValidationGroup="save" ErrorMessage="*"></asp:RequiredFieldValidator>
     </td>
     <td>
     <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
             onclick="btnList_Click" />
     &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" CausesValidation="false" OnClick="btnClear_Click" />
     </td>
     </tr>
     </table>
     </div>
     </br>
     <%--<div style="margin-top:10px;overflow:auto;width:800px;" >--%>
     <div style="margin-top:10px;" >
     <asp:GridView ID="grdULDDetails" runat="server"
               AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
               CellPadding="2" CellSpacing="3">
               <Columns>
                   <asp:TemplateField>
                       <ItemTemplate>
                           <asp:CheckBox ID="checkULD" runat="server" />
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="ULD #" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDNo" runat="server" AutoPostBack="true" EnableViewState="true" 
                                Text='<%# Eval("ULDNo") %>' Width="80px" Enabled="false"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="AWB #" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdAWBNo" runat="server" AutoPostBack="true" EnableViewState="true" 
                                Text='<%# Eval("AWBNumber") %>' Width="80px" Enabled="false"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt#" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' 
                               Width="60px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt Dt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDFlightDate" runat="server" Text='<%# Eval("FlightDate") %>' 
                               Width="70px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Orgin" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDOrg" runat="server" Text='<%# Eval("Origin") %>' Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Dest" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDDestn" runat="server" Text='<%# Eval("Dest") %>' Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="POL" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDPOL" runat="server" Text='<%# Eval("POL") %>' Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="ULD Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDULDWt" runat="server" Text='<%# Eval("ULDWt") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Scale Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDScaleWt" runat="server" Text='<%# Eval("ScaleWt") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="AWB Ct" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDAWBCount" runat="server" Text='<%# Eval("AWBCount") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="AWB Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDAWBPcs" runat="server" Text='<%# Eval("AWBPcs") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="AWB Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDAWBWt" runat="server" Text='<%# Eval("AWBWt") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <%--<asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Accepted Pcs." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDStdPcs" runat="server" Text='<%# Eval("StatedPCS") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Accepted Wt." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDStdWt" runat="server" Text='<%# Eval("StatedWgt") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="true" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Manifested Pcs." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDMftPcs" runat="server" 
                               Text='<%# Eval("PCS") %>' Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Manifested Wt." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDMftWt" runat="server" Text='<%# Eval("GrossWgt") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arrived Pcs." 
                       ItemStyle-Wrap="false" Visible="false">                       
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDArrivedPcs" runat="server" 
                               Text='<%# Eval("ArrivedPieces") %>' Width="40px" Enabled="false">   
                    </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arrived Wt." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDArrivedWt" runat="server" 
                               Text='<%# Eval("ArrivedWeight") %>' Width="40px" Enabled="false">
                    </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Received Pcs." 
                       ItemStyle-Wrap="false" Visible="false">                       
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDRcvPcs" runat="server" ReadOnly="true" 
                               onchange="javascript:calculateActualWt(this);" Text="0" Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Received Wt." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDRcvWt" runat="server" ReadOnly="true" Text="0" Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>                 
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Remaining Pcs." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDExpectedpcs" runat="server" Text='<%# Eval("ExpectedPcs") %>' 
                               Width="40px" Enabled="false"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Remaining Wt." 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDExpectedWeight" runat="server" 
                               Text='<%# Eval("ExpectedWeight") %>' Width="40px" Enabled="false"></asp:TextBox>
                           
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>--%>
                   
                   <%--<asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Security Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="SecurityCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="CustomCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Status Code" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="CustomStatusCode" runat="server" Text="" Width="55px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Remarks" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Remark" runat="server" Width="80px"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>--%>
                   
                   
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Location" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDLocation" runat="server" AutoPostBack="true" EnableViewState="true" 
                               Text='<%# Eval("Location") %>' Width="80px"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <%--<asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Arrived" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="isarrived" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>--%>
                   
               </Columns>
               <HeaderStyle CssClass="titlecolr" />
               <RowStyle HorizontalAlign="Center" />
               <AlternatingRowStyle HorizontalAlign="Center" />
           </asp:GridView>
      </br>
      
     <asp:GridView ID="gvCTM" runat="server" AutoGenerateColumns="False">
     <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
     <Columns>
     
     <asp:TemplateField HeaderText="">
     <ItemTemplate>
         <asp:CheckBox ID="chkgvCTM" runat="server" />
     </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField HeaderText="AWB #">
     <ItemTemplate>
     <asp:Label ID="lblAWB" runat="server" Text='<%# Eval("AWBNo") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="ULD #">
     <ItemTemplate>
     <asp:Label ID="lblULD" runat="server" Text='<%# Eval("ULDNo") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Owner Code">
     <ItemTemplate>
     <asp:Label ID="lblOwnerCode" runat="server" Text='<%# Eval("OwnerCode") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Std Pcs">
     <ItemTemplate>
     <asp:Label ID="lblStdPcs" runat="server" Text='<%# Eval("StdPcs") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Std Wt">
     <ItemTemplate>
     <asp:Label ID="lblStdWgt" runat="server" Text='<%# Eval("StdWt") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
       <asp:TemplateField HeaderText="Wt Unit">
     <ItemTemplate>
     <asp:Label ID="lblWgtUnit" runat="server" Text='<%# Eval("StdWtUnit") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Std Vol">
     <ItemTemplate>
     <asp:Label ID="lblStdVol" runat="server" Text='<%# Eval("StdVol") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Vol Unit">
     <ItemTemplate>
     <asp:Label ID="lblVolUnit" runat="server" Text='<%# Eval("VolUnit") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Act Pcs">
     <ItemTemplate>
     <asp:Label ID="lblActPcs" runat="server" Text='<%# Eval("ActPcs") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Act Wt">
     <ItemTemplate>
     <asp:Label ID="lblActWgt" runat="server" Text='<%# Eval("ActWt") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Wt Unit">
     <ItemTemplate>
     <asp:Label ID="lblWtUnit" runat="server" Text='<%# Eval("ActWtUnit") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Act Vol">
     <ItemTemplate>
     <asp:Label ID="lblActVol" runat="server" Text='<%# Eval("ActVol") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Vol Unit">
     <ItemTemplate>
     <asp:Label ID="lblActVolUnit" runat="server" Text='<%# Eval("ActVolUnit") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Origin">
     <ItemTemplate>
     <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Dest">
     <ItemTemplate>
     <asp:Label ID="lblDest" runat="server" Text='<%# Eval("Dest") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="SCC">
     <ItemTemplate>
     <asp:Label ID="lblSCC" runat="server" Text='<%# Eval("SCC") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Ship Details">
     <ItemTemplate>
     <asp:Label ID="lblShipDetails" runat="server" Text='<%# Eval("ShipDetails") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Remarks">
     <ItemTemplate>
     <asp:Label ID="lblRemarks" runat="server" Text='<%# Eval("Remarks") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     </Columns>
     <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
     <HeaderStyle CssClass="titlecolr"></HeaderStyle>
     <RowStyle CssClass="grdrowfont"></RowStyle>
     </asp:GridView>
     
     <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
             onclick="btnSave_Click" ValidationGroup="save" />
     &nbsp;
     <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="button" 
             onclick="btnPrint_Click" />
       &nbsp;
       <asp:Button ID="btnprintUCR" runat="server" 
                        Text="Print UCR" CssClass="button" OnClientClick="popup()" Visible="true"/>&nbsp;
     <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" />
     
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" 
            Font-Size="8pt" Height="301px" Width="767px" Visible="False">
            <LocalReport ReportPath="Reports\EAWB.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" 
                        Name="DataSet1_DataTable1" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
    
     </div>
     </div>
</asp:Content>
