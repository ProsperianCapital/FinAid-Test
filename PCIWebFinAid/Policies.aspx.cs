using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class Policies : BasePage
	{
		byte   errPriority;
		int    ret;
		string sql;
		string spr;
		string productCode;
		string languageCode;
		string languageDialectCode;

		protected Literal F12014; // Favicon

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			errPriority = 19;

			if ( Page.IsPostBack )
			{
				productCode         = hdnProductCode.Value;
				languageCode        = hdnLangCode.Value;
				languageDialectCode = hdnLangDialectCode.Value;
			}
			else
			{
				productCode         = WebTools.RequestValueString(Request,"ProductCode");
				languageCode        = WebTools.RequestValueString(Request,"LanguageCode");
				languageDialectCode = WebTools.RequestValueString(Request,"LanguageDialectCode");

//				if ( ! Tools.SystemIsLive() )
//				{
					if ( productCode.Length         < 1 ) productCode         = "10387";
					if ( languageCode.Length        < 1 ) languageCode        = "ENG";
					if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";
//				}	

//				LoadStaticDetails();
				LoadDynamicDetails();
				LoadGoogleAnalytics();
				LoadChat();

				hdnProductCode.Value     = productCode;
				hdnLangCode.Value        = languageCode;
				hdnLangDialectCode.Value = languageDialectCode;
				btnErrorDtl.Visible      = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				btnWidth.Visible         = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
			}
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

//					if ( P12010.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12010',false);";
//					if ( P12011.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12011',false);";
//					if ( P12012.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12012',false);";
//					if ( P12023.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12023',false);";
//					if ( P12024.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12024',false);";
//					if ( P12028.ImageUrl.Length < 5 ) spr = spr + "ShowElt('D12028',false);";
//					ascxFooter.JSText = WebTools.JavaScriptSource(spr);

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
							ret = 10440;
							if ( mList.GetColumn("DocumentTypeCode") == "003" )
							{
								ret          = 10444;
								polHead.Text = mList.GetColumn("DocumentHeader",1,6);
								polDtl.Text  = mList.GetColumn("DocumentText",1,6);
								break;
							}
							mList.NextRow();
						}

					pnlContact01.Visible = ( X100093.Text.Length > 0 );
					pnlContact02.Visible = ( X104402.Text.Length > 0 );
					pnlContact03.Visible = ( X100095.Text.Length > 0 );
					pnlContact04.Visible = ( X100096.Text.Length > 0 || P12031.ImageUrl.Length > 0 );
					pnlContact05.Visible = ( X100101.Text.Length > 0 );
					pnlContact06.Visible = ( X104404.Text.Length > 0 || P12032.ImageUrl.Length > 0 );
					pnlContact07.Visible = ( X100102.Text.Length > 0 || P12033.ImageUrl.Length > 0 );
					pnlContact08.Visible = ( X104418.Text.Length > 0 );
					pnlContact09.Visible = ( X100105.Text.Length > 0 || P12034.ImageUrl.Length > 0 );

//	Testing
//					WebTools.ReplaceImage(this.Page,"12002","isos1.png","isos1");
//					WebTools.ReplaceImage(this.Page,"12036","isos2.png","isos2");
//	Testing
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}
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