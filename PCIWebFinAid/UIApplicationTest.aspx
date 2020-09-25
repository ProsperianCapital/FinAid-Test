<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="UIApplicationTest.aspx.cs" Inherits="PCIWebFinAid.UIApplicationTest" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainAdmin.htm" -->
</head>
<body>
<script type="text/javascript">
function SetType()
{
	ShowElt('txtJSON',GetElt('rdoJSON').checked);
	ShowElt('txtXML' ,GetElt('rdoXML').checked);
	ShowElt('txtWeb' ,GetElt('rdoWeb').checked);
	ShowElt('txtForm',GetElt('rdoForm').checked);
}
</script>

<form id="frmHome" runat="server">

	<div class="Header3">
	Prosperian Integration Testing
	</div>

	<asp:RadioButton runat="server" id="rdoJSON" GroupName="rdoP" onclick="JavaScript:SetType()" checked="true" />POST JSON data<br />
	<asp:RadioButton runat="server" id="rdoXML"  GroupName="rdoP" onclick="JavaScript:SetType()" />POST XML data<br />
	<asp:RadioButton runat="server" id="rdoWeb"  GroupName="rdoP" onclick="JavaScript:SetType()" />POST form data<br />
	<asp:RadioButton runat="server" id="rdoForm" GroupName="rdoP" onclick="JavaScript:SetType()" />SUBMIT web form
	<br /><br />
	Input data<br />
	<asp:TextBox runat="server" ID="txtJSON" TextMode="MultiLine" Height="140" Width="800px" Rows="8"></asp:TextBox>
	<asp:TextBox runat="server" ID="txtXML"  TextMode="MultiLine" Height="140" Width="800px" Rows="8" style="visibility:hidden;display:none"></asp:TextBox>
	<asp:TextBox runat="server" ID="txtWeb"  TextMode="MultiLine" Height="140" Width="800px" Rows="8" style="visibility:hidden;display:none"></asp:TextBox>
	<asp:TextBox runat="server" ID="txtForm" TextMode="MultiLine" Height="140" Width="800px" Rows="8" style="visibility:hidden;display:none"></asp:TextBox>
	<br /><br />
	Target URL<br />
	<asp:TextBox runat="server" ID="txtURL" Width="800px" ReadOnly="True"></asp:TextBox>

	<p class="ButtonBox">
		<asp:Button runat="server" ID="btnOK" Text="OK" OnClick="btnOK_Click" />&nbsp;
	</p>

	<hr />

	<p class="error">
	<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>
	</p>
	Output data<br />
	<asp:TextBox runat="server" ID="txtOut" TextMode="MultiLine" Height="140" Width="800px" Rows="8" ReadOnly="true"></asp:TextBox>
</form>

<script type="text/javascript">
SetType();
</script>
</body>
</html>