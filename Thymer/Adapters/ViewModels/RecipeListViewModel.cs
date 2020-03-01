using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Ports.Services;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class RecipeListViewModel : BaseViewModel
    {
        public ICommand Add { get; }
        public ICommand Update { get; }
        public ICommand Refresh { get; }
        public ObservableCollection<Recipe> Items { get; }

        public Recipe SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                _selectedRecipe = value;
                _navigationService.NavigateTo<UpdateRecipeViewModel>(("recipeId", $"{_selectedRecipe.Id}"));
            }
        }

        private readonly INavigationService _navigationService;
        private readonly IAmADatabase _database;
        private readonly StateService _stateService;

        public RecipeListViewModel(INavigationService navigationService, IAmADatabase database, StateService stateService)
        {    
            _navigationService = navigationService;
            _database = database;
            _stateService = stateService;
            
            Title = "My Recipes";
            Items = new ObservableCollection<Recipe>();
            
            Add = new Command(async () => await AddRecipe());
            Refresh = new Command(LoadRecipes);
            Update = new Command(async (recipe) => await UpdateRecipe(recipe as Recipe));

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
            await _navigationService.NavigateTo<NewRecipeViewModel>();
        }
        
        private async Task UpdateRecipe(Recipe recipe)
        {
            _stateService.Recipe = recipe;
            await _navigationService.NavigateTo<UpdateRecipeViewModel>();
        }

        private Recipe _selectedRecipe;
    }
}