<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomAgentTab.ascx.cs" Inherits="ProjectSmartCargoManager.CustomAgentTab" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
 <style type="text/css">
        .style1
        {
            width: 30%;
        }
    </style>
   <script src="/js/libs/jquery.validate.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../scripts/excanvas.js"></script>
    <script language="javascript" type="text/javascript" src="../scripts/jquery.min.js"></script>
    <script language="javascript" type="text/javascript" src="../scripts/jquery.jqplot.min.js"></script>
    <script type="text/javascript" src="../scripts/jqplot.barRenderer.min.js"></script>
    <script type="text/javascript" src="../scripts/jqplot.categoryAxisRenderer.min.js"></script>
    <script type="text/javascript" src="../scripts/jqplot.pointLabels.min.js"></script>
    <script type="text/javascript" src="../scripts/plugin/jqplot.json2.min.js"></script>
    <script type="text/javascript" src="../scripts/plugin/jqplot.json2.js"></script>
    <script type="text/javascript" src="../scripts/plugin/jqplot.canvasTextRenderer.min.js"></script>
    <script type="text/javascript" src="../scripts/plugin/jqplot.canvasAxisTickRenderer.min.js"></script>
    <script type="text/javascript" src="../scripts/plugin/jqplot.enhancedLegendRenderer.js"></script>
    <script type="text/javascript" src="../scripts/plugin/jqplot.enhancedLegendRenderer.min.js"></script>
    <script type="text/javascript" src="../scripts/myScript.js"></script>
    <script type="text/javascript" src="../scripts/jqplot.highlighter.js"></script>

    <link rel="stylesheet" type="text/css" href="../style/jquery.jqplot.css" />


<script language="javascript" type="text/javascript">

    function DisplayGraph() {
        var PlotDiv = document.getElementById('chartdiv').style.display = "block";
    }
    function HideGraph() {
        var PlotDiv = document.getElementById('chartdiv').style.display = "none";

    }
    function setSelectedIndex(s, v) {
        for (var i = 0; i < s.options.length; i++) {
            if (s.options[i].value == v) {
                s.options[i].selected = true;
                return;
            }
        }
    }
    function GeneratePlot(JS) {
        try {

            //succes - data loaded, now use plot:
            var JSONString = JSON.parse(JS);
            var plotarea = $("#chartdiv");
            var dataBar = JSONString[0].FlightNo;
            var dataLine = JSONString[0].FlightNo;
            var line1 = [];
            var line2 = [];
            var line3 = [];
            if (JSONString.length > 0) {
                DisplayGraph();
            }

            for (i = 0; i < JSONString.length; i++) {
                line1[i] = JSONString[i].FlightNo;
                line2[i] = JSONString[i].Wt;

            }
            var MaxTonnage;
            var slots = line2;
            var slots_max = slots[0];
            for (var s = 0; s < slots.length; s++) //finding maximum value of slots array
            {
                if (slots[s] > slots_max) {
                    slots_max = slots[s];
                }
            }

            var plot2 = $.jqplot('chartdiv', [line2], {
                // The "seriesDefaults" option is an options object that will
                // be applied to all series in the chart.
                //width: 1000,
                title: '',
                //            axes: { yaxis: { min: 0} },
                seriesDefaults: {
                    renderer: $.jqplot.BarRenderer,
                    rendererOptions: { barWidth: 25 },
                    //color: '#5FAB78',
                    color: '#f7811f',
                    pointLabels: { show: true }
                },

                axes: {
                    // Use a category axis on the x axis and use our custom ticks.
                    xaxis: {
                        renderer: $.jqplot.CategoryAxisRenderer,
                        ticks: line1
                    },
                    // Pad the y axis just a little so bars can get close to, but
                    // not touch, the grid boundaries.  1.2 is the default padding.
                    yaxis: {
                        pad: 1.05,
                        min: 0,
                        //max: parseInt(RetMaxVal(Tonnage10)),
                        max: slots_max + 20,
                        //max: parseInt(Tonnage10),
                        tickOptions: { formatString: '%d' }
                    }
                },
                series: [
            { label: 'X:Flights<br />Y:Tonnage(Kg)' }
                ],
                legend: {
                    show: true,
                    placement: 'insideGrid',
                    showSwatches: false,
                    showLabels: true
                },
                highlighter: {
                    show: false,
                    sizeAdjust: 3,
                    tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                        return line2[pointIndex];

                    }

                }

            });
            $('#chartdiv').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
                var FightNo = document.getElementById("<%=hdFlightNo.ClientID%>");
                FightNo.value = line1[pointIndex];
                document.getElementById("<%=TabContainer1.ClientID%>").control.set_activeTabIndex(1);
//                var btnPopulate = document.getElementById("<%=btnFlightList.ClientID%>");
//                btnPopulate.click();

            }
        );
        }
        catch (Err) {
            HideGraph();
        }
    }
    

</script> 
    

      
    <asp:Panel ID="PanelGraph" runat="server">
      <table width="100%">
      <tr>
      <td>
      <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
     <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Font-Size="Medium" 
            Width="650px" Height="300px" AutoPostBack="true" OnActiveTabChanged="OnActiveTab_Changed">
            <%--OnActiveTabChanged="GetData_Click"--%><%--OnClientActiveTabChanged="CallPopulateClick();"--%>
            
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Flights" Width="620px" 
                Height="27px"><HeaderTemplate>
              Flight Tonnage
</HeaderTemplate>
                
                <ContentTemplate>
                
      <asp:Panel 
          ID="pnlFlightDashboard" runat="server" Width="100%" ScrollBars="Auto">
    
      <table width = "100%">
    <tr align = "center">
    <td align = "center">
    <div id="chartdiv" style="width:1000px; display:none;">
    </div>
    </td>
    
    </tr>
    
    </table>
    
      </asp:Panel>
     
      
</ContentTemplate>
</asp:TabPanel>


           <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Agents" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    AWB Details
</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel2" runat="server" Width="100%">
    
      <table width = "100%">
    <tr>
    <td>
  Flight#
    <asp:DropDownList ID="ddlFlightNo" runat="server">
    <asp:ListItem>ALL</asp:ListItem></asp:DropDownList>
    <asp:Button ID="btnFlightList" Text="List" runat="server" class = "button" OnClick="GetDataPerFlight_Click"  />
    </td>
    
    </tr>
    </table>
    
    <div style="width:600px; height:250px; overflow:auto;">
    <asp:GridView ID="grdAWBDetailsFlightWise" runat="server" AutoGenerateColumns="false">
    <Columns>
                <asp:TemplateField HeaderText="AWB #">
                <ItemTemplate>
                <asp:HyperLink ID="hlnkAWBNumber" runat="server" Text='<%#Eval("AWBNumber") %>' NavigateUrl='<%# "~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + Eval("AWBNumber")%>'></asp:HyperLink>
                </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Status">
                <ItemTemplate>
                <asp:HyperLink ID="hlnkTracking" runat="server" Text='<%#Eval("ConfirmationStatus") %>' NavigateUrl='<%#Eval("TrackingLink")%>'></asp:HyperLink>
                </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="AgentCode" HeaderText="AgentCode" Visible="false">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="BookedPcs" HeaderText="BookedPcs">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="GrossWt" HeaderText="BookedWt">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="OriginCode" HeaderText="Origin">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="DestinationCode" HeaderText="Destination">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FltNumber" HeaderText="FlightNo">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="FltDate" HeaderText="FlightDate">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="AcceptedPcs" HeaderText="AcceptedPcs" Visible="false">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="AcceptedWt" HeaderText="AcceptedWt" Visible="false">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="BookingStatus" HeaderText="Booking Status" Visible="false">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="ConfirmationStatus" HeaderText="Confirmation Status" Visible="false">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                
                                  
    </Columns>
     <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
   
    </asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
           
           <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Import" Width="620px"
                Height="27px">
                <HeaderTemplate>
                   Invoice Details
</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="PNImport" runat="server" Width="100%">
    
      <div style="width:600px; height:250px; overflow:auto;">
    <asp:GridView ID="grdInvoiceListing" runat="server" AutoGenerateColumns="false">
    <Columns>
                <asp:TemplateField HeaderText="Invoice #">
                <ItemTemplate>
                <asp:HyperLink ID="hlnkInvoiceNo" runat="server" Text='<%#Eval("InvoiceNumber") %>' NavigateUrl='<%# Eval("InvoiceLink")%>'></asp:HyperLink>
                </ItemTemplate>
                </asp:TemplateField>
                 <asp:BoundField DataField="InvoiceDt" HeaderText="Invoice Date">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="InvoiceAmount" HeaderText="InvoiceAmount">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="PendingAmount" HeaderText="PendingAmount">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                <%--<asp:BoundField DataField="AgentCode" HeaderText="AgentCode">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>--%>
                 <asp:BoundField DataField="Pieces" HeaderText="Pieces">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="GrossWt" HeaderText="GrossWt">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="DiscountAmt" HeaderText="DiscountAmt">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="CommissionAmt" HeaderText="CommissionAmt">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  
             <%--    <asp:BoundField DataField="ChargableWt" HeaderText="ChargableWt">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="FreightIATA" HeaderText="FreightIATA">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FreightMKT" HeaderText="FreightMKT">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="Surcharge" HeaderText="Surcharge">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="OCDueCar" HeaderText="OCDueCar">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="OCDueAgent" HeaderText="OCDueAgent">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="Total" HeaderText="Total">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="Discount" HeaderText="Discount">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  
                  <asp:BoundField DataField="TAD" HeaderText="TAD">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="Commission" HeaderText="Commission">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                
                   <asp:BoundField DataField="RevisedTotal" HeaderText="RevisedTotal">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="TDS" HeaderText="TDS">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                 <asp:BoundField DataField="TDSAmt" HeaderText="TDSAmt">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                    <asp:BoundField DataField="TDSOnCommission" HeaderText="TDSOnCommission">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="TDSOnCommAmt" HeaderText="TDSOnCommAmt">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="ServiceTax" HeaderText="ServiceTax">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="FinalAmt" HeaderText="FinalAmt">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                
                 
                  <asp:BoundField DataField="CollectedAmount" HeaderText="CollectedAmount">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>--%>
                
    </Columns>
     <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
            
           <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Export" Width="620px" ScrollBars="Auto"
                Height="27px">
                <HeaderTemplate>
                    Deals
                </HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel3" runat="server" Width="100%">
    <div style="width:600px; height:250px; overflow:auto;">
    <asp:GridView ID="grdDeals" runat="server" AutoGenerateColumns="false">
    <Columns>
                <asp:TemplateField HeaderText="DealID">
                <ItemTemplate>
                <asp:Label ID="lblDealID" runat="server" Text='<%#Eval("DealId") %>' ToolTip='<%#Eval("DealId") %>'  Width="150px"></asp:Label>
                <%--<asp:HyperLink ID="hlnkDealID" runat="server" Text='<%#Eval("DealId") %>' ToolTip='<%#Eval("DealId") %>' NavigateUrl='<%# "~/DCMApplyDeals.aspx?DealID=" + Eval("DealId")%>' Width="150px"></asp:HyperLink>--%>
                </ItemTemplate>
                </asp:TemplateField>
              <%--  <asp:BoundField DataField="AgentCode" HeaderText="AgentCode">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FromDate" HeaderText="ApplicableFrom">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="ToDate" HeaderText="ApplicableTo">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>--%>
                 <asp:BoundField DataField="Status" HeaderText="Status">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                 <asp:BoundField DataField="DealType" HeaderText="DealType">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
               <%--    <asp:BoundField DataField="KickBackAmount" HeaderText="KickBackAmount">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FlatAmount" HeaderText="FlatAmount">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="IsLocal" HeaderText="IsLocal">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                 <asp:TemplateField HeaderText="Parameters">
                <ItemTemplate>
                <asp:Label ID="lblDealParams" Width="150px" runat="server" Text='<%#Eval("DealParams").ToString().PadRight(10).Substring(0,10)+"..." %>' ToolTip='<%#Eval("DealParams") %>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>--%>
                 
                  <asp:TemplateField HeaderText="Slabs">
                <ItemTemplate>
                <asp:Label ID="lblDealSlabs" Width="250px"  runat="server" Text='<%#Eval("DealSlabs").ToString().PadRight(25).Substring(0,25)+"..." %>' ToolTip='<%#Eval("DealSlabs") %>'></asp:Label>
                </ItemTemplate>
                </asp:TemplateField>
                   <asp:BoundField DataField="ApplicableTonnage" HeaderText="ApplicableTonnage">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
    </Columns>
     <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
             
           <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Export" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    Quotes
</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel1" runat="server" Width="100%">
     <div style="width:600px; height:250px; overflow:auto;">
      <asp:GridView ID="grdAgentQuotes" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True">
           <Columns>
               <%-- <asp:BoundField DataField="AgentCode" HeaderText="AgentCode">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:TemplateField HeaderText="AgentName">
                 <ItemTemplate>
                 <asp:Label ID="lblAgentName" Width="150px" runat="server" Text='<%# Eval("AgentName").ToString().PadRight(15).Substring(0,15)+"..." %>' ToolTip='<%# Eval("AgentName") %>'></asp:Label>
                 </ItemTemplate>
                 </asp:TemplateField>--%>
                  <%--<asp:TemplateField HeaderText="QuoteID">
                <ItemTemplate>
                <asp:HyperLink ID="hlnkQuoteID" runat="server" Text='<%#Eval("SrNo") %>' ToolTip='<%#Eval("SrNo") %>' NavigateUrl='<%# "~/frmAgentQuoteListNew.aspx?QuoteID=" + Eval("SrNo")%>' Width="150px"></asp:HyperLink>
                </ItemTemplate>
                </asp:TemplateField>--%>
                   <asp:BoundField DataField="Origin" HeaderText="Origin">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="Dest" HeaderText="Destination">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="FlightNo" HeaderText="FlightNo">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FlightDt" HeaderText="FlightDate">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="CommodityCode" HeaderText="CommodityCode">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="FreightWeight" HeaderText="Weight">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FreightRate" HeaderText="Rate/Kg">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  <%-- <asp:BoundField DataField="StationCode" HeaderText="Station">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>--%>
             
    </Columns>
            
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
           
           <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="Export" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    Capacity
</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel4" runat="server" Width="100%">
     <div style="width:600px; height:250px; overflow:auto;">
      <asp:GridView ID="grdAgentCapacity" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3">
           <Columns>
           
              <%--  <asp:BoundField DataField="AgentCode" HeaderText="AgentCode">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:TemplateField HeaderText="AgentName">
                 <ItemTemplate>
                 <asp:Label ID="lblAgentName" Width="150px" runat="server" Text='<%# Eval("AgentName").ToString().PadRight(15).Substring(0,15)+"..." %>' ToolTip='<%# Eval("AgentName") %>'></asp:Label>
                 </ItemTemplate>
                 </asp:TemplateField>--%>
                  <%--<asp:TemplateField HeaderText="CapacityID">
                <ItemTemplate>
                <asp:HyperLink ID="hlnkCapacityID" runat="server" Text='<%#Eval("SrNo") %>' ToolTip='<%#Eval("SrNo") %>' NavigateUrl='<%# "~/frmAgentCapacityList.aspx?CapacityID=" + Eval("SrNo")%>' Width="150px"></asp:HyperLink>
                </ItemTemplate>
                </asp:TemplateField>--%>
                   <asp:BoundField DataField="Origin" HeaderText="Origin">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="Dest" HeaderText="Destination">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="FlightNo" HeaderText="FlightNo">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FromDt" HeaderText="ValidFrom">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                  <asp:BoundField DataField="ToDt" HeaderText="ValidTo">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <asp:BoundField DataField="ComodityCode" HeaderText="CommodityCode">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                   <asp:BoundField DataField="FreightWeight" HeaderText="Weight">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>
                 <asp:BoundField DataField="FreightRate" HeaderText="Rate/Kg">
                 <ItemStyle Width="100px" />
                  </asp:BoundField>
                   <%--<asp:BoundField DataField="StationCode" HeaderText="Station">
                 <ItemStyle Width="100px" />
                 </asp:BoundField>--%>
             
    </Columns>
            
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>  
           
           <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="Export" Width="620px"
                Height="27px">
                <HeaderTemplate>
                     Claims </HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel5" runat="server" Width="100%">
     <div style="width:600px; height:250px; overflow:auto;">
      <asp:GridView ID="grdClaims" runat="server" ShowFooter="false" Width="100%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3">
           <Columns>
                
<asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWBNo">
<ItemTemplate>
<asp:HyperLink ID="hlnkAWBNumber" runat="server" Text='<%#Eval("AWBNo") %>' NavigateUrl='<%# "~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + Eval("AWBNo")%>'></asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>

<%--<asp:TemplateField HeaderText="AgentCode">
<ItemTemplate>
<asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>--%>

<asp:TemplateField HeaderText="Name of Applicant">
<ItemTemplate>
<asp:Label ID="lblFullName" runat="server" Text='<%# Eval("FullName").ToString().PadRight(20).Substring(0,20)+"..." %>' ToolTip='<%#Eval("FullName") %>' Width="150px"></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Claim Type">
<ItemTemplate>
<asp:Label ID="lblClaimType" runat="server" Text='<%# Eval("ClaimType") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Claim Station">
<ItemTemplate>
<asp:Label ID="lblClaimStation" runat="server" Text='<%# Eval("ClaimStation") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Claim Date">
<ItemTemplate>
<asp:Label ID="lblClaimDate" runat="server" Text='<%# Eval("ClaimDt") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Claim Status">
<ItemTemplate>
<%--<asp:Label ID="lblClaimStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
--%>
<asp:HyperLink ID="hlnkClaimStatus" runat="server" Text='<%#Eval("Status") %>' NavigateUrl='<%# "~/frmClaimApplication.aspx?View="+Eval("ClaimID")%>'></asp:HyperLink>

</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Claim Amount">
<ItemTemplate>
<asp:Label ID="lblClaimAmt" runat="server" Text='<%# Eval("ClaimAmt") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField HeaderText="Nature Of Goods">
<ItemTemplate>
<asp:Label ID="Commodity" runat="server" Text='<%# Eval("Commodity") %>'></asp:Label>
</ItemTemplate>
</asp:TemplateField>
<%--<asp:TemplateField>
<ItemTemplate>
<asp:HyperLink ID="hlnkEdit" runat="server" Text="Edit" NavigateUrl='<%# "~/frmClaimApplication.aspx?Edit=" + Eval("ClaimID")%>'></asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>

<asp:TemplateField>
<ItemTemplate>
<asp:HyperLink ID="hlnkView" runat="server" Text="View" NavigateUrl='<%# "~/frmClaimApplication.aspx?View="+Eval("ClaimID")%>'></asp:HyperLink>
</ItemTemplate>
</asp:TemplateField>
--%>

</Columns>
            
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>   
                
      </asp:TabContainer>
      </td>
      </tr>
      <tr>
      <td>
      <br />
      <table style="width:620px">
    <tr align = "center">
    <td align = "right" >
        Location :
        </td>
    <td align = "left">
        <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="True">
        </asp:DropDownList>
    </td>
    <td align = "right">
        From Date :
        </td>
    <td align = "left">
        <asp:TextBox ID="txtFrmDate" runat="server" Width="100px"></asp:TextBox>
         <asp:CalendarExtender ID="CalendarExtender3" runat="server" 
          Enabled="True" TargetControlID="txtFrmDate" Format="dd/MM/yyyy" PopupButtonID="ImageButton1" >
         </asp:CalendarExtender>
          <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td align = "right">
        To Date :
        </td>
    <td align = "left">
        <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender4" runat="server" 
          Enabled="True" TargetControlID="txtToDate" Format="dd/MM/yyyy" PopupButtonID="imgFltToDt" >
        </asp:CalendarExtender>
        <asp:ImageButton ID="imgFltToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
    </td>
    <td>
        <asp:Button ID="GetData" runat="server" Text="Populate Data" class = "button" OnClick="GetData_Click" 
            />
            <asp:HiddenField ID="hdFlightNo" runat="server" />
    </td>
    </tr>
    </table>
      </td>
      </tr>
      </table>
    </asp:Panel>
  