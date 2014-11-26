<%@ Page Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="frmCustomsAuditTrail.aspx.cs" Inherits="ProjectSmartCargoManager.frmCustomsAuditTrail" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    
 </asp:Content> 
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>

    <script language="javascript" type="text/javascript">
        function printPartOfPage(elementId) {
            //            var printContent = document.getElementById(elementId);
            //            var windowUrl = 'about:blank';
            //            var uniqueName = new Date();
            //            var windowName = 'Print' + uniqueName.getTime();
            //            var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

            //            printWindow.document.write(printContent.innerHTML);
            //            printWindow.document.close();
            //            printWindow.focus();
            //            printWindow.print();
            //            printWindow.close();
            this.print();
        }
        
        
        
    </script>

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
       Customs Audit Trail
    </h1> 
  
   <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"></asp:Label>
       
  <div class="botline"> 
  
  <table width="37%">
   <tr>
    <td>
        AWBNumber
    </td>
    <td>
     <asp:TextBox ID="txtprefix" runat="server" Width="29px" MaxLength="4"></asp:TextBox>
     <asp:TextBox ID="txtAWBNumber" runat="server" Width="100px" MaxLength="11"></asp:TextBox>
     </td>
    <td>
        <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" onclick="btnList_Click"/>
        <asp:Button ID="BtnClear" runat="server" CssClass="button" Text="Clear" onclick="BtnClear_Click"/>
    </td>
   </tr>
  </table>  
  
  </div> 
  <div style="overflow:auto; float:left; width:100%;">
    <asp:GridView ID="grdCustomsAudit" runat="server" BackColor="White" 
              BorderColor="#E7E7FF" BorderStyle="Solid" BorderWidth="5px" CellPadding="3" 
              ToolTip="CustomsDetails" Width="100%">
              <EditRowStyle CssClass="grdrowfont" />
              <FooterStyle CssClass="grdrowfont" />
              <HeaderStyle CssClass="titlecolr" />
              <RowStyle CssClass="grdrowfont" />
          </asp:GridView>
  </div>
  
  </div>
<div ID="msglight" class="white_contentmsg">
          <table>
              <tr>
                  <td align="center" width="5%">
                      <br />
                      <img src="Images/loading.gif" />
                      <br />
                      <asp:Label ID="msgshow" runat="server"></asp:Label>
                  </td>
              </tr>
          </table>
      </div>
      <div ID="msgfade" class="black_overlaymsg">
      </div>
    </ContentTemplate>
    </asp:UpdatePanel>


  </asp:Content> 

