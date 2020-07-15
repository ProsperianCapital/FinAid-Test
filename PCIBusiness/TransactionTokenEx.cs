using System;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;

namespace PCIBusiness
{
	public class TransactionTokenEx : Transaction
	{
		TransactionPeach tranPeach;

		private int PostHTML(byte transactionType,Payment payment)
		{
			int    ret  = 10;
			string pURL = "https://test.oppwa.com/v1/registrations";
			string tURL = "https://test-api.tokenex.com/TransparentGatewayAPI/Detokenize";
			strResult   = "";
			payRef      = "";
			resultCode  = "999.999.999";
			resultMsg   = "Internal error";

			try
			{
				if ( payment.ProviderURL.Length > 0 )
					pURL = payment.ProviderURL;
				if ( payment.TokenizerURL.Length > 0 )
					tURL = payment.TokenizerURL;
				if ( tranPeach == null )
					tranPeach = new TransactionPeach();

				Tools.LogInfo("TransactionTokenEx.PostHTML/10","URL=" + pURL + ", URL data=" + xmlSent,221);

				ret                              = 20;
				byte[]         buffer            = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest request           = (HttpWebRequest)HttpWebRequest.Create(tURL);
				ret                              = 30;
				request.Method                   = "POST";
				request.Headers["Authorization"] = "Bearer " + payment.ProviderKey;
				request.Headers["TX_URL"]        = pURL;
				request.Headers["TX_TokenExID"]  = payment.TokenizerID;  // "4311038889209736";
				request.Headers["TX_APIKey"]     = payment.TokenizerKey; // "54md8h1OmLe9oJwYdp182pCxKF0MUnWzikTZSnOi";
				request.ContentType              = "application/x-www-form-urlencoded";
				ret                              = 40;
				Stream postData                  = request.GetRequestStream();
				ret                              = 50;
				postData.Write(buffer, 0, buffer.Length);
				postData.Close();

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					ret                     = 60;
					Stream       dataStream = response.GetResponseStream();
					ret                     = 70;
					StreamReader reader     = new StreamReader(dataStream);
					ret                     = 80;
					strResult               = reader.ReadToEnd();
					ret                     = 90;
//					var s       = new JavaScriptSerializer();
//					xmlReceived = s.Deserialize<Dictionary<string, dynamic>>(reader.ReadToEnd());
					reader.Close();
					dataStream.Close();
					ret                  = 100;
					resultCode           = Tools.JSONValue(strResult,"code");
					resultMsg            = Tools.JSONValue(strResult,"description");
					ret                  = 110;
					tranPeach.ResultCode = resultCode;
					if ( tranPeach.Successful )
						ret = 0;
					else
						Tools.LogInfo("TransactionTokenEx.PostHTML/110","resultCode="+resultCode+", resultMsg="+resultMsg,221);
				}
			}
			catch (WebException ex1)
			{
				resultCode = ex1.Response.Headers["tx_code"];
				resultMsg  = ex1.Response.Headers["tx_message"];

				Tools.LogInfo     ("TransactionTokenEx.PostHTML/198","Ret="+ret.ToString()+", URL=" + pURL + ", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionTokenEx.PostHTML/199","Ret="+ret.ToString()+", URL=" + pURL + ", XML Sent="+xmlSent,ex1);
			}
			catch (Exception ex2)
			{
				Tools.LogInfo     ("TransactionTokenEx.PostHTML/198","Ret="+ret.ToString()+", URL=" + pURL + ", XML Sent="+xmlSent,255);
				Tools.LogException("TransactionTokenEx.PostHTML/199","Ret="+ret.ToString()+", URL=" + pURL + ", XML Sent="+xmlSent,ex2);
			}
			return ret;
		}

		public override int GetToken(Payment payment)
		{
			int    ret = 10;
			string url = "https://test-api.tokenex.com/TokenServices.svc/REST/Tokenize";

			try
			{
				xmlSent = "<TokenAction>"
				        + Tools.XMLCell("APIKey",payment.ProviderKey)
				        + Tools.XMLCell("TokenExID",payment.ProviderUserID)
				        + Tools.XMLCell("Data",payment.CardNumber)
				        + Tools.XMLCell("TokenScheme","sixTOKENfour")
				        + "</TokenAction>";

				if ( payment.ProviderURL.Length > 0 )
					url = payment.ProviderURL;

				Tools.LogInfo("TransactionTokenEx.GetToken/10","Post="+xmlSent+", Key="+payment.ProviderKey,10);

				ret                     = 20;
				byte[]         buffer   = Encoding.UTF8.GetBytes(xmlSent);
				HttpWebRequest request  = (HttpWebRequest)HttpWebRequest.Create(url);
				ret                     = 30;
				request.Method          = "POST";
				request.ContentType     = "text/xml"; // ;charset=\"utf-8\"";
				ret                     = 40;
				Stream postData         = request.GetRequestStream();
				ret                     = 50;
				postData.Write(buffer, 0, buffer.Length);
				postData.Close();

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					ret                  = 60;
					Stream       dStream = response.GetResponseStream();
					ret                  = 70;
					StreamReader sReader = new StreamReader(dStream);
					ret                  = 80;
					strResult            = sReader.ReadToEnd();
					ret                  = 90;
					sReader.Close();
					dStream.Close();
					sReader              = null;
					ret                  = 100;
					if ( xmlResult == null )
						xmlResult         = new XmlDocument();
					else
						xmlResult.RemoveAll();
					ret                  = 110;
					xmlResult.LoadXml(strResult);
					ret                  = 120;
					resultCode           = Tools.XMLNode(xmlResult,"Success");
					ret                  = 130;
					resultMsg            = Tools.XMLNode(xmlResult,"Error");
					ret                  = 140;
					payRef               = Tools.XMLNode(xmlResult,"ReferenceNumber");
					ret                  = 150;
					payToken             = Tools.XMLNode(xmlResult,"Token");
					ret                  = 160;
					if ( resultCode.ToUpper() == "TRUE" )
						ret = 0;
					else
						Tools.LogInfo("TransactionTokenEx.PostHTML/110","resultCode="+resultCode+", resultMsg="+resultMsg,221);
				}
			}
			catch (Exception ex)
			{
				Tools.LogException("TransactionTokenEx.GetToken/199","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex);
			}
			return ret;
		}

		public override int CardPayment(Payment payment)
		{
//			For Peach Payments

			int ret = 10;

			try
			{
				xmlSent = "entityId="          + Tools.URLString(payment.ProviderUserID)
				        + "&paymentBrand="     + Tools.URLString(payment.CardType.ToUpper())
				        + "&card.number={{{"   + Tools.URLString(payment.CardToken) + "}}}"
				        + "&card.holder="      + Tools.URLString(payment.CardName)
				        + "&card.expiryMonth=" + Tools.URLString(payment.CardExpiryMM)
				        + "&card.expiryYear="  + Tools.URLString(payment.CardExpiryYYYY)
				        + "&card.cvv={{{cvv}}}"
				        + "&amount="           + Tools.URLString(payment.PaymentAmountDecimal)
				        + "&currency="         + Tools.URLString(payment.CurrencyCode)
				        + "&paymentType=DB" // DB = Instant, PA = Pre-authorize, CP =
				        + "&recurringType=REPEATED";

				Tools.LogInfo("TransactionTokenEx.TokenPayment/10","Post="+xmlSent+", Key="+payment.ProviderKey,10);

				ret      = PostHTML((byte)Constants.TransactionType.CardPayment,payment);
				payToken = Tools.JSONValue(strResult,"id");
				payRef   = Tools.JSONValue(strResult,"ndc");
				if ( payToken.Length < 1 && ret == 0 )
					ret = 248;

				Tools.LogInfo("TransactionTokenEx.TokenPayment/20","ResultCode="+ResultCode + ", payRef=" + payRef + ", payToken=" + payToken,221);
			}
			catch (Exception ex)
			{
				Tools.LogException("TransactionTokenEx.TokenPayment/90","Ret="+ret.ToString()+", XML Sent=" + xmlSent,ex);
			}
			return ret;
		}

		public override int TokenPayment(Payment payment)
		{
			return 0;
		}

      public override void Close()
		{
			tranPeach = null;
			base.Close();
		}

		public TransactionTokenEx() : base()
		{
			bureauCode                            = Tools.BureauCode(Constants.PaymentProvider.TokenEx);
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol  = SecurityProtocolType.Tls12;
		}
	}
}
