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
			pnlError.Visible = false;
			pnlOK.Visible    = false;

			if ( transRef.Length < 1 || peachID.Length < 1 ) // || peachURL.Length < 1 )
			{
				pnlError.Visible = true;
				lblErr.Text      = "The data returned from 3d Secure processing is invalid/missing";
				return;
			}

//			Get MID, Key and URL here
//			Populate fields in object "trans"

			using (TransactionPeach trans = new TransactionPeach())
			{
				int    ret   = trans.ThreeDSecureCheck(peachID);
				lblData.Text = "<p>Transaction<br />"
				             + " - Contract Id : <b>" + transRef + "</b><br />"
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
			}
		}
	}
}