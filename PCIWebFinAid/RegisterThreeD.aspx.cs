using System;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class RegisterThreeD : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			string transRef  = WebTools.RequestValueString(Request,"TransRef");
			string peachID   = WebTools.RequestValueString(Request,"id");
			string peachURL  = WebTools.RequestValueString(Request,"resourcePath");
			string errorCode = WebTools.RequestValueString(Request,"ErrorCode");
			string sql       = "exec sp_WP_PaymentRegister3DSec @ContractCode=" + Tools.DBString(transRef)
				                                             + ",@ReferenceNumber=" + Tools.DBString(peachID)
				                                             + ",@Status=";
			pnlError.Visible = false;
			pnlOK.Visible    = false;
			lblErr.Text      = "<br /><p>Details<br />"
			                 + " - Contract Code : <b>" + transRef + "</b><br />"
			                 + " - Transaction Id : <b>" + peachID + "</b><br />"
			                 + " - Error Code : <b>" + errorCode + "</b></p>";

			if ( transRef.Length < 1 )
			{
				pnlError.Visible = true;
				lblErr.Text      = "The 'Contract Code' returned from 3d Secure processing is invalid/missing" + lblErr.Text;
				sql              = sql + "'999.999.001'";
			}
			else if ( peachID.Length < 1 )
			{
				pnlError.Visible = true;
				lblErr.Text      = "The 'Transaction Id' returned from 3d Secure processing is invalid/missing" + lblErr.Text;
				sql              = sql + "'999.999.002'";
			}
			else if ( errorCode.Length > 0 )
			{
				pnlError.Visible = true;
				lblErr.Text      = "Your initial verification payment failed" + lblErr.Text;
				sql              = sql + "'999.999.003'";
			}
			else
				using (TransactionPeach trans = new TransactionPeach())
				{
					int ret      = trans.ThreeDSecureCheck(peachID);
					lblData.Text = "<p>Transaction<br />"
					             + " - Contract Code : <b>" + transRef + "</b><br />"
					             + " - Card Holder : <b>" + trans.CardHolder + "</b><br />"
					             + " - Card Number : <b>" + trans.CardNumber + "</b><br />"
					             + " - Currency : <b>" + trans.Currency + "</b><br />"
					             + " - Amount : <b>" + trans.Amount + "</b></p>"
					             + "<p>Payment Gateway Response<br />"
					             + " - id : <b>" + peachID + "</b><br />"
					             + " - resourcePath : <b>" + peachURL + "</b><br />"
					             + " - resultCode : <b>" + trans.ResultCode + "</b><br />"
					             + " - resultMessage : <b>" + trans.ResultMessage + "</b></p>";
					lblErr.Text      = lblData.Text;
					pnlOK.Visible    = ( ret == 0 );
					pnlError.Visible = ( ret != 0 );
					sql              = sql + Tools.DBString(trans.ResultCode);
				}

			using (MiscList mList = new MiscList())
				mList.ExecQuery(sql,0,"",false,true);
		}
	}
}