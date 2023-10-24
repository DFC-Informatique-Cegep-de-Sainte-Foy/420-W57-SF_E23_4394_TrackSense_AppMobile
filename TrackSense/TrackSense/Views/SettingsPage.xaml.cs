using Microsoft.Maui.Controls;
using TrackSense.ViewModels;

namespace TrackSense.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage(SettingsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

	protected override void OnAppearing()
	{
        base.OnAppearing();

		if (BindingContext is SettingsViewModel viewModel)
		{
            Shell.Current.CurrentPage.FindByName<Grid>("screenRotationGrid").IsVisible = viewModel.IsDeviceConnected();
        }
    }
}