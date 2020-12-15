using System;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;

namespace PCIBusiness
{
	public class TransactionCyberSource : Transaction
	{
		public  bool Successful
		{
			get { return Tools.JSONValue(strResult,"success").ToUpper() == "TRUE"; }
		}

		public override int CardPayment(Payment payment)
		{
//			Testing only!
			try
			{
				xmlSent  = "{"
					+ "  \"clientReferenceInformation\": {"
					+ "    \"code\": \"TC50171_3\""
					+ "  },"
					+ "  \"processingInformation\": {"
					+ "    \"commerceIndicator\": \"internet\""
					+ "  },"
					+ "  \"paymentInformation\": {"
					+ "    \"card\": {"
					+ "      \"number\": \"4111111111111111\","
					+ "      \"expirationMonth\": \"12\","
					+ "      \"expirationYear\": \"2031\","
					+ "      \"securityCode\": \"123\""
					+ "    }"
					+ "  },"
					+ "  \"orderInformation\": {"
					+ "    \"amountDetails\": {"
					+ "      \"totalAmount\": \"102.21\","
					+ "      \"currency\": \"USD\""
					+ "    },"
					+ "    \"billTo\": {"
					+ "      \"firstName\": \"John\","
					+ "      \"lastName\": \"Doe\","
					+ "      \"company\": \"Visa\","
					+ "      \"address1\": \"1 Market St\","
					+ "      \"address2\": \"Address 2\","
					+ "      \"locality\": \"san francisco\","
					+ "      \"administrativeArea\": \"CA\","
					+ "      \"postalCode\": \"94105\","
					+ "      \"country\": \"US\","
					+ "      \"email\": \"test@cybs.com\","
					+ "      \"phoneNumber\": \"4158880000\""
					+ "    }"
					+ "  }"
					+ "}";
				int ret = CallWebService(null,(byte)Constants.TransactionType.CardPayment);
				return ret;
			}
			catch (Exception ex)
			{

			}
			return 203;
		}

		public override int GetToken(Payment payment)
		{
			int ret  = 10;
			payToken = "";

			try
			{
				Tools.LogInfo("GetToken/10","Merchant Ref=" + payment.MerchantReference,10,this);

				xmlSent  = "{ \"creditCard\" : " + Tools.JSONPair("number"     ,payment.CardNumber,1,"{")
				                                 + Tools.JSONPair("cardHolder" ,payment.CardName,1)
				                                 + Tools.JSONPair("expiryYear" ,payment.CardExpiryYYYY,11)
				                                 + Tools.JSONPair("expiryMonth",payment.CardExpiryMonth.ToString(),11) // Not padded, so 7 not 07
				                                 + Tools.JSONPair("type"       ,payment.CardType,1)
				                                 + Tools.JSONPair("cvv"        ,payment.CardCVV,1,"","}") // Changed to STRING from NUMERIC
				         + "}";
				ret      = 20;
//				ret      = TestService(0); // Dev
//				ret      = TestService(1); // Live
//				ret      = CallWebService(payment,"/pg/api/v2/card/register");
				ret      = CallWebService(payment,(byte)Constants.TransactionType.GetToken);
				ret      = 30;
				payToken = Tools.JSONValue(XMLResult,"token");
				ret      = 40;
				if ( Successful && payToken.Length > 0 )
					ret   = 0;
//				else
//					Tools.LogInfo("GetToken/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,199,this);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("GetToken/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("GetToken/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex,this);
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

			try
			{
				xmlSent = "{ \"creditCard\" : "  + Tools.JSONPair("token"    ,payment.CardToken,1,"{","}")
				        + ", \"transaction\" : " + Tools.JSONPair("reference",payment.MerchantReference,1,"{")
				                                 + Tools.JSONPair("currency" ,payment.CurrencyCode,1)
				                                 + Tools.JSONPair("amount"   ,payment.PaymentAmount.ToString(),11,"","}")
				        + ", "                   + Tools.JSONPair("threeDSecure","false",12,"","")
				        + "}";

				ret     = 20;
//				ret     = CallWebService(payment,"/pg/api/v2/payment/create");
				ret     = CallWebService(payment,(byte)Constants.TransactionType.TokenPayment);
				ret     = 30;
				payRef  = Tools.JSONValue(XMLResult,"reference");
				ret     = 40;
				if ( Successful && payRef.Length > 0 )
					ret  = 0;
//				else
//					Tools.LogInfo("TransactionPayGenius.TokenPayment/50","JSON Sent="+xmlSent+", JSON Rec="+XMLResult,199);
			}
			catch (Exception ex)
			{
				Tools.LogInfo("TokenPayment/98","Ret="+ret.ToString()+", JSON Sent="+xmlSent,255,this);
				Tools.LogException("TokenPayment/99","Ret="+ret.ToString()+", JSON Sent="+xmlSent,ex,this);
			}
			return ret;
		}

		private int CallWebService(Payment payment,byte transactionType)
      {
			if ( payment == null )
			{
				payment             = new Payment();
				payment.BureauCode  = Tools.BureauCode(Constants.PaymentProvider.CyberSource);
				payment.ProviderURL = "https://apitest.cybersource.com";
			}

//			TO DO

			int    ret      = 10;
			string url      = payment.ProviderURL;
			string tranDesc = "";

			if ( Tools.NullToString(url).Length == 0 )
				url = BureauURL;

			ret = 20;
			if ( url.EndsWith("/") )
				url = url.Substring(0,url.Length-1);

			ret = 30;
			if ( transactionType == (byte)Constants.TransactionType.GetToken )
			{
				url      = url + "/customers";
				tranDesc = "Get Token";
			}
			else if ( transactionType == (byte)Constants.TransactionType.TokenPayment )
			{
				url      = url + "/payments";
				tranDesc = "Token Payment";
			}
			else if ( transactionType == (byte)Constants.TransactionType.CardPayment )
			{
				url      = url + "/pts/v2/payments/";
				tranDesc = "Card Payment";
			}
			else
			{ }

			ret        = 60;
			strResult  = "";
			resultCode = "99";
			resultMsg  = "Internal error connecting to " + url;
			ret        = 70;

			try
			{
				string         digest;
				string         sigCoded;
				string         sigSource;
				string         sep                    = "\"";
				byte[]         page                   = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest webRequest             = (HttpWebRequest)WebRequest.Create(url);
				webRequest.ContentType                = "application/json";
				webRequest.Accept                     = "application/json";
				webRequest.Method                     = "POST";
//				webRequest.Date                       = System.DateTime.Now;
				webRequest.Date                       = new DateTime(2018,12,14,02,00,00);
				webRequest.Host                       = "apitest.cybersource.com";
				ret                                   = 60;
				webRequest.Headers["v-c-merchant-id"] = payment.ProviderAccount;
//				webRequest.Headers["date"]            = System.DateTime.UtcNow.ToString(); // "Fri, 11 Dec 2020 07:18:03 GMT";
//				webRequest.Headers["host"]            = webRequest.Host; // "apitest.cybersource.com";
				digest                                = GenerateDigest(xmlSent);
				webRequest.Headers["Digest"]          = digest;
				ret                                   = 70;
				sigSource                             = "host: "            + webRequest.Host            + "\n"
				                                      + "date: "            + webRequest.Headers["Date"] + "\n"
				                                      + "(request-target): post /pts/v2/payments/"       + "\n"
				                                      + "digest: "          + digest                     + "\n"
				                                      + "v-c-merchant-id: " + payment.ProviderAccount;
				ret                                   = 80;
				sigCoded                              = GenerateSignature(sigSource,payment.ProviderKey);
				webRequest.Headers["Signature"]       =   "keyid="     + sep + payment.ProviderUserID + sep
				                                      + ", algorithm=" + sep + "HmacSHA256" + sep
				                                      + ", headers="   + sep + "host date (request-target) digest v-c-merchant-id" + sep
				                                      + ", signature=" + sep + sigCoded + sep;
				ret                                   = 100;

				Tools.LogInfo("CallWebService/20",
				              "Transaction Type=" + tranDesc +
				            ", URL=" + url +
				            ", Key=" + payment.ProviderKey +
				            ", Signature Input=" + sigSource +
				            ", Signature Output=" + sigCoded +
				            ", JSON Sent=" + xmlSent, 220, this);

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
						ret        = 140;
						strResult  = rd.ReadToEnd();
					}
					if ( strResult.Length == 0 )
					{
						ret        = 150;
						resultMsg  = "No data returned from " + url;
						Tools.LogInfo("CallWebService/30","Failed, JSON Rec=(empty)",199,this);
					}
					else
					{
						ret        = 160;
						resultCode = Tools.JSONValue(strResult,"code");
						resultMsg  = Tools.JSONValue(strResult,"message");

						if (Successful)
						{
							ret        = 170;
							resultCode = "00";
							Tools.LogInfo("CallWebService/40","Successful, JSON Rec=" + strResult,255,this);
						}
						else
						{
							ret = 180;
							Tools.LogInfo("CallWebService/50","Failed, JSON Rec=" + strResult,199,this);
							if ( Tools.StringToInt(resultCode) == 0 )
								resultCode = "99";
						}
					}
				}
				ret = 0;
			}
			catch (WebException ex1)
			{
				Tools.DecodeWebException(ex1,ClassName+".CallWebService/297","ret="+ret.ToString());
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("CallWebService/298","ret="+ret.ToString(),220,this);
				Tools.LogException("CallWebService/299","ret="+ret.ToString(),ex2,this);
			}
			return ret;
		}

//		private string GetSignature(string secretKey,string endPoint,string jsonData)
//		{
//			HMACSHA256 hmac = new HMACSHA256 (Encoding.Default.GetBytes(secretKey));
//			byte[]     hash = hmac.ComputeHash (Encoding.Default.GetBytes(endPoint + "\n" + jsonData));
//			string     sig  = "";
//			hmac            = null;
//
//			for (int k = 0; k < hash.Length; k++)
//				sig = sig + hash[k].ToString("X2"); // Hexadecimal
//
//			return sig.ToLower();
//		}

//	Code from CyberSource
//	Start
		private string GenerateDigest(string jsonData)
		{
			try
			{
//				string jsonData = "{ your JSON payload }";
				using (SHA256 sha256hash = SHA256.Create())
				{
					byte[] payloadBytes = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(jsonData));
					string digest       = Convert.ToBase64String(payloadBytes);
					return "SHA-256=" + digest;
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("GenerateDigest",jsonData,ex,this);
			}
			return "";
		}
		private string GenerateSignature(string signatureParams, string secretKey)
		{
			var sigBytes      = Encoding.UTF8.GetBytes(signatureParams);
			var decodedSecret = Convert.FromBase64String(secretKey);
			var hmacSha256    = new HMACSHA256(decodedSecret);
			var messageHash   = hmacSha256.ComputeHash(sigBytes);
			return Convert.ToBase64String(messageHash);
		}
//	End

		public TransactionCyberSource() : base()
		{
			base.LoadBureauDetails(Constants.PaymentProvider.CyberSource);
			xmlResult                             = null;
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
		}
	}
}
