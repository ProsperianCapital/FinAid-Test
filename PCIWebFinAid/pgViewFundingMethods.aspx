<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgViewFundingMethods.aspx.cs" Inherits="PCIWebFinAid.pgViewFundingMethods" %>
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

<script type="text/javascript">
function Validate(eltID,len,eltType)
{
	try
	{
		var img = GetElt('img'+eltID);
		var val = GetEltValue('txt'+eltID);
		var dir = '<%=PCIBusiness.Tools.ImageFolder() %>';
		var ok  = true;
		if ( val.length < len )
			ok = false;
		else if ( eltType == 1 ) // Card number
			ok = ValidCardNumber(val);
		else if ( eltType == 2 ) // CVV
			ok = ValidPIN(val,len);
		if (ok)
			img.src = dir + 'Tick.png';
		else
		{
			img.src = dir + 'Cross.png';
			DisableElt('X104279',true);
		}
		return ok;
	}
	catch (x)
	{ }
	return false;
}
</script>

<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104268">104268</asp:Literal>
</div>

<table>
<tr>
	<td><asp:Literal runat="server" ID="X104269">104269</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblName">Card holder name</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104273">104273</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblNumber">Card number</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104275">104275</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblDate">MM / YYYY</asp:Literal></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="X104277">104277</asp:Literal></td>
	<td class="DataStatic"><asp:Literal runat="server" ID="lblCVV">CVV</asp:Literal></td></tr>
<tr>
	<td colspan="2"><hr /></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104269">104269</asp:Literal></td>
	<td class="DataStatic">
		<asp:TextBox runat="server" Width="400px" OnKeyUp="JavaScript:Validate('Name',3,0)" ID="txtName"></asp:TextBox>&nbsp;
		<img id="imgName" /></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104273">104273</asp:Literal></td>
	<td class="DataStatic">
		<asp:TextBox runat="server" Width="200px" OnKeyUp="JavaScript:Validate('Number',14,1)" ID="txtNumber" MaxLength="20"></asp:TextBox>&nbsp;
		<img id="imgNumber" /></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104275">104275</asp:Literal></td>
	<td class="DataStatic">
		<asp:DropDownList runat="server" ID="lstMM" Width="60px"></asp:DropDownList>
		<asp:DropDownList runat="server" ID="lstYY" Width="60px"></asp:DropDownList></td></tr>
<tr>
	<td><asp:Literal runat="server" ID="Y104277">104277</asp:Literal></td>
	<td class="DataStatic">
		<asp:TextBox runat="server" Width="40px" OnKeyUp="JavaScript:Validate('CVV',3,2)" ID="txtCVV" MaxLength="4"></asp:TextBox>&nbsp;
		<img id="imgCVV" /></td></tr>
</table>

<br />
<asp:Button runat="server" id="X104279" Text="104279" OnClick="btnOK_Click" />&nbsp;
<asp:Label runat="server" ID="X104281"  CssClass="Header7">104281</asp:Label>
<br /><br />
<asp:Label runat="server" ID="X104358"  CssClass="Info">104358</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
if ( Validate('Name',3,0) && Validate('Number',14,1) && Validate('CVV',3,2) )
	DisableElt('X104279',false);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>