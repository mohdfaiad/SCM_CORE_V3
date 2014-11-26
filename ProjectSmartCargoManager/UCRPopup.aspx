<%@ Page Language="C#" CodeBehind="UCRPopup.aspx.cs" Inherits="ProjectSmartCargoManager.UCRPopup" AutoEventWireup="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="style/style.css" rel="stylesheet" type="text/css" />

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat = "server">
    
    <style type="text/css">
        .style1
        {
            width: 322px;
        }
        </style>
   </head>

<body class="divback">
<form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    
    <div id="contentarea">
    <div id="singlecol">
    <div id = "SearchEdit" runat = "server" >
    <div class="pagetitle">Search/Edit UCR</div>
    <table width="100%" cellpadding="3" cellspacing="3">
        <tr>
        <td>
        <asp:Label ID="lblSError" runat="server" Text="SError" ForeColor="Red"></asp:Label>
        </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label10" runat="server" Text=" UCR # :"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSUCR" runat="server"></asp:TextBox>
                &nbsp;
                </td>
            <td>
                <asp:Label ID="Label20" runat="server" Text=" ULD # :"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSUld" runat="server"></asp:TextBox>
            </td>
        </tr>    
        <tr>
            <td>
                <asp:Label ID="Label21" runat="server" Text=" From Date : *"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSfrmDate" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="txtSfrmDate_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtSfrmDate">
                </asp:CalendarExtender>
            </td>
            <td>
                <asp:Label ID="Label22" runat="server" Text=" To Date : *"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtSToDate" runat="server"></asp:TextBox>
                <asp:CalendarExtender ID="txtSToDate_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtSToDate">
                </asp:CalendarExtender>
            </td>
        </tr>    
        <tr>
            <td>
                            WareHouse
                            Transferring</td><td>
                           <asp:DropDownList ID="drpSTraWH" runat="server" Width="100px"> </asp:DropDownList> 
            </td>
            <td>
                            Carrier
                            Transferring</td><td>
                <asp:DropDownList ID="drpSTraCar" runat="server" Width="100px"> </asp:DropDownList>
            </td>
        </tr>    
        <tr>
            <td>
                            WareHouse
                            Final</td><td>
                            <asp:DropDownList ID="drpSFinWH" runat="server" Width="100px"> </asp:DropDownList>
            </td>
            <td>
                            Carrier
                            Receiving</td><td>
                <asp:DropDownList ID="drpSRecCar" runat="server" Width="100px"> </asp:DropDownList>
            </td>
        </tr>    
       
        </table>

      
                               <asp:Button ID="btnListUCR" runat="server" 
            CssClass="button"   Text="List UCRs" onclick="btnListUCR_Click" />
                                                                            <br /><br />

    <div><h2>Search Result </h2>
    <asp:GridView ID="gvUCRSearch" runat="server"   HeaderStyle-CssClass="HeaderStyle" 
            RowStyle-CssClass="RowStyle" AlternatingRowStyle-CssClass="AltRowStyle" 
            AllowPaging="True" PagerStyle-CssClass="PagerStyle" AutoGenerateColumns="False" 
            AutoGenerateEditButton="True" onrowediting="gvUCRSearch_RowEditing" 
            onpageindexchanging="gvUCRSearch_PageIndexChanging"   >
<RowStyle CssClass="RowStyle"></RowStyle>
    <Columns>
            <asp:TemplateField HeaderText="UCR #">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblUCRNo" runat="server" Text = '<%# Eval("UCRNo") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="UCR Date">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblUCRDt" runat="server" Text = '<%# Eval("UCRDate") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Transferring Carrier">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblTraCar" runat="server" Text = '<%# Eval("TraCar") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Transfer Warehouse">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblTraWH" runat="server" Text = '<%# Eval("TraWH") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Receiving Carrier">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblRecCar" runat="server" Text = '<%# Eval("RecCar") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Final Warehouse">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblFinWH" runat="server" Text = '<%# Eval("FinWH") %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="ULD #'s">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblULDs" runat="server" Text = '<%# Eval("ULDs") %>'></asp:Label></ItemTemplate></asp:TemplateField></Columns><EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>
    </asp:GridView>
    </div>
</div>
<br />
<div id = "New" runat = "server">
<div id = "NEWTitle" runat = "server" class="pagetitle"> New UCR </div>

<table width ="100%">
<tr>
    <td colspan="4" align="right">
        <asp:Label ID="lblError" runat="server" Text="Label" ForeColor="Red" Font-Bold="true" 
        Font-Size="Medium"></asp:Label>
       </td>
       </tr>
       <tr>
            <td>
                <asp:Label ID="Label9" runat="server" Text="Label"> UCR # : </asp:Label>
            </td>
            <td class="style1">
                <asp:TextBox ID="txtUCR" runat="server"></asp:TextBox>&nbsp;
                <%--<asp:ImageButton  ImageUrl="~/Images/searchbut.png" ID="btnSearch" runat="server" 
                Width="20px" Height="20px" onclick="btnSearch_Click" ValidationGroup="SEARCH"/>--%>
                <asp:Button ID="btnSearch" runat="server" onclick="btnSearch_Click" CssClass="button"   Text="List" ValidationGroup="SEARCH" />    
                       </td>
            <td>
                <asp:Label ID="Label19" runat="server" Text=" Transfer Date :  *"></asp:Label></td><td>
               <asp:TextBox ID="txtTraDt" runat="server"></asp:TextBox><asp:CalendarExtender ID="txtTraDt_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtTraDt">
                </asp:CalendarExtender>
            &nbsp;</td></tr><tr>
            <td>
                <asp:Label ID="Label11" runat="server" Text=" Transferring Party : *"></asp:Label></td><td class="style1">
                <asp:TextBox ID="txtTraPar" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label12" runat="server" Text=" Act Tra Date: "></asp:Label></td><td>
                <asp:TextBox ID="txtTraCarDt" runat="server"></asp:TextBox><asp:CalendarExtender ID="txtTraCarDt_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtTraCarDt">
                </asp:CalendarExtender>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label13" runat="server" Text=" Receiving Party : *"></asp:Label></td><td class="style1">
                <asp:TextBox ID="txtRecPar" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="Label14" runat="server" Text=" Act Rec Date: "></asp:Label></td><td>
                <asp:TextBox ID="txtRecCarDt" runat="server" CssClass="inputbgmed"></asp:TextBox><asp:CalendarExtender ID="txtRecCarDt_CalendarExtender" runat="server" 
                    Enabled="True" TargetControlID="txtRecCarDt">
                </asp:CalendarExtender>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label15" runat="server" Text=" Transfer Location : *"></asp:Label></td><td class="style1">
                <asp:DropDownList ID="drpTraWH" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drpTraWH_SelectedIndexChanged"></asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="drpTraSubWH" runat="server"></asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="Label16" runat="server" Text=" Final Location :  *"></asp:Label></td><td>
                <asp:DropDownList ID="drpFinWH" runat="server" AutoPostBack="True" 
                    onselectedindexchanged="drpFinWH_SelectedIndexChanged"></asp:DropDownList>
                &nbsp;
                <asp:DropDownList ID="drpFinSubWH" runat="server"></asp:DropDownList>
            
            </td>
            
        </tr>
        <tr>
            <td>
                Loaded:</td><td>
            <asp:CheckBox ID="chkIsLoaded" runat="server" Text = ""/>
            </td>
            <td rowspan="2">
                <asp:Label ID="Label17" runat="server" Text="Label"> Remarks : </asp:Label>
            </td>
            <td rowspan="2">
                <asp:TextBox ID="txtRemarks" runat="server" 
                    TextMode="MultiLine" Height="61px" Width="181px"></asp:TextBox></td></tr><tr>
        <td>
            AWB Number </td>
        <td class="style1">
            <asp:TextBox ID="txtAWBPrefix" runat="server" CssClass = "liwidthsm" MaxLength="3"></asp:TextBox>&nbsp; 
            <asp:TextBox ID="txtAWBNo" runat="server" MaxLength="8"></asp:TextBox></td></tr></table><div class="pagetitle">UCR ULD Details</div><table>
                    
            <tr>
            <td>
            
        <asp:GridView ID="gvULDDetails" runat="server"   
            HeaderStyle-CssClass="HeaderStyle" RowStyle-CssClass="RowStyle" 
            AlternatingRowStyle-CssClass="AltRowStyle" AllowPaging="True" 
            PagerStyle-CssClass="PagerStyle" ShowFooter="True" 
            AutoGenerateDeleteButton="True" 
            AutoGenerateColumns="False" onrowdeleting="gvULDDetails_RowDeleting" PageSize="30" onrowediting="gvULDDetails_RowEditing" 
            >
<RowStyle CssClass="RowStyle"></RowStyle>
                        <Columns>
                        <asp:TemplateField HeaderText="ULD #">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblULDNo" runat="server" Text = '<%# Eval("ULDNo") %>'></asp:Label>
                                   </ItemTemplate>
                                   <FooterTemplate>
                                    <asp:TextBox ID="txtULDNo" CssClass = "liwidthsm" runat="server" MaxLength="10"></asp:TextBox>
                                   </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Receipt No">    
                                   <ItemTemplate>    
                                    <asp:Label ID="lblRecNo" runat="server" Text = '<%# Eval("RecNo") %>'></asp:Label>
                                   </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="isDamaged">    
                                   <ItemTemplate>    
                                    <asp:CheckBox ID="chkDamaged" Checked='<%# bool.Parse(Eval("isDamaged").ToString()) %>' Enable='<%# !bool.Parse(Eval("isDamaged").ToString()) %>' runat="server"></asp:CheckBox>
                                   </ItemTemplate>
                                   <FooterTemplate>    
                                    <asp:CheckBox ID="chkFDamaged" runat="server"></asp:CheckBox>
                                   </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Returned At">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="lblRetAt" runat="server" Text = '<%# Eval("returnedAtWHCode") %>'></asp:TextBox>
                                   </ItemTemplate>
                                   <FooterTemplate>
                                    <asp:TextBox ID="txtRetAt" CssClass = "liwidthsm" runat="server" ></asp:TextBox>
                                   </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Returned On">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="lblRetOn" runat="server" Text = '<%# Eval("returnedOn") %>'></asp:TextBox><asp:CalendarExtender ID = "lblRetOn_Calender" runat = "server" TargetControlID = "lblRetOn" Enabled = "true"></asp:CalendarExtender>
                                   </ItemTemplate>
                                   <FooterTemplate>
                                    <asp:TextBox ID="txtRetOn" runat="server" CssClass = "liwidthsm" ></asp:TextBox><asp:CalendarExtender ID="txtRetOn_CalendarExtender" runat="server" Enabled="True" TargetControlID="txtRetOn"></asp:CalendarExtender>
                                   </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="AWB Prefix">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="lblAWBPrefix" CssClass = "liwidthsm" runat="server" Text = '<%# Eval("AWBPrefix") %>'></asp:TextBox>
                                   </ItemTemplate>
                                   <FooterTemplate>
                                    <asp:TextBox ID="txtAWBPrefix" CssClass = "liwidthsm" runat="server" MaxLength="3"></asp:TextBox>
                                   </FooterTemplate>
                        </asp:TemplateField><asp:TemplateField HeaderText="AWBNo">    
                                   <ItemTemplate>    
                                    <asp:TextBox ID="lblAWBNo" CssClass = "liwidthsm" runat="server" Text = '<%# Eval("AWBNo") %>'></asp:TextBox>
                                   </ItemTemplate>
                                   <FooterTemplate>
                                    <asp:TextBox ID="txtAWBNo" runat="server" CssClass = "liwidthsm"  MaxLength="8"></asp:TextBox>
                                   </FooterTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="isLoaded">    
                                   <ItemTemplate>    
                                    <asp:CheckBox ID="chkTLoaded" Checked='<%# bool.Parse(Eval("isLoaded").ToString()) %>' Enable='<%# !bool.Parse(Eval("isDamaged").ToString()) %>' runat="server"></asp:CheckBox>
                                   </ItemTemplate>
                                   <FooterTemplate>    
                                    <asp:CheckBox ID="chkLoaded" runat="server"></asp:CheckBox>
                                   </FooterTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Action">
                        <FooterTemplate>
                                   <asp:Button ID="btnADDULD" runat="server" Text="Add ULD" CssClass="button" OnClick = "btnADDULD_Click"/>
                        </FooterTemplate>
                        </asp:TemplateField>
                        </Columns>
         <EditRowStyle CssClass="grdrowfont"></EditRowStyle>
                    <FooterStyle CssClass="grdrowfont"></FooterStyle>
                    <HeaderStyle CssClass="titlecolr" Height="30px"></HeaderStyle>
                    <RowStyle CssClass="grdrowfont"></RowStyle>

            </asp:GridView>
                        <asp:Button ID="btnUploadImage" runat="server" Text="Upload Image" 
            CssClass="button" Enabled="false" onclick="btnUploadImage_Click" Visible="False" />
  

            </td>
            </tr>
        </table>
     
     <div class="pagetitle">
        Accessories
     </div>
     <div>
      <table border="1" >
        <tr class="titlecolr">
        <td>
            <span>Status</span> 
        </td>
        <td>
            <span>Nets</span>
        </td>
        <td>
        Doors
        </td>
        <td>
            <span>Straps</span>
        </td>
        <td>
            <span>Fittings</span>
        </td>
        </tr>
        
        <tr>
        <td>
            <asp:Label ID="lblRelealsed" runat="server" Text="Released"></asp:Label></td><td>
            <asp:TextBox ID="txtNetsRel" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtDoorsRel" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtStrapsRel" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtFittingsRel" runat="server"></asp:TextBox></td></tr><tr>
        <td>
            <asp:Label ID="lblReturned" runat="server" Text="Returned"></asp:Label></td><td>
            <asp:TextBox ID="txtNetsRet" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtDoorsRet" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtStrapsRet" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtFittingsRet" runat="server"></asp:TextBox></td></tr><tr>
        <td>
            <asp:Label ID="lblDamaged" runat="server" Text="Damaged"></asp:Label></td><td>
            <asp:TextBox ID="txtNetsDam" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtDoorsDam" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtStrapsDam" runat="server"></asp:TextBox></td><td>
            <asp:TextBox ID="txtFittingsDam" runat="server"></asp:TextBox></td></tr><tr>
        <td>
            <asp:Label ID="lblYetToArrive" runat="server" Text="Yet To Arrive"></asp:Label></td><td>
            <asp:Label ID="lblNetYet" runat="server" Text=""></asp:Label></td><td>
            <asp:Label ID="lblDoorsYet" runat="server" Text=""></asp:Label></td><td>
            <asp:Label ID="lblStrapsYet" runat="server" Text=""></asp:Label></td><td>
            <asp:Label ID="lblFittingsYet" runat="server" Text=""></asp:Label></td></tr></table></div></div><br />
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button" 
            onclick="btnSave_Click" /> <asp:Button ID="btnPrint" runat="server" 
            Text="Print" CssClass="button" onclick="btnPrint_Click" /> 
            <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
            onclick="btnClear_Click" /> 
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button" 
            onclick="btnCancel_Click" Visible="false" />
  
</div>
    <div id = "" style = "height:130px;width:100%;visibility:hidden;" visible="false">
                    <asp:FileUpload ID="PhotoUpload" runat="server" Width="250px" />
             
             
                     
                                   <asp:Button ID="btnOpenPic" runat="server" 
                                                     CssClass = "button"  Text="Preview And ADD Pic" 
                                       onclick="btnOpenPic_Click" />
                                                       &nbsp;                          
                                                                                                                
                    <asp:Image ID="ImagePreview" runat="server" BorderStyle="Solid"  Height="100px" 
                           Width="100px" BorderWidth="1px" />
    </div>
    </div>
</form>    
    

</body>
</html>
