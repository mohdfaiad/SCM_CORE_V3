<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmAddChildAWBForULD.aspx.cs" Inherits="ProjectSmartCargoManager.frmAddChildAWBForULD" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ARRIVAL</title>
</head>
<body class="divback">
<form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePageMethods="True">
    </asp:ToolkitScriptManager>
    
    <script type="text/javascript">

        function CloseWindow(AWBCount, ULDPcs, ULDWt) 
        {
            
            opener.document.getElementById('<%= Request["AWBCount"] %>').value = AWBCount;
            
            opener.document.getElementById('<%= Request["ULDPcs"] %>').value = ULDPcs;
            
            opener.document.getElementById('<%= Request["AWBWt"] %>').value = ULDWt;

            opener.document.getElementById('<%= Request["ULDWt"] %>').value = ULDWt;
            
            window.close();
        }

        function Close() {

            window.close();
        } 
        
    </script>
    <div style="width: 1024px; padding-left: 20px;">
        <h1> 
           Child AWBs
 <%--    <img alt="" src="images/txtarrival.png" />--%>
        </h1>
        <br />
        <table style="height: 30px" width="800px">
            <%--<tr>
                <td style="font-size: medium; width: 50%">
                    AWB Number
                </td>
                <td style="font-size: medium;">
                    <asp:Label ID="lblawbno" runat="server"></asp:Label>
                </td>
            </tr>--%>
            <tr>
                <td style="font-size: medium;">
                   <asp:Label ID="lblParentType" runat="server" Text="ULD #"></asp:Label>
                    &nbsp;
                    <asp:Label ID="lblParentNumber" runat="server" Font-Underline="true"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    Flight #
                    &nbsp;
                    <asp:Label ID="lblFlightNumber" runat="server" Font-Underline="true"></asp:Label>
                </td>
                <td style="font-size: medium;">
                    Flight Date
                    &nbsp;
                    <asp:Label ID="lblFlightDate" runat="server" Font-Underline="true" 
                    ToolTip="Flight date in DD/MM/YYYY format."></asp:Label>
                </td>
                <td style="font-size: medium;">
                    POU
                    &nbsp;
                    <asp:Label ID="lblPOU" runat="server" Font-Underline="true"></asp:Label>
                </td>
            </tr>                        
        </table>
        <asp:UpdatePanel runat="server" ID="UPAddChild">
            <ContentTemplate>
                <br />
                <asp:Label ID="lblStatus" runat="server" Text="" ForeColor="Red"></asp:Label>
                <br />
                <div style="overflow:auto;width:1020px; border:solid 1px #ccc;">
                
                <asp:GridView ID="GVArrDet" runat="server" AllowPaging="False" 
               AutoGenerateColumns="False" BorderColor="#BFBEBE" BorderStyle="None" 
               CellPadding="2" CellSpacing="3" PageSize="10">
               <Columns>
                   <asp:TemplateField>
                       <%--<HeaderTemplate>
                     <input type="checkbox" name = "checkall" onclick="javascript:SelectAllgrdAddRate(this);" />
                     </HeaderTemplate>--%>
                       <ItemTemplate>
                           <asp:CheckBox ID="check" runat="server" />
                       </ItemTemplate>
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="AWB *<br />(Prefix-Number)" 
                       ItemStyle-Wrap="false" >
                       <ItemTemplate>
                           <asp:TextBox ID="AWB" runat="server" AutoPostBack="true" EnableViewState="true" MaxLength="12" 
                               OnTextChanged="Getdata" Text='<%# Eval("AWBno") %>' Width="90px"> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt# *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="FlightNo" runat="server" Text='<%# Eval("FltNo") %>' 
                               Width="60px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Flt Dt *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="FltDate" runat="server" Text='<%# Eval("FltDate") %>' 
                               Width="70px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Origin *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Origin" runat="server" Text='<%# Eval("Org") %>' Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Dest *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Destn" runat="server" Text='<%# Eval("Dest") %>' Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="POL *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="POL" runat="server" Text='<%# Eval("POL") %>' Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Bkd Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="BkdPcs" runat="server" Text='<%# Eval("BookedPcs") %>' 
                           Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Bkd Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="BkdWt" runat="server" Text='<%# Eval("BookedWt") %>' 
                              Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="true" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Acc Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="StdPcs" runat="server" Text='<%# Eval("StatedPCS") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Acc Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="StdWt" runat="server" Text='<%# Eval("StatedWgt") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="true" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mft Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="MftPcs" runat="server" 
                               Text='<%# Eval("PCS") %>' Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Mft Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="MftWt" runat="server" Text='<%# Eval("GrossWgt") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arr Pcs" 
                       ItemStyle-Wrap="false">
                       <EditItemTemplate>
                           <asp:TextBox ID="txtArrPcs" runat="server" Text="">
                             </asp:TextBox>
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:TextBox ID="txtArrivedPcs" runat="server" 
                               Text='<%# Eval("ArrivedPieces") %>' Width="40px">   
                    </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Arr Wt" 
                       ItemStyle-Wrap="false">
                       <EditItemTemplate>
                           <asp:TextBox ID="txtArrWt" runat="server" Text=""></asp:TextBox>
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:TextBox ID="txtArrivedWt" runat="server" 
                               Text='<%# Eval("ArrivedWeight") %>' Width="40px">
                    </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Received Pcs. *" 
                       ItemStyle-Wrap="false">
                       <EditItemTemplate>
                           <asp:TextBox ID="TextBox1" runat="server" Text='<%# Eval("RcvPcs") %>' ></asp:TextBox>
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:TextBox ID="RcvPcs" runat="server" 
                               onchange="javascript:calculateActualWt(this);" Text='<%# Eval("RcvPcs") %>' Width="40px"></asp:TextBox>
                               <asp:ImageButton ID="btnDimensionsPopup" Enabled="false" runat="server" ImageUrl="~/Images/list_bullets.png"
                                ImageAlign="AbsMiddle" OnClientClick="javascript:dimension(this);return false;"/>
                               
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Received Wt. *" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="RcvWt" runat="server" Text='<%# Eval("RcvWt") %>'  
                            OnTextChanged="SetReceivedWeight" AutoPostBack="true" Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                 
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Rem Pcs" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Expectedpcs" runat="server" Text='<%# Eval("ExpectedPcs") %>' 
                               Width="40px"></asp:TextBox>
                           <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="Expectedpcs">
                        </asp:RequiredFieldValidator>--%> <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Rem Wt" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ExpectedWeight" runat="server" 
                               Text='<%# Eval("ExpectedWeight") %>' Width="40px"></asp:TextBox>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Comm Code" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="CommCode" runat="server" 
                               Text='<%# Eval("SCC") %>' Width="40px"></asp:TextBox>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="true" HeaderText="Comm Desc" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="CommDesc" runat="server" Visible="false" 
                               Text='<%# Eval("DESC") %>' Width="80px"></asp:TextBox>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="ULD Dstn" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ULDDestn" runat="server" Text='<%# Eval("ULDdest") %>' 
                               Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Discrepancy" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:DropDownList ID="ddlDiscrepancy" runat="server">
                           </asp:DropDownList>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="ULD#" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ULD" runat="server" Text='<%# Eval("ULDno") %>' Width="32px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Security Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="SecurityCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Check" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:CheckBox ID="CustomCheck" runat="server" Text="" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Custom Status Code" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="CustomStatusCode" runat="server" 
                           Text='<%# Eval("CustomStatusCode") %>' Width="55px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="true" />
                       <ItemStyle Wrap="false" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Remarks" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Remark" runat="server" Width="80px"
                           Text='<%# Eval("ArrivalRemarks") %>'> </asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Status" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Status" runat="server" Enabled="false" 
                               Text='<%# Eval("status") %>' Width="25px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Owner" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Owner" runat="server" Text="" Width="60px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Discription" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="lblDiscription" runat="server" Text='<%# Eval("Desc") %>'></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Reassign" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="statusreassign" runat="server" Enabled="false" 
                               Text='<%# Eval("statusreassign") %>' Width="40px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="Remainingpcs" runat="server" 
                               Text='<%# Eval("RemainingPcs") %>' Width="0px"></asp:TextBox>
                           <%--'<%# Eval("ExpectedPcs") %>'--%>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="" 
                       ItemStyle-Wrap="false" Visible="false">
                       <ItemTemplate>
                           <asp:TextBox ID="ArrivedPcs" runat="server" Enabled="false" 
                               Text='<%# Eval("ArrivedPieces") %>' Width="0px"></asp:TextBox>
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Wrap="false" HeaderText="Location" 
                       ItemStyle-Wrap="false">
                       <ItemTemplate>
                           <asp:TextBox ID="txtLocation" runat="server" 
                           EnableViewState="true" Text='<%# Eval("Location") %>' Width="80px"> </asp:TextBox>
                           <asp:HiddenField ID="hdnManualAWB" runat="server" Value='<%# Eval("ManualAWB") %>' Visible="false" />
                       </ItemTemplate>
                       <HeaderStyle Wrap="False" />
                       <ItemStyle Wrap="False" />
                   </asp:TemplateField>
                   <asp:ButtonField CommandName="Edit" Text="Edit" Visible="false">
                       <ItemStyle Width="50px"/>
                   </asp:ButtonField>
               </Columns>
               <HeaderStyle CssClass="titlecolr" />
               <RowStyle HorizontalAlign="Center" />
               <AlternatingRowStyle HorizontalAlign="Center" />
           </asp:GridView>
                
                <br />
                
                    <asp:Button ID="btnAdd" runat="server" CssClass="button" Text="Add"
                    Visible="true" onclick="btnAdd_Click"></asp:Button>
                    &nbsp;
                    <asp:Button ID="btnRemove" runat="server" CssClass="button" Text="Remove"
                    Visible="true" onclick="btnRemove_Click"></asp:Button>
                </div>
                <br />
                <table width="100%" style="visibility:hidden">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" Style="font-size: medium; font-weight: bold;" runat="server"
                                Text="Total Pieces"></asp:Label>
                        </td>
                        <td>                            
                            <asp:TextBox ID="txtTotalPcs" runat="server" Text="" Width="80px" CssClass="alignrgt" ></asp:TextBox>
                        </td>                        
                        <td>
                            <asp:Label ID="lblTotalWeight" Style="font-size: medium; font-weight: bold;" runat="server"
                                Text="Total Weight"></asp:Label>
                        </td>
                        <td>                            
                            <asp:TextBox ID="txtTotalWeight" runat="server" Text="" Width="80px" CssClass="alignrgt"></asp:TextBox>
                            &nbsp;Kg
                        </td>
                    </tr>                    
                </table>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
            onclick="btnSave_Click" >
        </asp:Button>
        &nbsp;<asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" 
            onclick="btnCancel_Click">
        </asp:Button>
    </div>
    </form>
</body>
</html>
