<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="NSTOX.WebService.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="refresh" content="60" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button runat="server" OnClick="Log_Click" Text="Log" />
            <asp:TextBox runat="server" ID="txtID" />
            <asp:Button Text="Process" runat="server" OnClick="Process_Click" />
        </div>
    </form>
</body>
</html>
