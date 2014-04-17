using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using CAPITAL.Engine.Interfaces;
using CAPITAL.ORM;
using CAPITAL.ORM.Exceptions;
using CAPITAL.ORM.Objects;
//using WindowsPhone.Recipes.Push.Messasges;

namespace CAPITAL.Engine.Utilities
{
    public class MessengerUtil : Base, IMessengerUtil
    {
        //private TilePushNotificationMessage tilePushNotificationMessage = new TilePushNotificationMessage(MessageSendPriority.High);
        //private ToastPushNotificationMessage toastPushNotificationMessage = new ToastPushNotificationMessage(MessageSendPriority.High);

        public void Register(Registration registration)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    registration.CreationDate = DateTime.Now;

                    ValidationContext valContext = new ValidationContext(this, null, null);
                    var errors = registration.Validate(valContext);
                    if (errors.Count() > 0)
                        throw new ModelException(errors);

                    // Check for existing registration first. 
                    if (context.Registration.Where(x => x.UserId == registration.UserId).FirstOrDefault() != null)
                    {
                        // Update
                        Registration reg = context.Registration.Where(x => x.UserId == registration.UserId).FirstOrDefault();
                        reg.URI = registration.URI;
                    }
                    else
                    {
                        // Insert
                        Registration reg = new Registration();
                        reg.URI = registration.URI;
                        reg.User = registration.User;
                        reg.UserId = registration.UserId;
                        reg.CreationDate = DateTime.Now;
                        context.Registration.Add(reg);
                    }
                    context.SaveChanges();
                }
            }
            catch (ModelException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                LogError(registration, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void SendTile(string name)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    var subscribers = context.Registration.Include(u => u.User).Where(u => u.User.Email == "brian@pwb3.com").FirstOrDefault();

                    if (subscribers != null)
                    {
                        //http://localhost/CAPITALServices/Tiles/ApplicationIcon.png

                        //tilePushNotificationMessage.BackgroundImageUri = new Uri(string.Format("http://services.ilikeitbland.com/Tiles/{0}", name));

                        ///tilePushNotificationMessage.SendAsync(new Uri(subscribers.URI, UriKind.Absolute), (result) => { }, (result) => { });
                    }
                }
            }
            catch(Exception ex)
            {
                LogError(name, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void SendEmail(string fromUser, string subject, string message)
        {
            try
            {
                // Email
                MailMessage mail = new MailMessage();
                SmtpClient smtpServer = new SmtpClient("");

                mail.From = new MailAddress(fromUser);
                mail.To.Add("");
                mail.Subject = subject;
                mail.Body = message;

                smtpServer.Port = 25;
                smtpServer.Credentials = new System.Net.NetworkCredential("", "");
                smtpServer.EnableSsl = false;

                smtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                LogError(new object[] { fromUser, subject, message }, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        }

        public void SendToast(User user, string title, string subTitle)
        {
            try
            {
                using (CapitalContext context = new CapitalContext())
                {
                    Registration registration = context.Registration.FirstOrDefault(x => x.UserId == user.UserId);

                    if (registration != null)
                    {
                        //toastPushNotificationMessage.Title = title;
                        //toastPushNotificationMessage.SubTitle = subTitle;
                        //toastPushNotificationMessage.SendAsync(new Uri(registration.URI, UriKind.Absolute), (result) => { }, (result) => { });
                    }
                }
            }
            catch(Exception ex)
            {
                LogError(new object[] { user, title, subTitle }, ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
        } 
    }
}
