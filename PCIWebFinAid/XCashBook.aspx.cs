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
				LoadData();
		}

		private void LoadData()
		{
			SetErrorDetail("",-888);
			int errNo = 0;

			try
			{
				sql   = "exec sp_Audit_Get_Company";
				errNo = WebTools.ListBind(lstSCompany,sql,null,"CompanyCode","CompanyDescription","(All/any company)","");
				SetErrorDetail("LoadData",errNo,"Unable to load company list",sql);
				errNo = WebTools.ListBind(lstSOBOCompany,sql,null,"CompanyCode","CompanyDescription","(All/any company)","");
				SetErrorDetail("LoadData",errNo,"Unable to load company list",sql);

//				sql   = "exec sp_Audit_Get_CompanyCashbook";
//				errNo = WebTools.ListBind(lstSCompany,sql,null,"CompanyCode","CompanyDescription","","");
//				SetErrorDetail("LoadData",errNo,"Unable to load comapny list",sql);

				sql   = "exec sp_Audit_Get_RP";
				errNo = WebTools.ListBind(lstSReceipt,sql,null,"ReceiptCode","ReceiptDescription","(All/any receipt)","");
				SetErrorDetail("LoadData",errNo,"Unable to load receipt/payment list",sql);

				sql   = "exec sp_Audit_Get_TransactionType";
				errNo = WebTools.ListBind(lstSTType,sql,null,"TransactionTypeCode","TransactionTypeDescription","(All/any type)","");
				SetErrorDetail("LoadData",errNo,"Unable to load transaction type list",sql);

//				sql   = "exec sp_Audit_Get_GLAccount";
//				errNo = WebTools.ListBind(lstSGLAccount,sql,null,"CompanyCode","CompanyDescription","","");
//				SetErrorDetail("LoadData",errNo,"Unable to load comapny list",sql);

//				sql   = "exec sp_Audit_Get_GLAccountDimension";
//				errNo = WebTools.ListBind(lstSGLDimension,sql,null,"CompanyCode","CompanyDescription","","");
//				SetErrorDetail("LoadData",errNo,"Unable to load comapny list",sql);

				sql   = "exec sp_Audit_Get_TaxRate";
				errNo = WebTools.ListBind(lstSTaxRate,sql,null,"TaxRateCode","TaxRateDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load tax rate list",sql);
			}
			catch (Exception ex)
			{
				SetErrorDetail("LoadData",80010,"Internal error",sql,2,2,ex);
			}
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
//			DateTime d1 = Tools.StringToDate(txtDate1.Text,7);
//			DateTime d2 = Tools.StringToDate(txtDate2.Text,7);
//			if ( d1 <= Constants.C_NULLDATE() )
//				err = err + "Invalid from date (it must be in yyyy/mm/dd format)<br />";
//			if ( d2 <= Constants.C_NULLDATE() )
//				err = err + "Invalid to date (it must be in yyyy/mm/dd format)<br />";
//			if ( d1 > d2 && d2 > Constants.C_NULLDATE() )
//				err = err + "From date cannot be later than to date<br />";
//			if ( err.Length > 0 )
//				SetErrorDetail("ValidateData",31010,err,err);
//			return err.Length;
		}


		protected void grdData_ItemCommand(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
//				DataGridItem row     = e.Item;
				string       cmdName = e.CommandName.Trim().ToUpper();
				string       tranID  = e.CommandArgument.ToString();

				if ( cmdName == "EDIT" && tranID.Length > 0 )
				{
					txtETranID.Text    = tranID;
					ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(1,'Edit/Delete')");
					lstECompany.Focus();
				}
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

			sql = "exec sp_Audit_Get_CashbookExtract @CompanyCode=" + Tools.DBString(WebTools.ListValue(lstSCompany,""));
	
			using ( MiscList miscList = new MiscList() )
				if ( miscList.ExecQuery(sql,1,"",false) == 0 || miscList.EOF )
					SetErrorDetail("btnSearch_Click",30061,"No transactions found. Refine your criteria and try again",sql,2,2);
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