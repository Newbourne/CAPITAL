using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Info;
using System.Windows;
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
    public class NewUserViewModel : MyViewModelBase
    {
        private User newUser;

        public NewUserViewModel()
        {
            newUser = new User();
            Messenger.Default.Register<LogoutMessage>(this, e => Logout(e));
        }

        public string Email
        {
            get
            {
                return newUser.Email;
            }
            set
            {
                if (newUser.Email != value)
                {
                    newUser.Email = value;
                    RaisePropertyChanged("Email");
                }
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (password != value)
                {
                    password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get
            {
                return confirmPassword;
            }
            set
            {
                confirmPassword = value;
                RaisePropertyChanged("ConfirmPassword");
            }
        }

        private RelayCommand createUserCommand;
        public RelayCommand CreateUserCommand
        {
            get
            {
                if (createUserCommand == null)
                    createUserCommand = new RelayCommand(UserCreation);
                return createUserCommand;
            }
        }

        private void UserCreation()
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

            if (!ModelValidation()) { return; };

            CapitalServiceClient client = new CapitalServiceClient();
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

                newUser.Password = ConvertToString(hashMessage);

                newUser.Devices = new System.Collections.ObjectModel.ObservableCollection<Device>();

                byte[] uniqueId = (byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId");

                newUser.Devices.Add(new Device
                {
                    // Get Phone Data
                    UniqueDeviceId = BitConverter.ToString(uniqueId),
                    Manufacturer = DeviceStatus.DeviceManufacturer,
                    Model = DeviceStatus.DeviceHardwareVersion,
                    DeviceName = DeviceStatus.DeviceName,
                    CreationDate = DateTime.Now
                });

                client.CreateUserCompleted += (s, e) =>
                {
                    if (e.Error == null)
                    {
                        if (e.Result != null)
                        {
                            Messenger.Default.Send(new SetUserMessage { User = e.Result });
                            Messenger.Default.Send(new ToastMessage());
                            Messenger.Default.Send(new NavigationMessage { Destination = "Home", Source = this.GetType().Name, OnStack = false });
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

                client.CreateUserAsync(newUser);
            }
            catch
            {
                client.Abort();
            }
            finally
            {
                client.CloseAsync();
            }
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
            newUser = new User();
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
