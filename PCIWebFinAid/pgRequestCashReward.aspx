<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgRequestCashReward.aspx.cs" Inherits="PCIWebFinAid.pgRequestCashReward" %>
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
<asp:Literal runat="server" ID="X104033">104033</asp:Literal>
</div>

<table>
<tr>
	<td><asp:Literal runat="server" ID="X104034">104034</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtAmount" style="width:160px"></asp:TextBox></td></tr>
<tr>
	<td colspan="2" class="DataStatic">
		<asp:Literal runat="server" ID="X104056">104056</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104061">104061</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtBank" style="width:360px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104063">104063</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtAccName" style="width:360px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104067">104067</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtAccNumber" style="width:360px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104069">104069</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtBranchName" style="width:360px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104077">104077</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtBranchCode" style="width:360px"></asp:TextBox></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104073">104073</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtSwift" style="width:360px"></asp:TextBox></td></tr>
</table>
<br />
<asp:Button runat="server" id="X104048" Text="104048" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104050" CssClass="Info">104050</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>