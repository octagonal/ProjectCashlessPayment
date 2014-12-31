﻿using GalaSoft.MvvmLight.CommandWpf;
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

namespace nmct.ba.cashlessproject.management.ViewModel
{
    class LoginVM : ObservableObject, IPage
    {
       public string Name
        {
            get { return "Login"; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; OnPropertyChanged("Username"); }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set { _error = value; OnPropertyChanged("Error"); }
        }


        public ICommand LoginCommand
        {
            get { return new RelayCommand(Login); }
        }

        private void Login()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();

            if (!ApplicationVM.token.IsError)
            {
                appvm.ChangePage(new ProductVM());
            }
            else
            {
                Error = "Gebruikersnaam of paswoord kloppen niet";
            }         
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri("http://localhost:43622/token"));
            // return client.RequestResourceOwnerPasswordAsync(Username, Password).Result;
            return client.RequestResourceOwnerPasswordAsync("admin", "password").Result;
        }
    }
}
