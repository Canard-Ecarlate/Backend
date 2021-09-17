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
    public partial class CreateRoomPage : ContentPage
    {
        readonly CreateRoomViewModel vm;

        public double nbJoueursSelect;
        public CreateRoomPage()
        {
            InitializeComponent();

            vm = new CreateRoomViewModel();

            CreateInterface();

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            backImage.Source = ImageSource.FromResource(vm.retourImage);
            this.nbJoueursSelect = sliderNbPlayers.Value;
            //labelNbPlayers.Text = "" + this.nbJoueursSelect;

        }

        private void CreateInterface()
        {
            CreateRoomContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
        }

        private void backImageClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void sliderNbPlayersValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.nbJoueursSelect = Math.Round(e.NewValue) ;

            var newStep = Math.Round(e.NewValue / 1.0);
            sliderNbPlayers.Value = newStep * 1.0;
        }

        private void createButtonClicked(object sender, EventArgs e)
        {
            if (vm.createRoom())
            {
                Navigation.PushAsync(new WaitingRoomPage());
            }
        }
    }
}