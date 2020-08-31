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

			using (TransactionPeach trans = new TransactionPeach())
			{
				ret       = trans.ThreeDSecureCheck(peachID);
				peachCode = Tools.DBString(trans.ResultCode);
				sql       = sql + Tools.DBString(peachCode);
			}

			using (MiscList mList = new MiscList())
			{
//	Single language
//				ret = mList.ExecQuery(sql,0,"",false,true);

//	Multi language
				if ( mList.ExecQuery(sql,0) == 0 )
				{
					string  fieldCode;
					Literal ctlLabel;
					sql = "exec sp_WP_Get_ProductWebsiteRegContent"
					    +     " @ProductCode="         + Tools.DBString(mList.GetColumn("ProductCode"))
						 +     ",@LanguageCode="        + Tools.DBString(mList.GetColumn("LanguageCode"))
						 +     ",@LanguageDialectCode=" + Tools.DBString(mList.GetColumn("LanguageDialectCode"))
						 +     ",@Page='S'";
					ret = mList.ExecQuery(sql,0);
					if ( ret == 0 && ! mList.EOF )
						while ( ! mList.EOF )
						{
							fieldCode = mList.GetColumn("WebsiteFieldCode");
							ctlLabel  = (Literal)FindControl("lbl"+fieldCode);
							if ( ctlLabel   != null )
								ctlLabel.Text = mList.GetColumn("WebsiteFieldValue").Replace(Environment.NewLine,"<br />");
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
						Tools.LogException("RegisterThreeD.PageLoad/4",sql);
						Tools.LogInfo     ("RegisterThreeD.PageLoad/5",sql,222);
					}
				}
			}
		}
	}
}