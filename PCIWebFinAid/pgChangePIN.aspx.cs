using System;
using System.Web.UI;

namespace PCIWebFinAid
{
	public partial class pgChangePIN : BasePageCRM
	{
		protected const int MIN_PIN_LENGTH = 4;
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck() != 0 )
				return;
			if ( PageCheck()    != 0 )
				return;

			ClearData();

			if ( Page.IsPostBack )
				return;

			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode,ApplicationCode) == 0 )
				LoadDataInitial();
			else
				StartOver(17010);
		}

		private void ClearData()
		{
//		Called every time

			SetErrorDetail("",-888);
			ascxXFooter.JSText = "";
		}

		private void LoadDataInitial()
		{
//		Called once in the beginning

			LoadLabelText(ascxXMenu);
			txtPINOld.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			string pinOld  = txtPINOld.Text.Trim();
			string pinNew1 = txtPINNew1.Text.Trim();
			string pinNew2 = txtPINNew2.Text.Trim();

			if ( pinNew1 == pinNew2 && PCIBusiness.Tools.CheckPIN(pinOld ,MIN_PIN_LENGTH)
			                        && PCIBusiness.Tools.CheckPIN(pinNew1,MIN_PIN_LENGTH)
			                        && PCIBusiness.Tools.CheckPIN(pinNew2,MIN_PIN_LENGTH) )
				SetErrorDetail("",17100,"[SQL] Update yet to be implemented","",102,0);
		}
	}
}