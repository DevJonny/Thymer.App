using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Thymer.Adapters.Services.Navigation;
using Thymer.Adapters.Views;
using Thymer.Core.Models;
using Thymer.Models;
using Thymer.Ports.Messaging;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class ItemsViewModel : ViewModelBase
    {
        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }

        private readonly INavigationService _navigationService;
        private readonly IMessagingCenter _messagingCenter;

        public ItemsViewModel(INavigationService navigationService, IMessagingCenter messagingCenter) : base(navigationService)
        {
            _navigationService = navigationService;
            _messagingCenter = messagingCenter;
            
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            messagingCenter.Subscribe<NewItemPage, Recipe>(this, Messages.AddRecipe, async (obj, recipe) =>
            {
                Recipe newItem = recipe;
//                Items.Add(newItem);
//                await DataStore.AddItemAsync(newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}