<%--

2012-04-05  vinayak
2012-07-24  vinayak

--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_CargoDimensions.aspx.cs" Inherits="ProjectSmartCargoManager.GHA_CargoDimensions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dimensions</title>

    <script language="javascript" type="text/javascript">
        function Error() {

            alert("Please check the values");

        }
        function NotInserted() {

            alert("Record Not Inserted Please try Again..");

        }


        function CloseWindow(remainingPcs, remainingWt, recievedPcs, recievedWt) {
           // opener.document.getElementById('<%= Request["RemainingPcsTxt"] %>').value = '' + remainingPcs;
            //opener.document.getElementById('<%= Request["RemainingWtTxt"] %>').value = '' + remainingWt;
            opener.document.getElementById('<%= Request["AccpPcsID"] %>').value = '' + recievedPcs;
            opener.document.getElementById('<%= Request["AccpWtID"] %>').value = '' + recievedWt;
            window.close();
        } 

        function SelectheaderCheckboxes(headerchk) {
            var gvcheck = document.getElementById("<%=grdAcceptance.ClientID %>");
            var i;
            //Condition to check header checkbox selected or not if that is true checked all checkboxes
            if (headerchk.checked) {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = true;
                }
            }
            //if condition fails uncheck all checkboxes in gridview
            else {
                for (i = 0; i < gvcheck.rows.length; i++) {
                    var inputs = gvcheck.rows[i].getElementsByTagName('input');
                    inputs[0].checked = false;
                }
            }
        }
        
    </script>

</head>
<body class="divback">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ToolkitScriptManager>
    <div style="width: 750px; padding-left: 20px;">
    <br />
           <asp:Label ID="LBLStatus" runat="server" Text="" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
        <br />
        <table style="height: 30px" width="730px">
            <tr>
             <td style="font-size: medium;font-weight:bold;">
                    AWB No:
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblAWB" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="font-size: medium;font-weight:bold;">
                    Commodity
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblCommodity" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;font-weight:bold;">
                    
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="LBLPcsCount" runat="server" Visible="false"></asp:Label>
                </td>
                <td style="font-size: medium;font-weight:bold;">
                    
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblGrossWt" runat="server"  Visible="false"></asp:Label>
                </td>
                </tr>
                <tr>
                <%--<td width="90px" valign="top" style="font-size: medium;">
                    Unit
                </td>
                <td valign="top">
                    <asp:DropDownList ID="ddlUnit" runat="server" AutoPostBack="false">
                        <asp:ListItem Text="Inches"></asp:ListItem>
                        <asp:ListItem Text="Cms" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Meters"></asp:ListItem>
                    </asp:DropDownList>
                </td>--%>
                <td style="font-size: medium;font-weight:bold;">
                    Flight No
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblFlightNo" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;font-weight:bold;">
                    Flight Date
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblFlightDate" runat="server"></asp:Label>
                </td>
                </tr>                                    
        </table>
        <asp:UpdatePanel runat="server" ID="UPDimension">
            <ContentTemplate>
             
       <div style="overflow:auto;height:200px; width:750px; border:solid 1px #ccc;">
       <asp:GridView ID="grdAcceptance" runat="server" AutoGenerateColumns="false" 
               Width="100%" onrowdatabound="grdAcceptance_RowDataBound"  
     >
     <Columns>
     
     <asp:TemplateField Visible="false">
      <HeaderTemplate>
           <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="javascript:SelectheaderCheckboxes(this);" />
      </HeaderTemplate>
     <ItemTemplate>
         <asp:CheckBox ID="chkAccept" runat="server" />
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Sr No" Visible="false">
     <ItemTemplate>
         <asp:Label ID="lblSrno" runat="server" Text='<%#Eval("SrNo")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Pieces Id">
     <ItemTemplate>
       <asp:Label ID="lblPcsId" runat="server" Text='<%#Eval("PieceId")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Length">
     <ItemTemplate>
         <asp:TextBox ID="txtLgth" runat="server" Text='<%#Eval("Length")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Breadth">
     <ItemTemplate>
     <asp:TextBox ID="txtBreadth" runat="server" Text='<%#Eval("Breadth")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Height">
     <ItemTemplate>
     <asp:TextBox ID="txtHeight" runat="server" Text='<%#Eval("Height")%>' Width="50px" AutoPostBack="true" OnTextChanged="CalculateVolume"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Volume">
     <ItemTemplate>
     <asp:TextBox ID="txtVol" runat="server" Text='<%#Eval("Volume")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Weight">
     <ItemTemplate>
     <asp:TextBox ID="txtWt" runat="server" Text='<%#Eval("Weight")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Copy">
     <ItemTemplate>
         <asp:TextBox ID="txtCopy" runat="server" Width="40px"></asp:TextBox>
         <asp:Button ID="btnCopy" runat="server" Text="Copy" CssClass="button" OnClick="CopyDimensions" />
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Scale Weight">
     <ItemTemplate>
     <asp:TextBox ID="txtScaleWt" runat="server" Width="50px" Text='<%#Eval("ScaleWeight")%>' AutoPostBack="true" OnTextChanged="CalculateScaleWeight"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     
     
      <asp:TemplateField HeaderText="Piece Type">
     <ItemTemplate>
         <%--<asp:TextBox ID="txtPcType" runat="server" Text='<%#Eval("PieceType")%>' Width="50px"></asp:TextBox>--%>
         <asp:DropDownList ID="ddlPieceType" runat="server" Width="75px" CssClass="grdrowfont">
          <asp:ListItem Selected="True">Bulk</asp:ListItem>
          <asp:ListItem>Bags</asp:ListItem>
          <asp:ListItem>ULD</asp:ListItem>                                                        
          </asp:DropDownList>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField HeaderText="Bag#">
     <ItemTemplate>
         <asp:TextBox ID="txtBagNo" runat="server" Text='<%#Eval("BagNo")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     <asp:TemplateField HeaderText="ULD#">
     <ItemTemplate>
     <asp:TextBox  ID="txtULD"  runat="server" Text='<%#Eval("ULDNo")%>' Width="70px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
      <asp:TemplateField HeaderText="Location" Visible="false">
     <ItemTemplate>
         <asp:TextBox ID="txtLocation" runat="server" Text='<%#Eval("Location")%>' Width="50px"></asp:TextBox>
     </ItemTemplate>
     </asp:TemplateField>
     
     <asp:TemplateField HeaderText="Is Accepted" Visible="false">
     <ItemTemplate>
       <asp:Label ID="lblIsAccp" runat="server" Text='<%#Eval("isAccepted")%>'></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
        <asp:TemplateField Visible="false">
     <ItemTemplate>
     <asp:Label ID="lblOrigin" runat="server" Text='<%#Eval("Origin")%>' Width="50px"></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
      <asp:TemplateField Visible="false">
     <ItemTemplate>
     <asp:Label ID="lblDestination" runat="server" Text='<%#Eval("Destination")%>' Width="50px"></asp:Label>
     </ItemTemplate>
     </asp:TemplateField>
     
     </Columns>
      <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
    </asp:GridView>
                </div>
        
                <div style="display:none">
                <h2>Acceptance Task Performed</h2>
<table width="100%" cellpadding="3" cellspacing="6">
<tr>
<td>
    <asp:CheckBox ID="chkTamper" runat="server" Text="Tamper"/>
    <asp:CheckBox ID="chkPackaging" runat="server" Text="Packaging" />
    <asp:CheckBox ID="chkVisual" runat="server" Text="Visual"/>
    <asp:CheckBox ID="chkSmell" runat="server" Text="Smell"/>
    <asp:CheckBox ID="chkDGR" runat="server" Text="DGR"/>
    <asp:CheckBox ID="chkLiveAnimal" runat="server" Text="Live Animal"/>
</td>
</tr>

</table>
</div>
                <table width="750px">
                    <tr>
                        <td>
                            <asp:Label ID="lblTotVol" Style="font-size: medium; font-weight: bold;" runat="server"
                                Text="Total Volume"></asp:Label>
                        </td>
                        <td>                            
                            <asp:TextBox ID="TXTVolume" runat="server" Text="" Width="80px" CssClass="alignrgt" ></asp:TextBox>
                            &nbsp;
                            <asp:Label ID="LBLVolumeUnit" runat="server" Text="Cubic Cms" Width="80px"/>                            
                            &nbsp;
                            <asp:TextBox ID="txtMeterVolume" runat="server" Text="" Width="80px" CssClass="alignrgt"></asp:TextBox>
                            &nbsp;
                            <asp:Label ID="lblMeterVolume" runat="server" Text="Cubic Mtrs." Width="70px"/>
                        </td>                        
                        
                        </tr>
                        <tr>
                        <td>
                            <asp:Label ID="LBLTotal" Style="font-size: medium; font-weight: bold;" runat="server"
                                Text="Total Volumetric Weight" Visible="false"></asp:Label>
                        </td>
                        <td>                            
                            <asp:TextBox ID="TXTTotal" runat="server" Text="" Width="80px" CssClass="alignrgt"  Visible="false"></asp:TextBox>
                            
                        </td>
                    </tr>    
                    <tr>
                    <td>
                            <asp:Label ID="LBLSclWT" Style="font-size: medium; font-weight: bold;" runat="server"
                                Text="Total Scale Weight"></asp:Label>
                        </td>
                        <td>                            
                            <asp:TextBox ID="txtTotScaleWt" runat="server" Text="" Width="80px" CssClass="alignrgt"></asp:TextBox>
                            &nbsp;<asp:Label ID="lblUnit" runat="server" Text="Kg"></asp:Label>
                        </td>
                    </tr>                
                </table>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" OnClick="btnSave_Click">
        </asp:Button>
        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" OnClick="btnCancel_Click">
        </asp:Button>
    </div>
    </form>
</body>
</html>
