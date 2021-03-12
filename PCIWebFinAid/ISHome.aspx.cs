using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class ISHome : BasePage
	{
		byte   errPriority;
		int    ret;
		string sql;
		string spr;
		string productCode;
		string languageCode;
		string languageDialectCode;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			ApplicationCode = "110";
			errPriority     = 19;

			if ( Page.IsPostBack )
			{
				productCode         = hdnProductCode.Value;
				languageCode        = hdnLangCode.Value;
				languageDialectCode = hdnLangDialectCode.Value;
			}
			else
			{
				LoadStaticDetails();
				LoadDynamicDetails();
				LoadGoogleAnalytics();
				LoadChat();

				btnErrorDtl.Visible = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				btnWidth.Visible    = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
			}
		}

		private void LoadStaticDetails()
		{
		//	Defaults
			productCode         = "10387";
			languageCode        = "ENG";
			languageDialectCode = "0002";
			ret                 = 10003;

			using (MiscList mList = new MiscList())
				try
				{
					ret             = 10008;
					string pageName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
					string refer    = Request.Url.AbsoluteUri.Trim();
					int    k        = refer.IndexOf("://");
					refer           = refer.Substring(k+3);

					if ( ! pageName.StartsWith("/") )
						pageName = "/" + pageName;

					k = refer.ToUpper().IndexOf(pageName.ToUpper());
					if ( k > 0 )
						refer = refer.Substring(0,k);

					ret = 10010;
					spr = "sp_WP_Get_WebsiteInfoByURL";
					sql = "exec " + spr + " " + Tools.DBString(refer);
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadStaticDetails", 10020, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadStaticDetails", 10030, "Internal database error (" + spr + " no data returned)", sql, 2, 2, null, false, errPriority);
					else
					{
						ret                 = 10040;
						productCode         = mList.GetColumn("ProductCode");
						languageCode        = mList.GetColumn("LanguageCode");
						languageDialectCode = mList.GetColumn("LanguageDialectCode");
						ret                 = 10042;
						if ( productCode.Length         < 1 ) productCode         = "10387";
						if ( languageCode.Length        < 1 ) languageCode        = "ENG";
						if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";
					}

					Tools.LogInfo("LoadStaticDetails/10040",sql+" ... PC/LC/LDC="+productCode+"/"+languageCode+"/"+languageDialectCode,222,this);
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
			string fieldURL;
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
							ret         = 10140;
							fieldCode   = mList.GetColumn("WebsiteFieldCode");
						//	fieldName   = mList.GetColumn("WebsiteFieldName");
						//	blocked     = mList.GetColumn("Blocked");
							fieldValue  = mList.GetColumn("WebsiteFieldValue");
							fieldURL    = mList.GetColumn("FieldHyperlinkTarget");
							if ( fieldURL.Length > 0 && fieldURL.Contains("[") )
								fieldURL = fieldURL.Replace("[PC]",Tools.URLString(productCode)).Replace("[LC]",Tools.URLString(languageCode)).Replace("[LDC]",Tools.URLString(languageDialectCode));

							Tools.LogInfo("LoadDynamicDetails/10140","FieldCode="+fieldCode,errPriority,this);
							err         = WebTools.ReplaceControlText(this.Page,"X"+fieldCode,fieldValue,fieldURL,ascxHeader,ascxFooter);
							if ( err   != 0 )
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
							fieldURL   = mList.GetColumn("ImageHyperLink");
							Tools.LogInfo("LoadDynamicDetails/10190","ImageCode="+fieldCode+"/"+fieldValue,errPriority,this);
							err        = WebTools.ReplaceImage(this.Page,fieldCode,fieldValue,
							                                   mList.GetColumn   ("ImageMouseHoverText"),
							                                   fieldURL,
							                                   mList.GetColumnInt("ImageHeight"),
							                                   mList.GetColumnInt("ImageWidth"),
							                                   ascxHeader,
							                                   ascxFooter);
							if ( err != 0 )
								SetErrorDetail("LoadDynamicDetails", 10200, "Unrecognized Image code ("+fieldCode + "/" + fieldValue.ToString() + ")", "WebTools.ReplaceImage('"+fieldCode+"') => "+err.ToString(), 2, 0, null, false, errPriority);
							mList.NextRow();
						}

					ret = 10205;
					spr = "";
					if ( P12010.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12010',false);";
					if ( P12011.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12011',false);";
					if ( P12012.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12012',false);";
					if ( P12023.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12023',false);";
					if ( P12024.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12024',false);";
					if ( P12028.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12028',false);";
					ascxFooter.JSText = WebTools.JavaScriptSource(spr);

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

					ret = 10410;
					spr = "sp_WP_Get_ProductLegalDocumentInfo";
					sql = "exec " + spr + stdParms;
					if ( mList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadDynamicDetails", 10420, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else if ( mList.EOF )
						SetErrorDetail("LoadDynamicDetails", 10430, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					else
						while ( ! mList.EOF )
						{
							ret       = 10440;
							fieldCode = mList.GetColumn("DocumentTypeCode");
							try
							{
								((Literal)FindControl("LH"+fieldCode)).Text = mList.GetColumn("DocumentHeader",1,6);
								((Literal)FindControl("LD"+fieldCode)).Text = mList.GetColumn("DocumentText",1,6);
							}
							catch
							{ }
							mList.NextRow();
						}
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}

			pnlTicks.Visible = ( X100287.Text.Length+X100288.Text.Length+X100289.Text.Length > 0 );
		}

		private void LoadChat()
		{
			lblChat.Text = Tools.LoadChat(productCode);
		}	

		private void LoadGoogleAnalytics()
		{
			lblGoogleUA.Text = Tools.LoadGoogleAnalytics(productCode);
		}
	}
}