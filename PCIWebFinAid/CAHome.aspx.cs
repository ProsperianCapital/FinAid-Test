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
					sql = "exec sp_WP_Get_ProductWebsiteInfo @ProductURL = 'www.careassistza.com'";
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadStaticDetails", 10020, "Internal database error (sp_WP_Get_ProductWebsiteInfo failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadStaticDetails", 10030, "Internal database error (sp_WP_Get_ProductWebsiteInfo no data returned)", sql, 2, 2, null, false, errPriority);
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
					sql = "exec sp_WP_Get_ProductLanguageInfo @ProductCode=" + Tools.DBString(productCode);
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadStaticDetails", 10060, "Internal database error (sp_WP_Get_ProductLanguageInfo failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadStaticDetails", 10070, "Internal database error (sp_WP_Get_ProductLanguageInfo no data returned)", sql, 2, 2, null, false, errPriority);
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
					sql = "exec sp_WP_Get_ProductContent" + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10120, "Internal database error (sp_WP_Get_ProductContent failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10130, "Internal database error (sp_WP_Get_ProductContent no data returned)", sql, 2, 2, null, false, errPriority);
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
							if ( err != 0 )
								SetErrorDetail("LoadDynamicDetails", 10150, "Unrecognized HTML control (X"+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceControlText('X"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							mList.NextRow();
						}

					ret = 10160;
					sql = "exec sp_WP_Get_ProductImageInfo" + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10170, "Internal database error (sp_WP_Get_ProductImageInfo failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10180, "Internal database error (sp_WP_Get_ProductImageInfo no data returned)", sql, 2, 2, null, false, errPriority);
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
					XHIW.Text = "";
					sql       = "exec sp_WP_Get_ProductHIWInfo" + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10220, "Internal database error (sp_WP_Get_ProductHIWInfo failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10230, "Internal database error (sp_WP_Get_ProductHIWInfo no data returned)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret        = 10240;
							fieldCode  = mList.GetColumn("Serial");
							fieldHead  = mList.GetColumn("HIWHeader",6);
							fieldValue = mList.GetColumn("HIWDetail",6);
						//	blocked    = mList.GetColumn("Blocked");
							Tools.LogInfo("LoadDynamicDetails/10240","HIW="+fieldCode+"/"+fieldHead,errPriority,this);
							if ( "0123456789".Contains(fieldHead.Substring(0,1)) )
								XHIW.Text = XHIW.Text + "<p class='HIWHead1'>"   + fieldHead  + "</p>"
								                      + "<p class='HIWDetail1'>" + fieldValue + "</p>";
							else
								XHIW.Text = XHIW.Text + "<p class='HIWHead2'>"   + fieldHead  + "</p>"
								                      + "<p class='HIWDetail2'>" + fieldValue + "</p>";
							mList.NextRow();
						}
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}
		}
	}
}