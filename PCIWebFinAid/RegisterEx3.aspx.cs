﻿using System;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class RegisterEx3 : BasePage
	{
		private   byte   logDebug = 40; // 240;
		private   string productCode;
		private   string languageCode;
		private   string languageDialectCode;
		private   string contractCode;
		private   string contractPIN;
		private   string sql;
//		private   int    tokenExMode;
		private   int    errNo;
		private   int    pageNo;

//	3d Secure stuff
//	See SPR "sp_WP_Get_ProductInfo"
		private string   bureauCodeToken;
		private string   tokenMID;
		private string   tokenKey;
		private string   bureauCodePayment;
		private string   paymentURL;
		private string   paymentMID;
		private string   paymentKey;
		private string   paymentCurrency;
		private string   paymentAmount;

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
//	TokenEx card number validation version. 3 configuration variables are needed in Web.Config.

//	<appSettings>
//	TEST
//		<add key="TokenEx/Id"     value="4311038889209736" />
//		<add key="TokenEx/Key"    value="njSRwZVKotSSbDAZtLBIXYrCznNUx2oOZFMVZp7I" />
//		<add key="TokenEx/Script" value="https://test-htp.tokenex.com/iframe/iframe-v3.min.js" />
//	LIVE
//		<add key="TokenEx/Id"     value="7244633025631206" />
//		<add key="TokenEx/Key"    value="" />
//		<add key="TokenEx/Script" value="https://htp.tokenex.com/iframe/iframe-v3.min.js" />
//	</appSettings>

			SetErrorDetail("",-888);
			SetPostBackURL();
			SetWarning("");

			pnl3d.Visible   = false;
//			lblRefresh.Text = "";
//			lbl3d.Text      = "";

			if ( Page.IsPostBack )
			{
				productCode         = WebTools.ViewStateString(ViewState,"ProductCode");
				languageCode        = WebTools.ViewStateString(ViewState,"LanguageCode");
				languageDialectCode = WebTools.ViewStateString(ViewState,"LanguageDialectCode");
				contractCode        = WebTools.ViewStateString(ViewState,"ContractCode");
				contractPIN         = WebTools.ViewStateString(ViewState,"ContractPIN");
				bureauCodeToken     = WebTools.ViewStateString(ViewState,"BureauCodeToken");
				bureauCodePayment   = WebTools.ViewStateString(ViewState,"BureauCodePayment");
				paymentURL          = WebTools.ViewStateString(ViewState,"PaymentURL");
//				tokenURL            = WebTools.ViewStateString(ViewState,"TokenURL");
				tokenMID            = WebTools.ViewStateString(ViewState,"TokenMID");
				tokenKey            = WebTools.ViewStateString(ViewState,"TokenKey");
				paymentMID          = WebTools.ViewStateString(ViewState,"PaymentMID");
				paymentKey          = WebTools.ViewStateString(ViewState,"PaymentKey");
				paymentCurrency     = WebTools.ViewStateString(ViewState,"PaymentCurrency");
				pageNo              = Tools.StringToInt(hdnPageNo.Value);

//				This code MUST BE HERE!
				if ( pageNo == 5 )
					if ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
						btnNext_Click(null,null); // Isn't called because TokenEx disables the .NET postback
			}
			else
			{
				txScript.Text       = "<script src='" + Tools.ConfigValue("TokenEx/Script") + "'></script>";
				lblJS.Text          = WebTools.JavaScriptSource("NextPage(0,null)");
				productCode         = WebTools.RequestValueString(Request,"PC");  // ProductCode");
				languageCode        = WebTools.RequestValueString(Request,"LC");  // LanguageCode");
				languageDialectCode = WebTools.RequestValueString(Request,"LDC"); // LanguageDialectCode");
//				hdnReferURL.Value   = WebTools.ClientReferringURL(Request,11);

				if ( ! Tools.SystemIsLive() )
				{
//	Testing 1 (English)
					if ( productCode.Length         < 1 ) productCode         = "10278";
					if ( languageCode.Length        < 1 ) languageCode        = "ENG";
					if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";
//	Testing 2 (Thai)
//					if ( productCode.Length         < 1 ) productCode         = "10024";
//					if ( languageCode.Length        < 1 ) languageCode        = "THA";
//					if ( languageDialectCode.Length < 1 ) languageDialectCode = "0001";
				}
				else if ( productCode.Length         < 1 ||
				          languageCode.Length        < 1 ||
					       languageDialectCode.Length < 1 )
				{
					SetErrorDetail("PageLoad",33088,"Invalid startup values ... system cannot continue","");
					SetWarning("B","Invalid startup values.");
					return;
				}

//				if ( WebTools.CheckProductProvider(productCode,"RegisterEx3.aspx",Request,Response) == 0 )
//					return;

				ViewState["ProductCode"]         = productCode;
				ViewState["LanguageCode"]        = languageCode;
				ViewState["LanguageDialectCode"] = languageDialectCode;

				hdnVer.Value        = "Version " + SystemDetails.AppVersion + " (" + SystemDetails.AppDate + ")";
				lblVer.Text         = "Version " + SystemDetails.AppVersion;
				lblVer.Visible      = ! Tools.SystemIsLive();
				btnBack1.Visible    = ! Tools.SystemIsLive();
				lblReg.Visible      = true;
				lblRegConf.Visible  = false;

				LoadGoogleAnalytics();
				LoadChat();
				LoadLabels();

				if ( CheckIP() != "B" ) // Blocked
				{
					LoadContractCode();
					btnNext.Visible = ( lblError.Text.Length == 0 );
					if ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
					{
						pnlTokenYes.Visible = true;
						pnlTokenNo.Visible  = false;
						txtCCNumber.Text    = "";
					}
					else
					{
						pnlTokenYes.Visible = false;
						pnlTokenNo.Visible  = true;
						txToken.Value       = "";
					}
				}

//	Testing 2
//				if ( hdn100137.Value.Length < 1 ) hdn100137.Value = "PRIME" + Environment.NewLine + "ASSIST";
//				if ( hdn100002.Value.Length < 1 ) hdn100002.Value = "Emergency Mobile, Legal & Financial Assistance";
//				if ( lblReg.Text.Length     < 1 ) lblReg.Text     = "Registration";
//				if ( lblRegConf.Text.Length < 1 ) lblRegConf.Text = "Registration Confirmation";

//	Testing 3
				if ( WebTools.RequestValueInt(Request,"PageNoX") > 0 )
				{
					hdnPageNo.Value = WebTools.RequestValueString(Request,"PageNoX");
					btnNext_Click(null,null);
				}
			}
		}

		private void SetWarning(string action="",string msg="")
		{
			action                = Tools.NullToString(action).ToUpper();
			pnlWarning.Visible    = ( action.Length > 0 );
			lblWarnP.Visible      = ( action == "P" );
			lblWarnB.Visible      = ( action == "B" );
			pnlDisabled.Visible   = ( action == "B" );

			if ( msg.Length > 0 )
				lblWarnX.Text      = msg;
			else
				lblWarnX.Text      = "Your IP address is not listed as from the country this product is sold.";

			if ( action == "B" )
			{
				btnBack1.Visible   = false;
				btnBack2.Visible   = false;
				btnNext.Visible    = false;
				lstTitle.Enabled   = false;
				txtSurname.Enabled = false;
				txtCellNo.Enabled  = false;
				chkAgree.Enabled   = false;
			}
		}

		private void SetPostBackURL()
		{
			int    pNo = Tools.StringToInt(hdnPageNo.Value);
			string url = "RegisterEx3.aspx";
			if ( pNo > 0 )
				url = url + "?PageNo=" + (pNo+1).ToString();
			btnNext.PostBackUrl  = url;
			btnAgree.PostBackUrl = url;
		}

		private void HideControls(string controlID)
		{
			try
			{
				Control ctlToHide;
				ctlToHide       = FindControl("lbl"+controlID+"Label");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("txt"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("lst"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("opt"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("chk"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("img"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("hdn"+controlID+"Help");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("hdn"+controlID+"Error");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("hdn"+controlID+"Guide");
				if ( ctlToHide != null ) ctlToHide.Visible = false;
				ctlToHide       = FindControl("lblp6"+controlID);
				if ( ctlToHide != null ) ctlToHide.Visible = false;
			}
			catch (Exception ex)
			{
				Tools.LogException("RegisterEx3.HideControls","",ex);
			}
			lblJS.Text = WebTools.JavaScriptSource("ShowElt('tr"+controlID+"',false);ShowElt('trp6"+controlID+"',false)",lblJS.Text,1);
		}

		private void LoadGoogleAnalytics()
		{
			lblGoogleUA.Text = "";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec sp_WP_Get_GoogleACA @ProductCode=" + Tools.DBString(productCode);

					if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
					{
						string gaCode    = miscList.GetColumn("GoogleAnalyticCode");
						string url       = miscList.GetColumn("URL");
//	V1 ... gtag.js
//						lblGoogleUA.Text = Environment.NewLine
//						                 + "<!-- Global site tag (gtag.js) - Google Analytics -->" + Environment.NewLine
//						                 + "<script async src=\"https://www.googletagmanager.com/gtag/js?id=" + gaCode + "\"></script>" + Environment.NewLine
//						                 + "<script>" + Environment.NewLine
//						                 + "window.dataLayer = window.dataLayer || [];" + Environment.NewLine
//						                 + "function gtag(){dataLayer.push(arguments);}" + Environment.NewLine
//						                 + "gtag('js', new Date());" + Environment.NewLine
//						                 + "gtag('config', '" + gaCode + "', { 'linker': { 'accept_incoming': true } });" + Environment.NewLine
// //					                 + "gtag('config', '" + gaCode + "');" + Environment.NewLine
//						                 + "</script>" + Environment.NewLine;

// V2 ... Analytics.js
						lblGoogleUA.Text = Environment.NewLine
						                 + "<script>" + Environment.NewLine
						                 + "(function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){"
						                 + "(i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),"
						                 + "m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)"
						                 + "})(window,document,'script','https://www.google-analytics.com/analytics.js','ga');" + Environment.NewLine
						                 + "ga('create', '" + gaCode + "', 'auto', {'allowLinker': true});" + Environment.NewLine
						                 + "ga('require', 'linker');" + Environment.NewLine
						                 + "ga('linker:autoLink', ['" + url + "'] );" + Environment.NewLine
						                 + "ga('send', 'pageview');" + Environment.NewLine
						                 + "</script>" + Environment.NewLine;
					}
					else
					{
						Tools.LogInfo     ("RegisterEx3.LoadGoogleAnalytics/1","Failed to load Google UA code");
						Tools.LogException("RegisterEx3.LoadGoogleAnalytics/2",sql,null);
					}
				}
				catch (Exception ex)
				{
					Tools.LogInfo     ("RegisterEx3.LoadGoogleAnalytics/8","Failed to load Google UA code");
					Tools.LogException("RegisterEx3.LoadGoogleAnalytics/9",sql,ex);
				}
		}

		private void LoadChat()
		{
			lblChat.Text = "";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec sp_WP_Get_ChatSnip @ProductCode=" + Tools.DBString(productCode);
					if ( miscList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadChat",90010,"Internal database error (sp_WP_Get_ChatSnip)",sql);
					else if ( miscList.EOF )
						SetErrorDetail("LoadChat",90011,"Chat widget code cannot be found (sp_WP_Get_ChatSnip)",sql);
					else
						lblChat.Text = miscList.GetColumn("ChatSnippet");
				}
				catch (Exception ex)
				{
					SetErrorDetail("LoadChat",90099,"Internal error ; please try again later",ex.Message + " (" + sql + ")",2,2,ex);
				}
		}	

		private string CheckIP()
		{
			string action = "";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec sp_Check_IPLocation"
					    +     " @ProductCode=" + Tools.DBString(productCode)
					    +     ",@IPAddress="   + Tools.DBString(WebTools.ClientIPAddress(Request,1));
					if ( miscList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("CheckIP",91010,"Internal database error (sp_Check_IPLocation)",sql);
					else if ( miscList.EOF )
						SetErrorDetail("CheckIP",91011,"Country/IP address check failed (sp_Check_IPLocation)",sql);
					else if ( miscList.GetColumn("Result") == "1" )
						action = miscList.GetColumn("Action");
				}
				catch (Exception ex)
				{
				//	Tools.LogException("RegisterCheckIP",sql,ex);
					SetErrorDetail("CheckIP",91099,"Internal error ; please try again later",ex.Message + " (" + sql + ")",2,2,ex);
				}

			SetWarning(action);
			return action;
		}	

		private void LoadLabels()
		{
			byte logNo      = 5;
			hdn100187.Value = "X";

			using (MiscList miscList = new MiscList())
				try
				{
					HiddenField ctlHidden;
					Literal     ctlLiteral;
					string      fieldFail;
					string      fieldPass;
					string      fieldCode;
					string      fieldValue;
					string      fieldMessage;
					string      fieldBlocked;
					string      screenGuide;
					string      regPageNo;
					string      controlID;
					int         k;

//	Static labels, help text, etc
					errNo = 10;
					logNo = 10;
					sql   = "exec sp_WP_Get_ProductWebsiteRegContent @ProductCode="         + Tools.DBString(productCode)
					                                             + ",@LanguageCode="        + Tools.DBString(languageCode)
					                                             + ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					if ( miscList.ExecQuery(sql,0) == 0 )
						while ( ! miscList.EOF )
						{
							fieldCode    = miscList.GetColumn("WebsiteFieldCode");
							fieldValue   = miscList.GetColumn("WebsiteFieldValue");
							fieldMessage = miscList.GetColumn("WebsiteFieldiMessage"); // Yes, this is spelt correctly
							screenGuide  = miscList.GetColumn("WebsiteFieldScreenGuide");
							fieldFail    = miscList.GetColumn("FieldValidationFailureText");
							fieldPass    = miscList.GetColumn("FieldValidationPassText");
							regPageNo    = miscList.GetColumn("RegistrationPageNumber");
							fieldBlocked = miscList.GetColumn("Blocked");
							controlID    = "";
							logNo        = 15;
							errNo        = 0;
 
//							if ( logNo <= 10 )
//								Tools.LogInfo("RegisterEx3.LoadLabels/15","Row 1, FieldCode="+fieldCode+", FieldValue="+fieldValue,logDebug);

						//	Common
							if ( fieldCode == "100119" )      // Next
								btnNext.Text = fieldValue;
//							else if ( fieldCode == "100xxx" ) // Back ... this button only exists in DEV
//								btnBack1.Text = fieldValue;
							else if ( fieldCode == "100195" ) // I agree
								btnAgree.Text = fieldValue;
							else if ( fieldCode == "100194" ) // Change payment method
								btnBack2.Text = fieldValue;
							else if ( fieldCode == "100135" ) // Registration
								lblReg.Text = fieldValue;
							else if ( fieldCode == "100207" ) // Registration Confirmation
								lblRegConf.Text = fieldValue;

						//	PDF Stuff
							else if ( fieldCode == "100002" ) // Emergency mobile assistance, blah
								hdn100002.Value  = fieldValue;
							else if ( fieldCode == "100137" ) // Product name
								hdn100137.Value  = fieldValue;

						//	Page 6
							if ( regPageNo == "6" || regPageNo == "S" ) // Confirmation page
							{
								ctlLiteral = (Literal)FindControl("lbl"+fieldCode);
								if ( ctlLiteral != null )
									ctlLiteral.Text = fieldValue;
								else if ( fieldCode == "100502" )
									btn3d.Text = fieldValue;
							}

						//	Page 1
							else if ( fieldCode == "100111" ) controlID      = "Title";
							else if ( fieldCode == "100114" ) controlID      = "Surname";
							else if ( fieldCode == "100117" ) controlID      = "CellNo";
							else if ( fieldCode == "104397" ) lbl104397.Text = fieldValue;
							else if ( fieldCode == "104398" ) lbl104398.Text = fieldValue;

						//	Page 2
							else if ( fieldCode == "100112" ) controlID = "FirstName";
							else if ( fieldCode == "100116" ) controlID = "EMail";
							else if ( fieldCode == "100118" ) controlID = "ID";

						//	Page 3
							else if ( fieldCode == "100123" ) controlID = "Income";
							else if ( fieldCode == "100131" ) controlID = "Status";
							else if ( fieldCode == "100132" ) controlID = "PayDay";

						//	Page 4
							else if ( fieldCode == "100138" ) controlID = "Options";
							else if ( fieldCode == "100147" ) controlID = "Payment";
							else if ( fieldCode == "100144" ) controlID = "Terms";

						//	Page 5
							else if ( fieldCode == "100186" ) controlID = "CCName";
							else if ( fieldCode == "100188" ) controlID = "CCExpiry";
							else if ( fieldCode == "100189" ) controlID = "CCCVV";
							else if ( fieldCode == "100190" ) controlID = "CCDueDay";
							else if ( fieldCode == "100187" ) // Credit card number
							{
								controlID        = "CCNumber";
								hdn100187.Value  = miscList.GetColumn("ValidationLuhnTest").ToUpper();
							}
							else if ( fieldCode == "100107" )
							{
								lblSubHead1Label.Text = fieldValue;
								lblSubHead2Label.Text = fieldValue;
							}
							else if ( fieldCode == "100122" )
								lblSubHead3Label.Text = fieldValue;
							else if ( fieldCode == "100136" )
								lblSubHead4aLabel.Text = fieldValue;
							else if ( fieldCode == "100137" )
								lblSubHead4bLabel.Text = fieldValue;
							else if ( fieldCode == "100143" )
								lblSubHead4cLabel.Text = fieldValue;
							else if ( fieldCode == "100148" )
								lblSubHead4dLabel.Text = fieldValue;
							else if ( fieldCode == "100084" )
								lblSubHead5Label.Text = fieldValue;
//							else if ( fieldCode == "100207" )
//								lblSubHead6Label.Text = fieldValue;
//							else if ( fieldCode == "100191" )
//								lblMandateHead.Text = fieldValue;
//							else if ( fieldCode == "100192" )
//								lblMandateDetail.Text = fieldValue;

							logNo = 18;

							if ( controlID.Length < 1 )
								logNo = 20;

							else if ( fieldBlocked == "1" ) // Don't show this field
							{
								logNo = 21;
								HideControls(controlID);
							}
							else
							{
								logNo      = 23;
								ctlLiteral = (Literal)FindControl("lbl"+controlID+"Label");
								if ( ctlLiteral != null )
									ctlLiteral.Text = fieldValue;

								logNo      = 26;
								ctlHidden  = (HiddenField)FindControl("hdn"+controlID+"Help");
								if ( ctlHidden != null )
									ctlHidden.Value = fieldMessage;

								logNo      = 29;
								ctlHidden  = (HiddenField)FindControl("hdn"+controlID+"Error");
								if ( ctlHidden != null )
								{
									k = fieldFail.IndexOf("  ");
									if ( k > 0 )
										fieldFail    = fieldFail.Substring(0,k) + "<br /><br />" + fieldFail.Substring(k+2);
									ctlHidden.Value = fieldFail.Replace("  "," ");
								}

								logNo      = 32;
								ctlHidden  = (HiddenField)FindControl("hdn"+controlID+"Guide");
								if ( ctlHidden != null )
								{
									k = screenGuide.IndexOf("  ");
									if ( k > 0 )
										screenGuide  = screenGuide.Substring(0,k) + "<br /><br />" + screenGuide.Substring(k+2);
									ctlHidden.Value = screenGuide.Replace("  "," ");
								}
							}
							miscList.NextRow();
						}
					else
						lblJS.Text = WebTools.JavaScriptSource("TestSetup()",lblJS.Text,1);

					SetErrorDetail("LoadLabels",errNo,"Unable to load registration page labels and data",sql);

					logNo = 37;

//	Note : btnBack1 ("Back") is only for DEV, not LIVE. So no label data exists

					if ( btnNext.Text.Length  < 1 || btnBack2.Text.Length < 1 || btnAgree.Text.Length < 1 )
						Tools.LogInfo("RegisterEx3.LoadLabels/37","Unable to load some or all button labels ("
						             + btnNext.Text + "/" + btnBack2.Text + "/" + btnAgree.Text + ")");

					if ( btnNext.Text.Length  < 1 ) btnNext.Text  = "NEXT";
//					if ( btnBack1.Text.Length < 1 ) btnBack1.Text = "BACK";
					if ( btnBack2.Text.Length < 1 ) btnBack2.Text = "Change Payment Method";
					if ( btnAgree.Text.Length < 1 ) btnAgree.Text = "I AGREE";

//	Title
					sql   = "exec sp_WP_Get_Title"
					      +     " @LanguageCode="        + Tools.DBString(languageCode)
					      +     ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 40;
					errNo =  WebTools.ListBind(lstTitle,sql,null,"TitleCode","TitleDescription","","");
					SetErrorDetail("LoadLabels/40",errNo,"Unable to load titles",sql);

//	Employment Status
					sql   = "exec sp_WP_Get_EmploymentStatus"
					      +     " @LanguageCode="        + Tools.DBString(languageCode)
					      +     ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 50;
					errNo = WebTools.ListBind(lstStatus,sql,null,"EmploymentStatusCode","EmploymentStatusDescription");
					SetErrorDetail("LoadLabels/50",errNo,"Unable to load employment statuses",sql);

//	Pay Date
					sql   = "exec sp_WP_Get_PayDate"
					      +     " @LanguageCode="        + Tools.DBString(languageCode)
					      +     ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 60;
					errNo = WebTools.ListBind(lstPayDay,sql,null,"PayDateCode","PayDateDescription");
					SetErrorDetail("LoadLabels/60",errNo,"Unable to load pay dates",sql);

//	Product Option
//	Deferred to the load of page 4
//	//				sql   = "exec sp_WP_Get_ProductOption"
//					sql   = "exec sp_WP_Get_ProductOptionA"
//					      +     " @ProductCode="         + Tools.DBString(productCode)
//					      +     ",@LanguageCode="        + Tools.DBString(languageCode)
//					      +     ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode)
//					      +     ",@Income="              + hdnIncomeError.ToString();
//					logNo = 65;
//					errNo = errNo + WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");

//	But we need the details
					sql = "exec sp_WP_Get_WebsiteProductOptionA"
					    +     " @ProductOptionCode='0'" // Return ALL product options
					    +     ",@ProductCode="         + Tools.DBString(productCode)
					    +     ",@LanguageCode="        + Tools.DBString(languageCode)
					    +     ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					int    opt;
					string F = "";
					logNo    = 70;
					errNo    = miscList.ExecQuery(sql,0);
					SetErrorDetail("LoadLabels/70",errNo,"Unable to retrieve product option descriptions",sql);
					while ( ! miscList.EOF )
					{
						opt = Tools.StringToInt(miscList.GetColumn("ProductOptionCode"));
						if ( opt > 0 )
							F = F + "<input type='hidden' id='hdnOption" + opt.ToString() + "' value='" + miscList.GetColumn("FieldValue").Replace("'","`").Replace(Environment.NewLine,"<br />") + "' />";
						miscList.NextRow();
					}
					lblOptionDescriptions.Text = F;

//	Payment Method
					sql   = "exec sp_WP_Get_PaymentMethod"
					      +     " @ProductCode="         + Tools.DBString(productCode)
					      +     ",@LanguageCode="        + Tools.DBString(languageCode)
					      +     ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);
					logNo = 80;
					errNo = WebTools.ListBind(lstPayment,sql,null,"PaymentMethodCode","PaymentMethodDescription");
					SetErrorDetail("LoadLabels/80",errNo,"Unable to load payment methods",sql);
				}
				catch (Exception ex)
				{
					SetErrorDetail("LoadLabels",logNo,"Internal error ; please try again later",ex.Message + " (" + sql + ")",2,2,ex);
				}

			lstCCYear.Items.Clear();
			for ( int y = System.DateTime.Now.Year ; y < System.DateTime.Now.Year+15 ; y++ )
				lstCCYear.Items.Add(new ListItem(y.ToString(),y.ToString()));
			lstCCYear.SelectedIndex = 1;
		}

		private bool LoadContractCode()
		{
			contractCode                   = "";
			contractPIN                    = "";
			ViewState["ContractCode"]      = null;
			ViewState["ContractPIN"]       = null;
			ViewState["BureauCodeToken"]   = null;
			ViewState["BureauCodePayment"] = null;
			ViewState["PaymentURL"]        = null;
			ViewState["PaymentMID"]        = null;
			ViewState["PaymentKey"]        = null;
//			ViewState["TokenURL"]          = null;
			ViewState["TokenMID"]          = null;
			ViewState["TokenKey"]          = null;
			ViewState["PaymentCurrency"]   = null;

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationC"
					    +     " @RegistrationPage = 'Z'"
					    +     ",@WebsiteCode ="               + Tools.DBString(WebTools.RequestValueString(Request,"WC"))
					    +     ",@ProductCode ="               + Tools.DBString(productCode)
					    +     ",@LanguageCode ="              + Tools.DBString(languageCode)
					    +     ",@GoogleUtmSource ="           + Tools.DBString(WebTools.RequestValueString(Request,"GUS"))
					    +     ",@GoogleUtmMedium ="           + Tools.DBString(WebTools.RequestValueString(Request,"GUM"))
					    +     ",@GoogleUtmCampaign ="         + Tools.DBString(WebTools.RequestValueString(Request,"GUC"))
					    +     ",@GoogleUtmTerm ="             + Tools.DBString(WebTools.RequestValueString(Request,"GUT"))
					    +     ",@GoogleUtmContent ="          + Tools.DBString(WebTools.RequestValueString(Request,"GUN"))
					    +     ",@AdvertCode ="                + Tools.DBString(WebTools.RequestValueString(Request,"AC"))
					    +     ",@ClientIPAddress ="           + Tools.DBString(WebTools.ClientIPAddress(Request,1))
					    +     ",@ClientDevice ="              + Tools.DBString(WebTools.ClientBrowser(Request,hdnBrowser.Value))
//					    +     ",@ReferringURL ="              + Tools.DBString(hdnReferURL.Value)
					    +     ",@WebsiteVisitorCode ="        + Tools.DBString(WebTools.RequestValueString(Request,"WVC"))
					    +     ",@WebsiteVisitorSessionCode =" + Tools.DBString(WebTools.RequestValueString(Request,"WVSC"));
					if ( miscList.ExecQuery(sql,0) != 0 )
						SetErrorDetail("LoadContractCode",10013,"Error retrieving new contract details ; please try again later",sql);

					else if ( miscList.EOF )
						SetErrorDetail("LoadContractCode",10023,"Error retrieving new contract details ; please try again later",sql);

					else
					{
						contractCode = miscList.GetColumn("ContractCode");
						contractPIN  = miscList.GetColumn("PIN");
						string stat  = miscList.GetColumn("Status");
						if ( contractCode.Length > 0 && contractPIN.Length > 0 && ( stat == "0" || stat.Length == 0 ) )
						{
							ViewState["ContractCode"] = contractCode;
							ViewState["ContractPIN"]  = contractPIN;
							lblError.Text             = "";
							sql                       = "exec sp_WP_Get_ProductInfo @ProductCode=" + Tools.DBString(productCode);
							if ( miscList.ExecQuery(sql,0) != 0 )
								SetErrorDetail("LoadContractCode",10033,"Error retrieving product info ; please try again later",sql);
							else if ( miscList.EOF )
								SetErrorDetail("LoadContractCode",10043,"Error retrieving product info ; please try again later",sql);
							else
							{
								bureauCodeToken   = miscList.GetColumn("TokenBureauCode");
//								tokenURL          = miscList.GetColumn("TokenBureauURL");
								tokenMID          = miscList.GetColumn("TokenBureauUserName");
								tokenKey          = miscList.GetColumn("TokenBureauUserSaveKey");
								bureauCodePayment = miscList.GetColumn("3DsecBureauCode");
								paymentURL        = miscList.GetColumn("3DsecURL");
								paymentMID        = miscList.GetColumn("PaymentBureauUserPassword");
								paymentKey        = miscList.GetColumn("PaymentBureauUserSaveKey");
								paymentCurrency   = miscList.GetColumn("TransactionalCurrencyCode");
								paymentAmount     = "010"; // miscList.GetColumn("TransactionalAmount");

								if ( paymentURL.Length < 1 || paymentMID.Length < 1 || paymentKey.Length < 1 )
									Tools.LogInfo("RegisterEx3.LoadContractCode",sql+" ->"
									            +  " bureauCodeToken="  +bureauCodeToken
									            + ", bureauCodePayment="+bureauCodePayment
									            + ", tokenMID="         +tokenMID
									            + ", tokenKey="         +tokenKey
									            + ", paymentURL="       +paymentURL
									            + ", paymentMID="       +paymentMID
									            + ", paymentKey="       +paymentKey
									            + ", paymentCurrency="  +paymentCurrency,224);
//	TESTING
								if ( ! Tools.SystemIsLive() )
								{
									bureauCodeToken = Tools.BureauCode(Constants.PaymentProvider.TokenEx);
									if ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
									{
//	TokenEx
										tokenMID = "4311038889209736";
										tokenKey = "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
									}
//	Peach
									bureauCodePayment = Tools.BureauCode(Constants.PaymentProvider.Peach);
//									paymentURL        = "https://test.oppwa.com/v1";
									paymentMID        = "8ac7a4c772b77ddf0172b7ed1cd206df";
									paymentKey        = "OGFjN2E0Yzc3MmI3N2RkZjAxNzJiN2VkMDFmODA2YTF8akE0aEVaOG5ZQQ==";
//	Transaction
									paymentCurrency   = "ZAR";
//	TESTING
								}

								ViewState["BureauCodeToken"]   = bureauCodeToken;
								ViewState["BureauCodePayment"] = bureauCodePayment;
//								ViewState["TokenURL"]          = tokenURL;
								ViewState["TokenMID"]          = tokenMID;
								ViewState["TokenKey"]          = tokenKey;
								ViewState["PaymentURL"]        = paymentURL;
								ViewState["PaymentMID"]        = paymentMID;
								ViewState["PaymentKey"]        = paymentKey;
								ViewState["PaymentCurrency"]   = paymentCurrency;
							}
						}
					}
				}
				catch (Exception ex)
				{
					SetErrorDetail("LoadContractCode",10093,"Error retrieving contract or product details ; please try again later",ex.Message + " (" + sql + ")",2,2,ex);
					return false;
				}
				return ( lblError.Text.Length == 0 );
		}

		private int ValidateData()
		{
			string err = "";
			if ( pageNo == 1 )
			{
				txtSurname.Text = txtSurname.Text.Trim();
				if ( txtSurname.Visible && txtSurname.Text.Length < 2 )
					err = err + "Invalid surname (at least 2 characters required)<br />";
				txtCellNo.Text = txtCellNo.Text.Trim();
				if ( txtCellNo.Visible && txtCellNo.Text.Length < 8 )
					err = err + "Invalid cell number (at least 8 digits required)<br />";
			}
			else if ( pageNo == 2 )
			{
				txtFirstName.Text = txtFirstName.Text.Trim();
				if ( txtFirstName.Visible && txtFirstName.Text.Length < 1 )
					err = err + "Invalid first name (at least 1 character required)<br />";
				txtEMail.Text = txtEMail.Text.Trim();
				if ( txtEMail.Visible && ! Tools.CheckEMail(txtEMail.Text) )
					err = err + "Invalid email address<br />";
				txtID.Text = txtID.Text.Trim();
				if ( txtID.Visible && txtID.Text.Length < 3 )
					err = err + "Invalid id number<br />";
			}
			else if ( pageNo == 3 )
			{
				int income = Tools.StringToInt(txtIncome.Text,true);
				if ( txtIncome.Visible && income < 100 )
					err = err + "Invalid income (it must be numeric and more than 100)<br />";
				else
					txtIncome.Text = income.ToString();
			}
			else if ( pageNo == 4 )
			{ }
			else if ( pageNo == 5 )
			{
				if ( bureauCodeToken != Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
				{
					txtCCNumber.Text = txtCCNumber.Text.Trim();
					if ( txtCCNumber.Visible && txtCCNumber.Text.Length < 12 )
						err = err + "Invalid credit/debit card number<br />";
				}
				else if ( txToken.Value.Trim().Length < 12 )
					err = err + "Invalid credit/debit card number [Token]<br />";
				txtCCName.Text = txtCCName.Text.Trim();
				if ( txtCCName.Visible && txtCCName.Text.Length < 3 )
					err = err + "Invalid credit/debit card name<br />";
				txtCCCVV.Text = txtCCCVV.Text.Trim();
				if ( txtCCCVV.Visible && txtCCCVV.Text.Length < 3 )
					err = err + "Invalid credit/debit card CVV code<br />";
				try
				{
					DateTime dt = new DateTime(WebTools.ListValue(lstCCYear),WebTools.ListValue(lstCCMonth),1,0,0,0);
					if ( System.DateTime.Now >= dt.AddMonths(1) )
						err = err + "Invalid credit/debit card expiry date<br />";
				}		
				catch
				{
					err = err + "Invalid credit/debit card expiry date<br />";
				}
			}

			if ( err.Length > 0 )
				SetErrorDetail("ValidateData",20022,err,err,1,1,null,false,50);
			return err.Length;
		}

		protected void btn3d_Click(Object sender, EventArgs e)
		{
//			Payment to Peach via TokenEx if we only have a token
			if ( paymentMID.Length < 1 || paymentKey.Length < 1 )
			{
				string x = "paymentMID="+paymentMID + ", paymentKey="+paymentKey + ", paymentURL=" + paymentURL;
				SetErrorDetail("btn3d_Click",26010,"Invalid payment provider MID/URL/Key",x,2,2,null,false,239);
				return;
			}

			if ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
				if ( tokenMID.Length < 1 || tokenKey.Length < 1 )
				{
					string x = "tokenMID="+tokenMID + ", tokenKey="+tokenKey;
					SetErrorDetail("btn3d_Click",26020,"Invalid token provider MID/Key",x,2,2,null,false,239);
					return;
				}

			using (Payment payment = new Payment())
			{
			//	Peach
				payment.BureauCode         = Tools.BureauCode(Constants.PaymentProvider.Peach);
				payment.ProviderUserID     = paymentMID;
				payment.ProviderKey        = paymentKey;
				payment.ProviderURL        = paymentURL;
			//	TokenEx ... might not be needed
				payment.TokenizerCode      = bureauCodeToken;
				payment.TokenizerID        = tokenMID;
				payment.TokenizerKey       = tokenKey;
			//	payment.TokenizerURL       = tokenURL;
			//	Customer
				payment.CardNumber         = txtCCNumber.Text;
				payment.CardToken          = txToken.Value;
				payment.CardName           = txtCCName.Text;
				payment.CardExpiryMM       = WebTools.ListValue(lstCCMonth).ToString();
				payment.CardExpiryYYYY     = WebTools.ListValue(lstCCYear).ToString();
				payment.CardCVV            = txtCCCVV.Text;
				payment.MerchantReference  = contractCode;
				payment.PaymentDescription = "Prosperian Initial Payment";
				payment.CurrencyCode       = paymentCurrency;
				payment.PaymentAmount      = Tools.StringToInt(paymentAmount);
				if ( payment.PaymentAmount < 1 )
					payment.PaymentAmount   = 031; // 31 cents

				using (TransactionPeach trans = new TransactionPeach())
					if (trans.ThreeDSecurePayment(payment) == 0)
						try
						{
							//	Ver 1
							//	lbl3d.Text = trans.ThreeDSecureHTML;
							//	Ver 2
							//	This always throws a "thread aborted" exception ... ignore it
							System.Web.HttpContext.Current.Response.Clear();
							System.Web.HttpContext.Current.Response.Write(trans.ThreeDSecureHTML);
							System.Web.HttpContext.Current.Response.End();
						}
						catch
						{ }
					else
						Response.Redirect("RegisterThreeD.aspx?TransRef=" + contractCode.ToString() + "&id=" + trans.PaymentReference);
			}
		}
		protected void btn3d_ClickPeachDirect(Object sender, EventArgs e)
		{
//			Straight payment to Peach if we have the card number
			if ( paymentMID.Length < 1 || paymentKey.Length < 1 )
			{
				string x = "paymentMID="+paymentMID + ", paymentKey="+paymentKey + ", paymentURL=" + paymentURL;
				SetErrorDetail("btn3d_Click",24010,"Invalid payment provider MID/URL/Key",x,2,2,null,false,239);
				return;
			}

			using (Payment payment = new Payment())
			{
			//	Peach
				payment.BureauCode         = Tools.BureauCode(Constants.PaymentProvider.Peach);
				payment.ProviderUserID     = paymentMID;
				payment.ProviderKey        = paymentKey;
				payment.ProviderURL        = paymentURL;
				payment.TokenizerCode      = "";
			//	Customer
				payment.CardNumber         = txtCCNumber.Text;
			//	payment.CardToken          = 
				payment.CardName           = txtCCName.Text;
				payment.CardExpiryMM       = WebTools.ListValue(lstCCMonth).ToString();
				payment.CardExpiryYYYY     = WebTools.ListValue(lstCCYear).ToString();
				payment.CardCVV            = txtCCCVV.Text;
				payment.MerchantReference  = contractCode;
				payment.PaymentDescription = "Prosperian Initial Payment";
				payment.CurrencyCode       = paymentCurrency;
				payment.PaymentAmount      = Tools.StringToInt(paymentAmount);
				if ( payment.PaymentAmount < 1 )
					payment.PaymentAmount   = 031; // 31 cents

				using (TransactionPeach trans = new TransactionPeach())
					if ( trans.ThreeDSecurePayment(payment) == 0 )
						try
						{
						//	Ver 1
						//	lbl3d.Text = trans.ThreeDSecureHTML;
						//	Ver 2
						//	This always throws a "thread aborted" exception ... ignore it
							System.Web.HttpContext.Current.Response.Clear();
							System.Web.HttpContext.Current.Response.Write(trans.ThreeDSecureHTML);
							Context.ApplicationInstance.CompleteRequest();
						//	System.Web.HttpContext.Current.Response.End();
						}
						catch
						{ }
					else
						Response.Redirect("RegisterThreeD.aspx?TransRef="+contractCode.ToString()+"&id="+trans.PaymentReference);
			}
		}

		protected void btnNext_Click(Object sender, EventArgs e)
		{
			int pdfErr = 0;
			errNo      = 0;

			if ( pageNo < 1 )
			{
				SetErrorDetail("btnNext_Click/30010",30010,"Page numbering error","The internal page number is " + pageNo.ToString());
				return;
			}
			if ( pageNo > 180 ) // Testing
			{
				contractCode      = "20200831/0014";
				txtSurname.Text   = "Smith";
				txtFirstName.Text = "Peter";
				txtEMail.Text     = "PaulKilfoil@gmail.com";
				txtIncome.Text    = "125000";
				txToken.Value     = "4111118034721111";
				txtCCNumber.Text  = "4111111111111111";
				txtCCCVV.Text     = "789";
			}

			if ( ValidateData() > 0 )
				return;

			string mailText = "";

			using (MiscList miscList = new MiscList())
				try
				{
					sql = "exec WP_ContractApplicationC"
					    +     " @RegistrationPage =" + Tools.DBString((pageNo-1).ToString())
					    +     ",@ContractCode ="     + Tools.DBString(contractCode)
					    +     ",@ClientIPAddress ="  + Tools.DBString(WebTools.ClientIPAddress(Request,1))
					    +     ",@ClientDevice ="     + Tools.DBString(WebTools.ClientBrowser(Request,hdnBrowser.Value));

//					if ( Tools.LiveTestOrDev() == Constants.SystemMode.Development )
					if ( Tools.SystemViaBackDoor() )
					{ }

					else if ( pageNo == 1 )
						sql = sql + ",@TitleCode ="        + Tools.DBString(WebTools.ListValue(lstTitle,""))
					             + ",@Surname ="          + Tools.DBString(txtSurname.Text,47) // Unicode
					             + ",@TelephoneNumberM =" + Tools.DBString(txtCellNo.Text,47);
					else if ( pageNo == 2 )
						sql = sql + ",@FirstName ="    + Tools.DBString(txtFirstName.Text,47)
					             + ",@EMailAddress =" + Tools.DBString(txtEMail.Text,47)
					             + ",@ClientCode ="   + Tools.DBString(txtID.Text,47);
					else if ( pageNo == 3 )
						sql = sql + ",@DisposableIncome ="           + Tools.DBString(txtIncome.Text,47)
					             + ",@ClientEmploymentStatusCode =" + Tools.DBString(WebTools.ListValue(lstStatus,""))
					             + ",@PayDateCode ="                + Tools.DBString(WebTools.ListValue(lstPayDay,""));
					else if ( pageNo == 4 )
						sql = sql + ",@ProductOptionCode =" + Tools.DBString(WebTools.ListValue(lstOptions,""))
					             + ",@TsCsRead = '1'"
					             + ",@PaymentMethodCode =" + Tools.DBString(WebTools.ListValue(lstPayment,""));
					else if ( pageNo == 5 )
						sql = sql + ",@CardNumber ="      + Tools.DBString(txtCCNumber.Text,47) // Will be blank if TokenEx is ON
					             + ",@CardCVVCode ="     + Tools.DBString(txtCCCVV.Text,47)
					             + ",@AccountHolder ="   + Tools.DBString(txtCCName.Text,47)
					             + ",@CardExpiryMonth =" + Tools.DBString(WebTools.ListValue(lstCCMonth).ToString())
					             + ",@CardExpiryYear ="  + Tools.DBString(WebTools.ListValue(lstCCYear).ToString());

					errNo = miscList.ExecQuery(sql,0);
					SetErrorDetail("btnNext_Click/30020",errNo,"Unable to update information (pageNo="+pageNo.ToString()+")",sql);

					if ( errNo == 0 || pageNo > 180 )
					{
						pageNo++;

						if ( pageNo == 4 )
						{
							int h = Tools.StringToInt(txtIncome.Text,true);
							sql   = "exec sp_WP_Get_ProductOptionA"
							      +     " @ProductCode ="         + Tools.DBString(productCode)
							      +     ",@LanguageCode ="        + Tools.DBString(languageCode)
							      +     ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode)
							      +     ",@Income ="              + h.ToString();
							errNo = WebTools.ListBind(lstOptions,sql,null,"ProductOptionCode","ProductOptionDescription");
							SetErrorDetail("btnNext_Click/30030",errNo,"Unable to obtain product options",sql);
						}
						else if ( pageNo == 5 )
						{
//	TokenEx Start
							if ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
							{
//	[TESTING]
//	iFrame API key = njSRwZVKotSSbDAZtLBIXYrCznNUx2oOZFMVZp7I
//	Mobile API key = bDjxBnxQFfv7mrFtPJA24sGGbNBYvUF7JMnlNjwq
//	TokenEx Id     = 4311038889209736
//	[TESTING]
								string uThis        = Request.Url.GetLeftPart(UriPartial.Authority);
								string uRefer       = Request.UrlReferrer.GetLeftPart(UriPartial.Authority);
								if ( uRefer.Length  > 0 && uRefer.ToUpper() != uThis.ToUpper() )
									uThis            = uRefer + "," + uThis; // Referring URL MUST be first in the list
								txOrigin.Value      = uThis;
//								txOrigin.Value      = "https://www.eservsecure.com," + Request.Url.GetLeftPart(UriPartial.Authority);
								string apiKey       = Tools.ConfigValue("TokenEx/Key");
								txID.Value          = Tools.ConfigValue("TokenEx/Id");
								txTimestamp.Value   = Tools.DateToString(System.DateTime.Now,5,2).Replace(" ","");
//								txTokenScheme.Value = "sixTOKENfour";
								string data         = txID.Value + "|" + txOrigin.Value + "|" + txTimestamp.Value + "|" + txTokenScheme.Value;
								txHMAC.Value        = Tools.GenerateHMAC(data,apiKey);
								lblTx.Text          = WebTools.JavaScriptSource("TokenSetup()");
//	[TESTING]
//								txConcatenatedString.Value = data;
								Tools.LogInfo("RegisterEx3.btnNext_Click/30035","Id="+txID.Value
								                                             +"; Key="+apiKey
								                                             +"; Origin="+txOrigin.Value
								                                             +"; TimeStamp="+txTimestamp.Value
								                                             +"; TokenScheme="+txTokenScheme.Value
								                                             +"; Concat="+data
								                                             +"; HMAC="+txHMAC.Value,233);
//	[TESTING]
							}
//	TokenEx End
							string productOption  = WebTools.ListValue(lstOptions,"X");
							string payMethod      = WebTools.ListValue(lstPayment,"X");
							txtCCName.Text        = "";

							if ( txtFirstName.Text.Length > 0 && txtSurname.Text.Length > 1 )
								txtCCName.Text     = txtFirstName.Text.Substring(0,1).ToUpper()
								                   + " "
								                   + txtSurname.Text.Substring(0,1).ToUpper()
								                   + txtSurname.Text.Substring(1).ToLower();
							else if ( txtSurname.Text.Length > 1 )
								txtCCName.Text     = txtSurname.Text.Substring(0,1).ToUpper()
								                   + txtSurname.Text.Substring(1).ToLower();
							else if ( txtFirstName.Text.Length > 1 )
								txtCCName.Text     = txtFirstName.Text.Substring(0,1).ToUpper()
								                   + txtFirstName.Text.Substring(1).ToLower();

							lblCCDueDate.Text     = lstPayDay.SelectedItem.Text;
							lblCCMandate.Text     = "";
							lblCCMandateHead.Text = "";
//							lblCCMandateHead.Text = "Collection Mandate: " + Tools.DateToString(System.DateTime.Now,2);
							sql                   = "exec sp_WP_Get_ProductOptionMandateA"
							                      +     " @ProductCode ="         + Tools.DBString(productCode)
							                      +     ",@LanguageCode ="        + Tools.DBString(languageCode)
							                      +     ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode);

							string w = " (looking for ProductOption="+productOption+" and PaymentMethod="+payMethod+")";
							Tools.LogInfo("RegisterEx3.btnNext_Click/30040",sql+w,logDebug);

							if ( miscList.ExecQuery(sql,0) == 0 )
								while ( ! miscList.EOF )
								{
									w = "ProductOptionCode=" + miscList.GetColumn("ProductOptionCode") +
									  ", PaymentMethodCode=" + miscList.GetColumn("PaymentMethodCode");
									if ( ( miscList.GetColumn("ProductOptionCode").ToUpper() == productOption.ToUpper()  &&
									       miscList.GetColumn("PaymentMethodCode").ToUpper() == payMethod.ToUpper()  )   ||
									       miscList.GetColumn("PaymentMethodCode") == "*" )
									{
										Tools.LogInfo("RegisterEx3.btnNext_Click/30045",w+" (match)",logDebug);
										lblCCMandate.Text = miscList.GetColumn("CollectionMandateText",0);
										int k             = lblCCMandate.Text.IndexOf("\n"); // Do NOT use Environment.NewLine here!
										if ( k > 0 && lblCCMandate.Text.Length > k+1 )
										{
											lblCCMandateHead.Text = lblCCMandate.Text.Substring(0,k);
											lblCCMandate.Text     = lblCCMandate.Text.Substring(k+1).Replace(Environment.NewLine,"<br />");
										}
										lblp6MandateHead.Text = lblCCMandateHead.Text;
										lblp6Mandate.Text     = lblCCMandate.Text;
										break;
									}
									Tools.LogInfo("RegisterEx3.btnNext_Click/30050",w,logDebug);
									miscList.NextRow();
								}

							if ( lblCCMandate.Text.Length < 1 )
								SetErrorDetail("btnNext_Click/30060",30045,"Unable to retrieve collection mandate",sql+" (looking for ProductOption="+productOption+" and PaymentMethod="+payMethod+"). SQL failed or returned no data or<br />the CollectionMandateText column was missing/empty/NULL");
						}
						else if ( pageNo == 6 || pageNo > 180 )
						{
							sql   = "exec WP_ContractApplicationC"
							      +     " @RegistrationPage = '5'"
							      +     ",@ContractCode =" + Tools.DBString(contractCode);
							errNo = miscList.ExecQuery(sql,0,"",false,true);
							SetErrorDetail("btnNext_Click/30065",(errNo==0?0:30065),"Unable to update information (WP_ContractApplicationC)",sql);

							if ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
							{
								sql   = "exec sp_TokenEx_Ins_CardToken"
								      +     " @ContractCode ="       + Tools.DBString(contractCode)
								      +     ",@MaskedCardNumber ="   + Tools.DBString(Tools.MaskCardNumber(txToken.Value))
								      +     ",@PaymentBureauCode ="  + Tools.DBString(Tools.BureauCode(Constants.PaymentProvider.TokenEx))
								      +     ",@PaymentBureauToken =" + Tools.DBString(txToken.Value)
								      +     ",@CardExpieryMonth ="   + Tools.DBString(WebTools.ListValue(lstCCMonth).ToString())
								      +     ",@CardExpieryYear ="    + Tools.DBString(WebTools.ListValue(lstCCYear).ToString())
								      +     ",@CardCVV ="            + Tools.DBString(txtCCCVV.Text)
								      +     ",@ReferenceNumber ="    + Tools.DBString(txReference.Value,47)
								      +     ",@TransactionStatusCode = ''"
								      +     ",@CardTokenisationStatusCode = '007'";
								errNo = miscList.ExecQuery(sql,0,"",false,true);
								SetErrorDetail("btnNext_Click/30070",(errNo==0?0:30070),"Unable to update card token (sp_TokenEx_Ins_CardToken)",sql);
							}
							lbl100325.Text = "";
							sql = "exec sp_WP_Get_WebsiteProductOptionA"
							    +     " @ProductCode ="         + Tools.DBString(productCode)
							    +     ",@ProductOptionCode ="   + Tools.DBString(WebTools.ListValue(lstOptions,""))
							    +     ",@LanguageCode ="        + Tools.DBString(languageCode)
							    +     ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode);
							if ( miscList.ExecQuery(sql,0) != 0 )
								SetErrorDetail("btnNext_Click/30060",30080,"Internal database error (sp_WP_Get_WebsiteProductOptionA)",sql);
							else if ( miscList.EOF )
								SetErrorDetail("btnNext_Click/30061",30081,"No product option data returned (sp_WP_Get_WebsiteProductOptionA)",sql);
							else
							{
								lbl100325.Text = miscList.GetColumn("FieldValue");
								if ( lbl100325.Text.Length > 0 )
									lbl100325.Text = lbl100325.Text.Replace(Environment.NewLine,"<br />");
								else
									SetErrorDetail("btnNext_Click/30062",30082,"Product option data is empty/blank (sp_WP_Get_WebsiteProductOptionA, column 'FieldValue')",sql,2,2);
							}

							string refundPolicy       = "";
							string moneyBackPolicy    = "";
							string cancellationPolicy = "";
							lblp6CCType.Text          = "";

							sql = "exec sp_WP_Get_ProductPolicy"
							    +     " @ProductCode ="         + Tools.DBString(productCode)
							    +     ",@LanguageCode ="        + Tools.DBString(languageCode)
							    +     ",@LanguageDialectCode =" + Tools.DBString(languageDialectCode);
							if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
							{
								refundPolicy                 = miscList.GetColumn("RefundPolicyText");
								moneyBackPolicy              = miscList.GetColumn("MoneyBackPolicyText");
								cancellationPolicy           = miscList.GetColumn("CancellationPolicyText");
								lblp6RefundPolicy.Text       = refundPolicy.Replace(Environment.NewLine,"<br />") + "<br />&nbsp;";
								lblp6MoneyBackPolicy.Text    = moneyBackPolicy.Replace(Environment.NewLine,"<br />") + "<br />&nbsp;";
								lblp6CancellationPolicy.Text = cancellationPolicy.Replace(Environment.NewLine,"<br />");
							}
							if ( refundPolicy.Length < 1 || moneyBackPolicy.Length < 1 || cancellationPolicy.Length < 1 )
								SetErrorDetail("btnNext_Click/30085",30080,"Unable to retrieve product policy text",sql);

							sql = ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) ? txToken.Value : txtCCNumber.Text ).Trim(); // Tokenized as SIXxxxxxxFOUR
							if ( sql.Length > 6 )
								sql = sql.Substring(0,6);
							sql = "exec WP_Get_CardAssociation @BIN=" + Tools.DBString(sql);
							if ( miscList.ExecQuery(sql,0) == 0 && ! miscList.EOF )
								lblp6CCType.Text = miscList.GetColumn("Brand");
							if ( lblp6CCType.Text.Length < 1 )
								SetErrorDetail("btnNext_Click/30090",30090,"Unable to retrieve card association details",sql);

							if ( lblError.Text.Length > 0 )
								throw new Exception("XYZ");

//	Confirmation Page

							try
							{
								mailText = File.ReadAllText(Tools.SystemFolder("Templates")+"ConfirmationMail.htm");
							}
							catch (Exception ex1)
							{
								mailText = "";
								SetErrorDetail("btnNext_Click/30095",30095,"Unable to open mail template (Templates/ConfirmationMail.htm)",ex1.Message);
							}

							lblReg.Visible      = false;
							lblRegConf.Visible  = true;
							lblp6Ref.Text       = contractCode;
							lblp6Pin.Text       = contractPIN;
							lblp6Title.Text     = lstTitle.SelectedItem.Text;
							lblp6FirstName.Text = txtFirstName.Text;
							lblp6Surname.Text   = txtSurname.Text;
							lblp6EMail.Text     = txtEMail.Text;
							lblp6CellNo.Text    = txtCellNo.Text;
							lblp6ID.Text        = txtID.Text;
							lblp6Income.Text    = txtIncome.Text;
							lblp6Status.Text    = lstStatus.SelectedItem.Text;
							lblp6PayDay.Text    = lstPayDay.SelectedItem.Text;
//							lblp6Option.Text    = hdnOption.Value;
							lblp6Payment.Text   = lstPayment.SelectedItem.Text;
							lbl100209.Text      = lbl100209.Text.Replace("[Title]",lstTitle.SelectedItem.Text).Replace("[Surname]",txtSurname.Text);
							if ( txtFirstName.Text.Length > 0 )
								lbl100209.Text   = lbl100209.Text.Replace("[Initials]",txtFirstName.Text.Substring(0,1).ToUpper());
							else
								lbl100209.Text   = lbl100209.Text.Replace("[Initials]","");
							lblp6CCName.Text    = txtCCName.Text;
							lblp6CCExpiry.Text  = lstCCYear.SelectedValue + "/" + lstCCMonth.SelectedValue;
							lblp6Date.Text      = Tools.DateToString(System.DateTime.Now,2,1);
							lblp6IP.Text        = WebTools.ClientIPAddress(Request,1);
							lblp6Browser.Text   = WebTools.ClientBrowser(Request,hdnBrowser.Value);
							if ( bureauCodeToken == Tools.BureauCode(Constants.PaymentProvider.TokenEx) )
								lblp6CCNumber.Text = Tools.MaskCardNumber(txToken.Value);
							else
								lblp6CCNumber.Text = Tools.MaskCardNumber(txtCCNumber.Text);

							foreach (Control ctlOuter in Page.Controls)
								if ( ctlOuter.GetType() == typeof(Literal) && mailText.Contains("#"+ctlOuter.ID+"#") )
									mailText = mailText.Replace("#"+ctlOuter.ID+"#",((Literal)ctlOuter).Text);
								else if ( ctlOuter.GetType() == typeof(Label) && mailText.Contains("#"+ctlOuter.ID+"#") )
									mailText = mailText.Replace("#"+ctlOuter.ID+"#",((Label)ctlOuter).Text);
								else
									foreach (Control ctlInner in ctlOuter.Controls)
										if ( ctlInner.GetType() == typeof(Literal) && mailText.Contains("#"+ctlInner.ID+"#") )
											mailText = mailText.Replace("#"+ctlInner.ID+"#",((Literal)ctlInner).Text);
										else if ( ctlInner.GetType() == typeof(Label) && mailText.Contains("#"+ctlInner.ID+"#") )
											mailText = mailText.Replace("#"+ctlInner.ID+"#",((Label)ctlInner).Text);
										else
											foreach (Control ctlDeep in ctlInner.Controls)
												if ( ctlDeep.GetType() == typeof(Literal) && mailText.Contains("#"+ctlDeep.ID+"#") )
													mailText = mailText.Replace("#"+ctlDeep.ID+"#",((Literal)ctlDeep).Text);
												else if ( ctlDeep.GetType() == typeof(Label) && mailText.Contains("#"+ctlDeep.ID+"#") )
													mailText = mailText.Replace("#"+ctlDeep.ID+"#",((Label)ctlDeep).Text);

							try
							{
								File.AppendAllText(Tools.SystemFolder("Contracts")+contractCode+".htm",mailText,Encoding.UTF8);
							}
							catch (Exception ex6)
							{
								SetErrorDetail("btnNext_Click/30105",30105,"Unable to create confirmation file ("+contractCode+".htm)",ex6.Message);
							}

//	Testing
//							if ( lblp6MandateHead.Text.Length < 1 )
//								lblp6MandateHead.Text = "Collection Mandate: " + Tools.DateToString(System.DateTime.Now,2,0);
//							if ( lblp6Mandate.Text.Length < 1 )
//								lblp6Mandate.Text     = "You hereby authorise and instruct us to collect all money due by you from your Card listed above or any other card that you may indicate from time to time";
//							if ( lblp6Billing.Text.Length < 1 )
//								lblp6Billing.Text     = "We confirm that we have received the above Billing Information as submitted by you";
//	Testing

							errNo = 30200;
							sql   = "exec sp_WP_Get_ProductEmail"
							      +     " @ProductCode="  + Tools.DBString(productCode)
							      +     ",@LanguageCode=" + Tools.DBString(languageCode);
							if ( miscList.ExecQuery(sql,0) == 0 )
								if ( miscList.EOF )
									errNo = 30210;
								else
									try								
									{
										errNo               = 30215;
										string err          = "";
										string emailFrom    = miscList.GetColumn("SenderEmailAddress");
										string emailReply   = miscList.GetColumn("ReplyEMailAddress");
										string emailRoute1  = miscList.GetColumn("Route1EMailAddress",0);
										string emailRoute2  = miscList.GetColumn("Route2EMailAddress",0);
										string emailRoute3  = miscList.GetColumn("Route3EMailAddress",0);
										string emailRoute4  = miscList.GetColumn("Route4EMailAddress",0);
										string emailRoute5  = miscList.GetColumn("Route5EMailAddress",0);
//										string emailHeader  = miscList.GetColumn("EMailHeaderText");
// Testing							string emailHeader  = miscList.GetColumn("EMailHeaderTextENG");
//										string emailBody    = miscList.GetColumn("EMailBodyText");
//	Testing							string emailBody    = miscList.GetColumn("EMailBodyTextENG");

//	NO! Allow sender to be invalid
//										if ( ! Tools.CheckEMail(emailFrom) )
//										{
//											errNo = 30220;
//											err   = "Invalid sender email (" + emailFrom + ")";
//											throw new Exception(err);
//										}

										errNo               = 30225;
										string smtpServer   = Tools.ConfigValue("SMTP-Server");
										string smtpUser     = Tools.ConfigValue("SMTP-User");
										string smtpPassword = Tools.ConfigValue("SMTP-Password");
										string smtpBCC      = Tools.ConfigValue("SMTP-BCC");
										int    smtpPort     = Tools.StringToInt(Tools.ConfigValue("SMTP-Port"));

										if ( smtpServer.Length < 3 || smtpUser.Length < 3 || smtpPassword.Length < 3 )
										{
											errNo = 30230;
											err   = "Invalid SMTP details, server=" + smtpServer + ", user=" + smtpUser + ", pwd=" + smtpPassword + ", port=" + smtpPort.ToString();
											throw new Exception(err);
										}

//	If sender is invalid, use the SMTP address
										if ( ! Tools.CheckEMail(emailFrom) )
											emailFrom = smtpUser;

										errNo                      = 30235;
										SmtpClient smtp            = new SmtpClient(smtpServer);
										smtp.Credentials           = new System.Net.NetworkCredential(smtpUser,smtpPassword);
										if ( smtpPort > 0 )
											smtp.Port               = smtpPort;
//										smtp.UseDefaultCredentials = false;
//										smtp.EnableSsl             = true;

										using (MailMessage mailMsg = new MailMessage())
										{
											errNo = 30240;
											mailMsg.To.Add(txtEMail.Text);
											if ( Tools.CheckEMail(emailReply) )
												mailMsg.ReplyToList.Add(emailReply);
											if ( Tools.CheckEMail(emailRoute1) )
												mailMsg.Bcc.Add(emailRoute1);
											if ( Tools.CheckEMail(emailRoute2) )
												mailMsg.Bcc.Add(emailRoute2);
											if ( Tools.CheckEMail(emailRoute3) )
												mailMsg.Bcc.Add(emailRoute3);
											if ( Tools.CheckEMail(emailRoute4) )
												mailMsg.Bcc.Add(emailRoute4);
											if ( Tools.CheckEMail(emailRoute5) )
												mailMsg.Bcc.Add(emailRoute5);
											if ( Tools.CheckEMail(smtpBCC) )
												mailMsg.Bcc.Add(smtpBCC);

											errNo                = 30245;
											if ( Tools.CheckEMail(smtpUser) )
												mailMsg.Sender    = new MailAddress(smtpUser);
											mailMsg.From         = new MailAddress(emailFrom);
											mailMsg.Subject      = "Contract " + contractCode;
											mailMsg.BodyEncoding = Encoding.UTF8;
											mailMsg.Body         = mailText;
											mailMsg.IsBodyHtml   = mailText.ToUpper().Contains("<HTML");
//											errNo                = 30250;
//	Do NOT send PDF					if ( pdfFileName.Length > 0 )
//												mailMsg.Attachments.Add(new Attachment(pdfFileName));
											errNo                = 30255;

											for ( int k = 0 ; k < 5 ; k++ )
												try
												{
													if ( Tools.ConfigValue("SMTP-Mode") != "29" ) // Do not send email
														smtp.Send(mailMsg);
													errNo = 0;
													break;
												}
												catch (Exception ex3)
												{
													if ( Tools.SystemViaBackDoor() ) // Email fails, no problem
													{
														errNo = 0;
														break;
													}
													if ( k > 1 ) // After 2 failed attempts
														smtp.UseDefaultCredentials = false;
													if ( k > 2 ) // After 3 failed attempts
														Tools.LogException("RegisterEx3.aspx/84","Mail send failure, errNo=" + errNo.ToString() + " (" + txtEMail.Text+")",ex3);
												}
										}
										SetErrorDetail("btnNext_Click/30205",errNo,"Unable to send confirmation email (5 failed attempts)","");
										smtp  = null;
										errNo = 0;
									}
									catch (Exception ex4)
									{
										SetErrorDetail("btnNext_Click/30210",errNo,"Unable to send confirmation email (SMTP error)",ex4.Message,2,2,ex4);
										errNo = 0;
									}

							SetErrorDetail("btnNext_Click/30215",errNo,"Unable to send confirmation email (SQL error)",sql);
							pnl3d.Visible = true;

//	This does not postback but does a redirect. ViewState is lost!
//							lblRefresh.Text = "<meta http-equiv=\"refresh\" content=\"5;URL='RegisterEx3.aspx?PageNo=43'\" />";
//	This works, but better to put it in the page itself
//							lblJS.Text = "<script type='text/javascript'>setTimeout(function(){GetElt('btn3d').click()},10000)</script>";
						}
					}
				}
				catch (Exception ex5)
				{
					if ( ex5.Message != "XYZ" )
						SetErrorDetail("btnNext_Click/30320",errNo,"Internal database error ; please try again later",ex5.Message,2,2,ex5);
				}

			if ( lblError.Text.Length == 0 && errNo + pdfErr > 0 )
				SetErrorDetail("btnNext_Click/30330",30330,"Internal error ; please try again later","errNo=" + errNo.ToString() + "<br />pdfErr=" + pdfErr.ToString());

			if ( lblError.Text.Length == 0 ) // No errors, can continue
			{
				hdnPageNo.Value = pageNo.ToString();
				SetPostBackURL();
			}
		}

//	Script timeout is set to 230 seconds in MS Azure and can't be changed

//		public Register() : base()
//		{
//			timeOut              = Server.ScriptTimeout;
//			Server.ScriptTimeout = 600; // 10 minutes
//		}
//
//		public override void CleanUp()
//		{
//			Server.ScriptTimeout = timeOut;
//		}
	}
}