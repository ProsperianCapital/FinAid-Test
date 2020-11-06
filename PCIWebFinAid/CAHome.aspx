<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="CAHome.aspx.cs" Inherits="PCIWebFinAid.CAHome" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="CAHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<title><asp:Literal runat="server" ID="X100003"></asp:Literal></title>
</head>
<body>
<form id="frmHome" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" Value="" ID="hdnProductCode" />
	<asp:HiddenField runat="server" Value="" ID="hdnLangCode" />
	<asp:HiddenField runat="server" Value="" ID="hdnLangDialectCode" />

	<img src="ImagesCA/CA-Assist1.jpg" style="width:100%" />
	<p style="text-align:center; color:#FFFFFF; font-family:Arial; font-size:35px; font-weight:600; line-height:1.5em; letter-spacing:0.8px; text-shadow:0px 0px 11px #000000; position:absolute; top:100px; left:0px; right:0px">
	<asp:Literal runat="server" ID="X100002"></asp:Literal>
	</p><p style="text-align:center; color:#54595F; font-family:Sans-serif; font-size:19px; font-weight:600; line-height:1.6em; letter-spacing:0.8px; text-shadow:0px 0px 51px #FFFFFF; position:absolute; top:210px; left:0px; right:0px">
	<asp:Literal runat="server" ID="X100004"></asp:Literal>
	</p>
	<div style="margin:0 auto;display:block;width:100%">
	<img src="ImagesCA/CA-Gold.png"   style="width:33%" />
	<img src="ImagesCA/CA-Silver.png" style="width:33%" />
	<img src="ImagesCA/CA-Bronze.png" style="width:33%" />
	</div>

	<div style="position:relative">
	<asp:Image runat="server" ID="P120001" style="width:100%" ImageUrl="ImagesCA/CA-Assist2.jpg" />
	<div style="color:#FFFFFF; font-family:Sans-serif; position:absolute; top:10px; left:5%; width:95%">
		<p style="font-size:35px; font-weight:400; line-height:1.4em; letter-spacing:0.8px">
		<asp:Literal runat="server" ID="X100045"></asp:Literal>
		</p><p style="font-size:19px;font-weight:300;line-height:1.6em">
		<asp:Literal runat="server" ID="X100046"></asp:Literal>
		</p>
		<div style="margin-right:120px;font-size:19px;font-weight:300;line-height:1.6em;float:right">
		Only 3 Minutes Easy<br />Online Application.<br />Select your option below.
		<p style="font-size:24px;font-weight:300;line-height:1.6em">
		GOLD<br />
		SILVER<br />
		BRONZE
		</p>
		<a href="#"><div class="TopButton TopButtonO" style="height:30px">REGISTER</div></a>
		</div>
		<p style="font-size:19px;font-weight:600;color:#54595F">
		<asp:Literal runat="server" ID="X105001"></asp:Literal>
		</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
		<asp:Literal runat="server" ID="X105002"></asp:Literal>
		</p><p style="font-size:19px;font-weight:600;color:#54595F">
		<asp:Literal runat="server" ID="X105003"></asp:Literal>
		</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
		<asp:Literal runat="server" ID="X105004"></asp:Literal>
		</p><p style="font-size:19px;font-weight:600;color:#54595F">
		<asp:Literal runat="server" ID="X105005"></asp:Literal>
		</p><p style="color: #54595F;font-family:Sans-serif;font-size: 15px;font-weight: 300;line-height: 1.8em">
		<asp:Literal runat="server" ID="X105006"></asp:Literal>
		</p>
	</div>
	</div>

	<div>
	<p style="color: #F88742;font-family:Sans-serif;font-size: 35px;font-weight: 400;line-height: 1.4em;letter-spacing: 0.8px;text-align:center">
	<asp:Literal runat="server" ID="X100051"></asp:Literal>
	</p>
	<div style="display:flex;margin:auto;width:100%">
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<img src="ImagesCA/isos.png" style="width:290px" />
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Mobile Response</figcaption>
	</figure> 
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<img src="ImagesCA/Legal.jpg" style="width:290px" />
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Legal Access</figcaption>
	</figure> 
	<figure style="box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px; transition: background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin: 15px 15px 15px 15px;padding: 60px 30px 60px 30px">
		<img src="ImagesCA/Loyalty.png" style="width:290px" />
		<figcaption style="font-size: 17px;letter-spacing: 0.5px;margin-top: 8px;text-align:center">Emergency Cash Reward</figcaption>
	</figure> 
	</div>
	</div>

	<div style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height:1.5;margin-left:50px;margin-right:50px">
	<asp:Literal runat="server" ID="XHIW"></asp:Literal>
	</div>

	<div>
	<img src="ImagesCA/CA-Assist3.jpg" style="width:99%" />
	</div>

	<p></p>
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<img src="Images/LogoENG.png" style="height:30px" />
		<p>
		<asp:Literal runat="server" ID="X100040"></asp:Literal>
		</p>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		<asp:Literal runat="server" ID="X100092"></asp:Literal>
		</p><p><b>
		<asp:Literal runat="server" ID="X100093"></asp:Literal>
		</b></p><p><b>
		<asp:Literal runat="server" ID="X100101"></asp:Literal>
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
