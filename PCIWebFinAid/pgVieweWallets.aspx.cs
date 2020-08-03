// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Text;
using System.Web.UI.WebControls;
using PCIBusiness;

// Error codes 80000-80099

namespace PCIWebFinAid
{
	public partial class pgVieweWallets : BasePageBackOffice
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck(99) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			int ret = ascxXMenu.LoadMenu(sessionGeneral.UserCode,ApplicationCode);
			if ( ret == 0 )
				return;
			Tools.LogInfo("pgVieweWallets.PageLoad","ret="+ret.ToString(),222);
			StartOver(10888);
		}

		private void LoadDataInitial()
		{
//		Called once in the beginning

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}
	}
}