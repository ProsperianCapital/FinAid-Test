<%@ Page Language="C#" AutoEventWireup="false" CodeBehind="CAHome.aspx.cs" Inherits="PCIWebFinAid.CAHome" %>
<%@ Register TagPrefix="ascx" TagName="Header" Src="CAHeader.ascx" %>
<%@ Register TagPrefix="ascx" TagName="Footer" Src="CAFooter.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<!--#include file="IncludeMainCA.htm" -->
	<title><asp:Literal runat="server" ID="X100003">100003</asp:Literal></title>
</head>
<body>
<script type="text/javascript">
function TickOver(img,mode)
{
	img.src = '<%=PCIBusiness.Tools.ImageFolder() %>' + ( mode == 1 ? 'TickOrange' : 'TickWhite' ) + '.png';
}
</script>
<form id="frmHome" runat="server">
	<ascx:Header runat="server" ID="ascxHeader" />

	<asp:HiddenField runat="server" Value="" ID="hdnProductCode" />
	<asp:HiddenField runat="server" Value="" ID="hdnLangCode" />
	<asp:HiddenField runat="server" Value="" ID="hdnLangDialectCode" />

	<asp:Button runat="server" ID="btnWidth" Text="Width" OnClientClick="JavaScript:alert('Width = '+window.innerWidth.toString()+' pixels');return false" />

	<div style="margin:0 auto;display:block;width:100%">
	<asp:Image runat="server" ID="P12002" style="max-width:100%;max-height:346px;float:left" />
	<p style="color:#242424;font-family:Arial;font-size:35px;font-weight:600;line-height:1.5em; letter-spacing:0.8px;text-shadow:0px 0px 0px #000000;top:100px;left:0px;right:0px;word-break:break-word;word-wrap:break-word;text-align:center">
	<asp:Literal runat="server" ID="X100002">100002</asp:Literal>
	</p><p style="color:#54595F;font-family:Sans-serif; font-size:19px; font-weight:500; line-height:1.6em; letter-spacing:0.8px; text-shadow:0px 0px 51px #FFFFFF; top:210px; left:0px; right:0px">
	<asp:Literal runat="server" ID="X100004">100004</asp:Literal>
	</p>
	</div>

	<div style="margin:0 auto;width:100%;display:inline-block">

	<div style="width:25%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:20% !important;margin:2px;width:346px;height:210px">
		<div style="background-color:#FFCC00;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			<asp:Literal runat="server" ID="X105012">105012</asp:Literal>
			</div>
			<div style="font-size:14px">
			<asp:Literal runat="server" ID="X105013">105013</asp:Literal>
			</div>
		</div>
		<div style="float:right;display:inline-block">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105017">105017</asp:Literal><br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105018">105018</asp:Literal><br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105019">105019</asp:Literal><br />&nbsp;
		</div>
		<div style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top;text-align:center">
			<asp:Literal runat="server" ID="X105014">105014</asp:Literal>
			<span style="font-size:40px">
			&nbsp;<asp:Literal runat="server" ID="X105015">105015</asp:Literal>
			</span>
			</div>
			<div style="font-size:20px;text-align:center">
			<asp:Literal runat="server" ID="X105016">105016</asp:Literal>
			</div>
		</div>
	</div>

	<div style="width:25%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:20% !important;margin:2px;width:346px;height:210px">
		<div style="background-color:#A6A6A6;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			<asp:Literal runat="server" ID="X105020">105020</asp:Literal>
			</div>
			<div style="font-size:14px">
			<asp:Literal runat="server" ID="X105021">105021</asp:Literal>
			</div>
		</div>
		<div style="float:right;display:inline-block">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105025">105025</asp:Literal><br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105026">105026</asp:Literal><br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105027">105027</asp:Literal><br />&nbsp;
		</div>
		<div style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top;text-align:center">
			<asp:Literal runat="server" ID="X105022">105022</asp:Literal>
			<span style="font-size:40px">
			&nbsp;<asp:Literal runat="server" ID="X105023">105023</asp:Literal>
			</span>
			</div>
			<div style="font-size:20px;text-align:center">
			<asp:Literal runat="server" ID="X105024">105024</asp:Literal>
			</div>
		</div>
	</div>

	<div style="width:25%;border:1px solid #000000;display:inline-block;padding:10px;float:left;min-width:20% !important;margin:2px;width:346px;height:210px">
		<div style="background-color:#B18135;color:#FDFEF2;border-radius:50%;height:75px;width:75px;padding:15px;float:left">
			<div style="font-size:10px">&nbsp;</div>
			<div style="font-size:20px">
			<asp:Literal runat="server" ID="X105028">105028</asp:Literal>
			</div>
			<div style="font-size:14px">
			<asp:Literal runat="server" ID="X105029">105029</asp:Literal>
			</div>
		</div>
		<div style="float:right;display:inline-block">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105033">105033</asp:Literal><br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105034">105034</asp:Literal><br />
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>ProductUSPTick.png" />&nbsp;<asp:Literal runat="server" ID="X105035">105035</asp:Literal><br />&nbsp;
		</div>
		<div style="background-color:#FF6E01;color:#FEFFFE;border-radius:10px 10px 10px 10px;border:2px solid #EF9714;padding:10px;float:left;width:90%;margin-left:5px;margin-right:5px">
			<div style="font-size:10px;vertical-align:top;text-align:center">
			<asp:Literal runat="server" ID="X105030">105030</asp:Literal>
			<span style="font-size:40px">
			&nbsp;<asp:Literal runat="server" ID="X105031">105031</asp:Literal>
			</span>
			</div>
			<div style="font-size:20px;text-align:center">
			<asp:Literal runat="server" ID="X105032">105032</asp:Literal>
			</div>
		</div>
	</div>
	</div> 

	<div style="color:#FFFFFF;background-color:#F9CF0E;font-family:Sans-serif;width:99%;min-width:346px;padding:0px;margin:0px">
		<div style="padding:10px;font-family:'Open Sans Hebrew',Sans-serif;font-size:20px;line-height:1.5em;letter-spacing:1.3px;margin:0px">
			<div style="margin-left:10px">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100287">100287</asp:Literal>
			</div>
			<br />
			<div style="margin-left:10px">		
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100288">100288</asp:Literal>
			</div>
			<br />
			<div style="margin-left:10px">		
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>TickOrange.png" onmouseover="JavaScript:TickOver(this,2)" onmouseout="JavaScript:TickOver(this,1)" style="vertical-align:middle" />
			<asp:Literal runat="server" ID="X100289">100289</asp:Literal>
			</div>
			<br />
			<div>			
			<a href="#"><div class="TopButton" style="height:32px;width:120px;color:#FFFFFF;background-color:#54595F" onmouseover="JavaScript:this.style.backgroundColor='#FF7400'" onmouseout="JavaScript:this.style.backgroundColor='#54595F'">REGISTER</div></a>
			</div>
		</div>

		<p style="font-size:35px;font-weight:400;line-height:1.4em;letter-spacing:0.8px;color:#333232;text-align:center">
		<asp:Literal runat="server" ID="X100045">100045</asp:Literal>
		</p><p style="font-size:19px;font-weight:300;line-height:1.6em;color:#272626">
		<asp:Literal runat="server" ID="X100046">100046</asp:Literal>
		</p>

		<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>Register.png" style="float:left" />
			<br /><asp:Label runat="server" ID="X105001">105001</asp:Label>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Label runat="server" ID="X105002">105002</asp:Label>
			</div>
		</div>

		<br />
		<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>Pay.png" style="float:left" />
			<br /><asp:Literal runat="server" ID="X105003">105003</asp:Literal>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Literal runat="server" ID="X105004">105004</asp:Literal>
			</div>
		</div>

		<br />
		<div style="color:#54595F;font-size:19px;font-weight:600">
			<img src="<%=PCIBusiness.Tools.ImageFolder() %>Enjoy.png" style="float:left" />
			<br /><asp:Literal runat="server" ID="X105005">105005</asp:Literal>
			<div style="color:#54595F;font-size:15px;font-weight:300;line-height:1.8em">
			<asp:Literal runat="server" ID="X105006">105006</asp:Literal>
			</div>
		</div>
		<br />
	</div>

	<div>
		<p style="color: #F88742;font-family:Sans-serif;font-size: 35px;font-weight: 400;line-height: 1.4em;letter-spacing: 0.8px;text-align:center">
		<asp:Literal runat="server" ID="X100051">100051</asp:Literal>
		</p>
		<!-- <div style="display:flex;margin:auto;width:100%" -->
		<div style="margin:0 auto;width:100%"> <!-- ";display:inline-block" -->
		<figure style="display:inline-block;box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:15px 15px 15px 15px;padding:60px 30px 60px 30px">
			<asp:Image runat="server" ID="P12010" style="width:290px" title="Image 12010" />
			<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center">Emergency Mobile Response</figcaption>
		</figure> 
		<figure style="display:inline-block;box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:15px 15px 15px 15px;padding:60px 30px 60px 30px">
			<asp:Image runat="server" ID="P12011" style="width:290px" title="Image 12011" />
			<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center">Emergency Legal Access</figcaption>
		</figure> 
		<figure style="display:inline-block;box-shadow: 0px 0px 50px 0px rgba(15,15,43,0.58);width:290px;border-radius:15px;transition:background 0.3s, border 0.3s, border-radius 0.3s, box-shadow 0.3s;margin:15px 15px 15px 15px;padding:60px 30px 60px 30px">
			<asp:Image runat="server" ID="P12012" style="width:290px" title="Image 12012" />
			<figcaption style="font-size:17px;letter-spacing:0.5px;margin-top:8px;text-align:center">Emergency Cash Reward</figcaption>
		</figure> 
		</div>
	</div>

	<div style="font-size:10.5pt;font-family:Helvetica,sans-serif;line-height:1.5;margin-left:10px;margin-right:10px">
	<asp:Literal runat="server" ID="XHIW"></asp:Literal>
	</div>

	<div style="color:#F9CF0E;font-family:Sans-serif;font-size:40px;font-weight:600;line-height:1.4em;letter-spacing:0.8px;margin-left:10px">
	<asp:Literal runat="server" ID="X105009">105009</asp:Literal>
	</div>
	<div style="margin:0px;margin-left:10px">
	<asp:Image runat="server" ID="P12013" title="Img 12013" />
	<asp:Image runat="server" ID="P12025" title="Img 12025" />
	</div>
	<div style="margin:0px;margin-left:10px">
	<asp:Image runat="server" ID="P12026" title="Img 12026" />
	<asp:Image runat="server" ID="P12027" title="Img 12027" />
	</div>
	<hr />

	<div>
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		<asp:Literal runat="server" ID="X100092">100092</asp:Literal>
		</p><p><b>
		<asp:Literal runat="server" ID="X100093">100093</asp:Literal>
		</b></p><p>
		<asp:Literal runat="server" ID="X104402">104402</asp:Literal>
		</p><p><b>
		<asp:Literal runat="server" ID="X100095">100095</asp:Literal>
		</b></p><p style="display:flex">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Envelope.png" style="object-fit:contain" />&nbsp;
		<asp:Label runat="server" ID="X100096" style="vertical-align:top">100096</asp:Label>
		</p><p><b>
		<asp:Literal runat="server" ID="X100101">100101</asp:Literal>
		</b></p><p style="display:flex">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Telephone.png" style="object-fit:contain" />
		<asp:Label runat="server" ID="X104404" style="vertical-align:top">104404</asp:Label>
		</p><p style="display:flex">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Clock.png" style="object-fit:contain" />
		<asp:Label runat="server" ID="X100102" style="vertical-align:top">100102</asp:Label>
		</p><p><b>
		<asp:Literal runat="server" ID="X104418">104418</asp:Literal>
		</b></p><p style="display:flex">
		<img src="<%=PCIBusiness.Tools.ImageFolder() %>Pin.png" style="object-fit:contain" />
		<asp:Label runat="server" ID="X100105" style="vertical-align:top">(100105) Please address any written correspondence to P O Box 1134234234, Westville, 8787. Thank you, have a nice day and don't call us, we'll call you</asp:Label>
		</p>
	</div>

	<div>
		<asp:Image runat="server" ID="P12001" style="height:30px" />
		<p style="line-height:1.5;margin: 0 0 1em 0;padding-top:10px;font-size:11px">
		<asp:Literal runat="server" ID="X100040">100040</asp:Literal>
		</p>
		<asp:Image runat="server" ID="P12015" />
		<asp:Image runat="server" ID="P12016" />
		<asp:Image runat="server" ID="P12017" />
		<asp:Image runat="server" ID="P12018" />
		<asp:Image runat="server" ID="P12019" />
	</div>

	<!--
	<div style="float:left;width:10%">&nbsp;</div>
	<div style="float:left;width:20%">
		<p style="color:#FF7400;font-family:Sans-serif;font-size:18px;font-weight:600;letter-spacing:0.8px">
		SITE MAP
		</p>
		<asp HyperLink run@t="server" ID="X100008" CssClass="TopButton TopButtonO"></asp:HyperLink>&nbsp;
		<asp HyperLink run@t="server" ID="X100009" CssClass="TopButton TopButtonY"></asp:HyperLink>
	</div>
	<div style="float:left;width:10%">&nbsp;</div>
	-->

	<!--#include file="IncludeErrorDtl.htm" -->

	<asp:Label runat="server" ID="lblError" CssClass="Error" Visible="false" Enabled="false" ViewStateMode="Disabled"></asp:Label>
	<asp:HiddenField runat="server" ID="hdnVer" />
</form>
<ascx:Footer runat="server" ID="ascxFooter" />
</body>
</html>
