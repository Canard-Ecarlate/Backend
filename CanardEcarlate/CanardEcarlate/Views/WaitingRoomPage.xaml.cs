using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CanardEcarlate.ViewsModels;
using CanardEcarlate.Models;
using CanardEcarlate.Utils;

namespace CanardEcarlate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitingRoomPage : ContentPage
    {

        readonly WaitingRoomViewModel vm;
        public WaitingRoomPage()
        {
            InitializeComponent();
            vm = new WaitingRoomViewModel();
            CreateInterface();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            GlobalVariable.socketIO.On("updateOneRoom", response =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Room room = response.GetValue<Room>();
                    if (room.name == GlobalVariable.CurrentRoom.name)
                    {
                        vm.currentRoom = GlobalVariable.CurrentRoom = room;
                    }
                });
            });
            GlobalVariable.socketIO.On("startGame", response =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    List<Player> players = response.GetValue<List<Player>>();
                    foreach (Player player in players)
                    {
                        if (player.pseudo == GlobalVariable.CurrentUser.Pseudo)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Navigation.PushAsync(new InGamePage());
                            });
                            break;
                        }
                    }
                });
            });
            GlobalVariable.Emit("updateOneRoom", GlobalVariable.CurrentRoom.name);

            imageRetour.Source = ImageSource.FromResource(vm.returnArrow);
        }

        private void CreateInterface()
        {
            waitingRoomContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
        }

        private void GoToRoomList(object o, EventArgs e)
        {
            Navigation.PushAsync(new RoomListPage());
            Navigation.RemovePage(this);
        }

        private void StartGame(object o, EventArgs e)
        {
            GlobalVariable.Emit("startGame",GlobalVariable.CurrentRoom.name);
        }

        private void returnAction(object sender, EventArgs e)
        {
            GlobalVariable.Emit("leaveRoom", GlobalVariable.CurrentUser.UserId);
            Navigation.PopToRootAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            GlobalVariable.Emit("leaveRoom", GlobalVariable.CurrentUser.UserId);
            return base.OnBackButtonPressed();
        }
    }
}