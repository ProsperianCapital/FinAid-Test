using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewProductActivityLog : BasePageCRM
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
				StartOver(14010);
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
				bool rowEven = true;
				sqlProc      = "sp_CRM_GetContractContactLog";
				sql          = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("LoadDataInitial",14100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else
					while ( ! mList.EOF )
					{
						TableRow  row  = new TableRow();
						TableCell col1 = new TableCell();
						TableCell col2 = new TableCell();
						col1.Text      = mList.GetColumn("ContactDate");
						col2.Text      = mList.GetColumn("ContactDescription");
						row.Cells.Add(col1);
						row.Cells.Add(col2);
						if (rowEven)
							row.CssClass = "tRow";
						else
							row.CssClass = "tRowAlt";
						rowEven = ! rowEven;
						tblHistory.Rows.Add(row);
						mList.NextRow();
					}
			}
		}
	}
}