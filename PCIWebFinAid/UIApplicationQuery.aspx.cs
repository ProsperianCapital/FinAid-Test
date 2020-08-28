using System;
using System.Text;
using System.Threading;
using PCIBusiness;

//	Developed by:
//		Paul Kilfoil
//		Software Development & IT Consulting
//		http://www.PaulKilfoil.co.za

namespace PCIWebFinAid
{
	public partial class UIApplicationQuery : BasePageBackOffice
	{
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
				errorCode           = 0;
				errorMsg            = "";

				queryName           = ParmValue("QueryName");
				applicationCode     = ParmValue("ApplicationCode");
				countryCode         = ParmValue("CountryCode");
				languageCode        = ParmValue("LanguageCode");
				languageDialectCode = ParmValue("LanguageDialectCode");
				userCode            = ParmValue("UserCode");
				string secretKey    = ParmValue("SecretKey");

//				string contractCode = ParmValue("ContractCode");

				Tools.LogInfo("UIApplicationQuery.QueryData/10","queryName="+queryName
				                                             +", applicationCode"+applicationCode
				                                             +", countryCode"+countryCode
				                                             +", languageCode"+languageCode
				                                             +", languageDialectCode"+languageDialectCode
				                                             +", userCode"+userCode,10);

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

				else
					SetError(10007,"Invalid query name");

				return SendJSON();
			}
			catch (ThreadAbortException)
			{ }
			catch (Exception ex)
			{
				Tools.LogException("UIApplicationQuery.QueryData/9","",ex);
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
			json.Append ( Tools.JSONPair("ApplicationCode","001") );
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

			json.Append ( Tools.JSONPair("UserCode","71349")
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

		private int GetMenuStructure()
		{
			if ( CheckParameters("App,Country,Lang,Dialect") > 0 )
				return errorCode;

			SetError(12308,"Not implemented yet");
			return 0;
		}

		private int Dashboard()
		{
			if ( CheckParameters("App,Country,Lang,Dialect,User") > 0 )
				return errorCode;

			SetError(12309,"Not implemented yet");
			return 0;
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
				Tools.LogException("UIApplicationQuery.SendJSON/9","",ex);
			}

			return errCode;
		}

		private string ParmValue(string parmName)
		{
			if ( Tools.SystemLiveTestOrDev() == Constants.SystemMode.Development )
				return WebTools.RequestValueString(Request,parmName);
			return WebTools.RequestValueString(Request,parmName,(byte)Constants.HttpMethod.Post);
		}

		public override void CleanUp()
		{
			json = null;
		}

//		public UIApplicationQuery()
//		{
//			json = new StringBuilder();
//		}
	}
}
