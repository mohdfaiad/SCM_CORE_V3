<%@ Page Title="AircraftEquipment" Language="C#" MasterPageFile="~/SmartCargoMaster.Master" AutoEventWireup="true" CodeBehind="AircraftEquipment.aspx.cs" Inherits="ProjectSmartCargoManager.AircraftEquipment"  %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 
    <script language="javascript" type="text/javascript">
        function Save() 
        {

        alert("Data Saved Successfuly");

    }
    
    function Discard() {

        alert("Data Not Saved");

    }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
       <div id="contentarea">
    <h1>                 &nbsp;AirCraft Equipment New</h1>
                     
            <asp:Label ID="lblStatus" runat="server" BackColor="White" Font-Bold="True" Font-Size="Large"
                ForeColor="Blue"></asp:Label>
        
     <div class="botline">
    <table width=80% cellpadding="3" cellspacing="3" >    
        <tr>
            <td>
                <asp:Label ID="lblManuf" runat="server" Text="Manufacturer"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtManuf" runat="server"></asp:TextBox>
            &nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtManuf" ErrorMessage="*"></asp:RequiredFieldValidator>
            </td>
          
            <td>
                <asp:Label ID="lblAcType" runat="server" Text="Aircraft Type"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtAcType" runat="server"></asp:TextBox>
               
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="txtAcType" ErrorMessage="*"></asp:RequiredFieldValidator>
               
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblVer" runat="server" Text="Version"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtAcVer" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtAcVer" ErrorMessage="*"></asp:RequiredFieldValidator>
                <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" 
                    ControlToValidate="txtAcVer" ErrorMessage="Invalid Version" 
                    ValidationExpression="^\d*\.?\d+$"></asp:RegularExpressionValidator>--%>
            </td>
          
            <td>
                <asp:Label ID="lblCount" runat="server" Text="Count"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCount" runat="server" AutoPostBack="True" 
                    ontextchanged="txtCount_TextChanged"></asp:TextBox>
                &nbsp;Nos.<asp:RangeValidator ID="txtCountRangeValidator" runat="server" 
                    ControlToValidate="txtCount" ErrorMessage="*" MaximumValue="999" 
                    MinimumValue="1" Type="Integer"></asp:RangeValidator>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" 
                    runat="server" ControlToValidate="txtCount" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                    ControlToValidate="txtCount" ErrorMessage="Non Integer" 
                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPassCap" runat="server" Text="Passenger Capacity"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPCap" runat="server"></asp:TextBox>
            &nbsp;Nos.<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtPCap" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                    ControlToValidate="txtPCap" ErrorMessage="Non Integer" 
                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
            </td>
           
            <td>
                <asp:Label ID="lblLWt" runat="server" Text="Landing Weight"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtLWt" runat="server"></asp:TextBox>
                &nbsp;Kg.<asp:RequiredFieldValidator ID="RequiredFieldValidator7" 
                    runat="server" ControlToValidate="txtLWt" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" 
                    ControlToValidate="txtLWt" ErrorMessage="Non Integer" 
                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblCarCap" runat="server" Text="Cargo Capacity"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtCarCap" runat="server"></asp:TextBox>
                &nbsp;Kg.<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtCarCap" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" 
                    ControlToValidate="txtCarCap" ErrorMessage="Non Integer" 
                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
            </td>
            
            <td>
                <asp:Label ID="lblMTOW" runat="server" Text="MTOW"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtMTOW" runat="server"></asp:TextBox>
                &nbsp;Kg.<asp:RequiredFieldValidator ID="RequiredFieldValidator8" 
                    runat="server" ControlToValidate="txtMTOW" ErrorMessage="*"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" 
                    ControlToValidate="txtMTOW" ErrorMessage="Non Integer" 
                    ValidationExpression="^\d+$"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblRwt" runat="server" Text="Restriction Wt/Pc."></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtRwt" runat="server"></asp:TextBox>
                Kg.</td>
            
            <td>
                <asp:Label ID="lblRwt0" runat="server" Text="Dimension/Pc."></asp:Label>
            </td>
            <td>
               
                <asp:TextBox ID="txtRl" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtRl_TextBoxWatermarkExtender" 
                    runat="server" WatermarkText="Length" Enabled="True" TargetControlID="txtRl">
                </asp:TextBoxWatermarkExtender>
                <asp:TextBox ID="txtRb" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtRb_TextBoxWatermarkExtender" 
                    runat="server" WatermarkText="Breadth"  Enabled="True" TargetControlID="txtRb">
                </asp:TextBoxWatermarkExtender>
                <asp:TextBox ID="txtRh" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtRh_TextBoxWatermarkExtender" 
                    runat="server" WatermarkText="Height"   Enabled="True" TargetControlID="txtRh">
                </asp:TextBoxWatermarkExtender>
                <asp:DropDownList ID="ddldimension" runat="server" Width="50px">
                <asp:ListItem Text="Cmt" Selected="True"></asp:ListItem> 
                <asp:ListItem Text="Inch"></asp:ListItem>  
                </asp:DropDownList>
                </td>
        </tr>
        <tr>
            <td>
                Identity</td>
            <td>
                <asp:DropDownList ID="ddlAircraftIdentity" runat="server">
                    <asp:ListItem>Narrow</asp:ListItem>
                    <asp:ListItem>Wide</asp:ListItem>
                </asp:DropDownList>
            </td>
            
            <td>
                <asp:Label ID="lblVolume" runat="server" Text="Volume"></asp:Label>
            </td>
            <td>
               
                <!--<asp:TextBox ID="txtVolLen" runat="server" Width="50px"></asp:TextBox>-->
                <asp:TextBox ID="txtVol" runat="server" Width="50px"></asp:TextBox>
                Cubic
               <!-- <asp:TextBoxWatermarkExtender ID="txtVl_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtVolLen" WatermarkText="Length">
                </asp:TextBoxWatermarkExtender>-->
                <!--<asp:TextBox ID="txtVolBreadth" runat="server" Width="50px"></asp:TextBox>
                <asp:TextBoxWatermarkExtender ID="txtVolBreadth_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtVolBreadth" WatermarkText="Breadth">
                </asp:TextBoxWatermarkExtender>-->
                <!--<asp:TextBox ID="txtVolHeight" runat="server" Width="50px"></asp:TextBox>-->
                <!--<asp:TextBoxWatermarkExtender ID="txtVolHeight_TextBoxWatermarkExtender" 
                    runat="server" Enabled="True" TargetControlID="txtVolHeight" WatermarkText="Height">
                </asp:TextBoxWatermarkExtender>-->
                <asp:DropDownList ID="ddlVolMeasure" runat="server">
                <asp:ListItem>Cmt</asp:ListItem>
                <asp:ListItem>Inch</asp:ListItem>
                </asp:DropDownList>
                </td>
        </tr>
    </table>
    </div>
    
   
    
    <asp:Button ID="btnAddEquip" runat="server" CssClass="button" 
        onclick="btnAddEquip_Click" Text="AddEquipment Details" Visible="False" />
    
    
    <div  class="ltfloat">
        <asp:GridView ID="dgAcEquip" runat="server" AutoGenerateColumns="False" 
                                    ShowFooter="True"   Width="40%" Height="82px" >
            <Columns>
               
                <asp:TemplateField HeaderText="TailNumber" HeaderStyle-Wrap="false">
                    <ItemTemplate>
                     
                        <asp:TextBox  ID="txtTailNumber"  runat="server"   ></asp:TextBox>
                                                         
                        <asp:RequiredFieldValidator ID="valtxtTailNumber" runat="server" 
                            ControlToValidate="txtTailNumber" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                         
                    </ItemTemplate>
                    
                    <HeaderStyle Wrap="True"></HeaderStyle>
                    <ItemStyle Wrap="False" />
                             
             
               </asp:TemplateField>
               
               
                <asp:TemplateField HeaderText="Status" HeaderStyle-Wrap="true">
               
                
                
                  <ItemTemplate>
                        <asp:DropDownList ID="ddlStatus" runat="server">
                        <asp:ListItem Text="Select"></asp:ListItem>
                        <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                         <asp:ListItem Text="Inactive" Value="Inactive"></asp:ListItem>
                        </asp:DropDownList>
                       
                        <asp:RequiredFieldValidator ID="rfddlStatus" runat="server" 
                            ControlToValidate="ddlStatus" ErrorMessage="*" InitialValue="Select"></asp:RequiredFieldValidator>
                       
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
       <div id="fotbut">
        <asp:Button ID="btnSave" Text="Save" runat="server" onclick="btnSave_Click" 
        CssClass="button"   />
       </div> 
    

</div>
</asp:Content>
