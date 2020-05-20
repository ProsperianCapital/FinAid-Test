using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

// Developed by Paul Kilfoil
// http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public abstract class BasePageLogin : BasePage
	{
		protected SessionGeneral sessionGeneral;
		protected Literal        lblJS;
		protected Button         btnError;
		protected Label          lblError;
		protected Label          lblErrorDtl;
		protected HiddenField    hdnTabNo;
		protected int            tabNo;
		protected int            maxTab;
		protected string         sql;

		protected void SessionSave(string clientCode=null,string contractCode=null,string accessType=null)
		{
			if ( sessionGeneral == null )
				sessionGeneral  = new SessionGeneral();
			if ( clientCode   != null )
				sessionGeneral.ClientCode   = Tools.NullToString(clientCode);
			if ( contractCode != null )
				sessionGeneral.ContractCode = Tools.NullToString(contractCode);
			if ( accessType   != null )
				sessionGeneral.AccessType   = Tools.NullToString(accessType);
			Session["SessionGeneral"]      = sessionGeneral;
		}

		protected void SessionClearLogin()
		{
			sessionGeneral            = null;
			Session["SessionGeneral"] = null;
			SessionClearData();
		}

		protected void SessionClearData()
		{
			Session["LFinnHubData"] = null;
		}

		protected byte SecurityCheck(byte mode=0)
		{
			if ( mode == 19 && sessionGeneral.AccessType != "A" )
			{
				StartOver(19);
				return 19;
			}
			return 0;
		}

		protected byte SessionCheck(byte sessionMode=43)
		{
			Response.Cache.SetExpires(DateTime.Now);
			Response.Cache.SetCacheability(HttpCacheability.NoCache);
			Response.Cache.SetValidUntilExpires(false);
			SetErrorDetail(null,-888);

//	sessionMode
//  1 : Not needed
//  2 : Clear/delete, leave NULL
//  3 : Clear/delete then recreate
//  4 : Not needed, access via back door, but set up session if it exists
//  5 : Not needed, but set up session if it exists
// 43 : Required
// 99 : Set up test session (no login)

//	Not needed, now done in web.config
//			if ( ssl > 0 )
//				if ( CheckSSL() > 0 ) // Means it is redirecting
//					return 0;

			if ( sessionMode == 1 )
				return 0;

			if ( sessionMode == 2 )
			{
				SessionClearLogin();
				return 0;
			}

			try
			{
				sessionGeneral = (SessionGeneral)Session["SessionGeneral"];
			}
			catch
			{
				sessionGeneral = null;
			}

			if ( sessionMode == 3 )
			{
				if ( sessionGeneral == null )
					sessionGeneral = new SessionGeneral();
				else
					sessionGeneral.Clear();
				SessionSave();
				return 0;
			}

			if ( sessionMode == 99 && sessionGeneral == null )
			{
				sessionGeneral              = new SessionGeneral();
				sessionGeneral.ClientCode   = "199383";
				sessionGeneral.ContractCode = "AZ-876342982/144";
				sessionGeneral.AccessType   = "N";
			}
			else if ( sessionGeneral == null || sessionGeneral.ClientCode.Length < 1 )
				if ( sessionMode == 4 )
				{
					string backDoor = WebTools.RequestValueString(Request,"BackDoor");
					if ( backDoor.Length < 1 && Session["BackDoor"] != null )
						backDoor = Session["BackDoor"].ToString();
					if ( backDoor != "9999" )
					{
						StartOver(10);
						return 10;
					}
//					if ( backDoor.Length > 0 && PCIBusiness.Tools.CheckSystemPassword(PCIBusiness.Constants.PasswordType.BackDoor,backDoor) )
//					{
//						Session["BackDoor"] = backDoor;
//						return 0;
//					}
				}
				else if ( sessionMode != 5 )
				{
					StartOver(20);
					return 20;
				}

			ShowUserDetails();
			SessionSave();
			return 0;
		}

		protected byte PageCheck()
		{
			if ( maxTab > 0 )
			{
				tabNo = PCIBusiness.Tools.StringToInt(hdnTabNo.Value);
				if ( tabNo < 1 || tabNo > maxTab )
					tabNo = 1;
				lblJS.Text = WebTools.JavaScriptSource("SetTab("+tabNo.ToString()+","+maxTab.ToString()+")");
			}
			return 0;
		}

		protected void ShowUserDetails()
		{
			try
			{
			//	Banner banner = (Banner)FindControl("ascxBanner");
			//	banner.ShowUser(sessionGeneral);
			}
			catch
			{ }
		}


		protected int LoadLabelText(Control subCtl)
		{
			if ( sessionGeneral == null )
				return 10010;

			int     ret = 10020;
			string  fieldCode;
			string  fieldValue;
			Control ctl;

			using (MiscList mList = new MiscList())
				try	
				{
					sql = "exec sp_WP_Get_ProductWebsiteRegContent @ProductCode=" + Tools.DBString(sessionGeneral.ProductCode)
					                                           + ",@LanguageCode=" + Tools.DBString(sessionGeneral.LanguageCode)
					                                           + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
					                                           + ",@Page='L'";
					if ( mList.ExecQuery(sql, 0) != 0 )
						SetErrorDetail("LoadLabelText", 10030, "Internal database error (sp_WP_Get_ProductWebsiteRegContent failed)", sql, 1, 1);
					else if ( mList.EOF )
						SetErrorDetail("LoadLabelText", 10040, "Internal database error (sp_WP_Get_ProductWebsiteRegContent no data returned)", sql, 1, 1);
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
					SetErrorDetail("LoadLabelText", ret, "Internal error (sp_WP_Get_ProductWebsiteRegContent)", "", 2, 2, ex);
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
			else if (ctl.GetType()  == typeof(Literal))
				((Literal)ctl).Text   = fieldValue;
			else if (ctl.GetType()  == typeof(Label))
				((Label)ctl).Text     = fieldValue;
			else if (ctl.GetType()  == typeof(TableCell))
				((TableCell)ctl).Text = fieldValue;
			else if (ctl.GetType()  == typeof(Button))
				((Button)ctl).Text    = fieldValue;
			else if (ctl.GetType()  == typeof(CheckBox))
				((CheckBox)ctl).Text  = fieldValue;
			else
				SetErrorDetail("ReplaceControlText", 90010, "Unrecognized HTML control (" + ctlID.ToString() + "/" + fieldValue.ToString() + ")",ctlID.ToString() + ", control type="+ctl.GetType().ToString());
		}

		protected void SetErrorDetail(string method,int errCode,string errBrief="",string errDetail="",byte briefMode=2,byte detailMode=2,Exception ex=null)
		{
			if ( errCode == 0 )
				return;

			if ( errCode <  0 )
			{
				lblError.Text    = "";
				lblErrorDtl.Text = "";
				lblError.Visible = false;
				btnError.Visible = false;
				return;
			}

			string pageName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
			if ( Tools.NullToString(pageName).Length < 1 )
				pageName = "BasePageLogin";
			method      = ( Tools.NullToString(method).Length > 0 ? method+"[SetErrorDetail]." : "SetErrorDetail/" ) + errCode.ToString();
			Tools.LogInfo(pageName+"."+method,errBrief+" ("+errDetail+")",10);

			if ( ex != null )
			{
				Tools.LogException(pageName+"."+method,errBrief,ex);
				if ( Tools.NullToString(errDetail).Length < 1 )
					errDetail = ex.Message;
			}

			if ( briefMode == 2 ) // Append
				lblError.Text = lblError.Text + ( lblError.Text.Length > 0 ? "<br />" : "" ) + errBrief;
			else
				lblError.Text = errBrief;

			if ( errDetail.Length < 1 )
				errDetail = errBrief;
			errDetail = "[" + errCode.ToString() + "] " + errDetail;

			errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
			if ( detailMode == 2 ) // Append
				errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
			lblErrorDtl.Text = errDetail;
			if ( ! lblErrorDtl.Text.StartsWith("<div") )
				lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white;height:20px'>Error Details<img src='Images/Close1.png' title='Close' style='float:right' onclick=\"JavaScript:ShowElt('lblErrorDtl',false)\" /></div>" + lblErrorDtl.Text;

			lblError.Visible = ( lblError.Text.Length    > 0 );
			btnError.Visible = ( lblErrorDtl.Text.Length > 0 ) && ! PCIBusiness.Tools.SystemIsLive();
		}
	}
}
