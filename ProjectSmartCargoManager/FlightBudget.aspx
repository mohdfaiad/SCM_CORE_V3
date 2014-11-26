<%@ Page Title="Budget Vs Actual Report" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="FlightBudget.aspx.cs" Inherits="ProjectSmartCargoManager.FlightBudget" %>
<%@ Register Assembly="DataDynamics.Reports.Web, Version=1.6.2084.14, Culture=neutral, PublicKeyToken=d557f2f30a260da2" Namespace="DataDynamics.Reports.Web" TagPrefix="dd" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
function expandcollapse(obj, row) {
        var div = document.getElementById(obj);
        var img = document.getElementById('img' + obj);
        //alert('Hello');

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
    <asp:ToolkitScriptManager ID="TSM0" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="contentarea">
   
          <div class="msg">
<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
</div>
    <%--<h1>
            <img alt="" src="Images/flightshlist.png"  style="vertical-align:5"/> </h1>--%>
       <h1>Budget Vs Actual</h1>
          <div class="botline">
            <table width="100%">
                <tr>
                    <td>
                        Region</td>
                    <td>
                        <asp:DropDownList ID="ddlRegion" runat="server">                        
                        </asp:DropDownList> </td>
                    <td >
                        Origin</td>
                    <td >
   <asp:DropDownList ID="ddlOrigin" runat="server"></asp:DropDownList>
                    </td>
                    
                    
                                        
                    <td >
                        Destination</td>
                    <td>
<asp:DropDownList ID="ddlDestination" runat="server"></asp:DropDownList>
                        </td>
                    
                </tr>
                <tr>
                    <td>
                        Month</td>
                    <td>
                        <asp:DropDownList ID="ddlMonth" runat="server">
                        <asp:ListItem Selected="True">Select</asp:ListItem>
                        <asp:ListItem>JAN</asp:ListItem>
                        <asp:ListItem>FEB</asp:ListItem>
                        <asp:ListItem>MAR</asp:ListItem>
                        <asp:ListItem>APR</asp:ListItem>
                        <asp:ListItem>MAY</asp:ListItem>
                        <asp:ListItem>JUN</asp:ListItem>
                        <asp:ListItem>JUL</asp:ListItem>
                        <asp:ListItem>AUG</asp:ListItem>
                        <asp:ListItem>SEP</asp:ListItem>
                        <asp:ListItem>OCT</asp:ListItem>
                        <asp:ListItem>NOV</asp:ListItem>
                        <asp:ListItem>DEC</asp:ListItem>
                        </asp:DropDownList> &nbsp;</td>
                    <td >
                        Year</td>
                    <td >
                      <asp:DropDownList ID="ddlYear" runat="server">
                      <asp:ListItem Selected="True">Select</asp:ListItem>
                      <asp:ListItem>2000</asp:ListItem>
                      <asp:ListItem>2001</asp:ListItem>
                      <asp:ListItem>2002</asp:ListItem>
                      <asp:ListItem>2003</asp:ListItem>
                      <asp:ListItem>2004</asp:ListItem>
                      <asp:ListItem>2005</asp:ListItem>
                      <asp:ListItem>2006</asp:ListItem>
                      <asp:ListItem>2007</asp:ListItem>
                      <asp:ListItem>2008</asp:ListItem>
                      <asp:ListItem>2009</asp:ListItem>
                      <asp:ListItem>2010</asp:ListItem>
                      <asp:ListItem>2011</asp:ListItem>
                      <asp:ListItem>2012</asp:ListItem>
                      <asp:ListItem>2013</asp:ListItem>
                      <asp:ListItem>2014</asp:ListItem>
                      <asp:ListItem>2015</asp:ListItem>
                      <asp:ListItem>2016</asp:ListItem>
                      <asp:ListItem>2017</asp:ListItem>
                      <asp:ListItem>2018</asp:ListItem>
                      <asp:ListItem>2019</asp:ListItem>
                      <asp:ListItem>2020</asp:ListItem>
                      </asp:DropDownList>
                        &nbsp;</td>
                    <td >
                        Flight #</td>
                    <td>
                    <asp:DropDownList ID="ddlFlightPrefix" runat="server" AutoPostBack="True" 
                            onselectedindexchanged="ddlFlightPrefix_SelectedIndexChanged" ></asp:DropDownList>
              <%--<asp:TextBox ID="txtFlightNo" runat="server" Width="100px"></asp:TextBox>
                        --%>
                       <asp:DropDownList ID="ddlFlightNumber" runat="server" ></asp:DropDownList>
                        </td>
                    
                    
                    
                </tr>
                
                
                
                <tr>
                    <td>
                        POS</td>
                    <td>
   <asp:DropDownList ID="ddlPOS" runat="server"></asp:DropDownList>
                        </td>
                    <td >
                        Currency</td>
                    <td >
                        <asp:DropDownList ID="ddlCurrency" runat="server">                        
                        </asp:DropDownList>&nbsp;</td>
                    <td >
                       Country</td>
                    <td>
                    <asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList>
                        
                        
                        </td>
                    
                    
                    
                </tr>
             <tr>
             <td colspan="2">
             
             <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
                            onclick="btnList_Click" />
                                            
             <asp:Button ID="btnExport" runat="server" Text="Export" Visible="true"
                            CssClass="button" onclick="btnExport_Click" />
    
             <asp:Button ID="btnclear" runat="server" Text="Clear" 
                            CssClass="button" onclick="btnclear_Click" />
             </td>
             </tr>
                
                <tr>
                    <td colspan="6">
                        <%--<asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" 
             ForeColor="Red"></asp:Label></td>--%>
                    
                </tr>
           </table>
    
   
    
    </div>
    <div style="margin-top:0px; float:left;width:100%;">
        <asp:GridView ID="GridView1" AllowPaging="True" 
            AutoGenerateColumns="false" 
            style="Z-INDEX: 101" Width="100%"
        ShowFooter="false" Font-Size="Small"
            Font-Names="Verdana" runat="server" GridLines="None" 
           OnRowDataBound="GridView1_RowDataBound"
         BorderStyle="Outset"
        AllowSorting="true" onpageindexchanging="GridView1_PageIndexChanging" 
             PageSize="7"  >
            <RowStyle  HorizontalAlign="Left"/>
            <HeaderStyle  Height="30px" HorizontalAlign="Left"/>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <a href="javascript:expandcollapse('div<%# Eval("Region") + "," + Eval("Origin")+ "," + Eval("Destination") + "," + Eval("FlightNo") + "," + Eval("Month") + "," + Eval("Year") + "," + Eval("POS") %>', 'one');">
                            <img id="imgdiv<%# Eval("Region") + "," + Eval("Origin")+ "," + Eval("Destination") + "," + Eval("FlightNo") + "," + Eval("Month") + "," + Eval("Year") + "," + Eval("POS") %>" alt="Click to show/hide Orders for Customer <%# Eval("Region") + "," + Eval("Origin")+ "," + Eval("Destination") + "," + Eval("FlightNo") + "," + Eval("Month") + "," + Eval("Year") + "," + Eval("POS") %>"  width="9px" border="0" src="plus.gif"/>
                        </a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Region" >
                    <ItemTemplate>
                        <asp:Label ID="lblRegion" Text='<%# Eval("Region") %>' runat="server"></asp:Label>
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
                <asp:TemplateField HeaderText="Flight Month" >
                    <ItemTemplate>
                        <asp:Label ID="lblMonth" Text='<%# Eval("Month") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Dep Time" >
                    <ItemTemplate>
                        <asp:Label ID="lblYear" Text='<%# Eval("Year") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
			      <asp:TemplateField HeaderText="POS" >
                    <ItemTemplate>
                        <asp:Label ID="lblPOS" Text='<%# Eval("POS") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="TotalTonnage" >
                    <ItemTemplate>
                        <asp:Label ID="lblTotalTonnage" Text='<%# Eval("TotalTonnage") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="TotalRevenue" >
                    <ItemTemplate>
                        <asp:Label ID="lblTotalRevenue" Text='<%# Eval("TotalRevenue") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Budget Yeild" >
                    <ItemTemplate>
                        <asp:Label ID="lblBYeild" Text='<%# Eval("Yeild") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ActualTonnage" >
                    <ItemTemplate>
                        <asp:Label ID="lblActualTonnage" Text='<%# Eval("Confirmed") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="ActualRevenue" >
                    <ItemTemplate>
                        <asp:Label ID="lblActualRevenue" Text='<%# Eval("Revenue") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TYeild" >
                    <ItemTemplate>
                        <asp:Label ID="lblTYeild" Text='<%# Eval("TYeild") %>' runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                        
                  <asp:TemplateField>
                  <ItemTemplate>
                  
                   
                          <tr>
                            <td colspan="100%">
                                <div id="div<%# Eval("Region") + "," + Eval("Origin")+ "," + Eval("Destination") + "," + Eval("FlightNo") + "," + Eval("Month") + "," + Eval("Year") + "," + Eval("POS") %>" style="display:none;position:relative;left:15px;OVERFLOW: auto;WIDTH:97%" >
                                    <asp:GridView ID="GridView2" AllowPaging="false" AllowSorting="false"
                                     Width="100%" Font-Size="X-Small"
                                        AutoGenerateColumns="false" Font-Names="Verdana" runat="server"  ShowFooter="false"
                                       GridLines="None" 
                                          BorderStyle="Double" BorderColor="#0083C1">
                                        <RowStyle  HorizontalAlign="Left"/>
                                        <HeaderStyle  Height="30px" HorizontalAlign="Left"/>
                                        <FooterStyle />
                                        <Columns>
                                            <asp:TemplateField HeaderText="AWB#">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAWBno" Text='<%# Eval("AWBNumber") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Region">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRegion" Text='<%# Eval("Region") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Origin">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrigin" Text='<%# Eval("Origin") %>' runat="server"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Destination">
                                                <ItemTemplate>  <asp:Label ID="lblDestination" Text='<%# Eval("Destination") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FlightNo" >
                                                <ItemTemplate> <asp:Label ID="lblFlightNo" Text='<%# Eval("FlightNo") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="FlightDate" >
                                                <ItemTemplate> <asp:Label ID="lblFlightDate" Text='<%# Eval("FlightDate") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Month" >
                                                <ItemTemplate><asp:Label ID="lblMonth" Text='<%# Eval("Month") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
			                                <asp:TemplateField HeaderText="Year" >
                                                <ItemTemplate><asp:Label ID="lblYear" Text='<%# Eval("Year") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="POS" >
                                                <ItemTemplate><asp:Label ID="lblPOS" Text='<%# Eval("POS") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="TotalTonnage" >
                                                <ItemTemplate><asp:Label ID="lblTotalTonnage" Text='<%# Eval("TotalTonnage") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="TotalRevenue" >
                                                <ItemTemplate><asp:Label ID="lblTotalRevenue" Text='<%# Eval("TotalRevenue") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Yeild" >
                                                <ItemTemplate><asp:Label ID="lblYeild" Text='<%# Eval("Yeild") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                             <%--<asp:TemplateField HeaderText="Cost" >
                                                <ItemTemplate><asp:Label ID="lblCost" Text='<%# Eval("Cost") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>--%>
                                            
                                             <asp:TemplateField HeaderText="ActualTonnage" >
                                                <ItemTemplate><asp:Label ID="lblActualTonnage" Text='<%# Eval("Confirmed") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ActualRevenue" >
                                                <ItemTemplate><asp:Label ID="lblActualRevenue" Text='<%# Eval("Revenue") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ActualYeild" >
                                                <ItemTemplate><asp:Label ID="lblActualYeild" Text='<%# Eval("TYeild") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Ident Row"  ItemStyle-CssClass="showh"  HeaderStyle-CssClass="showh">
                                                <ItemTemplate><asp:Label ID="lblIdentRow" Text='<%# Eval("IdentRow") %>' runat="server"></asp:Label></ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                
                                                <%--<asp:Button ID="btnManage" runat="server" Text="Manage" OnClientClick="return getid(this);" OnClick="editMode" />--%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                            <asp:TemplateField >
                                            <ItemTemplate>
                                            
                                                <%--<asp:Label ID="btnviability" runat="server" Text="V"   />--%>
                                                
                                                
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
    <table width="100%">
        <tr>
        <td>
            
            <dd:WebReportViewer ID="rptViewerFlightBudget" runat="server" Height="500px" Width="1000px" />
            
                    </td>  
            
            
</tr>
   </table>
         
         </div>
</asp:Content>
