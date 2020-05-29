<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ShowBusy.ascx.cs" Inherits="PCIWebFinAid.ShowBusy" %>

<script type="text/javascript">
function ShowBusy(msg,ctlID,backGround)
{
	var p = GetElt('ctlBusy');
	if ( backGround == null )
		backGround = 1;
	ShowElt(p,true,backGround);
	if ( msg != null )
	{
		SetEltValue('<%=MessageCtlName%>',msg);
//		var w = (60 + ( msg.length * 10 )).toString() + "px";
//		p.style.width = w;
	}
	if ( ctlID != null )
		ShowElt(ctlID,false);
}
function HideBusy()
{
	ShowElt("ctlBusy",false);
}
</script>

<div id="ctlBusy" style="display:none;visibility:hidden;width:300px" class="Popup2">
	<div class="PopupHead">Please wait ...</div>
	&nbsp;
	<asp:Table runat="server">
		<asp:TableRow>
			<asp:TableCell Width="1%" style="padding-right:10px"><img src="Images/Busy.gif" title="Busy..." /></asp:TableCell>
			<asp:TableCell ID="ctlBusyMsg" style="font-style:italic">Processing ... please be patient</asp:TableCell>
		</asp:TableRow>
	</asp:Table>
</div>