<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlightCapacityPlanning.aspx.cs"    MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.FlightCapacityPlanning" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

 <script type="text/javascript">

     Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
     Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

     function callShow() {
         document.getElementById('msglight').style.display = 'block';
         document.getElementById('msgfade').style.display = 'block';

     }
     function callclose() {
         document.getElementById('msglight').style.display = 'none';
         document.getElementById('msgfade').style.display = 'none';
     }

     </script>
     <style type="text/css">
     .black_overlaymsg{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: White;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_contentmsg {
			display: none;
			position: fixed;
			top: 45%;
			left: 45%;
			width: 5%;
			height: 5%;
			padding: 16px;
			background-color: Transparent;
			z-index:1002;
			
		}
		
        </style>
    <script language="javascript" type="text/javascript">
        function getBtnid(e, val) {
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
        function getid(e) {
            var ctrlname = e.id.replace('btnManage', 'lblAWBno');
            var valExpectedAWB = document.getElementById(ctrlname).innerHTML;
            if (valExpectedAWB.trim() == "") {
                alert("No AWB Available");
                return false;
            }
            else {
                document.getElementById("<%=hdn.ClientID %>").value = valExpectedAWB;
                return true;
            }
        }

        function cllsa() {

            document.getElementById("<%=btnList.ClientID %>").click();
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
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
        <asp:HiddenField ID="hdn" runat="server" />
        <asp:HiddenField ID="hdnFliNo" runat="server" />
        <asp:HiddenField ID="hdnFliDt" runat="server" />
        <div id="contentarea">
     
            <h1>Confirm Booking</h1>
            <asp:UpdatePanel ID="UPFourth" runat="server">
                <ContentTemplate>
                    <asp:Label ID="lblStatus" runat="server" ForeColor="Red" Font-Bold="True" Font-Size="Large"></asp:Label>
                </ContentTemplate>
            </asp:UpdatePanel>
        
           <div class="botline">
        <table width="100%">
        <tr>
        <td>
            <asp:UpdatePanel ID="UPFirst" runat="server">
                <ContentTemplate>
                    <table style="width: 950px; height: 100%" border="0">
                        <tr>
                            <td>
                                Origin
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSource" runat="server" 
                                    OnSelectedIndexChanged="ddlSource_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Destination
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlDest" runat="server" 
                                    OnSelectedIndexChanged="ddlDest_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                            <td>
                                Flight
                            </td>
                            <td>
                                  <asp:TextBox ID="txtFlightCode" runat="server" MaxLength="2" Width="30px"></asp:TextBox>&nbsp;
                                  <asp:TextBox ID="txtFlightNo" runat="server" MaxLength="4" Width="70px"></asp:TextBox>
                            &nbsp;
                                <asp:TextBox ID="txtFromDate" runat="server" Width="115px"></asp:TextBox>
                                <asp:ImageButton ID="btnFromDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                    ImageAlign="AbsMiddle" />
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtFromDate"
                                  runat="server" ErrorMessage="*" ValidationGroup="List">
                                 </asp:RequiredFieldValidator>
                                <asp:CalendarExtender ID="CEFromDate" Format="dd/MM/yyyy" TargetControlID="txtFromDate"
                                    PopupButtonID="btnFromDate" runat="server" PopupPosition="BottomLeft">
                                </asp:CalendarExtender>
                            
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
                           
                        </tr>
                        <tr>
                        
                            <td>
                                <asp:Label ID="LBLAWBStatus" runat="server" Text="Status"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="DDLStatus" runat="server">
                                   <%-- <asp:ListItem Text="Overbooked" Value="B"></asp:ListItem>  --%>     
                                    <asp:ListItem Text="ALL" Value="A" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </td>                            
                            <td>
                              Agent Code
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlAgentCode" runat="server" Width="80px" 
                                    AppendDataBoundItems="True">                                  
                                </asp:DropDownList>
                            </td>
                             <td>
                               <%-- AWBNumber--%>
                                 AWB</td>
                            <td>
                                <asp:TextBox ID="TXTAWBPrefix" runat="server" Width="30px" MaxLength="3" ></asp:TextBox>&nbsp
                                <asp:TextBox ID="TXTAWBNumber" runat="server" Width="110px" MaxLength="8"></asp:TextBox>
                            </td>
                            
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                   <%-- <asp:AsyncPostBackTrigger ControlID="btnList" EventName="Click" />--%>
                </Triggers>
            </asp:UpdatePanel>
            </td>
          
        </tr>    
         <td>
             <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
                                    onclick="btnList_Click"  ValidationGroup="List" OnClientClick="callShow();" />
                                    
             <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
                                    onclick="btnClear_Click"  ValidationGroup="Clear" />
          </td>      
        </table>
        </div>

        </div>
        <br />
        <asp:UpdatePanel ID="upup" runat="server">
        <ContentTemplate>
        <div class="ltfloat" style="width:100%; border:1px solid gray;">
        <asp:GridView ID="GridView1" AllowPaging="false" 
            AutoGenerateColumns="false" 
            style="Z-INDEX: 101; border:0px;" Width="100%"
        ShowFooter="false" Font-Size="Small"
            Font-Names="Verdana" runat="server" GridLines="None" 
           OnRowDataBound="GridView1_RowDataBound"
         BorderStyle="Outset"
        AllowSorting="true"  
             PageSize="7" HeaderStyle-CssClass="HeaderStyle"
                                CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle"
                                PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle"  >
            <RowStyle  HorizontalAlign="Left" BorderStyle="Solid" BorderWidth="1px" />
            <HeaderStyle  Height="30px" HorizontalAlign="Left"/>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:expandcollapse('div<%# Eval("FlightNo") + "," + Eval("FlightDate") + "," + Eval("DeptTime") + "," + Eval("ArrTime") %>', 'one');">
                            <img id="imgdiv<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" alt="Click to show/hide Orders for Customer <%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>"  width="9px" border="0" src="plus.gif"/>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Flt Orig" >
                    <ItemTemplate>
                        <asp:Label ID="lblOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                    
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Flt Dest" >
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
                
			    <asp:TemplateField>
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
                            <td colspan="100%">
                                <div id="div<%# Eval("FlightNo") + "," + Eval("FlightDate")+ "," + Eval("DeptTime") + "," + Eval("ArrTime") %>" style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:97%" >
                                    <asp:GridView ID="GridView2" AllowPaging="false" AllowSorting="false"
                                     Width="100%" Font-Size="X-Small"
                                        AutoGenerateColumns="false" Font-Names="Verdana" runat="server"  ShowFooter="false"
                                       GridLines="None" 
                                          BorderStyle="Double" BorderColor="#0083C1"
                                          HeaderStyle-CssClass="HeaderStyle"
                                CssClass="GridViewStyle" RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle"
                                PagerStyle-CssClass="PagerStyle" SelectedRowStyle-CssClass="SelectedRowStyle"
                                 OnRowDataBound="GridView2_RowDataBound" >
                                        <RowStyle  HorizontalAlign="Left"/>
                                        <HeaderStyle  Height="30px" HorizontalAlign="Left"/>
                                        <FooterStyle />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Product Type">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProductType" Text='<%# Eval("ProductType") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="AWB#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBno" Text='<%# Eval("AWBNo") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Comm. Code">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCommCode" Text='<%# Eval("CommodityCode") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Origin">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                              <asp:TemplateField HeaderText="Dest.">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBDest" Text='<%# Eval("Destination") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Agent">
                                                <ItemTemplate>  <asp:Label ID="lblAgent" Text='<%# Eval("Agent") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pcs" >
                                                <ItemTemplate> <asp:Label ID="lblPriority" Text='<%# Eval("Priority") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Weight" >
                                                <ItemTemplate><asp:Label ID="lblWeight" Text='<%# Eval("Weight") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
			                                <asp:TemplateField HeaderText="Status" >
                                                <ItemTemplate><asp:Label ID="lblStatus" Text='<%# Eval("Status") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                
                                                <asp:Button ID="btnManage" runat="server" Text="Edit" OnClientClick="return getid(this,'m');" OnClick="editMode" />
                                                 <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClientClick="return getBtnid(this,'c');" OnClick="ConfirmShipment"/>
                                               
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
			 <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>   
       </div>
       </ContentTemplate>
        </asp:UpdatePanel>
        
        <div id="msglight" class="white_contentmsg" >
<table>

<tr>

<td width="5%" align="center">
    <br />
    <img src="Images/loading.gif" />
    
</td>
</tr>
</table>
		</div>
		<div id="msgfade" class="black_overlaymsg"></div>
       </asp:Content>