﻿using System;
using System.Xml;
using System.Web;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using PCIBusiness;

//	Developed by:
//		Paul Kilfoil
//		Software Development & IT Consulting
//		http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public partial class UIApplicationQuery : BasePageBackOffice
	{
		private byte          inputDataType;
		private string        inputDataJSON;
		private XmlDocument   inputDataXML;

		private int           errorCode;
		private string        errorMsg;
		private StringBuilder json;

		private string        queryName;
		private string        userCode;
		private string        applicationCode;
//		private string        contractCode;
		private string        countryCode;
		private string        languageCode;
		private string        languageDialectCode;

		private enum ResultCode
		{
			OK = 77777
		}

		protected override void PageLoad(object sender, EventArgs e) // AutoEventWireup = false
		{
			if ( Page.IsPostBack )
				SendJSON(10001,"Internal error");
			else
				QueryData();
		}

		private int QueryData()
		{
			try
			{
				string secretKey   = "";
				string contentType = Request.ContentType.Trim().ToUpper();
				inputDataType      = (byte)Constants.WebDataType.FormPost;
				errorCode          = 0;
				errorMsg           = "";

				if ( contentType.Contains("JSON") || contentType.Contains("XML") )
				{
					System.IO.Stream strIn = Request.InputStream;
					int              ch    = strIn.ReadByte();
					inputDataJSON          = "";

					while ( ch >= 0 )		
					{
						inputDataJSON = inputDataJSON + (Convert.ToChar(ch)).ToString();
						ch            = strIn.ReadByte();
					}
					strIn.Close();
					strIn         = null;
					inputDataJSON = inputDataJSON.Replace(Environment.NewLine,"");

					if ( contentType.Contains("JSON") )
						inputDataType = (byte)Constants.WebDataType.JSON;

					else if ( contentType.Contains("XML") )
						try
						{
							inputDataType = (byte)Constants.WebDataType.XML;
							inputDataXML  = new XmlDocument();
							inputDataXML.LoadXml(inputDataJSON);
						}
						catch
						{ }
				}

				queryName           = ParmValue("QueryName");
				applicationCode     = ParmValue("ApplicationCode");
				countryCode         = ParmValue("CountryCode");
				languageCode        = ParmValue("LanguageCode");
				languageDialectCode = ParmValue("LanguageDialectCode");
				userCode            = ParmValue("UserCode");
				secretKey           = ParmValue("SecretKey");

				if ( inputDataType != (byte)Constants.WebDataType.FormPost )
					Tools.LogInfo("UIApplicationQuery.QueryData/10","dataType="+inputDataType.ToString()
					                                             +", queryName="+queryName
					                                             +", applicationCode"+applicationCode
					                                             +", countryCode"+countryCode
					                                             +", languageCode"+languageCode
					                                             +", languageDialectCode"+languageDialectCode
					                                             +", userCode"+userCode,220);
	
				if ( Tools.SystemLiveTestOrDev() != Constants.SystemMode.Development && secretKey != "7e6415a7cb790238fd12430a0ce419b3" )
					return SendJSON(10005,"Invalid secret key");

				if ( queryName.Length < 1 )
					return SendJSON(10006,"Missing query name");

				queryName = queryName.ToUpper();

				if ( json == null )
					json = new StringBuilder();
				else
					json.Length = 0;

				if ( queryName == ("Test").ToUpper() )
					GetTestData();

				else if ( queryName == ("FinTechGetApplicationCode").ToUpper() )
					GetApplicationCode();

				else if ( queryName == ("FinTechGetApplicationInfo").ToUpper() )
					GetApplicationInfo();

				else if ( queryName == ("FinTechGetPageInfoLogin").ToUpper() )
					GetPageInfoLogin();

				else if ( queryName == ("FinTechLogOn").ToUpper() )
					LogOn();

				else if ( queryName == ("FinTechGetPageInfo2FA").ToUpper() )
					GetPageInfo2FA();

				else if ( queryName == ("FinTechLogOn2FA").ToUpper() )
					LogOn2FA();

				else if ( queryName == ("FinTechGetMenuStructure").ToUpper() )
					GetMenuStructure();

				else if ( queryName == ("FinTechDashboard").ToUpper() )
					Dashboard();

				else if ( queryName == ("FinTechGeteWalletList").ToUpper() )
					GetEWalletList();

				else
					SetError(10007,"Invalid query name");

				return SendJSON();
			}
			catch (ThreadAbortException)
			{ }
			catch (Exception ex)
			{
				Tools.LogException("QueryData/9","",ex,this);
			}

			return 0;
		}

		private int SetError(int code,string msg="")
		{
			if ( code > 0 )
			{
				errorCode   = code;
				errorMsg    = ( msg.Length > 0 ? msg : "Internal error 18888" );
				json.Length = 0;
			}
			else
			{
				errorCode = 0;
				errorMsg  = "";
			}
			return errorCode;
		}

		private int CheckParameters(string parmsRequired)
		{
			try
			{
				SetError(0);
				parmsRequired = "," + parmsRequired.Trim().ToUpper() + ",";

				if ( parmsRequired.Contains(",APP,")     && applicationCode.Length < 1 )
					SetError(11001,"Parameter ApplicationCode is missing");
				if ( parmsRequired.Contains(",COUNTRY,") && countryCode.Length < 1 )
					SetError(11002,"Parameter CountryCode is missing");
				if ( parmsRequired.Contains(",LANG,")    && languageCode.Length < 1 )
					SetError(11003,"Parameter LanguageCode is missing");
				if ( parmsRequired.Contains(",DIALECT,") && languageDialectCode.Length < 1 )
					SetError(11004,"Parameter LanguageDialectCode is missing");
				if ( parmsRequired.Contains(",USER,")    && userCode.Length < 1 )
					SetError(11005,"Parameter UserCode is missing");
			}
			catch (Exception ex)
			{
				SetError(19999,"Internal error 19999");
			}
			return errorCode;
		}

		private int GetApplicationCode()
		{
			json.Append ( Tools.JSONPair("ApplicationCode","003") );
			return 0;
		}

		private int GetApplicationInfo()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("ApplicationDescription","Pay Pay Ya")
			            + Tools.JSONPair("ApplicationURL","https://www.paypayya.com")
			            + Tools.JSONPair("ApplicationStatusCode","001")
			            + Tools.JSONPair("ApplicationStatusDescription","Running") );
			return 0;
		}

		private int GetPageInfoLogin()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("LoginLine1","Your Email Address")
			            + Tools.JSONPair("LoginLine2","Password")
			            + Tools.JSONPair("LoginButtonText","Login")
			            + Tools.JSONPair("ForgotPasswordText","Forgot Password") );
			return 0;
		}

		private int GetPageInfo2FA()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			json.Append ( Tools.JSONPair("2FALine1Text","Please enter your verification code")
			            + Tools.JSONPair("2FALine2Text","The verification code is sent to your mobile number. It will expire in 5 minutes")
			            + Tools.JSONPair("2FAButtonText","Submit")
			            + Tools.JSONPair("2FAResendText","Resend verification code") );
			return 0;
		}

		private int LogOn()
		{
			if ( CheckParameters("App") > 0 )
				return errorCode;

			string userName = ParmValue("UserName");
			string passWord = ParmValue("UserPassword");

			if ( userName.Length < 3 || passWord.Length < 3 )
				return SetError(11101,"Invalid user name and/or password");

			json.Append ( Tools.JSONPair("UserCode","013")
			            + Tools.JSONPair("UserDisplayName","Sheila Coleman")
			            + Tools.JSONPair("CountryCode","RSA")
			            + Tools.JSONPair("LanguageCode","ENG")
			            + Tools.JSONPair("LanguageDialectCode","0002")
			            + Tools.JSONPair("2FAChannelCode",((int)Constants.SystemPassword.MobileDev).ToString()) );
			return 0;
		}

		private int LogOn2FA()
		{
			if ( CheckParameters("App,User") > 0 )
				return errorCode;

			string twoFA = ParmValue("2FAChannelCode");

			if ( twoFA != ((int)Constants.SystemPassword.MobileDev).ToString() )
				return SetError(11102,"Invalid verification code");

			return 0;
		}

		private int GetEWalletList()
		{
			if ( CheckParameters("App,User,Country,Lang,Dialect") > 0 )
				return errorCode;

			using (MiscList mList = new MiscList())
				try
				{
					sql = "exec sp_FinTechGeteWalletList"
					    +     " @AppplicationCode="    + Tools.DBString(applicationCode)
					    +     ",@UserCode="            + Tools.DBString(userCode)
					    +     ",@CountryCode="         + Tools.DBString(countryCode)
					    +     ",@LanguageCode="        + Tools.DBString(languageCode)
					    +     ",@LanguageDialectCode=" + Tools.DBString(languageDialectCode);

					if ( mList.ExecQuery(sql,0) != 0 )
						return SetError(11202,"Internal error: SQL sp_FinTechGeteWalletList");

					if ( mList.EOF )
						return SetError(11204,"No data returned: SQL sp_FinTechGeteWalletList");

					int    k = 0;
					int    p1;
					int    p2;
					string bal;

					json.Append ( Tools.JSONPair("HeadingText1"               ,mList.GetColumn("HeadingText1"))
					            + Tools.JSONPair("HeadingText2"               ,mList.GetColumn("HeadingText2"))
					            + Tools.JSONPair("AccountNumberLabelText"     ,mList.GetColumn("AccountNumberLabelText"))
					            + Tools.JSONPair("AccountDescriptionLabelText",mList.GetColumn("AccountDescriptionLabelText"))
					            + Tools.JSONPair("AccountHolderLabelText"     ,mList.GetColumn("AccountHolderLabelText"))
					            + "\"Accounts\": [" );

					while ( ! mList.EOF )
					{
						k++;
						if ( k > 1 )
							json.Append(",");

						bal = mList.GetColumn("Balance");
						p1  = bal.LastIndexOf(",");
						p2  = bal.LastIndexOf(".");
						if ( p1 > 0 && p1 > p2 && bal.Length > p1 + 3 )
							bal = bal.Substring(0,p1+3);
						if ( p2 > 0 && p2 > p1 && bal.Length > p2 + 3 )
							bal = bal.Substring(0,p2+3);

						json.Append ( Tools.JSONPair("eWalletAccountCode"      ,mList.GetColumn("eWalletAccountCode"), 1, "{")
					               + Tools.JSONPair("FlagImageCode"           ,mList.GetColumn("FlagImageCode"))
					               + Tools.JSONPair("CUR"                     ,mList.GetColumn("CUR"))
					               + Tools.JSONPair("Balance"                 ,bal, 11)
					               + Tools.JSONPair("AccountIconImageCode"    ,mList.GetColumn("AccountIconImageCode"))
					               + Tools.JSONPair("AssociationIconImageCode",mList.GetColumn("AssociationIconImageCode"), 1, "", "}") );
						mList.NextRow();
					}
					json.Append("]");

					return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("GetEWalletList",sql,ex,this);
				}

			return SetError(11206,"Internal error: SQL sp_FinTechGeteWalletList");
		}

		private int GetMenuStructure()
		{
			if ( CheckParameters("App,User") > 0 )
				return errorCode;

			List<MenuItem> menuList;
			using ( MenuItems menuItems = new MenuItems() )
				menuList = menuItems.LoadMenu(userCode,applicationCode);

			if ( menuList == null || menuList.Count < 1 )
			{
				SetError(12310,"Internal error retrieving menu structure");
				return 0;
			}

			int k = 0;
			json.Append("\"Menu"+(++k).ToString()+"\":[");
			foreach (MenuItem m1 in menuList)
			{
				json.Append ( Tools.JSONPair("MenuLevel","1",11,"{")
				            + Tools.JSONPair("MenuDescription",m1.Description)
				            + Tools.JSONPair("MenuImage",m1.ImageName)
				            + Tools.JSONPair("SubItems",m1.SubItems.Count.ToString(),11) );
				if ( m1.SubItems.Count > 0 )
				{
					json.Append("\"Menu"+(++k).ToString()+"\":[");
					foreach (MenuItem m2 in m1.SubItems)
					{
						json.Append ( Tools.JSONPair("MenuLevel","2",11,"{")
						            + Tools.JSONPair("MenuDescription",m2.Description)
				                  + Tools.JSONPair("SubItems",m2.SubItems.Count.ToString(),11) );
						if ( m2.SubItems.Count > 0 )
						{
							json.Append("\"Menu"+(++k).ToString()+"\":[");
							foreach (MenuItem m3 in m2.SubItems)
							{
								json.Append ( Tools.JSONPair("MenuLevel","3",11,"{")
								            + Tools.JSONPair("MenuDescription",m3.Description)
				                        + Tools.JSONPair("SubItems",m3.SubItems.Count.ToString(),11) );
								if ( m3.SubItems.Count > 0 )
								{
									json.Append("\"Menu"+(++k).ToString()+"\":[");
									foreach (MenuItem m4 in m3.SubItems)
										json.Append ( Tools.JSONPair("MenuLevel","4",11,"{")
										            + Tools.JSONPair("MenuDescription",m4.Description)
				                              + Tools.JSONPair("SubItems","0",11)
										            + Tools.JSONPair("Url",m4.URL,1,"","},") );
									JSONAppend("],");
								}
								else
									json.Append ( Tools.JSONPair("Url",m3.URL) );
								JSONAppend("},");
							}
							JSONAppend("],");
						}
						else
							json.Append ( Tools.JSONPair("Url",m2.URL) );
						JSONAppend("},");
					}
					JSONAppend("],");
				}
				else
					json.Append ( Tools.JSONPair("Url",m1.URL) );
				JSONAppend("},");
			}
			JSONAppend("],");

			return 0;
		}

		private int Dashboard()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			SetError(12309,"Not implemented yet");
			return 0;
		}

		private void JSONAppend(string term)
		{
			while ( json.ToString().EndsWith(" ") || json.ToString().EndsWith(",") )
				json.Remove(json.Length-1,1);
			json.Append(term);
		}

		private int GetTestData()
		{
			json.Append ( Tools.JSONPair("ClientCode","10927483")
		               + Tools.JSONPair("ClientName","John D Klutz",1,"")
		               + Tools.JSONPair("NumberOfAccounts","3",11,"")
		               + "\"Accounts\":["
		               + Tools.JSONPair("AccountNumber","12345678",1,"{")
		               + Tools.JSONPair("AccountType","Current")
		               + Tools.JSONPair("AccountBalance","2093.76",11,"","},")
		               + Tools.JSONPair("AccountNumber","98765432",1,"{")
		               + Tools.JSONPair("AccountType","Savings")
		               + Tools.JSONPair("AccountBalance","143.76",11,"","},")
		               + Tools.JSONPair("AccountNumber","11223344",1,"{")
		               + Tools.JSONPair("AccountType","Investment")
		               + Tools.JSONPair("AccountBalance","112093.76",11,"","}") + "]" );
			return 0;
		}

		private int SendJSON(int errCode=0,string errMessage="")
		{
			if ( errCode < 1 )
				errCode    = errorCode;
			if ( errMessage.Length < 1 )
				errMessage = errorMsg;

			string data = Tools.JSONPair("QueryResultCode",( errCode > 0 ? errCode.ToString() : ((int)ResultCode.OK).ToString() ),11)
			            + Tools.JSONPair("QueryResultMessage",errMessage,1);
			if ( errCode == 0 && json != null && json.Length > 0 )
				data = data + json.ToString();

			data = data.Trim();
			if ( data.EndsWith(",") )
				data = data.Substring(0,data.Length-1);
			data = "{" + data + "}";

//			contractCode = Tools.NullToString(contractCode);
//			string log   = "ContractCode="+contractCode+", Query: "+queryName
//			             + ( errCode > 0 ? " (Error " + errCode.ToString() + ": " + errMessage + ")" : "" );

			try
			{
				Tools.LogInfo("UIApplicationQuery.SendJSON/1",data,10);

				Response.Clear();
				Response.ContentType = "application/json; charset=utf-8";
//				Response.Write(data.Replace("'","\""));
				Response.Write(data);
				Response.Flush();
				Response.End();
			}
			catch (ThreadAbortException)
			{ }
			catch (Exception ex)
			{
				Tools.LogException("SendJSON/9","",ex,this);
			}

			return errCode;
		}

		private string ParmValue(string parmName) // ,byte parmType=0,Object parmData=null)
		{
			try
			{
				if ( inputDataType == (byte)Constants.WebDataType.JSON ) // && ! String.IsNullOrEmpty(inputDataJSON) )
					return Tools.JSONValue(inputDataJSON,parmName);

				if ( inputDataType == (byte)Constants.WebDataType.XML ) // && inputDataXML != null )
					return Tools.XMLNode(inputDataXML,parmName);

				if ( inputDataType == (byte)Constants.WebDataType.FormPost )
					return WebTools.RequestValueString(Request,parmName,(byte)Constants.HttpMethod.Post);

				if ( inputDataType == (byte)Constants.WebDataType.FormGetOrPost ||
				     Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development )
					return WebTools.RequestValueString(Request,parmName);
			}
			catch (Exception ex)
			{
				Tools.LogException("ParmValue","parmName="+parmName+", inputDataType="+inputDataType.ToString(),ex,this);
			}
			return "";
		}

		public override void CleanUp()
		{
			json         = null;
			inputDataXML = null;
		}
	}
}
