using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using MimeKit.Text;
using SupportCore.App.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MailKit.Security;
using MailKit.Net.Imap;
using SupportCore.Models;
using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace SupportCore.App.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string message,List<CoAuthor> coAuthors=null);
        Task<List<Requests>> GetEmailsAsync(Context _context);
        Task<Requests> ReadEmailAsync(uint uid, Context _context);
        Task<AttachFile> GetAttach(uint id, string FileName);
        void MakeReadAsync(uint uid);
    }

        public class AttachFile
        {
            public byte[] Stream { set; get; }
            public string ContentType { set; get; }
        }


    public class EmailService : IEmailService
    {
        private readonly EmailConfig ec;
        
        public EmailService(IOptions<EmailConfig> emailConfig)
        {
            this.ec = emailConfig.Value;
        }

        public async Task SendEmailAsync(String email, String subject, String message,List<CoAuthor> coAuthors=null )
        {
            if (String.IsNullOrEmpty(email) && coAuthors == null) {
                return;
            }
            try
            {
                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(ec.FromName, ec.FromAddress));
                if (coAuthors?.Count>0)
                {
                   foreach(CoAuthor  coauthor in coAuthors)
                    {
                        emailMessage.To.Add(new MailboxAddress("", coauthor.Email));
                    }
                }
                if (!String.IsNullOrEmpty(email)){ 
                    emailMessage.To.Add(new MailboxAddress("", email));
                }
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(TextFormat.Html) { Text = message + @"<hr>
                <br>
                <br>"+ec.Signature
                };

                using (var client = new SmtpClient())
                {
                    client.LocalDomain = ec.LocalDomain;
                    await client.ConnectAsync(ec.MailServerAddress, Convert.ToInt32(ec.MailServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                    await client.SendAsync(emailMessage).ConfigureAwait(false);
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task<List<Requests>> GetEmailsAsync(Context _context)
        {
            try
            {
                var client = new ImapClient();
                using (client)
                {
                    await client.ConnectAsync(ec.ImapServerAddress, Convert.ToInt32(ec.ImapServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                    await client.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                    await client.Inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);
                    var uids = await client.Inbox.SearchAsync(SearchQuery.NotSeen);
                    List<Requests> emails = new List<Requests>();
                    var messages = await client.Inbox.FetchAsync(uids, MessageSummaryItems.Envelope | MessageSummaryItems.UniqueId);
                    for(int m=0;m<messages.Count();m++)
                    {

                        
                        var Email = messages[m].Envelope.From.Mailboxes.FirstOrDefault().Address.ToString();
                       
                        
                             string PersonId = null;
                              var person = await _context.Person.AsNoTracking().FirstOrDefaultAsync(e => e.Email == Email);
                              if (person != null)
                              {
                                    Email = person.Name;
                                    PersonId = person.Id;
                              }
                        emails.Add(new Requests
                        {
                            Id = messages[m].UniqueId.Id.ToString(),
                            Email=Email,
                            Subject=messages[m].NormalizedSubject ?? "<i>Без темы</i>",
                            DateCreate=messages[m].Date.DateTime,
                            From = 1,
                            PersonId = PersonId
                        });
                           
                    }
                    await client.DisconnectAsync(true).ConfigureAwait(false);
                    return emails;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Requests> ReadEmailAsync(uint uid,Context _context)
        {
            using (var emailClient = new ImapClient())
            {
                //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await emailClient.ConnectAsync(ec.ImapServerAddress, Convert.ToInt32(ec.ImapServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                await emailClient.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                await emailClient.Inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);
                var message = await emailClient.Inbox.GetMessageAsync(new MailKit.UniqueId(uid));
                string attach_list="";
                var attachments = message.Attachments;
                if (attachments.Count() > 0)
                {
                    attach_list = "<p> Вложения: ";
                    string span_attach = "<span class=\"mif-attachment\"></span>";
                    string url;
                    foreach (MimeEntity attach in attachments)
                    {
                        url = span_attach+"<a target='_blank' href='Requests\\Attach\\"+uid.ToString()+"?FileName="+attach.ContentDisposition.FileName + "'>"+attach.ContentDisposition.FileName+"</a>";
                        attach_list = attach_list+url+" ";
                    }
                }
                var Email = message.From.Mailboxes.FirstOrDefault().Address.ToString();
                string PersonId = null;
                var person = await _context.Person.AsNoTracking().FirstOrDefaultAsync(e => e.Email == Email);
                if (person != null)
                {
                    Email = person.Name;
                    PersonId = person.Id;
                }
        
               
                Requests email = new Requests
                {
                    Id = uid.ToString(),
                    Email = Email,
                    Subject = message.Subject??"Без темы",
                    DateCreate = message.Date.DateTime,
                    From = 1,
                    PersonId = PersonId,
                    Text = attach_list+message.HtmlBody ?? message.TextBody 
                };
                await emailClient.DisconnectAsync(true).ConfigureAwait(false);
                return email;
            }
        }
        public async void MakeReadAsync(uint uid)
        {
            using (var emailClient = new ImapClient())
            {
                //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await emailClient.ConnectAsync(ec.ImapServerAddress, Convert.ToInt32(ec.ImapServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                await emailClient.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                await emailClient.Inbox.OpenAsync(MailKit.FolderAccess.ReadWrite);
                emailClient.Inbox.AddFlags(new MailKit.UniqueId(uid), MessageFlags.Seen, true);
                var message = await emailClient.Inbox.GetMessageAsync(new MailKit.UniqueId(uid));
                
                var Email = message.From.Mailboxes.FirstOrDefault().Address.ToString();
                await emailClient.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public async Task<AttachFile> GetAttach(uint id, string FileName)
        {

            using (var emailClient = new ImapClient())
            {
                //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                await emailClient.ConnectAsync(ec.ImapServerAddress, Convert.ToInt32(ec.ImapServerPort), SecureSocketOptions.Auto).ConfigureAwait(false);
                await emailClient.AuthenticateAsync(new NetworkCredential(ec.UserId, ec.UserPassword));
                await emailClient.Inbox.OpenAsync(MailKit.FolderAccess.ReadOnly);
                var message = await emailClient.Inbox.GetMessageAsync(new MailKit.UniqueId(id));
                var attach = message.Attachments.SingleOrDefault(f => f.ContentDisposition?.FileName == FileName);
                using (var memory = new MemoryStream())
                {
                    if (attach is MimePart)
                        ((MimePart)attach).Content.DecodeTo(memory);
                    else
                        ((MessagePart)attach).Message.WriteTo(memory);

                    var bytes = memory.ToArray();
                    await emailClient.DisconnectAsync(true).ConfigureAwait(false);
                    return new AttachFile { Stream = bytes,ContentType=attach.ContentType.MimeType };
                }               
            }
              //  var stream = new FileStream(path, FileMode.Open) ;
            throw new NotImplementedException();
        }
    }
}