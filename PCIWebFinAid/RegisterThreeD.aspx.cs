using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
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

			if ( transRef.Length < 1 || peachID.Length < 1 || peachURL.Length < 1 )
			{
				pnlError.Visible = true;
				lblErr.Text      = "The data returned from 3d Secure processing is invalid/missing";
				return;
			}

			using (TransactionPeach trans = new TransactionPeach())
			{
				int    ret   = trans.ThreeDSecureCheck(peachID);
				lblData.Text = "<p>Transaction<br />"
				             + " - Contract Id : " + transRef + "<br />"
				             + " - Card Holder : " + trans.CardHolder + "<br />"
				             + " - Card Number : " + trans.CardNumber + "<br />"
				             + " - Currency : " + trans.Currency + "<br />"
				             + " - Amount : " + trans.Amount + "</p>"
				             + "<p>Payment Gateway Response<br />"
				             + " - id : " + peachID + "<br />"
				             + " - resourcePath : " + peachURL + "<br />"
				             + " - resultCode : " + trans.ResultCode + "<br />"
				             + " - resultMessage : " + trans.ResultMessage + "</p>";
				lblErr.Text      = lblData.Text;
				pnlOK.Visible    = ( ret == 0 );
				pnlError.Visible = ( ret != 0 );
			}
		}
	}
}