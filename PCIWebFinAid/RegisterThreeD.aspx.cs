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
			string sql       = "exec sp_WP_PaymentRegister3DSec @ContractCode="    + Tools.DBString(transRef)
				                                             + ",@ReferenceNumber=" + Tools.DBString(peachID)
				                                             + ",@Status=";

			using (TransactionPeach trans = new TransactionPeach())
			{
				int ret = trans.ThreeDSecureCheck(peachID);
				sql     = sql + Tools.DBString(trans.ResultCode);
			}

			using (MiscList mList = new MiscList())
				mList.ExecQuery(sql,0,"",false,true);
		}
	}
}