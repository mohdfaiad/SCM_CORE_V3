<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmBookingfromTmp.aspx.cs" Inherits="ProjectSmartCargoManager.frmBookingfromTmp" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title></title>
   
    <link href="style/style.css" rel="stylesheet" type="text/css" />
</head>
<body style="background:#fff;">
    
    
     <form id="form1" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
        </asp:ToolkitScriptManager>
     <div id="msgfade" class="black_overlaymsg">
     </div>
     <div id="Lightsplit" class="white_contentnew">
         <div style="height: 523px; width: 944px">
             <fieldset>
                 <legend>Other Details</legend>
                 <table>
                     <tr>
                         <td>
                             Flight</td>
                         <td colspan="3">
                             <asp:TextBox ID="txtFlightCode" runat="server" CssClass="styleUpper" 
                                 MaxLength="2" Width="45px"></asp:TextBox>
                             <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" 
                                 TargetControlID="txtFlightCode" WatermarkText="Prefix" />
                             <asp:TextBox ID="txtFlightID" runat="server" AutoPostBack="false" MaxLength="4" 
                                 Width="55px"></asp:TextBox>
                             <asp:RequiredFieldValidator ID="ReqFltno" runat="server" 
                                 ControlToValidate="txtFlightID" ErrorMessage="*"></asp:RequiredFieldValidator>
                             <asp:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" 
                                 TargetControlID="txtFlightID" WatermarkText="Flight ID" />
                             <asp:TextBox ID="TextBoxdate" runat="server" AutoPostBack="false" Width="85px"></asp:TextBox>
                             <asp:ImageButton ID="imgDate" runat="server" ImageAlign="AbsMiddle" 
                                 ImageUrl="~/Images/calendar_2.png" />
                             <asp:CalendarExtender ID="TextBoxdate_CalendarExtender" runat="server" 
                                 Enabled="True" Format="dd/MM/yyyy" PopupButtonID="imgDate" 
                                 TargetControlID="TextBoxdate">
                             </asp:CalendarExtender>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Commdity</td>
                         <td>
                             <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                         </td>
                         <td>
                         </td>
                         <td>
                             <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                     <tr>
                         <td>
                             Pices</td>
                         <td>
                             <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                         </td>
                         <td>
                             Weight</td>
                         <td>
                             <asp:TextBox ID="txtweight" runat="server"></asp:TextBox>
                         </td>
                     </tr>
                 </table>
             </fieldset>
             <fieldset>
                 <legend>Recurrence pattern</legend>
                 <table>
                     <tr>
                         <td>
                             <asp:RadioButton ID="rbdnDaily" runat="server" Text="Daily" />
                             <br />
                             <asp:RadioButton ID="rbdnWeekly" runat="server" Text="Weekly" />
                             <br />
                             <asp:RadioButton ID="rbdnMonthly" runat="server" Text="Monthly" />
                             <br />
                             <asp:RadioButton ID="rbdnyearrly" runat="server" Text="yearrly" />
                         </td>
                         <td>
                         </td>
                         <td valign="top">
                             Recur every
                             <asp:TextBox ID="txtrecur" runat="server" Width="30"></asp:TextBox>
                             on:
                             <br />
                             <asp:CheckBox ID="chksunday" runat="server" Text="Sunday" />
                             <asp:CheckBox ID="chkmonday" runat="server" Text="Monday" />
                             <asp:CheckBox ID="ChkTuesday" runat="server" Text="Tuesday" />
                             <asp:CheckBox ID="ChkWednesday" runat="server" Text="Wednesday" />
                             <asp:CheckBox ID="ChkThursday" runat="server" Text="Thursday" />
                             <asp:CheckBox ID="ChkFriday" runat="server" Text="Friday" />
                             <asp:CheckBox ID="ChkSaturday" runat="server" Text="Saturday" />
                         </td>
                     </tr>
                 </table>
             </fieldset>
             <fieldset>
                 <legend>Range of recurrence</legend>
                 <table>
                     <tr>
                         <td valign="top">
                             Start
                             <asp:TextBox ID="txtFromDate" runat="server" Width="115px"></asp:TextBox>
                             <asp:ImageButton ID="btnFromDate" runat="server" ImageAlign="AbsMiddle" 
                                 ImageUrl="~/Images/calendar_2.png" />
                             <asp:CalendarExtender ID="CEFromDate" runat="server" Format="yyyy-MM-dd" 
                                 PopupButtonID="btnFromDate" PopupPosition="BottomLeft" 
                                 TargetControlID="txtFromDate">
                             </asp:CalendarExtender>
                         </td>
                         <td>
                             <asp:RadioButton ID="RadioButton1" runat="server" Text="No end date" />
                             <br />
                             <asp:RadioButton ID="RadioButton2" runat="server" Text="end after:" />
                             <asp:TextBox ID="Textbox31" runat="server" Width="30"></asp:TextBox>
                             occurrences<br />
                             <asp:RadioButton ID="RadioButton3" runat="server" Text="end by:" />
                             <asp:TextBox ID="TextBox4" runat="server" Width="115px"></asp:TextBox>
                             <asp:ImageButton ID="ImageButton11" runat="server" ImageAlign="AbsMiddle" 
                                 ImageUrl="~/Images/calendar_2.png" />
                             <asp:CalendarExtender ID="CalendarExtender1" runat="server" Format="yyyy-MM-dd" 
                                 PopupButtonID="btnFromDate" PopupPosition="BottomLeft" 
                                 TargetControlID="txtFromDate">
                             </asp:CalendarExtender>
                             <br />
                         </td>
                     </tr>
                 </table>
             </fieldset>
             <asp:Button ID="Button1" runat="server" CssClass="button" Text="Save" />
             <asp:Button ID="Button12" runat="server" CssClass="button" Text="Cancel" />
         </div>
     </div>
     <div id="fadesplit" class="black_overlaynew">
     </div>
    <%--<asp:Button ID="Button1" runat="server" onclick="Button1_Click" Text="Back" />--%>
     </form>
    
    
    
</body>
</html>
