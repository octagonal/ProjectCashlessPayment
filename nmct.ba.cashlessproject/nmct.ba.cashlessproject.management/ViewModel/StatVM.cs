using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.it;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.management.ViewModel
{
    class StatVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Stat page"; }
        }

        public RelayCommand ShowSalesCommand { get; private set; }

        public StatVM()
        {
            SelectedFromDate =  DateTime.Now.AddDays(-1);
            SelectedUntilDate = DateTime.Now.AddDays(1);

            Registers = new ObservableCollection<Register>();
            GetRegisters();
            Products = new ObservableCollection<Product>();
            GetProducts();

            ShowSales();
        }

        private async void GetRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync(lib.Constants.WEBURL + "api/Register");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Registers = JsonConvert.DeserializeObject<ObservableCollection<Register>>(json);
                }
                else
                {
                    Registers = new ObservableCollection<Register>();
                    Console.WriteLine("No Registers");
                }
            }
        }

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync(lib.Constants.WEBURL + "api/Product");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Products = JsonConvert.DeserializeObject<ObservableCollection<Product>>(json);
                }
                else
                {
                    Products = new ObservableCollection<Product>();
                    Console.WriteLine("No Products");
                }
            }
        }

        private async void ShowSales()
        {
            if (SelectedFromDate != Convert.ToDateTime("1/01/0001 0:00:00") && SelectedUntilDate != Convert.ToDateTime("1/01/0001 0:00:00"))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.GetAsync(lib.Constants.WEBURL + "api/sale/product/4/" + Convert.ToString(SelectedFromDate.ToEpochTime()) + "/" + Convert.ToString(SelectedUntilDate.ToEpochTime()));
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Sale> saleList = JsonConvert.DeserializeObject<List<Sale>>(json);
                        Sales = JsonConvert.DeserializeObject<ObservableCollection<Sale>>(json);

                    }
                    else
                    {
                        Sales = new ObservableCollection<Sale>();
                    }
                }
            }
        }

        private async void ShowSalesWithItem(string type, int id)
        {
            if (SelectedFromDate != Convert.ToDateTime("1/01/0001 0:00:00") && SelectedUntilDate != Convert.ToDateTime("1/01/0001 0:00:00"))
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.GetAsync(lib.Constants.WEBURL + "api/sale/" + type + "/" + id + "/" + Convert.ToString(SelectedFromDate.ToEpochTime()) + "/" + Convert.ToString(SelectedUntilDate.ToEpochTime()));
                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        List<Sale> saleList = JsonConvert.DeserializeObject<List<Sale>>(json);
                        Sales = JsonConvert.DeserializeObject<ObservableCollection<Sale>>(json);
                        SelectedRegister = null;
                        SelectedProduct = null;
                    }
                    else
                    {
                        Sales = new ObservableCollection<Sale>();
                    }
                }
            }
        }
        
        private DateTime _selectedFromDate;
        public DateTime SelectedFromDate
        {
            get { return _selectedFromDate; }
            set { _selectedFromDate = value; OnPropertyChanged("SelectedFromDate"); ShowSales(); }
        }

        private DateTime _selectedUntilDate;
        public DateTime SelectedUntilDate
        {
            get { return _selectedUntilDate; }
            set { _selectedUntilDate = value; OnPropertyChanged("SelectedUntilDate"); ShowSales(); }
        }


        private Register _selectedRegister;
        public Register SelectedRegister
        {
            get { return _selectedRegister; }
            set { _selectedRegister = value; OnPropertyChanged("SelectedRegister"); if (value != null) { ShowSalesWithItem("register", SelectedRegister.ID); } }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); if (value != null) { ShowSalesWithItem("product", SelectedProduct.ID); } }
        }

        private ObservableCollection<Register> _registers;
        public ObservableCollection<Register> Registers
        {
            get { return _registers; }
            set { _registers = value; OnPropertyChanged("Registers"); }
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private ObservableCollection<Sale> _sales;
        public ObservableCollection<Sale> Sales{
            get { return _sales;}
            set { _sales = value; OnPropertyChanged("Sales"); }
        }

        private Sale _selectedSale;
        public Sale SelectedSale
        {
            get { return _selectedSale; }
            set { _selectedSale = value; OnPropertyChanged("SelectedSale"); }
        }
    }

}
