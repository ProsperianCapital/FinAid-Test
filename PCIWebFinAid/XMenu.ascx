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
.VText {
	font-size: 20px;
	font-weight: bold;
	text-decoration: none;
	color: white;
	display: flex; /* inline-block; */
	height: 75px;
	align-items: center;
	justify-content: space-between;
}
</style>
<script type="text/javascript">
var xActive    = null;
var xSubActive = null;

function XMenu(mID,mShow)
{
	if ( mShow == 0 && xSubActive != null )
		return;

	ShowElt(xActive,false);
	if ( mShow > 0 )
	{
		ShowElt(mID,true);
		ShowElt(xSubActive,false);
	}
	xActive = mID;
}

function XSubMenu(mID,parent)
{
	ShowElt(xSubActive,false);
	xSubActive = null;
	if ( mID  == null )
		return;

	var p = GetElt(mID);
	try
	{
		if ( parent == null )
			ShowElt(p,false);
		else
		{
			var rectBody   = document.body.getBoundingClientRect();
			var rectParent = parent.getBoundingClientRect();
			p.style.top    = ( 8+rectParent.top-rectBody.top).toString() + "px";
			p.style.left   = (12+rectParent.right-rectBody.left).toString() + "px";
			xSubActive     = p;
			ShowElt(p,true);
		}
	}
	catch (x)
	{
		alert(x.message);
	}	
}

function TestPos(obj)
{
	var h = GetElt('divTest');
	if ( h == null || obj == null )
		return;
	var bodyX = document.body.getBoundingClientRect();
	var eltX  = obj.getBoundingClientRect();
	var p = "[offset] Left="+obj.offsetLeft.toString()
	    + ", Top="+obj.offsetTop.toString()
	    + ", Width="+obj.offsetWidth.toString()
	    + ", Height="+obj.offsetHeight.toString();
	var q = "[body] Left="+bodyX.left.toString()
	    + ", Top="+bodyX.top.toString()
	    + ", Right="+bodyX.right.toString()
	    + ", Bottom="+bodyX.bottom.toString();
	var r = "[td] Left="+eltX.left.toString()
	    + ", Top="+eltX.top.toString()
	    + ", Right="+eltX.right.toString()
	    + ", Bottom="+eltX.bottom.toString();
	alert(p);
	alert(q);
	alert(r);
//	h.style.top  = obj.offsetTop.toString() + "px";
//	h.style.left = (obj.offsetLeft + obj.offsetWidth).toString() + "px";
	h.style.top  = (eltX.top-bodyX.top).toString() + "px";
	h.style.left = (3+eltX.right-bodyX.left).toString() + "px";
	ShowElt(h,true);
}
</script>

<asp:Literal runat="server" ID="lblMenu"></asp:Literal>