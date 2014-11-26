<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="AirCraftTonnageMaster.aspx.cs" Inherits="ProjectSmartCargoManager.AirCraftTonnageMaster" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:ToolkitScriptManager ID="TSM" runat="server">
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
            Sectorwise Capacity
         </h1>
             <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
          <div class="botline">  
<asp:Panel ID="pnlNew" runat="server">
<table width="100%" border="0" cellpadding="3" cellspacing="3">
<tr>
<td>
    Origin*
</td>
    
<td>
<!--<asp:TextBox ID="txtOrigin" runat="server"></asp:TextBox>-->
<asp:DropDownList ID="ddlOrigin" runat="server">
    </asp:DropDownList>
</td>
<td>
        Destination*
</td>
<td>
    <!--<asp:TextBox ID="txtDestination" runat="server"></asp:TextBox>-->
    <asp:DropDownList ID="ddlDest" runat="server">
    </asp:DropDownList>
    </td>
    
        <td>
            Valid From</td>
        <td>
        <asp:TextBox ID="txtvalidfrom" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="CalendarExtender3" runat="server" Enabled="True" TargetControlID="txtvalidfrom"  Format="yyyy-MM-dd">
                                    </asp:CalendarExtender>
                                    &nbsp;</td>
        <td>
            Valid To</td>
        <td>
            <asp:TextBox ID="txtvalidto" runat="server"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" Enabled="True" TargetControlID="txtvalidto"  Format="yyyy-MM-dd">
                                    </asp:CalendarExtender>
                                    &nbsp;</td>
         
         
    </tr>
    <tr>
        <td>
            Weight</td>
        <td>
            <asp:TextBox ID="txtWeight" runat="server"></asp:TextBox>
        </td>
        <td>
            Volume</td>
        <td>
            <asp:TextBox ID="txtVolume" runat="server"></asp:TextBox></td><td>
            Cubic</td><td>
            <asp:DropDownList ID="ddlVolMeasure" runat="server">
            <asp:ListItem Selected="True">Cmt</asp:ListItem>
            <asp:ListItem>Inch</asp:ListItem>
            </asp:DropDownList>
        </td>
    

<td>
Active
</td>
<td>
    <asp:CheckBox ID="chkActive" runat="server" />
</td>
</tr>
   
        
        <tr>
        <td>
    Aircraft Type
</td>
<td>
    <asp:DropDownList ID="ddlAircraftType" runat="server"></asp:DropDownList>    
</td>
            <td colspan="6" align="right">
                <asp:Button ID="btnSave" runat="server" CssClass="button" 
                    onclick="btnSave_Click" Text="Save" />
                
                <asp:Button ID="btnClear" runat="server" CssClass="button" Text="Clear" 
                    onclick="btnClear_Click" />
                &nbsp;&nbsp;
                <asp:Button ID="btnList" runat="server" CssClass="button" Text="List" 
                    onclick="btnList_Click" />
            
            </td>
        </tr>
   

</table>
</asp:Panel>  </div> 
<div style="float:left">
<asp:Panel ID="pnlGrid"  runat="server">
    <asp:GridView ID="grvArcraftTonnageList" runat="server" ShowFooter="false" Width="100%" 
            AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
            CellPadding="2" CellSpacing="3" PageSize="10" AllowPaging="True" 
        onrowcommand="grvArcraftTonnageList_RowCommand" 
        onrowediting="grvArcraftTonnageList_RowEditing" 
        onpageindexchanging="grvArcraftTonnageList_PageIndexChanging"  >
           
            <Columns>
            <asp:TemplateField HeaderText="Serial Number" HeaderStyle-Wrap="true" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblSrNo" runat="server" Text = '<%# Eval("SerialNumber") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Aircraft Type" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblAircraftType" runat="server" Text = '<%# Eval("AircraftType") %>'/>
                    </ItemTemplate>
            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Valid From" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblValidFrom" runat="server" Text = '<%# Eval("ValidFrom") %>'/>
                    </ItemTemplate>

            <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Valid To" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblValidTo" runat="server" Text = '<%# Eval("ValidTo") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblOrigin" runat="server" Text = '<%# Eval("Origin") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Destination" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblDestination" runat="server" Text = '<%# Eval("Destination") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Weight" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblWeight" runat="server" Text = '<%# Eval("Weight") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Volume" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblVolume" runat="server" Text = '<%# Eval("Volume") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Volume Unit" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblVolUnit" runat="server" Text = '<%# Eval("VolUnit") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
             <ItemTemplate>
             <asp:Label ID="lblStatus" runat="server" Text = '<%# Eval("IsActive") %>'/>
             </ItemTemplate>
             <HeaderStyle Wrap="True"></HeaderStyle>
             </asp:TemplateField>
             
            <asp:ButtonField CommandName="Edit" Text="Edit">
                                    <ItemStyle Width="50px" />
                                </asp:ButtonField>
            </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
</asp:Panel>
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
