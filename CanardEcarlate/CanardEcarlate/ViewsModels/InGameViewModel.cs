using System;
using System.Collections.Generic;
using System.Text;
using CanardEcarlate.Models;
using CanardEcarlate.Controlers;
using CanardEcarlate.Utils;
using Xamarin.Forms;

namespace CanardEcarlate.ViewsModels
{
    class InGameViewModel : ViewModelBase
    {
        private GameControler gameControler;

        private Game _game;
        public Game game { get => _game; set => RaisePropertyChanged(ref _game, value); }
        
        private Player _player;
        public Player player { get => _player; set => RaisePropertyChanged(ref _player, value); }


        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private List<string> _backCards;
        public List<string> backCards { get => _backCards; set => RaisePropertyChanged(ref _backCards, value); }


        private string _greenCard;
        public string greenCard { get => _greenCard; set => RaisePropertyChanged(ref _greenCard, value); }

        private string _backCard;
        public string backCard { get => _backCard; set => RaisePropertyChanged(ref _backCard, value); }

        private string _yellowCard;
        public string yellowCard { get => _yellowCard; set => RaisePropertyChanged(ref _yellowCard, value); }

        private string _bombCard;
        public string bombCard { get => _bombCard; set => RaisePropertyChanged(ref _bombCard, value); }

        private string _blueDuckImage;
        public string blueDuckImage { get => _blueDuckImage; set => RaisePropertyChanged(ref _blueDuckImage, value); }

        private string _redDuckImage;
        public string redDuckImage { get => _redDuckImage; set => RaisePropertyChanged(ref _redDuckImage, value); }

        private string _blackDuckImage;
        public string blackDuckImage { get => _blackDuckImage; set => RaisePropertyChanged(ref _blackDuckImage, value); }

        private string _eye;
        public string eye { get => _eye; set => RaisePropertyChanged(ref _eye, value); }

        private string _barredEye;
        public string barredEye { get => _barredEye; set => RaisePropertyChanged(ref _barredEye, value); }
        
        private int _actualTour;
        public int actualTour { get => _actualTour; set => RaisePropertyChanged(ref _actualTour, value); }


        private Dictionary<string, List<View>> _playerSeating;
        public Dictionary<string, List<View>> playerSeating { get => _playerSeating; set => RaisePropertyChanged(ref _playerSeating, value); }

        public InGameViewModel()
        {
            gameControler = new GameControler();

            background = TableBackground;

            backCards = new List<string>
            {
                ZeroCards,
                OneCards,
                TwoCards,
                ThreeCards,
                FourCards,
                FiveCards
            };

            backCard = BackCard;
            greenCard = GreenCard;
            yellowCard = YellowCard;
            bombCard = BombCard;

            blueDuckImage = BlueDuck;
            redDuckImage = RedDuck;
            blackDuckImage = BlackDuck;

            eye = Eye;
            barredEye = BarredEye;

            actualTour = 0;
        }

        public void getPlayer()
        {
            if (gameControler.getPlayer(GlobalVariable.CurrentUser.UserId))
            {
                player = GlobalVariable.Player;
            }
        }

        public bool isNewTour ()
        {
            bool res = game.numOfTour + 1 != actualTour;
            if (res)
            {
                actualTour = game.numOfTour + 1;
                return true;
            }
            return false;
        }
    }
}
