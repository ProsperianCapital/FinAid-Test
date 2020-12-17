<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="CAHome.aspx.cs" Inherits="PCIWebFinAid.CAHome" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="CAHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<title><asp:Literal runat="server" ID="X100003">Id 100003</asp:Literal></title>
</head>
<body>
<form id="frmHome" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" Value="" ID="hdnProductCode" />
	<asp:HiddenField runat="server" Value="" ID="hdnLangCode" />
	<asp:HiddenField runat="server" Value="" ID="hdnLangDialectCode" />

	<!-- TEST
	<a href="JavaScript:alert('Width='+window.innerWidth.toString()+' pixels')">WIDTH</a>
	-->

	<div style="margin:0 auto;display:block;width:100%">
	<asp:Image runat="server" ID="P12002" style="max-width:100%;max-height:346px;float:left" />
	<p style="color:#242424; font-family:Arial; font-size:35px; font-weight:600; line-height:1.5em; letter-spacing:0.8px; text-shadow:0px 0px 0px #000000; top:100px; left:0px; right:0px; word-break:break-word; word-wrap:break-word">
	<asp:Literal runat="server" ID="X100002">Id 100002</asp:Literal>
	</p><p style="color:#54595F; font-family:Sans-serif; font-size:19px; font-weight:500; line-height:1.6em; letter-spacing:0.8px; text-shadow:0px 0px 51px #FFFFFF; top:210px; left:0px; right:0px">
	<asp:Literal runat="server" ID="X100004">Id 100004</asp:Literal>
	</p>
	</div>

	<div style="margin:0 auto;width:100%;display:inline-block">

	<div style="width:30%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:25% !important;margin:2px;width:350px;height:210px">
		<div style="background-color:#FFCC00;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			&nbsp;&nbsp;GOLD
			</div>
			<div style="font-size:14px">
			$ 39.95/m
			</div>
		</div>
		<div style="float:right;display:inline-block">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;iSOS Mobile Response<br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;LifeGuru Online Coach<br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;Subscription Rewards<br />&nbsp;
		</div>
		<div style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top">
			UP TO
			<span style="font-size:40px">
			&nbsp;$400.00
			</span>
			</div>
			<div style="font-size:20px">
			Loyalty CASH Reward
			</div>
		</div>
	</div>

	<div style="width:30%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:25% !important;margin:2px;width:350px;height:210px">
		<div style="background-color:#A6A6A6;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			SILVER
			</div>
			<div style="font-size:14px">
			$ 29.95/m
			</div>
		</div>
		<div style="float:right;display:inline-block">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;iSOS Mobile Response<br />
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;LifeGuru Online Coach<br />
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;Subscription Rewards<br />&nbsp;
		</div>
		<div style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top">
			UP TO
			<span style="font-size:40px">
			&nbsp;$300.00
			</span>
			</div>
			<div style="font-size:20px">
			Loyalty CASH Reward
			</div>
		</div>
	</div>

	<div style="width:30%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:25% !important;margin:2px;width:350px;height:210px">
		<div style="background-color:#B18135;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			BRONZE
			</div>
			<div style="font-size:14px">
			$ 19.95/m
			</div>
		</div>
		<div style="float:right;display:inline-block">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;iSOS Mobile Response<br />
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;LifeGuru Online Coach<br />
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;Subscription Rewards<br />&nbsp;
		</div>
		<div style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top">
			UP TO
			<span style="font-size:40px">
			&nbsp;$200.00
			</span>
			</div>
			<div style="font-size:20px">
			Loyalty CASH Reward
			</div>
		</div>
	</div>

	</div> 
<!-- #F9CF0E

	<div style="position:relative;display:inline-block;width:100%;background-color:#F9CF0E">
-->
		<div style="color:#FFFFFF;background-color:#F9CF0E;font-family:Sans-serif;width:100%;position:relative;padding:25px">
			<p style="font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;color:#333232">
			<asp:Literal runat="server" ID="X100045">Id 100045</asp:Literal>
			</p><p style="font-size:19px;font-weight:300;line-height:1.6em;color:#272626">
			<asp:Literal runat="server" ID="X100046">Id 100046</asp:Literal>
			</p>

			<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="ImagesCA/X1.png" style="float:left" />
			Register
			<p style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			Online 24/7 via this website (Please have your ID Number & Debit/Credit Card details handy).
			</p>
			</div>
			<br />
			<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="ImagesCA/X2.png" style="float:left" />
			Pay
			<p style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			Your Annual Registration & Monthly Subscription Fees (Via Direct Debit from your Master or VISA card).
			</p>
			</div>
			<br />
			<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="ImagesCA/X3.png" style="float:left" />
			Enjoy
			<p style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			All your Subscription Benefits (Accessible Online 24/7).
			</p>
			</div>
			<br />

			<div style="margin-right:100px;font-size:19px;font-weight:300;line-height:1.6em;float:right">
			Only 3 Minutes Easy<br />Online Application.<br />Select your option below.
			<p style="font-size:24px;font-weight:300;line-height:1.6em">
			GOLD<br />
			SILVER<br />
			BRONZE
			</p>
			<a href="#"><div class="TopButton TopButtonO" style="height:30px">REGISTER</div></a>
			</div>
			<p style="font-size:19px;font-weight:600;color:#54595F">
			<asp:Literal runat="server" ID="X105001">Id 105001</asp:Literal>
			</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
			<asp:Literal runat="server" ID="X105002">Id 105002</asp:Literal>
			</p><p style="font-size:19px;font-weight:600;color:#54595F">
			<asp:Literal runat="server" ID="X105003">Id 105003</asp:Literal>
			</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
			<asp:Literal runat="server" ID="X105004">Id 105004</asp:Literal>
			</p><p style="font-size:19px;font-weight:600;color:#54595F">
			<asp:Literal runat="server" ID="X105005">Id 105005</asp:Literal>
			</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
			<asp:Literal runat="server" ID="X105006">Id 105006</asp:Literal>
			</p>
		</div>
<!--
	</div> -->

	<div>
	<p style="color: #F88742;font-family:Sans-serif;font-size: 35px;font-weight: 400;line-height: 1.4em;letter-spacing: 0.8px;text-align:center">
	<asp:Literal runat="server" ID="X100051">YOUR SUBSCRIPTION BENEFITS (Id 100051)</asp:Literal>
	</p>
	<div style="display:flex;margin:auto;width:100%">
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<asp:Image runat="server" ID="P12020" style="width:290px" title="Image 12020" /> <!-- isos -->
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Mobile Response</figcaption>
	</figure> 
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<asp:Image runat="server" ID="P12021" style="width:290px" title="Image 12021" /> <!-- legal -->
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Legal Access</figcaption>
	</figure> 
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<asp:Image runat="server" ID="P12022" style="width:290px" title="Image 12022" /> <!-- loyalty -->
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Cash Reward</figcaption>
	</figure> 
	</div>
	</div>

	<div style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height:1.5;margin-left:50px;margin-right:50px">
	<asp:Literal runat="server" ID="XHIW"></asp:Literal>
	</div>

	<p style="color:#F9CF0E;font-family:Sans-serif;font-size:40px;font-weight:600;line-height:1.4em;letter-spacing:0.8px">
	<asp:Literal runat="server" ID="X999999">For more, visit...</asp:Literal>
	</p>
	<img src="<%=PCIBusiness.Tools.ImageFolder() %>ServiceLinkiSOS.png" />
	<img src="<%=PCIBusiness.Tools.ImageFolder() %>ServiceLinkLifeGuru.png" />
	<br />
	<hr />

	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<img src="ImagesCA/LogoENG.png" style="height:30px" />
		<p>
		<asp:Literal runat="server" ID="X100040">Id 100040</asp:Literal>
		</p>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		<asp:Literal runat="server" ID="X100092">Id 100092</asp:Literal>
		</p><p><b>
		<asp:Literal runat="server" ID="X100093">Id 100093</asp:Literal>
		</b></p><p><b>
		<asp:Literal runat="server" ID="X100101">Id 100101</asp:Literal>
		</b></p>
	</div>

	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		SITE MAP
		</p>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>

	<!--#include file="IncludeErrorDtl.htm" -->

	<asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false" Enabled="false" ViewStateMode="Disabled"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
</form>
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
