using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class pgChangeEMail : BasePageCRM
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
				LoadDataInitial();
			else
				StartOver(13444);
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
			txtEMailNew1.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			string emailNew1 = txtEMailNew1.Text.Trim().ToUpper();
			string emailNew2 = txtEMailNew2.Text.Trim().ToUpper();

			if ( emailNew1 == emailNew2 && PCIBusiness.Tools.CheckEMail(emailNew1,1)
			                            && PCIBusiness.Tools.CheckEMail(emailNew2,1) )
				SetErrorDetail("",13445,"[SQL] Update yet to be implemented","",102,0);
		}
	}
}