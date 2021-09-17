using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CanardEcarlate.Controlers;
using CanardEcarlate.Utils;
using Xamarin.Essentials;

namespace CanardEcarlate.ViewsModels
{
    public class LoginViewModel : ViewModelBase
    {
        readonly UserControler userControler;

        private string _pseudo;
        public string pseudo { get => _pseudo; set => RaisePropertyChanged(ref _pseudo, value); }

        private string _password;
        public string password { get => _password; set => RaisePropertyChanged(ref _password, value); } 

        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private string _logo;
        public string logo { get => _logo; set => RaisePropertyChanged(ref _logo, value); }

        private string _submitError;
        public string submitError { get => _submitError; set => RaisePropertyChanged(ref _submitError, value); }

        private bool _isVisibleSubmit;
        public bool isVisibleSubmit { get => _isVisibleSubmit; set => RaisePropertyChanged(ref _isVisibleSubmit, value); }

        public LoginViewModel()
        {
            background = AuthBackground;
            userControler = new UserControler();
            logo = Logo;
            isVisibleSubmit = false;
            
        }

        public bool signIn()
        {
            submitError = "";
            GlobalVariable.CurrentUser.Error = null;
            if (userControler.signIn(pseudo, password))
            {
                if (GlobalVariable.CurrentUser.Error != null)
                {
                    isVisibleSubmit = true;
                    submitError = GlobalVariable.CurrentUser.Error;
                    return false;
                }
                stockIndentifiantAsync(pseudo, password);
                return true;
            }
            isVisibleSubmit = true;
            submitError = GlobalVariable.CurrentUser.Error;
            return false;
        }

        public bool signInAuto() {
            string pseudoCookie = SecureStorage.GetAsync("pseudo").Result;
            string passwordCookie = SecureStorage.GetAsync("password").Result;
            if (pseudoCookie != null && passwordCookie != null)
            {
                this.pseudo = pseudoCookie;
                this.password = passwordCookie;
                return signIn();
            }
            else {
                return false;
            }
        }

        public async void stockIndentifiantAsync(string pseudo, string password) {
            await SecureStorage.SetAsync("pseudo", pseudo);
            await SecureStorage.SetAsync("password", password);
        }
    }
}