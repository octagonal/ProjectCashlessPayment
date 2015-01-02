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

        public class MyButtonSimple : Button
        {
            // Create a custom routed event by first registering a RoutedEventID 
            // This event uses the bubbling routing strategy 
            public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent(
                "Tap", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MyButtonSimple));

            // Provide CLR accessors for the event 
            public event RoutedEventHandler Tap
            {
                add { AddHandler(TapEvent, value); }
                remove { RemoveHandler(TapEvent, value); }
            }

            // This method raises the Tap event 
            void RaiseTapEvent()
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(MyButtonSimple.TapEvent);
                RaiseEvent(newEventArgs);
            }
            // For demonstration purposes we raise the event when the MyButtonSimple is clicked 
            protected override void OnClick()
            {
                RaiseTapEvent();
            }

        }

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
