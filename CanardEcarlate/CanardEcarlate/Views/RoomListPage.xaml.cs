using CanardEcarlate.ViewsModels;
using SocketIOClient;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CanardEcarlate.Models;
using CanardEcarlate.Utils;
using System.Collections.Generic;
using Android.Widget;
using Android.Content;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace CanardEcarlate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomListPage : ContentPage
    {
        readonly RoomListViewModel vm;
        Context context = Android.App.Application.Context;

        //temp, voire plus bas
        static bool fromLogin=true;
        public RoomListPage()
        {
            InitializeComponent();

            vm = new RoomListViewModel();

            CreateInterface();

            BindingContext = vm;

            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Toast.MakeText(context, "Connexion réussie en tant que "+ GlobalVariable.CurrentUser.Pseudo, ToastLength.Long).Show();


            vm.GetListRoomsBySocket();

            //reconnexion (temporaire, à modifier dans le back)
            if (!fromLogin) return;
            fromLogin = false;
            while (vm.listRoomsFiltered == null) { }
            foreach (Room room in vm.listRoomsFiltered)
            {
                foreach (Player p in room.players)
                {
                    if (p.pseudo == GlobalVariable.CurrentUser.Pseudo)
                    {
                        GlobalVariable.CurrentRoom = room;
                        if (room.isInGame)
                        {
                            Navigation.PushAsync(new InGamePage());
                            //Navigation.RemovePage(this);
                        }
                        else
                        {
                            JoinRoom info = new JoinRoom()
                            {
                                userId = GlobalVariable.CurrentUser.UserId,
                                roomName = room.name
                            };
                            GlobalVariable.Emit("joinRoom", info);
                            Navigation.PushAsync(new WaitingRoomPage());
                        }
                        return;
                    }
                }
            }

            //vm.Traductions();
            //if (DeviceInfo.Platform == DevicePlatform.Android)
            //    CreateInterface();
        }

        private void CreateInterface()
        {
            roomListContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
            CreateDataTemplate();
        }

        private async void GoToWaitingRoom(object o, EventArgs e)
        {
            Frame frame = (Frame)o;
            TapGestureRecognizer tapGesture = (TapGestureRecognizer)frame.GestureRecognizers[0];
            Label label = (Label)tapGesture.CommandParameter;
            frame.GestureRecognizers.Remove(tapGesture);
            GlobalVariable.CurrentRoom.name = label.Text;

            Room result = vm.listRoomsFiltered.Find(room => room.name == GlobalVariable.CurrentRoom.name);
            if (result != null && result.RoomIsNotFull)
            {
                JoinRoom info = new JoinRoom()
                {
                    userId = GlobalVariable.CurrentUser.UserId,
                    roomName = GlobalVariable.CurrentRoom.name
                };
                GlobalVariable.Emit("joinRoom", info);
                await Navigation.PushAsync(new WaitingRoomPage());
            }
            else
            {
                await DisplayAlert("Info", "Salle complète !", "Ok");
            }
        }

        private void GoToCreate(object o, EventArgs e)
        {
            Navigation.PushAsync(new CreateRoomPage());
        }

        private void CreateDataTemplate()
        {
            DataTemplate template = new DataTemplate(() =>
            {
                Frame frame = new Frame { HeightRequest = 100, Padding = 0, Margin = 10, HasShadow = false, BackgroundColor = Color.Transparent };
                Grid content = new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) }, new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) } },
                    RowDefinitions = { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }, new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) } },
                    Padding = new Thickness(0, 10, 0, 0),
                };
                //Image backgroundImage = new Image { Source = ImageSource.FromResource( vm.background_grey), Aspect=Aspect.AspectFill};
                Frame subcontentFrame = new Frame { CornerRadius = 20, Padding = 0, HasShadow = false, BackgroundColor = Color.Transparent };
                Grid subcontent = new Grid {
                    ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(4, GridUnitType.Star) }, new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) } },
                    RowDefinitions = { new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) }, new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) } },
                    Padding = new Thickness(10, 0, 10, 10),
                    HeightRequest = 100,
                };
                subcontent.SetDynamicResource(BackgroundColorProperty, "FiligraneWhite");

                Frame roomNameFrame = new Frame { CornerRadius = 100, Margin = new Thickness(0, -10, 0, 0), Padding = 0 };
                Label roomName = new Label { FontAttributes = FontAttributes.Bold, HeightRequest = 40, Padding = new Thickness(10, 8, 10, 0), HorizontalTextAlignment = TextAlignment.Center };
                roomName.SetDynamicResource(Label.TextColorProperty, "DarkText");

                Label nbPlayers = new Label { HorizontalTextAlignment = TextAlignment.End, Padding = new Thickness(10, 10, 10, 0) };
                nbPlayers.SetDynamicResource(Label.TextColorProperty, "DarkText");
                //Image creatorImage = new Image { Source = , WidthRequest = 10 };
                Label creator = new Label { VerticalOptions = LayoutOptions.Center, Margin = new Thickness(0, 0, 0, 0)};
                creator.SetDynamicResource(Label.TextColorProperty, "DarkText");

                Frame joinFrame = new Frame { BorderColor = Color.Transparent, Padding = 10, Margin = 0, CornerRadius = 100, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.End };
                joinFrame.SetDynamicResource(BackgroundColorProperty, "ButtonGreen");
                Label join = new Label { Text = "Rejoindre", FontAttributes = FontAttributes.Bold, BackgroundColor = Color.Transparent, VerticalTextAlignment = TextAlignment.Center, HorizontalTextAlignment = TextAlignment.Center};
                join.SetDynamicResource(Label.TextColorProperty, "White");

                roomName.SetBinding(Label.TextProperty, "name");
                nbPlayers.SetBinding(Label.TextProperty, "GetNbPlayersInRoom");
                creator.SetBinding(Label.TextProperty, "GetCreatorPseudo");
                joinFrame.Content = join;

                TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer { NumberOfTapsRequired = 1 };
                tapGestureRecognizer.Tapped += GoToWaitingRoom;
                tapGestureRecognizer.CommandParameter = roomName;
                frame.GestureRecognizers.Add(tapGestureRecognizer);

                roomNameFrame.Content = roomName;

                subcontent.Children.Add(nbPlayers, 1, 0);
                subcontent.Children.Add(creator, 0, 1);
                //subcontent.Children.Add(creatorImage, 0, 1);
                subcontent.Children.Add(joinFrame, 1, 1);

                subcontentFrame.Content = subcontent;

                content.Children.Add(subcontentFrame);
                Grid.SetRowSpan(subcontentFrame, 2);
                Grid.SetColumnSpan(subcontentFrame, 2);
                content.Children.Add(roomNameFrame);

                frame.Content = content;

                return new ViewCell { View = frame };
            });

            roomList.ItemTemplate =template;
        }

        protected override bool OnBackButtonPressed()
        {
            base.OnBackButtonPressed();

            //new thread
            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("DECONNEXION", "Voulez-vous vous déconnecter ?", "Non", "Oui");

                if (!result)
                {
                    GlobalVariable.Emit("removePlayer", GlobalVariable.CurrentUser.UserId);
                    GlobalVariable.socketIO.DisconnectAsync();
                    SecureStorage.Remove("pseudo");
                    SecureStorage.Remove("password");
                    Navigation.PushAsync(new LoginPage());
                    Navigation.RemovePage(this);
                }

            });

            return true; //Do not navigate backwards by pressing the button
        }

        private void filterList(object o, EventArgs e)
        {
            vm.filterList();
        }
    }
}

