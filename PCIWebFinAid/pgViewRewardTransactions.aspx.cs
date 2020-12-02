using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewRewardTransactions : BasePageCRM
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
				StartOver(19010);
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
				sqlProc = "sp_CRM_GetContractEmergencyCash";
				sql     = "exec " + sqlProc + " @ContractCode="        + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@LanguageCode="        + Tools.DBString(sessionGeneral.LanguageCode)
				                            + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
				                            + ",@Access="              + Tools.DBString(sessionGeneral.AccessType);
				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("LoadDataInitial",19100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else
					while ( ! mList.EOF )
					{
						TableRow  row = new TableRow();
						TableCell col;
						for ( int k = 0 ; k < 6 ; k++ )
						{
							col      = new TableCell();
							col.Text = mList.GetColumn(k);
							row.Cells.Add(col);
						}
						tblData.Rows.Add(row);
						mList.NextRow();
					}
			}
		}
	}
}