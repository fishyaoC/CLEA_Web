using System.Diagnostics.Contracts;
using System.Net.Mail;
using System.Net;
using System.Text;
using Clea_Web.Models;
using Clea_Web.ViewModels;
using X.PagedList;

namespace Clea_Web.Service
{
	//郵件發送模組
	public class SMTPService //: BaseService
	{

		//public SMTPService(dbContext dbContext)
		//{
		//	db = dbContext;
		//}

		#region Mail Object
		public class SmtpConfig
		{
			/// <summary>
			/// SMTP位址
			/// </summary>
			public string SMTP_PATH { get; set; }
			/// <summary>
			/// SMTP埠號
			/// </summary>
			public string SMTP_PORT { get; set; }
			/// <summary>
			/// 寄件者郵件
			/// </summary>
			public string SMTP_SENDERMAIL { get; set; }
			/// <summary>
			/// 寄件者名稱
			/// </summary>
			public string SMTP_SENDERMAILNAME { get; set; }
			/// <summary>
			/// SMTP認證帳號
			/// </summary>
			public string SMTP_USERNAME { get; set; }
			/// <summary>
			/// SMTP認證密碼
			/// </summary>
			public string SMTP_PASSWORD { get; set; }
			/// <summary>
			/// 是否使用SSL
			/// </summary>
			public string SMTP_USESSL { get; set; }
			/// <summary>
			/// DOMAIN名稱
			/// </summary>
			public string SMTP_DOMAINNAME { get; set; }
			/// <summary>
			/// IIS路徑
			/// </summary>
			public string SMTP_IISPATH { get; set; }
			public string SMTP_BATCHNUM { get; set; }
		}

		public class FileData
		{
			/// <summary>
			/// 檔案位置
			/// </summary>
			public string FilePath { get; set; }

			/// <summary>
			/// 檔案名稱
			/// </summary>
			public string FileNewName { get; set; }
		}
		#endregion

		#region
		public async Task<Boolean> SendMail(SmtpConfig smtpConfig, List<string> toEmail, string subject, string content, List<string> ccEmail = null, List<string> BccEmail = null, List<FileData> FileList = null)
		//public async Task<Boolean> SendMail(SmtpConfig smtpConfig)
		{
			string smtp_sendermailname = string.Empty;
			string smtp_sendermail = string.Empty;
			string smtp_host = "";
			string smtp_port = "";
			string maileDomainName = "";
			string smtp_username = "";
			string smtp_password = "";
			string smtp_usessl = string.Empty;

			if (!smtpConfig.SMTP_PATH.Equals(string.Empty))
			{
				smtp_host = smtpConfig.SMTP_PATH;
				smtp_port = smtpConfig.SMTP_PORT;
				maileDomainName = smtpConfig.SMTP_DOMAINNAME;
				smtp_sendermail = smtpConfig.SMTP_SENDERMAIL;
				smtp_sendermailname = smtpConfig.SMTP_SENDERMAILNAME;
				smtp_username = smtpConfig.SMTP_USERNAME;
				smtp_password = smtpConfig.SMTP_PASSWORD;
				smtp_usessl = smtpConfig.SMTP_USESSL;
			}
			string fromEmail = smtp_sendermail;
			string fromName = smtp_sendermailname;

			MailAddress from = new MailAddress(fromEmail, fromName, Encoding.UTF8);

			MailMessage mail = new MailMessage();
			mail.From = from;

			if (toEmail != null && toEmail.Count() > 0)
			{
				foreach (string addr in toEmail)
				{
					mail.To.Add(new MailAddress(addr));
				}
			}

			if (ccEmail != null && ccEmail.Count() > 0)
			{
				foreach (string addr in ccEmail)
				{
					mail.CC.Add(new MailAddress(addr));
				}
			}

			if (BccEmail != null && BccEmail.Count() > 0)
			{
				foreach (string addr in BccEmail)
				{
					mail.Bcc.Add(new MailAddress(addr));
				}
			}

			if (mail.To == null && mail.Bcc == null && mail.CC == null)
			{
				return false;
			}

			//信件夾帶附件
			if (FileList != null)
			{
				try
				{
					foreach (var item in FileList)
					{
						Attachment attachment = new Attachment(item.FilePath);
						if (!string.IsNullOrEmpty(item.FileNewName))
						{
							attachment.ContentDisposition.FileName = item.FileNewName;
						}
						mail.Attachments.Add(attachment);
					}
				}
				catch
				{
				}

			}

			mail.SubjectEncoding = Encoding.UTF8;
			mail.Subject = subject;
			mail.BodyEncoding = Encoding.UTF8;
			mail.Body = content;
			mail.IsBodyHtml = true;
			mail.Priority = MailPriority.Normal;

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

			SmtpClient client = new SmtpClient();
			client.Host = smtp_host;
			client.Timeout = 200000;
			client.UseDefaultCredentials = true;
			client.Port = int.Parse(smtp_port);
			client.EnableSsl = smtp_usessl.ToUpper().Equals("TRUE") || smtp_usessl.Equals("1");

			if (smtp_username != "")
			{
				client.Credentials = new NetworkCredential(smtp_username, smtp_password);
			}

			try
			{						
				client.SendAsync(mail,"");
				//client.Send(mail);
				return true;
			}
			catch (Exception ex)
			{
				//msg = ex.ToString();
				return false;
			}
		}
		#endregion
	}
}

