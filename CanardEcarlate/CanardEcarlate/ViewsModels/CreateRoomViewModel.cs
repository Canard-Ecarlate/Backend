using CanardEcarlate.Controlers;
using CanardEcarlate.Models;
using CanardEcarlate.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CanardEcarlate.ViewsModels
{
    class CreateRoomViewModel : ViewModelBase
    {
        RoomControler roomControler;

        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private string _logo;
        public string logo { get => _logo; set => RaisePropertyChanged(ref _logo, value); }

        private string _retourImage;
        public string retourImage { get => _retourImage; set => RaisePropertyChanged(ref _retourImage, value); }

        private string _roomName;
        public string roomName { get => _roomName; set => RaisePropertyChanged(ref _roomName, value); }

        private int _nbOfPlayers;
        public int nbOfPlayers { get => _nbOfPlayers; set => RaisePropertyChanged(ref _nbOfPlayers, value); }

        private string _submitError;
        public string submitError { get => _submitError; set => RaisePropertyChanged(ref _submitError, value); }

        private bool _isVisibleSubmit;
        public bool isVisibleSubmit { get => _isVisibleSubmit; set => RaisePropertyChanged(ref _isVisibleSubmit, value); }

        public CreateRoomViewModel()
        {
            roomControler = new RoomControler();
            background = AuthBackground;
            logo = Logo;
            retourImage = ReturnArrow;
            nbOfPlayers = 3;
            isVisibleSubmit = false;
        }

        public bool createRoom()
        {
            isVisibleSubmit = false;
            submitError = "";
            if (roomControler.createRoom(roomName, nbOfPlayers.ToString()))
            {
                GlobalVariable.Emit("updateAllRooms", null);
                JoinRoom info = new JoinRoom()
                {
                    userId = GlobalVariable.CurrentUser.UserId,
                    roomName = GlobalVariable.CurrentRoom.name
                };
                GlobalVariable.Emit("joinRoom", info);
                return true;
            }
            isVisibleSubmit = true;
            submitError = GlobalVariable.CurrentUser.Error;
            return false;
        }
    }
}
