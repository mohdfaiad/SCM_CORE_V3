<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmUserActivityLog.aspx.cs" Inherits="ProjectSmartCargoManager.FrmUserActivityLog"  MasterPageFile="~/SmartCargoMaster.Master"%>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
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
    
    <%--<asp:UpdatePanel runat="server" ID="updtPnl">--%>
    <ContentTemplate>
    
    <div id="contentarea">
    <div class="msg">
     <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large" ForeColor="Red"></asp:Label>
    </div>
    <h1>
    User Login Log
    </h1>
    
     <div class="botline">
      <table >
       <tr>
        <td>
          From Date
        </td>
       <td>
                    
                    <asp:TextBox ID="txtFromDate" runat="server" CssClass="txtCommon" Width="95px"
                        ToolTip="Please Select Date" ></asp:TextBox>
                        <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFromDate"
                        ErrorMessage="*"></asp:RequiredFieldValidator>
                    <asp:CalendarExtender ID="txtFlightDate_CalendarExtender" runat="server" 
                        Enabled="True" TargetControlID="txtFromDate" Format="dd/MM/yyyy"  PopupButtonID="imgDate" >
          </asp:CalendarExtender>
       </td>
       <td>
         To Date
       </td>
       <td>
       
        <asp:TextBox ID="txtToDate" runat="server" CssClass="txtCommon" Width="95px"
                        ToolTip="Please Select Date" ></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/Images/calendar_2.png" />

                    <asp:CalendarExtender ID="txtToDate_CalendarExtender1" runat="server" 
                        Enabled="True" TargetControlID="txtToDate" Format="dd/MM/yyyy" PopupButtonID="ImageButton1" >
          </asp:CalendarExtender>
           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtToDate"></asp:RequiredFieldValidator>
       </td>
       <td>
        User
       </td>
       <td>
           <asp:TextBox ID="txtUser" runat="server"></asp:TextBox>
           <asp:AutoCompleteExtender ID="txtUser_AutoCompleteExtender" runat="server" 
            TargetControlID="txtUser" ServiceMethod="GetUser" MinimumPrefixLength="1"  Enabled="True" ServicePath="~/FrmUserActivityLog.aspx">
          </asp:AutoCompleteExtender>
       
       </td>
       <td>
        
       </td>
       
       </tr>
      <tr>
      <td><asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click" />
           <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" onclick="btnClear_Click" CausesValidation="false" />
      <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
              onclick="btnExport_Click" />
      </td></tr>
      </table>
      <table width="100%">
      <tr>
      <td>
          <asp:GridView ID="GrdUserLog" runat="server" Width="100%" AllowPaging="true" 
              PageSize="15" onpageindexchanging="GrdUserLog_PageIndexChanging">
           <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
          </asp:GridView>
      </td>
      </tr>
      </table>
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
   <%-- </asp:UpdatePanel>--%>
   
</asp:Content>