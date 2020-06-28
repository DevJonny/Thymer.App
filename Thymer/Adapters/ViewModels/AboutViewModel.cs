using System;
using System.Windows.Input;
using MvvmHelpers;
using Thymer.Adapters.Services.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel(INavigationService navigationService)
        {
            Title = "About";

            OpenWebCommand = new Command(() => Launcher.OpenAsync(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}