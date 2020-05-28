<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="XHome.aspx.cs" Inherits="PCIWebFinAid.XHome" %>
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
<style>
.VHead {
	font-weight: bold;
	background-color: orange;
	padding: 5px;
}
.VMenu {
	background-color: lightgreen;
	padding: 5px;
}
.VMenu a {
	color: black;
	text-decoration: none;
}
.VMenu:hover {
	background-color: red;
}
.VMenu a:hover {
	color: white;
	text-decoration: none;
}
</style>
<script type="text/javascript">
var mActive = new Array('','','','');
function EltOffSet(eltID)
{
	var x = 0;
	var y = 0;
	var p = GetElt(eltID);
	while ( p && !isNaN(p.offsetLeft) && !isNaN(p.offsetTop) )
	{
		x += p.offsetLeft - p.scrollLeft;
		y += p.offsetTop  - p.scrollTop;
		p = p.offsetParent;
	}
	return { top: y, left:x };
}
function XMenu(mID,mLevel,mShow)
{
	ShowElt(mActive[mLevel],false);
	ShowElt(mID,(mShow>0));
	mActive[mLevel] = mID;
}
</script>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<form id="frmMain" runat="server">
	<div style="float:left;vertical-align:top;padding:5px;margin-right:5px;background-color:pink">
		<table id="m01" style="position:absolute;left:150px;visibility:hidden;display:none;border:1px #000000 solid" onmouseleave="JavaScript:XMenu('',1,0)">
			<tr><td class="VHead">Prosperian Capital</td></tr>
			<tr><td class="VMenu"><a href="XHome.aspx"> Dashboard </a></td></tr>
			<tr><td class="VMenu"> FinAudit </td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Cashbooks </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> General Journal </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Cashbook Recon </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Purchases </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Payments </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Intra-Group Billing </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Processing </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Loan Account Recon </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Reports & Extracts </a></td></tr>
		</table>
		<a href="JavaScript:XMenu('m01',1,1)"><img src="Images/PCapital.png" height="75" onmouseover="JavaScript:XMenu('m01',1,1)" /></a>
		<br /><br />
		<table id="m02" style="position:absolute;left:150px;visibility:hidden;display:none;border:1px #000000 solid" onmouseleave="JavaScript:XMenu('',1,0)">
			<tr><td class="VHead">Prosperian FinTech</td></tr>
			<tr><td class="VMenu"><a href="XHome.aspx"> Dashboard </a></td></tr>
			<tr><td class="VMenu"> eServe CRM </td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Contract Lookup </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Transaction Lookup </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Loyalty Rewards </a></td></tr>
			<tr><td class="VMenu"> PPC Costing </td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Marketing Return </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Vintage Analysis </a></td></tr>
		</table>
		<a href="JavaScript:XMenu('m02',1,1)"><img src="Images/PFintech.png" height="75" onmouseover="JavaScript:XMenu('m02',1,1)" /></a>
		<br /><br />
		<table id="m03" style="position:absolute;left:150px;visibility:hidden;display:none;border:1px #000000 solid" onmouseleave="JavaScript:XMenu('',1,0)">
			<tr><td class="VHead">Prosperian Wealth</td></tr>
			<tr><td class="VMenu"><a href="XHome.aspx"> Dashboard </a></td></tr>
			<tr><td class="VMenu"> Interactive Brokers </td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Account Summary </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Portfolio Summary </a></td></tr>
			<tr><td class="VMenu">&nbsp;&nbsp;-> <a href="XHome.aspx"> Orders Review </a></td></tr>
			<tr><td class="VMenu"><a href="XHome.aspx"> Currency Exchange </a></td></tr>
			<tr><td class="VMenu"><a href="XHome.aspx"> Stock Tickers </a></td></tr>
		</table>
		<a href="JavaScript:XMenu('m03',1,1)"><img src="Images/PWealth.png" height="75" onmouseover="JavaScript:XMenu('m03',1,1)" /></a>
		<br /><br />
		<table id="m04" style="position:absolute;left:150px;visibility:hidden;display:none;border:1px #000000 solid" onmouseleave="JavaScript:XMenu('',1,0)">
			<tr><td class="VHead">KNAB Global</td></tr>
			<tr><td class="VMenu"><a href="XHome.aspx"> Dashboard </a></td></tr>
			<tr><td class="VMenu"><a href="XHome.aspx"> Transactions </a></td></tr>
		</table>
		<a href="JavaScript:XMenu('m04',1,1)"><img src="Images/PKnab.png" width="130" onmouseover="JavaScript:XMenu('m04',1,1)" /></a>
		<br /><br />
		<hr />
		<br />
		<div style="text-align:center">
		<a href="XLogin.aspx" style="font-size:20px;font-weight:bold;text-decoration:none" onmouseover="JavaScript:XMenu('',1,0)" title="Close all resources and log out">Log Off</a>
		</div>
		<br />
	</div>
	<div class="Header3" style="margin-top:2px">
	Home
	</div>
	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>
