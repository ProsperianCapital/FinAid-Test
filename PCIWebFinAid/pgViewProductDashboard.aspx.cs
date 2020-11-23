using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewProductDashboard : BasePageBackOffice
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck() != 0 )
				return;
			if ( PageCheck()    != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode,ApplicationCode) != 0 )
				StartOver(10999);
		}
	}
}