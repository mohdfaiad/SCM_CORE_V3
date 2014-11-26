<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="CTMNew.aspx.cs" Inherits="ProjectSmartCargoManager.CTMNew" %>
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
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
     <div id="contentarea">
     <asp:Label ID="lblMsg" runat="server" ForeColor="Red" CssClass="msg" Font-Size="Large" Font-Bold="true"></asp:Label>
      <h1>
         <asp:Label ID="lblTitle" runat="server" Text="CTM New [Transfer Manifest]"></asp:Label>
             </h1>
     <div style="margin-top:15px;" class="botline">
     <table width="100%">
     <tr>
     <td style="width:25%;">CTM Ref&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtCTMRef" runat="server" Width="150px" ReadOnly="true"></asp:TextBox></td>
     <td style="width:20%;">CTM&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:DropDownList ID="ddlCTM" runat="server">
         <asp:ListItem Selected="True">Select</asp:ListItem>
         <asp:ListItem>Inbound</asp:ListItem>
         <asp:ListItem>Outbound</asp:ListItem>
         </asp:DropDownList></td>
     <td style="width:22%;">AWB #&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <asp:TextBox ID="txtAWBPre" runat="server" 
     MaxLength="4" Width="30px"></asp:TextBox>
     <asp:TextBox ID="txtAWBNo" runat="server" Width="70px" MaxLength="10"></asp:TextBox>
         </td>
      <td style="width:20%;">ULD #&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <%--<asp:TextBox ID="txtAWBPre" runat="server" 
             Width="30px"></asp:TextBox>--%>
     <asp:TextBox ID="txtULDNo" runat="server" Width="70px"></asp:TextBox>
         </td>
     <td style="width:10%;">Airport&nbsp;&nbsp;&nbsp;&nbsp; <asp:Label ID="lblAirport" runat="server"></asp:Label></td>
     </tr>
     <tr>
     <td>From Carrier&nbsp;&nbsp; <asp:TextBox ID="txtFromPre" runat="server" Width="50px" MaxLength="4"></asp:TextBox>

     <asp:TextBox ID="txtFromFlight" runat="server" Width="70px" MaxLength="6"></asp:TextBox>
     </td>
     <td>
     Flight Date&nbsp;&nbsp; <asp:TextBox ID="txtFDate" runat="server"  Width="80px" 
             MaxLength="10"></asp:TextBox>
         <asp:CalendarExtender ID="txtFDate_CalendarExtender" runat="server" 
             Enabled="True" TargetControlID="txtFDate" PopupButtonID="imgFromDate" 
             Format="dd/MM/yyyy">
         </asp:CalendarExtender>
                    <asp:ImageButton ID="imgFromDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
     </td>
     <td >
     To Carrier&nbsp;&nbsp;<asp:TextBox ID="txtToPre" runat="server" Width="50px" MaxLength="4"></asp:TextBox>
     <asp:TextBox ID="txtToFlight" runat="server" Width="70px" MaxLength="6"></asp:TextBox>
     </td>
     <td>
     Flight Date&nbsp;&nbsp; <asp:TextBox ID="txtTDate" runat="server"  Width="80px" MaxLength="10"></asp:TextBox>
         <asp:CalendarExtender ID="txtTDate_CalendarExtender" runat="server" 
             Enabled="True" TargetControlID="txtTDate" PopupButtonID="imgToDate" 
             Format="dd/MM/yyyy">
         </asp:CalendarExtender>
                    <asp:ImageButton ID="imgToDate" runat="server" ImageAlign="AbsMiddle" 
                     ImageUrl="~/Images/calendar_2.png" />
     </td>
     <td>
     &nbsp;</td>
     </tr>
     <tr>
     <td colspan="5">
     <asp:Button ID="btnList" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_LIST %>" CssClass="button" 
             onclick="btnList_Click" />
     &nbsp;<asp:Button ID="btnClear" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CLEAR %>" CssClass="button" CausesValidation="false" OnClick="btnClear_Click" />
     </td>
     </tr>
     </table>
     </div>
 
     <%--<div style="margin-top:10px;overflow:auto;width:800px;" >--%>
     <div  class="ltfloat">
     <asp:GridView ID="grdULDDetails" runat="server"
               AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
               CellPadding="2" CellSpacing="3" OnPageIndexChanging="grdULDDetails_PageIndexChanging" AllowPaging="true" PageSize="10">
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
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt Date" 
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
                   
                   
                   
                   
                   
                   
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Location" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="GrdULDLocation" runat="server" AutoPostBack="true" EnableViewState="true" 
                               Text='<%# Eval("Location") %>' Width="80px"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   
                   
               </Columns>
               <HeaderStyle CssClass="titlecolr" />
               <RowStyle HorizontalAlign="Center" />
               <AlternatingRowStyle HorizontalAlign="Center" />
           </asp:GridView>
      </br>
      
     <asp:GridView ID="gvCTM" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="gvCTM_PageIndexChanging" AllowPaging="true" PageSize="10">
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
     
     <asp:TemplateField HeaderText="From Carrier">
     <ItemTemplate>
     <asp:Label ID="lblFrmCarrier" runat="server" Text='<%# Eval("FrmCarrier") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
   
     
     <asp:TemplateField HeaderText="To Carrier">
     <ItemTemplate>
     <asp:Label ID="lblToCarrier" runat="server" Text='<%# Eval("ToCarrier") %>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
       
     <asp:TemplateField HeaderText="Flt Date">
     <ItemTemplate>
     <asp:Label ID="lblFDt" runat="server" Text='<%# Eval("FDt") %>'></asp:Label>
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
     
     <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_SAVE %>" CssClass="button" 
           onclick="btnSave_Click" ValidationGroup="save" />
     &nbsp;
     <asp:Button ID="btnPrint" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_PRINT %>" CssClass="button" 
             onclick="btnPrint_Click" />
       &nbsp;
       <asp:Button ID="btnprintUCR" runat="server" 
                        Text="<%$ Resources:LabelNames, LBL_BTN_PRINTUCR %>" CssClass="button" OnClientClick="popup()" Visible="true"/>&nbsp;
                        <asp:Button ID="btnFSUTFD" runat="server" Text="FSU/TFD" 
             CssClass="button" onclick="btnFSUTFD_Click" />
             &nbsp;
     <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:LabelNames, LBL_BTN_CANCEL %>" CssClass="button" />
     
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
