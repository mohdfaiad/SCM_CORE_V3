<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DateControl.ascx.cs" Inherits="ProjectSmartCargoManager.UserControls.DateControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<table>
    <tr>
        <td>
            <asp:TextBox ID="txtDate" runat="server" Width="90px" AutoPostBack="true" 
                                        ontextchanged="txtDate_TextChanged"/>
             <asp:ImageButton ID="imgDate" runat="server" ImageUrl="~/Images/calendar_2.png"
                                        ImageAlign="AbsMiddle" />
                                    <asp:CalendarExtender ID="extDate" TargetControlID="txtDate"
                                        PopupButtonID="imgDate" runat="server" PopupPosition="BottomLeft">
                                    </asp:CalendarExtender>
        </td>
    </tr>
</table>
