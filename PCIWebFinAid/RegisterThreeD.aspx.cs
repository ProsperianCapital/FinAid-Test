using System;
using System.Web.UI.WebControls;
using System.IO;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class RegisterThreeD : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			int    ret;
			string providerRef;
//			string providerURL;
			string providerCode = WebTools.RequestValueString(Request,"ProviderCode");
			string transRef     = WebTools.RequestValueString(Request,"TransRef");
			string resultCode   = WebTools.RequestValueString(Request,"ResultCode");
			string resultMsg    = WebTools.RequestValueString(Request,"ResultMessage");
//			string errorCode    = WebTools.RequestValueString(Request,"ErrorCode");
			string sql          = "exec sp_WP_PaymentRegister3DSecA @ContractCode=" + Tools.DBString(transRef);

			try
			{
//				Stream webData = Request.InputStream;
				using (StreamReader reader = new StreamReader(Request.InputStream))
				{
					string webStr = reader.ReadToEnd();
					Tools.LogInfo("PageLoad/14",webStr,222,this);
				}
			}
			catch
			{ }

			try
			{
				if ( resultCode.Length > 0 || resultMsg.Length > 0 || transRef.Length < 1 )
				{
					lbl100503.Text = "Error ...";
					lbl100504.Text = "Your payment failed.<br /><br />"
					               + "<table style='white-space:nowrap'>"
					               + "<tr><td><b>Transaction Result Code</b></td><td> : " + resultCode + "</td></tr>"
					               + "<tr><td><b>Transaction Message</b></td><td> : " + resultMsg + "</td></tr>"
					               + "<tr><td><b>Transaction Reference</b></td><td> : " + transRef + "</td></tr></table>";
//					Tools.LogException("PageLoad/20",sql,this);
//					Tools.LogInfo     ("PageLoad/13",sql,222,this);
					return;
				}

				Transaction trans  = null;
				string      token  = "";

				if ( providerCode == Tools.BureauCode(Constants.PaymentProvider.CyberSource) )
				{
					trans       = new TransactionCyberSource();
					resultCode  = WebTools.RequestValueString(Request,"auth_response");
					providerRef = WebTools.RequestValueString(Request,"transaction_id");
					token       = WebTools.RequestValueString(Request,"payment_token");
				//	providerURL = WebTools.RequestValueString(Request,"X");
				}
				else
				{
					trans       = new TransactionPeach();
				//	resultCode  = "XXX-XXX-XXX";
				//	providerURL = WebTools.RequestValueString(Request,"resourcePath");
					providerRef = WebTools.RequestValueString(Request,"id");
					ret         = trans.ThreeDSecureCheck(providerRef);
					resultCode  = trans.ResultCode;
				}

				trans = null;
				sql   = sql + ",@ReferenceNumber="    + Tools.DBString(providerRef)
				            + ",@Status="             + Tools.DBString(resultCode)
				            + ",@PaymentBureauCode="  + Tools.DBString(providerCode)
				            + ",@PaymentBureauToken=" + Tools.DBString(token);

//				using (TransactionPeach trans = new TransactionPeach())
//				{
//					ret       = trans.ThreeDSecureCheck(peachID);
//					peachCode = trans.ResultCode;
//					sql       = sql + Tools.DBString(peachCode);
//				}

//				Tools.LogInfo("RegisterThreeD.PageLoad/5",sql,222);

				using (MiscList mList = new MiscList())
				{
//	Single language
//					ret = mList.ExecQuery(sql,0,"",false,true);

//	Multi language
					ret = mList.ExecQuery(sql,0);
					if ( ret == 0 )
					{
						string  pc  = mList.GetColumn("ProductCode");
						string  lc  = mList.GetColumn("LanguageCode");
						string  ldc = mList.GetColumn("LanguageDialectCode");
						string  fieldCode;
						string  fieldValue;
						Literal ctlLabel;
						sql = "exec sp_WP_Get_ProductWebsiteRegContent"
						    +     " @ProductCode="         + Tools.DBString(pc)
							 +     ",@LanguageCode="        + Tools.DBString(lc)
							 +     ",@LanguageDialectCode=" + Tools.DBString(ldc)
							 +     ",@Page='S'";

//						Tools.LogInfo("RegisterThreeD.PageLoad/10",sql,222);

						ret = mList.ExecQuery(sql,0);
						if ( ret == 0 && ! mList.EOF )
							while ( ! mList.EOF )
							{
								fieldCode  = mList.GetColumn("WebsiteFieldCode");
								fieldValue = mList.GetColumn("WebsiteFieldValue");
//								Tools.LogInfo("RegisterThreeD.PageLoad/15","WebsiteFieldCode="+fieldCode+" ("+fieldValue+")",222);
								ctlLabel  = (Literal)FindControl("lbl"+fieldCode);
								if ( ctlLabel   != null )
									ctlLabel.Text = fieldValue.Replace(Environment.NewLine,"<br />");
								mList.NextRow();
							}
						else
						{
							lbl100503.Text = "Oops ...";
							lbl100504.Text = "Something seems to have gone wrong with your payment.<br /><br />We have logged the error and will investigate";
							sql            =   "Contract Code="           + transRef
							               + ", Transaction Ref="         + providerRef
							               + ", Transaction Result Code=" + resultCode
							               + ", Ret="                     + ret.ToString()
							               + ", SQL="                     + sql;
//							Tools.LogException("PageLoad/20",sql,this);
							Tools.LogInfo     ("PageLoad/25",sql,222,this);
						}
					}
					else
					{
						Tools.LogException("PageLoad/30","Ret="+ret.ToString()+" ("+sql+")",null,this);
						Tools.LogInfo     ("PageLoad/35","Ret="+ret.ToString()+" ("+sql+")",222,this);
					}
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("PageLoad/90","",ex,this);
				Tools.LogInfo     ("PageLoad/95",ex.Message,222,this);
			}
		}
	}
}