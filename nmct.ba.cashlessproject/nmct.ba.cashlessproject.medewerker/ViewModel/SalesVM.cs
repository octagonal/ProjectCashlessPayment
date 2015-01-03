using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.it;
using Swelio.Engine;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.medewerker.ViewModel
{
    class SalesVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Product page"; }
        }

        public RelayCommand PayBillCommand { get; private set; }

        public SalesVM()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();
            
            ProductsToBuy = new ObservableCollection<Product>();
            GetProducts();

            Customer = new Customer();
            CustomerLoggedIn = false;

            PayBillCommand = new RelayCommand(
                PayBill, 
                () => { return (Customer != null && AmountToPay < Customer.Balance); }
            ); 
        }

        private TokenResponse GetToken()
        {
            OAuth2Client client = new OAuth2Client(new Uri(lib.Constants.WEBURL + "token"));
            return client.RequestResourceOwnerPasswordAsync("admin", "password").Result;
        }

        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get { return _products; }
            set { _products = value; OnPropertyChanged("Products"); }
        }

        private ObservableCollection<Product> _productsToBuy;
        public ObservableCollection<Product> ProductsToBuy
        {
            get { return _productsToBuy; }
            set { _productsToBuy = value; OnPropertyChanged("ProductsToBuy"); }
        }

        private Product _SelectedProductToBuy;
        public Product SelectedProductToBuy
        {
            get { return _SelectedProductToBuy; }
            set { _SelectedProductToBuy = value; OnPropertyChanged("SelectedProductToBuy"); }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }

        public ICommand RemoveCardCommand
        {
            get { return new RelayCommand(RemoveCard); }
        }

        private void RemoveCard()
        {
            if (CustomerLoggedIn)
            {
                CustomerLoggedIn = false;
                Customer = new Customer();
                ProductsToBuy.Clear();
                AmountToPay = 0;
            }
            else
            {
                CustomerLoggedIn = true;
                LoginCustomer();
            }
        }

        private async void PayBill()
        {
            SaveCash();
            await RefreshCustomer(Customer);
            ProductsToBuy.Clear();
        }
        public ICommand RemoveProductFromBillCommand
        {
            get { return new RelayCommand(RemoveProductFromBill); }
        }

        private void RemoveProductFromBill()
        {
            if (SelectedProductToBuy != null)
            {
                ProductsToBuy.RemoveAt(ProductsToBuy.IndexOf(SelectedProductToBuy));
                SelectedProductToBuy = null;
            }
            else
            {
                Console.WriteLine("No selection");
                if (ProductsToBuy.Count() == 1)
                    ProductsToBuy.RemoveAt(0); //Selectie bug?
            }
            AmountToPay = ProductsToBuy.Sum(el => el.Price);
        }

        public ICommand AddProductToBillCommand
        {
            get { return new RelayCommand(AddProductToBill); }
        }

        private void AddProductToBill()
        {
            if (SelectedProduct != null)
            {
                ProductsToBuy.Add(SelectedProduct);
                SelectedProduct = null;
            }
            AmountToPay = ProductsToBuy.Sum(el => el.Price);
        }

        private double _amountToPay;
        public double AmountToPay
        {
            get { return _amountToPay; }
            set { 
                _amountToPay = value; 
                OnPropertyChanged("AmountToPay"); 
                AfterSale = Customer.Balance - AmountToPay;
                PayBillCommand.RaiseCanExecuteChanged();
            }
        }

        private double _afterSale;
        public double AfterSale
        {
            get { return _afterSale; }
            set { _afterSale = value; OnPropertyChanged("AfterSale"); }
        }

        private Customer _customer;

        public Customer Customer
        {
            get { return _customer; }
            set { _customer = value; OnPropertyChanged("Customer"); }
        }

        private string _customerStageMsg;
        public string CustomerStateMsg
        {
            get { return _customerStageMsg; }
            set { _customerStageMsg = value; OnPropertyChanged("CustomerStateMsg"); }
        }

        private bool _customerLoggedIn;

        public bool CustomerLoggedIn
        {
            get { return _customerLoggedIn; }
            set {
                if (value == true)
                {
                    CustomerStateMsg = "Kaart uitwerpen";
                } else {
                    CustomerStateMsg = "Inloggen";
                }
                _customerLoggedIn = value; 
                OnPropertyChanged("CustomerLoggedIn"); 
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

        private async void SaveCash()
        {
            Customer.Balance -= AmountToPay;
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
            AmountToPay = 0;
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
            }
            Customer = cust;
        }

        private void LoginCustomer()
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

            ApplicationVM.token = GetToken();

            if (!ApplicationVM.token.IsError)
                GetCustomer();
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

                    /*if (Customer.ID == 0)
                    {
                        Customer = BuildCustomer();
                        RegisterCustomer(Customer);
                    }*/
                }
                else
                {
                    Customer = new Customer();
                    Console.WriteLine("No customer");
                }
            }
            AmountToPay = 0;
            PayBillCommand.RaiseCanExecuteChanged();
        }

    }
}
