using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewCurrentBalances : BasePageCRM
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

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_Get_CRMClientBalancesA @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadDataInitial",11840,"Internal database error (sp_Get_CRMClientBalancesA failed)",sql,1,1);
				else if ( ! mList.EOF )
				{
					lblRegFee.Text      = mList.GetColumn("txtRegistrationFeeDue");
					lblGrantLimit.Text  = mList.GetColumn("txtEmergencyCashLimit");
					lblGrantAvail.Text  = mList.GetColumn("txtEmergencyCashAvailable");
					lblGrantStatus.Text = mList.GetColumn("txtEmergencyCashBenefitStatus");
					lblMonthlyFee.Text  = mList.GetColumn("txtMonthlyFee");
					lblFeeDate.Text     = mList.GetColumn("txtLastFeePaymentDate");
				}
			}
		}
	}
}