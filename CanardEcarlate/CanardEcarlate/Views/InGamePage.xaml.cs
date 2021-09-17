using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CanardEcarlate.ViewsModels;
using CanardEcarlate.Utils;
using CanardEcarlate.Models;

namespace CanardEcarlate.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InGamePage : ContentPage
    {
        readonly InGameViewModel vm;

        public bool handVisible = false;

        private bool init = false;
        public InGamePage()
        {
            InitializeComponent();

            vm = new InGameViewModel();

            CreateInterface();

            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            GlobalVariable.Emit("getInfoGame", GlobalVariable.CurrentRoom.name);
            
            GlobalVariable.socketIO.On("getInfoGame", response =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    Game game = response.GetValue<Game>();
                    if (game != null && game.name == GlobalVariable.CurrentRoom.name)
                    {           
                        GlobalVariable.Game = vm.game = game;
                        ShowLastCard();
                        
                        if (vm.game.win == "")
                        {
                            vm.getPlayer();
                            if (handVisible) ShowHand();
                            else HideHand();

                            if (vm.isNewTour())
                            {
                                showNbCardsOfEachPlayer();
                            }
                            else
                            {
                                showNbCardsOfActualPlayer();
                            }
                        }
                        else
                        {
                            Navigation.PushAsync(new EndingGamePage());
                        }
                    }
                });
            });

            GlobalVariable.socketIO.On("leaveGame", response =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    string roomName = response.GetValue<string>();
                    if (roomName != null && roomName == GlobalVariable.CurrentRoom.name)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            Navigation.PopToRootAsync();
                        });
                    }
                });
            });

            vm.playerSeating = PlayerSeating();

            greenCardCounterImage.Source = ImageSource.FromResource(vm.greenCard);
            lastCardImage.Source = ImageSource.FromResource(vm.yellowCard);

            mainPlayerRole.Source = ImageSource.FromResource(vm.blackDuckImage);
            toggleVisibilityImage.Source = ImageSource.FromResource(vm.eye);
            handVisible = false;


            

            //imageLogo.Source = ImageSource.FromResource(vm.logo);

            //vm.Traductions();

            //if (DeviceInfo.Platform == DevicePlatform.Android)
            //    CreateInterface();
        }

        private void CreateInterface()
        {
            inGameContentPage.BackgroundImageSource = ImageSource.FromResource(vm.background);
        }

        private void ToggleVisibility(object o, EventArgs e)
        {
            if (handVisible)
            {
                HideHand();
            }
            else
            {
                ShowHand();
            }
            handVisible = !handVisible;
        }

        private void HideHand()
        {
            mainPlayerRole.Source = ImageSource.FromResource(vm.blackDuckImage);

            mainPlayerCard1.Source = ImageSource.FromResource(vm.backCard);
            mainPlayerCard2.Source = ImageSource.FromResource(vm.backCard);
            mainPlayerCard3.Source = ImageSource.FromResource(vm.backCard);
            mainPlayerCard4.Source = ImageSource.FromResource(vm.backCard);
            mainPlayerCard5.Source = ImageSource.FromResource(vm.backCard);

            if (vm.player.hand.Count() <= 0) mainPlayerCard1.IsVisible = false;
            if (vm.player.hand.Count() <= 1) mainPlayerCard2.IsVisible = false;
            if (vm.player.hand.Count() <= 2) mainPlayerCard3.IsVisible = false;
            if (vm.player.hand.Count() <= 3) mainPlayerCard4.IsVisible = false;
            if (vm.player.hand.Count() <= 4) mainPlayerCard5.IsVisible = false;

            toggleVisibilityImage.Source = ImageSource.FromResource(vm.eye);
        }

        private void ShowHand()
        {
            if (vm.player.role == "Rouge") mainPlayerRole.Source = ImageSource.FromResource(vm.redDuckImage);
            else mainPlayerRole.Source = ImageSource.FromResource(vm.blueDuckImage);

            if (vm.player.hand.Count() > 0)
            {
                mainPlayerCard1.Source = ImageSource.FromResource(getCardRessource(vm.player.hand[0]));
                mainPlayerCard1.IsVisible = true;
            }
            else mainPlayerCard1.IsVisible = false;
            if (vm.player.hand.Count() > 1)
            {
                mainPlayerCard2.Source = ImageSource.FromResource(getCardRessource(vm.player.hand[1]));
                mainPlayerCard2.IsVisible = true;
            }
            else mainPlayerCard2.IsVisible = false;
            if (vm.player.hand.Count() > 2)
            {
                mainPlayerCard3.Source = ImageSource.FromResource(getCardRessource(vm.player.hand[2]));
                mainPlayerCard3.IsVisible = true;
            }
            else mainPlayerCard3.IsVisible = false;
            if (vm.player.hand.Count() > 3)
            {
                mainPlayerCard4.Source = ImageSource.FromResource(getCardRessource(vm.player.hand[3]));
                mainPlayerCard4.IsVisible = true;
            }
            else mainPlayerCard4.IsVisible = false;
            if (vm.player.hand.Count() > 4)
            {
                mainPlayerCard5.Source = ImageSource.FromResource(getCardRessource(vm.player.hand[4]));
                mainPlayerCard5.IsVisible = true;
            }
            else mainPlayerCard5.IsVisible = false;

            toggleVisibilityImage.Source = ImageSource.FromResource(vm.barredEye);
        }

        private Dictionary<string, List<View>> PlayerSeating()
        {
            List<Player> opponents = new List<Player>(GlobalVariable.CurrentRoom.players);
            Player me = opponents.Find(player => player.userId == GlobalVariable.CurrentUser.UserId);
            opponents.Remove(me);

            var seating = new Dictionary<string, List<View>>();

            switch (opponents.Count)
            {
                case 2:
                    seating.Add(opponents[0].pseudo, new List<View>() { player3Name, player3Cards, player3Click });
                    seating.Add(opponents[1].pseudo, new List<View>() { player5Name, player5Cards, player5Click });
                    player3Cards.IsVisible = true;
                    player3Name.IsVisible = true;
                    player3Click.IsVisible = true;
                    player5Cards.IsVisible = true;
                    player5Name.IsVisible = true;
                    player5Click.IsVisible = true;

                    player3Name.Text = opponents[0].pseudo;
                    player5Name.Text = opponents[1].pseudo;
                    break;
                case 3:
                    seating.Add(opponents[0].pseudo, new List<View>() { player2Name, player2Cards, player2Click });
                    seating.Add(opponents[1].pseudo, new List<View>() { player4Name, player4Cards, player4Click });
                    seating.Add(opponents[2].pseudo, new List<View>() { player6Name, player6Cards, player6Click });
                    player2Cards.IsVisible = true;
                    player2Name.IsVisible = true;
                    player2Click.IsVisible = true;
                    player4Cards.IsVisible = true;
                    player4Cards.Source = ImageSource.FromResource(vm.backCards[5]);
                    player4Name.IsVisible = true;
                    player4Click.IsVisible = true;
                    player6Cards.IsVisible = true;
                    player6Cards.Source = ImageSource.FromResource(vm.backCards[5]);
                    player6Name.IsVisible = true;
                    player6Click.IsVisible = true;

                    player2Name.Text = opponents[0].pseudo;
                    player4Name.Text = opponents[1].pseudo;
                    player6Name.Text = opponents[2].pseudo;
                    break;
                case 4:
                    seating.Add(opponents[0].pseudo, new List<View>() { player1Name, player1Cards, player1Click });
                    seating.Add(opponents[1].pseudo, new List<View>() { player3Name, player3Cards, player3Click });
                    seating.Add(opponents[2].pseudo, new List<View>() { player5Name, player5Cards, player5Click });
                    seating.Add(opponents[3].pseudo, new List<View>() { player7Name, player7Cards, player7Click });
                    player1Cards.IsVisible = true;
                    player1Name.IsVisible = true;
                    player1Click.IsVisible = true;
                    player3Cards.IsVisible = true;
                    player3Name.IsVisible = true;
                    player3Click.IsVisible = true;
                    player5Cards.IsVisible = true;
                    player5Name.IsVisible = true;
                    player5Click.IsVisible = true;
                    player7Cards.IsVisible = true;
                    player7Name.IsVisible = true;
                    player7Click.IsVisible = true;

                    player1Name.Text = opponents[0].pseudo;
                    player3Name.Text = opponents[1].pseudo; 
                    player5Name.Text = opponents[2].pseudo;
                    player7Name.Text = opponents[3].pseudo;
                    break;
                case 5:
                    seating.Add(opponents[0].pseudo, new List<View>() { player1Name, player1Cards, player1Click });
                    seating.Add(opponents[1].pseudo, new List<View>() { player3Name, player3Cards, player3Click });
                    seating.Add(opponents[2].pseudo, new List<View>() { player4Name, player4Cards, player4Click });
                    seating.Add(opponents[3].pseudo, new List<View>() { player5Name, player5Cards, player5Click });
                    seating.Add(opponents[4].pseudo, new List<View>() { player7Name, player7Cards, player7Click });
                    player1Cards.IsVisible = true;
                    player1Name.IsVisible = true;
                    player1Click.IsVisible = true;
                    player3Cards.IsVisible = true;
                    player3Name.IsVisible = true;
                    player3Click.IsVisible = true;
                    player4Cards.IsVisible = true;
                    player4Name.IsVisible = true;
                    player4Click.IsVisible = true;
                    player5Cards.IsVisible = true;
                    player5Name.IsVisible = true;
                    player5Click.IsVisible = true;
                    player7Cards.IsVisible = true;
                    player7Name.IsVisible = true;
                    player7Click.IsVisible = true;

                    player1Name.Text = opponents[0].pseudo;
                    player3Name.Text = opponents[1].pseudo;
                    player4Name.Text = opponents[2].pseudo;
                    player5Name.Text = opponents[3].pseudo;
                    player7Name.Text = opponents[4].pseudo;
                    break;
                case 6:
                    seating.Add(opponents[0].pseudo, new List<View>() { player1Name, player1Cards, player1Click });
                    seating.Add(opponents[1].pseudo, new List<View>() { player2Name, player2Cards, player2Click });
                    seating.Add(opponents[2].pseudo, new List<View>() { player3Name, player3Cards, player3Click });
                    seating.Add(opponents[3].pseudo, new List<View>() { player5Name, player5Cards, player5Click });
                    seating.Add(opponents[4].pseudo, new List<View>() { player6Name, player6Cards, player6Click });
                    seating.Add(opponents[5].pseudo, new List<View>() { player7Name, player7Cards, player7Click });
                    player1Cards.IsVisible = true;
                    player1Name.IsVisible = true;
                    player1Click.IsVisible = true;
                    player2Cards.IsVisible = true;
                    player2Name.IsVisible = true;
                    player2Click.IsVisible = true;
                    player3Cards.IsVisible = true;
                    player3Name.IsVisible = true;
                    player3Click.IsVisible = true;
                    player5Cards.IsVisible = true;
                    player5Name.IsVisible = true;
                    player5Click.IsVisible = true;
                    player6Cards.IsVisible = true;
                    player6Name.IsVisible = true;
                    player6Click.IsVisible = true;
                    player7Cards.IsVisible = true;
                    player7Name.IsVisible = true;
                    player7Click.IsVisible = true;

                    player1Name.Text = opponents[0].pseudo;
                    player2Name.Text = opponents[1].pseudo;
                    player3Name.Text = opponents[2].pseudo;
                    player5Name.Text = opponents[3].pseudo;
                    player6Name.Text = opponents[4].pseudo;
                    player7Name.Text = opponents[5].pseudo;
                    break;
                case 7:
                    seating.Add(opponents[0].pseudo, new List<View>() { player1Name, player1Cards, player1Click });
                    seating.Add(opponents[1].pseudo, new List<View>() { player2Name, player2Cards, player2Click });
                    seating.Add(opponents[2].pseudo, new List<View>() { player3Name, player3Cards, player3Click });
                    seating.Add(opponents[3].pseudo, new List<View>() { player4Name, player4Cards, player4Click });
                    seating.Add(opponents[4].pseudo, new List<View>() { player5Name, player5Cards, player5Click });
                    seating.Add(opponents[5].pseudo, new List<View>() { player6Name, player6Cards, player6Click });
                    seating.Add(opponents[6].pseudo, new List<View>() { player7Name, player7Cards, player7Click });
                    player1Cards.IsVisible = true;
                    player1Name.IsVisible = true;
                    player1Click.IsVisible = true;
                    player2Cards.IsVisible = true;
                    player2Name.IsVisible = true;
                    player2Click.IsVisible = true;
                    player3Cards.IsVisible = true;
                    player3Name.IsVisible = true;
                    player3Click.IsVisible = true;
                    player4Cards.IsVisible = true;
                    player4Name.IsVisible = true;
                    player4Click.IsVisible = true;
                    player5Cards.IsVisible = true;
                    player5Name.IsVisible = true;
                    player5Click.IsVisible = true;
                    player6Cards.IsVisible = true;
                    player6Name.IsVisible = true;
                    player6Click.IsVisible = true;
                    player7Cards.IsVisible = true;
                    player7Name.IsVisible = true;
                    player7Click.IsVisible = true;

                    player1Name.Text = opponents[0].pseudo;
                    player2Name.Text = opponents[1].pseudo; 
                    player3Name.Text = opponents[2].pseudo;
                    player4Name.Text = opponents[3].pseudo; 
                    player5Name.Text = opponents[4].pseudo;
                    player6Name.Text = opponents[5].pseudo;
                    player7Name.Text = opponents[6].pseudo;
                    break;
            }

            return seating;
        }

        private void PickOnPlayer(object o, EventArgs e)
        {
            if (vm.game.actualPlayer == GlobalVariable.CurrentUser.Pseudo)
            {
                foreach(KeyValuePair<string,List<View>> entry in vm.playerSeating)
                {
                    if (entry.Value[2] == (Frame)o)
                    {
                        GlobalVariable.Emit("selectedPlayer", entry.Key);
                    }
                }
            }
        }

        private string getCardRessource(string cardFromBack)
        {
            if (cardFromBack == "verte") return vm.greenCard;
            else if (cardFromBack == "bombe") return vm.bombCard;
            else return vm.yellowCard;
        }

        private void showNbCardsOfActualPlayer()
        {
            List<View> playerCards;
            bool hasValue = vm.playerSeating.TryGetValue(vm.game.actualPlayer, out playerCards);
            if (hasValue)
            {
                Image image = (Image)playerCards[1];
                Player player = vm.game.players.Find(player => player.pseudo == vm.game.actualPlayer);
                image.Source = ImageSource.FromResource(vm.backCards[player.nbCards]);
            }
        }

        private void showNbCardsOfEachPlayer()
        {
            int compt = 0;
            foreach (Player player in vm.game.players)
            {
                List<View> playerCards;
                bool hasValue = vm.playerSeating.TryGetValue(player.pseudo, out playerCards);
                if (hasValue)
                {
                    Image image = (Image)playerCards[1];
                    image.Source = ImageSource.FromResource(vm.backCards[player.nbCards]);
                }
            }
        }

        private void ShowLastCard()
        {
            lastCardImage.Source = ImageSource.FromResource(getCardRessource(vm.game.lastCard));
        }
    }
}