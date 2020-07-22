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
			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode,ApplicationCode) != 0 )
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