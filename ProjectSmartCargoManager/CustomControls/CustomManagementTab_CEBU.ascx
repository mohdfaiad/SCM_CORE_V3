<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomManagementTab_CEBU.ascx.cs" Inherits="ProjectSmartCargoManager.CustomControls.CustomManagementTab_CEBU" %>

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

        
        
    
//*********************************CEBU SPECIFIC TABS******************************************//


       //Top Ten Locations Based ON Yield

        var plotLocationYield;
        function DisplayGraph9() {
            var PlotDiv = document.getElementById('chartdiv9').style.display = "block";
        }
        function HideGraph9() {
            var PlotDiv = document.getElementById('chartdiv9').style.display = "none";

        }


        // Top Ten Locations
        function GeneratePlotLocationYield(JS) {
            try {
                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv9");
                var dataBar = JSONString[0].Origin;
                var dataLine = JSONString[0].Origin;
                var line1 = [];
                var line2 = [];
                var line3=[];
                if (JSONString.length > 0) {
                    DisplayGraph9();
                }

                for (i = 0; i < JSONString.length; i++) {
                    line1[i] = JSONString[i].Origin;
                    line2[i] = JSONString[i].Yield;
                    line3[i]=JSONString[i].AirportName;
                    

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

                plotLocationYield = $.jqplot('chartdiv9', [line2], {
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
            { label: 'Yield' },
        ],
                    legend: {
                        show: true,
                        location: 'e',
                        placement: 'inside'
                    },
                    highlighter: {
                        show: true,
                        sizeAdjust: 3,
                        tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                            return line3[pointIndex];

                        }

                    }

                });
                $('#chartdiv9').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
            var FromDate=document.getElementById("<%=txtFrmDate.ClientID %>").value;
            var ToDate=document.getElementById("<%=txtToDate.ClientID %>").value;
                window.open("rptDailySalesReport.aspx?Station="+line1[pointIndex]+"&FromDate="+FromDate+"&ToDate="+ToDate);
                

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv9').bind('resize', function(event, ui) {

                    plotLocationYield.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph9();
            }
        }

        //END
        
        
         //Top Ten Locations Based ON Revenue

        var plotLocationRevenue;
        function DisplayGraph10() {
            var PlotDiv = document.getElementById('chartdiv10').style.display = "block";
        }
        function HideGraph10() {
            var PlotDiv = document.getElementById('chartdiv10').style.display = "none";

        }


        // Top Ten Locations
        function GeneratePlotLocationRevenue(JS) {
            try {
                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv10");
                var dataBar = JSONString[0].Origin;
                var dataLine = JSONString[0].Origin;
                var line1 = [];
                var line2 = [];
                var line3=[];
                if (JSONString.length > 0) {
                    DisplayGraph10();
                }

                for (i = 0; i < JSONString.length; i++) {
                    line1[i] = JSONString[i].Origin;
                    line2[i] = JSONString[i].Revenue;
                    line3[i] = JSONString[i].AirportName;
                    

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

                plotLocationRevenue = $.jqplot('chartdiv10', [line2], {
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
            { label: 'Revenue' },
        ],
                    legend: {
                        show: true,
                        location: 'e',
                        placement: 'inside'
                    },
                    highlighter: {
                        show: true,
                        sizeAdjust: 3,
                        tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                            return line3[pointIndex];

                        }

                    }

                });
                $('#chartdiv10').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
            var FromDate=document.getElementById("<%=txtFrmDate.ClientID %>").value;
            var ToDate=document.getElementById("<%=txtToDate.ClientID %>").value;
                window.open("rptDailySalesReport.aspx?Station="+line1[pointIndex]+"&FromDate="+FromDate+"&ToDate="+ToDate);
                

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv10').bind('resize', function(event, ui) {

                    plotLocationRevenue.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph10();
            }
        }

        //END
        
        
                //Top Ten Locations Based ON Volume

        var plotLocationVolume;
        function DisplayGraph11() {
            var PlotDiv = document.getElementById('chartdiv11').style.display = "block";
        }
        function HideGraph11() {
            var PlotDiv = document.getElementById('chartdiv11').style.display = "none";

        }


        // Top Ten Locations
        function GeneratePlotLocationVolume(JS) {
            try {
                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv11");
                var dataBar = JSONString[0].Origin;
                var dataLine = JSONString[0].Origin;
                var line1 = [];
                var line2 = [];
                var line3=[];
                if (JSONString.length > 0) {
                    DisplayGraph11();
                }

                for (i = 0; i < JSONString.length; i++) {
                    line1[i] = JSONString[i].Origin;
                    line2[i] = JSONString[i].Volume;
                    line3[i] = JSONString[i].AirportName;
                    

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

                plotLocationVolume = $.jqplot('chartdiv11', [line2], {
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
            { label: 'Volume' },
        ],
                    legend: {
                        show: true,
                        location: 'e',
                        placement: 'inside'
                    },
                    highlighter: {
                        show: true,
                        sizeAdjust: 3,
                        tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                            return line3[pointIndex];

                        }

                    }

                });
                $('#chartdiv11').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
                var FromDate=document.getElementById("<%=txtFrmDate.ClientID %>").value;
            var ToDate=document.getElementById("<%=txtToDate.ClientID %>").value;
                window.open("rptDailySalesReport.aspx?Station="+line1[pointIndex]+"&FromDate="+FromDate+"&ToDate="+ToDate);
            
                

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv11').bind('resize', function(event, ui) {

                    plotLocationVolume.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph11();
            }
        }

        //END
 
    
                //Top Ten Locations Based ON Volume

        var plotLocationShipper;
        function DisplayGraph12() {
            var PlotDiv = document.getElementById('chartdiv12').style.display = "block";
        }
        function HideGraph12() {
            var PlotDiv = document.getElementById('chartdiv12').style.display = "none";

        }


        // Top Ten Locations
        function GeneratePlotLocationShipper(JS) {
            try {
                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv12");
                var dataBar = JSONString[0].Shipper;
                var dataLine = JSONString[0].Shipper;
                var line1 = [];
                var line2 = [];
                var line3=[];
                if (JSONString.length > 0) {
                    DisplayGraph12();
                }

                for (i = 0; i < JSONString.length; i++) {
                    line1[i] = JSONString[i].Shipper;
                    line2[i] = JSONString[i].Revenue;
                    line3[i] = JSONString[i].ShipperName;
                    

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

                plotLocationShipper = $.jqplot('chartdiv12', [line2], {
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
            { label: 'Shipper' },
        ],
                    legend: {
                        show: true,
                        location: 'e',
                        placement: 'inside'
                    },
                    highlighter: {
                        show: true,
                        sizeAdjust: 3,
                        tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                            return line3[pointIndex];

                        }

                    }

                });
                $('#chartdiv12').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
               var FromDate=document.getElementById("<%=txtFrmDate.ClientID %>").value;
            var ToDate=document.getElementById("<%=txtToDate.ClientID %>").value;
                window.open("rptDailySalesReport.aspx?Shipper="+line1[pointIndex]+"&FromDate="+FromDate+"&ToDate="+ToDate);
            
                

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv12').bind('resize', function(event, ui) {

                    plotLocationShipper.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph12();
            }
        }

        //END

    

                    //Top Ten Locations Based ON Commodity

        var plotLocationVolume;
        function DisplayGraph13() {
            var PlotDiv = document.getElementById('chartdiv13').style.display = "block";
        }
        function HideGraph13() {
            var PlotDiv = document.getElementById('chartdiv13').style.display = "none";

        }


        // Top Ten Locations
        function GeneratePlotLocationCommodity(JS) {
            try {
                //succes - data loaded, now use plot:
                var JSONString = JSON.parse(JS);
                var plotarea = $("#chartdiv13");
                var dataBar = JSONString[0].Commodity;
                var dataLine = JSONString[0].Commodity;
                var line1 = [];
                var line2 = [];
                var line3=[];
                if (JSONString.length > 0) {
                    DisplayGraph13();
                }

                for (i = 0; i < JSONString.length; i++) {
                    line1[i] = JSONString[i].Commodity;
                    line2[i] = JSONString[i].Revenue;
                    line3[i] = JSONString[i].CommodityName;
                    

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

                plotLocationCommodity = $.jqplot('chartdiv13', [line2], {
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
            { label: 'Commodity' },
        ],
                    legend: {
                        show: true,
                        location: 'e',
                        placement: 'inside'
                    },
                    highlighter: {
                        show: true,
                        sizeAdjust: 3,
                        tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                            return line3[pointIndex];

                        }

                    }

                });
                $('#chartdiv13').bind('jqplotDataClick',
            function(ev, seriesIndex, pointIndex, data) {
                var FromDate=document.getElementById("<%=txtFrmDate.ClientID %>").value;
            var ToDate=document.getElementById("<%=txtToDate.ClientID %>").value;
                window.open("rptDailySalesReport.aspx?Commodity="+line1[pointIndex]+"&FromDate="+FromDate+"&ToDate="+ToDate);
            
                

            }
        );

                //            $('#chartdiv').resizable({ delay: 20 });
                //            $('#chartdiv').bind('resize', function(event, ui) {
                //                plot2.replot();
                //            });
                $('#chartdiv13').bind('resize', function(event, ui) {

                    plotLocationCommodity.replot({ resetAxes: true });
                });

            }
            catch (Err) {
                HideGraph13();
            }
        }

        //END



    
    
    
    
    


    
    
    
    


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
            
 
               <asp:TabPanel ID="TabPanel99" runat="server" HeaderText="Yield" Width="620px"
                Height="27px">
                <HeaderTemplate>
                     Yields</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel20" runat="server" Width="100%" ScrollBars="Auto">
      <div id="chartdiv9" style="display:none; "></div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Yield" Width="620px"
                Height="27px">
                <HeaderTemplate>
                     Revenues</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel21" runat="server" Width="100%" ScrollBars="Auto">
      <div id="chartdiv10" style="display:none; "></div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel5" runat="server" HeaderText="Revenue" Width="620px"
                Height="27px">
                <HeaderTemplate>
                     Volumes</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel1" runat="server" Width="100%" ScrollBars="Auto">
      <div id="chartdiv11" style="display:none; "></div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
                <asp:TabPanel ID="TabPanel6" runat="server" HeaderText="Volume" Width="620px"
                Height="27px">
                <HeaderTemplate>
                     Shippers</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel6" runat="server" Width="100%" ScrollBars="Auto">
      <div id="chartdiv12" style="display:none; "></div>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
                
                <asp:TabPanel ID="TabPanel7" runat="server" HeaderText="Volume" Width="620px"
                Height="27px">
                <HeaderTemplate>
                     Commodities</HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel7" runat="server" Width="100%" ScrollBars="Auto">
      <div id="chartdiv13" style="display:none; "></div>
    
      
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
    
    