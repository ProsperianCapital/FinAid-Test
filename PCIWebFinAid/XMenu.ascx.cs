using System;

namespace PCIWebFinAid
{
	public partial class XMenu : System.Web.UI.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

		public void LoadMenu(string userCode)
		{
			userCode = PCIBusiness.Tools.NullToString(userCode);
			if ( userCode.Length < 1 )
				return;

			lblMenu.Text =
				@"<div style=""float:left;vertical-align:top;padding:5px;margin-right:8px;background-color:black"">
					<table id=""m01"" style=""position:absolute;left:152px;visibility:hidden;display:none;border:1px #000000 solid"" onmouseleave=""JavaScript:XMenu('',0)"">
						<tr><td class=""VHead"">Prosperian Capital</td></tr>
						<tr><td class=""VMenu""><a href=""XHome.aspx""> Dashboard </a></td></tr>
						<tr><td class=""VMenu""> FinAudit </td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Cashbooks </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> General Journal </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Cashbook Recon </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Purchases </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Payments </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Intra-Group Billing </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Processing </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Loan Account Recon </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Reports & Extracts </a></td></tr>
					</table>
					<a href=""JavaScript:XMenu('m01',1)""><img src=""Images/PCapital.png"" height=""75"" onmouseover=""JavaScript:XMenu('m01',1)"" /></a>
					<br /><hr />
					<table id=""m02"" style=""position:absolute;left:152px;visibility:hidden;display:none;border:1px #000000 solid"" onmouseleave=""JavaScript:XMenu('',0)"">
						<tr><td class=""VHead"">Prosperian FinTech</td></tr>
						<tr><td class=""VMenu""><a href=""XHome.aspx""> Dashboard </a></td></tr>
						<tr><td class=""VMenu""> eServe CRM </td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Contract Lookup </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XTransLookup.aspx""> Transaction Lookup </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Loyalty Rewards </a></td></tr>
						<tr><td class=""VMenu""> PPC Costing </td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Marketing Return </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Vintage Analysis </a></td></tr>
					</table>
					<a href=""JavaScript:XMenu('m02',1)""><img src=""Images/PFintech.png"" height=""75"" onmouseover=""JavaScript:XMenu('m02',1)"" /></a>
					<br /><hr />
					<table id=""m03"" style=""position:absolute;left:152px;visibility:hidden;display:none;border:1px #000000 solid"" onmouseleave=""JavaScript:XMenu('',0)"">
						<tr><td class=""VHead"">Prosperian Wealth</td></tr>
						<tr><td class=""VMenu""><a href=""XHome.aspx""> Dashboard </a></td></tr>
						<tr><td class=""VMenu""> Interactive Brokers </td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Account Summary </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Portfolio Summary </a></td></tr>
						<tr><td class=""VMenu"">&nbsp;&nbsp;-> <a href=""XHome.aspx""> Orders Review </a></td></tr>
						<tr><td class=""VMenu""><a href=""XHome.aspx""> Currency Exchange </a></td></tr>
						<tr><td class=""VMenu""><a href=""XHome.aspx""> Stock Tickers </a></td></tr>
					</table>
					<a href=""JavaScript:XMenu('m03',1)""><img src=""Images/PWealth.png"" height=""75"" onmouseover=""JavaScript:XMenu('m03',1)"" /></a>
					<br /><hr />
					<table id=""m04"" style=""position:absolute;left:152px;visibility:hidden;display:none;border:1px #000000 solid"" onmouseleave=""JavaScript:XMenu('',0)"">
						<tr><td class=""VHead"">KNAB Global</td></tr>
						<tr><td class=""VMenu""><a href=""XHome.aspx""> Dashboard </a></td></tr>
						<tr><td class=""VMenu""><a href=""XHome.aspx""> Transactions </a></td></tr>
					</table>
					<a href=""JavaScript:XMenu('m04',1)""><img src=""Images/PKnab.png"" width=""130"" onmouseover=""JavaScript:XMenu('m04',1)"" /></a>
					<br /><hr />
					<br />
					<div style=""text-align:center"">
					<a href=""XLogin.aspx"" style=""font-size:20px;font-weight:bold;text-decoration:none;color:white"" onmouseover=""JavaScript:XMenu('',0)"" title=""Close all resources and log out"">Log Off</a>
					</div>
					<br />
				</div>";
		}

	}
}