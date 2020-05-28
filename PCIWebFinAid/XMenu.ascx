<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="XMenu.ascx.cs" Inherits="PCIWebFinAid.XMenu" %>

<style>
.VHead {
	font-weight: bold;
	background-color: orange;
	padding: 5px;
}
.VMenu {
	background-color: #C3C3C3;
	padding: 5px;
}
.VMenu a {
	color: black;
	text-decoration: none;
}
.VMenu:hover {
	background-color: red;
}
.VMenu a:hover {
	color: white;
	text-decoration: none;
}
</style>
<script type="text/javascript">
var mActive = '';

function XMenu(mID,mShow)
{
	ShowElt(mActive,false);
	ShowElt(mID,(mShow>0));
	mActive = mID;
}
</script>

<asp:Literal runat="server" ID="lblMenu"></asp:Literal>