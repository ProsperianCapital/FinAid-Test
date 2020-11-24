using System;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewProductDashboard : BasePageBackOffice
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
				StartOver(12777);
		}

		private void LoadDataInitial()
		{
//		Called once in the beginning

			SetErrorDetail("",-888);
			LoadLabelText(ascxXMenu);

			ascxXFooter.JSText = "";

			using (MiscList mList = new MiscList())
			{
				sql = "exec sp_WP_Get_DashboardInfo @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("PageLoad",11040,"Internal database error (sp_WP_Get_DashboardInfo failed)",sql,1,1);
				else if ( mList.EOF )
					SetErrorDetail("PageLoad",11050,"Internal database error (sp_WP_Get_DashboardInfo no data returned)",sql,1,1);
				else
				{
					lblName.Text         = mList.GetColumn("ClientName");
					lblStatus.Text       = mList.GetColumn("ContractStatusDescription");
					lblContractCode.Text = mList.GetColumn("ContractCode");
					lblClientCode.Text   = mList.GetColumn("ClientCode");
				}
			}
		}
	}
}