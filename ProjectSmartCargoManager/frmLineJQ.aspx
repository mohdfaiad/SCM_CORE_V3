<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmLineJQ.aspx.cs" Inherits="ProjectSmartCargoManager.frmLineJQ" MasterPageFile ="~/SmartCargoMaster.Master" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
<script type="text/javascript" src="scripts/jqplot.highlighter.js"></script>
<script type="text/javascript" src="scripts/jqplot.highlighter.min.js"></script>
<script type="text/javascript" src="scripts/jqplot.dateAxisRenderer.js"></script>
<script type="text/javascript" src="scripts/jqplot.cursor.js"></script>
<link rel="stylesheet" type="text/css" href="style/jquery.jqplot.css" />
    <style type="text/css">
        .style1
        {
            height: 19px;
        }
    </style>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

<script type="text/javascript">

    function HideDiv() {

        $('#<%=divLabels.ClientID %>').hide();        

    }

    function validation() { 
    var SelectedFN1 = $("#<%=ddlFortnight1.ClientID%> option:selected").text();
    var SelectedFN2 = $("#<%=ddlFortnight2.ClientID%> option:selected").text();
        if (SelectedFN1 != SelectedFN2) {
            alert("Fortnight Options Must Be Same !!");
            return false;
        }
        else {
            return true;
        }

    }

    function SplitArray() {
       // alert("In");
        $.jqplot.config.enablePlugins = true;
        var Production1 = $('#<%=lblMonth1.ClientID%>').html(); // "1000,1200,1300,1400,2500,1000,1200,1300,1400,2500";
       // alert("Prod1 "+Production1);
        var Production2 = $('#<%=lblMonth2.ClientID%>').html(); //"2000,2200,2300,2400,3500,2000,2200,2300,2400,3500";
       // alert("Prod2 " + Production2);
        var Tick1 = $('#<%=lblTicks1.ClientID%>').html(); //[1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 30, 31];
        var Tick2 = $('#<%=lblTicks2.ClientID%>').html();
        var SelectedMonth1 = $("#<%=ddlMonth1.ClientID%> option:selected").text();  //document.getElementById('ddlMonth1').options[document.getElementById('ddlMonth1').selectedIndex].text;
        //alert(SelectedMonth1);
        var SelectedMonth2 = $("#<%=ddlMonth2.ClientID%> option:selected").text();  //document.getElementById('ddlMonth2').options[document.getElementById('ddlMonth2').selectedIndex].text;
        //alert(SelectedMonth2);
        // var Date1 = "'2012-07-9','2012-07-10','2012-07-11','2012-07-12','2012-07-13','2012-07-14','2012-07-15','2012-07-16','2012-07-17','2012-07-18'"
        
        var ProductionArr1 = Production1.split(",");
        var ProductionArr2 = Production2.split(",");
        var TicksArr1 = Tick1.split(",");
        var TicksArr2 = Tick2.split(",");
        //alert("1");
        var Graph1 = [];
        var Graph2 = [];
//        alert(TicksArr1[0]);
//        alert(TicksArr2[0]);
        
        var index1 = 0,index2 = 0;
        for (var i = parseInt(TicksArr1[0]); i < parseInt(TicksArr1[0]) + parseInt(TicksArr1.length); i += 1) {
            Graph1.push([i, ProductionArr1[index1]]);
           // alert(i+","+ProductionArr1[index1]);
            index1 = index1 + 1;
            
        }

        for (var i = parseInt(TicksArr2[0]); i < parseInt(TicksArr2[0]) + parseInt(TicksArr2.length); i += 1) {
            Graph2.push([i, ProductionArr2[index2]]);
           // alert(i+","+ProductionArr2[index2]);
            index2 = index2 + 1;
        }

        var chSeries = [{
        color: '#436277'}];

//        alert(Graph1);
//        alert(Graph2);

        var plot2 = $.jqplot('divProduction', [Graph1, Graph2], {
            // Give the plot a title.
            title: 'Production Everyday',
            // You can specify options for all axes on the plot at once with
            // the axesDefaults object.  Here, we're using a canvas renderer
            // to draw the axis label which allows rotated text.
            highlighter: {
                show: true,
                sizeAdjust: 7.5
            },
            seriesDefaults: {
                lineWidth: 3,
                rendererOptions: {
                    smooth: true
                },
                pointLabels: { show: true }
            },
            series: chSeries,
            series: [
          {// Change our line width and use a diamond shaped marker.
              lineWidth: 2,
              markerOptions: { size: 7, style: 'dimaond' },
              label: SelectedMonth1//"Month1"
          },
          {
              // Don't show a line, just show markers.
              // Make the markers 7 pixels with an 'x' style
              lineWidth: 4,
              showLine: true,
              markerOptions: { size: 7, style: 'dimaond' },
              label: SelectedMonth2 //"Month2"
}],
            axes: {
                xaxis: {
                    min: 0,
                    renderer: $.jqplot.CategoryAxisRenderer
                    //                ticks: Ticks,

                    //                showTickMarks : true
                },
                yaxis:
                 {
                     min: 0,
                     max: parseInt(Math.max(Math.max.apply(Math, ProductionArr1),Math.max.apply(Math, ProductionArr2))) * 1.3 //45000
                     //renderer: $.jqplot.CategoryAxisRenderer
           
//                tickOptions: {
//                    formatString: '%.2f',
//                    textColor: 'black',
//                    fontSize: '11px'
//                    }
                }
            },
            legend: {
                show: true,
                rendererOptions: {
                    placement: "outsideGrid"
                },
                location: 'e'
            },
            cursor: {
                show: true,
                zoom: true,
                showTooltip: false
            },
            grid: {
                background: 'lightblue',
                borderColor: 'white',
                shadow: false,
                gridLineColor: '#999999'
            }
        });

            $('#<%=divLabels.ClientID %>').hide();        
//        for (var i = 0; i < 5; i++) {
//            alert(NumArray[i]);
//        }
    }

    $(document).ready(function() {
        // SplitArray();
        $('#<%=divLabels.ClientID %>').hide();        
});

function RetMaxVal(Values) {
    var Tonlength = (Values + "").length;
    //alert(Tonlength);
    var FirstDigit = parseInt((Values + "")[0]);
    //alert(FirstDigit);
    var tenPowerInt = Math.pow(10, parseInt(Tonlength) - 1);
    //alert(tenPowerInt);
    var MaxVal = (FirstDigit + 2) * tenPowerInt;
    //alert(MaxVal);
    return MaxVal;
}
</script>    

    
<div id="contentarea">
<h1><img src="Images/txt_campargraph.png" /></h1>
<div class="botline">
<table width = "100%">
    <tr>
    <td>Month 
        </td>
        <td >
           <asp:DropDownList ID="ddlMonth1" runat="server">
                <asp:ListItem Value="1">January</asp:ListItem>
                <asp:ListItem Value="2">February</asp:ListItem>
                <asp:ListItem Value="3">March</asp:ListItem>
                <asp:ListItem Value="4">April</asp:ListItem>
                <asp:ListItem Value="5">May</asp:ListItem>
                <asp:ListItem Value="6">June</asp:ListItem>
                <asp:ListItem Value="7">July</asp:ListItem>
                <asp:ListItem Value="8">August</asp:ListItem>
                <asp:ListItem Value="9">September</asp:ListItem>
                <asp:ListItem Value="10">October</asp:ListItem>
                <asp:ListItem Value="11">November</asp:ListItem>
                <asp:ListItem Value="12">December</asp:ListItem>
            </asp:DropDownList>
             <asp:DropDownList ID="ddlYear1" runat="server">
                <asp:ListItem>2012</asp:ListItem>
                <asp:ListItem>2013</asp:ListItem>
                <asp:ListItem>2014</asp:ListItem>
                <asp:ListItem>2015</asp:ListItem>
                <asp:ListItem>2016</asp:ListItem>
                <asp:ListItem>2017</asp:ListItem>
                <asp:ListItem>2018</asp:ListItem>
                <asp:ListItem>2019</asp:ListItem>
                <asp:ListItem>2020</asp:ListItem>
                <asp:ListItem>2021</asp:ListItem>
                <asp:ListItem>2022</asp:ListItem>
                <asp:ListItem>2023</asp:ListItem>
            </asp:DropDownList></td>
        <td>
            Fortnight 
        </td>
        <td>
            
            <asp:DropDownList ID="ddlFortnight1" runat="server">
                <asp:ListItem>ALL</asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
            </asp:DropDownList></td>
        <td align="center">
            <img src="Images/vs2.png" />
            </td>
        <td>
            &nbsp;</td>
        <td>
            Month 
        </td>
        <td>
            <asp:DropDownList ID="ddlMonth2" runat="server">
                <asp:ListItem Value="1">January</asp:ListItem>
                <asp:ListItem Value="2">February</asp:ListItem>
                <asp:ListItem Value="3">March</asp:ListItem>
                <asp:ListItem Value="4">April</asp:ListItem>
                <asp:ListItem Value="5">May</asp:ListItem>
                <asp:ListItem Value="6">June</asp:ListItem>
                <asp:ListItem Value="7">July</asp:ListItem>
                <asp:ListItem Value="8">August</asp:ListItem>
                <asp:ListItem Value="9">September</asp:ListItem>
                <asp:ListItem Value="10">October</asp:ListItem>
                <asp:ListItem Value="11">November</asp:ListItem>
                <asp:ListItem Value="12">December</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlYear2" runat="server">
                <asp:ListItem>2012</asp:ListItem>
                <asp:ListItem>2013</asp:ListItem>
                <asp:ListItem>2014</asp:ListItem>
                <asp:ListItem>2015</asp:ListItem>
                <asp:ListItem>2016</asp:ListItem>
                <asp:ListItem>2017</asp:ListItem>
                <asp:ListItem>2018</asp:ListItem>
                <asp:ListItem>2019</asp:ListItem>
                <asp:ListItem>2020</asp:ListItem>
                <asp:ListItem>2021</asp:ListItem>
                <asp:ListItem>2022</asp:ListItem>
                <asp:ListItem>2023</asp:ListItem>
            </asp:DropDownList></td>
        <td>
            Fortnight 
        </td>
        <td>
            <asp:DropDownList ID="ddlFortnight2" runat="server">
                <asp:ListItem>ALL</asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
            </asp:DropDownList></td>
    </tr>
    <tr>
    <td class="style1" colspan="2"><asp:RadioButtonList ID="rbRegLoc" runat="server" 
                RepeatDirection="Horizontal" 
                onselectedindexchanged="rbRegLoc_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Selected="True">Location</asp:ListItem>
                <asp:ListItem>Region</asp:ListItem>
            </asp:RadioButtonList>
            </td>
        <td class="style1" colspan="2"><asp:DropDownList ID="ddlRegionLocation" runat="server">
            </asp:DropDownList>
            </td>
        <td class="style1">
            </td>
        <td class="style1">
            &nbsp;</td>
        <td class="style1">
            </td>
        <td class="style1">
            </td>
        <td class="style1">
            </td>
        <td class="style1"><asp:Button ID="btnPopulate" runat="server" Text="Populate" CssClass = "button" 
                onclick="btnPopulate_Click" OnClientClick = "if (!validation()) return false;" />
            </td>
    </tr>
</table>
</div>
<div style="min-height:400px;">
<div id = "divProduction" style="top:90px;">
</div></div>
</div>

<div id = "divLabels" style="width:0%;background-color:White" runat = "server" >
<asp:Label ID="lblMonth1" runat="server" EnableViewState="true" Width="0px" ></asp:Label>
<asp:Label ID="lblMonth2" runat="server" EnableViewState="true" Width="0px" ></asp:Label>
<asp:Label ID="lblTicks1" runat="server" EnableViewState="true" Width="0px" ></asp:Label>
<asp:Label ID="lblTicks2" runat="server" EnableViewState="true" Width="0px" ></asp:Label>
</div>
    
    
    
</asp:Content>