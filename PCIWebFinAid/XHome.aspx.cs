using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XHome : BasePageLogin
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			ascxXMenu.LoadMenu(sessionGeneral.UserCode);
		}
	}
}