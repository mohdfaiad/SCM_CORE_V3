<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebUserControl1.ascx.cs" Inherits="ProjectSmartCargoManager.WebUserControl1" %>
<script type="text/javascript">
    function GetName() {
        var k = "<%= ConfigurationManager.AppSettings["LinkPath"].ToString() %>"
        return k
    }
</script>