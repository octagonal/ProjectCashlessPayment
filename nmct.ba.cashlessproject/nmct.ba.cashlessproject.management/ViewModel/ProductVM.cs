﻿using GalaSoft.MvvmLight.CommandWpf;
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

namespace nmct.ba.cashlessproject.management.ViewModel
{
    class ProductVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Product page"; }
        }

        public ProductVM()
        {
            if (ApplicationVM.token != null)
            {
                GetProducts();
            }
        }

        private ObservableCollection<Product> _Products;
        public ObservableCollection<Product> Products
        {
            get { return _Products; }
            set { _Products = value; OnPropertyChanged("Products"); }
        }

        private async void GetProducts()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:43622/api/Product");
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

        private Product _selected;
        public Product SelectedProduct
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedProduct"); }
        }

        public ICommand NewProductCommand
        {
            get { return new RelayCommand(NewProduct); }
        }
        
        public ICommand SaveProductCommand
        {
            get { return new RelayCommand(SaveProduct); }
        }

        public ICommand DeleteProductCommand
        {
            get { return new RelayCommand(DeleteProduct); }
        }

        private void NewProduct()
        {
            Product c = new Product();
            Products.Add(c);
            SelectedProduct = c;
        }

        private async void SaveProduct()
        {
            string input = JsonConvert.SerializeObject(SelectedProduct);

            // check insert (no ID assigned) or update (already an ID assigned)
            if (SelectedProduct.ID == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:43622/api/Product", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedProduct.ID = Int32.Parse(output);
                    }
                    else
                    {
                        Console.WriteLine("error");
                    }
                }
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PutAsync("http://localhost:43622/api/Product", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                }
            }
        }

        private async void DeleteProduct()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:43622/api/Product/" + SelectedProduct.ID);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    Products.Remove(SelectedProduct);
                }
            }
        }

    }

}
