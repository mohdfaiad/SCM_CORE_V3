<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomManagementTab.ascx.cs" Inherits="ProjectSmartCargoManager.CustomControls.CustomManagementTab" %>

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
    <script type="text/javascript">

        // Top Ten Flights Start

        var plot2;
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

        function ResizeDiv(json) {
            try {
                var targetDiv = document.getElementById('chartdiv');
                if (json.length > 0) {
                    if (json.length > 10) {
                        for (i = 10; i < json.length; i + 10) {
                            targetDiv.style.width = "1000px";

                        }
                    }
                }
            }
            catch (Error)
        { alert("Error"); }
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
                plot2 = $.jqplot('chartdiv', [line2], {
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

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv').bind('resize', function(event, ui) {

                    plot2.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph();
            }
        }

        //END



        //Top Ten Agents START

        var plot4;
        function DisplayGraph2() {
            var PlotDiv = document.getElementById('chartdiv2').style.display = "block";
        }
        function HideGraph2() {
            var PlotDiv = document.getElementById('chartdiv2').style.display = "none";

        }


        // Top Ten Agents
        function GeneratePlotAgent(JS) {
            try {

                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv2");
                var dataBar = JSONString[0].AgentCode;
                var dataLine = JSONString[0].AgentCode;
                var line1 = [];
                var line2 = [];
                var line3 = [];
                if (JSONString.length > 0) {
                    DisplayGraph2();
                }

                for (i = 0; i < JSONString.length; i++) {
                    line1[i] = JSONString[i].AgentCode;
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
                plot4 = $.jqplot('chartdiv2', [line2], {
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
            { label: 'X:Agents<br />Y:Tonnage(Kg)' }
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
                $('#chartdiv2').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
                var AgentCode = document.getElementById("<%=hdAgent.ClientID%>");
                AgentCode.value = line1[pointIndex];
                document.getElementById("<%=TabContainer1.ClientID%>").control.set_activeTabIndex(3);

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv2').bind('resize', function(event, ui) {

                    plot2.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph2();
            }
        }

        //END

        //Top Ten Booked Flights START

        var plotBooked;
        function DisplayGraph4() {
            var PlotDiv = document.getElementById('chartdiv4').style.display = "block";
        }
        function HideGraph4() {
            var PlotDiv = document.getElementById('chartdiv4').style.display = "none";

        }


        // Top Ten Booked Flights
        function GeneratePlotBooked(JS) {
            try {
                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv4");
                var dataBar = JSONString[0].FlightNo;
                var dataLine = JSONString[0].FlightNo;
                var line1 = [];
                var line2 = [];
                var line3 = [];
                if (JSONString.length > 0) {
                    DisplayGraph4();
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
                plotBooked = $.jqplot('chartdiv4', [line2], {
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
            { label: 'X:FlightNo<br />Y:Tonnage(Kg)' }
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
                $('#chartdiv4').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
//                var FightNo = document.getElementById("<%=hdFlightNo.ClientID%>");
//                FightNo.value = line1[pointIndex];
//                document.getElementById("<%=TabContainer1.ClientID%>").control.set_activeTabIndex(1);

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv4').bind('resize', function(event, ui) {

                    plotBooked.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph4();
            }
        }

        //END

        //Top Ten Locations Inbound & Outbound START

        var plotLocation;
        function DisplayGraph8() {
            var PlotDiv = document.getElementById('chartdiv8').style.display = "block";
        }
        function HideGraph8() {
            var PlotDiv = document.getElementById('chartdiv8').style.display = "none";

        }


        // Top Ten Locations
        function GeneratePlotLocation(JS) {
            try {
                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv8");
                var dataBar = JSONString[0].Origin;
                var dataLine = JSONString[0].Origin;
                var line1 = [];
                var line2 = [];
                var line3 = [];
                if (JSONString.length > 0) {
                    DisplayGraph8();
                }

                for (i = 0; i < JSONString.length; i++) {
                    line1[i] = JSONString[i].Origin;
                    line2[i] = JSONString[i].ExportWt;
                    line3[i] = JSONString[i].ImportWt;

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

                plotLocation = $.jqplot('chartdiv8', [line2, line3], {
                    stackSeries: true,
                    // The "seriesDefaults" option is an options object that will
                    // be applied to all series in the chart.
                    //width: 1000,
                    title: '',
                    //            axes: { yaxis: { min: 0} },
                    seriesDefaults: {
                        renderer: $.jqplot.BarRenderer,
                        rendererOptions: {
                            barWidth: 25,
                        },
                        pointLabels: { show: true }
                    },

                    axes: {
                        xaxis: {
                            renderer: $.jqplot.CategoryAxisRenderer,
                            ticks: line1
                        },
                        yaxis: {
                            // Don't pad out the bottom of the data range.  By default,
                            // axes scaled as if data extended 10% above and below the
                            // actual range to prevent data points right on grid boundaries.
                            // Don't want to do that here.
                            padMin: 0
                        }
                    },
                    series: [
            { label: 'Export Wt' },
            { label: 'Import Wt' }
        ],
                    legend: {
                        show: true,
                        location: 'e',
                        placement: 'inside'
                    },
                    highlighter: {
                        show: false,
                        sizeAdjust: 3,
                        tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                            return line2[pointIndex];

                        }

                    }

                });
                $('#chartdiv8').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
                var Station = document.getElementById("<%=hdStation.ClientID%>");
                Station.value = line1[pointIndex];
                //document.getElementById("<%=TabContainer1.ClientID%>").control.set_activeTabIndex(5);

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv8').bind('resize', function(event, ui) {

                    plotLocation.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph8();
            }
        }

        //END
    


    



    
    
    
    
    


    
    
    
    


    </script>
    
    <script language="javascript" type="text/javascript">

        function getid(e, val) {
            var ctrlname;
            if (val == "m") {
                ctrlname = e.id.replace('btnManage', 'lblAWBno');
            } else {
                ctrlname = e.id.replace('btnConfirm', 'lblAWBno');

            }

            var str = e.id;
            var n = str.indexOf("GridView2");
            var m = str.length;
            var o = str.substr(n, m);

            var valExpectedAWB = document.getElementById(ctrlname).innerHTML;
            str = str.replace(o, "lblFlightNo");
            var fltno = document.getElementById(str).innerHTML;
            str = str.replace("lblFlightNo", "lblFlightDate");
            var fltDate = document.getElementById(str).innerHTML;

            if (valExpectedAWB.trim() == "") {
                alert("No AWB Available");
                return false;
            }
            else {
                document.getElementById("<%=hdn.ClientID %>").value = valExpectedAWB;
                document.getElementById("<%=hdnFliNo.ClientID %>").value = fltno;
                document.getElementById("<%=hdnFliDt.ClientID %>").value = fltDate;
                return true;
            }
        }

        function cllsa() {
            document.getElementById("<%=GetData.ClientID %>").click();
        }

        function expandcollapse(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "block";
                if (row == 'alt') {
                    img.src = "minus.gif";
                }
                else {
                    img.src = "minus.gif";
                }
                img.alt = "Close to view other Customers";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "plus.gif";
                }
                else {
                    img.src = "plus.gif";
                }
                img.alt = "Expand to show Orders";
            }
        }


        function expandcollapseforgreen(obj, row) {
            var div = document.getElementById(obj);
            var img = document.getElementById('img' + obj);

            if (div.style.display == "none") {
                div.style.display = "block";
                if (row == 'alt') {
                    img.src = "minus.gif";
                }
                else {
                    img.src = "minus.gif";
                }
                img.alt = "Close to view other Customers";
            }
            else {
                div.style.display = "none";
                if (row == 'alt') {
                    img.src = "plusgreen.gif";
                }
                else {
                    img.src = "plusgreen.gif";
                }
                img.alt = "Expand to show Orders";
            }
        }

        $(document).ready(function() {
            $('#<%= GridView1.ClientID %>').find('tr').click(function callg() {
                if ($(this).hasClass('RowStyle')) {
                    $('#<%= GridView1.ClientID %> tr').removeClass('highlight');
                    $(this).removeClass('RowStyle');
                    $(this).addClass('highlight');
                }
            }
        );
        }
    );
    
</script>

    <link rel="stylesheet" type="text/css" href="../style/jquery.jqplot.css" />
    


 
    

      
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
              Top Flights
</HeaderTemplate>
                
                <ContentTemplate>
                
      <asp:Panel 
          ID="pnlFlightDashboard" runat="server" Width="100%" ScrollBars="Auto">
    
      <table width = "100%">
    <tr align = "center">
    <td align = "center">
    <div id="resizable1">
    <div id="chartdiv" style=" display:none; ">
    </div>
    </div>
    </td>
    
    </tr>
    
    </table>
    
      </asp:Panel>
     
      
</ContentTemplate>
</asp:TabPanel>

<asp:TabPanel ID="subTabPanel1" runat="server" HeaderText="Agents" Width="620px" Height="27px">
                        <HeaderTemplate>
                            &nbsp; Revenue &nbsp;
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Flight#
                                            <asp:DropDownList ID="ddlFlightNo" runat="server">
                                            </asp:DropDownList>
                                            <asp:HiddenField ID="hdn" runat="server" />
                                            <asp:HiddenField ID="hdnFliNo" runat="server" />
                                            <asp:HiddenField ID="hdnFliDt" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                                <asp:Panel ID="pnlRevenueDetails" runat="server" Visible="false">
                                <div style="margin-top:0px; height:250px; overflow:auto" id="dispgrid" runat="server">
                                    <div class="divback">
                                        <table width="100%" cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td>
                                                <asp:Label ID="label1" runat="server" Text="Origin: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblOrigin" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label2" runat="server" Text="Dest: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblDestination" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label3" runat="server" Text="Capacity: "></asp:Label>
                                               </td><td>
                                                <asp:Label ID="lblCapacity" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label4" runat="server" Text="Revenue: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblRevenue" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="label5" runat="server" Text="Flt Date: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblFltDate" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label7" runat="server" Text="Flight #: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblFltNum" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label9" runat="server" Text="Confirmed: "></asp:Label>
                                               </td><td>
                                                <asp:Label ID="lblConfirmed" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label11" runat="server" Text="Cost: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblCost" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="label6" runat="server" Text="Dept Time: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblDeptTime" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label10" runat="server" Text="Arr Time: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblArrivalTime" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label13" runat="server" Text="Queued: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblQueued" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label15" runat="server" Text="Profit: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblProfitability" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="label8" runat="server" Text="Flt Status: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblFltStatus" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="label14" runat="server" Text="Blocked: "></asp:Label>
                                                </td><td>
                                                <asp:Label ID="lblBlocked" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                
                                            </td>
                                            <td>
                                            </td>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            <td>
                                                <asp:Label ID="label17" runat="server" Text="Available: "></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAvailable" runat="server" Font-Bold="true" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                    </div>
                                    <table>
                                        <tr>
                                        
                                            <td  style="border-bottom:0px;">
                                                <div id="div<%# Eval("Origin") +","+ Eval("Destination") +","+ Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" 
                                                style="display:block;position:relative; OVERFLOW: auto;" >
                                                <strong>Summary</strong>
                                                <asp:Table ID="tblLYP" runat="server" BorderWidth="1px" Width="97%"  GridLines="Both" CssClass="tbcen">
                                                <asp:TableRow  CssClass="titlecolr">
                                                 <asp:TableCell ColumnSpan="8" Width="50%" ><strong>Cargo History Management</strong></asp:TableCell>
                                                 <asp:TableCell ColumnSpan="4" Width="50%" ><strong>PAX History Management</strong></asp:TableCell>
                                                 </asp:TableRow>
                                                 <asp:TableRow>
                                                  <asp:TableCell ColumnSpan="2" Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff">Today</asp:TableCell>
                                                 <asp:TableCell ColumnSpan="2" Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff"> DOW (1 Yr)</asp:TableCell>
                                                 <asp:TableCell ColumnSpan="2" Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff">30 Day Avg</asp:TableCell>
                                                 <asp:TableCell ColumnSpan="2" Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff">1 Year Avg</asp:TableCell>
                                                 <asp:TableCell Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff">Today</asp:TableCell>
                                                 <asp:TableCell Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff"> DOW (1 Yr)</asp:TableCell>
                                                 <asp:TableCell Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff">30 Day Avg</asp:TableCell>
                                                 <asp:TableCell Font-Bold="true"  BackColor="#656667" ForeColor="#ffffff">1 Year Avg</asp:TableCell> 
                                                 </asp:TableRow>
                                                 <asp:TableRow>
                                                <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px; ">Load</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">Yield</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">Load</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">Yield</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">Load</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">Yield</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">Load</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">Yield</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">PAX/Load</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">PAX/Load</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">PAX/Load</span></asp:TableCell>
                                                 <asp:TableCell BackColor="#656667" ForeColor="#ffffff"><span style=" font-weight:bold; font-size:10px">PAX/Load</span></asp:TableCell>
                                                 </asp:TableRow>
                                                 <asp:TableRow>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl1dL" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl1dYa" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl52dL" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl52dY" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl30dL" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl30dY" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl365dL" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl365dY" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl1dP" runat="server"></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl52dP" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl30dP" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 <asp:TableCell>
                                                 <asp:Label ID="lbl365dP" runat="server" ></asp:Label>
                                                 </asp:TableCell>
                                                 </asp:TableRow>
                               
                                                </asp:Table> 
                                                </div>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td colspan="13" style="border-top:0px;">
                                                <div id="div<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" style="display:block;position:relative; OVERFLOW:auto;" >
                                                    <strong> AWB Details</strong><br />
                                                    <asp:GridView ID="GridView2" AllowPaging="false" AllowSorting="false"
                                                     Width="100%" Font-Size="Small"
                                                        AutoGenerateColumns="false" runat="server"  ShowFooter="false"
                                                        HeaderStyle-CssClass="titlecolr" CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" 
                                                        PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
                                                        <RowStyle  HorizontalAlign="Left" />
                                                        <HeaderStyle />
                                                        <FooterStyle />
                                                        <Columns>
                                                            <asp:TemplateField HeaderText="AWB#">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAWBno" Text='<%# Eval("AWBNo") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Origin">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Dest.">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDest" Text='<%# Eval("Destination") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Agent">
                                                                <ItemTemplate>  
                                                                    <asp:Label ID="lblAgent" Text='<%# Eval("Agent") %>' runat="server">
                                                                    </asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Billing">
                                                                <ItemTemplate>  
                                                                    <asp:HyperLink ID="lblBilling" Text='<%# Eval("BillingStatus") %>' runat="server"
                                                                    NavigateUrl='<%# "~/BillingInvoiceMatching.aspx?AWBNumber=" + Eval("AWBNo")%>' >
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Invoice">
                                                                <ItemTemplate>  
                                                                    <asp:HyperLink ID="lblInvoice" Text='<%# Eval("InvoiceNo") %>' runat="server" 
                                                                    NavigateUrl='<%# "~/frmInvoiceListing.aspx?invno=" + Eval("InvoiceNo")%>' >
                                                                    </asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Type" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductType" Text='<%# Eval("ProductType") %>' runat="server"></asp:Label>
                                                                </ItemTemplate> 
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Comm. Code" Visible="false">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblCommCode" Text='<%# Eval("CommodityCode") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>                                            
                                                            <asp:TemplateField HeaderText="Priority" Visible="false">
                                                                <ItemTemplate> <asp:Label ID="lblPriority" Text='<%# Eval("Priority") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Pcs" >
                                                                <ItemTemplate><asp:Label ID="lblAccPcs" Text='<%# Eval("AcceptedPcs") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Weight" >
                                                                <ItemTemplate><asp:Label ID="lblWeight" Text='<%# Eval("Weight") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>                                            
                                                            <asp:TemplateField HeaderText="Load/Cu M." Visible="false" >
                                                                <ItemTemplate><asp:Label ID="lblVolume" Text='<%# Eval("Volume") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>                                            
	                                                        <asp:TemplateField HeaderText="Status" Visible="false" >
                                                                <ItemTemplate><asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Rate Per Kg" >
                                                                <ItemTemplate><asp:Label ID="lblRatePerKg" Text='<%# Eval("RatePerKg") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Revenue" >
                                                                <ItemTemplate><asp:Label ID="lblrevenue" Text='<%# Eval("Total") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Cost" >
                                                                <ItemTemplate><asp:Label ID="lblCost" Text='<%# Eval("Cost") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Profit" >
                                                                <ItemTemplate><asp:Label ID="lblProfit" Text='<%# Eval("Profit") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Ident Row"  ItemStyle-CssClass="showh"  HeaderStyle-CssClass="showh" Visible="false">
                                                                <ItemTemplate><asp:Label ID="lblIdentRow" Text='<%# Eval("IdentRow") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                   </asp:GridView>
                                                </div>
                                             </td>
                                        </tr>
                                    </table>
                                    <asp:GridView ID="GridView1" AllowPaging="True" AutoGenerateColumns="false" 
                                        style="Z-INDEX: 101" Width="100%" ShowFooter="false" Font-Size="Small" runat="server" 
                                       OnRowDataBound="GridView1_RowDataBound" AllowSorting="true" PageSize="7" Visible="false"
                                       onpageindexchanging="GridView1_PageIndexChanging" HeaderStyle-CssClass="HeaderStyle"  
                                       CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" PagerStyle-CssClass="PagerStyle" 
                                       SelectedRowStyle-CssClass="SelectedRowStyle" >
                                    <RowStyle  HorizontalAlign="Left"/>
                                    <HeaderStyle  Height="30px" HorizontalAlign="center" />
                                        <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <a href="javascript:expandcollapse('div<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>', 'one');">                                                
                                                    <img id="imgdiv<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" alt="Click to show/hide Orders for Customer <%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>"  width="9px" border="0" src="../Images/plus.gif">
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                            
                                        <asp:TemplateField HeaderText="Origin" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Destination" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblDestination" Text='<%# Eval("Destination") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Flight#" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFlightNo" Text='<%# Eval("FlightNo") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Flight Date" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFlightDate" Text='<%# Eval("FlightDate") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Dep Time" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblDepTime" Text='<%# Eval("DeptTime") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
			                              <asp:TemplateField HeaderText="Arr Time" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblArrTime" Text='<%# Eval("ArrTime") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Flt Status" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblFltStatus" Text='<%# Eval("FlightStatus") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                          <asp:TemplateField HeaderText="Maximum" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblMaximum" Text='<%# Eval("Maximum") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Confirmed" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblConfirmed" Text='<%# Eval("Confirmed") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Queued" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblQueued" Text='<%# Eval("Queued") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Blocked" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblBlocked" Text='<%# Eval("Blocked") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                         <asp:TemplateField HeaderText="Available" >
                                            <ItemTemplate>
                                                <asp:Label ID="lblAvailable" Text='<%# Eval("Available") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <a href="javascript:expandcollapseforgreen('div<%# Eval("Origin") +","+ Eval("Destination") +","+ Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>', 'one');">
                                                <img id="imgdiv<%# Eval("Origin") +","+ Eval("Destination") +","+ Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" alt="Click to show/hide<%# Eval("Origin") +","+ Eval("Destination") +","+ Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>"  width="9px" border="0" src="../Images/plusgreen.gif"/>
                                                </a>
                                            </ItemTemplate>
                                        </asp:TemplateField>			    
			                        </Columns>
			                         <%--<HeaderStyle CssClass="titlecolr"/>--%>
                                    <RowStyle  HorizontalAlign="Center"/>
                                    <AlternatingRowStyle  HorizontalAlign="Center"/>
                                </asp:GridView>
                                </div>
                                </asp:Panel>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
           
           <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Import" Width="620px"
                Height="27px">
                <HeaderTemplate>
                  Top Agents
</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="PNImport" runat="server" Width="100%" ScrollBars="Auto">
    
      
     <div id="chartdiv2" style=" display:none; ">
    
    </div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
            
           <asp:TabPanel ID="subTabPanel2" runat="server" HeaderText="FlightDetails" Width="620px"
                Height="27px">
                <HeaderTemplate>Agent AWB Details</HeaderTemplate>
           <ContentTemplate>
           <asp:Panel 
          ID="Panel2" runat="server" Width="100%">
    
      <table width = "100%">
    <tr>
    <td>
  Agent :
    <asp:DropDownList ID="ddlAgent" runat="server">
    <asp:ListItem>ALL</asp:ListItem></asp:DropDownList>
    <asp:Button ID="btnFlightList" Text="List" runat="server" class = "button" OnClick="GetDataPerAgent_Click"  />
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
             
           <%--<asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Export" Width="620px"
                Height="27px" Visible="false">
                <HeaderTemplate>
                    Top Booked Flights
</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
     <div id="chartdiv4" style="display:none; "> 
     </div>
    
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>--%>
           
           <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Export" Width="620px"
                Height="27px">
                <HeaderTemplate>
                     Top Stations </HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel5" runat="server" Width="100%" ScrollBars="Auto">
      <div id="chartdiv8" style="display:none; "></div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>   
               
           <asp:TabPanel ID="tbExportInventory" runat="server" HeaderText="Export WH" Width="620px"
                        ScrollBars="Auto" Visible="false" >
                        <HeaderTemplate>
                           Inventory
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel4" runat="server" Width="100%">
                            <div class="divback">
                                        <table width="100%" cellpadding="3" cellspacing="3">
                                        <tr>
                                            <td>
                                                <asp:Label ID="label12" runat="server" Text="Total Export Tonnage: "></asp:Label>
                                                <asp:Label ID="lblTotalExpTonnage" runat="server" Text=""></asp:Label>
                                                </td>
                                            
                                            <td>
                                                <asp:Label ID="label24" runat="server" Text="Export Revenue: "></asp:Label>
                                                <asp:Label ID="lblExpRevenue" runat="server" Text=""></asp:Label>
                                                </td>
                                            
                                            <td>
                                                <asp:Label ID="label16" runat="server" Text="Import Revenue: "></asp:Label>
                                          
                                                <asp:Label ID="lblImpRevenue" runat="server" Text=""></asp:Label>
                                            </td>
                                          
                                        </tr>
                                        <tr>
                                        <td>
                                                <asp:Label ID="label18" runat="server" Text="Total Import Tonnage: "></asp:Label>
                                            
                                                <asp:Label ID="lblTotalImpTonnage" runat="server" Text=""></asp:Label>
                                            </td>
                                            
                                          
                                            <td>
                                                <asp:Label ID="label30" runat="server" Text=" Export Cost: "></asp:Label>
                                              
                                                <asp:Label ID="lblExpCost" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="label25" runat="server" Text="Import Cost: "></asp:Label>
                                             
                                                <asp:Label ID="lblImpCost" runat="server" Text=""></asp:Label>
                                            </td>
                                              
                                        </tr>
                                        <tr>
                                           <td></td>
                                            
                                          
                                            <td>
                                                <asp:Label ID="label26" runat="server" Text="Export Profit: "></asp:Label>
                                               
                                                <asp:Label ID="lblExpProfit" runat="server" Text=""></asp:Label>
                                            </td>
                                             <td>
                                                <asp:Label ID="label19" runat="server" Text="Import Profit: "></asp:Label>
                                               
                                                <asp:Label ID="lblImpProfit" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        
                                    </table>
                                    </div>
                                <div style="overflow:auto; height:200px; width:100%;">
                                <strong>Export</strong>
                                    <asp:GridView ID="grdexportWareHouseList" runat="server" ShowFooter="false" 
                                        AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2"
                                        CellSpacing="3" PageSize="10" AllowPaging="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="AWB No." HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblAWBNo" Width="100px" runat="server" Text='<%# string.Concat(Eval("AWBPrefix"), "-", Eval("AWBNumber"))%>' />--%>
                                                    <asp:HyperLink ID="hlnkAWBNumber" runat="server" Width="100px" Text='<%# string.Concat(Eval("AWBPrefix"), "-", Eval("AWBNumber"))%>' NavigateUrl='<%# "~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + Eval("AWBPrefix") + "-" + Eval("AWBNumber")%>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Booked Pcs" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedPcs" runat="server"  Text='<%#Eval("Pcs")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Booked Wt" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server"  Text='<%#Eval("Wt")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ProductType" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriority" runat="server" ToolTip='<%#Eval("ProductDescription") %>' Text='<%# Eval("ProductDescription").ToString().PadRight(20).Substring(0,20) %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Priority" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriority" runat="server" Text='<%# Eval("ShipmentPriority") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flt #" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flt Dt." HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlightDt" runat="server" Text='<%# Eval("FlightDate","{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Accp Status" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBooked" runat="server" Text='<%# Eval("AccpStatus") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Accp. Date" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" Width="120px" runat="server" Text='<%# Eval("AccpDate","{0:dd/MM/yyyy HH:mm}") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="Location" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" Width="120px" runat="server" Text='<%# Eval("Location") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Commodity" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("CommodityCode") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Commodity Desc" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" Width="400px" runat="server"   ToolTip='<%#Eval("CommodityDesc") %>' Text='<%# Eval("CommodityDesc").ToString().PadRight(50).Substring(0,50)+"..." %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="titlecolr" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                    
                                    <strong>Import</strong>
                                    <asp:GridView ID="grdImportWarehouse" runat="server" ShowFooter="false" 
                                        AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2"
                                        CellSpacing="3" PageSize="10" AllowPaging="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="AWB No." HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblAWBNo" Width="100px" runat="server" Text='<%# string.Concat(Eval("AWBPrefix"), "-", Eval("AWBNumber"))%>' />--%>
                                                    <asp:HyperLink ID="hlnkAWBNumber" runat="server" Width="100px" Text='<%# string.Concat(Eval("AWBPrefix"), "-", Eval("AWBNumber"))%>' NavigateUrl='<%# "~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + Eval("AWBPrefix") + "-" + Eval("AWBNumber")%>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arrived Pcs" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArrivedPcs" runat="server"  Text='<%#Eval("Pcs")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Arrived Wt" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblArrivedWt" runat="server"  Text='<%#Eval("Wt")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Type" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriority" Width="200px" runat="server"  ToolTip='<%#Eval("CommodityDesc") %>' Text='<%# Eval("CommodityDesc").ToString().PadRight(20).Substring(0,20)+"..." %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Priority" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriority" runat="server" Text='<%# Eval("ShipmentPriority") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flt #" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flt Dt." HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlightDt" runat="server" Text='<%# Eval("FlightDate","{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("FlightOrigin") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dest." HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("FlightDestination") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arr. Pcs" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("Pcs") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arr. Wt" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("Wt") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arr. Date" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" Width="120px" runat="server" Text='<%# Eval("ArrDate","{0:dd/MM/yyyy HH:mm}") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Dly. Status" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBooked" runat="server" Text='<%# Eval("DlvStatus") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>                                                                                                                                   
                                            <asp:TemplateField HeaderText="Commodity" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("CommodityCode") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Commodity Desc" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" Width="400px" runat="server" ToolTip='<%#Eval("CommodityDesc") %>' Text='<%# Eval("CommodityDesc").ToString().PadRight(50).Substring(0,50)+"..." %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="titlecolr" />
                                        <RowStyle HorizontalAlign="Center" />
                                        <AlternatingRowStyle HorizontalAlign="Center" />
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
            <asp:HiddenField ID="hdAgent" runat="server" />
              <asp:HiddenField ID="hdStation" runat="server" />
    </td>
    </tr>
    </table>
      </td>
      </tr>
      </table>
    </asp:Panel>