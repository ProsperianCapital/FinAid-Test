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
		protected Literal        lblJS;
		protected Label          lblError;
		protected Button         btnErrorDtl;
		protected Label          lblErrorDtl;
		protected string         sql;

		protected void SessionSave(string userCode=null,string userName=null,string accessType=null,string contractCode=null)
		{
			if ( sessionGeneral == null )
				sessionGeneral  = new SessionGeneral();
			if ( userCode     != null )
				sessionGeneral.UserCode     = Tools.NullToString(userCode);
			if ( userName     != null )
				sessionGeneral.UserName     = Tools.NullToString(userName);
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
			}

			else if ( sessionMode == 4 && ( sessionGeneral == null || sessionGeneral.UserCode.Length < 1 ) )
			{
				string backDoor = WebTools.RequestValueString(Request,"BackDoor");
				if ( backDoor.Length < 1 && Session["BackDoor"] != null )
					backDoor = Session["BackDoor"].ToString();
				if ( backDoor != "9999" )
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


		protected void SetErrorDetail(string method,int errCode,string errBrief="",string errDetail="",byte briefMode=2,byte detailMode=2,Exception ex=null,bool alwaysShow=false)
		{
			if ( errCode == 0 )
				return;

			if ( errCode <  0 )
			{
				if ( lblError != null )
				{
					lblError.Text    = "";
					lblError.Visible = false;
				}
				if ( lblErrorDtl != null ) lblErrorDtl.Text    = "";
				if ( btnErrorDtl != null ) btnErrorDtl.Visible = false;
				return;
			}

			string pageName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath);
			if ( Tools.NullToString(pageName).Length < 1 )
				pageName = "BasePageBackOffice";
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
			else if ( briefMode == 23 ) // Use "lblErr2", <p></p>
				try
				{
					((Label)FindControl("lblErr2")).Text = "<p>" + errBrief + "</p>";
				}
				catch {}
			else if ( briefMode == 33 ) // Use "lblErr3", <br />
				try
				{
					((Label)FindControl("lblErr3")).Text = "<br />" + errBrief;
				}
				catch {}
			else
				lblError.Text = errBrief;
			lblError.Visible = ( lblError.Text.Length > 0 );

			if ( lblErrorDtl == null )
				return;

			if ( errDetail.Length < 1 )
				errDetail = errBrief;
			errDetail = "[" + errCode.ToString() + "] " + errDetail;
			errDetail = errDetail.Replace(",","<br />,").Replace(";","<br />;").Trim();
			if ( detailMode == 2 ) // Append
				errDetail = lblErrorDtl.Text + ( lblErrorDtl.Text.Length > 0 ? "<br /><br />" : "" ) + errDetail;
			lblErrorDtl.Text = errDetail;
			if ( ! lblErrorDtl.Text.StartsWith("<div") )
				lblErrorDtl.Text = "<div style='background-color:blue;padding:3px;color:white;height:20px'>Error Details<img src='Images/Close1.png' title='Close' style='float:right' onclick=\"JavaScript:ShowElt('lblErrorDtl',false)\" /></div>" + lblErrorDtl.Text;

			btnErrorDtl.Visible = ( lblErrorDtl.Text.Length > 0 ) && ( ! Tools.SystemIsLive() || alwaysShow );
		}
	}
}
