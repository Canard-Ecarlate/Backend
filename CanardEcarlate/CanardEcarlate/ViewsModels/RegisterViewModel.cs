using CanardEcarlate.Controlers;
using CanardEcarlate.Utils;
using System;
using System.Collections.Generic;
using System.Text;

using Xamarin.Forms;

namespace CanardEcarlate.ViewsModels
{
    class RegisterViewModel : ViewModelBase
    {
        readonly UserControler user;

        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private string _logo;
        public string logo { get => _logo; set => RaisePropertyChanged(ref _logo, value); }

        private string _pseudo;
        public string pseudo { get => _pseudo; set => RaisePropertyChanged(ref _pseudo, value); }

        private string _email;
        public string email { get => _email; set => RaisePropertyChanged(ref _email, value); }

        private string _password;
        public string password { get => _password; set => RaisePropertyChanged(ref _password, value); }

        private string _confirmPassword;
        public string confirmPassword { get => _confirmPassword; set => RaisePropertyChanged(ref _confirmPassword, value); }

        private string _emailError;
        public string emailError { get => _emailError; set => RaisePropertyChanged(ref _emailError, value); }

        private string _passwordError;
        public string passwordError { get => _passwordError; set => RaisePropertyChanged(ref _passwordError, value); }

        private string _submitError;
        public string submitError { get => _submitError; set => RaisePropertyChanged(ref _submitError, value); }

        private bool _isVisibleEmail;
        public bool isVisibleEmail { get => _isVisibleEmail; set => RaisePropertyChanged(ref _isVisibleEmail, value); }

        private bool _isVisiblePassword;
        public bool isVisiblePassword { get => _isVisiblePassword; set => RaisePropertyChanged(ref _isVisiblePassword, value); }

        private bool _isVisibleSubmit;
        public bool isVisibleSubmit { get => _isVisibleSubmit; set => RaisePropertyChanged(ref _isVisibleSubmit, value); }

        public RegisterViewModel()
        {
            user = new UserControler();
            background = AuthBackground;
            logo = Logo;
            isVisibleEmail = false;
            isVisiblePassword = false;
            isVisibleSubmit = false;
        }

        public bool pseudoVerification()
        {
            if(pseudo != "" && pseudo != null && !(pseudo.Contains("/") || pseudo.Contains(";") || pseudo.Contains("\\") || pseudo.Contains("\"") || pseudo.Contains("'")))
            {
                return true;
            }
            else
            {
                isVisibleSubmit = true;
                submitError = "Ne doit pas contenir; / \\ & ' \"";
                return false;
            }
        }

        public bool emailVerification()
        {
            if(email != "" && email != null && RegexUtilities.IsValidEmail(email))
            {
                return true;
            }
            else
            {
                isVisibleEmail = true;
                emailError = "Mail Invalide";
                return false;
            }
        }

        public bool passwordVerification()
        {
            if (password != "" && password != null && password == confirmPassword)
            {
                return true;
            }
            else
            {
                isVisiblePassword = true;
                passwordError = "Les mots de passe ne correspondent pas";
                return false;
            }
        }

        public bool signUp()
        {
            emailError = "";
            passwordError = "";
            submitError = "";
            GlobalVariable.CurrentUser.Error = null;
            if (password == "" || password == null || email == "" || email == null || pseudo == "" && pseudo == null)
            {
                isVisibleSubmit = true;
                submitError = "Tous les champs sont requis";
                return false;
            }
            if (pseudoVerification() && emailVerification() && passwordVerification())
            {
                if (user.signUp(pseudo, email, password))
                {
                    if (user.signIn(pseudo, password)) {
                        if (GlobalVariable.CurrentUser.Error != null)
                        {
                            isVisibleSubmit = true;
                            submitError = GlobalVariable.CurrentUser.Error;
                            return false;
                        }
                        return true;
                    }
                }
                else
                {
                    isVisibleSubmit = true;
                    submitError = GlobalVariable.CurrentUser.Error;
                }
            }
            return false;
        }
    }
}
