using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using CAPITAL.Messages;
using Microsoft.Phone.Notification;
using GalaSoft.MvvmLight.Messaging;
using CAPITAL.CapitalService;
using System.Collections.ObjectModel;

namespace CAPITAL.Tools
{
    public class NotificationService
    {
        private HttpNotificationChannel httpChannel;
        private const string channelName = "CAPITALChannel";

        public NotificationService()
        {
            // Bind to message for init.
            Messenger.Default.Register<ToastMessage>(this, s => Init());
        }

        public void Init()
        {
            try
            {
                //First, try to pick up existing channel
                httpChannel = HttpNotificationChannel.Find(channelName);

                if (null != httpChannel)
                {
                    SubscribeToChannelEvents();

                    SubscribeToService();

                    SubscribeToNotifications();
                }
                else
                {
                    httpChannel = new HttpNotificationChannel(channelName, "BlandCAPITAL.Shared.FinanceService");

                    SubscribeToChannelEvents();

                    httpChannel.Open();
                }
            }
            catch
            {

            }
        }

        private void SubscribeToService()
        {
            try
            {
                // Get User
                User msgUser = new User();

                var msg = new GetUserMessage(this, message =>
                {
                    msgUser = message;
                });

                Messenger.Default.Send(msg);

                // Create Registration
                Registration registration = new Registration();
                registration.URI = httpChannel.ChannelUri.ToString();
                registration.UserId = msgUser.UserId;

                // Register
                CapitalServiceClient client = new CapitalServiceClient();
                client.RegisterCompleted += (s, e) => { };
                client.RegisterAsync(registration);
            }
            catch
            {

            }
        }

        private void SubscribeToNotifications()
        {
            // Bind to Toast Notification 
            try
            {
                if (!httpChannel.IsShellToastBound)
                {
                    httpChannel.BindToShellToast();
                }
            }
            catch (Exception)
            {
                // handle error here
            }

            // Bind to Tile Notification 
            try
            {
                if (!httpChannel.IsShellTileBound)
                {
                    // you can register the phone application to receive tile images from remote servers [this is optional]
                    Collection<Uri> uris = new Collection<Uri>();
                    uris.Add(new Uri("http://services.ilikeitbland.com"));
                    uris.Add(new Uri("http://localhost"));
                    

                    httpChannel.BindToShellTile(uris);
                }
            }
            catch (Exception)
            {
                //handle error here
            }
        }

        private void SubscribeToChannelEvents()
        {
            //Register to UriUpdated event - occurs when channel successfully opens
            httpChannel.ChannelUriUpdated += new EventHandler<NotificationChannelUriEventArgs>(httpChannel_ChannelUriUpdated);

            //Subscribed to Raw Notification
            //httpChannel.HttpNotificationReceived += new EventHandler<HttpNotificationEventArgs>(httpChannel_HttpNotificationReceived);

            //general error handling for push channel
            httpChannel.ErrorOccurred += new EventHandler<NotificationChannelErrorEventArgs>(httpChannel_ExceptionOccurred);

            //subscrive to toast notification when running app    
            //httpChannel.ShellToastNotificationReceived += new EventHandler<NotificationEventArgs>(httpChannel_ShellToastNotificationReceived);
        }

        void httpChannel_ChannelUriUpdated(object sender, NotificationChannelUriEventArgs e)
        {
            SubscribeToService();
            SubscribeToNotifications();
        }

        void httpChannel_ExceptionOccurred(object sender, NotificationChannelErrorEventArgs e)
        {

        }
    }
}
