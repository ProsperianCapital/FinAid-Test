using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XHome : BasePageBackOffice
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode,sessionGeneral.ApplicationCode) != 0 )
				StartOver(10999);
		}
	}
}