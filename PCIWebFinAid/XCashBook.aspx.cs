using System;
using System.Text;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XCashBook : BasePageLogin
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( SessionCheck(19) != 0 )
				return;
			if ( PageCheck()      != 0 )
				return;
			if ( Page.IsPostBack )
				return;
			if ( ascxXMenu.LoadMenu(sessionGeneral.UserCode) != 0 )
				StartOver(10888);
			else
				SetErrorDetail("",-888);
		}

		private int ValidateData()
		{
			return 0;

//			lblTransactions.Text = "";
//			txtCard1.Text        = txtCard1.Text.Trim().Replace(" ","");
//			txtCard3.Text        = txtCard3.Text.Trim().Replace(" ","");
//			string cardNo        = txtCard1.Text + txtCard3.Text;
//			string err           = "";
//			if ( cardNo.Length  != 10 )
//				err = "Invalid card number (6 and 4 digits needed)<br />";
//			else
//				for ( int k = 0 ; k < cardNo.Length ; k++ )
//					if ( ! "0123456789".Contains(cardNo.Substring(k,1)) )
//					{
//						err = "Invalid card number (only digits allowed)<br />";
//						break;
//					}
//			DateTime d1 = Tools.StringToDate(txtDate1.Text,1);
//			DateTime d2 = Tools.StringToDate(txtDate2.Text,1);
//			if ( d1 <= Constants.C_NULLDATE() )
//				err = err + "Invalid from date (it must be in dd/mm/yyyy format)<br />";
//			if ( d2 <= Constants.C_NULLDATE() )
//				err = err + "Invalid to date (it must be in dd/mm/yyyy format)<br />";
//			if ( d1 > d2 && d2 > Constants.C_NULLDATE() )
//				err = err + "From date cannot be after to date<br />";
//			if ( err.Length > 0 )
//				SetErrorDetail("ValidateData",31010,err,err);
//			return err.Length;
		}


		protected void grdData_ItemCommand(Object sender, DataGridCommandEventArgs e)
		{
			try
			{


			}
			catch
			{ }
		}

		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			grdData.Visible    = false;
			pnlGridBtn.Visible = false;

			if ( ValidateData() > 0 )
				return;

			sql = "exec sp_Audit_Get_CashbookExtract @CompanyCode=''";
	
			using ( MiscList miscList = new MiscList() )
				if ( miscList.ExecQuery(sql,1,"",false) == 0 || miscList.EOF )
					SetErrorDetail("btnSearch_Click",30061,"No transactions found. Refine your criteria and try again",sql,2,0);
				else
				{
					grdData.Visible    = true;
					pnlGridBtn.Visible = true;
					grdData.DataSource = miscList;
					grdData.DataBind();
				}
		}
	}
}