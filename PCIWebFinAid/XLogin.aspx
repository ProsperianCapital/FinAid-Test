<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="XLogin.aspx.cs" Inherits="PCIWebFinAid.XLogin" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainAdmin.htm" -->
	<title>Prosperian Back Office</title>
	<link rel="stylesheet" href="CSS/BackOffice.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<form id="frmLogin" runat="server">
	<div class="Header3" style="margin-top:2px">
	Back Office
	</div>
	<p style="text-align:center">
	<asp:TextBox runat="server" ID="txtID" placeholder="User ID" width="300px"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:TextBox runat="server" ID="txtPW" placeholder="Password" TextMode="Password" Width="300px"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:Button runat="server" ID="btnLogin" Text="LOGIN" OnClick="btnLogin_Click" Width="310px" />
	</p><p style="text-align:center">
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>
	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>
