<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XHeader.ascx.cs" Inherits="PCIWebFinAid.XHeader" %>

<div class="Header1">
	<img src="Images/PlaNet.png" height="60" title="PlaNet Tech Limited" />
	<div style="float:right;margin-right:20px">
		<asp:Label runat="server" ID="lblUName" style="top:12px;position:relative"></asp:Label>
		&nbsp;&nbsp;&nbsp;&nbsp;
		<asp:HyperLink runat="server" ID="lnkMessages" ToolTip="You have unread message(s)" onclick="JavaScript:ShowMessages(1)" NavigateUrl="#" style="top:18px;position:relative">
			<img src="Images/Bell1.png" height="24" />
		</asp:HyperLink>
	</div>
</div>

<div id="pnlMessages" style="border:1px solid #000000;width:200px;float:right;visibility:hidden;display:none;padding:3px;margin-top:2px;background-color:yellow">
<div class="HelpHead">Notifications&nbsp;<img src="Images/Close1.png" style="float:right" onclick="JavaScript:ShowMessages(0)" title="Close" /></div>
<p>
You have to blah, blah, blah
</p><p>
Answer your phone!
</p><p>
That email from Francois needs attention ... NOW!
</p>
</div>

<script type="text/javascript">
function ShowMessages(show)
{
	ShowElt('pnlMessages',(show>0));
}
</script>
