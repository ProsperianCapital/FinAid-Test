using System;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class RegisterThreeD : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			int    ret;
			string peachCode = "XXX-XXX-XXX";
			string transRef  = WebTools.RequestValueString(Request,"TransRef");
			string peachID   = WebTools.RequestValueString(Request,"id");
			string peachURL  = WebTools.RequestValueString(Request,"resourcePath");
			string errorCode = WebTools.RequestValueString(Request,"ErrorCode");
			string sql       = "exec sp_WP_PaymentRegister3DSecA @ContractCode="    + Tools.DBString(transRef)
				                                              + ",@ReferenceNumber=" + Tools.DBString(peachID)
				                                              + ",@Status=";

			try
			{
				using (TransactionPeach trans = new TransactionPeach())
				{
					ret       = trans.ThreeDSecureCheck(peachID);
					peachCode = trans.ResultCode;
					sql       = sql + Tools.DBString(peachCode);
				}

				Tools.LogInfo("RegisterThreeD.PageLoad/5",sql,222);

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

						Tools.LogInfo("RegisterThreeD.PageLoad/10",sql,222);

						ret = mList.ExecQuery(sql,0);
						if ( ret == 0 && ! mList.EOF )
							while ( ! mList.EOF )
							{
								fieldCode  = mList.GetColumn("WebsiteFieldCode");
								fieldValue = mList.GetColumn("WebsiteFieldValue");
								Tools.LogInfo("RegisterThreeD.PageLoad/15","WebsiteFieldCode="+fieldCode+" ("+fieldValue+")",222);
								ctlLabel  = (Literal)FindControl("lbl"+fieldCode);
								if ( ctlLabel   != null )
									ctlLabel.Text = fieldValue.Replace(Environment.NewLine,"<br />");
								mList.NextRow();
							}
						else
						{
							lbl100503.Text = "Oops ...";
							lbl100504.Text = "Something seems to have gone wrong with your payment.<br /><br />We have logged the error and will investigate";
							sql            =   "Contract Code="     + transRef
							               + ", Peach Id="          + peachID
							               + ", Peach Result Code=" + peachCode
							               + ", Ret="               + ret.ToString()
							               + ", SQL="               + sql;
							Tools.LogException("RegisterThreeD.PageLoad/20",sql);
							Tools.LogInfo     ("RegisterThreeD.PageLoad/25",sql,222);
						}
					}
					else
					{
						Tools.LogException("RegisterThreeD.PageLoad/30","Ret="+ret.ToString()+" ("+sql+")");
						Tools.LogInfo     ("RegisterThreeD.PageLoad/35","Ret="+ret.ToString()+" ("+sql+")",222);
					}
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("RegisterThreeD.PageLoad/90","",ex);
				Tools.LogInfo     ("RegisterThreeD.PageLoad/95",ex.Message,222);
			}
		}
	}
}