using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CanardEcarlate.ViewsModels;
using CanardEcarlate.Utils;

namespace CanardEcarlate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        readonly LoginViewModel vm;
        public LoginPage()
        {
            GlobalVariable.ConnectSocket();

            InitializeComponent();

            vm = new LoginViewModel();

            CreateInterface();

            BindingContext = vm;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //vm.Traductions();

            //if (DeviceInfo.Platform == DevicePlatform.Android)
            //    CreateInterface();

            if (vm.signInAuto())
            {
                Navigation.PushAsync(new RoomListPage());
                Navigation.RemovePage(this);
            }

        }

        private void CreateInterface()
        {
            loginContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
        }

        private void GoToRegister(object o, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage());
            Navigation.RemovePage(this);
        }
        private void GoToForgotPassword(object o, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage());
            Navigation.RemovePage(this);
        }

        private void signIn(object sender, EventArgs e)
        {
            if (vm.signIn())
            {
                Navigation.PushAsync(new RoomListPage());
                Navigation.RemovePage(this);
            }
        }
    }
}