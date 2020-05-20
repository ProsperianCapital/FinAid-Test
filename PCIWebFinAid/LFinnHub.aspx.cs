using System;
using System.Net;
using System.IO;
using System.Text;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class LFinnHub : BasePageLogin
	{
//		private string refreshHTML;
		private struct RefreshData
		{
			public int    iter;
			public byte   status;
			public string interval;
			public string apiKey;
			public string stockList;
			public string result;
		}
		private RefreshData pageParms;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck()    != 0 )
				return;
			if ( SecurityCheck(19) != 0 )
				return;

			try
			{
				lblErr.Text = "";
				pageParms   = (RefreshData)Session["LFinnHubData"];
			}
			catch
			{
				pageParms.iter   = 0;
				pageParms.status = 1;
			}

			if ( Page.IsPostBack )
				return;

			if ( pageParms.iter > 0 )
			{
				txtKey.Text     = pageParms.apiKey;
				txtRefresh.Text = pageParms.interval;
				txtStock.Text   = pageParms.stockList;
				lblData.Text    = pageParms.result;
			}

			StartStop(pageParms.status);
			ascxMenu.SetAdmin();
		}

		private void StartStop(byte which)
		{
		//	btnStart.Text      = ( which != 2 ? "Start" : "Busy ..." );
			btnStart.Enabled   = ( which != 2 );
			txtKey.Enabled     = ( which != 2 );
			txtRefresh.Enabled = ( which != 2 );
			txtStock.Enabled   = ( which != 2 );

			btnStop.Enabled    = ( which == 2 );

			if ( which == 2 )
			{
				lblStatus.Text      = "<span style='color:red'>Busy</span>";
				lblRefresh.Text     = "<meta http-equiv='Refresh' content='" + txtRefresh.Text + "' />";
				pageParms.status    = 2;
				pageParms.interval  = txtRefresh.Text;
				pageParms.apiKey    = txtKey.Text;
				pageParms.stockList = txtStock.Text;
//				pageParms.result    = lblData.Text;
				FetchData();
			}
			else
			{
				lblStatus.Text   = "<span style='color:green'>Inactive</span>";
				lblRefresh.Text  = "";
				pageParms.status = 1;
			}
		}

		private void FetchData()
		{
			if ( lblData.Text.Length > 2000 )
				lblData.Text = lblData.Text.Substring(0,1000);

			string         json;
			string         dataOK  = "<span style='color:green'>&nbsp;&nbsp;&nbsp;&nbsp;";
			string         dataErr = "<span style='color:red'>&nbsp;&nbsp;&nbsp;&nbsp;";
			string         url     = "https://finnhub.io/api/v1/quote?token=" + txtKey.Text + "&symbol=";
			string         stk     = "";
			string[]       stkList = txtStock.Text.Split(',');
			int            ret     = 10;
			string         result  = "";
			HttpWebRequest webRequest;

			for ( int k = 0 ; k < stkList.Length ; k++ )
				try
				{
					ret = 20;
					stk = stkList[k].ToUpper().Trim();
					if ( stk.Length < 1 )
						continue;

					ret                    = 30;
					webRequest             = (HttpWebRequest)WebRequest.Create(url+stk);
//					webRequest.Accept      = "text/xml";
					webRequest.ContentType = "application/x-www-form-urlencoded";
					webRequest.Method      = "GET";
//					webRequest.KeepAlive   = false;
					result                 = result + "&nbsp;- Stock " + stk + "<br />";
					json                   = "";
					ret                    = 40;

					using (WebResponse webResponse = webRequest.GetResponse())
					{
						ret = 50;
						using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
						{
							ret  = 60;
							json = rd.ReadToEnd().Trim();
						}
					}

					if ( json.Length < 1 )
						result = result + dataErr + "No data returned";
					else if ( json.StartsWith("{") )
						result = result + dataOK + json;
					else
						result = result + dataErr + json;
					result = result + "</span><br />";
					ret    = 70;
				}
				catch (Exception ex)
				{
					result = result + dataErr + "Error, ret="+ret.ToString()+"</span><br />";
					Tools.LogException("LFinnHub.FetchData","ret="+ret.ToString()+", "+url+stk,ex);
				}
				
			pageParms.iter++;
			ret                     = 80;
			pageParms.result        = "(" + pageParms.iter.ToString() + ") " + Tools.DateToString(System.DateTime.Now,7,1) + " : Fetch data<br />" + result + "<br />" + lblData.Text;
			lblData.Text            = pageParms.result;
			Session["LFinnHubData"] = pageParms;
			webRequest              = null;
		}

		private int ValidatePage()
		{
			string apiKey  = txtKey.Text.Trim();
			string stocks  = txtStock.Text.Trim();
			int    refresh = Tools.StringToInt(txtRefresh.Text);
			lblErr.Text    = "";

			if ( apiKey.Length < 4 )
				lblErr.Text = lblErr.Text + "Invalid API key<br />";
			if ( stocks.Length < 2 )
				lblErr.Text = lblErr.Text + "Invalid list of stock symbols<br />";
			if ( refresh < 5 || refresh > 1200 )
				lblErr.Text = lblErr.Text + "Invalid refresh interval (must be between 5 and 1200)<br />";
			else
				txtRefresh.Text = refresh.ToString();

			return lblErr.Text.Length;
		}

		protected void btnStart_Click(Object sender, EventArgs e)
		{
			if ( ValidatePage() == 0 )
				StartStop(2);
		}

		protected void btnStop_Click(Object sender, EventArgs e)
		{
			StartStop(1);
		}

		protected void btnClear_Click(Object sender, EventArgs e)
		{
			lblData.Text    = "";
			txtStock.Text   = "";
			txtRefresh.Text = "10";
			SessionClearData();
			StartStop(1);
		}
	}
}