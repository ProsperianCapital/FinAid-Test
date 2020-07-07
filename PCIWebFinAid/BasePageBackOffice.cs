// Developed by Paul Kilfoil
// www.PaulKilfoil.co.za

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

// Error codes 19000-19099

namespace PCIWebFinAid
{
	public abstract class BasePageBackOffice : BasePage
	{
		protected SessionGeneral sessionGeneral;
		protected string         sql;

		protected void SessionSave(string applicationCode=null,string userCode=null,string userName=null,string accessType=null)
		{
			if ( sessionGeneral  == null )
				sessionGeneral     = new SessionGeneral();
			if ( applicationCode != null )
				sessionGeneral.ApplicationCode = Tools.NullToString(applicationCode);
			if ( userCode        != null )
				sessionGeneral.UserCode        = Tools.NullToString(userCode);
			if ( userName        != null )
				sessionGeneral.UserName        = Tools.NullToString(userName);
			if ( accessType      != null )
				sessionGeneral.AccessType      = Tools.NullToString(accessType);
			Session["SessionGeneral"]         = sessionGeneral;
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

//		protected byte SecurityCheck(byte mode=0)
//		{
//			if ( mode == 19 && ! sessionGeneral.AdminUser )
//			{
//				StartOver(19);
//				return 19;
//			}
//			return 0;
//		}

		protected byte SessionCheck(byte sessionMode=43,string applicationCode="")
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
				SessionSave(applicationCode);
				return 0;
			}

			if ( sessionMode == 99 && sessionGeneral == null )
			{
				sessionGeneral                 = new SessionGeneral();
				sessionGeneral.UserCode        = "653";
				sessionGeneral.UserName        = "Samual Briggs";
				sessionGeneral.AccessType      = "P";
				sessionGeneral.ApplicationCode = "001";
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
			if ( applicationCode.Length > 0 )
				SessionSave(applicationCode);
			else
				SessionSave();
			return 0;
		}

		protected byte PageCheck()
		{
		//	Check whether user has access to this page here
			return 0;
		}

		protected void ShowUserDetails()
		{
			try
			{
				XHeader head = (XHeader)FindControl("ascxXHeader");
				if ( head   != null )
					head.ShowUser(sessionGeneral);
			}
			catch
			{ }
		}
	}
}
