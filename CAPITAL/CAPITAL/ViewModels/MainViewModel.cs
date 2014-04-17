using System;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using CAPITAL.Tools;
using CAPITAL.CapitalService;
using CAPITAL.Messages;

namespace CAPITAL.ViewModels
{
    public class MainViewModel : MyViewModelBase
    {
        // Use this view Model to control the application states... 
        // trying to avoid cache elements (messenger instead)
        // All application navigation will pass through this ViewModel. Other view Models will send a NavigationMessage.
        private MyNavigationService navService;
        private NotificationService notifyService;
        private TileService tileService;

        private User user;
        public User User
        {
            get
            {
                return user;
            }
            private set
            {
                user = value;
                RaisePropertyChanged("User");
            }
        }

        private Popup popup;

        public MainViewModel()
        {
            Messenger.Default.Register<NavigationMessage>(this, s => GoToView(s));
            Messenger.Default.Register<NavigateBackMessage>(this, s => GoBack());
            Messenger.Default.Register<RemoveBackEntry>(this, s => RemoveBackEntry());

            Messenger.Default.Register<GetUserMessage>(this, message =>
            {
                message.Execute(User);
            });

            Messenger.Default.Register<SetUserMessage>(this, message =>
            {
                User = message.User;
            });



            popup = new Popup();
            popup.Child = new LoadingScreen();
            notifyService = new NotificationService();
            tileService = new TileService();
        }

        private bool isStartup = true; 

        public void AppInit(NavigationService navServiceParam, NavigationContext navContextParam)
        {
            // This is the primary location for special logic when entering the app.
            // Here is where we determine to go to the home screen or login screen.
            // And it will be the starting point for ALL tile entries

            // Instaniate here and not in the constructor
            // If you do it in the constructor the MainFrame will be null.
            navService = new MyNavigationService(navServiceParam as NavigationService);

            if(isStartup)
            {
                isStartup = false;
                popup.IsOpen = true;

                NavigationContext navContext = (navContextParam as NavigationContext);

                var settings = IsolatedStorageSettings.ApplicationSettings;
                User savedUser;
                //settings.Remove("appUser");
                settings.TryGetValue<User>("appUser", out savedUser);

                if (savedUser == null)
                {
                    popup.IsOpen = false;
                    GoToView(new NavigationMessage()
                    {
                        Source = this.GetType().Assembly.ToString(),
                        Destination = "Login",
                        OnStack = false
                    });
                }
                else
                {
                    CapitalServiceClient client = new CapitalServiceClient();
                    client.LoginCompleted += (s, e) =>
                    {
                        try
                        {
                            popup.IsOpen = false;

                            if (e.Result != null)
                            {
                                User = e.Result;

                                // Needed for Notification/Tile/Toast Config
                                Messenger.Default.Send(new ToastMessage());

                                if (navContext.QueryString.ContainsKey("Statement"))
                                {
                                    int id = Convert.ToInt32(navContext.QueryString["Statement"]);
                                    LoadStatement(id);
                                }
                                else
                                {
                                    GoToView(new NavigationMessage()
                                    {
                                        Source = this.GetType().Assembly.ToString(),
                                        Destination = "Home",
                                        OnStack = false
                                    });
                                }
                            }
                            else
                            {
                                GoToView(new NavigationMessage()
                                {
                                    Source = this.GetType().Assembly.ToString(),
                                    Destination = "Login",
                                    OnStack = false
                                });
                            }
                        }
                        catch(Exception)
                        {
                            //MessageBox.Show(ex.Message);

                            GoToView(new NavigationMessage()
                            {
                                Source = this.GetType().Assembly.ToString(),
                                Destination = "Login",
                                OnStack = false
                            });
                        }
                    };

                    client.LoginAsync(savedUser);
                }
            }
        }

        private void GoToView(NavigationMessage message)
        {
            try
            {
                //Messenger.Default.Send<NavigationMessage>(message);
                navService.Navigate(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Navigation Error", MessageBoxButton.OK);
            }
        }

        private void GoBack()
        {
            try
            {
                navService.GoBack();
            }
            catch
            {

            }
        }

        private void RemoveBackEntry()
        {
            try
            {
                navService.RemoveBackEntry();
            }
            catch
            {

            }
        }
        private void LoadStatement(int key)
        {
            try
            {
                IsBusy = true;
                CapitalServiceClient client = new CapitalServiceClient();
                client.GetStatementByIdCompleted += (s, e) =>
                {
                    try
                    {
                        if (e.Error == null)
                        {
                            if (e.Result != null)
                            {
                                Messenger.Default.Send(new StatementEditMessage { Statement = e.Result });
                                GoToView(new NavigationMessage()
                                {
                                    Source = this.GetType().Assembly.ToString(),
                                    Destination = "StatementViewer",
                                    OnStack = false
                                });
                            }
                            else
                            {
                                GoToView(new NavigationMessage()
                                {
                                    Source = this.GetType().Assembly.ToString(),
                                    Destination = "Home",
                                    OnStack = false
                                });
                            }
                        }
                        else
                            MessageBox.Show(e.Error.Message, "Error", MessageBoxButton.OK);
                        IsBusy = false;
                        popup.IsOpen = false;
                    }
                    catch
                    {
                        IsBusy = false;
                    }
                };
                client.GetStatementByIdAsync(User, key);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
            }
        }
    }
}
