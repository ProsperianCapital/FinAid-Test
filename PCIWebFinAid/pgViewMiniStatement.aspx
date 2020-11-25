<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewMiniStatement.aspx.cs" Inherits="PCIWebFinAid.pgViewMiniStatement" %>
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
<asp:Literal runat="server" ID="X104117">104117</asp:Literal>
</div>

<asp:Table runat="server" ID="tblStatement" style="border:1px solid #000000">
<asp:TableRow>
	<asp:TableCell ID="X104118" style="border-bottom:1px solid #000000;font-weight:bold">104118</asp:TableCell>
	<asp:TableCell ID="X104119" style="border-bottom:1px solid #000000;font-weight:bold">104119</asp:TableCell>
	<asp:TableCell ID="X104120" style="border-bottom:1px solid #000000;font-weight:bold">104120</asp:TableCell>
	<asp:TableCell ID="X104121" style="border-bottom:1px solid #000000;font-weight:bold">104121</asp:TableCell></asp:TableRow>
</asp:Table>
<br />
<asp:Literal runat="server" ID="X104360">104360</asp:Literal>
<asp:TextBox runat="server" Width="160px" ReadOnly="true" ID="txtBalance"></asp:TextBox>
<br /><br />
<asp:Label runat="server" ID="X104126" CssClass="Error">104126</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>