using System;
using System.Windows.Input;
using Thymer.Adapters.Services.Navigation;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        public AboutViewModel(INavigationService navigationService) : base(navigationService)
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}