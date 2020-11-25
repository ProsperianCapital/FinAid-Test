<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="RegisterThreeD.aspx.cs" Inherits="PCIWebFinAid.RegisterThreeD" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<!--#include file="IncludeMainRegister.htm" -->
	<title>Secure Registration</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>

<div class="Header3">
<asp:Literal runat="server" id="lbl100503">Thank You ...</asp:Literal>
</div>
<p style="color:red;font-weight:bold">
<br />
<asp:Literal runat="server" id="lbl100504">Your application has been successfully received.</asp:Literal>
</p>
</body>
</html>