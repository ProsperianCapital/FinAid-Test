<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="ISCRM.aspx.cs" Inherits="PCIWebFinAid.ISCRM" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="ISHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<asp:Literal runat="server" ID="lblGoogleUA"></asp:Literal>
	<title><asp:Literal runat="server" ID="X105040">105040</asp:Literal></title>
	<style>
	.InputRow
	{
		display: flex;
		flex-wrap: wrap;
		margin: 0 0 10px 0;
	}
	.InputCol33
	{
		flex: 33%;
	}
	.InputCol66
	{
		flex: 66%;
	}
	@media screen and (max-width: 800px)
	{
		.InputCol33
		{
			flex: 100%;
		}
		.InputCol66
		{
			flex: 100%;
		}
	}
	.InputLabel
	{
		color: #EAA62E;
		cursor: pointer;
		font-weight: bold;
	}
	.InputBox
	{
		-webkit-box-flex: 1;
		-moz-appearance: none;
		background-color: #ECECEC;
		border-color: #D8CDCD;
		border-width: 0px 0px 0px 0px;
		border-radius: 20px 20px 20px 20px;
		border: 1px solid #818a91;
		box-shadow: none;
		box-sizing: border-box;
		color: #54595F;
		display: block;
		flex-basis: 100%;
		flex-grow: 1;
		font-family: var( --e-global-typography-text-font-family ), Sans-serif;
		font-size: 13px;
		line-height: 0.1em;
		letter-spacing: 1px;
		margin: 0;
		max-width: 100%;
		min-height: 33px;
		text-align: inherit;
		padding: 4px 12px;
		width: 90%;
	}
	.InputBox:focus
	{
		outline: none;
		box-shadow: 0px 0px 4px blue;
	}
	</style>
</head>
<body>
<form id="frmHome" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" ID="hdnProductCode" />
	<asp:HiddenField runat="server" ID="hdnLangCode" />
	<asp:HiddenField runat="server" ID="hdnLangDialectCode" />

	<asp:Button runat="server" ID="btnWidth" Text="Width" OnClientClick="JavaScript:alert('Width = '+window.innerWidth.toString()+' pixels');return false" />

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lblName" for="txtName" class="InputLabel">Name</label>
			<asp:TextBox runat="server" ID="txtName" CssClass="InputBox" PlaceHolder="Your full name"></asp:TextBox>
		</div>
		<div class="InputCol33">
			<label id="lblNumber" for="txtNumber" class="InputLabel">Number</label>
			<asp:TextBox runat="server" ID="txtNumber" CssClass="InputBox" PlaceHolder="Your phone number"></asp:TextBox>
		</div>
		<div class="InputCol33">
			<label id="lblEMail" for="txtEMail" class="InputLabel">Email</label>
			<asp:TextBox runat="server" ID="txtEMail" CssClass="InputBox" PlaceHolder="Your email address"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl1n" for="txt1n" class="InputLabel">1. Medical Emergency</label>
			<asp:TextBox runat="server" ID="txt1n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl1d" for="txt1d" class="InputLabel">Message 1</label>
			<asp:TextBox runat="server" ID="txt1d" CssClass="InputBox" PlaceHolder="MEDICAL EMERGENCY: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl2n" for="txt2n" class="InputLabel">2. Roadside Incident</label>
			<asp:TextBox runat="server" ID="txt2n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl2d" for="txt2d" class="InputLabel">Message 2</label>
			<asp:TextBox runat="server" ID="txt2d" CssClass="InputBox" PlaceHolder="ROADSIDE INCIDENT: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl3n" for="txt3n" class="InputLabel">3. Burglary/Intrusion</label>
			<asp:TextBox runat="server" ID="txt3n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl3d" for="txt3d" class="InputLabel">Message 3</label>
			<asp:TextBox runat="server" ID="txt3d" CssClass="InputBox" PlaceHolder="BURGLARY/INTRUSION: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl4n" for="txt4n" class="InputLabel">4. Fire</label>
			<asp:TextBox runat="server" ID="txt4n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl4d" for="txt4d" class="InputLabel">Message 4</label>
			<asp:TextBox runat="server" ID="txt4d" CssClass="InputBox" PlaceHolder="FIRE: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl5n" for="txt5n" class="InputLabel">5. Hijack</label>
			<asp:TextBox runat="server" ID="txt5n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl5d" for="txt5d" class="InputLabel">Message 5</label>
			<asp:TextBox runat="server" ID="txt5d" CssClass="InputBox" PlaceHolder="HIJACK: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl6n" for="txt6n" class="InputLabel">6. Wildlife Attack</label>
			<asp:TextBox runat="server" ID="txt6n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl6d" for="txt6d" class="InputLabel">Message 6</label>
			<asp:TextBox runat="server" ID="txt6d" CssClass="InputBox" PlaceHolder="WILDLIFE ATTACK: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl7n" for="txt7n" class="InputLabel">7. Kidnap</label>
			<asp:TextBox runat="server" ID="txt7n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl7d" for="txt7d" class="InputLabel">Message 7</label>
			<asp:TextBox runat="server" ID="txt7d" CssClass="InputBox" PlaceHolder="KIDNAPPING: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl8n" for="txt8n" class="InputLabel">8. Fetch Me</label>
			<asp:TextBox runat="server" ID="txt8n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl8d" for="txt8d" class="InputLabel">Message 8</label>
			<asp:TextBox runat="server" ID="txt8d" CssClass="InputBox" PlaceHolder="PICKUP: Please assist user at the listed location"></asp:TextBox>
		</div>
	</div>

	<div class="InputRow">
		<div class="InputCol33">
			<label id="lbl9n" for="txt9n" class="InputLabel">9. General/Other</label>
			<asp:TextBox runat="server" ID="txt9n" CssClass="InputBox" PlaceHolder="Number you want to contact"></asp:TextBox>
		</div>
		<div class="InputCol66">
			<label id="lbl9d" for="txt9d" class="InputLabel">Message 9</label>
			<asp:TextBox runat="server" ID="txt9d" CssClass="InputBox" PlaceHolder="Type your message"></asp:TextBox>
		</div>
	</div>

	<asp:HyperLink runat="server" ID="xSend" CssClass="TopButton TopButtonO">Save</asp:HyperLink>

	<div>
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		<asp:Literal runat="server" ID="X100092"></asp:Literal>
		</p>

		<asp:PlaceHolder runat="server" ID="pnlContact01">
		<p><b>
		<asp:Literal runat="server" ID="X100093"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact02">
		<p>
		<asp:Literal runat="server" ID="X104402"></asp:Literal>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact03">
		<p><b>
		<asp:Literal runat="server" ID="X100095"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact04">
		<p style="display:flex">
		<asp:Image runat="server" ID="P12031" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X100096"></asp:Label>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact05">
		<p><b>
		<asp:Literal runat="server" ID="X100101"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact06">
		<p style="display:flex">
		<asp:Image runat="server" ID="P12032" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X104404" style="padding-top:4px"></asp:Label>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact07">
		<p style="display:flex">
		<asp:Image runat="server" ID="P12033" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X100102" style="padding-top:4px"></asp:Label>
		</p>
		</asp:PlaceHolder>

		<asp:PlaceHolder runat="server" ID="pnlContact08">
		<p><b>
		<asp:Literal runat="server" ID="X104418"></asp:Literal>
		</b></p>
		</asp:PlaceHolder>
		
		<asp:PlaceHolder runat="server" ID="pnlContact09">
		<div style="display:flex">
		<div style="vertical-align:top">
		<asp:Image runat="server" ID="P12034" style="object-fit:contain" />
		</div>
		<asp:Label runat="server" ID="X100105"></asp:Label>
		</div>
		</asp:PlaceHolder>
	</div>

	<!--
	<asp Image run@t="server" ID="P12001" style="height:30px" />
	-->
	<p style="line-height:1.5;margin: 0 0 1em 0;padding-top:10px;font-size:11px">
	<asp:Literal runat="server" ID="X100040">100040</asp:Literal>
	</p>

	<asp:HyperLink runat="server" ID="X100041" ForeColor="Orange" NavigateUrl="JavaScript:Legal(1)">100041</asp:HyperLink> |
	<asp:HyperLink runat="server" ID="X100042" ForeColor="Orange" NavigateUrl="JavaScript:Legal(3)">100042</asp:HyperLink> |
	<asp:HyperLink runat="server" ID="X100043" ForeColor="Orange" NavigateUrl="JavaScript:Legal(5)">100043</asp:HyperLink> |
	<asp:HyperLink runat="server" ID="X100044" ForeColor="Orange" NavigateUrl="JavaScript:Legal(4)">100044</asp:HyperLink>

	<div id="LV001" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV001',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH001">Header 001</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD001">Detail 001</asp:Literal></p>
	</div>

	<div id="LV003" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV003',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH003">Header 003</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD003">Detail 003</asp:Literal></p>
	</div>

	<div id="LV005" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV005',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH005">Header 005</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD005">Detail 005</asp:Literal></p>
	</div>

	<div id="LV004" style="color:#FFFFFF;font-family:Sans-serif;background-color:#F9CF0E;width:100%;padding:2px 0px 5px 0px;margin:10px 0px 0px 0px;visibility:hidden;display:none">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Close1.png" onclick="JavaScript:ShowElt('LV004',false)" title="Close" style="float:right;padding:4px" />
		<p class='FAQQuestion'><asp:Literal runat="server" ID="LH004">Header 004</asp:Literal></p>
		<p class='FAQAnswer'><asp:Literal runat="server" ID="LD004">Detail 004</asp:Literal></p>
	</div>

	<!--#include file="IncludeErrorDtl.htm" -->

	<asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false" Enabled="false" ViewStateMode="Disabled"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
	<asp:Literal runat="server" id="lblChat"></asp:Literal>
</form>
<br />
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
