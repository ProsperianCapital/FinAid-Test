<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewCurrentBalances.aspx.cs" Inherits="PCIWebFinAid.pgViewCurrentBalances" %>
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
<asp:Literal runat="server" ID="X104095">104095</asp:Literal>
</div>

<table>
<tr>
	<td><asp:Literal runat="server" ID="X104096">104096</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblRegFee">lblRegFee</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104106">104106</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblMonthlyFee">lblMonthlyFee</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104098">104098</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblGrantLimit">lblGrantLimit</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104102">104102</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblGrantAvail">lblGrantAvail</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104104">104104</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblGrantStatus">lblGrantStatus</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104108">104108</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblFeeDate">lblFeeDate</asp:Literal></td></tr>
</table>
<br /><br />
<asp:Label runat="server" ID="X104110" CssClass="Info">104110</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>