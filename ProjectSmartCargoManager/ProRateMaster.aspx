<%@ Page Title="" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="ProRateMaster.aspx.cs" Inherits="ProjectSmartCargoManager.ProRateMaster" %>
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
            Pro-Rate Factor Master
         </h1>
         
                <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Red"></asp:Label>
           
 
<asp:Panel ID="pnlNew" runat="server">
<div class="botline">
<table width="100%" border="0">
<tr>
<td>
    Origin*
</td>
<td>
   <asp:DropDownList ID="ddlOrigin" runat="server"></asp:DropDownList></td><td>
       Destination*
</td>
<td>
<asp:DropDownList ID="ddlDestination" runat="server"></asp:DropDownList>
    </td>
       <td>
             Prorate Factor*</td>
            <td>
                <asp:TextBox ID="txtProFact" runat="server" Width="55px" style="text-align:right"></asp:TextBox> 
                <asp:RequiredFieldValidator ID="req_valid_txtProFact" runat="server" ErrorMessage="*" 
                ControlToValidate="txtProFact"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="valid_txtProFact" runat="server" ErrorMessage="Only Digits"
ControlToValidate="txtProFact" ValidationExpression="^\d+\.?\d*$"></asp:RegularExpressionValidator>
                </td>
            <td>
               Constr Factor</td>
            <td>
                <asp:TextBox ID="txtConstr" runat="server" Width="55px" style="text-align:right"></asp:TextBox></td>
            <td>
               Valid From
            </td>
            <td>
                <asp:TextBox ID="txtValidFrm" runat="server" Width="80px"></asp:TextBox>
                <asp:CalendarExtender ID="ext_validfrm" TargetControlID="txtValidFrm" runat="server"></asp:CalendarExtender> 
            </td>
            <td>
             Valid To 
            </td>
            <td>
                <asp:TextBox ID="txtValidTo" runat="server" Width="80px"></asp:TextBox>
                <asp:CalendarExtender ID="ext_validto" TargetControlID="txtValidTo" runat="server"></asp:CalendarExtender>
                </td>
                <td>
               Active
                </td>
                <td>
                    <asp:CheckBox ID="chkAct" runat="server"/></td>
        </tr>
        </table>
 <table style="float:right;">
        <tr>
         <td>
         <asp:Button ID="btnSave" runat="server" CssClass="button" 
            Text="Save" onclick="btnSave_Click" />
        
        <asp:Button ID="btnClear" runat="server" CssClass="button" 
            onclick="btnClear_Click" Text="Clear" CausesValidation="false"/>
        
        <asp:Button ID="btnList" runat="server" CssClass="button" 
            onclick="btnList_Click" Text="List" CausesValidation="false" />
    </td>
    <td>
        <%--<asp:Button ID="btnImoprt" runat="server" Text="Import" CssClass="button" 
            onclick="btnImoprt_Click" />--%>
        <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="button" 
            onclick="btnExport_Click"/>
    </td>
        </tr>
   
    </table>
   </div>
</asp:Panel>
<asp:Panel ID="pnlGrid"  runat="server">
<div class="ltfloat">
    <asp:GridView ID="grdProRateList" runat="server" ShowFooter="false" Width="100%" 
 AutoGenerateColumns="False" CellPadding="2" 
 CellSpacing="3" PageSize="10" AllowPaging="True" OnPageIndexChanging="grdProRateList_PageIndexChanging"
 OnRowCommand="grdProRateList_RowCommand" OnRowEditing="grdProRateList_RowEditing"
 AlternatingRowStyle-CssClass="AltRowStyle" CssClass="GridViewStyle" HeaderStyle-CssClass="HeaderStyle"  
PagerStyle-CssClass="PagerStyle" RowStyle-CssClass="RowStyle" SelectedRowStyle-CssClass="SelectedRowStyle">
           
            <Columns>
            <asp:TemplateField HeaderText="ID" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lblId" runat="server" Text = '<%# Eval("ID") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Origin" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblOriginCode" runat="server" Text = '<%# Eval("OriginCode") %>'/>
             </ItemTemplate>
            </asp:TemplateField>
             
            <asp:TemplateField HeaderText="Origin Name" Visible="false" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblOriginName" runat="server" Text = '<%# Eval("OrgName") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Origin Country" Visible="false" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
            <asp:Label ID="lblOriginCountry" runat="server" Text = '<%# Eval("OrgCountry") %>'/>
            </ItemTemplate>
            </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Destiantion" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblDestinationCode" runat="server" Text = '<%# Eval("DestCode") %>'/>
             </ItemTemplate>
             </asp:TemplateField>   
             
             <asp:TemplateField HeaderText="Destination Name" ItemStyle-HorizontalAlign="Center" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lblDestName" runat="server" Text = '<%# Eval("DestName") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Destination Country" Visible="false" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblDestCountry" runat="server" Text = '<%# Eval("DestCountry") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
                         
             <asp:TemplateField HeaderText="ProRate Factor" ItemStyle-HorizontalAlign="Right">
             <ItemTemplate>
             <asp:Label ID="lblProRateFactor" runat="server" Text = '<%# Eval("ProrateFactor") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Constructed Factor" ItemStyle-HorizontalAlign="Right" Visible="false">
             <ItemTemplate>
             <asp:Label ID="lblConstrFactor" runat="server" Text = '<%# Eval("ConstrFactor") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Valid From" Visible="false" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblValidFrm" runat="server" Text = '<%# Eval("ValidFrom") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
             
             <asp:TemplateField HeaderText="Valid To" Visible="false" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblValidTo" runat="server" Text = '<%# Eval("ValidTo") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
            
             <asp:TemplateField HeaderText="Active" Visible="false" ItemStyle-HorizontalAlign="Center">
             <ItemTemplate>
             <asp:Label ID="lblact" runat="server" Text = '<%# Eval("isActive") %>'/>
             </ItemTemplate>
             </asp:TemplateField>
            
            <asp:ButtonField CommandName="Edit" Text="Edit">
            <ItemStyle Width="50px"/>
            </asp:ButtonField>
            
            <asp:ButtonField CommandName="DeleteRecord" Text="Delete">
            <ItemStyle Width="50px"/>
            </asp:ButtonField>
            </Columns>
             <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
    </asp:GridView>
    </div>
</asp:Panel>
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
    <Triggers>
    <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    </asp:UpdatePanel>

</asp:Content>
