using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Services.Database;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        public ObservableCollection<Recipe> Items { get; }
        public ICommand Add { get; }
        public ICommand Refresh { get; }

        private readonly INavigationService _navigationService;
        private readonly IAmADatabase _database;

        public ItemsViewModel(INavigationService navigationService, IAmADatabase database)
        {    
            _navigationService = navigationService;
            _database = database;
            
            Title = "Browse";
            Items = new ObservableCollection<Recipe>();
            
            Add = new Command(async () => await AddRecipe());
            Refresh = new Command(LoadRecipes);

            LoadRecipes();
        }
        
        private void LoadRecipes()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = _database.GetAllRecipes();
                
                foreach (var item in items)
                    Items.Add(item);
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

        private async Task AddRecipe()
        {
            await _navigationService.NavigateTo<NewItemViewModel>();
        }
    }
}