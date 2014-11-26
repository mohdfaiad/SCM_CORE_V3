<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomITAdminTab.ascx.cs" Inherits="ProjectSmartCargoManager.CustomControls.CustomITAdminTab" %>

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
            width:1000,
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
      <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
      
     <asp:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Font-Size="Medium" 
            Width="650px" Height="300px" AutoPostBack="true" 
              OnActiveTabChanged="OnActiveTab_Changed">
            <%--OnActiveTabChanged="GetData_Click"--%><%--OnClientActiveTabChanged="CallPopulateClick();"--%>
            
            
   <asp:TabPanel ID="TabPanel1" runat="server" HeaderText="Flights" Width="620px" Height="27px">
         <HeaderTemplate>&nbsp;Internal Users&nbsp;</HeaderTemplate>
                
     <ContentTemplate>
                
      <asp:Panel ID="PnlInternal" runat="server" Width="100%" ScrollBars="Auto">
    
    <asp:GridView ID="grdInternal" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" OnPageIndexChanging="grdInternal_PageIndexChanging">
           
            <Columns>
             <asp:TemplateField HeaderText="UserName" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblUserName" runat="server" Text = '<%# Eval("UserId") %>'/>--%>
                        <asp:HyperLink ID="HyperLink1" runat="server" Text = '<%# Eval("UserId") %>' NavigateUrl='<%#Eval("userURL")%>'></asp:HyperLink>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Location" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text = '<%# Eval("Location") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="LogedIn" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblLogedIn" runat="server" Text = '<%# Eval("LoginTime") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="IPaddress" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblIPaddress" runat="server" Text = '<%# Eval("IPaddress") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text = '<%# Eval("Status") %>'/>
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


   <asp:TabPanel ID="TabPanel2" runat="server" HeaderText="Agents" Width="620px" Height="27px">
                <HeaderTemplate>
                    &nbsp; Agents/External Users&nbsp; 
               </HeaderTemplate>
                
                <ContentTemplate>
      <asp:Panel ID="PnlExternal" runat="server" Width="100%">
      
    <asp:GridView ID="grdExternal" runat="server" ShowFooter="false" Width="80%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" >
           
            <Columns>
             <asp:TemplateField HeaderText="UserName" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblUserName" runat="server" Text = '<%# Eval("UserId") %>'/>--%>
                        <asp:HyperLink ID="HyperLink2" runat="server" Text = '<%# Eval("UserId") %>' NavigateUrl='<%#Eval("userURL")%>'></asp:HyperLink>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Location" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblLocation" runat="server" Text = '<%# Eval("Location") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="LogedIn" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblLogedIn" runat="server" Text = '<%# Eval("LoginTime") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="IPaddress" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblIPaddress" runat="server" Text = '<%# Eval("IPaddress") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text = '<%# Eval("Status") %>'/>
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
                
           
   <asp:TabPanel ID="TabPanel3" runat="server" HeaderText="Import" Width="620px" Height="27px">
                <HeaderTemplate>  &nbsp; Report Audit Log &nbsp; </HeaderTemplate>
                
                <ContentTemplate>
            <asp:Panel ID="PnlRptAudit" runat="server" Width="100%">
    <div style="height:300px">
      <asp:GridView ID="grdRptAuditLog" runat="server" ShowFooter="false" Width="100%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" OnPageIndexChanging="grdRptAuditLog_PageIndexChanging"> 
           
            <Columns>
             <asp:TemplateField HeaderText="User Name" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblUserName" runat="server" Text = '<%# Eval("UserName") %>' NavigateUrl='<%#Eval("userURL")%>'/>--%>
                        <asp:HyperLink ID="HyperLink3" runat="server" Text = '<%# Eval("UserName") %>' NavigateUrl='<%#Eval("userURL")%>'></asp:HyperLink>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="IP Address" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblIP" runat="server" Text = '<%# Eval("IP") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
                         
             <asp:TemplateField HeaderText="Page Name" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblPage" runat="server" Text = '<%# Eval("Page") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Access Time" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblAccessTime" runat="server" Text = '<%# Eval("AccessTime") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Station" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStation" runat="server" Text = '<%# Eval("Station") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
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
                </asp:TabPanel>--%>
                
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