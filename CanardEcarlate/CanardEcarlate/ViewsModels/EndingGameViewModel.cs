using CanardEcarlate.Models;
using CanardEcarlate.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace CanardEcarlate.ViewsModels
{
    public class EndingGameViewModel : ViewModelBase
    {
        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private string _logo;
        public string logo { get => _logo; set => RaisePropertyChanged(ref _logo, value); }

        private string _quitButtonArrow;
        public string quitButtonArrow { get => _quitButtonArrow; set => RaisePropertyChanged(ref _quitButtonArrow, value); }

        private string _replayButtonArrow;
        public string replayButtonArrow { get => _replayButtonArrow; set => RaisePropertyChanged(ref _replayButtonArrow, value); }

        private string _kirby;
        public string kirby { get => _kirby; set => RaisePropertyChanged(ref _kirby, value); }

        public Game game;
        public EndingGameViewModel()
        {
            background = AuthBackground;
            logo = Logo;
            quitButtonArrow = StraightArrowLeft;
            replayButtonArrow = StraightArrowRight;
            kirby = Kirby;
            game = GlobalVariable.Game;
        }
    }
}
