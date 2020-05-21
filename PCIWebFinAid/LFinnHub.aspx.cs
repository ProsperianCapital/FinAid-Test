using System;
using System.Net;
using System.IO;
using System.Text;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class LFinnHub : BasePageLogin
	{
		private struct RefreshData
		{
			public int    iter;
			public byte   status;
			public byte   ticker;
			public string interval;
			public string apiKey;
			public string stockList;
			public string forexList;
			public string result;
		}
		private RefreshData pageParms;
		private int         ret;
		private string      dataOK;
		private string      dataErr;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( SessionCheck()    != 0 )
				return;
			if ( SecurityCheck(19) != 0 )
				return;

			dataOK     = "<span style='color:green'>&nbsp;&nbsp;&nbsp;&nbsp;";
			dataErr    = "<span style='color:red'>&nbsp;&nbsp;&nbsp;&nbsp;";
			lblJS.Text = "";

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
				txtKey.Text      = pageParms.apiKey;
				txtRefresh.Text  = pageParms.interval;
				txtStock.Text    = pageParms.stockList;
				txtForex.Text    = pageParms.forexList;
				lblData.Text     = pageParms.result;
				rdoTick1.Checked = ( pageParms.ticker == 1 );
				rdoTick2.Checked = ( pageParms.ticker == 2 );
				if ( pageParms.status != 22 )
					lblJS.Text    = WebTools.JavaScriptSource("ChooseTicker("+pageParms.ticker.ToString()+")");
			}
			else
				lblJS.Text = WebTools.JavaScriptSource("ChooseTicker(0)");

			StartStop(pageParms.status);
			lblDate.Text = Tools.DateToString(System.DateTime.Now,7,1); // yyyy-mm-dd hh:mm:ss
			ascxMenu.SetAdmin();
		}

		private void StartStop(byte which)
		{
		//	btnStart.Text      = ( which != 22 ? "Start" : "Busy ..." );
			btnStart.Enabled   = ( which != 22 );
			txtKey.Enabled     = ( which != 22 );
			txtRefresh.Enabled = ( which != 22 );
			txtStock.Enabled   = ( which != 22 );
			txtForex.Enabled   = ( which != 22 );
			lstCurr.Enabled    = ( which != 22 );
			rdoTick1.Enabled   = ( which != 22 );
			rdoTick2.Enabled   = ( which != 22 );

			btnStop.Enabled    = ( which == 22 );

			if ( which == 22 )
			{
				lblStatus.Text      = "<span style='color:red'>Busy</span>";
				lblRefresh.Text     = "<meta http-equiv='Refresh' content='" + txtRefresh.Text + "' />";
				pageParms.status    = 22;
				pageParms.interval  = txtRefresh.Text;
				pageParms.apiKey    = txtKey.Text;
				pageParms.stockList = txtStock.Text;
				pageParms.forexList = txtForex.Text;
				pageParms.ticker    = (byte)( rdoTick1.Checked ? 1 : ( rdoTick2.Checked ? 2 : 0 ) );
//				lblJS.Text          = WebTools.JavaScriptSource("ChooseTicker("+pageParms.ticker.ToString()+")");
//				pageParms.result    = lblData.Text;
				FetchData();
			}
			else
			{
				lblJS.Text       = WebTools.JavaScriptSource("ChooseTicker("+pageParms.ticker.ToString()+")");
				lblStatus.Text   = "<span style='color:green'>Inactive</span>";
				lblRefresh.Text  = "";
				pageParms.status = 11;
			}
		}

		private void FetchData()
		{
			if ( lblData.Text.Length > 2000 )
				lblData.Text = lblData.Text.Substring(0,1000);

			string   url;
			string   urlX;
			string   show;
			string   result  = "";
			string   curr    = "";
			string   symb    = "";
			string[] symbols = null;

			if ( rdoTick1.Checked )
			{
				ret     = 10;
				show    = "Stock";
				symbols = txtStock.Text.Split(',');
				url     = "https://finnhub.io/api/v1/quote?token=" + txtKey.Text + "&symbol=";
			}
			else if ( rdoTick2.Checked )
			{
				ret     = 15;
				show    = "Currency";
				curr    = lstCurr.SelectedValue.Trim().ToUpper();
				symbols = txtForex.Text.Split(',');
//				url     = "https://finnhub.io/api/v1/forex/candle?resolution=D&token=" + txtKey.Text;
				url     = "https://finnhub.io/api/v1/forex/candle?resolution=D&token=" + txtKey.Text
					     + "&from=" + DateTimeOffset.Now.Subtract(new TimeSpan(1,0,0,0)).ToUnixTimeSeconds().ToString()
						  + "&to="   + DateTimeOffset.Now.ToUnixTimeSeconds().ToString() + "&symbol=OANDA:";
			}
			else
				return;

			for ( int k = 0 ; k < symbols.Length ; k++ )
				try
				{
					ret  = 20;
					symb = symbols[k].ToUpper().Trim();
					if ( symb.Length < 1 )
						continue;

					if ( rdoTick1.Checked )
					{
						urlX   = url + symb;
						result = result + "&nbsp;- " + show + " " + symb + "<br />";
					}
					else
					{
						result = result + "&nbsp;- " + show + " " + curr + "->" + symb + "<br />";
						urlX   = "";
						if ( symb == curr )
							result = result + dataErr + "Cannot convert";
						else
						{
							result = result + GetWebData (url+curr+"_"+symb) + "</span><br />";
							urlX   = url + symb + "_" + curr;
							result = result + "&nbsp;- " + show + " " + symb + "->" + curr + "<br />";
						}
					}
	
					result = result + GetWebData(urlX) + "</span><br />";

/*
					ret                    = 30;
					webRequest             = (HttpWebRequest)WebRequest.Create(urlX);
//					webRequest.Accept      = "text/xml";
					webRequest.ContentType = "application/x-www-form-urlencoded";
					webRequest.Method      = "GET";
//					webRequest.KeepAlive   = false;
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
*/
				}
				catch (Exception ex)
				{
					result = result + dataErr + "Error, ret="+ret.ToString()+"</span><br />";
					Tools.LogException("LFinnHub.FetchData","ret="+ret.ToString()+", "+url+symb,ex);
				}
				
			pageParms.iter++;
			ret                     = 80;
			pageParms.result        = "(" + pageParms.iter.ToString() + ") " + Tools.DateToString(System.DateTime.Now,7,1) + " : Fetch data<br />" + result + "<br />" + lblData.Text;
			lblData.Text            = pageParms.result;
			Session["LFinnHubData"] = pageParms;
		}


		private string GetWebData(string url)
		{
			if ( string.IsNullOrWhiteSpace(url) )
				return "";

			try
			{
				string         json       = "";
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
//				webRequest.Accept         = "text/xml";
				webRequest.ContentType    = "application/x-www-form-urlencoded";
				webRequest.Method         = "GET";
//				webRequest.KeepAlive      = false;
				ret                       = 80010;

				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 80020;;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret  = 80030;
						json = rd.ReadToEnd().Trim();
					}
				}

				webRequest = null;

				if ( json.Length < 1 )
					return "No data returned";
				else if ( json.StartsWith("{") )
					return dataOK + json;
				else
					return dataErr + json;
			}
			catch (Exception ex)
			{
				Tools.LogException("LFinnHub.GetWebData","ret="+ret.ToString()+", "+url,ex);
			}
			return dataErr + "Error, ret="+ret.ToString();
		}

		private int ValidatePage()
		{
			string apiKey  = txtKey.Text.Trim();
			string stocks  = txtStock.Text.Trim();
			string forex   = txtForex.Text.Trim();
			int    refresh = Tools.StringToInt(txtRefresh.Text);
			lblErr.Text    = "";

			if ( apiKey.Length < 4 )
				lblErr.Text = lblErr.Text + "Invalid API key<br />";
			if ( refresh < 5 || refresh > 1200 )
				lblErr.Text = lblErr.Text + "Invalid refresh interval (must be between 5 and 1200)<br />";
			else
				txtRefresh.Text = refresh.ToString();
			if ( stocks.Length < 2 && rdoTick1.Checked )
				lblErr.Text = lblErr.Text + "Invalid list of stock symbols<br />";
			if ( forex.Length  < 2 && rdoTick2.Checked )
				lblErr.Text = lblErr.Text + "Invalid list of currency symbols<br />";

			return lblErr.Text.Length;
		}

		protected void btnStart_Click(Object sender, EventArgs e)
		{
			if ( ValidatePage() == 0 )
				StartStop(22);
		}

		protected void btnStop_Click(Object sender, EventArgs e)
		{
			StartStop(11);
		}

		protected void btnClear_Click(Object sender, EventArgs e)
		{
			lblData.Text     = "";
			txtStock.Text    = "";
			txtForex.Text    = "";
			txtRefresh.Text  = "10";
			rdoTick1.Checked = false;
			rdoTick2.Checked = false;
			SessionClearData();
			StartStop(1);
		}
	}
}