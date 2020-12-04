// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public abstract class BasePageCRM : BasePageLogin
	{
		protected string sqlProc;

		protected override void StartOver(int errNo,string pageName="")
		{
			base.StartOver ( errNo, ( pageName.Length > 0 ? pageName : "pgLogonCRM.aspx" ) );
		}

		protected abstract void LoadPageData();

		protected void UpdatePageData(string module)
		{
			using (MiscList mList = new MiscList())
			{
				mList.UpdateQuery(sql);
				string msg = mList.ReturnMessage;
				if ( mList.ReturnCode == 0 )
					LoadPageData();
				else if ( msg.Length > 0 )
					msg = "[" + mList.ReturnCode.ToString() + "] " + msg;
				else
					msg = "[" + mList.ReturnCode.ToString() + "] Update failed (" + sqlProc + ")";
				SetErrorDetail(module+".UpdatePageData",99010,msg,"",102,1);
			}
		}

//		protected void UpdatePageData(string module)
//		{
//			using (MiscList mList = new MiscList())
//			{
//				if ( mList.ExecQuery(sql,0,"",false) != 0 )
//					SetErrorDetail(module,99010,"Internal database error (" + sqlProc + ")",sql,102,1);
//				else if ( mList.EOF )
//					SetErrorDetail(module,99020,"No data returned (" + sqlProc + ")",sql,102,1);
//				else
//				{
//					int    errCode = mList.GetColumnInt("ResultCode");
//					string errMsg  = mList.GetColumn   ("ResultMessage");
//					if ( errCode > 0 && errMsg.Length > 0 )
//						errMsg = "[" + errCode.ToString() + "] " + errMsg;
//					else if ( errCode > 0 )
//						errMsg = "[" + errCode.ToString() + "] Update failed (" + sqlProc + ")";
//					else
//						LoadPageData();
//					SetErrorDetail(module,99030,errMsg,"",102,1);
//				}
//			}
//		}

		protected int LoadLabelText(Control subCtl)
		{
			if ( sessionGeneral == null )
				return 10010;

			int    ret = 10020;
			string fieldCode;
			string fieldValue;

			using (MiscList mList = new MiscList())
				try	
				{
					sql = "exec sp_WP_Get_ProductWebsiteCRMContent @ProductCode=" + Tools.DBString(sessionGeneral.ProductCode)
					                                           + ",@LanguageCode=" + Tools.DBString(sessionGeneral.LanguageCode)
					                                           + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode);
					if ( mList.ExecQuery(sql, 0) != 0 )
						SetErrorDetail("LoadLabelText", 10010, "Internal database error (sp_WP_Get_ProductWebsiteCRMContent failed)", sql, 1, 1);
					else if ( mList.EOF )
						SetErrorDetail("LoadLabelText", 10020, "Internal database error (sp_WP_Get_ProductWebsiteCRMContent no data returned)", sql, 1, 1);
					else
						while ( ! mList.EOF )
						{
							ret        = 10050;
							fieldCode  = mList.GetColumn("WebsiteFieldCode");
							fieldValue = mList.GetColumn("WebsiteFieldValue").Replace(Environment.NewLine,"<br />");
							ret        = 10060;
							ReplaceControlText("X"+fieldCode,fieldValue,subCtl);
							ReplaceControlText("Y"+fieldCode,fieldValue);
							ReplaceControlText("Z"+fieldCode,fieldValue);
							mList.NextRow();
						}
				}
				catch (Exception ex)
				{
					SetErrorDetail("LoadLabelText", ret, "Internal error (sp_WP_Get_ProductWebsiteCRMContent)", "", 2, 2, ex);
					return ret;
				}

			return 0;
		}

		private void ReplaceControlText(string ctlID,string fieldValue,Control subControl=null)
		{
			Control ctl = FindControl(ctlID);
			if ( ctl == null && subControl != null )
				ctl    = subControl.FindControl(ctlID);
			if ( ctl == null )
				return;
			else if (ctl.GetType()    == typeof(Literal))
				((Literal)ctl).Text     = fieldValue;
			else if (ctl.GetType()    == typeof(Label))
				((Label)ctl).Text       = fieldValue;
			else if (ctl.GetType()    == typeof(TableCell))
				((TableCell)ctl).Text   = fieldValue;
			else if (ctl.GetType()    == typeof(Button))
				((Button)ctl).Text      = fieldValue;
			else if (ctl.GetType()    == typeof(CheckBox))
				((CheckBox)ctl).Text    = fieldValue;
			else if (ctl.GetType()    == typeof(RadioButton))
				((RadioButton)ctl).Text = fieldValue;
			else if (ctl.GetType()    == typeof(HyperLink))
				((HyperLink)ctl).Text   = fieldValue;
			else
				SetErrorDetail("ReplaceControlText", 10030, "Unrecognized HTML control (" + ctlID.ToString() + "/" + fieldValue.ToString() + ")",ctlID.ToString() + ", control type="+ctl.GetType().ToString());
		}
	}
}
