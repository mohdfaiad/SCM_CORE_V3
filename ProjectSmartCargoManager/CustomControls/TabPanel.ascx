<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabPanel.ascx.cs" Inherits="ProjectSmartCargoManager.TabPanel" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
 <style type="text/css">
        .style1
        {
            width: 30%;
        }
    </style>
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

    <link rel="stylesheet" type="text/css" href="style/jquery.jqplot.css" />
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
        var MaxVal = (FirstDigit + 1) * tenPowerInt;
        //alert(MaxVal);
        return MaxVal;
    }

    function RetMaximum(Value1, Value2, Value3, Value4, Value5) {
        var Maximum = Math.max(Value1, Math.max(Value2, Math.max(Value3, Math.max(Value4, Value5))));
        return Maximum;
    }



    function Final() {
        //$($get("populate")).click(function()
        //            Swapnil

        Clear();

        //  Graph 1
        //alert("Graph 1 Data");
        var Flight1 = $('#<%=lblG1Flt1.ClientID%>').html();

        var Tonnage1 = $('#<%=lblG1Wgt1.ClientID%>').html();

        var Flight2 = $('#<%=lblG1Flt2.ClientID%>').html();

        var Tonnage2 = $('#<%=lblG1Wgt2.ClientID%>').html();

        var Flight3 = $('#<%=lblG1Flt3.ClientID%>').html();
        //alert(Flight3);

        var Tonnage3 = $('#<%=lblG1Wgt3.ClientID%>').html();

        var Flight4 = $('#<%=lblG1Flt4.ClientID%>').html();

        var Tonnage4 = $('#<%=lblG1Wgt4.ClientID%>').html();

        var Flight5 = $('#<%=lblG1Flt5.ClientID%>').html();

        var Tonnage5 = $('#<%=lblG1Wgt5.ClientID%>').html();
        //alert(Tonnage5);
        //  Graph 2
        //alert("Graph 2 Data");
        var Flight6 = $('#<%=lblG2Flt1.ClientID%>').html();

        var Tonnage6 = $('#<%=lblG2Wgt1.ClientID%>').html();

        var Flight7 = $('#<%=lblG2Flt2.ClientID%>').html();

        var Tonnage7 = $('#<%=lblG2Wgt2.ClientID%>').html();

        var Flight8 = $('#<%=lblG2Flt3.ClientID%>').html();

        var Tonnage8 = $('#<%=lblG2Wgt3.ClientID%>').html();

        var Flight9 = $('#<%=lblG2Flt4.ClientID%>').html();

        var Tonnage9 = $('#<%=lblG2Wgt4.ClientID%>').html();

        var Flight10 = $('#<%=lblG2Flt5.ClientID%>').html();

        var Tonnage10 = $('#<%=lblG2Wgt5.ClientID%>').html();
        //alert(Tonnage10);
        //alert("Graph 2 finish");

        var TopAgent1 = $('#<%=lblAgent1G1.ClientID%>').html();

        var TopAgTonnage1 = $('#<%=lblTopAgTon1.ClientID%>').html();

        var TopAgent2 = $('#<%=lblAgent2G1.ClientID%>').html();

        var TopAgTonnage2 = $('#<%=lblTopAgTon2.ClientID%>').html();

        var TopAgent3 = $('#<%=lblAgent3G1.ClientID%>').html();
        //alert(Flight3);

        var TopAgTonnage3 = $('#<%=lblTopAgTon3.ClientID%>').html();

        var TopAgent4 = $('#<%=lblAgent4G1.ClientID%>').html();

        var TopAgTonnage4 = $('#<%=lblTopAgTon4.ClientID%>').html();

        var TopAgent5 = $('#<%=lblAgent5G1.ClientID%>').html();

        var TopAgTonnage5 = $('#<%=lblTopAgTon5.ClientID%>').html();

        //------

        var BotAgent1 = $('#<%=lblAgent1G2.ClientID%>').html();

        var BotAgTonnage1 = $('#<%=lblBotAgTon1.ClientID%>').html();

        var BotAgent2 = $('#<%=lblAgent2G2.ClientID%>').html();

        var BotAgTonnage2 = $('#<%=lblBotAgTon2.ClientID%>').html();

        var BotAgent3 = $('#<%=lblAgent3G2.ClientID%>').html();
        //alert(Flight3);

        var BotAgTonnage3 = $('#<%=lblBotAgTon3.ClientID%>').html();

        var BotAgent4 = $('#<%=lblAgent4G2.ClientID%>').html();

        var BotAgTonnage4 = $('#<%=lblBotAgTon4.ClientID%>').html();

        var BotAgent5 = $('#<%=lblAgent5G2.ClientID%>').html();

        var BotAgTonnage5 = $('#<%=lblBotAgTon5.ClientID%>').html();


        //----------------------------------------------------------------------

        var s1 = [Tonnage1, Tonnage2, Tonnage3, Tonnage4, Tonnage5];
        //alert(s1);

        var ticks = [Flight1, Flight2, Flight3, Flight4, Flight5];
        //alert(ticks);
        var plot1 = $.jqplot('chartdiv', [s1], {
            title: 'Top 5 Flights',

            //            axes: { yaxis: { min: 0} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                //color: '#5FAB78',
                color: '#f7811f',
                pointLabels: { show: true }
            },

            axes: {
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks
                },
                yaxis: {
                    pad: 1.05,
                    min: 0,
                    max: parseInt(RetMaxVal(Tonnage1)),
                    //max: parseInt(Tonnage1),
                    tickOptions: { formatString: '%d' }
                }
            },
            series: [
            { label: 'X:Flights<br />Y:Tonnage (Kg)' }
                ],
            legend: {
                show: true,
                placement: 'insideGrid',
                showSwatches: false,
                showLabels: true
            }
        });
        //alert("Graph 1 Drow");
        //---------------------------------------------------------
        //var s2 = [100, 200,300, 400,500];
        //alert("Graph 2 Tonnage");
        var s2 = [Tonnage6, Tonnage7, Tonnage8, Tonnage9, Tonnage10];

        //alert(s2);
        //            alert(ctonage1);
        //            alert(ctonage2);
        //            alert(ctonage3);
        //            alert(ctonage4);
        //            alert(ctonage5);
        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        //alert("Graph 2 Flights");
        var ticks1 = [Flight6, Flight7, Flight8, Flight9, Flight10];

        //alert(ticks1);
        var plot2 = $.jqplot('chartdiv1', [s2], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            title: 'Low 5 Flights',
            //            axes: { yaxis: { min: 0} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                //color: '#5FAB78',
                color: '#f7811f',
                pointLabels: { show: true }
            },

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
                    //max: parseInt(RetMaxVal(Tonnage10)),
                    max: parseInt(RetMaxVal(RetMaximum(Tonnage6, Tonnage7, Tonnage8, Tonnage9, Tonnage10))),
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
            }
        });
        //alert("Graph 2 drow");
        //----------------------------------------------------------------------

        var s3 = [TopAgTonnage1, TopAgTonnage2, TopAgTonnage3, TopAgTonnage4, TopAgTonnage5];

        //alert(s2);
        //            alert(ctonage1);
        //            alert(ctonage2);
        //            alert(ctonage3);
        //            alert(ctonage4);
        //            alert(ctonage5);
        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        //alert("Graph 2 Flights");
        var ticks3 = [TopAgent1, TopAgent2, TopAgent3, TopAgent4, TopAgent5];

        //alert(ticks1);
        var plot3 = $.jqplot('chartdiv2', [s3], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            title: 'Top 5 Agents',
            //            axes: { yaxis: { min: 0} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                //color: '#5FAB78',
                color: '#f7811f',
                pointLabels: { show: true }
            },

            axes: {
                // Use a category axis on the x axis and use our custom ticks.
                xaxis: {
                    renderer: $.jqplot.CategoryAxisRenderer,
                    ticks: ticks3
                },
                // Pad the y axis just a little so bars can get close to, but
                // not touch, the grid boundaries.  1.2 is the default padding.
                yaxis: {
                    pad: 1.05,
                    min: 0,
                    //max: parseInt(RetMaxVal(Tonnage10)),
                    max: parseInt(RetMaxVal(RetMaximum(TopAgTonnage1, TopAgTonnage2, TopAgTonnage3, TopAgTonnage4, TopAgTonnage5))),
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
            }
        });
        //-----------------------------------------------------------------------

        var s4 = [BotAgTonnage1, BotAgTonnage2, BotAgTonnage3, BotAgTonnage4, BotAgTonnage5];

        //alert(s2);
        //            alert(ctonage1);
        //            alert(ctonage2);
        //            alert(ctonage3);
        //            alert(ctonage4);
        //            alert(ctonage5);
        // Can specify a custom tick Array.
        // Ticks should match up one for each y value (category) in the series.
        //alert("Graph 2 Flights");
        var ticks4 = [BotAgent1, BotAgent2, BotAgent3, BotAgent4, BotAgent5];

        //alert(ticks1);
        var plot4 = $.jqplot('chartdiv3', [s4], {
            // The "seriesDefaults" option is an options object that will
            // be applied to all series in the chart.
            title: 'Bottom 5 Agents',
            //            axes: { yaxis: { min: 0} },
            seriesDefaults: {
                renderer: $.jqplot.BarRenderer,
                rendererOptions: { fillToZero: true },
                //color: '#5FAB78',
                color: '#f7811f',
                pointLabels: { show: true }
            },

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
                    //max: parseInt(RetMaxVal(Tonnage10)),
                    max: parseInt(RetMaxVal(RetMaximum(BotAgTonnage1, BotAgTonnage2, BotAgTonnage3, BotAgTonnage4, BotAgTonnage5))),
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
            }
        });

        //=----------------------------------------------------------------------
        $('#<%=divLabels.ClientID %>').hide();
    }
    
    </script>

    

      <div id = "divLabels" style="width:0%;background-color:White" runat = "server" >
        
        <asp:Button id="populate" runat="server" value = "Populate" class = "button" Text = "" Height = "0px" Width = "0px" EnableViewState = "true" Visible = "false" />
        
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
     <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Font-Size="Medium" 
            Width="650px" Height="300px" AutoPostBack="true" 
              OnActiveTabChanged="OnActiveTab_Changed">
            <%--OnActiveTabChanged="GetData_Click"--%><%--OnClientActiveTabChanged="CallPopulateClick();"--%>
            <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Flights" Width="620px"
                Height="27px"><HeaderTemplate>
              &nbsp; Flights
&nbsp;</HeaderTemplate>
                
                <ContentTemplate>
      <asp:Panel 
          ID="pnlFlightDashboard" runat="server" Width="100%">
    
      <table width = "100%">
    <tr align = "center">
    <td align = "center">
    <div id="chartdiv">
    </div>
    </td>
    <td align = "center">
    <div id="chartdiv1"></div>
    </td>
    </tr>
    
    </table>
    
      </asp:Panel>
</ContentTemplate>
</asp:TabPanel>

           <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Agents" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    &nbsp; Agents
&nbsp; </HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel2" runat="server" Width="100%">
    
      <table width = "100%">
    <tr align = "center">
    <td align = "center">
    <div id="chartdiv2">
    </div>
    </td>
    <td align = "center">
    <div id="chartdiv3"></div>
    </td>
    </tr>
    
    </table>
    
      <%--<table style="width:100%">
    <tr align = "center">
    <td align = "right">
        Location :
    </td>
    <td align = "left">
        <asp:DropDownList ID="ddlLocationAg" runat="server" AppendDataBoundItems="True">
        </asp:DropDownList>
    </td>
    <td align = "right">
        From Date :
    </td>
    <td align = "left">
        <asp:TextBox ID="txtFrmDtAg" runat="server" Width="100px"></asp:TextBox>
         <asp:CalendarExtender ID="CalendarExtender1" runat="server" 
          Enabled="True" TargetControlID="txtFrmDtAg" Format="dd/MM/yyyy" >
         </asp:CalendarExtender>
    </td>
    <td align = "right">
        To Date :
    </td>
    <td align = "left">
        <asp:TextBox ID="txtToDtAg" runat="server" Width="100px"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender2" runat="server" 
          Enabled="True" TargetControlID="txtToDtAg" Format="dd/MM/yyyy" >
        </asp:CalendarExtender>
    </td>
    <td>
        <asp:Button ID="btnGetDataAg" runat="server" Text="Populate Data" class = "button" 
            onclick="GetData_Click"/>
    </td>
    </tr>
    </table>--%></asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
           
           <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Import" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    &nbsp; Import
&nbsp; </HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="PNImport" runat="server" Width="100%">
    
      <asp:GridView ID="grdImportList" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
         OnPageIndexChanging="grdImportList_PageIndexChanging">
           
            <Columns>
             <asp:TemplateField HeaderText="FltNo" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFlightNo" runat="server" Text = '<%# Eval("FlightNo") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOrigin" runat="server" Text = '<%# Eval("Origin") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Loc" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDestination" runat="server" Text = '<%# Eval("Destination") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
             
             <asp:TemplateField HeaderText="DeptPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDeparted" runat="server" Text = '<%# Eval("Departed") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="DeptWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDepartedWt" runat="server" Text = '<%# Eval("PcDWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="OffPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadPcs" runat="server" Text = '<%# Eval("OffloadPcs") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
             <asp:TemplateField HeaderText="OffWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadWt" runat="server" Text = '<%# Eval("OffloadWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="ArrPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblArrived" runat="server" Text = '<%# Eval("Arrival") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="ArrWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblArrivedWt" runat="server" Text = '<%# Eval("PcArWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="DelPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDelivered" runat="server" Text = '<%# Eval("Delivered") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="DelWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDeliveredWt" runat="server" Text = '<%# Eval("PcDlWeight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="FltStatus" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFltStatus" runat="server" Text = '<%# Eval("FltStatus") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                      
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
            
           <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Export" Width="620px"
                Height="27px">
                <HeaderTemplate>
                    &nbsp; Export
&nbsp; </HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel 
          ID="Panel3" runat="server" Width="100%">
    
      <asp:GridView ID="grdexportList" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
         OnPageIndexChanging="grdexportList_PageIndexChanging">
           
            <Columns>
             <asp:TemplateField HeaderText="FltNo" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblFlightNo" runat="server" Text = '<%# Eval("FlightNo") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Loc" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblOrigin" runat="server" Text = '<%# Eval("Origin") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
                         
             <asp:TemplateField HeaderText="Dest" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDestination" runat="server" Text = '<%# Eval("Destination") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
             
             <asp:TemplateField HeaderText="BkdPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBooked" runat="server" Text = '<%# Eval("Booked") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="BkdWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblBookedWt" runat="server" Text = '<%# Eval("PcBookedWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="AccptPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblAccepted" runat="server" Text = '<%# Eval("Accepted") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             
             <asp:TemplateField HeaderText="AccptWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblAcceptedWt" runat="server" Text = '<%# Eval("PcAceeptedWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="ManiPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblManifested" runat="server" Text = '<%# Eval("Manifested") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="ManiWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblManifestedWt" runat="server" Text = '<%# Eval("PcManifestedWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="OffPcs" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadPcs" runat="server" Text = '<%# Eval("OffloadPcs") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
              <asp:TemplateField HeaderText="OffWt" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOffloadWt" runat="server" Text = '<%# Eval("OffloadWt") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="FltStatus" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblFltStatus" runat="server" Text = '<%# Eval("FltStatus") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                      
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    
      
</asp:Panel>
 
                </ContentTemplate>
                </asp:TabPanel>
                
      </asp:TabContainer>
      </td>
      </tr>
      <tr>
      <td>
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
          Enabled="True" TargetControlID="txtFrmDate" Format="dd/MM/yyyy" >
         </asp:CalendarExtender>
    </td>
    <td align = "right">
        To Date :
        </td>
    <td align = "left">
        <asp:TextBox ID="txtToDate" runat="server" Width="100px"></asp:TextBox>
        <asp:CalendarExtender ID="CalendarExtender4" runat="server" 
          Enabled="True" TargetControlID="txtToDate" Format="dd/MM/yyyy" >
        </asp:CalendarExtender>
    </td>
    <td>
        <asp:Button ID="GetData" runat="server" Text="Populate Data" class = "button" OnClick="GetData_Click" 
            />
    </td>
    </tr>
    </table>
      </td>
      </tr>
      </table>
    </asp:Panel>
    