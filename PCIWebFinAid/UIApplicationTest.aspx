<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="UIApplicationTest.aspx.cs" Inherits="PCIWebFinAid.UIApplicationTest" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<script type="text/javascript">
</script>

<form id="frmHome" runat="server">

	<div class="Header3">
	Prosperian Integration Testing
	</div>

	<asp:RadioButton runat="server" id="rdoJSON" GroupName="rdoP" Checked="true" />POST JSON data<br />
	<asp:RadioButton runat="server" id="rdoXML"  GroupName="rdoP" />POST XML data<br />
	<asp:RadioButton runat="server" id="rdoWeb"  GroupName="rdoP" />POST web form data
	<br /><br />
	Input data<br />
	<asp:TextBox runat="server" ID="txtIn" TextMode="MultiLine" Height="120" Width="480px" Rows="5">
	
	</asp:TextBox>

	<hr />

	<div class="ButtonBox">
		<asp:Button runat="server" ID="btnOK" Text="OK" OnClick="btnOK_Click" />&nbsp;
	</div>
	<p class="error">
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>
	Output data<br />
	<asp:TextBox runat="server" ID="txtOut" TextMode="MultiLine" Height="120" Width="480px" Rows="5" ReadOnly="true"></asp:TextBox>
</form>
</body>
</html>