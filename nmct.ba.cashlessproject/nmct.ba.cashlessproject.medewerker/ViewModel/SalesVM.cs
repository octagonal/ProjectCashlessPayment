using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.it;
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

        public SalesVM()
        {
            ApplicationVM appvm = App.Current.MainWindow.DataContext as ApplicationVM;
            ApplicationVM.token = GetToken();
            
            ProductsToBuy = new ObservableCollection<Product>();
            GetProducts();

            AmountToPay = 0;
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

        }

        public ICommand PayBillCommand
        {
            get { return new RelayCommand(PayBill); }
        }

        private void PayBill()
        {

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
            set { _amountToPay = value; OnPropertyChanged("AmountToPay"); AfterSale = AmountToPay - 50; }
        }

        private double _afterSale;
        public double AfterSale
        {
            get { return _afterSale; }
            set { _afterSale = value; OnPropertyChanged("AfterSale"); }
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

    }
}
