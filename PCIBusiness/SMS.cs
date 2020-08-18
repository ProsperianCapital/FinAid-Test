using System;
using System.Net;
using System.IO;
using System.Xml;

namespace PCIBusiness
{
	public class SMS : Message
	{
	//	Message details
		string phoneNumber;

	//	JSON template
	//	string json;

		public string PhoneNumber
		{
			get { return Tools.NullToString(phoneNumber); }
			set { phoneNumber = value.Trim().Replace(" ",""); }
		}

		public override void Clear()
		{
			base.Clear();
			phoneNumber = "";
		}

		public override int Send()
		{
			string resultData = "";
			int    ret        = 10;
			resultMsg         = "Missing/invalid phone number and/or message";

			if ( PhoneNumber.Length < 10 || MessageBody.Length < 3 )
				return ret;

			try
			{
				resultMsg      = "";
				ret            = 20;
				string sendURL = ProviderAddress + "?apiKey=" + Tools.URLString(providerPassword)
				                                 + "&to=" + Tools.URLString(phoneNumber)
				                                 + "&content=" + Tools.URLString(messageBody);
				ret                          = 30;
				HttpWebRequest webReq        = (HttpWebRequest)WebRequest.Create(sendURL);
				ret                          = 60;
				HttpWebResponse webResponse  = (HttpWebResponse)webReq.GetResponse();
				ret                          = 70;
				using (StreamReader streamIn = new StreamReader(webResponse.GetResponseStream()))
					resultData = streamIn.ReadToEnd().Trim();

				string resultOK = "false";

				if ( resultData.Length > 0 && resultData.StartsWith("{") ) // JSON
				{
					resultID     = Tools.JSONValue(resultData,"apiMessageId");
					resultOK     = Tools.JSONValue(resultData,"accepted");
					resultCode   = Tools.JSONValue(resultData,"errorCode");
					resultDetail = Tools.JSONValue(resultData,"errorDescription");
					resultMsg    = Tools.JSONValue(resultData,"error");
				}
				else if ( resultData.Length > 0 && resultData.StartsWith("<") ) // XML
				{
					XmlDocument h = new XmlDocument();
					h.Load(resultData);
					resultID      = Tools.XMLNode(h,"apiMessageId");
					resultOK      = Tools.XMLNode(h,"accepted");
					resultCode    = Tools.XMLNode(h,"errorCode");
					resultDetail  = Tools.XMLNode(h,"errorDescription");
					resultMsg     = Tools.XMLNode(h,"error");
					h             = null;
				}

				if ( resultID.ToUpper()     == "NULL" ) resultID     = "";
				if ( resultCode.ToUpper()   == "NULL" ) resultCode   = "";
				if ( resultDetail.ToUpper() == "NULL" ) resultDetail = "";
				if ( resultMsg.ToUpper()    == "NULL" ) resultMsg    = "";

				if ( resultOK.ToUpper() == "TRUE" )
					return 0;

				Tools.LogInfo("SMS.Send/199",resultData,229);
				return 199;
			}
			catch (Exception ex)
			{
				Tools.LogException("SMS.Send/301","ret="+ret.ToString()+", "+resultData+"",ex);
				resultMsg = ex.Message;
			}
			return ret;
		}


		public int SendV1()
		{
			int ret = 10;
			resultMsg  = "Missing/invalid phone number and/or message";

			if ( PhoneNumber.Length < 10 || MessageBody.Length < 3 )
				return ret;

			try
			{
				HttpWebRequest webReq  = (HttpWebRequest)WebRequest.Create(providerAddress);
				ret                    = 20;
				resultMsg              = "Internal error";
            webReq.ContentType     = "application/json";
            webReq.Method          = "POST";
            webReq.Accept          = "application/json";
            webReq.PreAuthenticate = true;
            webReq.Headers.Add("Authorization", providerPassword);
				ret                    = 30;
//				string data            = json.Replace("@PHONE@",PhoneNumber).Replace("@MSG@",MessageBody);
				string data            = "Not used";
				ret                    = 40;

				using (StreamWriter streamOut = new StreamWriter(webReq.GetRequestStream()))
				{
					ret = 50;
					streamOut.Write(data);
					streamOut.Flush();
					streamOut.Close();
				}

				ret                          = 60;
				HttpWebResponse webResponse  = (HttpWebResponse)webReq.GetResponse();
				ret                          = 70;

				using (StreamReader streamIn = new StreamReader(webResponse.GetResponseStream()))
					resultMsg = streamIn.ReadToEnd();

				return 0;
			}
			catch (Exception ex)
			{
				Tools.LogException("SMS.Send","ret="+resultCode.ToString(),ex);
				resultMsg = ex.Message;
			}
			return ret;
		}

		public SMS()
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			providerAddress  = "https://platform.clickatell.com/messages/http/send";
		//	providerAddress  = "https://platform.clickatell.com/messages";
		//	providerAddress  = "https://platform.clickatell.com/v1/message";
		//	providerPassword = "-03qEod-S2KbSMLcooBm1w==";
			providerPassword = "E8gSxksaQpmEDZ4OvabmlQ==";
		//	json             = "{ #messages#: [{#channel#:#sms#,#to#:#@PHONE@#,#content#:#@MSG@#}] }";
		//	json             = json.Replace("#","\"");
		}
	}
}