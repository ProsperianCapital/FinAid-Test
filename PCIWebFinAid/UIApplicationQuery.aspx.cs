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
				ReturnJSON(101,"Internal error");
			else
				QueryData();
		}

		private int QueryData()
		{
			try
			{
				contractCode     = WebTools.RequestValueString(Request,"ContractCode");
				string userCode  = WebTools.RequestValueString(Request,"UserCode");
				string passWord  = WebTools.RequestValueString(Request,"UserPassword");
				string secretKey = WebTools.RequestValueString(Request,"SecretKey");
				queryName        = WebTools.RequestValueString(Request,"QueryName");

				if ( secretKey != "7e6415a7cb790238fd12430a0ce419b3" )
					return ReturnJSON(102,"Invalid secret key");

				if ( contractCode.ToUpper() != "TEST" && userCode.ToUpper() != "TEST" )
					return ReturnJSON(103,"Invalid contract code");

				if ( passWord.ToUpper() != "TEST" )
					return ReturnJSON(104,"Invalid password");

				if ( queryName.ToUpper() != "TEST" )
					return ReturnJSON(105,"Invalid query");

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
				return ReturnJSON(0,"",json.ToString());
			}
			catch (ThreadAbortException)
			{ }
			catch (Exception ex)
			{
				Tools.LogException("UIApplicationQuery.QueryData","",ex);
			}

			return 0;
		}

		private int ReturnJSON(int errCode,string errMessage,string json="")
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
			catch (ThreadAbortException)
			{ }
			catch (Exception ex)
			{
				Tools.LogException("UIApplicationQuery.ReturnJSON/2",log,ex);
			}

			return errCode;
		}
	}
}
