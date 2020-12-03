<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangeEMail.aspx.cs" Inherits="PCIWebFinAid.pgChangeEMail" %>
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

<script type="text/javascript">
function Validate(eltID,eltType)
{
	try
	{
		var img = GetElt('img'+eltID);
		var val = GetEltValue('txt'+eltID);
		var dir = '<%=PCIBusiness.Tools.ImageFolder() %>';
		var ok  = false;
		if ( eltType == 1 ) // EMail
			ok = ValidEmail(val);
		else if ( eltType == 2 ) // Phone
			ok = ValidPhone(val,0);
		if (ok)
			img.src = dir + 'Tick.png';
		else
		{
			img.src = dir + 'Cross.png';
			DisableElt('X104258',true);
		}
		return ok;
	}
	catch (x)
	{ }
	return false;
}
</script>

<div class="Header3">
<asp:Literal runat="server" ID="X104247">104247</asp:Literal>
</div>

<table>
<tr>
	<td colspan="2" class="Header7"><asp:Literal runat="server" ID="X104248">104248</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104249">104249</asp:Literal></td>
	<td><asp:Literal runat="server" ID="lblEMail"></asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104251">104251</asp:Literal></td>
	<td><asp:Literal runat="server" ID="lblPhone"></asp:Literal></td></tr>
<tr>
	<td colspan="2"><hr /></td></tr>
<tr>
	<td colspan="2" class="Header7"><asp:Literal runat="server" ID="X104253">104253</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104254">104254</asp:Literal></td>
	<td>
		<asp:TextBox runat="server" Width="480px" ID="txtEMail" OnKeyUp="JavaScript:Validate('EMail',1)"></asp:TextBox>&nbsp;
		<img id="imgEMail" />
	</td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104256">104256</asp:Literal></td>
	<td>
		<asp:TextBox runat="server" Width="480px" ID="txtPhone" OnKeyUp="JavaScript:Validate('Phone',2)"></asp:TextBox>&nbsp;
		<img id="imgPhone" />
	</td></tr>
</table>
<br />
<asp:Button runat="server" id="X104258" Text="104258" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104357" CssClass="Info">104357</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
if ( Validate('EMail',1) && Validate('Phone',2) )
	DisableElt('X104258',false);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>