<%--

2012-04-05  vinayak
2012-07-24  vinayak

--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GHA_Dimensions.aspx.cs" Inherits="ProjectSmartCargoManager.GHA_Dimensions" %>

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


        function CloseWindow(selectedval, id, volume) {

            opener.document.getElementById('<%= Request["TargetTXT"] %>').value = '' + selectedval;
            opener.document.getElementById('<%= Request["Hid"] %>').value = '' + id;
            opener.document.getElementById('<%= Request["VolumeTXT"] %>').value = '' + volume;


            window.close();
            //opener.SumVolume('<%= Request["VolumeTXT"] %>');
        }
        function CloseWindowAcceptance(recievedPcs, recievedWt) {
            opener.document.getElementById('<%= Request["AccpPcsID"] %>').value = '' + recievedPcs;
            opener.document.getElementById('<%= Request["AccpWtID"] %>').value = '' + recievedWt;
            window.close();
        } 
        function ShowHideShipperDetials() {
            var DivDisplayStatus;
            DivDisplayStatus = (document.getElementById('divDimenssionCore').style.display == "block" ? "none" : "block");

            document.getElementById('divDimenssionCore').style.display = DivDisplayStatus;

            if (DivDisplayStatus == "none") {
                document.getElementById('imgPlus').style.display = "block";
                document.getElementById('imgMinus').style.display = "none";
            }
            else {
                document.getElementById('imgMinus').style.display = "block";
                document.getElementById('imgPlus').style.display = "none";
            }
        }
        function ShowHideShipperDetials1() {
            var DivDisplayStatus;
            if(document.getElementById('divDimenssionCore').style.display == "block") {
            
            document.getElementById('divDimenssionCore').style.display = "block";
            }
            if(document.getElementById('divDimenssionCore').style.display == "none")
            {
            document.getElementById('divDimenssionCore').style.display = "none";
            }
            

            document.getElementById('divDimenssionCore').style.display = DivDisplayStatus;

            if (DivDisplayStatus == "none") {
                document.getElementById('imgPlus').style.display = "block";
                document.getElementById('imgMinus').style.display = "none";
            }
            else {
                document.getElementById('imgMinus').style.display = "block";
                document.getElementById('imgPlus').style.display = "none";
            }
        }
        function ShowDimensionsGrid() {
            document.getElementById('divDimenssionCore').style.display = "block"; 
        }
    </script>

</head>
<body class="divback">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ToolkitScriptManager>
    <div>
        <table width="100%">
                <tr>
                <td style="font-size: medium;">
                    Commodity
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblCommodity" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    Pcs Count
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="LBLPcsCount" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    Gross Wt
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblGrossWt" runat="server"></asp:Label>
                </td>
                <td width="90px" valign="top" style="font-size: medium;">
                    Unit</td>
                    <td ><asp:DropDownList ID="ddlUnit" runat="server" OnSelectedIndexChanged="ddlUnit_SelectedIndexChanged"
                        AutoPostBack="true">
                        <asp:ListItem Text="Inches"></asp:ListItem>
                        <asp:ListItem Text="Cms" Selected="True"></asp:ListItem>
                       
                    </asp:DropDownList></td>
                </tr>
                <tr>
                <%--<td><asp:CheckBox ID="chkBulk" runat="server" Text="Bulk" /></td>--%>
                <td style="font-size: medium;">
                    <asp:Label ID="lblFlightNoDisplay" Text = "Flight No" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblFlightNo" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblFlightDateDisplay" Text = "Flight Date" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblFlightDate" runat="server"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    &nbsp;</td>
                </tr>                                    
        </table>
        <asp:UpdatePanel runat="server" ID="UPDimension">
            <ContentTemplate>
             <asp:Label ID="LBLStatus" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
                <asp:GridView ID="grdDim" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                    ShowFooter="True" Width="100%" Height="21px" 
                    >
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="CHKSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bulk" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="CkBulk" runat="server" Checked='<%# Eval("IsBulk") %>' ></asp:CheckBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Length" HeaderStyle-Wrap="true" HeaderStyle-Width="303px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLength" onfocus="javascript:this.select();" runat="server" Width="40px" EnableViewState="true" Text='<%# Eval("Length","{0:0}") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                            <FooterTemplate>
                                
                                <asp:Button ID="btnAddRow" runat="server" CssClass="button" Text="Add" OnClick="btnAddRowtop_Click"  />
                                
                                <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" OnClick="btnSavetop_Click" />
                                
                                <asp:Button ID="btnDeleteRow" runat="server" CssClass="button" Text="Delete" OnClick="btnDeleteRowtop_Click" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Breadth" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBreadth" runat="server" EnableViewState="true" Width="40px" Text='<%# Eval("Breadth","{0:0}") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtHeight" runat="server" Width="40px" Text='<%# Eval("Height","{0:0}") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="No Of Pcs" HeaderStyle-Wrap="True" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPcs" runat="server" Width="40px" OnClick="btnSavetop_Click" OnTextChanged="txtPcstop_TextChanged" Text='<%# Eval("PcsCount") %>'
                                    AutoPostBack="True" ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="True" />
                            
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Vol." HeaderStyle-Wrap="True" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVol" runat="server" Width="80px" AutoPostBack="True" Text='<%# Eval("Volume") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="True" />
                            
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Vol. Wt." HeaderStyle-Wrap="True" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtVolWT" runat="server" Width="80px" AutoPostBack="True" Text='<%# Eval("Weight") %>' ></asp:TextBox>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="True" />
                            
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="PieceId From" HeaderStyle-Wrap="True" HeaderStyle-Width="100px">
                            <ItemTemplate>
                            <asp:Label ID="lblpId" runat="server" Width="100px" Text='<%# Eval("Pids") %>'></asp:Label>
                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="True" />
                            
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="PieceId To" HeaderStyle-Wrap="True" HeaderStyle-Width="100px">
                            <ItemTemplate>
                            <asp:Label ID="lblpIdto" runat="server" Width="100px" Text='<%# Eval("Pidsto") %>'></asp:Label>
                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="True" />
                            
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                </asp:GridView>
                <img ID="imgPlus" src="plus.gif" onclick="ShowHideShipperDetials(); return false;" style="display:block;vertical-align:middle; padding-bottom:5px; padding-top:10px;cursor:pointer;" />
                        <img ID="imgMinus" src="minus.gif" onclick="ShowHideShipperDetials(); return false;" style="display:none;vertical-align:middle; padding-bottom:5px; padding-top:10px;cursor:pointer;" />
                        
                <div id="divDimenssionCore" style="overflow:auto;height:200px; width:775px; border:solid 1px #ccc; display: none;" runat="server">
                <asp:GridView ID="grdDimension" runat="server" AutoGenerateColumns="False" EnableViewState="true"
                    ShowFooter="True" Width="550px" OnRowDataBound="grdDimension_RowDataBound" 
                        >
                    <Columns>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:CheckBox ID="CHKSelect" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Piece No" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:Label ID="txtPieceNo" runat="server" Width="20px" EnableViewState="true" Text='<%# Eval("PieceNo") %>' ></asp:Label>
                                <%--AutoPostBack="True" OnTextChanged="txtPcs_TextChanged"--%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Piece Id" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:Label ID="txtPieceId" runat="server" Width="80px" EnableViewState="true" Text='<%# Eval("IdentificationNo") %>' ></asp:Label>
                                <%--AutoPostBack="True" OnTextChanged="txtPcs_TextChanged"--%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Length" HeaderStyle-Wrap="true">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLength" CssClass="alignrgt" runat="server" Width="40px" MaxLength="8" EnableViewState="true" Text='<%# Eval("Length") %>' AutoPostBack="True" OnTextChanged="CalculateVolume"></asp:TextBox>
                                <%--AutoPostBack="True" OnTextChanged="CalculateVolume"--%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Breadth" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBreadth" CssClass="alignrgt" runat="server" EnableViewState="true" MaxLength="8" Width="40px" Text='<%# Eval("Breath") %>' AutoPostBack="True" OnTextChanged="CalculateVolume"></asp:TextBox>
                                <%--AutoPostBack="True"  OnTextChanged="txtPcs_TextChanged"--%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="False"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Height" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtHeight" CssClass="alignrgt" runat="server" Width="40px" MaxLength="8" Text='<%# Eval("Height") %>' AutoPostBack="True"  OnTextChanged="CalculateVolume"></asp:TextBox>
                                <%--AutoPostBack="True"  OnTextChanged="txtPcs_TextChanged"--%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="Volume" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblVolume" runat="server" Width="60px" Text='<%# Eval("Vol") %>'></asp:Label>
                                <%--AutoPostBack="True"  OnTextChanged="txtPcs_TextChanged"--%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Weight" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWeight" CssClass="alignrgt" runat="server" Width="60px" Text='<%# Eval("Wt") %>'></asp:TextBox>
                                <%--AutoPostBack="True"  OnTextChanged="txtPcs_TextChanged"--%>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Piece Type">
                        <ItemTemplate>
                            <asp:DropDownList ID="ddlPieceType" runat="server" Width="75px" CssClass="grdrowfont">                                
                                <asp:ListItem Selected="True">Bulk</asp:ListItem>
                                <asp:ListItem>Bags</asp:ListItem>
                                <asp:ListItem>ULD</asp:ListItem>                                                        
                            </asp:DropDownList>
                        </ItemTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bag#" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBagNo" runat="server" Width="60px" Text='<%# Eval("BagNo") %>'></asp:TextBox>                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ULD#" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtULDNo" runat="server" Width="100px" Text='<%# Eval("ULDNo") %>'></asp:TextBox>                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location" HeaderStyle-Wrap="false" Visible="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLocation" runat="server" Width="60px" Text='<%# Eval("Location") %>'></asp:TextBox>                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Fligh No." HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFlightNo" runat="server" ReadOnly="false" Width="60px" Text='<%# Eval("FlightNo") %>'></asp:TextBox>                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Fligh Date" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFlightDate" runat="server" Width="60px" ReadOnly="false" Text='<%# Eval("FlightDate") %>'></asp:TextBox>                                
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Copy" HeaderStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLines" CssClass="alignrgt" runat="server" Width="20px"></asp:TextBox>
                                <asp:Button ID="btnCopy" Text="Copy" runat="server" OnClick="CopyDimensions"></asp:Button>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bulk" HeaderStyle-Wrap="false" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBulk" CssClass="alignrgt" runat="server" Width="20px" Text='<%# Eval("IsBulk") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hide" HeaderStyle-Wrap="false" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblHide" CssClass="alignrgt" runat="server" Width="20px"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle Wrap="True"></HeaderStyle>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle CssClass="titlecolr" />
                    <RowStyle HorizontalAlign="Center" />
                    <AlternatingRowStyle HorizontalAlign="Center" />
                </asp:GridView>
                </div>
                <br />
                <table width="750px">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Style="font-size: medium; font-weight: bold;" runat="server"
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
                                Text="Total Volumetric Weight"></asp:Label>
                        </td>
                        <td>                            
                            <asp:TextBox ID="TXTTotal" runat="server" Text="" Width="80px" CssClass="alignrgt"></asp:TextBox>
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
        <asp:HiddenField ID="hdVolumetriccheck" runat="server" />
    </div>
    </form>
</body>
</html>
