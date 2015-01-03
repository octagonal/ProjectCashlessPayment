using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.it;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.klant.ViewModel
{
    public class CashIOVM : ViewModelBase, IPage
    {
        public RelayCommand<object> AddCashCommand { get; private set; }
        public RelayCommand PrintBalanceCommand { get; private set; }
        public RelayCommand UploadCashCommand { get; private set; }

        public string Name { get { return "CashIO"; } }

        private Customer customer;
        public Customer Customer
        {
            get { return customer; }
            set { customer = value; RaisePropertyChanged("Customer"); AddCashCommand.RaiseCanExecuteChanged(); }
        }

        public int MaxCash { get; private set; }

        public CashIOVM()
        {
            MaxCash = 100;
           
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;

            Messenger.Default.Register<Customer>(
                this,
                msg => { Console.WriteLine(msg.CustomerName); Customer = msg; }
            );

            AddCashCommand = new RelayCommand<object>(
                (obj) => CurrentAmountUpload += Convert.ToDouble(obj), 
                canAddCash
            );

            PrintBalanceCommand = new RelayCommand(
                () => MessageBox.Show(CurrentAmountUpload.ToString(), "Error"),
                () => Customer != null
            );

            UploadCashCommand = new RelayCommand(
                SaveCash,
                () => Customer != null
            );
        }

        private bool canAddCash(object value)
        {
            double amount = Convert.ToDouble(value);
            if (Customer != null)
            {
                if ((CurrentAmountUpload + amount + Customer.Balance) <= (MaxCash))
                {
                    return true;
                }
            }
            return false;
        }

        private double _currentAmountUpload;
        public double CurrentAmountUpload
        {
            get { return _currentAmountUpload; }
            set { 
                _currentAmountUpload = value; 
                RaisePropertyChanged("CurrentAmountUpload"); 
                AddCashCommand.RaiseCanExecuteChanged();
                UploadCashCommand.RaiseCanExecuteChanged(); 
            }
        }

        private async void SaveCash()
        {
            Customer.Balance += CurrentAmountUpload;
            string input = JsonConvert.SerializeObject(Customer);

            // check insert (no ID assigned) or update (already an ID assigned)
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.PutAsync(lib.Constants.WEBURL + "/api/customer", new StringContent(input, Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
            }
            await RefreshCustomer(Customer);
            CurrentAmountUpload = 0;
        }

        private async Task RefreshCustomer(Customer cust)
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync(lib.Constants.WEBURL + "api/customer/" + cust.NationalNumber);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    cust = JsonConvert.DeserializeObject<Customer>(json);

                }
                else
                {
                    Console.WriteLine("No customer");
                }
            }
            Customer = cust;
        }

    }
}
