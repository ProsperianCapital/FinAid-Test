using System;
using System.Text;
using System.Net;
using System.IO;
using PCIBusiness;

//	Developed by:
//		Paul Kilfoil
//		Software Development & IT Consulting
//		http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public partial class UIApplicationTest : BasePage
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( ! Page.IsPostBack )
			{
				string[] uName  = { "UserName"           , "SheilaColeman@getLost.net" };
				string[] uCode  = { "UserCode"           , "013" };
				string[] uPwd   = { "UserPassword"       , "hello4goodbye" };
				string[] lCode  = { "LanguageCode"       , "ENG" };
				string[] lDial  = { "LanguageDialectCode", "0002" };
				string[] coun   = { "CountryCode"        , "RSA" };
				string[] mobile = { "MobileNumber"       , "0842226666" };
				string[] query  = { "QueryName"          , "FinTechLogOn" };
				string[] key    = { "SecretKey"          , "7e6415a7cb790238fd12430a0ce419b3" };
				string[] app    = { "ApplicationCode"    , "003" };

				txtURL.Text  = TargetURL;
				lblVer.Text  = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";

				txtJSON.Text = Tools.JSONPair(uName [0],uName [1],1,"{") + Environment.NewLine
					          + Tools.JSONPair(uCode [0],uCode [1]      ) + Environment.NewLine
					          + Tools.JSONPair(uPwd  [0],uPwd  [1]      ) + Environment.NewLine
					          + Tools.JSONPair(lCode [0],lCode [1]      ) + Environment.NewLine
					          + Tools.JSONPair(lDial [0],lDial [1]      ) + Environment.NewLine
					          + Tools.JSONPair(coun  [0],coun  [1]      ) + Environment.NewLine
					          + Tools.JSONPair(mobile[0],mobile[1]      ) + Environment.NewLine
					          + Tools.JSONPair(query [0],query [1]      ) + Environment.NewLine
					          + Tools.JSONPair(key   [0],key   [1]      ) + Environment.NewLine
					          + Tools.JSONPair(app   [0],app   [1],1,"","}");

				txtXML.Text  = "<Test>"
				             + Tools.XMLCell(uName [0],uName [1]) + Environment.NewLine
					          + Tools.XMLCell(uCode [0],uCode [1]) + Environment.NewLine
					          + Tools.XMLCell(uPwd  [0],uPwd  [1]) + Environment.NewLine
					          + Tools.XMLCell(lCode [0],lCode [1]) + Environment.NewLine
					          + Tools.XMLCell(lDial [0],lDial [1]) + Environment.NewLine
					          + Tools.XMLCell(coun  [0],coun  [1]) + Environment.NewLine
					          + Tools.XMLCell(mobile[0],mobile[1]) + Environment.NewLine
					          + Tools.XMLCell(query [0],query [1]) + Environment.NewLine
					          + Tools.XMLCell(key   [0],key   [1]) + Environment.NewLine
					          + Tools.XMLCell(app   [0],app   [1]) + Environment.NewLine
				             + "</Test>";

				txtWeb.Text  = uName [0] + "=" + uName [1] + "&" + Environment.NewLine
				             + uCode [0] + "=" + uCode [1] + "&" + Environment.NewLine
				             + uPwd  [0] + "=" + uPwd  [1] + "&" + Environment.NewLine
				             + lCode [0] + "=" + lCode [1] + "&" + Environment.NewLine
				             + lDial [0] + "=" + lDial [1] + "&" + Environment.NewLine
				             + coun  [0] + "=" + coun  [1] + "&" + Environment.NewLine
				             + mobile[0] + "=" + mobile[1] + "&" + Environment.NewLine
				             + query [0] + "=" + query [1] + "&" + Environment.NewLine
				             + key   [0] + "=" + key   [1] + "&" + Environment.NewLine
				             + app   [0] + "=" + app   [1];

				txtForm.Text = "<html><body onload='document.forms[\"frmTest\"].submit()'>" + Environment.NewLine
				             + "<form name='frmTest' method='POST' action='" + TargetURL + "'>" + Environment.NewLine
				             + "<input type='hidden' name='" + uName [0] + "' value='" + uName [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + uCode [0] + "' value='" + uCode [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + uPwd  [0] + "' value='" + uPwd  [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + lCode [0] + "' value='" + lCode [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + lDial [0] + "' value='" + lDial [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + coun  [0] + "' value='" + coun  [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + mobile[0] + "' value='" + mobile[1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + query [0] + "' value='" + query [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + key   [0] + "' value='" + key   [1] + "' />" + Environment.NewLine
				             + "<input type='hidden' name='" + app   [0] + "' value='" + app   [1] + "' />" + Environment.NewLine
				             + "</form></body></html>";
			}
		}

		private string TargetURL
		{
			get
			{
				string url = Request.Url.GetLeftPart(UriPartial.Authority);
				return url + ( url.EndsWith("/") ? "" : "/" ) + "UIApplicationQuery.aspx";
			}
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			lblError.Visible = true;
			lblError.Text    = "";
			txtOut.Text      = "";

			try
			{
				if ( rdoForm.Checked )
				{
					System.Web.HttpContext.Current.Response.Clear();
					System.Web.HttpContext.Current.Response.Write(txtForm.Text.Trim());
					System.Web.HttpContext.Current.Response.Flush();
					System.Web.HttpContext.Current.Response.End();
					return;
				}

				byte[]         page;
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(TargetURL);
				webRequest.Method         = "POST";

				if ( rdoJSON.Checked )
				{
					webRequest.ContentType = "application/json;charset=\"utf-8\"";
					webRequest.Accept      = "application/json";
					page                   = Encoding.UTF8.GetBytes(txtJSON.Text.Trim());
				}
				else if ( rdoXML.Checked )
				{
					webRequest.ContentType = "text/xml;charset=\"utf-8\"";
					webRequest.Accept      = "text/xml";
					page                   = Encoding.UTF8.GetBytes(txtXML.Text.Trim());
				}
				else if ( rdoWeb.Checked )
				{
					webRequest.ContentType = "application/x-www-form-urlencoded";
					webRequest.Accept      = "application/x-www-form-urlencoded";
					page                   = Encoding.UTF8.GetBytes(txtWeb.Text.Trim().Replace(Environment.NewLine,""));
				}
				else
					return;

				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}

				using (WebResponse webResponse = webRequest.GetResponse())
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
						txtOut.Text = rd.ReadToEnd();
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,"btnOK_Click/5","XTest");
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("btnOK_Click/10","",220,this);
				Tools.LogException("btnOK_Click/15","",ex2,this);
			}
		}
	}
}