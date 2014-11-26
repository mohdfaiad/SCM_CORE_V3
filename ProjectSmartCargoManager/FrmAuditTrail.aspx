<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAuditTrail.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FrmAuditTrail" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    
 </asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <script language="javascript" type="text/javascript">
    function printPartOfPage(elementId) {
        //            var printContent = document.getElementById(elementId);
        //            var windowUrl = 'about:blank';
        //            var uniqueName = new Date();
        //            var windowName = 'Print' + uniqueName.getTime();
        //            var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

        //            printWindow.document.write(printContent.innerHTML);
        //            printWindow.document.close();
        //            printWindow.focus();
        //            printWindow.print();
        //            printWindow.close();
        this.print();
    }
        
        
        
    </script>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
     </script>
    
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
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .black_overlaymsg
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: White;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_contentmsg
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 5%;
            height: 5%;
            padding: 16px;
            background-color: Transparent;
            z-index: 1002;
        }
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    
  <div id="contentarea">
  
    <h1>       
        Audit Trail
    </h1> 
        
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
       <div class="botline"> 
       <table width="40%">
   <tr>
    <td>
        AWBNumber
    </td>
    <td>
     <asp:TextBox ID="txtprefix" runat="server" Width="29px" MaxLength="4" ></asp:TextBox>-
     
     
     <asp:TextBox ID="txtAWBNumber" runat="server" Width="100px" MaxLength="11"></asp:TextBox>
       
     </td>
   
    <td>
        <asp:Button ID="btnlist" runat="server" CssClass="button" Text="List" 
            onclick="btnList_Click"/>
        <asp:Button ID="btnclear" runat="server" CssClass="button" Text="Clear" 
            onclick="btnclear_Click"/>
    </td>
   </tr>
  </table>
       </div>
    <h2>AWB Summary Details </h2>
     
      <div ID="divPrint" class="ltfloat" >  
          <asp:GridView ID="GrdAWBSummary" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="AWBSummaryDetails" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
      <%----------------------------------------------------------------------%>
    
      <h2>AWB Operation Details </h2>
      <div ID="div1" class="ltfloat">
          <asp:GridView ID="GrdRouteDetails" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="AWBRouteDetails" Width="99%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
    
      <h2>AWB Rate Details </h2>
      <div ID="div2" class="ltfloat">
          <asp:GridView ID="GrdRateMaster" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="AWBRate Details" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
     
      <h2>ULD Build Details </h2>
      <div ID="div8" class="ltfloat">
          <asp:GridView ID="GrdULDAWB" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="Operation AWB Details" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
     
      <h2>Manifest Details </h2>
      <div ID="div3" class="ltfloat">
          <asp:GridView ID="GrdManifestSummary" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="ManifeestDetails" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
     
      <div style="display:none">
       <h2>Manifest Summary Details </h2>
          <table width="50%">
              <tr>
                  <td>
                      Flight Number:
                  </td>
                  <td>
                      &nbsp;<asp:TextBox ID="txtFlightNumber" runat="server" 
                          ToolTip="Please Enter FlightNumber With Prefix" Width="80px"></asp:TextBox>
                      <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender5" runat="server" 
                          TargetControlID="txtFlightNumber" WatermarkText="FlightNumber">
                      </asp:TextBoxWatermarkExtender>
                  </td>
                  <td>
                      Flight Date:
                  </td>
                  <td>
                      <asp:TextBox ID="txtFlightDate" runat="server" Width="100px"></asp:TextBox>
                      <asp:TextBoxWatermarkExtender ID="txtFlightDate_TextBoxWatermarkExtender" 
                          runat="server" TargetControlID="txtFlightDate" 
                          WatermarkText="Select Flight Date">
                      </asp:TextBoxWatermarkExtender>
                      <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
                          Enabled="True" Format="dd-MM-yyyy" TargetControlID="txtFlightDate">
                      </asp:CalendarExtender>
                  </td>
                  <td>
                      <asp:Button ID="BtnListManifest" runat="server" CssClass="button" 
                          onclick="BtnListManifest_Click" Text="List" />
                  </td>
              </tr>
          </table>
      </div>
      <div ID="div7" class="ltfloat" style="display:none">
          <asp:GridView ID="GrdManifstSummaryDetails" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="ManifeestSummary" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
      <%--------------------------------------------------------------------------------------------%>
      
      <h2>Arrival Details </h2>
      <div ID="div4" class="ltfloat">
          <asp:GridView ID="GrdArrival" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              HorizontalAlign="Center" ToolTip="ArrivalDetails" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle BorderStyle="Solid" CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
              <AlternatingRowStyle CssClass="trcolor" />
          </asp:GridView>
      </div>
      
      <h2>ULD Break Details </h2>
      <div ID="div9" class="ltfloat">
          <asp:GridView ID="GrdBreakULD" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="Operation AWB Details" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
      
      <h2>Delivery Details </h2>
      <div ID="div5" class="ltfloat">
          <asp:GridView ID="GrdDelivery" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="Delivery Details" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
      </div>
      
      <h2>Billing Details </h2>
      <div ID="div6" class="ltfloat">
          <asp:GridView ID="GrdBillingDetails" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="Billing Details" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
          <asp:Button ID="BtnPrint" runat="server" CssClass="button" 
              onclick="BtnPrint_Click" OnClientClick="JavaScript: printPartOfPage('div');" 
              Text="Print" />
          <div ID="div" runat="server" clientidmode="Static">
          </div>
          <br />
      </div>
      <div ID="msglight" class="white_contentmsg">
          <table>
              <tr>
                  <td align="center" width="5%">
                      <br />
                      <img src="Images/loading.gif" />
                      <br />
                      <asp:Label ID="msgshow" runat="server"></asp:Label>
                  </td>
              </tr>
          </table>
      </div>
      <div ID="msgfade" class="black_overlaymsg">
      </div>
    
       </div>   
    </ContentTemplate>
    </asp:UpdatePanel>


  </asp:Content> 
