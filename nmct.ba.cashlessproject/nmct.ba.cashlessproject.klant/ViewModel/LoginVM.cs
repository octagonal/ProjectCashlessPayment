using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using nmct.ba.cashlessproject.klant.ViewModel;
using nmct.ba.cashlessproject.model.it;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;
using Swelio.Engine;
using nmct.ba.cashlessproject.klant;
using System.Windows;
using System.Runtime.InteropServices;

namespace nmct.ba.cashlessproject.klant.ViewModel
{
    class LoginVM : ViewModelBase, IPage
    {
        public string Name
        {
            get { return "Login"; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; RaisePropertyChanged("Username"); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; RaisePropertyChanged("Password"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; RaisePropertyChanged("Error"); }
        }


        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login); }
        }

        private void Login()
        {
            //MessageBox.Show();
            Card card = null;
            Manager engine = new Manager();
            engine.Active = true;
            CardReader reader = engine.GetReader(0);
            if (reader != null)
            {
                reader.ActivateCard();
                card = reader.GetCard();
                if (card != null)
                {
                    Identity identity = card.ReadIdentity();
                    if (identity != null)
                    {
                        Console.WriteLine("Welcome {0} {1}", identity.FirstName1, identity.Surname);
                    }
                }
                reader.DeactivateCard();
            }
            engine.Dispose();
            /*
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();

            if (!ApplicationVM.token.IsError)
            {
                //appvm.ChangePage(new ProductVM());
                appvm.LoggedIn();
            }
            else
            {
                Error = "Gebruikersnaam of paswoord kloppen niet";
            }*/
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri(lib.Constants.WEBURL + "token"));
            // return client.RequestResourceOwnerPasswordAsync(Username, Password).Result;
            return client.RequestResourceOwnerPasswordAsync("admin", "password").Result;
        }
    }
}
