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
using Newtonsoft.Json;
using GalaSoft.MvvmLight.Messaging;

namespace nmct.ba.cashlessproject.klant.ViewModel
{
    class LoginVM : ViewModelBase, IPage
    {
        public Customer Customer { get; private set; }
        public LoginVM()
        {
            Customer = new Customer();
        }

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

        private string registerStatus;
        public string RegisterStatus
        {
            get { return registerStatus; }
            set { registerStatus = value; RaisePropertyChanged("RegisterStatus"); }
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
                        Console.WriteLine(identity.NationalNumber);
                        Customer.NationalNumber = identity.NationalNumber;
                    }
                }
                reader.DeactivateCard();
            }
            card = reader.GetCard();
            if (reader.CardPresent) { card = reader.GetCard(); }

            engine.Dispose();

            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            //MessageBox.Show();
            
            ApplicationVM.token = GetToken();

            if (!ApplicationVM.token.IsError)
            {
                
                GetCustomer();
                appvm.ChangePage(new CashIOVM());
                appvm.LoggedIn();
            }
            else
            {
                Error = "Gebruikersnaam niet herkend";
            }
        }

        private Customer BuildCustomer()
        {
            Customer cust = new Customer();
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
                    Address addr = card.ReadAddress();
                    Console.WriteLine("{0} {1}", addr.Zip, addr.Street);
                    if (identity != null)
                    {
                        Console.WriteLine(identity.NationalNumber);
                        cust = new Customer()
                        {
                            NationalNumber = identity.NationalNumber,
                            Balance = 0,
                            CustomerName = identity.FirstName1 + " " + identity.Surname,
                            ID = 0,
                            Address = addr.Zip + " " + addr.Street,
                            Picture = null
                        };
                    }
                    else { throw new System.IO.IOException("Kon de kaart niet lezen."); }
                }
                reader.DeactivateCard();
            }
            engine.Dispose();
            Customer = cust;
            return cust;
        }

        private async void GetCustomer()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync(lib.Constants.WEBURL + "api/customer/" + Customer.NationalNumber);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Customer = JsonConvert.DeserializeObject<Customer>(json);

                    if (Customer.ID == 0)
                    {
                        Customer = BuildCustomer();
                        RegisterCustomer(Customer);
                        GetCustomer();
                    }
                }
                else
                {
                    Customer = new Customer();
                    Console.WriteLine("No customer");
                }
            }
            Messenger.Default.Send<Customer>(Customer);
        }

        private async void RegisterCustomer(Customer c)
        {
            string input = JsonConvert.SerializeObject(c);
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PostAsync(lib.Constants.WEBURL + "api/customer", new StringContent(input, Encoding.UTF8, "application/json"));
                if (response.IsSuccessStatusCode)
                {
                    string output = await response.Content.ReadAsStringAsync();
                    //c.ID = Int32.Parse(output);
                }
                else
                {
                    Console.WriteLine("error");
                }
            }
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri(lib.Constants.WEBURL + "token"));
            // return client.RequestResourceOwnerPasswordAsync(Username, Password).Result;
            return client.RequestResourceOwnerPasswordAsync(lib.Constants.MockCredentials["Username"], lib.Constants.MockCredentials["Password"]).Result;
        }
    }
}
