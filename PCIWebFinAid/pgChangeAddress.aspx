<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgChangeAddress.aspx.cs" Inherits="PCIWebFinAid.pgChangeAddress" %>
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
function Validate(n,mode)
{
	try
	{
		var img = GetElt('img'+n.toString());
		var adr = GetEltValue('txtLine'+n.toString());
		var dir = '<%=PCIBusiness.Tools.ImageFolder() %>';
		if ( adr.length == 0 && mode == 1 )
			img.src = '';
		else if ( adr.length < 3 && adr.length > 0 )
			img.src = dir + 'Cross.png';
		else if ( adr.length < 3 && n < 3 )
			img.src = dir + 'Cross.png';
		else
			img.src = dir + 'Tick.png';

		var bad = false;
		for ( var k = 1 ; k < 6 ; k++ )
			if ( GetElt('img'+k.toString()).src.indexOf('Cross') > 0 )
				bad = true;
		DisableElt('X104237',bad);
	}
	catch (x)
	{ }
	return false;
}
</script>

<div class="Header3">
<asp:Literal runat="server" ID="X104214">104214</asp:Literal>
</div>

<b><asp:Literal runat="server" ID="X104215">104215</asp:Literal></b>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="X104216">104216</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblLine1">Current Addr Line 1</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104218">104218</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblLine2">Current Addr Line 2</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104220">104220</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblLine3">Current Addr Line 3</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104222">104222</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblLine4">Current Addr Line 4</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104224">104224</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblLine5">Current Addr Line 5</asp:Literal></td></tr>
</table>
<hr />
<b><asp:Literal runat="server" ID="X104226">104226</asp:Literal></b>
<br /><br />
<table>
<tr>
	<td><asp:Literal runat="server" ID="Y104216">104216</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtLine1" Width="400px" OnChange="JavaScript:Validate(1)"></asp:TextBox>&nbsp;<img id="img1" /></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104218">104218</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtLine2" Width="400px" OnChange="JavaScript:Validate(2)"></asp:TextBox>&nbsp;<img id="img2" /></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104220">104220</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtLine3" Width="400px" OnChange="JavaScript:Validate(3)"></asp:TextBox>&nbsp;<img id="img3" /></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104222">104222</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtLine4" Width="400px" OnChange="JavaScript:Validate(4)"></asp:TextBox>&nbsp;<img id="img4" /></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104224">104224</asp:Literal></td>
	<td><asp:TextBox runat="server" ID="txtLine5" Width="400px" OnChange="JavaScript:Validate(5)"></asp:TextBox>&nbsp;<img id="img5" /></td></tr>
</table>
<br />
<asp:Button runat="server" id="X104237" Text="104237" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104356" CssClass="Info">104356</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
Validate(1,1);
Validate(2,1);
Validate(3,1);
Validate(4,1);
Validate(5,1);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>