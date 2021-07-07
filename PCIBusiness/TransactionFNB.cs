using System;
using System.Text;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionFNB : Transaction
	{
		public override int GetToken(Payment payment)
		{
			int ret  = 10;
			payToken = "";

			try
			{
				Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

//	Testing
//				xmlSent  = Tools.JSONPair("cardHolderName","A N Other",1,"{")
//				         + Tools.JSONPair("pan"           ,"5413330089010483",1)
//				         + Tools.JSONPair("cvv"           ,"603",1)
//				         + Tools.JSONPair("expiryDate"    ,"20251231",1,"","}");
//	Testing

				xmlSent  = Tools.JSONPair("cardHolderName",payment.CardName,1,"{")
				         + Tools.JSONPair("pan"           ,payment.CardNumber,1)
				         + Tools.JSONPair("cvv"           ,payment.CardCVV,1)
				         + Tools.JSONPair("expiryDate"    ,payment.CardExpiryYYYY + payment.CardExpiryMM + payment.CardExpiryDD,1,"","}");
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				payToken = Tools.JSONValue(strResult,"transactionId");
				if ( ret == 0 && payToken.Length > 0 )
					return 0;

				Tools.LogInfo("GetToken/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("GetToken/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		public override int Reversal(Payment payment)
		{
			int ret = 10;
			payRef  = "";
			xmlSent = "";

			try
			{
				Tools.LogInfo("Reversal/10","Merchant Ref=" + payment.MerchantReference,10,this);
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.Reversal);
				payRef   = Tools.JSONValue(strResult,"transactionId");
				if ( ret == 0 && payRef.Length > 0 )
					return 0;
				Tools.LogInfo("Reversal/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("Reversal/98","Ret="+ret.ToString(),255,this);
				Tools.LogException("Reversal/99","Ret="+ret.ToString(), ex,this);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			if ( ! EnabledFor3d(payment.TransactionType) )
				return 590;

			int ret = 10;
			payRef  = "";

			Tools.LogInfo("TokenPayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

//	Testing
//			payment.CardToken = "HgYwCV8vUE0qAFvxGr902XM1J04cFHgDSlW7sq5KmldkWi5zRjX2zrq8psW826Al";
//			payment.CardToken = "IdYsxppCTuLbrXDW3shAFCOXP1vpdSxH62dMR25I99UkabHaHY0vmeOfzXvEWhrJ";
//			payment.CardToken = "u2SP9zdVP1Xda44E4gkjlfIkp64owKqN5ucKsvKFskSqdFihSiQD3HZi44fKUNQK";
//	Testing

			try
			{
				xmlSent = "{ \"paymentSplit\" : " + Tools.JSONPair("cardToken"  ,payment.CardToken,1,"[{")
				                                  + Tools.JSONPair("amount"     ,payment.PaymentAmount.ToString(),11)
				                                  + Tools.JSONPair("paymentType","MOTO",1,"","}],")
				        +    "\"basket\" : { \"usingBasketDetails\" :"
				        +                 "{ \"items\" : " + Tools.JSONPair("description","Subscription",1,"[{")
				                                           + Tools.JSONPair("amount",payment.PaymentAmount.ToString(),11,"","}],")
				        +                  Tools.JSONPair("merchantOrderNumber",payment.MerchantReference,1,"","")
				        + "}}}";
/*
{ "paymentSplit" :
  [
   { "cardToken" : "IdYsxppCTuLbrXDW3shAFCOXP1vpdSxH62dMR25I99UkabHaHY0vmeOfzXvEWhrJ",
     "amount" : 899,
     "paymentType" : "MOTO"
   }
  ],
  "basket" :
  { 
    "usingBasketDetails" :
    {
      "items" :
      [
       { "description" : "Subscription",
         "amount" : 899
       }
      ],
      "merchantOrderNumber" : "A133208703"
    }
  }
}
*/
				ret    = 20;
				ret    = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				payRef = Tools.JSONValue(strResult,"transactionId");
				if ( ret == 0 && payRef.Length > 0 )
					return 0;

				Tools.LogInfo("TokenPayment/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("TokenPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType)
      {
			int    k;
			int    ret       = 10;
			string url       = payment.ProviderURL;
			string resultURL = "";
			string urlPart   = "";
			string webMethod = "POST";

			if ( Tools.NullToString(url).Length == 0 )
				url = BureauURL;

			ret = 20;
			if ( url.EndsWith("/") )
				url = url.Substring(0,url.Length-1);

			ret = 30;
			if ( transactionType == (byte)Constants.TransactionType.GetToken )
			{
				url      = url + "/mtokenizer/api/card";
			//	tranDesc = "Get Token";
				urlPart  = "API/CARD/";
			}
			else if ( transactionType == (byte)Constants.TransactionType.TokenPayment ) // Spec section 3.3.3
			{
				url      = url + "/ape/api/pay/simple";
			//	tranDesc = "Process Payment";
				urlPart  = "PAY/TRACK/";
			}
			else if ( transactionType == (byte)Constants.TransactionType.Refund )
			{
				if ( payment.TransactionID.Length < 1 )
				{
					Tools.LogInfo("CallWebService/261","Refund, empty TransactionID",220,this);
					return 33;
				}
				url      = url + "/ape/api/refund";
			//	tranDesc = "Process Refund";
				urlPart  = "PAY/BLAH/";
			}
			else if ( transactionType == (byte)Constants.TransactionType.Reversal )
			{
				if ( payment.TransactionID.Length < 1 )
				{
					Tools.LogInfo("CallWebService/262","Reversal, empty TransactionID",220,this);
					return 36;
				}
				url       = url + "/ape/api/pay/reverse/" + payment.TransactionID;
				urlPart   = "PAY/BLAH/";
				webMethod = "PUT";
			}
			else if ( transactionType == (byte)Constants.TransactionType.ThreeDSecurePayment )
			{
				url      = url + "/eCommerce/v2/prepareTransaction";
			//	tranDesc = "3d Secure Payment";
			}
			else
			{
				Tools.LogInfo("CallWebService/263","transactionType="+transactionType.ToString(),220,this);
				return 50;
			}

			ret        = 60;
			strResult  = "";
			resultCode = "99";
			resultMsg  = "(99) Internal error connecting to " + url;
			ret        = 70;

//	Testing
//			payment.ProviderKey      = "REVqzPb4PTiD4n7Fo3e1p1VyQUbvmy5YZuhxhUpqL0EcUTGWHPchIUd8m3LeixLf"; // API Key
//			payment.ProviderPassword = "sbyq0CUAvUSPMifwRH0f68fByQ5ZgSjyEpbeKg77o1Cuh9BD30ucakuXtpCCUMJN"; // Instance Key
//	Testing

			try
			{
				byte[]         page            = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest      = (HttpWebRequest)WebRequest.Create(url);
			//	webRequest.ContentType         = "application/json;charset=UTF-8";
				webRequest.ContentType         = "application/json";
				webRequest.Accept              = "application/json";
				webRequest.Method              = webMethod;
				ret                            = 80;
				webRequest.Headers["API-KEY"]  = payment.ProviderKey;
				ret                            = 90;
				webRequest.Headers["INST-KEY"] = payment.ProviderPassword;
				ret                            = 100;

//	Testing
//				string h = "";
//				k        = 0;
//				foreach (string key in webRequest.Headers.AllKeys )
//					h = h + Environment.NewLine + "[" + (k++).ToString() + "] " + key + " : " + webRequest.Headers[key];
//
//				Tools.LogInfo("CallWebService/20","Transaction Type=" + tranDesc +
//				                                ", URL="              + url +
//				                                ", Request Body="     + xmlSent +
//				                                ", Request Headers="  + h, 199, this);
//	Testing

				using (Stream stream = webRequest.GetRequestStream())
				{
					ret = 110;
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}

				ret = 120;
				using (WebResponse webResponse = webRequest.GetResponse())
				{
					ret = 130;
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
					{
						ret       = 140;
						strResult = rd.ReadToEnd().Trim();
					}
					if ( urlPart.Length > 0 )
					{
						ret       = 150;
						resultURL = webResponse.Headers["Location"].ToString();
						ret       = 160;
						k         = resultURL.ToUpper().IndexOf(urlPart);
						if ( resultURL.Length > 0 && k > 0 )
						{
							Tools.LogInfo("CallWebService/31","Success (" + urlPart + "), Location="+resultURL,199,this);
							ret        = 170;
							strResult  = Tools.JSONPair("transactionId",resultURL.Substring(urlPart.Length+k),1,"{",",")
							           + Tools.JSONPair("resultUrl",resultURL,1,"","}")
							           + strResult;
							ret        = 180;
							strResult  = strResult.Replace("}{",",");
							ret        = 0;
							resultCode = "00";
							resultMsg  = "";
						}
						else
							ret = 220;
					}
					else
						ret = 230;
				}
			}
			catch (WebException ex1)
			{
				strResult = Tools.DecodeWebException(ex1,ClassName+".CallWebService/297","ret="+ret.ToString());
				if ( strResult.Length == 0 )
					strResult = Tools.JSONPair("status" ,ex1.Status.ToString(),1,"{")
					          + Tools.JSONPair("error"  ,ex1.Message,1)
					          + Tools.JSONPair("message",ex1.ToString(),1,"","}");
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("CallWebService/298","ret="+ret.ToString(),220,this);
				Tools.LogException("CallWebService/299","ret="+ret.ToString(),ex2,this);
				if ( strResult.Length == 0 )
					strResult = Tools.JSONPair("status" ,ex2.Source.ToString(),1,"{")
					          + Tools.JSONPair("error"  ,ex2.Message,1)
					          + Tools.JSONPair("message",ex2.ToString(),1,"","}");
			}
			return ret;
		}

		private int TestService(byte live=0)
      {
//			Testing only!
			try
			{
				string         url        = BureauURL + "/pg/api/v2/util/validate";
				string         data       = "{\"data\":\"value\"}";
				byte[]         page       = Encoding.UTF8.GetBytes(data);
				HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);

				webRequest.ContentType         = "application/json";
				webRequest.Accept              = "application/json";
				webRequest.Method              = "POST";
				webRequest.Headers["API-KEY"]  = "yZgqflutOnJ3nSN7s9ylGMAZDNpmllGzuPMHPVBwDOW8riDT3qu8Uivbg7xlWbeK";
				webRequest.Headers["INST-KEY"] = "sxuAbskjc5AVdQ5qbyja7ClSAZQ9NmMYmzoTGQ1ucRB6Jxg3LAq9tOZLWFYorWpl";

				using (Stream stream = webRequest.GetRequestStream())
				{
					stream.Write(page, 0, page.Length);
					stream.Flush();
					stream.Close();
				}

				using (WebResponse webResponse = webRequest.GetResponse())
				{
					using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
						strResult = rd.ReadToEnd();
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("TestService/99","",ex,this);
			}
			return 0;
		}

		public override int ThreeDSecurePayment(Payment payment,Uri postBackURL,string languageCode="",string languageDialectCode="")
		{
			int    ret       = 10;
			string urlReturn = "";
			d3Form           = "";

			try
			{
				Tools.LogInfo("ThreeDSecurePayment/10","Merchant Ref=" + payment.MerchantReference,10,this);

				if ( postBackURL == null )
					urlReturn = Tools.ConfigValue("SystemURL");
				else
					urlReturn = postBackURL.GetLeftPart(UriPartial.Authority);
				if ( ! urlReturn.EndsWith("/") )
					urlReturn = urlReturn + "/";
				ret       = 20;
				urlReturn = urlReturn + "RegisterThreeD.aspx?ProviderCode="+bureauCode
				                      +                    "&TransRef="+Tools.XMLSafe(payment.MerchantReference)
				                      +                    "&ResultCode=";

/*
{"apiKey" : "f9bd07c6-a662-441c-8335-365a967cf1b3",
"merchantOrderNumber" : "MON123456",
"amount" : 12300,
"validationURL" : "http://test.co.za/validate",
"description" : "Test Transaction"}
*/

				xmlSent  = Tools.JSONPair("apiKey"             ,payment.ProviderKey,1,"{")
				         + Tools.JSONPair("merchantOrderNumber",payment.MerchantReference,1)
				         + Tools.JSONPair("amount"             ,"100",11)
				         + Tools.JSONPair("successURL"         ,urlReturn+"00",1)
				         + Tools.JSONPair("failureURL"         ,urlReturn+"09",1)
				         + Tools.JSONPair("description"        ,"Test",1,"","}");
				ret      = 20;
				ret      = CallWebService(payment,(byte)Constants.TransactionType.ThreeDSecurePayment);
				ret      = 30;
				payToken = Tools.JSONValue(strResult,"txnToken");
				d3Form   = Tools.JSONValue(strResult,"URL");
				ret      = 40;
				if ( payToken.Length > 0 && d3Form.Length > 0 )
					ret   = 0;
//				else
//					Tools.LogInfo("ThreeDSecurePayment/50","JSON Sent="+xmlSent+", JSON Rec="+strResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo     ("ThreeDSecurePayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("ThreeDSecurePayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent, ex,this);
			}
			return ret;
		}

		public TransactionFNB() : base()
		{
			ServicePointManager.Expect100Continue = false; // Yes, this must be FALSE
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
			base.LoadBureauDetails(Constants.PaymentProvider.FNB);
		}
	}
}
