// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;

// Error codes 80000-80099

namespace PCIWebFinAid
{
	public partial class pgVieweWallets : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck() != 0 )
				return;
			if ( PageCheck()    != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode,ApplicationCode) == 0 )
				LoadDataInitial();
			else
				StartOver(17666);
		}

		private void LoadDataInitial()
		{
//		Called once in the beginning

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
			LoadLabelText(ascxXMenu);
		}
	}
}