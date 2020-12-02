using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewMiniStatement : BasePageCRM
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
				StartOver(13010);
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
				string balance = "";
				sqlProc = "sp_Get_CRMClientMiniStatementB";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0,"",false) != 0 )
					SetErrorDetail("LoadDataInitial",13100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else
					while ( ! mList.EOF )
					{
						TableRow  row  = new TableRow();
						TableCell col1 = new TableCell();
						TableCell col2 = new TableCell();
						TableCell col3 = new TableCell();
						TableCell col4 = new TableCell();
//						col1.Text      = Tools.DateToString(mList.GetColumnDate("Date"),7,1); // yyyy-mm-dd hh:mm:ss
						col1.Text      = mList.GetColumn("Date");
						col2.Text      = mList.GetColumn("Description");
						col3.Text      = mList.GetColumn("Amount");
						col4.Text      = mList.GetColumn("Balance");
						balance        = col4.Text;
						col3.HorizontalAlign = HorizontalAlign.Right;
						col4.HorizontalAlign = HorizontalAlign.Right;
						row.Cells.Add(col1);
						row.Cells.Add(col2);
						row.Cells.Add(col3);
						row.Cells.Add(col4);
						tblStatement.Rows.Add(row);
						mList.NextRow();
					}
				lblBalance.Text = balance;
			}
		}
	}
}