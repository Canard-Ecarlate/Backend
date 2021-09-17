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
    public partial class RegisterPage : ContentPage
    {
        readonly RegisterViewModel vm;
        public RegisterPage()
        {
            InitializeComponent();

            vm = new RegisterViewModel();

            CreateInterface();

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            //imageLogo.Source = ImageSource.FromResource(vm.logo);

            //vm.Traductions();

            //if (DeviceInfo.Platform == DevicePlatform.Android)
            //    CreateInterface();
        }

        private void CreateInterface()
        {
            registerContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
        }

        private void GoToLogin(object o, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage());
            Navigation.RemovePage(this);
        }

        private void signUp(object o, EventArgs e)
        {
            if (vm.signUp() && GlobalVariable.CurrentUser.Error == null)
            {
                Navigation.PushAsync(new RoomListPage());
                Navigation.RemovePage(this);
            }
        }
    }
}