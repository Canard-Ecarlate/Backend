using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace CanardEcarlate.ViewsModels
{
    public class ForgotpasswordViewModel : ViewModelBase
    {
        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private string _logo;
        public string logo { get => _logo; set => RaisePropertyChanged(ref _logo, value); }

        public ForgotpasswordViewModel()
        {
            background = AuthBackground;
            logo = Logo;
        }
    }
}