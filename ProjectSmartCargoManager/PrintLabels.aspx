<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PrintLabels.aspx.cs" Inherits="ProjectSmartCargoManager.PrintLabels" %>

<html>
<head>
    <title></title>

    <script type="text/javascript">

        function Go() {

            var PrintCommand = '<object ID="PrintControl" WIDTH=0 HEIGHT=0 CLASSID="CLSID:8856F961-340A-11D0-A96B-00C04FD705A2"></object>';
            var count = parseInt(document.getElementById('<%=HidPcs.ClientID%>').value);

            for (var i = 1; i <= count; i++) {

                document.body.insertAdjacentHTML('beforeEnd', PrintCommand);
                document.getElementById('<%=LBLPcs.ClientID%>').innerHTML = "" + i + "/" + count;

                document.getElementById('BarcodeImg').src = "GetBarcodeImage.aspx";

                PrintControl.ExecWB(6, 2);
                PrintControl.outerHTML = "";

            }

        }


        function sleep(milliseconds) {
            var start = new Date().getTime();
            for (var i = 0; i < 1e7; i++) {
                if ((new Date().getTime() - start) > milliseconds) {
                    break;
                }
            }
        }
        
    
    </script>

</head>
<body onload="javascript:Go()">
    <form id="Form1" runat="server">
    <table style="width: 260px; height: 220px; border: solid black 2; left: 0px; top: 0px;
        position: absolute" cellspacing="0">
        <tr style="border: solid black 2">
            <td colspan="2" align="center" style="border: solid black 2">
                <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="22pt" Text="JET AIRWAYS"></asp:Label>
                <asp:HiddenField ID="HidPcs" runat="server" />
            </td>
        </tr>
        <tr style="border: solid black 2">
            <td style="border: solid black 2">
                &nbsp;
                <asp:Label ID="LBLAWBPartI" runat="server" Text="11000"></asp:Label>
                &nbsp;<asp:Label ID="LBLAWBPartII" runat="server" Font-Bold="True" Font-Size="18pt"
                    Text="0012"></asp:Label>
            </td>
            <td rowspan="2" style="border: solid black 2">
                &nbsp; &nbsp;
                <asp:Label ID="LBLDestination" runat="server" Font-Bold="True" Font-Overline="False"
                    Font-Size="40pt" Text="DEL"></asp:Label>
            </td>
        </tr>
        <tr style="border: solid black 2">
            <td style="border: solid black 2">
                &nbsp;
                <asp:Label ID="Label4" runat="server" Text="Pcs"></asp:Label>
                &nbsp;&nbsp;
                <asp:Label ID="LBLPcs" runat="server" Font-Bold="True" Font-Size="19pt" Text="2/2"></asp:Label>
            </td>
        </tr>
        <tr style="border: solid black 2">
            <td style="border: solid black 2">
                <asp:Label ID="Label6" runat="server" Text="Org"></asp:Label>
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="LBLOrg" runat="server" Font-Bold="True" Font-Size="19pt" Text="BOM"></asp:Label>
            </td>
            <td style="border: solid black 2">
                <asp:Label ID="Label8" runat="server" Text="Via"></asp:Label>
                &nbsp;&nbsp;&nbsp;
                <asp:Label ID="LBLVia" runat="server" Font-Bold="True" Font-Size="19pt" Text="AMD"></asp:Label>
            </td>
        </tr>
        <tr style="border: solid black 2">
            <td colspan="2" align="center" style="border: solid black 2">                
                <img id="BarcodeImg" style="width:110px; height:34px;" src="GetBarcodeImage.aspx" alt="" />
                <br />
                <asp:Label ID="LBLDate" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>



