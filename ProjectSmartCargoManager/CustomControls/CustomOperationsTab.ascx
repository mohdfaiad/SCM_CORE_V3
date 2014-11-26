<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomOperationsTab.ascx.cs"
    Inherits="ProjectSmartCargoManager.CustomOperationsTab" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
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

    function GeneratePlotForIncoming(JS) {

        //succes - data loaded, now use plot:
        var JSONString = JSON.parse(JS);
        var plotarea = $("#ChartIncomingFlight");
        var dataBar = JSONString[0].FlightNo;
        var dataLine = JSONString[0].FlightNo;
        var line1 = [];
        var line2 = [];
        var line3 = [];

        for (i = 0; i < JSONString.length; i++) {
            line1[i] = JSONString[i].FlightNo;
            line2[i] = JSONString[i].Wt;
            line3[i] = JSONString[i].FlightDate;

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

        var plot2 = $.jqplot('ChartIncomingFlight', [line2], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            width: 1000,
            title: 'Flight',
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
                show: true,
                sizeAdjust: 3,
                tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                    return line2[pointIndex];

                }

            }

        });
    }


    function GeneratePlot(JS) {

        //succes - data loaded, now use plot:
        var JSONString = JSON.parse(JS);
        var plotarea = $("#chartdiv");
        var dataBar = JSONString[0].FlightNo;
        var dataLine = JSONString[0].FlightNo;
        var line1 = [];
        var line2 = [];
        var line3 = [];

        for (i = 0; i < JSONString.length; i++) {
            line1[i] = JSONString[i].FlightNo;
            line2[i] = JSONString[i].Wt;
            line3[i] = JSONString[i].FlightDate;

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
            width: 1000,
            title: 'Agent',
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
                show: true,
                sizeAdjust: 3,
                tooltipContentEditor: function(str, seriesIndex, pointIndex, jqPlot) {

                    return line2[pointIndex];

                }

            }

        });
    }
    

</script>

<div id="divLabels" style="width: 0%; background-color: White" runat="server">
    <asp:Button ID="populate" runat="server" value="Populate" class="button" Text=""
        Height="0px" Width="0px" EnableViewState="true" Visible="false" />
    <%--<%--Graph 1--%>
    <%--<asp:Label ID="lblG1Flt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>--%>
    <%--Graph 2--%>
    <%--<asp:Label ID="lblG2Flt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>--%>
    <%--Top Agent Graph1 --%>
    <%--<asp:Label ID="lblAgent1G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent2G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent3G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent4G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent5G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon5" runat="server" EnableViewState="true" Width="0px"></asp:Label>--%>
    <%--Bot Agent Graph2 --%>
    <%--<asp:Label ID="lblAgent1G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent2G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent3G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent4G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent5G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon5" runat="server" EnableViewState="true" Width="0px"></asp:Label>--%>
    <%-------------------------------------------------------------------------------------------------%>
</div>
<asp:Panel ID="PanelGraph" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Font-Size="Medium"
                    Width="650px" Height="300px" AutoPostBack="true" OnActiveTabChanged="OnActiveTab_Changed">
                    <%--OnActiveTabChanged="GetData_Click"--%><%--OnClientActiveTabChanged="CallPopulateClick();"--%>
                    <asp:TabPanel ID="tbOutFlight" runat="server" HeaderText="Flights" Width="620px" Height="27px">
                        <HeaderTemplate>
                            Out Flights</HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="pnlFlightDashboard" runat="server" Width="100%" ScrollBars="Auto">
                                <table width="100%">
                                    <tr align="center">
                                        <td align="center">
                                            <div id="chartdiv" style="width: 1000px">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="tbOutAWBS" runat="server" HeaderText="Pending AWB" Width="620px" Height="27px">
                        <HeaderTemplate>
                            Out AWB's
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel5" runat="server" Width="100%">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            Flight#
                                            <asp:DropDownList ID="ddlFlightNo" runat="server">
                                            </asp:DropDownList>
                                            <asp:TextBox ID="txtFltDate" runat="server" Width="80px"></asp:TextBox>
                                             <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
                                              Enabled="True" TargetControlID="txtFltDate" Format="dd/MM/yyyy" PopupButtonID="imgFltDate" >
                                             </asp:CalendarExtender>
                                              <asp:ImageButton ID="imgFltDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                                            <asp:Button ID="btnFlightList" Text="List" runat="server" class = "button" OnClick="GetDataPerFlight_Click"/>
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
                                                                    <%--<asp:Label ID="lblAWBno" Text='<%# Eval("AWBNo") %>' runat="server"></asp:Label>--%>
                                                                    <asp:HyperLink ID="hlnkAWBNumber" runat="server" Width="100px" Text='<%# Eval("AWBNo") %>' NavigateUrl='<%# "~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + Eval("AWBNo") %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Product Type">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblProductType" Text='<%# Eval("ProductType") %>' runat="server"></asp:Label>
                                                                </ItemTemplate> 
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Priority">
                                                                <ItemTemplate> <asp:Label ID="lblPriority" Text='<%# Eval("Priority") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Pcs" >
                                                                <ItemTemplate><asp:Label ID="lblAccPcs" Text='<%# Eval("AcceptedPcs") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Weight" >
                                                                <ItemTemplate><asp:Label ID="lblWeight" Text='<%# Eval("Weight") %>' runat="server"></asp:Label></ItemTemplate>
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
                                                            <asp:TemplateField HeaderText="AWB<br/>Status">
                                                                <ItemTemplate><asp:Label ID="lblAWBStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Accp.<br/>Status">
                                                                <ItemTemplate>
                                                                    <%--<asp:Label ID="lblStatus" Text='<%# Eval("AccpStatus") %>' runat="server"></asp:Label>--%>
                                                                    <asp:HyperLink ID="lblStatus" runat="server" Width="100px" Text='<%# Eval("AccpStatus") %>' NavigateUrl='<%# Eval("AccpURL") %>'></asp:HyperLink>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Revenue" Visible="false">
                                                                <ItemTemplate><asp:Label ID="lblrevenue" Text='<%# Eval("Total") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Cost" Visible="false">
                                                                <ItemTemplate><asp:Label ID="lblCost" Text='<%# Eval("Cost") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                             <asp:TemplateField HeaderText="Profit" Visible="false">
                                                                <ItemTemplate><asp:Label ID="lblProfit" Text='<%# Eval("Profit") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>   
                                                            <asp:TemplateField HeaderText="">
                                                                <ItemTemplate><asp:Button ID="btnConfirm" Text='Confirm' OnClick="ConfirmShipment" runat="server"/></ItemTemplate>
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
                    <asp:TabPanel ID="tbExportInventory" runat="server" HeaderText="Export WH" Width="620px"
                        ScrollBars="Auto" Height="27px">
                        <HeaderTemplate>
                            Export Inventory
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server" Width="100%">
                                <div style="overflow: auto; height: 300px">
                                    <asp:GridView ID="grdexportWareHouseList" runat="server" ShowFooter="false" Width="80%"
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
                                            <asp:TemplateField HeaderText="ProductType" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriority" runat="server" Text='<%# Eval("ProductDescription") %>' />
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
                                                    <asp:Label ID="lblBookedWt" Width="400px" runat="server" Text='<%# Eval("CommodityDesc") %>' />
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
                    <asp:TabPanel ID="tbInComingFlight" runat="server" HeaderText="InComing Flights"
                        Width="620px" ScrollBars="Auto" Height="27px">
                        <HeaderTemplate>
                            InComing Flights
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel4" runat="server" Width="100%" ScrollBars="Auto">
                                <table width="100%">
                                    <tr align="center">
                                        <td align="center">
                                            <div id="ChartIncomingFlight" style="width: 1000px">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <asp:TabPanel ID="tbImportInventory" runat="server" HeaderText="Export WH" Width="620px"
                        ScrollBars="Auto" Height="27px">
                        <HeaderTemplate>
                            Import Inventory
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel1" runat="server" Width="100%">
                                <div style="overflow: auto; height: 300px">
                                    <asp:GridView ID="grdImportWarehouse" runat="server" ShowFooter="false" Width="80%"
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
                                            <asp:TemplateField HeaderText="Product Type" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPriority" Width="200px" runat="server" Text='<%# Eval("CommodityDesc") %>' />
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
                                                    <asp:Label ID="lblBookedWt" Width="400px" runat="server" Text='<%# Eval("CommodityDesc") %>' />
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
                    <asp:TabPanel ID="tbOutGoingFlight" runat="server" HeaderText="Outgoing Flights"
                        Width="620px" ScrollBars="Auto" Height="27px" Visible="false">
                        <HeaderTemplate>
                            Outgoing Flights
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" Width="100%">
                                <div style="overflow: auto; height: 300px">
                                    <asp:GridView ID="grdOutGoingFlight" runat="server" ShowFooter="false" Width="80%"
                                        AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2"
                                        CellSpacing="3" PageSize="10" AllowPaging="false">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Flt #" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <%--<asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' />--%>
                                                    <asp:HyperLink ID="hlnkFlightNo" runat="server" Text='<%#Eval("FlightNo") %>' NavigateUrl='<%# Eval("URL")%>'></asp:HyperLink>
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Flt Dt." HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblFlightDt" runat="server" Text='<%# Eval("FlightDt","{0:dd/MM/yyyy}") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dest." HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBNo" runat="server" Text='<%# Eval("Destination")%>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Dep. Time" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBooked" runat="server" Text='<%# Eval("DepartureTime") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle Wrap="True"></HeaderStyle>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Arr. Time" HeaderStyle-Wrap="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("ArrivalTime") %>' />
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
                <table style="width: 620px">
                    <tr align="center">
                        <td align="right">
                            Location :
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlLocation" runat="server" AppendDataBoundItems="True">
                            </asp:DropDownList>
                        </td>
                        <td align="right">
                            From Date :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtFrmDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtFrmDate"
                                Format="dd/MM/yyyy" PopupButtonID="ImageButton1">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                        <td align="right">
                            To Date :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
                            <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtToDate"
                                Format="dd/MM/yyyy" PopupButtonID="imgFltToDt">
                            </asp:CalendarExtender>
                            <asp:ImageButton ID="imgFltToDt" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                        </td>
                        <td>
                            <asp:Button ID="GetData" runat="server" Text="Populate Data" class="button" OnClick="GetData_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
