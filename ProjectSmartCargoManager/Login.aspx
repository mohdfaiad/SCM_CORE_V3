<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" MasterPageFile="~/VisibleCargo.Master" Inherits="ProjectSmartCargoManager.Login" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <%--<script language="JavaScript"> 
  function clearText() {
      document.getElementById("<%=txtTrack.ClientID%>").title = 'hi';
            } 
            </script>--%>
<script type="text/javascript" src="http://code.jquery.com/jquery-latest.min.js"></script>
<script type="text/javascript" src="http://www.modernizr.com/downloads/modernizr-latest.js"></script>
<script type="text/javascript" src="scripts/placeholder.js"></script>
            
<script type="text/javascript">
              window.history.forward();
              function noBack() { window.history.forward(); }

              function openPopUp() {
                  window.open('FrmAWBTracking.aspx', null, 'height=600, width=1024,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no ');
              }
              function openPopUp1() {
                  window.open('FrmAWBTrackFromLogin.aspx', null, 'height=600, width=1050,status= no, resizable= no, scrollbars=yes, toolbar=no,location=no,menubar=no ')
              }


              function disableBackButton() {
                  window.history.forward();
              }
              setTimeout("disableBackButton()", 0);

              function ValidateUser() {
                  var UserID = document.getElementById("<%= txtUserName.ClientID%>").value;
                  if (UserID == "") {
                      alert('Enter User Name');
                      return false;
                  }

                  var HdnUsers = document.getElementById("<%= hdnUserList.ClientID%>").value;
                  var UserList = HdnUsers.split(',');
                  
                  var arraycontains = UserList.indexOf(UserID);
                  if (arraycontains < 0) {
                      alert('Enter Valid User Name');
                      return false;
                  }
                  return true;
              }
</script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div class="container" style="left:348px; top:339px; position:absolute;">
        <!--Gray box 1 starts here-->
       <%-- <div class="grayBox">
    <div class="grayBoxMiddle">
      <table width="100%" border="0">
        <tr>
          <td><h4>Airline Cargo &amp; Courier Logistics System</h4></td>
        </tr>
        <tr>
          <td><p>QID’s Airline Cargo and Courier Logistics Tracking (AC@CT) System is the air cargo industry’s first transformative barcode compatible. <a href="#"><span>More</span></a></p></td>
        </tr>
      </table>
    </div>
    <div class="grayBoxBot"></div>
  </div>--%>
  <!--Gray box 1 ends here-->
  
  <!--Gray box 2 starts here-->
  <div class="grayBox">
    <div class="grayBoxMiddle" style="height:155px"  >
      
        <table width="100%" border="0">
          <tr>
            <td colspan="2"><h4 style="color:#000000; font-weight:bold;">Login to SmartKargo</h4></td>
          </tr>
          <tr>
            <td width="75%">
                <asp:TextBox ID="txtUserName" runat="server" CssClass="inputbg" 
                    ToolTip="Login ID"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtUserName_TextBoxWatermarkExtender"  WatermarkText="User Name" 
                    runat="server" TargetControlID="txtUserName">
                </asp:TextBoxWatermarkExtender>
              </td>
            <td width="25%">
                &nbsp;</td>
          </tr>
          <tr>
            <td>
                <asp:TextBox ID="txtPwd" runat="server" CssClass="inputbg" TextMode="Password" 
                    ToolTip="Password"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtPwd_TextBoxWatermarkExtender" WatermarkText="Password" 
                    runat="server" TargetControlID="txtPwd">
                </asp:TextBoxWatermarkExtender>
              </td>
            <td>
                    
            
            <%--<a href="#" style=" background:url('../images/buton.png') repeat-x; 
border-radius:6px; color:#FFFFFF; font-weight:bold;font-family:Calibri, Arial, Helvetica, sans-serif;
    font-size:13px;
	height: 30px; padding:7px;">
                <asp:Button ID="btnLogin" runat="server" onclick="btnLogin_Click" 
                    Text="Login" Font-Names="Verdana" Font-Size="Small"  ForeColor="#ffffff" /></a>--%>
              </td>
          </tr>
          
          
          <tr>
            <td>
                <asp:TextBox ID="txtStation" runat="server" CssClass="inputbg"  
                    ToolTip="Station Code"></asp:TextBox>
                    <asp:TextBoxWatermarkExtender ID="txtStation_TextBoxWatermarkExtender" WatermarkText="Station Code"  
                    runat="server" TargetControlID="txtStation">
                </asp:TextBoxWatermarkExtender>
                <%--    <asp:AutoCompleteExtender ID="txtStation_AutoCompleteExtender" runat="server" ServiceMethod="GetStation" MinimumPrefixLength="1"  Enabled="True"  ServicePath="~/Home.aspx" 
                    TargetControlID="txtStation">
                </asp:AutoCompleteExtender>--%>
              </td>
            <td>
                    
            
            <a href="#" style=" background:url('images/buton.png') repeat-x; border-radius:6px; color:#FFFFFF; font-weight:bold;font-family:Calibri, Arial, Helvetica, sans-serif;
    font-size:13px;
	height: 30px; padding:7px;">
                <asp:Button ID="Button1" runat="server" onclick="btnLogin_Click" 
                    Text="Login" Font-Names="Verdana" Font-Size="Small"  ForeColor="#ffffff" /></a>
              </td>
          </tr>
          
          <tr>
          <td colspan="2">
              <asp:LinkButton ID="lnkFrgtPwd" runat="server" ForeColor="Blue" 
               OnClientClick="javascript:return ValidateUser();"   onclick="lnkFrgtPwd_Click">Forgot Password?</asp:LinkButton>
          </td>
          </tr>
      </table>
      
    </div>
    <div class="grayBoxBot"></div>
    
  </div>
  <!--Gray box 2 ends here-->
  
  <!--Gray box 3 starts here-->
  <div class="grayBox">
  
    <div class="grayBoxMiddle" style="height:155px"  >
     
        <table width="70%" border="0">
          <tr>
            <td colspan="2"><h4 style="color:#000000; font-weight:bold;"> Track your Bill</h4></td>
          </tr>
          <tr>
           <td valign="top">
                <asp:TextBox ID="txtPrefix"  runat="server" CssClass="inputbg60" MaxLength="4" 
                    ></asp:TextBox>
              </td>
            <td valign="top" style="margin:0">
                <asp:TextBox ID="txtTrack"  runat="server" CssClass="inputbgmulti" MaxLength="200"   
                    ontextchanged="txtTrack_TextChanged1" TextMode="MultiLine" 
                    BorderStyle="None" Height="45px" 
                    ToolTip="Enter AWB no.If Multiple AWB's Enter comma separated.eg:98989898,98989899" 
                    Width="140px"></asp:TextBox>
              </td>
            <td >
            <a href="#" style=" background:url('images/buton.png') repeat-x; 
border-radius:6px; color:#FFFFFF; font-weight:bold;font-family:Calibri, Arial, Helvetica, sans-serif;
    font-size:13px;
	height: 30px; padding:7px;">
                <asp:Button ID="btnTrack" runat="server" onclick="btnTrack_Click" 
                    Text="Track" Font-Names="Verdana" Font-Size="Small"  ForeColor="#ffffff" 
                    ondatabinding="btnTrack_DataBinding" /></a>
  
            
            
            </td>
          </tr>
          <tr>
            <td colspan="2" class="style7" align="left" rowspan="1" valign="top">
<asp:Label ID="lblMsg" runat="server" Text="" Height="16px"></asp:Label>
            </td>
          </tr>
        </table>
    
    </div>
    <div class="grayBoxBot"></div>
  </div>
  
  <%--<div style="height:20px">
  <br />
  <br />
  </div>  
  --%>
  <div class="clearBoth">
   
      
        </div>
<%--  <div class="mainGraphic"><img src="images/main_graphic.png" alt="Smart Cargo" title="Smart Cargo" border="0" align="middle" /></div>
--%></div>

    <asp:HiddenField ID="hdnUserList" runat="server" />

</asp:Content>