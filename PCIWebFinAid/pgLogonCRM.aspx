<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgLogonCRM.aspx.cs" Inherits="PCIWebFinAid.pgLogonCRM" %>
<%@ Register TagPrefix="ascx" TagName="XHeader" Src="XHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="XFooter" Src="XFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCRM.htm" -->
</head>
<body>
<ascx:XHeader runat="server" ID="ascxXHeader" />
<form id="frmLogin" runat="server">
	<div class="Header3" style="margin-top:2px;text-align:center">
	CareAssist
	</div>
	<p style="text-align:center">
	<asp:TextBox runat="server" ID="txtID" placeholder="User Name" width="300px" Height="25px"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:TextBox runat="server" ID="txtPW" placeholder="Password" TextMode="Password" Width="300px" Height="25px"></asp:TextBox>
	</p><p style="text-align:center">
	<asp:Button runat="server" ID="btnLogin" Text="LOGIN" OnClick="btnLogin_Click" Width="310px" />
	</p>

	<p style="text-align:center">
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>

	<!--#include file="IncludeErrorDtl.htm" -->
	<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>
