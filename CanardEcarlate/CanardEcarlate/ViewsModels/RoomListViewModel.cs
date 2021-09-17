using CanardEcarlate.Models;
using System;
using System.Collections.Generic;
using System.Text;
using CanardEcarlate.Utils;

namespace CanardEcarlate.ViewsModels
{
    class RoomListViewModel : ViewModelBase
    {

        private List<Room> listRooms;

        private string _background;
        public string background { get => _background; set => RaisePropertyChanged(ref _background, value); }

        private string _background_grey;
        public string background_grey { get => _background_grey; set => RaisePropertyChanged(ref _background_grey, value); }

        private string _logo;
        public string logo { get => _logo; set => RaisePropertyChanged(ref _logo, value); }

        private string _search;
        public string search { get => _search; set => RaisePropertyChanged(ref _search, value); }

        private string _submitError;
        public string submitError { get => _submitError; set => RaisePropertyChanged(ref _submitError, value); }

        private bool _isVisibleSubmit;
        public bool isVisibleSubmit { get => _isVisibleSubmit; set => RaisePropertyChanged(ref _isVisibleSubmit, value); }

        private List<Room> _listRoomsFiltered;
        public List<Room> listRoomsFiltered { get => _listRoomsFiltered; set => RaisePropertyChanged(ref _listRoomsFiltered, value); }

        public RoomListViewModel()
        {
            background = AuthBackground;
            logo = Logo;
            isVisibleSubmit = false;
            search = "";
            listRooms = new List<Room>();
            submitError = "Pas de salles trouvées";
        }

        public void GetListRoomsBySocket()
        {
            GlobalVariable.socketIO.On("updateAllRooms", response =>
            {
                isVisibleSubmit = false;
                listRooms = response.GetValue<List<Room>>();
                filterList();
            });
            GlobalVariable.Emit("updateAllRooms", null);
        }

        public void filterList()
        {
            Console.WriteLine("coucou"+search);
            isVisibleSubmit = false;
            listRoomsFiltered = new List<Room>(listRooms.FindAll(room => room.name.Contains(search)));
            if(listRoomsFiltered.Count == 0)
            {
                isVisibleSubmit = true;
            }
        }
    }

}
