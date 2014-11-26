<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmOperationTime.aspx.cs" Inherits="ProjectSmartCargoManager.frmOperationTime" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server" visible="True" enableviewstate="True">
    <title></title>
    
    <script type="text/javascript" language="javascript">
        
        function CloseWindow() {
            window.close();
        }
    
    </script>

    <link href="style/style.css" rel="stylesheet" type="text/css" />
</head>
<body style="background:#fff;">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
     <div id="Lightsplit" class="white_content">
        <div style="margin:10px;">
            <asp:Label ID="lblPnlError" runat="server" ForeColor="Red"></asp:Label>
            
               <h3><asp:Label ID="lblOperationDetails" Text="Actual Operation Time" runat="server" Font-Bold="true" Font-Size="Larger"></asp:Label>
                </h3> 
            <hr />
           
            <div style="width:350px;">
            <table width="100%" cellpadding="3" cellspacing="3">
                <tr>
                    <td>
                        Date
                    </td>
                    <td>
                        <asp:TextBox ID="txtOpsDate" runat="server" Width="80px"></asp:TextBox>
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageAlign="AbsMiddle" 
                             ImageUrl="~/Images/calendar_2.png" />
    
    </td><td style="width:70px;" >
                        <asp:TextBox ID="txtOpsTimeHr" runat="server" DataTextField="" Width="70px" 
                           ></asp:TextBox></td>
                           <td style="width:120px;" valign="bottom">
                            <asp:TextBox ID="txtOpsTimeMin" runat="server" DataTextField="" Width="70px"></asp:TextBox>
                            (HR:MI)</td>
                        
                        <td>                
                        <asp:CalendarExtender ID="txtOpsDate_CalendarExtender" Format="dd/MM/yyyy" runat="server" 
                             Enabled="True" TargetControlID="txtOpsDate" PopupButtonID="ImageButton1" PopupPosition="BottomLeft">
                         </asp:CalendarExtender>
                        
                             <asp:NumericUpDownExtender ID="txtDeptTimeHr_NumericUpDownExtender" 
                                        runat="server" Maximum="23" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtOpsTimeHr" Width="40" />
                                        
                        <asp:NumericUpDownExtender ID="txtDeptTimeMin_NumericUpDownExtender1" 
                                        runat="server" Maximum="59" Minimum="0" RefValues="" ServiceDownMethod="" 
                                        ServiceUpMethod="" TargetButtonDownID="" TargetButtonUpID="" 
                                        TargetControlID="txtOpsTimeMin" Width="40" />
                        </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btnOpsSave" runat="server" Text="Save" CssClass="button" onclick="btnOpsSave_Click" 
                              />
                    </td>
                    <td>&nbsp;</td>
                    <td>
                    <asp:Button ID="btnOpsCancel" runat="server" Text="Cancel" CssClass="button" 
                            onclick="btnOpsCancel_Click" />
                    </td>
              </tr>
            </table>
        </div>
	</div>
		<div id="fadesplit" class="black_overlay"></div>
    </div>
    </form>
</body>
</html>
