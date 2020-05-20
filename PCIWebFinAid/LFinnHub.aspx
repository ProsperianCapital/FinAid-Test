<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="LFinnHub.aspx.cs" Inherits="PCIWebFinAid.LFinnHub" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="Header.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="Footer.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Menu"   Src="Menu.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<!--#include file="IncludeMain.htm" -->
	<title>FinnHub Financial Data</title>
	<link rel="stylesheet" href="CSS/FinAid.css" type="text/css" />
	<link rel="shortcut icon" href="Images/favicon.ico" />
	<meta content="width=device-width, initial-scale=1, maximum-scale=1" name="viewport" />
	<asp:Literal runat="server" ID="lblRefresh"></asp:Literal>
</head>
<body>
<form id="frmMain" runat="server">
<ascx:Header runat="server" ID="ascxHeader" />
<ascx:Menu   runat="server" ID="ascxMenu" />
<table style="width:100%">
<tr>
<td style="width:1%;vertical-align:top;background-color:lightgrey;color:black;padding:10px;white-space:nowrap">
<asp:Label runat="server" ID="X104002" Font-Bold="true" Font-Size="24px">Admin</asp:Label>
<br /><br /><br />
<asp:Label runat="server" ID="X104004" Font-Bold="true">Prosperian Capital</asp:Label><br />
4 Cybercity<br />
4’th Floor<br />
Ebène Heights<br />
Ebène<br />
Mauritius<br />
72201
<br /><br /><br />
<asp:Literal runat="server" ID="lblDate"></asp:Literal>
</td>
<td style="vertical-align:top;padding-left:10px">

<div class="Header4">FinnHub Stock Ticker</div>
<br />

<table>
<tr>
	<td>FinnHub API Key</td>
	<td>
		<asp:TextBox runat="server" ID="txtKey" Width="200px" value="bqvq9gnrh5rapls47e8g"></asp:TextBox></td></tr>
<tr>
	<td>Stock Symbol(s)<br />Separated by commas<br />Eg. AAPL,MSFT,GOOGL</td>
	<td><asp:TextBox runat="server" ID="txtStock" Width="200px" TextMode="MultiLine" Rows="4"></asp:TextBox></td></tr>
<tr>
	<td>Refresh every</td>
	<td><asp:TextBox runat="server" ID="txtRefresh" Width="30px" value="10"></asp:TextBox> seconds</td></tr>
<tr>
	<td>Status</td><td><b><asp:Literal runat="server" ID="lblStatus"></asp:Literal></b></td></tr>
<tr>
	<td colspan="2"><br />
		<asp:Button runat="server" ID="btnStart" Text="Start"  OnClick="btnStart_Click" />
		<asp:Button runat="server" ID="btnStop"  Text="Stop"   OnClick="btnStop_Click" />
		<asp:Button runat="server" ID="btnClear" Text="Clear"  OnClick="btnClear_Click" /></td></tr>
<tr>
	<td colspan="2" class="Error"><asp:Label ID="lblErr" runat="server"></asp:Label></td></tr>
</table>

</td><td style="vertical-align:top;padding-left:10px;border:1px solid #000000">
	<asp:Literal runat="server" id="lblData"></asp:Literal>
</td></tr>
</table>

<!--#include file="IncludeLogin2.htm" -->

<ascx:Footer runat="server" ID="ascxFooter" />
</form>
</body>
</html>