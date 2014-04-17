using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using CAPITAL.Engine.Interfaces;
using CAPITAL.ORM;
using CAPITAL.ORM.Exceptions;
using CAPITAL.ORM.Objects;

namespace CAPITAL.Engine.Utilities
{
    public class FeedbackUtil : Base, IFeedbackUtil
    {
        public void SendFeedback(Feedback userFeedback)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    userFeedback.CreationDate = DateTime.Now;

                    ValidationContext valContext = new ValidationContext(this, null, null);
                    var errors = userFeedback.Validate(valContext);

                    if (errors.Count() > 0)
                        throw new ModelException(errors);

                    context.Feedback.Add(userFeedback);
                    context.SaveChanges();

                    int maxFeedback = context.Feedback.Max(x => x.FeedbackId);
                    Feedback feedback = context.Feedback.Where(x => x.FeedbackId == maxFeedback).Include(x => x.User).FirstOrDefault();

                    if (feedback != null)
                    {
                        // Email
                        MailMessage mail = new MailMessage();
                        SmtpClient smtpServer = new SmtpClient("mail.ilikeitbland.com");

                        mail.From = new MailAddress(feedback.User.Email);
                        mail.To.Add("BRIAN@ILIKEITBLAND.COM");
                        mail.Subject = "CAPITAL - FEEDBACK";
                        mail.IsBodyHtml = true;
                        mail.Body = "<b>" + feedback.ReportType + "</b>" + "<br />" + feedback.Message;

                        smtpServer.Port = 25;
                        smtpServer.Credentials = new System.Net.NetworkCredential("brian@ILIKEITBLAND.COM", "$bland#42");
                        smtpServer.EnableSsl = false;

                        smtpServer.Send(mail);
                    }
                }
            }
            catch (ModelException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(userFeedback, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }        
    }
}
