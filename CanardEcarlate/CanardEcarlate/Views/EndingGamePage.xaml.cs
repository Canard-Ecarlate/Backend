using CanardEcarlate.Models;
using CanardEcarlate.Utils;
using CanardEcarlate.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CanardEcarlate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EndingGamePage : ContentPage
    {
        readonly EndingGameViewModel vm;
        public EndingGamePage()
        {
            InitializeComponent();

            vm = new EndingGameViewModel();

            CreateInterface();

            BindingContext = vm;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            quitButtonArrow.Source = ImageSource.FromResource(vm.quitButtonArrow);
            replayButtonArrow.Source = ImageSource.FromResource(vm.replayButtonArrow);


            if (vm.game.win.Equals("Bleu"))
            {
                msgVictoire.Text = " Les BLEUS ont gagné !!!";
                msgVictoire.TextColor = Color.Blue;
                ImgFinPartie.Source = ImageSource.FromResource(vm.kirby);
            }
            else if (vm.game.win.Equals("Rouge"))
            {
                msgVictoire.Text = " Les CANARDS ECARLATES ont gagné !!!";
                msgVictoire.TextColor = Color.Red;
                ImgFinPartie.Source = ImageSource.FromResource(vm.kirby);
            }


        }

        private void CreateInterface()
        {
            endingGameContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
        }


        private void QuitClicked(object sender, EventArgs e)
        {
            GlobalVariable.socketIO.On("leaveGame", response =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    string roomName = response.GetValue<string>();
                    if (roomName.Equals(vm.game.name))
                    {
                        Navigation.PopToRootAsync();
                    }
                });
            });
            GlobalVariable.Emit("leaveGame", vm.game.name);
        }

        private void ReplayClicked(object sender, EventArgs e)
        {
            GlobalVariable.Emit("startGame", GlobalVariable.CurrentRoom.name);
            Navigation.PushAsync(new InGamePage());
            Navigation.RemovePage(this);
        }
    }
}