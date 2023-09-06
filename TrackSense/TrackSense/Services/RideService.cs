﻿using CommunityToolkit.Mvvm.ComponentModel;
using Plugin.BLE.Abstractions.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackSense.Data;
using TrackSense.Entities;

namespace TrackSense.Services
{
    public class RideService
    {
        private RideData _rideData;
        BluetoothService _bluetoothService;

        public RideService(RideData rideData, BluetoothService bluetoothService)
        {
            _rideData = rideData;
            _bluetoothService = bluetoothService;

            //BluetoothObserver bluetoothObserver = new BluetoothObserver(this._bluetoothService,
            //    async (value) =>
            //    {
            //        if (value.Type == BluetoothEventType.SENDING_RIDE_STATS)
            //        {
            //            this.ReceiveRideData(value.RideData);
            //            IDevice connectedDevice = this._bluetoothService.GetConnectedDevice();
            //            Guid completedRideServiceUID = new Guid("62ffab64-3646-4fb9-88d8-541deb961192");
            //            IService completedRideService = await connectedDevice.GetServiceAsync(completedRideServiceUID);
            //            Guid characteristicIsReadyUID = new Guid("9456444a-4b5f-11ee-be56-0242ac120002");
            //            ICharacteristic characteristicIsReceived = await completedRideService.GetCharacteristicAsync(characteristicIsReadyUID);
            //            byte[] dataFalse = Encoding.UTF8.GetBytes("false");
            //            await characteristicIsReceived.WriteAsync(dataFalse);
            //        }
            //        else if (value.Type == BluetoothEventType.SENDING_RIDE_POINT)
            //        {
            //            this.ReceivePoints(value.RideData);
            //        }
            //    });
        }

        public async void ReceivePoints(CompletedRide rideData)
        {
            throw new NotImplementedException();
        }

        internal async void ReceiveRideData(CompletedRide rideData)
        {
            if (rideData is null)
            {
                throw new ArgumentNullException(nameof(rideData));
            }

            IDevice connectedDevice = this._bluetoothService.GetConnectedDevice();
            Guid completedRideServiceUID = new Guid("62ffab64-3646-4fb9-88d8-541deb961192");
            IService completedRideService = await connectedDevice.GetServiceAsync(completedRideServiceUID);
            Guid characteristicIsReadyUID = new Guid("9456444a-4b5f-11ee-be56-0242ac120002");
            ICharacteristic characteristicIsReceived = await completedRideService.GetCharacteristicAsync(characteristicIsReadyUID);
            byte[] dataFalse = Encoding.UTF8.GetBytes("false");
            await characteristicIsReceived.WriteAsync(dataFalse);

            _rideData.AddRide(rideData);
            
            //Vérifier connection à internet et envoyer à API.
        }
    }
}
