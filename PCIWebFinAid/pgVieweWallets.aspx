<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgVieweWallets.aspx.cs" Inherits="PCIWebFinAid.pgVieweWallets" EnableEventValidation="false" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XMenu"   Src="XMenu.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<!--#include file="IncludeBusy.htm" -->
<form id="frmMain" runat="server">
	<script type="text/javascript" src="JS/Calendar.js"></script>

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	My eWallets
	</div>

	<div style="border:1px solid #000000;background-color:lightgrey;padding:10px;width:600px;display:inline-block">
	<table style="width:90%">
	<tr>
		<td>
			Currency<br />
			<img src="Images/Flag-USA.png" title="US Dollars" style="width:60px" /></td>
		<td style="font-size:16px;font-weight:bold">
			<br />
			USD</td>
		<td>
			Balance<br />
			<span style="font-size:16px;font-weight:bold">38 901.68</span></td>
		<td>
			<img src="Images/eWallet.png" style="width:50px" /><br />
			e-Wallet</td>
	</tr>
	</table>
	Account Number<br />
	<span style="font-size:22px;font-weight:bold">1234 5678 9012 3456</span><br />
	<br />
	Account Description<br />
	<span style="font-size:22px;font-weight:bold">My PayPayYa USD eWallet</span><br />
	<img src="Images/PlaNet.png" style="float:right;width:200px" />
	<br />
	Account Name<br />
	<span style="font-size:22px;font-weight:bold">Samual Briggs</span>
	</div>

	<br /><br />

	<div style="border:1px solid #000000;background-color:lightgrey;padding:10px;width:600px;display:inline-block">
	<table style="width:90%">
	<tr>
		<td>
			Currency<br />
			<img src="Images/Flag-EUR.png" title="Euros" style="width:60px" /></td>
		<td style="font-size:16px;font-weight:bold">
			<br />
			EUR</td>
		<td>
			Balance<br />
			<span style="font-size:16px;font-weight:bold">9 012.77</span></td>
		<td>
			<img src="Images/eWallet.png" style="width:50px" /><br />
			e-Wallet</td>
	</tr>
	</table>
	Account Number<br />
	<span style="font-size:22px;font-weight:bold">8445 6342 1099 0456</span><br />
	<br />
	Account Description<br />
	<span style="font-size:22px;font-weight:bold">My PayPayYa EUR eWallet</span><br />
	<img src="Images/PlaNet.png" style="float:right;width:200px" />
	<br />
	Account Name<br />
	<span style="font-size:22px;font-weight:bold">Samual Briggs</span>
	</div>

	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>