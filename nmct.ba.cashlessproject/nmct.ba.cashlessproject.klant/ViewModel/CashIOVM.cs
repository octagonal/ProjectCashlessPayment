using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using nmct.ba.cashlessproject.model.it;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.klant.ViewModel
{
    class CashIOVM : ViewModelBase, IPage
    {
        public RelayCommand<object> AddCashCommand { get; private set; }
        public RelayCommand PrintBalanceCommand { get; private set; }

        public string Name { get { return "CashIO"; } }

        public int MaxCash { get; private set; }

        public Customer customer { get; set;}

        public CashIOVM()
        {
            MaxCash = 100;

            AddCashCommand = new RelayCommand<object>(
                (obj) => CurrentAmountUpload += Convert.ToDouble(obj), 
                (obj) => ((CurrentAmountUpload + Convert.ToDouble(obj)) <= MaxCash == true)
            );

            PrintBalanceCommand = new RelayCommand(
                () => MessageBox.Show("Printer malfunction.","ERROR"),
                () => true
            );

            /*
            customer = new Customer()
            {
                Address = "Hamme",
                Balance = 90,
                CustomerName = "Anthony",
                ID = 20,
                Picture = null
            };*/
        }

        private double _currentAmountUpload;
        public double CurrentAmountUpload
        {
            get { return _currentAmountUpload; }
            set { _currentAmountUpload = value; RaisePropertyChanged("CurrentAmountUpload"); AddCashCommand.RaiseCanExecuteChanged(); }
        }

    }
}
