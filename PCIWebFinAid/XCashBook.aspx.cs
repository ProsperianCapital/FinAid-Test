using System;
using System.Text;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class XCashBook : BasePageLogin
	{
		string cashBook;
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
			ascxXFooter.JSText = WebTools.JavaScriptSource("HidePopups()");
			int errNo          = 0;

			try
			{
				sql   = "exec sp_Audit_Get_Company";
				errNo = WebTools.ListBind(lstSCompany,sql,null,"CompanyCode","CompanyDescription","(All/any company)","");
				errNo = WebTools.ListBind(lstECompany,sql,null,"CompanyCode","CompanyDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load company list",sql);
				errNo = WebTools.ListBind(lstSOBOCompany,sql,null,"CompanyCode","CompanyDescription","(All/any company)","");
				errNo = WebTools.ListBind(lstEOBOCompany,sql,null,"CompanyCode","CompanyDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load company list",sql);

//				sql   = "exec sp_Audit_Get_CompanyCashbook";
//				This is done via an AJAX call

				sql   = "exec sp_Audit_Get_RP";
				errNo = WebTools.ListBind(lstSReceipt,sql,null,"RP","RP","(All/any receipt)","");
				errNo = WebTools.ListBind(lstEReceipt,sql,null,"RP","RP","","");
				SetErrorDetail("LoadData",errNo,"Unable to load receipt/payment list",sql);

				if ( lstSTType.Enabled )
				{
					sql   = "exec sp_Audit_Get_TransactionType";
					errNo = WebTools.ListBind(lstSTType,sql,null,"TransactionTypeCode","TransactionTypeDescription","(All/any type)","");
					errNo = WebTools.ListBind(lstETType,sql,null,"TransactionTypeCode","TransactionTypeDescription","","");
					SetErrorDetail("LoadData",errNo,"Unable to load transaction type list",sql);
				}

				sql   = "exec sp_Audit_Get_GLAccount";
				errNo = WebTools.ListBind(lstSGLCode,sql,null,"GLAccount","GLAcountDescription","(All/any account)","");
				errNo = WebTools.ListBind(lstEGLCode,sql,null,"GLAccount","GLAccountDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load GL account codes",sql);

				sql   = "exec sp_Audit_Get_GLAccountDimension";
				errNo = WebTools.ListBind(lstSGLDimension,sql,null,"CompanyCode","CompanyDescription","(All/any dimension)","");
				errNo = WebTools.ListBind(lstEGLDimension,sql,null,"CompanyCode","CompanyDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load GL account dimensions",sql);

				sql   = "exec sp_Audit_Get_TaxRate";
				errNo = WebTools.ListBind(lstSTaxRate,sql,null,"VatRateCode","VatRateDescription","(All/any tax rate)","");
//				errNo = WebTools.ListBind(lstETaxRate,sql,null,"VatRateCode","VatRateDescription","","");
				SetErrorDetail("LoadData",errNo,"Unable to load tax rate list",sql);

				sql   = "exec sp_Audit_Get_CUR";
				errNo = WebTools.ListBind(lstECurr,sql,null,"CUR","CUR","","");
				SetErrorDetail("LoadData",errNo,"Unable to currency list",sql);
			}
			catch (Exception ex)
			{
				SetErrorDetail("LoadData",80010,"Internal error",sql,2,2,ex);
			}
		}

		protected void grdData_ItemCommand(Object sender, DataGridCommandEventArgs e)
		{
			try
			{
				string       cmdName = e.CommandName.Trim().ToUpper();
				int          tranID  = Tools.StringToInt(e.CommandArgument.ToString());
//				DataGridItem row1    = grdData.Items[e.Item.ItemIndex];
				DataGridItem row     = e.Item;
				sql                  = "exec sp_Audit_Get_CashbookExtractAllFields @TransactionID=" + tranID.ToString();
				lblErr2.Text         = "";

				if ( cmdName == "EDIT" && tranID > 0 )
					using (MiscList cbTran = new MiscList())
						if ( cbTran.ExecQuery(sql,0) == 0 )
						{
							ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(1)");
							txtETranID.Text    = tranID.ToString();
							txtEDate.Text      = Tools.DateToString(cbTran.GetColumnDate("TransactionDate"),7);
							txtERecon.Text     = Tools.DateToString(cbTran.GetColumnDate("ReconDate"),7);
							txtEAmt.Text       = cbTran.GetColumnCurrency("TransactionAmountInclusive");
							txtEDesc.Text      = cbTran.GetColumn("TransactionDescription");
							txtETaxRate.Text   = cbTran.GetColumn("TaxRate");
							hdnECashBook.Value = cbTran.GetColumn("CashbookCode");
							lstECashBook.Items.Clear();
							WebTools.ListAdd(lstECashBook,0,hdnECashBook.Value,hdnECashBook.Value);
							WebTools.ListSelect(lstECompany    ,cbTran.GetColumn("CompanyCode"));
							WebTools.ListSelect(lstEOBOCompany ,cbTran.GetColumn("OBOCompanyCode"));
							WebTools.ListSelect(lstEReceipt    ,cbTran.GetColumn("RP"));
							WebTools.ListSelect(lstECurr       ,cbTran.GetColumn("CUR"));
							WebTools.ListSelect(lstETType      ,cbTran.GetColumn("TransactionDescription"));
							WebTools.ListSelect(lstEGLCode     ,cbTran.GetColumn("GLAccountCode"));
							WebTools.ListSelect(lstEGLDimension,cbTran.GetColumn("GLAccountDimension"));
//							WebTools.ListSelect(lstETaxRate    ,cbTran.GetColumn("TaxRate"));
							lstECompany.Focus();
						}

//					txtETranID.Text    = tranID;
//					txtEDate.Text      = row.Cells[2].Text;
//					txtEDesc.Text      = row.Cells[7].Text;
//					txtEAmt.Text       = row.Cells[9].Text;
//					WebTools.ListSelect(lstEReceipt    ,row.Cells[3].Text);
//					WebTools.ListSelect(lstETType      ,row.Cells[4].Text);
//					WebTools.ListSelect(lstEGLCode     ,row.Cells[5].Text);
//					WebTools.ListSelect(lstEGLDimension,row.Cells[6].Text);
//					WebTools.ListSelect(lstETaxRate    ,row.Cells[8].Text);
			}
			catch
			{ }
		}

//		private int ValidateSearch()
//		{
//			DateTime d1   = Tools.StringToDate(txtSDate1.Text,7);
//			DateTime d2   = Tools.StringToDate(txtSDate2.Text,7);
//			decimal  a1   = Tools.StringToDecimal(txtSAmt1.Text);
//			decimal  a2   = Tools.StringToDecimal(txtSAmt2.Text);
//			lblError.Text = "";
//
//			if ( d1 > Constants.C_NULLDATE() && d2 <= Constants.C_NULLDATE() )
//				SetErrorDetail("ValidateData",43400,"If you specify a start date you must also specify an end date");
//			else if ( d2 > Constants.C_NULLDATE() && d1 <= Constants.C_NULLDATE() )
//				SetErrorDetail("ValidateData",43400,"If you specify an end date you must also specify a start date");
//			else if ( d1 > Constants.C_NULLDATE() && d2 > Constants.C_NULLDATE() && d1 > d2 )
//				SetErrorDetail("ValidateData",43400,"The start date cannot be after the end date");
//			if ( a1 > 0 && a2 <= (decimal)0.01 )
//				SetErrorDetail("ValidateData",43430,"If you specify a from amount you must also specify a to amount");
//			else if ( a2 > 0 && a1 <= (decimal)0.01 )
//				SetErrorDetail("ValidateData",43430,"If you specify a to amount you must also specify a from amount");
//			else if ( a1 > 0 && a2 > 0 && a1 > a2 )
//				SetErrorDetail("ValidateData",43430,"The from amount cannot be greater than the to amount");
//
//			return lblError.Text.Length;
//		}

		protected void btnNew_Click(Object sender, EventArgs e)
		{
			ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(2)");
			txtETranID.Text    = "";
			txtEDate.Text      = "";
			txtERecon.Text     = "";
			txtEAmt.Text       = "";
			txtEDesc.Text      = "";
			txtETaxRate.Text   = "";
			hdnECashBook.Value = "";
			lblErr2.Text       = "";
			lstECashBook.Items.Clear();
			lstECompany.Focus();
		}

		protected void btnDelete_Click(Object sender, EventArgs e)
		{
			try
			{
				cashBook = Tools.NullToString(hdnECashBook.Value);
				sql      = "exec sp_Audit_Del_Cashbook @TransactionID=" + txtETranID.Text;
				if ( Tools.StringToInt(txtETranID.Text) > 0 )
					using ( MiscList miscList = new MiscList() )
						if ( miscList.ExecQuery(sql,0,"",false,true) == 0 )
							return;
			}
			catch (Exception ex)
			{
				SetErrorDetail("btnDelete_Click",30371,"Internal error deleting cash book transaction",sql,23,2,ex);
				return;
			}	
			SetErrorDetail("btnDelete_Click",30377,"Failed to delete cash book transaction",sql,23,2);
			ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode(1);LoadCashBooks(" + (cashBook.Length > 0 ? "'" + cashBook + "'" : "null") + ",'E')");
		}

		protected void btnUpdate_Click(Object sender, EventArgs e)
		{
			int      editInsert = Tools.StringToInt(hdnEditInsert.Value);
			int      taxRate    = Tools.StringToInt(txtETaxRate.Text);
			decimal  amt        = Tools.StringToDecimal(txtEAmt.Text);
			decimal  amtX       = 0;
			DateTime d1         = Tools.StringToDate(txtEDate.Text,7);
			DateTime d2         = Tools.StringToDate(txtERecon.Text,7);
			cashBook            = Tools.NullToString(hdnECashBook.Value);

			if ( amt > 0 && taxRate > 0 )
				amtX = amt / ( 1 + ( (decimal)taxRate / (decimal)100.00 ) );

			try
			{
				sql =  "@CompanyCode="                + Tools.DBString(WebTools.ListValue(lstECompany,""))
				    + ",@OBOCompanyCode="             + Tools.DBString(WebTools.ListValue(lstEOBOCompany,""))
				    + ",@CashbookCode="               + Tools.DBString(cashBook)
				    + ",@RP="                         + Tools.DBString(WebTools.ListValue(lstEReceipt,""))
				    + ",@TransactionDate="            + Tools.DateToSQL(d1,0)
				    + ",@ReconDate="                  + Tools.DateToSQL(d2,0)
				    + ",@GLAccountCode="              + Tools.DBString(WebTools.ListValue(lstEGLCode,""))
				    + ",@GLAccountDimension="         + Tools.DBString(WebTools.ListValue(lstEGLDimension,""))
				    + ",@TransactionDescription="     + Tools.DBString(txtEDesc.Text)
				    + ",@CUR="                        + Tools.DBString(WebTools.ListValue(lstECurr,""))
				    + ",@TaxRate="                    + taxRate.ToString()
					 + ",@TransactionAmountExclusive=" + amtX.ToString();
				if ( editInsert == 1 && Tools.StringToInt(txtETranID.Text) > 0 )
					sql = "exec sp_Audit_Upd_Cashbook @TransactionID=" + txtETranID.Text
					                              + ",@TransactionAmountTax=" + ( amtX * (decimal)taxRate / (decimal)100.00 ).ToString()
					                              + ",@TransactionAmountInclusive=" + amt.ToString() + "," + sql;
				else if ( editInsert == 2 )
					sql = "exec sp_Audit_Ins_Cashbook " + sql;
				else
					return;
	
				using ( MiscList miscList = new MiscList() )
					if ( miscList.ExecQuery(sql,0,"",false,true) == 0 )
						return;
			}
			catch (Exception ex)
			{
				SetErrorDetail("btnUpdate_Click",30171,"Internal error updating cash book transaction",sql,23,2,ex);
				return;
			}	
			SetErrorDetail("btnUpdate_Click",30177,"Failed to update cash book transaction",sql,23,2);
			ascxXFooter.JSText = WebTools.JavaScriptSource("EditMode("+editInsert.ToString()+");LoadCashBooks(" + (cashBook.Length > 0 ? "'" + cashBook + "'" : "null") + ",'E')");
		}

		protected void btnSearch_Click(Object sender, EventArgs e)
		{
			string   cashBook  = Tools.NullToString(hdnSCashBook.Value);
			DateTime d1        = Tools.StringToDate(txtSDate1.Text,7);
			DateTime d2        = Tools.StringToDate(txtSDate2.Text,7);
//			DateTime dR        = Tools.StringToDate(txtSRecon.Text,7);
			decimal  a1        = Tools.StringToDecimal(txtSAmt1.Text);
			decimal  a2        = Tools.StringToDecimal(txtSAmt2.Text);
			lblError.Text      = "";
			ascxXFooter.JSText = WebTools.JavaScriptSource("LoadCashBooks(" + (cashBook.Length > 0 ? "'" + cashBook + "'" : "null") + ",'S')");
			grdData.Visible    = false;
			pnlGridBtn.Visible = false;

			if ( d1 > Constants.C_NULLDATE() && d2 <= Constants.C_NULLDATE() )
				SetErrorDetail("ValidateData",43400,"If you specify a start date you must also specify an end date");
			else if ( d2 > Constants.C_NULLDATE() && d1 <= Constants.C_NULLDATE() )
				SetErrorDetail("ValidateData",43410,"If you specify an end date you must also specify a start date");
			else if ( d1 > Constants.C_NULLDATE() && d2 > Constants.C_NULLDATE() && d1 > d2 )
				SetErrorDetail("ValidateData",43420,"The start date cannot be after the end date");
//			if ( d1 > Constants.C_NULLDATE() && dR > Constants.C_NULLDATE() && dR < d1 )
//				SetErrorDetail("ValidateData",43430,"The recon date cannot be before the start date");
//			if ( d2 > Constants.C_NULLDATE() && dR > Constants.C_NULLDATE() && dR > d2 )
//				SetErrorDetail("ValidateData",43440,"The recon date cannot be after the end date");
			if ( a1 > 0 && a2 <= (decimal)0.01 )
				SetErrorDetail("ValidateData",43450,"If you specify a from amount you must also specify a to amount");
			else if ( a2 > 0 && a1 <= (decimal)0.01 )
				SetErrorDetail("ValidateData",43460,"If you specify a to amount you must also specify a from amount");
			else if ( a1 > 0 && a2 > 0 && a1 > a2 )
				SetErrorDetail("ValidateData",43470,"The from amount cannot be greater than the to amount");

			if ( lblError.Text.Length > 0 )
				return;

			string coy       = WebTools.ListValue(lstSCompany,"");
			string coyOBO    = WebTools.ListValue(lstSOBOCompany,"");
			string receipt   = WebTools.ListValue(lstSReceipt,"");
			string transType = WebTools.ListValue(lstSTType,"");
//			string taxType   = WebTools.ListValue(lstSTaxRate,"");
			string glAcc     = WebTools.ListValue(lstSGLCode,"");
			string glDim     = WebTools.ListValue(lstSGLDimension,"");
			string taxRate   = WebTools.ListValue(lstSTaxRate,"");

//			sql = "exec sp_Audit_Get_CashbookExtract @CompanyCode=" + Tools.DBString(coy);
			sql = "exec sp_Audit_Get_CashbookExtractA"
			    +     " @CompanyCode="            + Tools.DBString(coy)
			    +     ",@CashbookCode="           + Tools.DBString(cashBook)
			    +     ",@RP="                     + Tools.DBString(receipt)
			    +     ",@OBOCompanyCode="         + Tools.DBString(coyOBO)
			    +     ",@GLAccountCode="          + Tools.DBString(glAcc)
			    +     ",@GLAccountDimension="     + Tools.DBString(glDim)
			    +     ",@TransactionDescription=" + Tools.DBString(txtSDesc.Text)
			    +     ",@StartDate="              + Tools.DateToSQL(d1,0)
			    +     ",@EndDate="                + Tools.DateToSQL(d2,0)
			    +     ",@TaxRate="                + Tools.DBString(taxRate)
			    +     ",@MinAmount="              + a1.ToString()
			    +     ",@MaxAmount="              + a2.ToString();
	
			using ( MiscList miscList = new MiscList() )
				if ( miscList.ExecQuery(sql,1,"",false,true) < 1 )
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