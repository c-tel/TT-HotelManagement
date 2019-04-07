﻿using System;
using System.Collections.Generic;
using System.Linq;
using TTHohel.Manager;
using TTHohel.Services;
using TTHotel.Contracts.Clients;
using TTHotel.Contracts.Rooms;

namespace TTHohel.Models
{
    class AddBookingModel
    {
        public event Action<ClientDTO> AllClientsChanged;

        public AddBookingModel()
        {
            Storage.Instance.AllClientsChanged += OnClientsChanged;
        }

        private void OnClientsChanged(ClientDTO clientDTO)
        {
            AllClientsChanged?.Invoke(clientDTO);
        }

        public List<ClientDTO> GetClientsList()
        {
            return HotelApiClient.GetInstance().GetAllClients();
        }

        public List<RoomDTO> GetRoomsList(DateTime dateFrom, DateTime dateTo, int places)
        {
            return HotelApiClient.GetInstance().GetFreeRooms(dateFrom, dateTo, places);
        }

        public double CalculatePeriodPrice(DateTime dateFrom, DateTime dateTo, double price)
        {
            return ((dateTo - dateFrom).TotalDays+1) * price;
        }

        public bool CreateNewBooking(DateTime dateFrom, DateTime dateTo, int num, string telNum, string commentText)
        {
            if(HotelApiClient.GetInstance().CreateBooking(dateFrom, dateTo, num, telNum, commentText))
            {
                Storage.Instance.ChangeBookings();
                return true;
            }
            return false;
        }

        public void GoToMain()
        {
            NavigationManager.Instance.Navigate(ModesEnum.Main);
        }

        public void GoToAddClient()
        {
            var data = new ClientDisplayData
            {
                Client = null,
                Mode = ClientViewModes.Creation
            };
            Storage.Instance.ChangeClientDisplayData(data);

            NavigationManager.Instance.Navigate(ModesEnum.Client);
        }

        public string GetSelectedRoomComforts(RoomDTO selectedRoom)
        {
            return string.Join(", ", selectedRoom.Comforts);
        }

        public void GoToClient(ClientDTO selectedClient)
        {
            var data = new ClientDisplayData
            {
                Client = selectedClient,
                Mode = ClientViewModes.Info
            };
            Storage.Instance.ChangeClientDisplayData(data);

            NavigationManager.Instance.Navigate(ModesEnum.Client);
        }
    }
}
