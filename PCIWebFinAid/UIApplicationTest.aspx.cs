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
	public partial class UIApplicationTest : BasePageBackOffice
	{
		protected override void PageLoad(object sender, EventArgs e)
		{
			if ( ! Page.IsPostBack )
				txtIn.Text = Tools.JSONPair("UserName","SheilaColeman@getLost.net",1,"{") + Environment.NewLine
					        + Tools.JSONPair("UserPassword","hello4goodbye") + Environment.NewLine
					        + Tools.JSONPair("QueryName","FinTechLogOn") + Environment.NewLine
					        + Tools.JSONPair("SecretKey","7e6415a7cb790238fd12430a0ce419b3") + Environment.NewLine
					        + Tools.JSONPair("ApplicationCode","001",1,"","}");
		}

		protected void btnOK_Click(Object sender, EventArgs e)
		{
			lblError.Visible = true;
			lblError.Text    = "";
			txtOut.Text      = "";

			try
			{
				byte[]         page       = Encoding.UTF8.GetBytes(txtIn.Text.Trim());
				string         url        = Request.Url.GetLeftPart(UriPartial.Authority);
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url+(url.EndsWith("/")?"":"/")+"UIApplicationQuery.aspx");
				webRequest.Method         = "POST";

				if ( rdoJSON.Checked )
				{
					webRequest.ContentType = "application/json;charset=\"utf-8\"";
					webRequest.Accept      = "application/json";
				}
				else if ( rdoXML.Checked )
				{
					webRequest.ContentType = "text/xml;charset=\"utf-8\"";
					webRequest.Accept      = "text/xml";
				}
				else if ( rdoWeb.Checked )
				{
					webRequest.ContentType = "application/x-www-form-urlencoded";
					webRequest.Accept      = "application/x-www-form-urlencoded";
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