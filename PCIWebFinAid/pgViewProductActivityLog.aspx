<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewProductActivityLog.aspx.cs" Inherits="PCIWebFinAid.pgViewProductActivityLog" %>
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
<asp:Literal runat="server" ID="X104128"></asp:Literal>
</div>

<asp:Table runat="server" ID="tblActivity" style="border:1px solid #000000">
<asp:TableRow>
	<asp:TableCell ID="X104134" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell>
	<asp:TableCell ID="X104136" style="border-bottom:1px solid #000000;font-weight:bold"></asp:TableCell></asp:TableRow>
</asp:Table>
<br /><br />
<asp:Label runat="server" ID="X104140" CssClass="Error"></asp:Label>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>