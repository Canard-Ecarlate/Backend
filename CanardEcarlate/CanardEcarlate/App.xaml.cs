using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CanardEcarlate.Views;
using CanardEcarlate.Utils;
using CanardEcarlate.Controlers;

namespace CanardEcarlate
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            GlobalVariable.Emit("removePlayer", GlobalVariable.CurrentUser.UserId);
            GlobalVariable.socketIO.DisconnectAsync();
        }

        protected override void OnResume()
        {
            UserControler.AddPlayerBySocket();
            GlobalVariable.ConnectSocket();
        }
    }
}
