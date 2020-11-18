<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewCurrentBalances.aspx.cs" Inherits="PCIWebFinAid.pgViewCurrentBalances" %>
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
<asp:Literal runat="server" ID="X104095"></asp:Literal>
</div>

<table>
<tr>
	<td><asp:Literal runat="server" ID="X104096" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtRegFee"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104106" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtMonthlyFee"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104098" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtGrantLimit"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104102" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtGrantAvail"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104104" /></td>
	<td><asp:TextBox runat="server" Width="480px" ReadOnly="true" ID="txtGrantStatus"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104108" /></td>
	<td><asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtFeeDate"></asp:TextBox></td></tr>
</table>
<br /><br />
<asp:Label runat="server" ID="X104110" CssClass="Error"></asp:Label>

<!-- OLD Start -->
<div id="div02" style="display:none;visibility:hidden">
<asp:Label runat="server" ID="X104117" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Table runat="server" ID="tblStatement" style="border:1px solid #000000">
<asp:TableRow>
	<asp:TableCell ID="X104118" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104119" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104120" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104121" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell></asp:TableRow>
</asp:Table>
<br />
<asp:Literal runat="server" ID="X104360" />
<asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtBalance"></asp:TextBox>
<br /><br />
<asp:Label runat="server" ID="X104126" CssClass="Error"></asp:Label>
</div>

<div id="div03" style="display:none;visibility:hidden">
<asp:Label runat="server" ID="X104128" CssClass="Header4"></asp:Label>
<br /><br />
<asp:Table runat="server" ID="tblActivity" style="border:1px solid #000000">
<asp:TableRow>
	<asp:TableCell ID="X104134" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104136" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell></asp:TableRow>
</asp:Table>
<br /><br />
<asp:Label runat="server" ID="X104140" CssClass="Error"></asp:Label>
</div>

<!-- OLD End -->

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>