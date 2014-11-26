<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="ListAircraftEquipment.aspx.cs" Inherits="ProjectSmartCargoManager.ListAircraftEquipment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <script type="text/javascript">

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(callShow);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(callclose);

        function callShow() {
            document.getElementById('msglight').style.display = 'block';
            document.getElementById('msgfade').style.display = 'block';
            //document.getElementById("<%= msgshow.ClientID %>").innerHTML = "Loading Data .......";

        }
        function callclose() {
            document.getElementById('msglight').style.display = 'none';
            document.getElementById('msgfade').style.display = 'none';
        }
     </script>
    
    <style type="text/css">
        .black_overlay
        {
            display: none;
            position: absolute;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 1000px;
            background-color: black;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: 0.8;
            filter: alpha(opacity=80);
        }
        .white_content
        {
            margin: 0 auto;
            display: none;
            position: absolute;
            top: 30%;
            left: 35%;
            width: 30%;
            height: 45%;
            padding: 16px;
            border: 16px solid #ccdce3;
            background-color: white;
            z-index: 1002;
            overflow: auto;
        }
        .black_overlaymsg
        {
            display: none;
            position: fixed;
            top: 0%;
            left: 0%;
            width: 100%;
            height: 100%;
            background-color: White;
            z-index: 1001;
            -moz-opacity: 0.8;
            opacity: .80;
            filter: alpha(opacity=80);
        }
        .white_contentmsg
        {
            display: none;
            position: fixed;
            top: 45%;
            left: 45%;
            width: 5%;
            height: 5%;
            padding: 16px;
            background-color: Transparent;
            z-index: 1002;
        }
    </style>
    
    <asp:UpdatePanel runat="server" ID="updtPnl">
    <ContentTemplate>
    
    <div id="contentarea">
        <h1>
            &nbsp; Aircraft Equipment List</h1>
       
    
    <div class="botline">
        <table style="width: 50%;">
            <tr>
                <td>
                    Aircraft Type
                </td>
                <td>
                    <asp:DropDownList ID="ddlAircraftType" runat="server" 
                        >
                        <asp:ListItem>Select</asp:ListItem>
                    </asp:DropDownList>
                       
                    
                </td>
                <td>
                   Version
                    </td>
                <td>
                    <asp:DropDownList ID="ddlVersion" runat="server">
                        <asp:ListItem>Select</asp:ListItem>
                    </asp:DropDownList>
                    
                </td>
                <td>
                    <asp:Button ID="btnList" runat="server" CssClass="button" OnClick="btnList_Click"  OnClientClick="callShow();" Text="List"/>
                    &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click" />
                    &nbsp; 
                 
                </td>
            </tr>

        </table>
    </div>
   
   <br />
   
    <div >
     <h2>Equipment Details
   </h2>
       
<asp:GridView ID="grdAcEq" runat="server"  AutoGenerateColumns="False" ShowFooter="True" 
Width="1024px" Height="82px" CellSpacing="2" CellPadding="2" onrowcommand="grdAcEq_RowCommand"  
AlternatingRowStyle-CssClass="AltRowStyle" HeaderStyle-CssClass="HeaderStyle"  
ShowHeaderWhenEmpty="True"  EmptyDataText="No Data Found"
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" 
            SelectedRowStyle-CssClass="SelectedRowStyle" HorizontalAlign="Center">
             <PagerStyle CssClass="PagerStyle" />
             <SelectedRowStyle CssClass="SelectedRowStyle" />
             <HeaderStyle CssClass="HeaderStyle" />
             <AlternatingRowStyle CssClass="trcolor"></AlternatingRowStyle>
             <RowStyle CssClass="RowStyle" />
            <Columns>
                <asp:BoundField DataField="Manufacturer" HeaderText="Manufacturer" />
                <asp:BoundField DataField="AircraftType" HeaderText="Aircraft   Type" />
                <asp:BoundField DataField="Version" HeaderText="Version" />
                <asp:BoundField DataField="PassengerCapacity" HeaderText="Capacity" />
                <asp:BoundField DataField="LandingWeight" HeaderText="Landing   Weight" />
                <asp:BoundField DataField="CargoCapacity" HeaderText="Cargo   Capacity" />
                <asp:BoundField DataField="MTOW" HeaderText="MTOW" />
                <asp:BoundField DataField="TailNo" HeaderText="TailNo" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="RWt" HeaderText="Weight" Visible="false" />
                <asp:BoundField DataField="Rl" HeaderText="Dim.       Length" />
                <asp:BoundField DataField="Rb" HeaderText="Dim.   Breadth" />
                <asp:BoundField DataField="Rh" HeaderText="Dim.Height" />
                <asp:BoundField DataField="RUnit" HeaderText="Dim.Unit" />
                <asp:BoundField DataField="Vl" HeaderText="Volume" />
                <asp:BoundField DataField="VUnit" HeaderText="Volume.    Unit" />
                <asp:BoundField DataField="AIdentity" HeaderText="Identity" />
                
               <%-- <asp:BoundField DataField="Srno" HeaderText="" Visible="false"/>--%>
                <asp:TemplateField HeaderText="Srno" Visible="false">
                                    <ItemTemplate>
                                    <asp:Label ID="lblSrno" runat="server" Text='<%# Eval("Srno") %>' ></asp:Label> 
                                    </ItemTemplate>
                                     </asp:TemplateField>
                  <asp:ButtonField CommandName="Edit" Text="Edit">
                </asp:ButtonField>
               <%-- <asp:TemplateField HeaderText="Edit" ShowHeader="False" Visible="false">
                    <ItemTemplate>
                        <asp:Button ID="btnDelete" runat="server" Text="Delete"  CssClass="button"/>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                
                
            </Columns>
           <EmptyDataRowStyle HorizontalAlign="Center" verticalAlign="Middle"></EmptyDataRowStyle>

        </asp:GridView>
    </div>
    </div>
    
    <div id="msglight" class="white_contentmsg">
        <table>
            <tr>
                <td width="5%" align="center">
                    <br />
                    <img src="Images/loading.gif" />
                    <br />
                    <asp:Label ID="msgshow" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    
    <div id="msgfade" class="black_overlaymsg">
    </div>
    
    </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>

