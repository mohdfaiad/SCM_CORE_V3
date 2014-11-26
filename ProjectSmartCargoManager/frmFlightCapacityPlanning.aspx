<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmFlightCapacityPlanning.aspx.cs"    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.frmFlightCapacityPlanning" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>

 <script language="javascript" type="text/javascript">
        
        function getid(e,val) {
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
        document.getElementById("<%=btnList.ClientID %>").click();
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

    function GetAgentCode() {
        var level = 'AgentCode';
        var TxtOriginClientObject = '<%=txtAgentCode.ClientID %>';
        window.open('ListDataView.aspx?Parent=AgentCode&level=' + level + '&TargetTXT=' + TxtOriginClientObject, '', 'left=' + (screen.availWidth / 2 - 100) + ',top=' + (screen.availHeight / 2 - 100) + ',width=300,height=300,toolbar=0,resizable=0');
        return false;
    }

    
</script>

<style type="text/css">
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
    
    .highlight{ background:#7eabfe !important; height:30px; color:#fff;}
    
    .style1
    {
        height: 29px;
    }
    
    .style2
    {
        height: 29px;
        width: 62px;
    }
    .style3
    {
        width: 62px;
    }
    
</style>

<script type="text/javascript">
    $(document).ready(function() {
        $('#<%= GridView1.ClientID %>').find('tr').click(function callg() {
            if ($(this).hasClass('RowStyle')) {
                $('#<%= GridView1.ClientID %> tr').removeClass('highlight');
                $(this).removeClass('RowStyle');
                $(this).addClass('highlight');
        });
    });
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:HiddenField ID="hdn" runat="server" />
        <asp:HiddenField ID="hdnFliNo" runat="server" />
        <asp:HiddenField ID="hdnFliDt" runat="server" />
        <div id="contentarea">
        <h1>
        <asp:Label ID="lblHeader" Text="Revenue Management" runat="server"></asp:Label>
        </h1>
        <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
        
        <div class="botline">
        <table width="100%">
        <tr>
        <td rowspan="2">
           <%-- <asp:UpdatePanel ID="UPFirst" runat="server">
                <ContentTemplate>--%>
                    <table style="width:90%; height: 100%;" border="0">
                        <tr>
                            <td class="style2">
                                Origin
                            </td>
                            <td class="style1">
                                <asp:DropDownList ID="ddlSource" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSource_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="style1">
                                Destination
                            </td>
                            <td class="style1">
                                <asp:DropDownList ID="ddlDest" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlDest_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td class="style1">
                                Flight
                            </td>
                            <td class="style1">
                                <asp:DropDownList ID="ddlFlight" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td class="style1">
                                <asp:Label ID="LBLAWBStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td class="style1">
                                <asp:DropDownList ID="DDLStatus" runat="server">
                                    <asp:ListItem Text="Overbooked" Value="B"></asp:ListItem>       
                                    <asp:ListItem Text="ALL" Value="A" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="style3">
                                From Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromDate" runat="server" Width="115px"></asp:TextBox>
                                <asp:ImageButton ID="btnFromDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFromDate"
                                  runat="server" ErrorMessage="*" ValidationGroup="List">
                                 </asp:RequiredFieldValidator>
                                <asp:CalendarExtender ID="CEFromDate" Format="dd/MM/yyyy" TargetControlID="txtFromDate"
                                    PopupButtonID="btnFromDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                            <td>
                                To Date
                            </td>
                            <td>
                                <asp:TextBox ID="txtToDate" runat="server" Width="115px"></asp:TextBox>
                                <asp:ImageButton ID="btnToDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txtToDate"
                                  runat="server" ErrorMessage="*" ValidationGroup="List">
                                 </asp:RequiredFieldValidator>
                                <asp:CalendarExtender ID="CEToDate" Format="dd/MM/yyyy" TargetControlID="txtToDate"
                                    PopupButtonID="btnToDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            </td>
                           
                            <td>
                              Agent Code
                            </td>
                            <td>
<%--                                <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
                                    AppendDataBoundItems="True">                                  
                                </asp:DropDownList>--%>
                                <asp:TextBox ID="txtAgentCode" runat="server" Width="98px"></asp:TextBox>
                                 <asp:ImageButton ID="IBOrigin" runat="server" ImageUrl="~/Images/list_bullets.png"
                            ImageAlign="AbsMiddle" OnClientClick="javascript:GetAgentCode();return false;" />
                                
                            </td>
                             <td colspan="2">
                               <asp:CheckBox ID="chkShowNilFlt" runat="server" Checked="false" Text="Show all flights"></asp:CheckBox>
                            </td>
                            
                        </tr>
                        <tr>
                            <td class="style3">
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Button ID="btnList" runat="server" CssClass="button" 
                                    onclick="btnList_Click" Text="List" ValidationGroup="List" />
                            </td>
                            <td>
                                <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="Export" CssClass="button"  />
                                &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                                    onclick="btnClear_Click" /></td>
                            <td>
                                </td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                &nbsp;</td>
                            <td>
                                <%-- AWBNumber--%></td>
                            <td>
                                <asp:TextBox ID="TXTAWBPrefix" runat="server" Width="30px" MaxLength="2" Visible="false" ></asp:TextBox>&nbsp
                                <asp:TextBox ID="TXTAWBNumber" runat="server" Width="110px" MaxLength="8" Visible="false"></asp:TextBox>
                                </td>
                        </tr>
                    </table>
             <%--   </ContentTemplate>
                <Triggers>
                  <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />
                </Triggers>--%>
           <%-- </asp:UpdatePanel>--%>
            </td>
            <td>
            
                                    
                                    </td>
        </tr>
        <tr>
            <td>
                                    </td>
        </tr>
        </table>
        </div>
     
        <br />
        <div style="margin-top:0px; float:left;width:100%;" id="dispgrid" runat="server">
        Click on 
            <img src="Images/plus.gif" /> To View Details of AWB,&nbsp; <img src="Images/plusgreen.gif" /> 
            To View Summery&nbsp; 
            
        <asp:GridView ID="GridView1" AllowPaging="True" 
            AutoGenerateColumns="false" 
            style="Z-INDEX: 101" Width="100%"
        ShowFooter="false" Font-Size="Small"
             runat="server" 
           OnRowDataBound="GridView1_RowDataBound"
          AllowSorting="true"  
             PageSize="7" onpageindexchanging="GridView1_PageIndexChanging"
                            HeaderStyle-CssClass="HeaderStyle"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle"  PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle" >
            <RowStyle  HorizontalAlign="Left"/>
            <HeaderStyle  Height="30px" HorizontalAlign="center" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:expandcollapse('div<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>', 'one');">
                            <img id="imgdiv<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" alt="Click to show/hide Orders for Customer <%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>"  width="9px" border="0" src="plus.gif"/>
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
                        <img id="imgdiv<%# Eval("Origin") +","+ Eval("Destination") +","+ Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" alt="Click to show/hide<%# Eval("Origin") +","+ Eval("Destination") +","+ Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>"  width="9px" border="0" src="plusgreen.gif"/>
                        </a>
                        </ItemTemplate>
                        </asp:TemplateField>
                 		    
			    <asp:TemplateField  HeaderStyle-CssClass="showh" ItemStyle-CssClass="showh">
			        <ItemTemplate>
			         <tr>
                            <td colspan="13" style="border-bottom:0px;">
                            
                                <div id="div<%# Eval("Origin") +","+ Eval("Destination") +","+ Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:100%;" >
                                <strong>Summary</strong>
                                <asp:Table ID="tblLYP" runat="server" BorderWidth="1px" Width="97%"  GridLines="Both" CssClass="tbcen">
                                <asp:TableRow>
                                 <asp:TableCell ColumnSpan="8" Width="50%" BackColor="#656667" ForeColor="#ffffff"><strong>Cargo History Management</strong></asp:TableCell>
                                 <asp:TableCell ColumnSpan="4" Width="50%"  BackColor="#656667" ForeColor="#ffffff"><strong>PAX History Management</strong></asp:TableCell>
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
                                <div id="div<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:97%" >
                                    <strong> AWB Details</strong><br />
                                    <asp:GridView ID="GridView2" AllowPaging="false" AllowSorting="false"
                                     Width="100%" Font-Size="Small"
                                        AutoGenerateColumns="false" runat="server"  ShowFooter="false"
                                        OnRowDataBound="GridView2_RowDataBound"
                                          HeaderStyle-CssClass="AltRowStyle" HeaderStyle-BackColor="#656667" HeaderStyle-ForeColor="#ffffff"  CssClass="GridViewStyle" RowStyle-CssClass="RowStyle"  PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
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
                                                <ItemTemplate>  <asp:Label ID="lblAgent" Text='<%# Eval("Agent") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Product Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductType" Text='<%# Eval("ProductType") %>' runat="server"></asp:Label>
                                                </ItemTemplate> 
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comm. Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommCode" Text='<%# Eval("CommodityCode") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="Priority" >
                                                <ItemTemplate> <asp:Label ID="lblPriority" Text='<%# Eval("Priority") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Pcs" >
                                                <ItemTemplate><asp:Label ID="lblAccPcs" Text='<%# Eval("AcceptedPcs") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight" >
                                                <ItemTemplate><asp:Label ID="lblWeight" Text='<%# Eval("Weight") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="Load/Cu M." >
                                                <ItemTemplate><asp:Label ID="lblVolume" Text='<%# Eval("Volume") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>                                            
			                                <asp:TemplateField HeaderText="Status" >
                                                <ItemTemplate><asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Rate" >
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
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                
                                                <asp:Button ID="btnManage" runat="server" Text="Edit" OnClientClick="return getid(this,'m');" OnClick="editMode" />
                                                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClientClick="return getid(this,'c');" OnClick="ConfirmShipment"/>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField >
                                            <ItemTemplate>
                                            <img id="btnviability" runat="server" src="~/Images/search1.png" />
                                                <%--<asp:Label ID="btnviability" runat="server" Text="V"   />--%>
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
                                                <%--<Columns>
                                                
                                                <asp:TemplateField HeaderText="Partner" >
                                                <ItemTemplate><asp:Label ID="lblPartner" Text='<%# Eval("Partner") %>' runat="server"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SPA Rate" >
                                                <ItemTemplate><asp:Label ID="lblSPARate" Text='<%# Eval("SPARate") %>' runat="server"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SPA Freight" >
                                                <ItemTemplate><asp:Label ID="lblSPAFreight" Text='<%# Eval("SPAFreight") %>' runat="server"></asp:Label></ItemTemplate>
                                                </asp:TemplateField>
                                                </Columns>--%>
                                                </asp:GridView>
                                                </asp:Panel>
                                            </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                   </asp:GridView>
                                </div>
                             </td>
                        </tr>
			        </ItemTemplate>			       
			    </asp:TemplateField>			    
			</Columns>
			 <%--<HeaderStyle CssClass="titlecolr"/>--%>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>   
       </div>
          </div>
       </asp:Content>