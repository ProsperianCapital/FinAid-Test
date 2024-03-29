﻿<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="pgRequestCashReward.aspx.cs" Inherits="PCIWebFinAid.pgRequestCashReward" %>
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
function Validate37(pic)
{
	try
	{
		var d = [null,null,null,null,null,null,null,null,null,null,null,null,null];
		d[ 1] = ( GetEltValueInt('txtAmount')         < 1 );
		d[ 2] = ( GetEltValue('txtBank').length       < 2 );
		d[ 3] = ( GetEltValue('txtAccName').length    < 2 );
		d[ 4] = ( GetEltValue('txtAccNumber').length  < 5 );
		d[ 5] = ( GetEltValue('txtBranchName').length < 2 );
		d[ 6] = ( GetEltValue('txtBranchCode').length < 4 );
		d[ 8] = ( GetEltValue('txtAddr1').length      < 3 );
		d[ 9] = ( GetEltValue('txtAddr2').length      < 3 );
		d[12] = ( GetEltValue('txtAddr5').length      < 4 );

		var disable = (d[1]||d[2]||d[3]||d[4]||d[5]||d[6]||d[8]||d[9]||d[12]);
		DisableElt('X104048',disable);

		if ( pic > 0 )
		{
			ShowElt('imgY'+pic.toString(),!d[pic]);
			ShowElt('imgN'+pic.toString(), d[pic]);
		}
		return !disable; 
	}
	catch (x)
	{ }
	return false;
}
</script>

<form id="frmMain" runat="server">
<ascx:XMenu runat="server" ID="ascxXMenu" />

<div class="Header3">
<asp:Literal runat="server" ID="X104033">104033</asp:Literal>
</div>

<div class="DataLabel" style="width:100%">
<asp:Literal runat="server" ID="X104034">104034</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAmount" CssClass="DataInput" onBlur="JavaScript:Validate37(1)"></asp:TextBox>&nbsp;
	<img id="imgY1" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN1" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div>

<p class="Note">
<asp:Literal runat="server" ID="X104056">104056</asp:Literal>
</p>

<div class="DataLabel" style="width:100%">
<div>
<asp:Literal runat="server" ID="X104061">104061</asp:Literal><br />
<asp:TextBox runat="server" ID="txtBank" CssClass="DataInput" onBlur="JavaScript:Validate37(2)"></asp:TextBox>&nbsp;
	<img id="imgY2" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN2" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div><div>
<asp:Literal runat="server" ID="X104063">104063</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAccName" CssClass="DataInput" onBlur="JavaScript:Validate37(3)"></asp:TextBox>&nbsp;
	<img id="imgY3" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN3" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div><div>
<asp:Literal runat="server" ID="X104067">104067</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAccNumber" CssClass="DataInput" onBlur="JavaScript:Validate37(4)"></asp:TextBox>&nbsp;
	<img id="imgY4" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN4" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div><div>
<asp:Literal runat="server" ID="X104069">104069</asp:Literal><br />
<asp:TextBox runat="server" ID="txtBranchName" CssClass="DataInput" onBlur="JavaScript:Validate37(5)"></asp:TextBox>&nbsp;
	<img id="imgY5" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN5" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div><div>
<asp:Literal runat="server" ID="X104377">104377</asp:Literal><br />
<asp:TextBox runat="server" ID="txtBranchCode" CssClass="DataInput" onBlur="JavaScript:Validate37(6)"></asp:TextBox>&nbsp;
	<img id="imgY6" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN6" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div><div>
<asp:Literal runat="server" ID="X104073">104073</asp:Literal><br />
<asp:TextBox runat="server" ID="txtSwift" CssClass="DataInput"></asp:TextBox>
</div>

<p class="Note">
<asp:Literal runat="server" ID="X104215">104215</asp:Literal>
</p>

<div>
<asp:Literal runat="server" ID="X104216">104216</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAddr1" CssClass="DataInput" Width="320px" MaxLength="150" onBlur="JavaScript:Validate37(8)"></asp:TextBox>&nbsp;
	<img id="imgY8" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN8" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div><div>
<asp:Literal runat="server" ID="X104218">104218</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAddr2" CssClass="DataInput" Width="320px" MaxLength="150" onBlur="JavaScript:Validate37(9)"></asp:TextBox>&nbsp;
	<img id="imgY9" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN9" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div><div>
<asp:Literal runat="server" ID="X104220">104220</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAddr3" CssClass="DataInput" Width="320px" MaxLength="150"></asp:TextBox>
</div><div>
<asp:Literal runat="server" ID="X104222">104222</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAddr4" CssClass="DataInput" Width="320px" MaxLength="150"></asp:TextBox>
</div><div>
<asp:Literal runat="server" ID="X104224">104224</asp:Literal><br />
<asp:TextBox runat="server" ID="txtAddr5" CssClass="DataInput" Width="320px" MaxLength="150" onBlur="JavaScript:Validate37(12)"></asp:TextBox>&nbsp;
	<img id="imgY12" src="<%=PCIBusiness.Tools.ImageFolder() %>Tick.png"  style="visibility:hidden;display:none" />
	<img id="imgN12" src="<%=PCIBusiness.Tools.ImageFolder() %>Cross.png" style="visibility:hidden;display:none" />
</div>
</div>
<br />
<asp:Button runat="server" id="X104048" Text="104048" OnClick="btnOK_Click" />
<br /><br />
<asp:Label runat="server" ID="X104050" CssClass="Info">104050</asp:Label>
<asp:Label runat="server" ID="lblError" CssClass="Error"></asp:Label>

<script type="text/javascript">
Validate37(0);
</script>

<!--#include file="IncludeErrorDtl.htm" -->
<ascx:XFooter runat="server" ID="ascxXFooter" />
</form>
</body>
</html>