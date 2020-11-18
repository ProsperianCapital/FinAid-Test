<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangeAddress.aspx.cs" Inherits="PCIWebFinAid.pgChangeAddress" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104214"></asp:Literal>
</div>

<b><asp:Literal runat="server" ID="Y104215" Text="Lbl 104215" /></b>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104216" /></td>
	<td><asp:Literal runat="server" ID="lblLine1">Current Addr Line 1</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104218" /></td>
	<td><asp:Literal runat="server" ID="lblLine2">Current Addr Line 2</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104220" /></td>
	<td><asp:Literal runat="server" ID="lblLine3">Current Addr Line 3</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104222" /></td>
	<td><asp:Literal runat="server" ID="lblLine4">Current Addr Line 4</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104224" /></td>
	<td><asp:Literal runat="server" ID="lblLine5">Current Addr Line 5</asp:Literal></td></tr>
</table>
<hr />
<b><asp:Literal runat="server" ID="Y104226" Text="Lbl 104226" /></b>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="Y104216" /></td>
	<td><asp:TextBox runat="server" ID="txtLine1" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104218" /></td>
	<td><asp:TextBox runat="server" ID="txtLine2" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104220" /></td>
	<td><asp:TextBox runat="server" ID="txtLine3" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104222" /></td>
	<td><asp:TextBox runat="server" ID="txtLine4" Width="400px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104224" /></td>
	<td><asp:TextBox runat="server" ID="txtLine5" Width="400px"></asp:TextBox></td></tr>
</table>
<br />
<asp:Button runat="server" id="X104237" />
<br /><br />
<asp:Label runat="server" ID="X104356" CssClass="Error"></asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>