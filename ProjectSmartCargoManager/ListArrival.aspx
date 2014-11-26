<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ListArrival.aspx.cs" MasterPageFile="~/SmartCargoMaster.Master" Inherits="ProjectSmartCargoManager.ListArrival" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script language="javascript" type="text/javascript">
     function setAgentName(DropDownList) {
         var strValue = DropDownList.options[DropDownList.selectedIndex].value;
         document.getElementById('ctl00_ContentPlaceHolder1_txtAgentCode').value = strValue;
     }


     function NoData() {

         alert("No Data Found");

     }
    
 </script>

    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="TSM" runat="server">
  </asp:ToolkitScriptManager>
  <div id="contentarea">
    
    <p>
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
             ForeColor="Red"></asp:Label>
        </p>
    <div class="botline">
        <table width="100%" >
        <tr>
       <td>
         AWB Number
      </td>
      <td>
          <asp:TextBox ID="txtawbnumber" runat="server"></asp:TextBox>
      </td>        
     <td>
        Flight Number
      </td>
      <td>
          <asp:TextBox ID="txtflightNumber" runat="server"></asp:TextBox>
      </td>   
      <td>
       <asp:Button ID="btnList" runat="server" Text="List" CssClass="button" 
              onclick="btnList_Click" />
        &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button" 
              onclick="btnClear_Click" />
       
      </td>
        </tr> 
     </table> 
   </div> 
   <div class="divback" style="overflow:auto "> 
         <asp:GridView ID="grdArrivalDetails" runat="server" AutoGenerateColumns="False" 
            ShowFooter="True"   Width="100%" Height="250px">
            <Columns>
                                            
                <asp:TemplateField HeaderText="ULD No" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lbluldno" runat="server" Text='<%# Eval("ULD") %>' Width="80px"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
               <asp:TemplateField HeaderText="POL" HeaderStyle-Wrap="true">
                    <ItemTemplate>
                        <asp:Label ID="lblPOL" runat="server" Text='<%# Eval("POL") %>' Width="80px" ></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False"/>
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="ULD Destination" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblULDDestination" runat="server" ValidationGroup="check" Width="80px" Text='<%# Eval("ULDDestination") %>' >
                        </asp:Label>
                       
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="AWB Number" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblAWBNumber" runat="server" Width="80px" Text='<%# Eval("AWBNumber") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
   
               <asp:TemplateField HeaderText="Flight No" HeaderStyle-Wrap="false" >
                    <ItemTemplate>
                        <asp:Label  ID="lblFltNo" runat="server" Width="60px" Text='<%# Eval("FltNo") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="MFT Pieces" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblMFTPieces" runat="server" Width="60px" Text='<%# Eval("MFTPieces") %>'>
                        </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
               
                             
                
                 <asp:TemplateField HeaderText="MFTWeight" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblMFTWeight" runat="server" Width="60px" Text='<%# Eval("MFTWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="RCV Pieces" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRCVPieces" runat="server" Width="80px" Text='<%# Eval("RCVPieces") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="RCV Weight" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblRCVWeight" runat="server" Width="80px" Text='<%# Eval("RCVWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Origin" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblOrigin" runat="server" Width="80px" Text='<%# Eval("Origin") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Destination" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblDestination" runat="server" Width="80px" Text='<%# Eval("Destination") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Stated Pieces" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblStatedPieces" runat="server" Width="80px" Text='<%# Eval("StatedPieces") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                <asp:TemplateField HeaderText="Stated Weight" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblStatedWeight" runat="server" Width="80px" Text='<%# Eval("StatedWeight") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Discrepancy" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                        <asp:Label  ID="lblDiscrepancy" runat="server" Width="80px" Text='<%# Eval("Discrepancy") %>'></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                </asp:TemplateField>
                                   
           </Columns>
            <HeaderStyle CssClass="titlecolr"/>
            <RowStyle  HorizontalAlign="Center"/>
            <AlternatingRowStyle  HorizontalAlign="Center"/>
        </asp:GridView>

 
 </div>
   </div> 
   </asp:Content> 