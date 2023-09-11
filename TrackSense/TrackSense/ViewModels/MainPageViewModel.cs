﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using TrackSense.Entities.Exceptions;
using TrackSense.Models;
using TrackSense.Services;
using TrackSense.Services.Bluetooth;
using TrackSense.Views;

namespace TrackSense.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    BluetoothService _bluetoothService;
    RideService _rideService;
    public ObservableCollection<CompletedRideSummary> CompletedRideSummaries { get; } = new();

    [ObservableProperty]
    bool isConnected;

    [ObservableProperty]
    bool isReceivingData;

    [ObservableProperty]
    bool isRefreshing;

    public MainPageViewModel(BluetoothService btService, RideService rideService)
    {
        Title = "Accueil";
        _bluetoothService = btService;
        _rideService = rideService;

        BluetoothObserver bluetoothObserver = new BluetoothObserver(this._bluetoothService,
            async (value) =>
            {
                switch (value.Type)
                {
                    case BluetoothEventType.CONNECTION:
                        isConnected = true;
                        break;
                    case BluetoothEventType.DECONNECTION:
                        isConnected = false;
                        break;
                    case BluetoothEventType.SENDING_RIDE_STATS:
                        isReceivingData = true;
                        this._rideService.ReceiveRideData(value.RideData);
                        if (value.RideData is Entities.CompletedRide ride)
                        {
                            await Shell.Current.DisplayAlert("Ajout", $"Le trajet {ride.CompletedRideId} est reçu!", "Ok");
                        }
                        break;
                    case BluetoothEventType.SENDING_RIDE_POINT:
                        isReceivingData = true;
                        this._rideService.ReceivePoint(value.RidePoint);
                        isReceivingData = false;
                        break;
                    default:
                        break;
                }
            });
    }

    [RelayCommand]
    async Task GoToTrackSenseDevices()
    {
        if (!IsConnected)
        {
            await Shell.Current.GoToAsync(nameof(TrackSenseDevicesPage));
        }
    }

    [RelayCommand]
    async Task GoToDetailsAsync(CompletedRideSummary rideSummary)
    {
        if (rideSummary is null)
        {
            return;
        }

        Entities.CompletedRide completedRide = await _rideService.GetCompletedRide(rideSummary.CompletedRideId);

        //Référence le shell, donc pas bonne pratique, il faudrait une interface.
        await Shell.Current.GoToAsync($"{nameof(CompletedRidePage)}", true,
            new Dictionary<string, object>
            {
                    {"Ride", completedRide }
            });
    }

    [RelayCommand]
    async Task GetCompletedRidesAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            IsRefreshing = true;

            List<TrackSense.Entities.CompletedRideSummary> completedRides = await _rideService.GetUserCompletedRides();

            if (CompletedRideSummaries.Count != 0)
            {
                CompletedRideSummaries.Clear();
            }

            foreach (TrackSense.Entities.CompletedRideSummary ride in completedRides)
            {
                CompletedRideSummaries.Add(new CompletedRideSummary(ride)); // si on a trop de données, ne pas utiliser cette méthode car lève un évenement pour chaque ajout
                // si on a trop de données, créer une nouvelle liste ou une nouvelle ObservableCollection et l'assigner à CompletedRides ou trouver des
                //library helpers qui ont des observableRange collections qui feront de l'ajout de batch
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            await Shell.Current.DisplayAlert("Erreur", $"Une erreur est survenue lors de la récupération des trajets: {ex.Message}", "Ok");
        }
        finally
        {
            IsBusy = false;
            IsRefreshing = false;
        }
    }

    [RelayCommand]
    async Task SimulateRideReceptionAsync()
    {
        this._bluetoothService.SimulateRideReception();
    }

    [RelayCommand]
    async Task SimulatePointsReceptionAsync()
    {
        this._bluetoothService.SimulatePointsReception();
    }
}
