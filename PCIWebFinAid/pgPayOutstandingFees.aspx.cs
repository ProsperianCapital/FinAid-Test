using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgPayOutstandingFees : BasePageCRM
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
				StartOver(18010);
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

//	Test
//			lblCurr.Text    = "R";
//			lblBalance.Text = "8913.76";
//	Test

			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_CRM_GetContractSettlementBalance";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadDataInitial",23100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
				{
					lblBalance.Text = mList.GetColumn("SettlementBalance");
					lblCurr.Text    = mList.GetColumn("CurrencySymbol");
				}
			}

			txtAmt.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			decimal amt = Tools.StringToDecimal(txtAmt.Text);

			if ( amt > 0 )
				using (MiscList mList = new MiscList())
				{
					sqlProc = "sp_CRM_Blah";
					sql     = "exec " + sqlProc + " @ContractCode="        + Tools.DBString(sessionGeneral.ContractCode)
					                            + ",@AmountToPay='"        + Tools.DecimalToCurrency(amt) + "'"
					                            + ",@LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
					                            + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
					                            + ",@Access="              + Tools.DBString(sessionGeneral.AccessType);
					if ( mList.ExecQuery(sql,0,"",false) != 0 )
						SetErrorDetail("btnOK_Click",18100,"Internal database error (" + sqlProc + ")",sql,102,1);
					else if ( ! mList.EOF )
						SetErrorDetail("btnOK_Click",18120,mList.GetColumn("ResultMessage"),"",102,0);
				}
		}
	}
}