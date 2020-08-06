<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="RegisterThreeD.aspx.cs" Inherits="PCIWebFinAid.RegisterThreeD" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<!--#include file="IncludeMain.htm" -->
	<title>Secure Registration</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
</head>
<body>

<asp:PlaceHolder runat="server" ID="pnlOK">
<div class="Header3">
	Success! Thank You ...
</div>
<p style="color:red;font-weight:bold">
<br />
An initial verification payment has been processed on your account.
</p><p>
<asp:Literal runat="server" ID="lblData"></asp:Literal>
</p>
</asp:PlaceHolder>

<asp:PlaceHolder runat="server" ID="pnlError">
<div class="Header3">
	Oops! Error ...
</div>
<p style="color:red;font-weight:bold">
<br />
Something seems to have gone wrong.
</p><p>
<asp:Literal runat="server" ID="lblErr"></asp:Literal>
</p>
</asp:PlaceHolder>

<br />
<hr />
<input type="button" class="Button" value="Home" onclick="JavaScript:location.href='RegisterEx3.aspx'" />

</body>
</html>