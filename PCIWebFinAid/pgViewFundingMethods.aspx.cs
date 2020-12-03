﻿using System;
using System.Web.UI;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class pgViewFundingMethods : BasePageCRM
	{
		protected override void PageLoad(object sender, EventArgs e)
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
				StartOver(20010);
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

			System.Web.UI.WebControls.ListItem lItem;
			for ( int k = 1 ; k < 13 ; k++ )
			{
				lItem       = new System.Web.UI.WebControls.ListItem();
				lItem.Value = k.ToString();
				lItem.Text  = k.ToString();
				if ( lItem.Text.Length < 2 )
					lItem.Text = "0" + lItem.Text;
				lstMM.Items.Add(lItem);
			}
			for ( int k = System.DateTime.Now.Year ; k < System.DateTime.Now.Year+16 ; k++ )
			{
				lItem       = new System.Web.UI.WebControls.ListItem();
				lItem.Value = k.ToString();
				lItem.Text  = k.ToString();
				lstYY.Items.Add(lItem);
			}

			using (MiscList mList = new MiscList())
			{
				sqlProc = "sp_CRM_GetContractPaymentInfoA";
				sql     = "exec " + sqlProc + " @ContractCode=" + Tools.DBString(sessionGeneral.ContractCode)
				                            + ",@Access="       + Tools.DBString(sessionGeneral.AccessType);
				if ( mList.ExecQuery(sql,0) != 0 )
					SetErrorDetail("LoadDataInitial",20100,"Internal database error (" + sqlProc + ")",sql,102,1);
				else if ( ! mList.EOF )
				{ 
					lblName.Text    = mList.GetColumn("CardHolderName");
					lblNumber.Text  = mList.GetColumn("MaskedCardNumber");
					lblCVV.Text     = mList.GetColumn("CardCVVCode");
					string mm       = mList.GetColumn("CardExpiryMonth");
					string yy       = mList.GetColumn("CardExpiryYear");
					if ( mm.Length > 0 && yy.Length > 0 )
						lblDate.Text = mm + " / " + yy;
					WebTools.ListSelect(lstMM,mm);
					WebTools.ListSelect(lstYY,yy);
				}
			}
			txtName.Focus();
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			try
			{
				string   cName   = txtName.Text.Trim();
				string   cNumber = txtNumber.Text.Trim();
				int      cCVV    = Tools.StringToInt(txtCVV.Text);
				int      cDD     = 31;
				int      cMM     = WebTools.ListValue(lstMM);
				int      cYY     = WebTools.ListValue(lstYY);

				if ( cName.Length < 3 || cNumber.Length < 14 || cCVV < 1 || txtCVV.Text.Length < 3 || cMM < 1 || cYY < System.DateTime.Now.Year )
					return;

				if ( cMM == 4 || cMM == 6 || cMM == 9 || cMM == 11 )
					cDD = 30;
				else if ( cMM == 2 && (cYY % 4) == 0 )
					cDD = 29;
				else if ( cMM == 2 )
					cDD = 28;
				DateTime expDt = new DateTime(cYY,cMM,cDD,23,59,59);
				bool     allOK = ( expDt > System.DateTime.Now );

				for ( int k = 0 ; k < cNumber.Length ; k++ )
					if ( ! ("0123456789").Contains(cNumber.Substring(k,1)) )
					{
						allOK = false;
						break;
					}

//			if (allOK)
//				using (MiscList mList = new MiscList())
//				{
//					sqlProc = "sp_CRM_Blah";
//					sql     = "exec " + sqlProc + " @ContractCode="        + Tools.DBString(sessionGeneral.ContractCode)
//					                            + ",@X='"        + Tools.DecimalToCurrency(amt) + "'"
//					                            + ",@Y="        + Tools.DBString(sessionGeneral.LanguageCode)
//					                            + ",@Z=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
//					                            + ",@Access="              + Tools.DBString(sessionGeneral.AccessType);
//					if ( mList.ExecQuery(sql,0,"",false) != 0 )
//						SetErrorDetail("btnOK_Click",20200,"Internal database error (" + sqlProc + ")",sql,102,1);
//					else if ( ! mList.EOF )
//						SetErrorDetail("btnOK_Click",20220,mList.GetColumn("ResultMessage"),"",102,0);
//				}

			}
			catch
			{ }
		}
	}
}