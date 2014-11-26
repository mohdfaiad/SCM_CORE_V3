<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomAgentTab.ascx.cs" Inherits="ProjectSmartCargoManager.CustomAgentTab" %>
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
    <script type="text/javascript" src="scripts/myScript.js"></script>
    <script type="text/javascript" src="scripts/jqplot.highlighter.js"></script>

    <link rel="stylesheet" type="text/css" href="style/jquery.jqplot.css" />

<script language="javascript" type="text/javascript">

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
            width:2000,
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
                    max: slots_max+20,
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
          ID="pnlFlightDashboard" runat="server" Width="100%" ScrollBars="Auto">
    
      <table width = "100%">
    <tr align = "center">
    <td align = "center">
    <div id="chartdiv" style="width:2000px">
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
            
           <asp:TabPanel ID="TabPanel4" runat="server" HeaderText="Export" Width="620px" ScrollBars="Auto"
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
    </td>
    </tr>
    </table>
      </td>
      </tr>
      </table>
    </asp:Panel>
    