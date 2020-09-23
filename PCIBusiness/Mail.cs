using System;
using System.Net;
using System.Net.Mail;

namespace PCIBusiness
{
	public class Mail : Message
	{
		private SmtpClient  smtp;
		private MailMessage msg;
		private string      provider;

		private void AddMail(MailAddressCollection mailList,string mail)
		{
			try
			{
				mail = Tools.NullToString(mail).Replace(" ","").Replace(">","").Replace("<","");
				if ( mail.Length < 5 || ! mail.Contains("@") )
				{ }
				else if ( mail.Contains(",") || mail.Contains(";") )
				{
					mail          = mail.Replace(";",",");
					string[] mArr = mail.Split(',');
					for ( int k = 0; k < mArr.Length ; k++ )
						if ( mArr[k].Length > 5 )
							mailList.Add(mArr[k].Trim());
				}
				else // if ( Tools.CheckEMail(mail) )
					mailList.Add(mail);
			}
			catch
			{ }
		}

		public  string Provider
		{
			get { return Tools.NullToString(provider); }
			set
			{
				provider = value.Trim();
			}
		}

		public override string Recipient
		{
			get { return msg.To.ToString(); }
		}

		public  string BCC
		{
			set { AddMail(msg.Bcc,value); }
		}
		public  string CC
		{
			set { AddMail(msg.CC,value); }
		}
		public  string To
		{
			set { AddMail(msg.To,value); }
		}
		public  string ReplyTo
		{
			set { AddMail(msg.ReplyToList,value); }
		}
		public  string From
		{
			set {	msg.From = new MailAddress(value); }
		}
//		public  string Sender
//		{
//			set {	msg.Sender = new MailAddress(value); }
//		}
		public  string Heading
		{
			set {	msg.Subject = value.Trim(); }
		}
		public  string Body
		{
			set {	msg.Body = value.Trim(); }
		}

		public override int Send()
		{
			int err = 0;

			while ( err == 0 )
			{
				err = 10;
				if ( msg == null )
					break;

				err = 20;
				if ( smtp == null )
					LoadProvider();

				err = 30;
				if ( smtp == null )
					break;

				err = 40;
				if ( msg.Body.Length < 10 )
					break;

				err = 50;
				if ( msg.To.Count < 1 )
					break;

//				if ( msg.Sender == null )
//					try
//					{
//						msg.Sender = new MailAddress(smtpSender);
//					//	msg.Sender = new MailAddress(Tools.ConfigValue("SMTP-From"));
//					}
//					catch
//					{ }

//				if ( msg.Sender == null )
//					try
//					{
//						msg.Sender = new MailAddress(Tools.ConfigValue("SMTP-User"));
//					}
//					catch
//					{ }

				err = 60;
				if ( msg.From == null )
					msg.From = msg.Sender;

				err = 70;
				msg.IsBodyHtml = msg.Body.ToUpper().Contains("<HTML");

				for ( int q = 1 ; q < 6 ; q++ ) // Try 5 times before quitting with an error
					try
					{
						err = 999;
						smtp.Send(msg);
						return 0;
					}
					catch (Exception ex)
					{
						if ( q > 3 ) // Log an error on the 4'th failed attempt
							Tools.LogException("Send/10","EMail failure (try " + q.ToString() + "), to " + msg.To.ToString(), ex, this);
					}

				break;
			}

			if ( err > 0 && err < 999 )
				Tools.LogException("Send/20","EMail failure (err=" + err.ToString() + "), to " + msg.To.ToString(), null, this);

			return err;
		}

		public override void Clear()
		{
			base.Clear();
			msg.CC.Clear();
			msg.Bcc.Clear();
			msg.To.Clear();
			msg.ReplyToList.Clear();
//			msg.From    = null;
			msg.Subject = "";
			msg.Body    = "";
		}

		public override byte LoadProvider()
		{
			providerAddress   = Tools.ConfigValue("SMTP-Server");
			providerID        = Tools.ConfigValue("SMTP-User");
			providerPassword  = Tools.ConfigValue("SMTP-Password");
			string smtpSender = Tools.ConfigValue("SMTP-Sender");
//			string smtpBCC    = Tools.ConfigValue("SMTP-BCC");
			int    smtpPort   = Tools.StringToInt(Tools.ConfigValue("SMTP-Port"));

			if ( providerAddress.Length  == 0 ) providerAddress  = Tools.ConfigValue("SMTP/Server");
			if ( providerID.Length       == 0 ) providerID       = Tools.ConfigValue("SMTP/User");
			if ( providerPassword.Length == 0 ) providerPassword = Tools.ConfigValue("SMTP/Password");
			if ( smtpSender.Length       == 0 ) smtpSender       = Tools.ConfigValue("SMTP/Sender");
//			if ( smtpBCC.Length          == 0 ) smtpBCC          = Tools.ConfigValue("SMTP/BCC");
			if ( smtpPort                == 0 ) smtpPort         = Tools.StringToInt(Tools.ConfigValue("SMTP/Port"));

			string smtpData = providerAddress + " / " + providerID
			                                  + " / " + providerPassword
			                                  + " / " + smtpSender
			                                  + " / " + smtpPort;

			if ( providerAddress.Length > 5 && smtpSender.Length > 5 )
				try
				{
					Tools.LogInfo("LoadProvider/10","Loading SMTP ... " + smtpData,222,this);
					msg.Sender = new MailAddress(smtpSender);
					smtp       = new SmtpClient(providerAddress);
					if ( smtpPort > 0 )
						smtp.Port = smtpPort;
					smtp.UseDefaultCredentials = false;
				//	smtp.EnableSsl             = false;
					smtp.Credentials           = new NetworkCredential(providerID,providerPassword);
					return 0;
				}
				catch (Exception ex)
				{
					Tools.LogException("LoadProvider/20",smtpData,ex,this);
					return 20;
				}

			Tools.LogException("LoadProvider/30","Invalid SMTP credentials (" + smtpData + ")",null,this);
			return 30;
		}

		public override void Close()
		{
			Clear();
			msg  = null;
			smtp = null;
		}

		public Mail()
		{
			msg = new MailMessage();
		}
	}
}