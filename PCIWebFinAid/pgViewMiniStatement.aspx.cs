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
				StartOver(18666);
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
				sql = "exec sp_Get_CRMClientMiniStatementA @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadDataInitial",11840,"Internal database error (sp_Get_CRMClientMiniStatementA failed)",sql,1,1);
				else
					while ( ! mList.EOF )
					{
						TableRow  row  = new TableRow();
						TableCell col1 = new TableCell();
						TableCell col2 = new TableCell();
						TableCell col3 = new TableCell();
						TableCell col4 = new TableCell();
						col1.Text      = mList.GetColumn("Date");
						col2.Text      = mList.GetColumn("Description");
						col3.Text      = mList.GetColumn("Amount");
						col4.Text      = mList.GetColumn("Balance");
						balance        = col4.Text;
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