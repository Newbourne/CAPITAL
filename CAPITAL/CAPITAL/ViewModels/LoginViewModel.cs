using System;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Info;
using System.IO.IsolatedStorage;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using CAPITAL.CapitalService;
using CAPITAL.Messages;
using System.ServiceModel;
using CAPITAL.Tools;

namespace CAPITAL.ViewModels
{
    public class LoginViewModel : MyViewModelBase 
    {
        private User loginUser;

        public LoginViewModel()
        {
            loginUser = new User();
            loginUser.Email = "test@test.com";
            Messenger.Default.Register<LogoutMessage>(this, e => Logout(e));
        }

        public string Email
        {
            get
            {
                return loginUser.Email;
            }
            set
            {
                if (loginUser.Email != value)
                {
                    loginUser.Email = value;
                    RaisePropertyChanged("Email");
                }
            }
        }

        private string password = "password";
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (value != password)
                {
                    password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        private RelayCommand loginCommand;
        public RelayCommand LoginCommand
        {
            get
            {
                if (loginCommand == null)
                    loginCommand = new RelayCommand(UserLogin);

                return loginCommand;
            }
        }

        private RelayCommand newUserCommand;
        public RelayCommand NewUserCommand
        {
            get
            {
                if (newUserCommand == null)
                    newUserCommand = new RelayCommand(NewUserCreation);

                return newUserCommand;
            }
        }

        private void UserLogin()
        {
            object focusObj = FocusManager.GetFocusedElement();
            if (focusObj != null && focusObj is TextBox)
            {
                var binding = (focusObj as TextBox).GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();
                (focusObj as TextBox).IsEnabled = false;
                (focusObj as TextBox).IsEnabled = true;
                return;
            }
            else if (focusObj != null && focusObj is PasswordBox)
            {
                var binding = (focusObj as PasswordBox).GetBindingExpression(PasswordBox.PasswordProperty);
                binding.UpdateSource();
                (focusObj as PasswordBox).IsEnabled = false;
                (focusObj as PasswordBox).IsEnabled = true;
                return;
            }

            if (!ModelValidation()) { return; }

            CapitalServiceClient service = service = new CapitalServiceClient();
            try
            {
                IsBusy = true;

                // Encrypt Password before sending across the wire
                string key = "BLANDDIESEL";
                UTF8Encoding encoding = new UTF8Encoding();
                byte[] keyByte = encoding.GetBytes(key);

                HMACSHA256 sha256 = new HMACSHA256(keyByte);

                byte[] passwordbytes = encoding.GetBytes(password);
                byte[] hashMessage = sha256.ComputeHash(passwordbytes);

                loginUser.Password = ConvertToString(hashMessage);
                loginUser.Devices = new System.Collections.ObjectModel.ObservableCollection<Device>();

                byte[] uniqueId = (byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId");

                loginUser.Devices.Add(new Device
                {
                    // Get Phone Data
                    UniqueDeviceId = BitConverter.ToString(uniqueId),
                    Manufacturer = DeviceStatus.DeviceManufacturer,
                    Model = DeviceStatus.DeviceHardwareVersion,
                    DeviceName = DeviceStatus.DeviceName,
                });

                service.LoginCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        if (e.Result != null)
                        {
                            var settings = IsolatedStorageSettings.ApplicationSettings;
                            settings.Remove("appUser");
                            settings.Add("appUser", e.Result);
 
                            Messenger.Default.Send<SetUserMessage>(new SetUserMessage { User = e.Result });
                            Messenger.Default.Send<NavigationMessage>(new NavigationMessage { Destination = "Home", Source = this.GetType().Name, OnStack = false });
                        }
                    }
                    else if (e.Error is FaultException<CapitalError>)
                    {
                        ErrorMessages.FaultError(e.Error.Message);
                    }
                    else
                    {
                        ErrorMessages.UnexpectedError();
                    }
                    IsBusy = false;
                };

                service.LoginAsync(loginUser);
            }
            catch (Exception)
            {
                service.Abort();
            }
            finally
            {
                service.CloseAsync();
            }
        }

        private void NewUserCreation()
        {
            Messenger.Default.Send<NavigationMessage>(new NavigationMessage { Destination = "NewUser", Source = this.GetType().Name, OnStack = true });
        }

        private bool ModelValidation()
        {
            bool valid = true;
            string error = "";

            if (Email == null || string.IsNullOrEmpty(Email))
            {
                valid = false;
                error += "Email Address Required. \n";
            }
            else
            {
                if (!IsEmail(Email))
                {
                    valid = false;
                    error += "Invalid email address. \n";
                }
            }

            if (Password == null || string.IsNullOrEmpty(Password))
            {
                valid = false;
                error += "Password Required. \n";
            }

            if (!valid)
                ErrorMessages.ValidationError(error);

            return valid;
        }

        private void Logout(LogoutMessage msg)
        {
            IsBusy = false;
            var settings = IsolatedStorageSettings.ApplicationSettings;
            settings.Remove("appUser");
            loginUser = new User();
            Password = "";
            Messenger.Default.Send(new SetUserMessage { User = null });
            Messenger.Default.Send(new NavigationMessage { Destination = "Login", OnStack = false, Source = this.GetType().ToString() });
        }

        private static string ConvertToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2");
            }
            return sbinary;
        }

        public static bool IsEmail(string inputEmail)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
    }
}
