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

namespace nmct.ba.cashlessproject.management.ViewModel
{
    class RegisterVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Register page"; }
        }

        public RegisterVM()
        {
            if (ApplicationVM.token != null)
            {
                GetRegisters();
            }
        }

        private ObservableCollection<Register> _Registers;
        public ObservableCollection<Register> Registers
        {
            get { return _Registers; }
            set { _Registers = value; OnPropertyChanged("Registers"); }
        }

        private async void GetRegisters()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync("http://localhost:43622/api/Register");
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

        private Register _selected;
        public Register SelectedRegister
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedRegister"); }
        }

        public ICommand NewRegisterCommand
        {
            get { return new RelayCommand(NewRegister); }
        }
        
        public ICommand SaveRegisterCommand
        {
            get { return new RelayCommand(SaveRegister); }
        }

        public ICommand DeleteRegisterCommand
        {
            get { return new RelayCommand(DeleteRegister); }
        }

        private void NewRegister()
        {
            Register c = new Register();
            Registers.Add(c);
            SelectedRegister = c;
        }

        private async void SaveRegister()
        {
            string input = JsonConvert.SerializeObject(SelectedRegister);

            // check insert (no ID assigned) or update (already an ID assigned)
            if (SelectedRegister.ID == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync("http://localhost:43622/api/Register", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedRegister.ID = Int32.Parse(output);
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
                    HttpResponseMessage response = await client.PutAsync("http://localhost:43622/api/Register", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                }
            }
        }

        private async void DeleteRegister()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync("http://localhost:43622/api/Register/" + SelectedRegister.ID);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    Registers.Remove(SelectedRegister);
                }
            }
        }

    }

}
