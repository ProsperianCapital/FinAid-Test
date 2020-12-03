using System;
using System.Web.UI;
using PCIBusiness;

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

			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_CRM_GetContractAddress";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@Access="       + Tools.DBString(sessionGeneral.AccessType);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadDataInitial",15100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
				{
					lblLine1.Text = mList.GetColumn("AddressLine1");
					lblLine2.Text = mList.GetColumn("AddressLine2");
					lblLine3.Text = mList.GetColumn("AddressLine3");
					lblLine4.Text = mList.GetColumn("AddressLine4");
					lblLine5.Text = mList.GetColumn("AddressLine5");
				}
			}
			txtLine1.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			string addr1 = txtLine1.Text.Trim();
			string addr2 = txtLine2.Text.Trim();
			string addr3 = txtLine3.Text.Trim();
			string addr4 = txtLine4.Text.Trim();
			string addr5 = txtLine5.Text.Trim();

			if ( addr1.Length < 2 || addr2.Length < 2 ) 
				return;
			if ( addr3.Length < 2 && addr4.Length+addr5.Length > 1 )
				return;
			if ( addr4.Length < 2 && addr5.Length > 1 )
				return;

			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_CRM_ChangeContractAddressA";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@Access="       + Tools.DBString(sessionGeneral.AccessType)
				                            + ",@NewLine1="     + Tools.DBString(addr1,47) // Unicode
				                            + ",@NewLine2="     + Tools.DBString(addr2,47)
				                            + ",@NewLine3="     + Tools.DBString(addr3,47)
				                            + ",@NewLine4="     + Tools.DBString(addr4,47)
				                            + ",@NewLine5="     + Tools.DBString(addr5,47);

				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("btnOK_Click",15200,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
					SetErrorDetail("btnOK_Click",15210,mList.GetColumn("ResultMessage"),"",102,0);
			}
		}
	}
}