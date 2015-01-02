using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Thinktecture.IdentityModel.Client;

namespace nmct.ba.cashlessproject.klant.ViewModel
{
    class ApplicationVM : ObservableObject
    {
        public static TokenResponse token = null;

        public ApplicationVM()
        {
            CurrentPage = new CashIOVM();
        }

        private object currentPage;
        public object CurrentPage
        {
            get { return currentPage; }
            set { currentPage = value; OnPropertyChanged("CurrentPage"); }
        }

        private ObservableCollection<IPage> pages;
        public ObservableCollection<IPage> Pages
        {
            get
            {
                if (pages == null)
                    pages = new ObservableCollection<IPage>();
                return pages;
            }
            set { pages = value; OnPropertyChanged("Pages"); }
        }

        public ICommand ChangePageCommand
        {
            get { return new RelayCommand<IPage>(ChangePage); }
        }

        private RelayCommand _logoutCommand;
        public RelayCommand LogoutCommand
        {
            get
            {
                if (_logoutCommand == null)
                    _logoutCommand = new RelayCommand(Logout, IsLoggedIn);
                return _logoutCommand;
            }
        }

        private bool IsLoggedIn()
        {
            return (ApplicationVM.token != null);
        }

        public void ChangePage(IPage page)
        {
            CurrentPage = page;
        }

        public void LoggedIn()
        {
            LogoutCommand.RaiseCanExecuteChanged();
        }

        private void Logout()
        {
            ApplicationVM.token = null;
            Pages.Clear();
            //ChangePage(new LoginVM());
            LogoutCommand.RaiseCanExecuteChanged();
        }
    }
}
