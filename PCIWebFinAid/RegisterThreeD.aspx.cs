using System;
using System.Web.UI.WebControls;
using System.IO;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class RegisterThreeD : BasePage
	{
		private string providerRef;
		private string providerCode;
		private string transRef;
		private string resultCode;
		private string resultMsg;
		private int    sqlRet;
		private int    ret;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ret          = 10;
			providerRef  = "";
			providerCode = WebTools.RequestValueString(Request,"ProviderCode");
			transRef     = WebTools.RequestValueString(Request,"TransRef");
			resultCode   = WebTools.RequestValueString(Request,"ResultCode");
			resultMsg    = WebTools.RequestValueString(Request,"ResultMessage");

//	Testing ...
//
//			try
//			{
//				using (StreamReader reader = new StreamReader(Request.InputStream))
//				{
//					ret           = 20;
//					string webStr = reader.ReadToEnd();
//					Tools.LogInfo("PageLoad/14",webStr,222,this);
//				}
//			}
//			catch
//			{ }

			try
			{
				if ( resultCode.Length > 0 || resultMsg.Length > 0 || transRef.Length < 1 )
				{
					ret = 30;
					SetMessage("Error ...","Your payment failed.");
//					Tools.LogException("PageLoad/20",sql,this);
//					Tools.LogInfo     ("PageLoad/13",sql,222,this);
					return;
				}

				ret               = 40;
				Transaction trans = null;
				string      token = "";
				string      sql   = "exec sp_WP_PaymentRegister3DSecA @ContractCode=" + Tools.DBString(transRef);

				if ( providerCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) )
				{
					Tools.LogInfo("PageLoad/16",Request.Url.AbsoluteUri,222,this);
					ret         = 50;
					trans       = new TransactionCyberSource();
					resultCode  = WebTools.RequestValueString(Request,"auth_response");
					providerRef = WebTools.RequestValueString(Request,"transaction_id");
					token       = WebTools.RequestValueString(Request,"payment_token");
					resultMsg   = WebTools.RequestValueString(Request,"decision");
					string xCd  = WebTools.RequestValueString(Request,"decision_return_code");
					string xMsg = WebTools.RequestValueString(Request,"message");
					if ( resultMsg.Length < 1 ) resultMsg = "FAIL";
					if ( xCd.Length       > 0 ) resultMsg = resultMsg + "/" + xCd;
					if ( xMsg.Length      > 0 ) resultMsg = resultMsg + " (" + xMsg + ")";

					if ( resultCode.Length > 0 && resultCode != "0" && resultCode != "00" && resultCode != "000" && resultCode != "0000" )
						SetMessage("Error ...","Your payment was rejected.");
				}
				else
				{
					ret         = 80;
					trans       = new TransactionPeach();
				//	resultCode  = "XXX-XXX-XXX";
				//	providerURL = WebTools.RequestValueString(Request,"resourcePath");
					providerRef = WebTools.RequestValueString(Request,"id");
					sqlRet      = trans.ThreeDSecureCheck(providerRef);
					resultCode  = trans.ResultCode;
				}

				ret   = 100;
				trans = null;
				sql   = sql + ",@ReferenceNumber="    + Tools.DBString(providerRef)
				            + ",@Status="             + Tools.DBString(resultCode)
				            + ",@PaymentBureauCode="  + Tools.DBString(providerCode)
				            + ",@PaymentBureauToken=" + Tools.DBString(token);

//				using (TransactionPeach trans = new TransactionPeach())
//				{
//					sqlRet    = trans.ThreeDSecureCheck(peachID);
//					peachCode = trans.ResultCode;
//					sql       = sql + Tools.DBString(peachCode);
//				}

//				Tools.LogInfo("RegisterThreeD.PageLoad/5",sql,222);

				using (MiscList mList = new MiscList())
				{
					ret    = 120;
//	Single language
//					sqlRet = mList.ExecQuery(sql,0,"",false,true);

//	Multi language
					sqlRet = mList.ExecQuery(sql,0);
					Tools.LogInfo("PageLoad/45",sql+" (sqlRet="+sqlRet.ToString()+")",222,this);

					if ( sqlRet == 0 )
					{
						ret = 0;

	//					ret         = 130;
	//					string  pc  = mList.GetColumn("ProductCode");
	//					string  lc  = mList.GetColumn("LanguageCode");
	//					string  ldc = mList.GetColumn("LanguageDialectCode");
	//					string  fieldCode;
	//					string  fieldValue;
	//					Literal ctlLabel;
	//					sql = "exec sp_WP_Get_ProductWebsiteRegContent"
	//					    +     " @ProductCode="         + Tools.DBString(pc)
	//						 +     ",@LanguageCode="        + Tools.DBString(lc)
	//						 +     ",@LanguageDialectCode=" + Tools.DBString(ldc)
	//						 +     ",@Page='S'";
	//
//	//					Tools.LogInfo("RegisterThreeD.PageLoad/10",sql,222);
	//
	//					ret    = 150;
	//					sqlRet = mList.ExecQuery(sql,0);
	//					Tools.LogInfo("PageLoad/55",sql+" (sqlRet="+sqlRet.ToString()+")",222,this);
	//
	//					if ( sqlRet == 0 && ! mList.EOF )
	//						while ( ! mList.EOF )
	//						{
	//							ret        = 0; // All OK
	//							fieldCode  = mList.GetColumn("WebsiteFieldCode");
	//							fieldValue = mList.GetColumn("WebsiteFieldValue");
	//							ctlLabel  = (Literal)FindControl("lbl"+fieldCode);
	//							if ( ctlLabel   != null )
	//								ctlLabel.Text = fieldValue.Replace(Environment.NewLine,"<br />");
	//							mList.NextRow();
	//						}
	//					else
	//					{
	//						ret = 190;
	//						sql =   "Contract Code="           + transRef
	//						    + ", Transaction Id="          + providerRef
	//						    + ", Transaction Result Code=" + resultCode
	//						    + ", sqlRet="                  + sqlRet.ToString()
	//						    + ", Ret="                     + ret.ToString()
	//						    + ", SQL="                     + sql;
//	//						Tools.LogException("PageLoad/20",sql,this);
	//						Tools.LogInfo     ("PageLoad/65",sql,222,this);
	//					}

					}
					else
					{
						ret = 210;
						Tools.LogException("PageLoad/70","sqlRet="+sqlRet.ToString()+" ("+sql+")",null,this);
						Tools.LogInfo     ("PageLoad/75","sqlRet="+sqlRet.ToString()+" ("+sql+")",222,this);
					}
				}
				if ( ret == 0 )
					SetMessage("Thank You ...","Your application has been successfully received.",false);
				else
					SetMessage("Oops ...","Something seems to have gone wrong with your payment.<br /><br />We have logged the error and will investigate.",false);
			}
			catch (Exception ex)
			{
				SetMessage("Oops ...","Something seems to have gone wrong.");
				Tools.LogException("PageLoad/90","",ex,this);
				Tools.LogInfo     ("PageLoad/95",ex.Message,222,this);
			}
		}

		private void SetMessage(string head1,string head2,bool overWrite=true)
		{
			if ( overWrite || lbl100503.Text.Length < 1 )
				lbl100503.Text = head1;
			if ( overWrite || lbl100504.Text.Length < 1 )
				lbl100504.Text = head2 + ( head2.Length > 0 ? "<br /><br />" : "" )
					            + "<table style='white-space:nowrap'>"
								   + "<tr><td><b>Transaction Result Code</b></td><td> : "        + resultCode     + "</td></tr>"
								   + "<tr><td><b>Transaction Message</b></td><td> : "            + resultMsg      + "</td></tr>"
								   + "<tr><td><b>Transaction Id</b></td><td> : "                 + providerRef    + "</td></tr>"
								   + "<tr><td><b>Contract/Transaction Reference</b></td><td> : " + transRef       + "</td></tr>"
								   + "<tr><td><b>Internal code (ret)</b></td><td> : "            + ret.ToString() + "</td></tr>"
				               + "</table>";
		}
	}
}