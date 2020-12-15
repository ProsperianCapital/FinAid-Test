using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewSubscriptionDetail : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck() != 0 )
				return;
			if ( PageCheck()    != 0 )
				return;

			ClearData();

			if ( Page.IsPostBack )
				return;

			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode,ApplicationCode) == 0 )
				LoadPageData();
			else
				StartOver(21010);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		protected override void LoadPageData()
		{
//		Called once in the beginning
//
//			LoadLabelText(ascxXMenu);
		}
	}
}