<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangePIN.aspx.cs" Inherits="PCIWebFinAid.pgChangePIN" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCRM.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104197">104197</asp:Literal>
</div>

<table>
<tr>
	<td><asp:Literal runat="server" ID="X104198">104198</asp:Literal></td>
	<td><asp:TextBox runat="server" Width="120px" ID="txtPINOld"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104200">104200</asp:Literal></td>
	<td><asp:TextBox runat="server" Width="120px" ID="txtPINNew1"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104202">104202</asp:Literal></td>
	<td><asp:TextBox runat="server" Width="120px" ID="txtPINNew2"></asp:TextBox></td></tr>
</table>
<br /><br />
<asp:Button runat="server" id="X104204" Text="104204" />
<br /><br />
<asp:Label runat="server" ID="X104355" CssClass="Error">104355</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>