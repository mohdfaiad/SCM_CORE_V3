<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FrmAuthenticateUser.aspx.cs" Inherits="ProjectSmartCargoManager.FrmAuthenticateUser" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
     <style>
		.black_overlay{
			display: none;
			position: fixed;
			top: 0%;
			left: 0%;
			width: 100%;
			height: 100%;
			background-color: black;
			z-index:1001;
			-moz-opacity: 0.8;
			opacity:.80;
			filter: alpha(opacity=80);
		}
		.white_content {
			display: none;
			position: fixed;
			top: 48%;
			left: 45%;
			width: 10%;
			height: 12%;
			padding: 16px;
			background-color: white;
			z-index:1002;
			overflow: auto;
		}
	</style>
     <style>
     #divwidth
        {
          width: 300px !important;    
        }
        #divwidth div
       {
        width: 300px !important;
       }
       
 </style>
     <script type="text/javascript">

         function SelectAllgrdAddRate(CheckBoxControl)
          {
              for (i = 0; i < document.forms[0].elements.length; i++)
              {
                  if (document.forms[0].elements[i].name.indexOf('check') > -1)
                  {
                     document.forms[0].elements[i].checked = CheckBoxControl.checked;
                 }
             }
         }
         function closePage()
          {
             window.close();
         }
            
            
function disableBackButton()
{
window.history.forward();
}
setTimeout("disableBackButton()", 0);

    </script>
    <link href="style/style.css" rel="stylesheet" type="text/css" />
    </head>
<body class="divback">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <div>
      
    </div>

  <div >
   <table width="50%">
      <tr>
       <td>
        Enter the verification code sent to your Mobile number
       </td>
       <td>
           <asp:TextBox ID="txtauthenicationcode" runat="server" MaxLength="5" ></asp:TextBox>
       </td>
       <td>
           <asp:Button ID="btnVerify" runat="server" Text="Verify" CssClass="button" onclick="btnVerify_Click" 
               />
       </td>
      </tr>
      <tr>
       <td>
           <asp:CheckBox ID="chktrust" runat="server" Text="Trust this computer"  />
       </td>
      </tr>
      <tr>
      <td>
          <asp:HyperLink ID="hnkcancel" runat="server" NavigateUrl="~/Login.aspx" >Cancel</asp:HyperLink>
      </td>
      </tr>
     </table>
		</div>
		<%--<div id="fade" class="black_overlay"></div>--%>





    </form>
</body>
</html>
