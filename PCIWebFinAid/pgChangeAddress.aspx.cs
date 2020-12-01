using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PCIWebFinAid
{
	public partial class pgChangeAddress : BasePageCRM
	{
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
				StartOver(15010);
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
			txtLine1.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			string addr1  = txtLine1.Text.Trim();
			string addr2  = txtLine2.Text.Trim();
			string addr3  = txtLine3.Text.Trim();
			string addr4  = txtLine4.Text.Trim();
			string addr5  = txtLine5.Text.Trim();

			if ( addr1.Length < 2 || addr2.Length < 2 ) 
				return;
			if ( addr3.Length < 2 && addr4.Length > 1 )
				return;
			if ( addr4.Length < 2 && addr5.Length > 1 )
				return;

			SetErrorDetail("",15100,"[SQL] Update yet to be implemented","",102,0);
		}
	}
}