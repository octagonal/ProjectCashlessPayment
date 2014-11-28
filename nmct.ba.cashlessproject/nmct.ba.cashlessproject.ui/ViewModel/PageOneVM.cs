using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace nmct.ba.cashlessproject.ui.ViewModel
{
    class PageOneVM : ObservableObject, IPage
    {
        public string Name
        {
            get { return "Page One"; }
        }
    }
}
