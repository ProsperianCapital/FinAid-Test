<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="false" CodeBehind="UIApplicationQuery.aspx.cs" Inherits="PCIWebFinAid.UIApplicationQuery" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html>
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<p style="color:red;font-size:large">
Oops, something seems to have gone badly wrong. Please click the link below to continue.
</p><p>
<a href="pgLogon.aspx">Propserian BackOffice Login</a>
</p>
<ascx:XFooter runat="server" ID="ascxXFooter" />
</body>
</html>