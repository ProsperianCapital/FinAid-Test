﻿using System;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewProductDashboard : BasePageCRM
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
				LoadPageData();
			else
				StartOver(12010);
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

			LoadLabelText(ascxXMenu);

			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_WP_Get_DashboardInfo";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadPageData",12100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( mList.EOF )
					SetErrorDetail("LoadPageData",12110,"Internal database error (" + sqlProc + ", no data returned)",sql,102,1);
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