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
<script type="text/javascript">
function EditMode(editInsert)
{
//	editInsert values:
//	0 : Hide
//	1 : Show, edit
//	2 : Show, insert
	HidePopups();
	var g = '&nbsp;<img src="Images/Close1.png" style="float:right" title="Close" onclick="JavaScript:EditMode(0)" />';
	if ( editInsert == 1 )
		SetEltValue('lblEditHead','Edit eWallet'+g);
	else
		SetEltValue('lblEditHead','Create eWallet'+g);
	ShowElt('pnlEdit',(editInsert> 0));
	SetEltValue('hdnEditInsert',editInsert);
}
function HidePopups()
{
}
</script>
<form id="frmMain" runat="server">
	<script type="text/javascript" src="JS/Calendar.js"></script>

	<ascx:XMenu runat="server" ID="ascxXMenu" />

	<div class="Header3">
	My eWallets
	</div>

	<div class="Wallet1" style="border:1px solid #000000;background-color:lightgrey;padding:10px;width:350px;height:200px;display:inline-block;border-radius:10px">
	<table style="width:100%">
	<tr>
		<td>
			Currency<br />
			<img src="Images/Flag-USA.png" title="US Dollars" style="width:60px" /></td>
		<td>
			<br />
			<span class="Wallet3">USD</span></td>
		<td>
			Balance<br />
			<span class="Wallet3">38 901.68</span></td>
		<td style="float:right">
			<img src="Images/PayPayYa.png" style="width:50px" /><br />
			e-Wallet</td>
	</tr>
	</table>
	Account Number<br />
	<span class="Wallet4">1234 5678 9012 3456</span>
	<p>
	Account Description<br />
	<span style="float:right"><img style="width:50px" src="Images/PlaNet-Image.png" />&nbsp;</span>
	<a href="JavaScript:EditMode(1)" class="Wallet2">My PayPayYa USD eWallet</a><br />
	</p><p>
	Account Name<br />
	<span class="Wallet2">Samual Briggs</span>
	</p>
	</div>

	<br /><br />

	<div class="Wallet1" style="border:1px solid #000000;background-color:lightgrey;padding:10px;width:350px;height:200px;display:inline-block;border-radius:10px">
	<table style="width:100%">
	<tr>
		<td>
			Currency<br />
			<img src="Images/Flag-EUR.png" title="Euros" style="width:60px" /></td>
		<td>
			<br />
			<span class="Wallet3">EUR</span></td>
		<td>
			Balance<br />
			<span class="Wallet3">1 903.44</span></td>
		<td style="float:right">
			<img src="Images/PayPayYa.png" style="width:50px" /><br />
			e-Wallet</td>
	</tr>
	</table>
	Account Number<br />
	<span class="Wallet4">8045 6723 0198 3755</span>
	<p>
	Account Description<br />
	<span style="float:right"><img style="width:50px" src="Images/PlaNet-Image.png" />&nbsp;</span>
	<a href="JavaScript:EditMode(1)" class="Wallet2">My PayPayYa EUR eWallet</a><br />
	</p><p>
	Account Name<br />
	<span class="Wallet2">Samual Briggs</span>
	</p>
	</div>

	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

	<div class="Popup2" id="pnlEdit" style="visibility:hidden;display:none">
	<asp:HiddenField runat="server" ID="hdnEditInsert" />
	<div class="Header6" id="lblEditHead"></div>
	<p>
	Wallet Account Number<br /><asp:TextBox runat="server" ID="txtAccNo" Width="200px" ReadOnly="true">1234 5678 9012 3456</asp:TextBox>
	</p><p>
	Wallet Currency<br />
	<asp:DropDownList runat="server" ID="lstCurrency" style="width:200px">
		<asp:ListItem Value="AUD" Text="Australian Dollar"></asp:ListItem>
		<asp:ListItem Value="NZD" Text="New Zealand Dollar"></asp:ListItem>
		<asp:ListItem Value="USD" Text="US Dollar"></asp:ListItem>
		<asp:ListItem Value="EUR" Text="Euro"></asp:ListItem>
		<asp:ListItem Value="GBP" Text="British Pound"></asp:ListItem>
		<asp:ListItem Value="SGD" Text="Singapore Dollar"></asp:ListItem>
	</asp:DropDownList>
	</p><p>
	Wallet Description<br /><asp:TextBox runat="server" ID="txtDescr" Width="400px">My USD eWallet blah</asp:TextBox>
	</p>
	<asp:Button runat="server" ID="btnSave" Text="Save" title="Save your changes" />&nbsp;
	<input type="button" value="Cancel" id="btnCancel" title="Exit without saving" onclick="JavaScript:EditMode(0)" />
	<asp:Label runat="server" id="lblErr2" cssclass="Error"></asp:Label>
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>