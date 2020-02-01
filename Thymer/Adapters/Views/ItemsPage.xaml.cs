using System.ComponentModel;
using Thymer.Adapters.Services.Navigation;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;
using Thymer.Models;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        readonly ItemsViewModel viewModel;
        readonly INavigationService _navigationService;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = TinyIoCContainer.Current.Resolve<ItemsViewModel>();
            
            _navigationService = TinyIoCContainer.Current.Resolve<INavigationService>();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Shell.Current.GoToAsync("//addRecipe");

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }
    }
}