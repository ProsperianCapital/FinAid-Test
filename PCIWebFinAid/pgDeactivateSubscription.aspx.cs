using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgDeactivateSubscription : BasePageCRM
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

//	Test
//				lblCurr.Text    = "R";
//				lblBalance.Text = "8913.76";
//				lstReason.Items.Add(new System.Web.UI.WebControls.ListItem("123","Lost Interest"));
//				lstReason.Items.Add(new System.Web.UI.WebControls.ListItem("443","Gave up"));
//				lstReason.Items.Add(new System.Web.UI.WebControls.ListItem("123","Bugger off"));
//	Test

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_CRM_GetContractSettlementBalance @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadDataInitial",11840,"<br /><br />Internal database error (sp_CRM_GetContractSettlementBalance failed)",sql,1,1);
				else if ( ! mList.EOF )
				{
					lblBalance.Text = mList.GetColumn("SettlementBalance");
					lblCurr.Text    = mList.GetColumn("CurrencySymbol");
				}

				System.Web.UI.WebControls.ListItem lItem;

				sql = "exec sp_CRM_GetContractCancellationReasonList"
				    +     " @LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
				    +     ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
				    +     ",@Access='N'";

				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadDataInitial",11840,"<br /><br />Internal database error (sp_CRM_GetContractCancellationReasonList failed)",sql,1,1);
				else
					while ( ! mList.EOF )
					{
						lItem       = new System.Web.UI.WebControls.ListItem();
						lItem.Value = mList.GetColumn("CancellationReasonCode");
						lItem.Text  = mList.GetColumn("CancellationReasonDescription");
						lstReason.Items.Add(lItem);
						mList.NextRow();
					}
			}

			lstReason.Focus();
		}

		protected void btnChange_Click(Object sender, EventArgs e)
		{
			SetErrorDetail("btnChange_Click",11840,"<br /><br />Not yet implemented ...");
		}

		protected void btnConfirm_Click(Object sender, EventArgs e)
		{
		//	int amt     = Tools.StringToInt(txtAmount.Text);

		//	if ( amt < 1 || txtBank.Text.Length       < 2
		//	             || txtBranchName.Text.Length < 2 
		//	             || txtBranchCode.Text.Length < 4
		//	             || txtAccName.Text.Length    < 2 
		//	             || txtAccNumber.Text.Length  < 5 )
		//		return;

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_CRM_LogClientCancellationRequestA"
				    +     " @ContractCode="           + Tools.DBString(sessionGeneral.ContractCode)
				    +     ",@CancellationReasonCode=" + Tools.DBString(WebTools.ListValue(lstReason,"0"))
				    +     ",@LanguageCode="           + Tools.DBString(sessionGeneral.LanguageCode)
				    +     ",@LanguageDialectCode="    + Tools.DBString(sessionGeneral.LanguageDialectCode)
				    +     ",@Access='N'";

				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnConfirm_Click",11840,"<br /><br />Internal database error (sp_CRM_LogClientCancellationRequestA)",sql,1,1);
				else if ( ! mList.EOF )
				{
					lblError.Text    = "<br /><br />" + mList.GetColumn("ResultMessage");
					lblError.Visible = true;
				}
			}
		}
	}
}