using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CanardEcarlate.ViewsModels;

namespace CanardEcarlate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        readonly ForgotpasswordViewModel vm;
        public ForgotPasswordPage()
        {
            InitializeComponent();

            vm = new ForgotpasswordViewModel();

            CreateInterface();

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //vm.Traductions();

            //if (DeviceInfo.Platform == DevicePlatform.Android)
            //    CreateInterface();
        }

        private void CreateInterface()
        {
            forgotPasswordContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
        }

        private void GoToLogin(object o, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }
    }
}