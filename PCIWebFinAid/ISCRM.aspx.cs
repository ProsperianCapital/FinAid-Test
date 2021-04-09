﻿using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using PCIBusiness;

namespace PCIWebFinAid
{
	public partial class ISCRM : BasePage
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
			ApplicationCode = "210";
			errPriority     = 19;

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

				if ( productCode.Length < 1 || languageCode.Length < 1 || languageDialectCode.Length < 1 )
					LoadProduct();

				if ( ! Tools.SystemIsLive() )
				{
					if ( productCode.Length         < 1 ) productCode         = "10387";
					if ( languageCode.Length        < 1 ) languageCode        = "ENG";
					if ( languageDialectCode.Length < 1 ) languageDialectCode = "0002";
				}	

//				LoadStaticDetails();
				LoadDynamicDetails();
				LoadGoogleAnalytics();
//				LoadChat();

				btnErrorDtl.Visible = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );
				btnWidth.Visible    = ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development );

				EnableControls(1);
			}
		}

		private void EnableControls(byte seq)
		{
		//	Phone number
			X105103.Enabled = ( seq == 1 );
			btnGet.Enabled  = ( seq == 1 );

		//	Emregency data
			X105105.Enabled = ( seq == 2 );
			X105106.Enabled = ( seq == 2 );
			X105108.Enabled = ( seq == 2 );
			X105109.Enabled = ( seq == 2 );
			X105111.Enabled = ( seq == 2 );
			X105112.Enabled = ( seq == 2 );
			X105114.Enabled = ( seq == 2 );
			X105115.Enabled = ( seq == 2 );
			X105117.Enabled = ( seq == 2 );
			X105118.Enabled = ( seq == 2 );
			X105120.Enabled = ( seq == 2 );
			X105121.Enabled = ( seq == 2 );
			X105123.Enabled = ( seq == 2 );
			X105124.Enabled = ( seq == 2 );
			X105126.Enabled = ( seq == 2 );
			X105127.Enabled = ( seq == 2 );
			X105129.Enabled = ( seq == 2 );
			X105130.Enabled = ( seq == 2 );
			X105131.Enabled = ( seq == 2 ); // Button "Save"

			if ( seq == 1 )
			{
				imgOK.ImageUrl = "";
			//	imgOK.ImageUrl = PCIBusiness.Tools.ImageFolder() + "Question.png";
				X105103.Text   = "";
				X105103.Focus();
			}
			else
			{
				imgOK.ImageUrl = PCIBusiness.Tools.ImageFolder() + "Tick.png";
				X105105.Focus();
			}
		}

		private void LoadDynamicDetails()
		{
			byte   err;
			string fieldCode;
		//	string fieldHead;
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

//					pnlContact01.Visible = ( X100093.Text.Length > 0 );
//					pnlContact02.Visible = ( X104402.Text.Length > 0 );
//					pnlContact03.Visible = ( X100095.Text.Length > 0 );
//					pnlContact04.Visible = ( X100096.Text.Length > 0 || P12031.ImageUrl.Length > 0 );
//					pnlContact05.Visible = ( X100101.Text.Length > 0 );
//					pnlContact06.Visible = ( X104404.Text.Length > 0 || P12032.ImageUrl.Length > 0 );
//					pnlContact07.Visible = ( X100102.Text.Length > 0 || P12033.ImageUrl.Length > 0 );
//					pnlContact08.Visible = ( X104418.Text.Length > 0 );
//					pnlContact09.Visible = ( X100105.Text.Length > 0 || P12034.ImageUrl.Length > 0 );

//	Testing
//					WebTools.ReplaceImage(this.Page,"12002","isos1.png","isos1");
//					WebTools.ReplaceImage(this.Page,"12036","isos2.png","isos2");
//					WebTools.ReplaceControlText(this.Page,"X105104","Label 105104","");
//					WebTools.ReplaceControlText(this.Page,"X105105","PlaceHolder 105105","");
//					WebTools.ReplaceControlText(this.Page,"X105106","PlaceHolder 105106","");
//	Testing
				}
				catch (Exception ex)
				{
					PCIBusiness.Tools.LogException("LoadDynamicDetails/99","ret="+ret.ToString(),ex,this);
				}
		}

		protected void btnGetData_Click(Object sender, EventArgs e)
		{
			X105103.Focus();
			imgOK.ImageUrl = PCIBusiness.Tools.ImageFolder() + "Cross.png";
			string phone   = X105103.Text.Trim().Replace(" ","").Replace("-","").Replace("(","").Replace(")","");
			
			if ( phone.Length < 6 )
				return;

			using (MiscList mList = new MiscList())
				try
				{
					ret = 14110;
					spr = "sp_Blah";
					sql = "exec " + spr + " @MobileNumber=" + Tools.DBString(phone,47);
				//	TEST
					sql = "select '1' as Col1";
				//	TEST
					ret = mList.ExecQuery(sql,0,"",false);
					if ( ret == 0 )
					{
						EnableControls(2);
						if ( mList.EOF)
						{
						//	Show meesage "New customer"
						}
						else
						{
						//	Populate fields
						}
					}
					else
						SetErrorDetail("btnGetData_Click", 14120, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
				}
				catch (Exception ex)
				{
					SetErrorDetail("btnGetData_Click", 14130, "Internal database error (" + spr + " failed)", sql, 2, 2, ex, false, errPriority);
				}
		}

		protected void btnSave_Click(Object sender, EventArgs e)
		{
			using (MiscList mList = new MiscList())
				try
				{
					ret = 13110;
					spr = "sp_WP_Upd_iSOSUser";
					sql = "exec " + spr + " @MobileNumber="   + Tools.DBString(X105103.Text,47)
					                    + ",@Button1Number="  + Tools.DBString(X105105.Text,47)
					                    + ",@Button1Message=" + Tools.DBString(X105106.Text,47)
					                    + ",@Button2Number="  + Tools.DBString(X105108.Text,47)
					                    + ",@Button2Message=" + Tools.DBString(X105109.Text,47)
					                    + ",@Button3Number="  + Tools.DBString(X105111.Text,47)
					                    + ",@Button3Message=" + Tools.DBString(X105112.Text,47)
					                    + ",@Button4Number="  + Tools.DBString(X105114.Text,47)
					                    + ",@Button4Message=" + Tools.DBString(X105115.Text,47)
					                    + ",@Button5Number="  + Tools.DBString(X105117.Text,47)
					                    + ",@Button5Message=" + Tools.DBString(X105118.Text,47)
					                    + ",@Button6Number="  + Tools.DBString(X105120.Text,47)
					                    + ",@Button6Message=" + Tools.DBString(X105121.Text,47)
					                    + ",@Button7Number="  + Tools.DBString(X105123.Text,47)
					                    + ",@Button7Message=" + Tools.DBString(X105124.Text,47)
					                    + ",@Button8Number="  + Tools.DBString(X105126.Text,47)
					                    + ",@Button8Message=" + Tools.DBString(X105127.Text,47)
					                    + ",@Button9Number="  + Tools.DBString(X105129.Text,47)
					                    + ",@Button9Message=" + Tools.DBString(X105130.Text,47);
					ret = mList.ExecQuery(sql,0,"",false,true);
					if ( ret != 0 )
						SetErrorDetail("btnSave_Click", 13120, "Internal database error (" + spr + " failed)", sql, 2, 2, null, false, errPriority);
					Tools.LogInfo("btnSave_Click","Save iSOS config (ret="+ret.ToString() + ") : " +sql,229,this);
				}
				catch (Exception ex)
				{
					SetErrorDetail("btnSave_Click", 13130, "Internal database error (" + spr + " failed)", sql, 2, 2, ex, false, errPriority);
				}
		}

		private void LoadProduct()
		{
			byte ret  = WebTools.LoadProductFromURL(Request,ref productCode,ref languageCode,ref languageDialectCode);
			if ( ret != 0 || productCode.Length < 1 || languageCode.Length < 1 || languageDialectCode.Length < 1 )
			{
				SetErrorDetail("LoadProduct", 10777, "Unable to load product/language details", "ret="+ret.ToString(), 2, 2, null, false, errPriority);
				productCode           = "10472";
				languageCode          = "ENG";
				languageDialectCode   = "0002";
			}
			hdnProductCode.Value     = productCode;
			hdnLangCode.Value        = languageCode;
			hdnLangDialectCode.Value = languageDialectCode;

			Tools.LogInfo("LoadProduct","PC/LC/LDC="+productCode+"/"+languageCode+"/"+languageDialectCode,10,this);
		}	

//		private void LoadChat()
//		{
//			lblChat.Text = Tools.LoadChat(productCode);
//		}	

		private void LoadGoogleAnalytics()
		{
			lblGoogleUA.Text = Tools.LoadGoogleAnalytics(productCode);
		}
	}
}