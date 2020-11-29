using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgRequestCashReward : BasePageCRM
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
				StartOver(15666);
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
			txtAmount.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			decimal amt = Tools.StringToDecimal(txtAmount.Text);

			if ( amt < 1 || txtBank.Text.Length       < 2
			             || txtBranchName.Text.Length < 2 
			             || txtBranchCode.Text.Length < 4
			             || txtAccName.Text.Length    < 2 
			             || txtAccNumber.Text.Length  < 5 )
				return;

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_CRM_ApplyForEmergencyCash"
				    +     " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode)
				    +     ",@ProductBenefitPurposeCode='000'"
				    +     ",@CB1=''"
				    +     ",@CB2=''"
				    +     ",@CB3=''"
				    +     ",@CB4=''"
				    +     ",@CB5=''"
				    +     ",@BankName="            + Tools.DBString(txtBank.Text)
				    +     ",@AccountHolderName="   + Tools.DBString(txtAccName.Text)
				    +     ",@AccountNumber="       + Tools.DBString(txtAccNumber.Text)
				    +     ",@BranchName="          + Tools.DBString(txtBranchName.Text)
				    +     ",@SWIFTorIBAN="         + Tools.DBString(txtSwift.Text)
				    +     ",@BranchCode="          + Tools.DBString(txtBranchCode.Text)
				    +     ",@Access='N'"
				    +     ",@Amount='"             + Tools.DecimalToCurrency(amt) + "'"
				    +     ",@LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
				    +     ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode);

				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnOK_Click",11840,"Internal database error (sp_CRM_ApplyForEmergencyCash)",sql,1,1);
				else if ( ! mList.EOF )
				{
					lblError.Text    = "<br /><br />" + mList.GetColumn("ActionResultMessage");
					lblError.Visible = true;
				}
			}
		}
	}
}