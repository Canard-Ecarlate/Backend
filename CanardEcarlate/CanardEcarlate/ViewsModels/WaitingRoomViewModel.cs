using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CanardEcarlate.Utils;
using CanardEcarlate.Models;
using Xamarin.Forms;

namespace CanardEcarlate.ViewsModels
{
    public class WaitingRoomViewModel : ViewModelBase
    {
        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private string _logo;
        public string logo { get => _logo; set => RaisePropertyChanged(ref _logo, value); }

        private string _returnArrow;
        public string returnArrow { get => _returnArrow; set => RaisePropertyChanged(ref _returnArrow, value); }

        private string _creatorIcon;
        public string creatorIcon { get => _creatorIcon; set => RaisePropertyChanged(ref _creatorIcon, value); }

        private Room _currentRoom;
        public Room currentRoom { get => _currentRoom; set => RaisePropertyChanged(ref _currentRoom, value); }

        public WaitingRoomViewModel()
        {
            background = AuthBackground;
            logo = Logo;
            returnArrow = ReturnArrow;
            creatorIcon = CreatorIcon;
        }
    }
}