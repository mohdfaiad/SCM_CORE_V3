<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JQPlots.aspx.cs" Inherits="ProjectSmartCargoManager.JQPlots" MasterPageFile ="~/SmartCargoMaster.Master"  %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

<script src="/js/libs/jquery.validate.js" type="text/javascript"></script>
  
  <script language="javascript" type="text/javascript" src="scripts/excanvas.js"></script>
 <script language="javascript" type="text/javascript" src="scripts/jquery.min.js"></script>
 <script language="javascript" type="text/javascript" src="scripts/jquery.jqplot.min.js"></script>
 <script type="text/javascript" src="scripts/jqplot.barRenderer.min.js"></script>
<script type="text/javascript" src="scripts/jqplot.categoryAxisRenderer.min.js"></script>
<script type="text/javascript" src="scripts/jqplot.pointLabels.min.js"></script>
<script type="text/javascript" src="scripts/plugin/jqplot.json2.min.js"></script>
<script type="text/javascript" src="scripts/plugin/jqplot.json2.js"></script>
<script type="text/javascript" src="scripts/plugin/jqplot.canvasTextRenderer.min.js"></script>
<script type="text/javascript" src="scripts/plugin/jqplot.canvasAxisTickRenderer.min.js"></script>
<script type="text/javascript" src="scripts/plugin/jqplot.enhancedLegendRenderer.js"></script>
<script type="text/javascript" src="scripts/plugin/jqplot.enhancedLegendRenderer.min.js"></script>
 <%--//    var Plot1 = $.jqplot('chartdiv', [[[1, 2], [3, 5.12], [5, 13.1], [7, 33.6], [9, 85.9], [11, 219.9]]]);
 //        var s2 = [460, -210, 690, 820];
        //        var s3 = [-260, -440, 320, 200]; , s2, s3--%>
 <link rel="stylesheet" type="text/css" href="style/jquery.jqplot.css" />
 <%--<link rel="stylesheet1" type="text/css" href="style/jquery.jqplot.min.css" />--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

<script type="text/javascript">
    function CallPopulateClick() {
        Final();
    }
    
    function Clear() {
        //alert("Clear");
        $('chartdiv').empty(); // Chart Div emptying
        $('chartdiv1').empty(); // Chart Div emptying
        $('chartdiv2').empty(); // Chart Div emptying
        $('chartdiv3').empty(); // Chart Div emptying
        $('chartdiv4').empty(); // Chart Div emptying
        //        $('chartdiv5').empty(); // Chart Div emptying
        $("#chartdiv").html("");
        $("#chartdiv1").html("");
        $("#chartdiv2").html("");
        $("#chartdiv3").html("");
        $("#chartdiv4").html("");
        $("#chartdiv5").html("");
        
        //alert("Clear End");
    }

    function RetMaxVal(Values) {
        var Tonlength = (Values + "").length;
        //alert(Tonlength);
        var FirstDigit = parseInt((Values + "")[0]);
        //alert(FirstDigit);
        var tenPowerInt = Math.pow(10, parseInt(Tonlength) - 1);
        //alert(tenPowerInt);
        var MaxVal = (FirstDigit + 5) * tenPowerInt;
        //alert(MaxVal);
        return MaxVal;
    }

    function RetMaximum(Value1, Value2, Value3, Value4, Value5) {
        var Maximum = Math.max(Value1, Math.max(Value2, Math.max(Value3, Math.max(Value4, Value5))));
        return Maximum;
    }

//    $(document).ready(function() {
//        var s1 = [2000, 6000, 7000, 10000];
//        var s2 = [7000, 5000, 3000, 4000];
//        var s3 = [1400, 900, 300, 800];
//        var ticks = ['Agent1', 'Agent2', 'Agent3'];
//        plot3 = $.jqplot('chartdiv5', [s1, s2, s3], {
//            // Tell the plot to stack the bars.
//            stackSeries: true,
//            captureRightClick: true,
//            seriesDefaults: {
//                renderer: $.jqplot.BarRenderer,
//                rendererOptions: {
//                    // Put a 30 pixel margin between bars.
//                    barMargin: 30,
//                    // Highlight bars when mouse button pressed.
//                    // Disables default highlighting on mouse over.
//                    highlightMouseDown: true
//                },
//                pointLabels: { show: true },
//                rendererOptions: {
//                    barDirection: 'horizontal'
//                }
//            },
//            axes: {
//                xaxis: {
//                padMin: 0,
//                
//                },
//                yaxis: {
//                    // Don't pad out the bottom of the data range.  By default,
//                    // axes scaled as if data extended 10% above and below the
//                    // actual range to prevent data points right on grid boundaries.
//                    // Don't want to do that here.
//                    renderer: $.jqplot.CategoryAxisRenderer,
//                    ticks: ticks
//                }
//            },
//            legend: {
//                show: true,
//                location: 'e',
//                placement: 'outside'
//            }
//        });
//        // Bind a listener to the "jqplotDataClick" event.  Here, simply change
//        // the text of the info3 element to show what series and ponit were
//        // clicked along with the data for that point.
//        $('#chart3').bind('jqplotDataClick',
//    function(ev, seriesIndex, pointIndex, data) {
//        $('#info3').html('series: ' + seriesIndex + ', point: ' + pointIndex + ', data: ' + data);
//    }
//  );
//    });

    function Final() {
        //$($get("populate")).click(function()
        //            Swapnil

        Clear();
        
        var Agent1;
        Agent1 = $('#<%=lblAgent1.ClientID%>').html();

        var Agent2;
        Agent2 = $('#<%=Label2.ClientID%>').html();


        var Tonnage1 = $('#<%=Label6.ClientID%>').html();


        var Agent3;
        Agent3 = $('#<%=Label3.ClientID%>').html();
        //alert(Agent3);

        var Agent4;
        Agent4 = $('#<%=Label4.ClientID%>').html();

        var Agent5;
        Agent5 = $('#<%=Label5.ClientID%>').html();


        var T1 = $('#<%=Label6.ClientID%>').html();

        var Tonnage1 = parseInt(T1);


        var T2 = $('#<%=Label7.ClientID%>').html();
        var Tonnage2 = parseInt(T2);

        var T3 = $('#<%=Label8.ClientID%>').html();
        var Tonnage3 = parseInt(T3);

        var T4 = $('#<%=Label9.ClientID%>').html();
        var Tonnage4 = parseInt(T4);

        var T5 = $('#<%=Label10.ClientID%>').html();
        //var T5JS = document.getElementById('<%=Label10.ClientID%>').innerHTML;
        //alert(T5JS);
        //alert(T5);
        var Tonnage5 = parseInt(T5);

        //--------------------------------------------------------------------
        var Origin1 = $('#<%=lblSector1.ClientID%>').html();
        //alert(Origin1);


        var Origin2 = $('#<%=lblSector2.ClientID%>').html();
        //alert(Origin2);

        var Origin3 = $('#<%=lblSector3.ClientID%>').html();
        //alert(Origin3);


        var origin4 = $('#<%=lblSector4.ClientID%>').html();
        //alert(origin4);


        var origin5 = $('#<%=lblSector5.ClientID%>').html();
        //alert(origin5);

        var c1 = $('#<%=lblTonnage1.ClientID%>').html();
        var ctonage1 = parseInt(c1);
        //alert(ctonage1);

        var c2 = $('#<%=lblTonnage2.ClientID%>').html();
        var ctonage2 = parseInt(c2);
        //alert(ctonage2);

        var c3 = $('#<%=lblTonnage3.ClientID%>').html();
        var ctonage3 = parseInt(c3);
        //alert(ctonage3);
        var c4 = $('#<%=lblTonnage4.ClientID%>').html();
        var ctonage4 = parseInt(c4);

        var c5 = $('#<%=lblTonnage5.ClientID%>').html();
        var ctonage5 = parseInt(c5);
        //alert(ctonage5);
        //----------------------------------------------------------------------


        var s1 = [Tonnage1, Tonnage2, Tonnage3, Tonnage4, Tonnage5];
        //var TonString = Tonnage1+"";
       // alert("");
//        var Tonlength = (Tonnage1+"").length;
//        alert(Tonlength);
//        var FirstDigit = parseInt((Tonnage1+"")[0]);
//        alert(FirstDigit);
//        var tenPowerInt = Math.pow(10,parseInt(Tonlength)-1);
//        alert(tenPowerInt);
//        var MaxVal = (FirstDigit + 2)*tenPowerInt;
//        alert(MaxVal);
        //var s1 = [Show];
        var ticks = [Agent1, Agent2, Agent3, Agent4, Agent5];
        var plot1 = $.jqplot('chartdiv', [s1], {
        title: 'Top 5 Agents',
        
            //            axes: { yaxis: { min: 0} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                color: '#5FAB78',
                pointLabels: { show: true }
            },
//            series: [
//            { label: 'Agent' }
//            ],
//            legend: {
//                show: true,
//                placement: 'outsideGrid'
            //            },
            
            axes: {
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks
                },
                yaxis: {
                pad: 1.05,
                min: 0,
                max: parseInt(RetMaxVal(Tonnage1)),
                tickOptions: { formatString: '%d' }
                }
            },
            series: [
            { label: 'X:Agents<br />Y:Charged Wt(Kg)' }
                ],
            legend: {
                show: true,
                placement: 'insideGrid',
                showSwatches: false,
                showLabels: true
            }
        });
        //---------------------------------------------------------
        //var s2 = [100, 200,300, 400,500];
        var s2 = [ctonage1, ctonage2, ctonage3, ctonage4, ctonage5];
        //            alert(ctonage1);
        //            alert(ctonage2);
        //            alert(ctonage3);
        //            alert(ctonage4);
        //            alert(ctonage5);
        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        var ticks1 = [Origin1, Origin2, Origin3, origin4, origin5];

        var plot2 = $.jqplot('chartdiv1', [s2], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            title: 'Top 5 Sectors',
//            axes: { yaxis: { min: 0} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                color: '#5FAB78',
                pointLabels: { show: true }
            },
            // Custom labels for the series are specified with the "label"
            // option on the series option.  Here a series option object
            // is specified for each series.
            
//            series: [
//            { label: 'Sectors' }
//            ],
//            // Show the legend and put it outside the grid, but inside the
//            // plot container, shrinking the grid to accomodate the legend.
//            // A value of "outside" wou5ld not shrink the grid and allow
//            // the legend to overflow the container.
//            legend: {
//                show: true,
//                placement: 'outsideGrid'
//            },
//            
            axes: {
                // Use a category axis on the x axis and use our custom ticks.
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks1
                },
                // Pad the y axis just a little so bars can get close to, but
                // not touch, the grid boundaries.  1.2 is the default padding.
                yaxis: {
                pad: 1.05,
                min: 0,
                max: parseInt(RetMaxVal(ctonage1)),
                    tickOptions: { formatString: '%d' }
                }
            },
            series: [
            { label: 'X:Sectors<br />Y:Tonnage(Kg)' }
                ],
            legend: {
                show: true,
                placement: 'insideGrid',
                showSwatches: false,
                showLabels: true
            }
        });
        //----------------------------------------------------------------------


        var BottomAgent1 = $('#<%=lblBtonage1.ClientID%>').html();
        var BottomAgent2 = $('#<%=lblBtonage2.ClientID%>').html();
        var BottomAgent3 = $('#<%=lblBtonage3.ClientID%>').html();
        var BottomAgent4 = $('#<%=lblBtonage4.ClientID%>').html();
        var BottomAgent5 = $('#<%=lblBtonage5.ClientID%>').html();

        var B1 = $('#<%=lblBotom1.ClientID%>').html();
        var botomc1 = parseInt(B1);

        var B2 = $('#<%=lblBotom2.ClientID%>').html();
        var botomc2 = parseInt(B2);

        var B3 = $('#<%=lblBotom3.ClientID%>').html();
        var botomc3 = parseInt(B3);

        var B4 = $('#<%=lblBotom4.ClientID%>').html();
        var botomc4 = parseInt(B4);

        var B5 = $('#<%=lblBotom5.ClientID%>').html();
        var botomc5 = parseInt(B5);



        //=----------------------------------------------------------------------
        var s3 = [botomc1, botomc2, botomc3, botomc4, botomc5];

        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        var ticks2 = [BottomAgent1, BottomAgent2, BottomAgent3, BottomAgent4, BottomAgent5];

        var plot2 = $.jqplot('chartdiv2', [s3], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            title: 'Bottom 5 Agents',
//            axes: { yaxis: { min: -10, max: 2000} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                color: '#5FAB78',
                pointLabels: { show: true }
            },
            // Custom labels for the series are specified with the "label"
            // option on the series option.  Here a series option object
            // is specified for each series.
            
//            series: [
//            { label: 'Agents' }
//            ],
//            // Show the legend and put it outside the grid, but inside the
//            // plot container, shrinking the grid to accomodate the legend.
//            // A value of "outside" would not shrink the grid and allow
//            // the legend to overflow the container.
//            legend: {
//                show: true,
//                placement: 'outsideGrid'
//            },
            
            axes: {
                // Use a category axis on the x axis and use our custom ticks.
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks2
                },
                // Pad the y axis just a little so bars can get close to, but
                // not touch, the grid boundaries.  1.2 is the default padding.
                yaxis: {
                pad: 1.05,
                min: 0,
                max: parseInt(RetMaxVal(RetMaximum(botomc1,botomc2,botomc3,botomc4,botomc5))),
                    tickOptions: { formatString: '%d' }
                }
            },
            series: [
            { label: 'X:Agents<br />Y:Charged Wt(Kg)' }
                ],
            legend: {
                show: true,
                placement: 'insideGrid',
                showSwatches: false,
                showLabels: true
            }
        });
        //-----------------------------------------------------------------------------------

        var BottomSector1 = $('#<%=lblbotsector1.ClientID%>').html();
        var BottomSector2 = $('#<%=lblbotsector2.ClientID%>').html();
        var BottomSector3 = $('#<%=lblbotsector3.ClientID%>').html();
        var BottomSector4 = $('#<%=lblbotsector4.ClientID%>').html();
        var BottomSector5 = $('#<%=lblbotsector5.ClientID%>').html();

        var C1 = $('#<%=lblbottonnage1.ClientID%>').html();
        var botomtonnage1 = parseInt(C1);

        var C2 = $('#<%=lblbottonnage2.ClientID%>').html();
        var botomtonnage2 = parseInt(C2);

        var C3 = $('#<%=lblbottonnage3.ClientID%>').html();
        var botomtonnage3 = parseInt(C3);

        var C4 = $('#<%=lblbottonnage4.ClientID%>').html();
        var botomtonnage4 = parseInt(C4);

        var C5 = $('#<%=lblbottonnage5.ClientID%>').html();
        var botomtonnage5 = parseInt(C5);


        var s4 = [botomtonnage1, botomtonnage2, botomtonnage3, botomtonnage4, botomtonnage5];

        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        var ticks4 = [BottomSector1, BottomSector2, BottomSector3, BottomSector4, BottomSector5];

        var plot4 = $.jqplot('chartdiv3', [s4], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            title: 'Bottom 5 Sectors',
//            axes: { yaxis: { min: -10, max: 2000} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                color: '#5FAB78',
                pointLabels: { show: true }
            },
            // Custom labels for the series are specified with the "label"
            // option on the series option.  Here a series option object
            // is specified for each series.
            
//            series: [
//            { label: 'Sectors' }
//            ],
//            // Show the legend and put it outside the grid, but inside the
//            // plot container, shrinking the grid to accomodate the legend.
//            // A value of "outside" would not shrink the grid and allow
//            // the legend to overflow the container.
//            legend: {
//                show: true,
//                placement: 'outsideGrid'
//            },
//            
            axes: {
                // Use a category axis on the x axis and use our custom ticks.
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks4
                },
                // Pad the y axis just a little so bars can get close to, but
                // not touch, the grid boundaries.  1.2 is the default padding.
                yaxis: {
                pad: 1.05,
                min: 0,
                max: parseInt(RetMaxVal(RetMaximum(botomtonnage1, botomtonnage2, botomtonnage3, botomtonnage4, botomtonnage5))),
                    tickOptions: { formatString: '%d' }
                }
            },
            series: [
            { label: 'X:Sectors<br />Y:Tonnage(Kg)' }
                ],
            legend: {
                show: true,
                placement: 'insideGrid',
                showSwatches: false,
                showLabels: true
            }
        });
        //----------------------------

        var PendingInvoice1 = $('#<%=lblPendingInvoice1.ClientID%>').html();
        var PendingInvoice2 = $('#<%=lblPendingInvoice2.ClientID%>').html();
        var PendingInvoice3 = $('#<%=lblPendingInvoice3.ClientID%>').html();
        var PendingInvoice4 = $('#<%=lblPendingInvoice4.ClientID%>').html();
        var PendingInvoice5 = $('#<%=lblPendingInvoice5.ClientID%>').html();

        var PIV1 = $('#<%=lblPendingInvoiceVal1.ClientID%>').html();
        var PIV2 = $('#<%=lblPendingInvoiceVal2.ClientID%>').html();
        var PIV3 = $('#<%=lblPendingInvoiceVal3.ClientID%>').html();
        var PIV4 = $('#<%=lblPendingInvoiceVal4.ClientID%>').html();
        var PIV5 = $('#<%=lblPendingInvoiceVal5.ClientID%>').html();


        var PIVal1 = parseInt(PIV1);
        var PIVal2 = parseInt(PIV2);
        var PIVal3 = parseInt(PIV3);
        var PIVal4 = parseInt(PIV4);
        var PIVal5 = parseInt(PIV5);
        
        var s5 = [PIVal1, PIVal2, PIVal3, PIVal4, PIVal5];

        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        var ticks5 = [PendingInvoice1, PendingInvoice2, PendingInvoice3, PendingInvoice4, PendingInvoice5];

        var plot5 = $.jqplot('chartdiv4', [s5], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            title: 'Top 5 Pending Invoices',
//            axes: { yaxis: { min: -10, max: 2000} },
            axesDefaults: {
                tickRenderer: $.jqplot.CanvasAxisTickRenderer,
                tickOptions: {
                    angle: -30,
                    fontSize: '10pt'
                }
            },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                color: '#5FAB78',
                pointLabels: { show: true }
            },
            // Custom labels for the series are specified with the "label"
            // option on the series option.  Here a series option object
            // is specified for each series.
            
//            series: [
//            { label: 'Pending Invoice' }
//            ],
//            // Show the legend and put it outside the grid, but inside the
//            // plot container, shrinking the grid to accomodate the legend.
//            // A value of "outside" would not shrink the grid and allow
//            // the legend to overflow the container.
//            legend: {
//                show: true,
//                placement: 'outsideGrid'
//            },
            
            axes: {
                // Use a category axis on the x axis and use our custom ticks.
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks5
                },
                // Pad the y axis just a little so bars can get close to, but
                // not touch, the grid boundaries.  1.2 is the default padding.
                yaxis: {
                pad: 1.05,
                min: 0,
                max: parseInt(RetMaxVal(RetMaximum(PIVal1,PIVal2,PIVal3,PIVal4,PIVal5))),
                    tickOptions: { formatString: '%dk' }
                }
            },
            series: [
            { label: 'X:Agents<br />Y:Amount(Rs)' }
                ],
            legend: {
                show: true,
                placement: 'insideGrid',
                showSwatches: false,
                showLabels: true
            }
        });
        
        //------------------------------------------
       
        var TotPendAmount1 = $('#<%=lblTotPendAmount1.ClientID%>').html();
        
        var TotColAmount1 = $('#<%=lblTotColAmount1.ClientID%>').html();

        var s7 = [parseInt(TotPendAmount1)];
        var s8 = [parseInt(TotColAmount1)];
        
        var ticks = ['Total(k)'];
        plot7 = $.jqplot('chartdiv5', [s7, s8], {
            // Tell the plot to stack the bars.
        stackSeries: true,
        title: 'Total Invoice Amount',    
            captureRightClick: true,
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: {
                    // Put a 30 pixel margin between bars.
                    barMargin: 30,
                    // Highlight bars when mouse button pressed.
                    // Disables default highlighting on mouse over.
                    highlightMouseDown: true
                    
                },
                pointLabels: { show: true },
                rendererOptions: {
                    barDirection: 'horizontal',
                    barWidth: 30
                }
            },
            axes: {
                xaxis: {
                padMin: 0,
                min: 0
                },
                yaxis: {
                    // Don't pad out the bottom of the data range.  By default,
                    // axes scaled as if data extended 10% above and below the
                    // actual range to prevent data points right on grid boundaries.
                    // Don't want to do that here.
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks
                }
            },
            series: [
            { label: 'Total Pending' },
            { label: 'Total Collected' }
                ],
            legend: {
                show: true,
                placement: 'insideGrid'
            }
        });
        
        $('#<%=divLabels.ClientID %>').hide();
    }

    
    
</script>

<script type="text/javascript">



    $(document).ready(function() {
        $('#<%= populate.ClientID %>').click(function(e) {
            Final();
        });
    });

</script>
<asp:UpdatePanel ID="up" runat="server">
<ContentTemplate>
    <asp:Timer ID="Timer1" runat="server"  Interval="15000" 
        ontick="Timer1_Tick">
    </asp:Timer>
  
    <div id="contentarea">
    <h1><img src="Images/txt_dashboard.png" /></h1>
    <div class="botline" style="margin-bottom:20px;">
    <table style="width:80%">
    <tr>
    <td>
    From Date :
    </td>
    <td align = "left">
        <asp:TextBox ID="txtFrmDate" runat="server" Width="100px"></asp:TextBox>
         <asp:CalendarExtender ID="txtFrmDate_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtFrmDate" Format="MM/dd/yyyy" >
         </asp:CalendarExtender>
    </td>
    <td>
    To Date :
    </td>
    <td align = "left">
        <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
        <asp:CalendarExtender ID="txtToDate_CalendarExtender" runat="server" 
          Enabled="True" TargetControlID="txtToDate" Format="MM/dd/yyyy" >
        </asp:CalendarExtender>
    </td>
    <td>
        <asp:Button ID="GetData" runat="server" Text="GetData" class = "button" 
            onclick="GetData_Click"/>
        
    </td>
    </tr>
    </table>
    </div><br />
    <table width = "100%">
    <tr><td></td><td></td><td></td></tr>
    <tr align = "center">
    <td align = "center" >
    <div id="chartdiv">
    </div>
    </td>
    <td align = "center">
    <div id="chartdiv2"></div>
    
    </td>
    <td align = "center">
    <div id="chartdiv5"></div>
    </td>
    </tr>
    <tr><td></td><td></td><td></td></tr>
        
    <tr align = "center">
    <td align = "center">
    <div id="chartdiv1">
    </div>
    </td>
    <td align = "center">
    <div id="chartdiv3"></div>
    </td>
    <td align = "center">
    
    <div id="chartdiv4"></div>
    </td>
    </tr>
    
    </table>
    </div>
    
    
    
    <div id = "divLabels" style="width:0%;background-color:White" runat = "server" >
        <asp:Button id="populate" runat="server" value = "Populate" class = "button" Text = "" Height = "0px" Width = "0px" EnableViewState = "true" Visible = "false" />
        <asp:Label ID="lblAgent1" runat="server" EnableViewState="true" Width="0px" ></asp:Label>
        <asp:Label ID="Label2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label6" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label7" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label8" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label9" runat="server" EnableViewState="true" Width="0px"></asp:Label>
        <asp:Label ID="Label10" runat="server" Width="0px" BackColor="White"></asp:Label>
        
        
         <asp:Label ID="lblSector1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblSector2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
           <asp:Label ID="lblSector3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
            <asp:Label ID="lblSector4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             <asp:Label ID="lblSector5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             
             
             
         <asp:Label ID="lblTonnage1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblTonnage2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
           <asp:Label ID="lblTonnage3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
            <asp:Label ID="lblTonnage4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             <asp:Label ID="lblTonnage5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
 <%-------------------------------------------------------------------------------------------------%>
 
 
         <asp:Label ID="lblBotom1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblBotom2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
           <asp:Label ID="lblBotom3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
            <asp:Label ID="lblBotom4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             <asp:Label ID="lblBotom5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
 
          <asp:Label ID="lblBtonage1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblBtonage2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblBtonage3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblBtonage4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblBtonage5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
 <%-------------------------------------------------------------------------------------------------%>
          <asp:Label ID="lblbotsector1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblbotsector2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
           <asp:Label ID="lblbotsector3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
            <asp:Label ID="lblbotsector4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             <asp:Label ID="lblbotsector5" runat="server" EnableViewState="true" Width="0px"></asp:Label>  
             
         <asp:Label ID="lblbottonnage1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblbottonnage2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
           <asp:Label ID="lblbottonnage3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
            <asp:Label ID="lblbottonnage4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             <asp:Label ID="lblbottonnage5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
 
         <asp:Label ID="lblPendingInvoice1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblPendingInvoice2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
           <asp:Label ID="lblPendingInvoice3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
            <asp:Label ID="lblPendingInvoice4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             <asp:Label ID="lblPendingInvoice5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
         
         <asp:Label ID="lblPendingInvoiceVal1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
          <asp:Label ID="lblPendingInvoiceVal2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
           <asp:Label ID="lblPendingInvoiceVal3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
            <asp:Label ID="lblPendingInvoiceVal4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
             <asp:Label ID="lblPendingInvoiceVal5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
         
         <asp:Label ID="lblTotPendAmount1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
         
         <asp:Label ID="lblTotColAmount1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
         
        <%--<asp:TextBox ID="TextBox1" runat="server" Width="0px"></asp:TextBox>--%>
    </div>
   
    
    
    <%--<div visible="false">
        <asp:HiddenField ID="HidAgent1" runat="server" Value="A"  />
        <asp:HiddenField ID="HidAgent2" runat="server" Value="B"/>
        <asp:HiddenField ID="HidAgent3" runat="server" Value="C"/>
        <asp:HiddenField ID="HidAgent4" runat="server" Value="D"/>
        <asp:HiddenField ID="HidAgent5" runat="server" Value="E"/>
       
        <asp:HiddenField ID="HidTonnage1" runat="server" Value="1" />
        <asp:HiddenField ID="HidTonnage2" runat="server" Value="2"/>
        <asp:HiddenField ID="HidTonnage3" runat="server" Value="3"/>
        <asp:HiddenField ID="HidTonnage4" runat="server" Value="4"/>
        <asp:HiddenField ID="HidTonnage5" runat="server" Value="5"/>
        
    </div>--%>
  </ContentTemplate>
    
</asp:UpdatePanel>    
    
</asp:Content>