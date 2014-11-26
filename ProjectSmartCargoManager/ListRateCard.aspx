<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListRateCard.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master"
    Inherits="ProjectSmartCargoManager.ListRateCard" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:ToolkitScriptManager ID="TSM0" runat="server">
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
           Rate Card <%--<img alt="" src="images/listratecard.gif" /> --%>
        </h1>
        <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ></asp:Label>
        </p>
        <div class="botline">
        <table style="width: 60%; height: 100%" border="0">
            <tr>
                <td>
                    Rate Card Type
                </td>
                <td>
                    <asp:DropDownList ID="ddlRateCardType" runat="server">
                        <asp:ListItem>IATA</asp:ListItem>
                        <asp:ListItem>MKT</asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    Date
                </td>
                <td>
                    <asp:TextBox ID="txtDate" runat="server" Width="115px"></asp:TextBox>
                    <asp:ImageButton ID="btnDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                        ImageAlign="AbsMiddle" />
                    <asp:CalendarExtender ID="CEDate" Format="dd/MM/yyyy" TargetControlID="txtDate" PopupButtonID="btnDate"
                        runat="server" PopupPosition="BottomLeft">
                    </asp:CalendarExtender>
                </td>
                <td>
                    <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" OnClick="btnList_Click" />
                    &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" OnClick="btnClear_Click" />
                </td>
            </tr>
        </table>
    </div>
    
    <br />
    <h2>
            Rate Card Details
        </h2>
    <div class="divback">
       
        <asp:GridView ID="grdCountry" runat="server" AutoGenerateColumns="False" ShowFooter="True"
            Width="100%" Height="82px" onrowcommand="grdCountry_RowCommand">
            <Columns>
                <asp:BoundField DataField="serial" HeaderText="">
                <ItemStyle Width="0px" ForeColor="White" BorderColor="Black"/>
                </asp:BoundField>
                <asp:BoundField DataField="RateCardName" HeaderText="Rate Card Name" >
                <ItemStyle Width="150px" />
                </asp:BoundField>
                <asp:BoundField DataField="RateCardType" HeaderText="Rate Card Type" >
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="StartDate" HeaderText="Start Date">
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="EndDate" HeaderText="End Date">
                <ItemStyle Width="100px" />
                </asp:BoundField>
                <asp:BoundField DataField="Status" HeaderText="Status" >
                <ItemStyle Width="100px" />
                </asp:BoundField>
                
                <asp:ButtonField CommandName="Edit" Text="Edit">
                <ItemStyle Width="50px" />
                </asp:ButtonField>
                <asp:ButtonField CommandName="View" Text="View">
                <ItemStyle Width="50px" />
                </asp:ButtonField>
            </Columns>
            <HeaderStyle CssClass="titlecolr" />
            <RowStyle HorizontalAlign="Center" />
            <AlternatingRowStyle HorizontalAlign="Center" />
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
