<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="EditAircraftEquipment.aspx.cs" Inherits="ProjectSmartCargoManager.EditAircraftEquipment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script language="javascript" type="text/javascript">
    function Save() {

        alert("Data Saved Successfuly");
    }
</script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
 <div id="contentarea">
        <h1>
            <img alt="" src="Images/aircarftedit.png"  style="vertical-align:5"/> </h1>
                        <asp:Label ID="lblstatusmsg" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
       
    <div class="divback"> 
    <table >     
        <tr>
            <td>
                <asp:Label ID="lblManuf" runat="server" Text="Manufacturer"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtManuf" runat="server"></asp:TextBox>
            &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="lblAcType" runat="server" Text="Aircraft Type"></asp:Label>
            </td>
            <td >
                <asp:TextBox ID="txtAcType" runat="server" ></asp:TextBox>
               
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblVer" runat="server" Text="Version"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtAcVer" runat="server" ></asp:TextBox>
            </td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="lblLWt" runat="server" Text="Landing Weight"></asp:Label>
            </td>
            <td class="style2">
                <asp:TextBox ID="txtLWt" runat="server"></asp:TextBox>
                Kg.</td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPassCap" runat="server" Text="Passenger Capacity"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPCap" runat="server"></asp:TextBox>
            &nbsp;Nos.</td>
            <td>
                &nbsp;</td>
            <td>
                <asp:Label ID="lblMTOW" runat="server" Text="MTOW" ></asp:Label>
            </td>
            <td >
                <asp:TextBox ID="txtMTOW" runat="server"></asp:TextBox>
                Kg.</td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="lblCarCap" runat="server" Text="Cargo Capacity"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCarCap" runat="server"></asp:TextBox>
                &nbsp;Kg.</td>
            <td >
                </td>
            <td >
                <asp:Label ID="lblTailNo" runat="server" Text="Tail Number"></asp:Label>
            </td>
            <td >
                <asp:TextBox ID="txtTailNo" runat="server" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="lblRwt0" runat="server" Text="Dimension/Pc."></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRl" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtRl_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtRl" WatermarkText="Length">
                </asp:TextBoxWatermarkExtender>
                <asp:TextBox ID="txtRb" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtRb_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtRb" WatermarkText="Breadth">
                </asp:TextBoxWatermarkExtender>
                <asp:TextBox ID="txtRh" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtRh_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtRh" WatermarkText="Height">
                </asp:TextBoxWatermarkExtender>
                <asp:DropDownList ID="ddldimension" runat="server" Width="50px">
                    <asp:ListItem Selected="True" Text="Cmt"></asp:ListItem>
                    <asp:ListItem Text="Inch"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td >
                &nbsp;</td>
            <td >
                <asp:Label ID="lblVolume" runat="server" Text="Volume"></asp:Label>
            </td>
            <td >
                            <asp:TextBox ID="txtVol" runat="server" Width="50px"></asp:TextBox>
                            
                
                <!--<asp:TextBox ID="txtVolLen" runat="server" Width="50px"></asp:TextBox>-->
                <!--<asp:TextBoxWatermarkExtender ID="txtVl_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtVolLen" 
                    WatermarkText="Length">
                </asp:TextBoxWatermarkExtender>-->
                <!--<asp:TextBox ID="txtVolBreadth" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtVolBreadth_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtVolBreadth" 
                    WatermarkText="Breadth">
                </asp:TextBoxWatermarkExtender>-->
                <!--<asp:TextBox ID="txtVolHeight" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtVolHeight_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtVolHeight" 
                    WatermarkText="Height">
                </asp:TextBoxWatermarkExtender>-->
                <!--<asp:DropDownList ID="ddldimensionVol" runat="server" Width="50px">
                    <asp:ListItem Selected="True" Text="Cmt"></asp:ListItem>
                    <asp:ListItem Text="Inc"></asp:ListItem>
                </asp:DropDownList>-->Cubic
                <asp:DropDownList ID="ddlVol" runat="server">
                <asp:ListItem Selected="True">Cmt</asp:ListItem>
                <asp:ListItem>Inch</asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td >
                <asp:Label ID="lblStatus" runat="server" Text="Status"></asp:Label>
            </td>
            <td>
               <asp:DropDownList  ID="ddlStatus" runat="server">
               <asp:ListItem Text="Active" Value="Active" ></asp:ListItem>
               <asp:ListItem Text="Inactive" Value="Inactive" ></asp:ListItem>
               </asp:DropDownList>
            </td>
            <td >
                &nbsp;</td>
            <td >
                <asp:Label ID="lblAIdentity" runat="server" Text="Identity"></asp:Label>
            </td>
            <td >
                            <asp:DropDownList ID="ddlAIdentity" runat="server">
                                <asp:ListItem>Narrow</asp:ListItem>
                                <asp:ListItem>Wide</asp:ListItem>
                            </asp:DropDownList>
            </td>
        </tr>
         <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
            </tr>
    </table>
    </div>
    <div id="fotbut">
        <asp:Button ID="btnSave" runat="server" Text="Save"  CssClass="button" 
            onclick="btnSave_Click"/>
            &nbsp &nbsp
        <asp:Button ID="btnBack" runat="server" onclick="btnBack_Click" Text="Back" 
            CssClass="button" />
    </div>    
   
   </div>
</asp:Content>
