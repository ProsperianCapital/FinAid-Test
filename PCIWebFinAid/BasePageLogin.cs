// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

// Error codes 10000-10099

namespace PCIWebFinAid
{
	public abstract class BasePageLogin : BasePage
	{
		protected SessionGeneral sessionGeneral;
		protected HiddenField    hdnTabNo;
		protected int            tabNo;
		protected int            maxTab;
		protected string         sql;

//		protected Button         btnErrorDtl;
//		protected Label          lblErrorDtl;
//		protected Label          lblError;

		protected void SessionSave(string userCode=null,string userName=null,string accessType=null,string contractCode=null)
		{
			if ( sessionGeneral  == null )
				sessionGeneral     = new SessionGeneral();
			if ( userCode        != null )
				sessionGeneral.UserCode        = Tools.NullToString(userCode);
			if ( userName        != null )
				sessionGeneral.UserName        = Tools.NullToString(userName);
			if ( accessType      != null )
				sessionGeneral.AccessType      = Tools.NullToString(accessType);
			if ( contractCode    != null )
				sessionGeneral.ContractCode    = Tools.NullToString(contractCode);
			Session["SessionGeneral"]         = sessionGeneral;
		}

		protected void SessionClearLogin()
		{
			sessionGeneral            = null;
			Session["SessionGeneral"] = null;
			Session["BackDoor"]       = null;
			SessionClearData();
		}

		protected void SessionClearData()
		{
			Session["LFinnHubData"] = null;
		}

//		protected byte SecurityCheck(byte mode=0)
//		{
//			if ( mode == 19 && ! sessionGeneral.AdminUser )
//			{
//				StartOver(19);
//				return 19;
//			}
//			return 0;
//		}

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
// 19 : Required, back office (admin)
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
				sessionGeneral            = new SessionGeneral();
				sessionGeneral.UserCode   = "653";
				sessionGeneral.UserName   = "Samual Briggs";
				sessionGeneral.AccessType = "P";
				ApplicationCode           = "002";
			}

			else if ( sessionMode == 4 && ( sessionGeneral == null || sessionGeneral.UserCode.Length < 1 ) )
			{
				string backDoor = WebTools.RequestValueString(Request,"BackDoor");
				if ( backDoor.Length < 1 && Session["BackDoor"] != null )
					backDoor = Session["BackDoor"].ToString();
				if ( backDoor != ((int)PCIBusiness.Constants.SystemPassword.BackDoor).ToString() )
				{
					StartOver(10);
					return 10;
				}
			}

			else if ( sessionMode == 19 && sessionGeneral != null && ! sessionGeneral.AdminUser )
			{
				StartOver(20);
				return 20;
			}

			else if ( sessionMode != 5 && sessionGeneral == null )
			{
				StartOver(30);
				return 30;
			}

			else if ( sessionMode != 5 && sessionGeneral != null && sessionGeneral.AccessType == "X" )
			{
				StartOver(40);
				return 40;
			}

			ShowUserDetails();
			SessionSave();
			return 0;
		}

		protected override void StartOver(int errNo,string pageName="")
		{
			if ( pageName.Length < 6 )
				pageName = "Login.aspx";
			Response.Redirect ( pageName + ( errNo > 0 ? "?ErrNo=" + errNo.ToString() : "" ) , true );
		}

		protected byte PageCheck()
		{
			if ( maxTab > 0 )
			{
				tabNo = PCIBusiness.Tools.StringToInt(hdnTabNo.Value);
				if ( tabNo < 1 || tabNo > maxTab )
					tabNo = 1;
				try
				{
					Footer foot = (Footer)FindControl("ascxFooter");
					foot.JSText = WebTools.JavaScriptSource("SetTab("+tabNo.ToString()+","+maxTab.ToString()+")");
				}
				catch
				{
					if ( FindControl("lblJS") != null )
						((Literal)FindControl("lblJS")).Text = WebTools.JavaScriptSource("SetTab("+tabNo.ToString()+","+maxTab.ToString()+")");
				}
			}
			return 0;
		}

		protected void ShowUserDetails()
		{
			try
			{
				XHeader head1 = (XHeader)FindControl("ascxXHeader");
				if ( head1   != null )
					head1.ShowUser(sessionGeneral);
				Header  head2 = (Header) FindControl("ascxHeader");
				if ( head2   != null )
					head2.ShowUser(sessionGeneral);
			}
			catch
			{ }
		}


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
					sql = "exec sp_WP_Get_ProductWebsiteRegContent @ProductCode=" + Tools.DBString(sessionGeneral.ProductCode)
					                                           + ",@LanguageCode=" + Tools.DBString(sessionGeneral.LanguageCode)
					                                           + ",@LanguageDialectCode=" + Tools.DBString(sessionGeneral.LanguageDialectCode)
					                                           + ",@Page='L'";
					if ( mList.ExecQuery(sql, 0) != 0 )
						SetErrorDetail("LoadLabelText", 10010, "Internal database error (sp_WP_Get_ProductWebsiteRegContent failed)", sql, 1, 1);
					else if ( mList.EOF )
						SetErrorDetail("LoadLabelText", 10020, "Internal database error (sp_WP_Get_ProductWebsiteRegContent no data returned)", sql, 1, 1);
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
				SetErrorDetail("ReplaceControlText", 10030, "Unrecognized HTML control (" + ctlID.ToString() + "/" + fieldValue.ToString() + ")",ctlID.ToString() + ", control type="+ctl.GetType().ToString());
		}
	}
}
