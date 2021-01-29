using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class CAHome : BasePage
	{
		byte   errPriority;
		int    ret;
		string sql;
		string spr;

		string productCode;
		string languageCode;
		string languageDialectCode;

		string countryCode;
		string templateCode;
		string currencyCode;
		string websiteCode;
		string googleCode;
		string chatSnippet;
		string facebookSnippet;
		string instagramSnippet;
		string baiduSnippet;
		string backgroundColour;
		string foregroundColour;
		string headerCode;
		string footerCode;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			errPriority = 241; // Log everything

			if ( Page.IsPostBack )
			{
				productCode         = hdnProductCode.Value;
				languageCode        = WebTools.ListValue(ascxHeader.lstLanguage,"");
				languageDialectCode = hdnLangDialectCode.Value;
				if ( languageCode  != hdnLangCode.Value && languageCode.Length > 0 )
					LoadDynamicDetails();
				else
					languageCode   = hdnLangCode.Value;
				hdnLangCode.Value = languageCode;
			}
			else
			{
				LoadStaticDetails();
				LoadDynamicDetails();

				btnErrorDtl.Visible = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				btnWidth.Visible    = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
			}
		}

		private void LoadStaticDetails()
		{
			productCode         = "";
			languageCode        = "";
			languageDialectCode = "";

			using (MiscList mList = new MiscList())
				try
				{
					ret = 10010;
					spr = "sp_WP_Get_ProductWebsiteInfo";
					sql = "exec " + spr + " @ProductURL = 'www.careassistza.com'";
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadStaticDetails", 10020, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadStaticDetails", 10030, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
					{
						ret              = 10040;
						productCode      = mList.GetColumn("ProductCode"); // Use 10122 for testing
						countryCode      = mList.GetColumn("CountryCode");
						templateCode     = mList.GetColumn("TemplateCode");
						currencyCode     = mList.GetColumn("ProductCurrencyCode");
						websiteCode      = mList.GetColumn("WebsiteCode");
						googleCode       = mList.GetColumn("GoogleAnalyticsCode");
						chatSnippet      = mList.GetColumn("ChatSnippet");
						facebookSnippet  = mList.GetColumn("FacebookSnippet");
						instagramSnippet = mList.GetColumn("InstagramSnippet");
						baiduSnippet     = mList.GetColumn("BaiduSnippet");
						backgroundColour = mList.GetColumn("WebsiteBackgroundColour");
						foregroundColour = mList.GetColumn("WebsiteTextColour");
						headerCode       = mList.GetColumn("WebsiteHeaderColour");
						footerCode       = mList.GetColumn("WebsiteFooterColour");
					//	blocked          = mList.GetColumn("Blocked");
						Tools.LogInfo("LoadStaticDetails/10040","Product="+productCode+"/"+countryCode+"/"+currencyCode,errPriority,this);
					}

					ret = 10050;
					spr = "sp_WP_Get_ProductLanguageInfo";
					sql = "exec " + spr + " @ProductCode=" + Tools.DBString(productCode);
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadStaticDetails", 10060, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadStaticDetails", 10070, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
					{
						string       lCode;
						string       lDialectCode;
						DropDownList lstLang = ascxHeader.lstLanguage;

						while ( ! mList.EOF )
						{
							ret          = 10080;
							lCode        = mList.GetColumn("LanguageCode");
							lDialectCode = mList.GetColumn("LanguageDialectCode");
						//	blocked      = mList.GetColumn("Blocked");
							Tools.LogInfo("LoadStaticDetails/10080","Language="+lCode+"/"+lDialectCode,errPriority,this);
							lstLang.Items.Add(new System.Web.UI.WebControls.ListItem(lCode,lDialectCode));
							if ( mList.GetColumn("DefaultIndicator").ToUpper() == "Y" )
							{
								ret                   = 10090;
								languageCode          = lCode;
								languageDialectCode   = lDialectCode;
								lstLang.SelectedIndex = lstLang.Items.Count - 1;
							}
							mList.NextRow();
						}
						if ( languageCode.Length == 0 )
						{
							ret                   = 10100;
							languageCode          = lstLang.Items[0].Text;
							languageDialectCode   = lstLang.Items[0].Value;
							lstLang.SelectedIndex = 0;
						}
					}
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadStaticDetails/99","ret="+ret.ToString(),ex,this);
				}

			hdnProductCode.Value     = productCode;
			hdnLangCode.Value        = languageCode;
			hdnLangDialectCode.Value = languageDialectCode;
			hdnVer.Value             = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
			string legalParms        = "&PC="  + productCode
			                         + "&LC="  + languageCode
			                         + "&LDC=" + languageDialectCode;
			X100041.NavigateUrl      = X100041.NavigateUrl + legalParms;
			X100042.NavigateUrl      = X100042.NavigateUrl + legalParms;
			X100043.NavigateUrl      = X100043.NavigateUrl + legalParms;
			X100044.NavigateUrl      = X100044.NavigateUrl + legalParms;
		}

		private void LoadDynamicDetails()
		{
			byte   err;
			string fieldCode;
			string fieldHead;
			string fieldValue;
			string stdParms = " @ProductCode="         + Tools.DBString(productCode)
					          + ",@LanguageCode="        + Tools.DBString(languageCode)
					          + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);

			using (MiscList mList = new MiscList())
				try
				{
					ret = 10110;
					spr = "sp_WP_Get_ProductContent";
					sql = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10120, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10130, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10140;
							fieldCode  = mList.GetColumn("WebsiteFieldCode");
						//	fieldName  = mList.GetColumn("WebsiteFieldName");
							fieldValue = mList.GetColumn("WebsiteFieldValue");
						//	blocked    = mList.GetColumn("Blocked");
							Tools.LogInfo("LoadDynamicDetails/10140","FieldCode="+fieldCode,errPriority,this);
							err        = WebTools.ReplaceControlText(this.Page,"X"+fieldCode,fieldValue,ascxHeader);
							if ( err  != 0 )
								SetErrorDetail("LoadDynamicDetails", 10150, "Unrecognized HTML control (X"+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceControlText('X"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							mList.NextRow();
						}

					ret = 10160;
					spr = "sp_WP_Get_ProductImageInfo";
					sql = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10170, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10180, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10190;
							fieldCode  = mList.GetColumn("ImageCode");
							fieldValue = mList.GetColumn("ImageFileName");
						//	blocked    = mList.GetColumn("Blocked");
							Tools.LogInfo("LoadDynamicDetails/10190","ImageCode="+fieldCode+"/"+fieldValue,errPriority,this);
							err        = WebTools.ReplaceImage(this.Page,fieldCode,fieldValue,
							                                   mList.GetColumn   ("ImageMouseHoverText"),
							                                   mList.GetColumn   ("ImageHyperLink"),
							                                   mList.GetColumnInt("ImageHeight"),
							                                   mList.GetColumnInt("ImageWidth"),
							                                   ascxHeader);
							if ( err != 0 )
								SetErrorDetail("LoadDynamicDetails", 10200, "Unrecognized Image code ("+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceImage('"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							mList.NextRow();
						}

					ret       = 10210;
					xHIW.Text = "";
					spr       = "sp_WP_Get_ProductHIWInfo";
					sql       = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10220, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10230, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10240;
							fieldCode  = mList.GetColumn("Serial");
							fieldHead  = mList.GetColumn("HIWHeader",0,6);
							fieldValue = mList.GetColumn("HIWDetail",0,6);
							Tools.LogInfo("LoadDynamicDetails/10240","HIW="+fieldCode+"/"+fieldHead,errPriority,this);
							if ( "0123456789".Contains(fieldHead.Substring(0,1)) )
								xHIW.Text = xHIW.Text + "<p class='HIWHead1'>"   + fieldHead  + "</p>"
								                      + "<p class='HIWDetail1'>" + fieldValue + "</p>";
							else
								xHIW.Text = xHIW.Text + "<p class='HIWHead2'>"   + fieldHead  + "</p>"
								                      + "<p class='HIWDetail2'>" + fieldValue + "</p>";
							mList.NextRow();
						}

					ret       = 10310;
					xFAQ.Text = "";
					spr       = "sp_WP_Get_ProductFAQInfo";
					sql       = "exec " + spr + stdParms;
					string hd = "*P*";

					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10320, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10330, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10340;
							fieldCode  = mList.GetColumn("FAQ"      ,0,6);
							fieldHead  = mList.GetColumn("FAQHeader",0,6);
							fieldValue = mList.GetColumn("FAQDetail",0,6);
							if ( fieldHead != hd )
							{
								ret = 10350;
								if ( hd == "*P*" )
									xFAQ.Text = xFAQ.Text + "<a href='JavaScript:FAQ()' title='Close' style='float:right;padding:4px'><img src='" + Tools.ImageFolder() + "Close1.png' /></a>";
								xFAQ.Text    = xFAQ.Text + "<p class='FAQHead'>" + fieldHead  + "</p>";
								hd           = fieldHead;
							}
							ret       = 10360;
							xFAQ.Text = xFAQ.Text + "<p class='FAQQuestion'>" + fieldCode  + "</p>"
								                   + "<p class='FAQAnswer'>"   + fieldValue + "</p>";
							Tools.LogInfo("LoadDynamicDetails/10240","FAQ="+fieldCode+"/"+fieldHead,errPriority,this);
							mList.NextRow();
						}
					X100063.Visible = ( xFAQ.Text.Length > 0 );
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}
		}
	}
}