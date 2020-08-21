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
		private string queryName;
		private string contractCode;

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
				contractCode     = WebTools.RequestValueString(Request,"ContractCode",(byte)Constants.HttpMethod.Post);
				string userCode  = WebTools.RequestValueString(Request,"UserCode",    (byte)Constants.HttpMethod.Post);
				string passWord  = WebTools.RequestValueString(Request,"UserPassword",(byte)Constants.HttpMethod.Post);
				string secretKey = WebTools.RequestValueString(Request,"SecretKey",   (byte)Constants.HttpMethod.Post);
				queryName        = WebTools.RequestValueString(Request,"QueryName",   (byte)Constants.HttpMethod.Post);

				if ( secretKey != "7e6415a7cb790238fd12430a0ce419b3" )
					return SendJSON(10005,"Invalid secret key");

				if ( contractCode.ToUpper() != "TEST" && userCode.ToUpper() != "TEST" )
					return SendJSON(10002,"Invalid contract code");

				if ( passWord.ToUpper() != "TEST" )
					return SendJSON(10003,"Invalid password");

				if ( queryName.ToUpper() != "TEST" )
					return SendJSON(10004,"Invalid query");

				if ( contractCode.Length < 1 )
					contractCode = userCode;

				string json = Tools.JSONPair("ClientCode","10927483")
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
				            + Tools.JSONPair("AccountBalance","112093.76",11,"","}") + "]";
				return SendJSON(77777,"",json.ToString());
			}
			catch (Exception ex)
			{
				Tools.LogException("UIApplicationQuery.QueryData","",ex);
			}

			return 0;
		}

		private int SendJSON(int errCode,string errMessage,string json="")
		{
			contractCode = Tools.NullToString(contractCode);
			json         = Tools.JSONPair("ErrorCode",errCode.ToString(),1,"{",",")
			             + Tools.JSONPair("ErrorMessage",errMessage,1,"","")
			             + ( json.Length == 0 ? "" : "," + json ) + "}";
			string log   = "ContractCode="+contractCode+", Query: "+queryName
			             + ( errCode > 0 ? " (Error " + errCode.ToString() + ": " + errMessage + ")" : "" );

			try
			{
				Tools.LogInfo("UIApplicationQuery.ReturnJSON/1",log);

				Response.Clear();
				Response.ContentType = "application/json; charset=utf-8";
//				Response.Write(json.Replace("'","\""));
				Response.Write(json);
				Response.Flush();
				Response.End();
			}
			catch
			{ }

//			catch (ThreadAbortException)
//			{ }
//			catch (Exception ex)
//			{
//				Tools.LogException("UIApplicationQuery.ReturnJSON/2",log,ex);
//			}

			return errCode;
		}
	}
}
