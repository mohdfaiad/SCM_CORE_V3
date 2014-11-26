<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomAccountsTab.ascx.cs"
    Inherits="ProjectSmartCargoManager.CustomControls.CustomAccountsTab" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
<link href="../style/style.css" rel="stylesheet" type="text/css" />
<link href="../style/jetGridView.css" rel="stylesheet" type="text/css" />
<style type="text/css">
    .style1
    {
        width: 30%;
    }
    
    .tbcen
		{
		    text-align:center;
		    }
		    
		    .op
		    {
		        z-index:99999;
		        }
		        	.showh
    {
    display:none;
    }
    .highlight{ background:#7eabfe !important; height:30px; color:#fff;
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
                width: 1000,
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
                
//                alert(FightNo.value);
//                var btnPopulate = document.getElementById("<%=GetData.ClientID%>");
//                btnPopulate.click();

            }
        );
        }
        catch (Err) {
            HideGraph();
        }
    }
    
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

    function expandcollapse(obj, row) 
    {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);

        if (div.style.display == "none") 
        {
            div.style.display = "block";
            if (row == 'alt') {
                img.src = "minus.gif";
            }
            else 
            {
                img.src = "minus.gif";
            }
            img.alt = "Close to view other Customers";
        }
        else 
        {
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
    $('#<%= GridView1.ClientID %>').find('tr').click(function callg() 
        {
            if ($(this).hasClass('RowStyle')) 
            {
                $('#<%= GridView1.ClientID %> tr').removeClass('highlight');
                $(this).removeClass('RowStyle');
                $(this).addClass('highlight');
            }
        }
        );
    }
    );
    
</script>

<div id="divLabels" style="width: 0%; background-color: White" runat="server">
    <asp:Button ID="populate" runat="server" value="Populate" class="button" Text=""
        Height="0px" Width="0px" EnableViewState="true" Visible="false" />
    <%--<%--Graph 1--%>
    <asp:Label ID="lblG1Flt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Flt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG1Wgt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <%--Graph 2--%>
    <asp:Label ID="lblG2Flt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Flt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblG2Wgt5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <%--Top Agent Graph1 --%>
    <asp:Label ID="lblAgent1G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent2G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent3G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent4G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent5G1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblTopAgTon5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <%--Bot Agent Graph2 --%>
    <asp:Label ID="lblAgent1G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent2G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent3G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent4G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblAgent5G2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon1" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon2" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon3" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon4" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <asp:Label ID="lblBotAgTon5" runat="server" EnableViewState="true" Width="0px"></asp:Label>
    <%-------------------------------------------------------------------------------------------------%>
</div>
<asp:Panel ID="PanelGraph" runat="server">
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
                <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Font-Size="Medium"
                    Width="630px" Height="300px" AutoPostBack="true" OnActiveTabChanged="OnActiveTab_Changed">
                    <%--OnActiveTabChanged="GetData_Click"--%><%--OnClientActiveTabChanged="CallPopulateClick();"--%>
                    <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Flights" Width="620px" Height="27px">
                        <HeaderTemplate>
                            &nbsp; Flight Tonnage &nbsp;</HeaderTemplate>
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
                    <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Agents" Width="620px" Height="27px">
                        <HeaderTemplate>
                            &nbsp; Revenue &nbsp;
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel2" runat="server" Width="100%">
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
                                                            <asp:TemplateField HeaderText="Ident Row"  ItemStyle-CssClass="showh"  HeaderStyle-CssClass="showh">
                                                                <ItemTemplate><asp:Label ID="lblIdentRow" Text='<%# Eval("IdentRow") %>' runat="server"></asp:Label></ItemTemplate>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField >
                                                            <ItemTemplate>
                                                            <img id="btnviability" runat="server" src="~/Images/search1.png" />
                                                                <asp:HoverMenuExtender ID="HoverMenuExtender1" runat="server" TargetControlID="btnviability"
                                                                 PopupControlID="Panel2" PopupPosition="Top" OffsetX="-180" OffsetY="20" PopDelay="50" HoverCssClass="op" >
                                                                </asp:HoverMenuExtender>
                                                                <asp:Panel ID="Panel2" runat="server">
                                                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="true" CellPadding="3" CellSpacing="3">
                                                                 <HeaderStyle CssClass="titlecolr" />
                                                                        <RowStyle HorizontalAlign="Center"  BackColor="White"/>
                                                                        <AlternatingRowStyle HorizontalAlign="Center" />
                                                                <EmptyDataTemplate>
                                                                <strong>No Data Available</strong>
                                                                </EmptyDataTemplate>
                                                                </asp:GridView>
                                                                </asp:Panel>
                                                            </ItemTemplate>
                                                            </asp:TemplateField>--%>
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
                    <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Import" Width="620px" Height="27px">
                        <HeaderTemplate>
                            &nbsp; Claims &nbsp;
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="PNClaims" runat="server" Width="100%">
                                <div style="width:600px; height:250px; overflow:auto;">
                                    <asp:GridView ID="grdClaims" runat="server" ShowFooter="false" Width="100%" 
                                    AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
                                    AllowPaging="true" PageSize="10" OnPageIndexChanging="grdClaims_PageIndexChanging" 
                                    CellPadding="2" CellSpacing="3">
                                   <Columns>
                                            <asp:TemplateField AccessibleHeaderText="AWBNo" HeaderText="AWBNo">
                                            <ItemTemplate>
                                            <asp:HyperLink ID="hlnkAWBNumber" runat="server" Text='<%#Eval("AWBNo") %>' NavigateUrl='<%# "~/GHA_ConBooking.aspx?command=Edit&AWBNumber=" + Eval("AWBNo")%>'></asp:HyperLink>
                                            </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="AgentCode">
                                            <ItemTemplate>
                                            <asp:Label ID="lblAgentCode" runat="server" Text='<%# Eval("AgentCode") %>'></asp:Label>
                                            </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Name of Applicant">
                                            <ItemTemplate>
                                            <asp:Label ID="lblFullName" runat="server" Text='<%# Eval("FullName") %>' Width="150px"></asp:Label>
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
                                            <asp:Label ID="lblClaimStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
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
                                            <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:HyperLink ID="hlnkEdit" runat="server" Text="Edit" NavigateUrl='<%# "~/frmClaimApplication.aspx?Edit=" + Eval("ClaimID")%>'></asp:HyperLink>
                                            </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                            <ItemTemplate>
                                            <asp:HyperLink ID="hlnkView" runat="server" Text="View" NavigateUrl='<%# "~/frmClaimApplication.aspx?View="+Eval("ClaimID")%>'></asp:HyperLink>
                                            </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
            
                                        <HeaderStyle CssClass="titlecolr"/>
                                        <RowStyle  HorizontalAlign="Center"/>
                                        <AlternatingRowStyle  HorizontalAlign="Center"/>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>
                    <%--<asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Export" Width="620px" ScrollBars="Auto"
                        Height="27px">
                        <HeaderTemplate>
                            &nbsp; Export &nbsp;
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:Panel ID="Panel3" runat="server" Width="100%">
                                <asp:GridView ID="grdexportList" runat="server" ShowFooter="false" Width="80%" AutoGenerateColumns="False"
                                    BorderColor="#BFBEBE" BorderStyle="None" CellPadding="2" CellSpacing="3" PageSize="10"
                                    AllowPaging="True" OnPageIndexChanging="grdexportList_PageIndexChanging">
                                    <Columns>
                                        <asp:TemplateField HeaderText="FltNo" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Loc" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOrigin" runat="server" Text='<%# Eval("Origin") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Dest" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDestination" runat="server" Text='<%# Eval("Destination") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BkdPcs" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBooked" runat="server" Text='<%# Eval("Booked") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="BkdWt" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBookedWt" runat="server" Text='<%# Eval("PcBookedWt") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AccptPcs" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccepted" runat="server" Text='<%# Eval("Accepted") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="AccptWt" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblAcceptedWt" runat="server" Text='<%# Eval("PcAceeptedWt") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ManiPcs" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManifested" runat="server" Text='<%# Eval("Manifested") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="ManiWt" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblManifestedWt" runat="server" Text='<%# Eval("PcManifestedWt") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OffPcs" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOffloadPcs" runat="server" Text='<%# Eval("OffloadPcs") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="OffWt" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblOffloadWt" runat="server" Text='<%# Eval("OffloadWt") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="FltStatus" HeaderStyle-Wrap="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblFltStatus" runat="server" Text='<%# Eval("FltStatus") %>' />
                                            </ItemTemplate>
                                            <HeaderStyle Wrap="True"></HeaderStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="titlecolr" />
                                    <RowStyle HorizontalAlign="Center" />
                                    <AlternatingRowStyle HorizontalAlign="Center" />
                                </asp:GridView>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:TabPanel>--%>
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
                            <asp:HiddenField ID="hdFlightNo" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
