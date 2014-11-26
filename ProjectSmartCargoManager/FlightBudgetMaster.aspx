<%@ Page Title="Flight Budget Master" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="FlightBudgetMaster.aspx.cs" Inherits="ProjectSmartCargoManager.FlightBudgetMaster" %>
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
       <h1>Flight Budget Master </h1>
          <div class="botline">
            <table width="100%">
                <tr>
                    <td>
                        Region</td>
                    <td>
                        <asp:DropDownList ID="ddlRegion" runat="server">                        
                        </asp:DropDownList> </td>
                    <td >
                        Origin*</td>
                    <td >
   <asp:DropDownList ID="ddlOrigin" runat="server"></asp:DropDownList>
                    </td>
                    
                    
                                        
                    <td >
                        Destination*</td>
                    <td>
<asp:DropDownList ID="ddlDestination" runat="server"></asp:DropDownList>
                        </td>
                    
                </tr>
                <tr>
                    <td>
                        Month*</td>
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
                        Year*</td>
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
    <table width="100%">
        <tr>
        <td>
            
            <asp:Panel ID="pnlGrid" runat="server" ScrollBars="Vertical" Width="1000px">
                <br />
                <asp:GridView ID="grvBudgetMaster" runat="server" AllowPaging="True" 
                    AlternatingRowStyle-CssClass="AltRowStyle" AutoGenerateColumns="False" 
                    AutoGenerateEditButton="false" HeaderStyle-CssClass="HeaderStyle" 
                    onpageindexchanging="grvBudgetMaster_PageIndexChanging" 
                    onselectedindexchanged="grvBudgetMaster_SelectedIndexChanged" 
                    PagerStyle-CssClass="PagerStyle" PageSize="10" RowStyle-CssClass="RowStyle" 
                    style="margin-top: 0px" Width="100%">
                    <RowStyle CssClass="RowStyle" HorizontalAlign="Center" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Region">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRegion" runat="server" Text='<%# Eval("Region") %>' 
                                    Width="60px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Market">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMarket" runat="server" Text='<%# Eval("Market") %>' 
                                    Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Currency">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCurrency" runat="server" Text='<%# Eval("Currency") %>' 
                                    Width="60px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FlightNo">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFlightNo" runat="server" Text='<%# Eval("FlightNo") %>' 
                                    Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Origin">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOrigin" runat="server" Text='<%# Eval("Origin") %>' 
                                    Width="70px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Dest.">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDestination" runat="server" 
                                    Text='<%# Eval("Destination") %>' Width="75px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Year">
                            <ItemTemplate>
                                <asp:TextBox ID="txtYear" runat="server" Text='<%# Eval("Year") %>' 
                                    Width="75px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Month">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMonth" runat="server" Text='<%# Eval("Month") %>' 
                                    Width="75px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AC_Type">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAC_Type" runat="server" Text='<%# Eval("AC_Type") %>' 
                                    Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="FLT_INDIC">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFLT_INDIC" runat="server" Text='<%# Eval("FLT_INDIC") %>' 
                                    Width="60px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="POS">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPOS" runat="server" Text='<%# Eval("POS") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No Of Flights">
                            <ItemTemplate>
                                <asp:TextBox ID="txtNo_Of_Flights" runat="server" 
                                    Text='<%# Eval("No_Of_Flights") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UoM">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUoM" runat="server" 
                                Text='<%# Eval("UoM") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cargo per Flight">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCargo_per_Flight" runat="server" 
                                    Text='<%# Eval("Cargo_per_Flight") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PO Mail per Flight">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPO_Mail_per_Flight" runat="server" 
                                    Text='<%# Eval("PO_Mail_per_Flight") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Courier per Flight">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCourier_per_Flight" runat="server" 
                                    Text='<%# Eval("Courier_per_Flight") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cargo Rate">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCargo_Rate" runat="server" Text='<%# Eval("Cargo_Rate") %>' 
                                    Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="PO Mail rate">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPO_Mail_rate" runat="server" 
                                    Text='<%# Eval("PO_Mail_rate") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Courier Rate">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCourier_Rate" runat="server" 
                                    Text='<%# Eval("Courier_Rate") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ForeCastBudget">
                            <ItemTemplate>
                                <asp:TextBox ID="txtForeCastBudget" runat="server" 
                                    Text='<%# Eval("ForeCastBudget") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TargetBudget">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTargetBudget" runat="server" 
                                    Text='<%# Eval("TargetBudget") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="ID" Visible="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtID" runat="server" Visible="false"
                                    Text='<%# Eval("ID") %>' Width="80px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                      <%--  <asp:TemplateField HeaderText="IsActive">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkboxSelect" runat="server" 
                                    Checked='<%# Convert.ToBoolean(Eval("IsActive").ToString()) %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                    <PagerStyle CssClass="PagerStyle" />
                    <HeaderStyle CssClass="HeaderStyle" />
                    <AlternatingRowStyle CssClass="AltRowStyle" />
                </asp:GridView>
                <br />
                <asp:Button ID="btnAdd" runat="server" CssClass="button" onclick="btnAdd_Click" 
                    Text="Add" />
                <asp:Button ID="btnSave" runat="server" CssClass="button" 
                    onclick="btnSave_Click" Text="Save" />
                &nbsp;
                <asp:FileUpload ID="BudgetFileUpload" runat="server" />
                &nbsp;
                <asp:Button ID="btnUploadBudget" runat="server" onclick="btnUploadBudget_Click" 
                    Text="Upload" />
                &nbsp;
                <asp:HyperLink ID="HyperLink11" runat="server" 
                    NavigateUrl="~/Templates/Budget_Template.xlsx" Text="Download Template" />
            </asp:Panel>
            
                    </td>  
            
            
</tr>
   </table>
         
         </div>
</asp:Content>
