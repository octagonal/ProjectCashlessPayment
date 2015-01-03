using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using Newtonsoft.Json;
using nmct.ba.cashlessproject.model.it;
using Swelio.Engine;
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
    class EmployeeVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Employee page"; }
        }

        public EmployeeVM()
        {
            if (ApplicationVM.token != null)
            {
                GetEmployees();
            }
        }

        private ObservableCollection<Employee> _Employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _Employees; }
            set { _Employees = value; OnPropertyChanged("Employees"); }
        }

        private async void GetEmployees()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.GetAsync(lib.Constants.WEBURL + "api/Employee");
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    Employees = JsonConvert.DeserializeObject<ObservableCollection<Employee>>(json);
                }
                else
                {
                    Employees = new ObservableCollection<Employee>();
                    Console.WriteLine("No Employees");
                }
            }
        }

        private Employee _selected;
        public Employee SelectedEmployee
        {
            get { return _selected; }
            set { _selected = value; OnPropertyChanged("SelectedEmployee"); }
        }

        public ICommand NewEmployeeCommand
        {
            get { return new RelayCommand(NewEmployee); }
        }
        
        public ICommand SaveEmployeeCommand
        {
            get { return new RelayCommand(SaveEmployee); }
        }

        public ICommand DeleteEmployeeCommand
        {
            get { return new RelayCommand(DeleteEmployee); }
        }

        public ICommand InjectEmployeeCommand
        {
            get { return new RelayCommand(InjectEmployee); }
        }

        private void NewEmployee()
        {
            Employee c = new Employee();
            Employees.Add(c);
            SelectedEmployee = c;
        }

        private void InjectEmployee()
        {
            BuildEmployee();
        }

        private Employee BuildEmployee()
        {
            Employee empl = new Employee();
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
                        empl = new Employee()
                        {
                            NationalNumber = identity.NationalNumber,
                            EmployeeName = identity.FirstName1 + " " + identity.Surname,
                            ID = 0,
                            Address = addr.Zip + " " + addr.Street,
                        };
                    }
                    else { throw new System.IO.IOException("Kon de kaart niet lezen."); }
                }
                reader.DeactivateCard();
            }
            engine.Dispose();

            SelectedEmployee = empl;
            return empl;
        }

        private async void SaveEmployee()
        {
            string input = JsonConvert.SerializeObject(SelectedEmployee);

            // check insert (no ID assigned) or update (already an ID assigned)
            if (SelectedEmployee.ID == 0)
            {
                using (HttpClient client = new HttpClient())
                {
                    client.SetBearerToken(ApplicationVM.token.AccessToken);
                    HttpResponseMessage response = await client.PostAsync(lib.Constants.WEBURL + "api/Employee", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (response.IsSuccessStatusCode)
                    {
                        string output = await response.Content.ReadAsStringAsync();
                        SelectedEmployee.ID = Int32.Parse(output);
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
                    HttpResponseMessage response = await client.PutAsync(lib.Constants.WEBURL + "api/Employee", new StringContent(input, Encoding.UTF8, "application/json"));
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("error");
                    }
                }
            }
        }

        private async void DeleteEmployee()
        {
            using (HttpClient client = new HttpClient())
            {
                client.SetBearerToken(ApplicationVM.token.AccessToken);
                HttpResponseMessage response = await client.DeleteAsync(lib.Constants.WEBURL + "api/Employee/" + SelectedEmployee.ID);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("error");
                }
                else
                {
                    Employees.Remove(SelectedEmployee);
                }
            }
        }

    }

}
