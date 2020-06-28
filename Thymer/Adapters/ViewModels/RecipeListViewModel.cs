using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Newtonsoft.Json;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Extensions;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
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
        private readonly IMessagingCenter _messagingCenter;
        private readonly StateService _stateService;

        public RecipeListViewModel(INavigationService navigationService, IAmADatabase database, IMessagingCenter messagingCenter, StateService stateService)
        {    
            _navigationService = navigationService;
            _database = database;
            _messagingCenter = messagingCenter;
            _stateService = stateService;
            
            Title = "My Recipes";
            Items = new ObservableCollection<Recipe>();
            
            Add = new Command(async () => await AddRecipe());
            Refresh = new Command(LoadRecipes);
            Update = new Command(async recipe => await UpdateRecipe(recipe as Recipe));

            BootstrapMessages();
            LoadRecipes();
        }

        public void ReceiveNewRecipe(string recipeMessage)
        {
            var recipe = JsonConvert.DeserializeObject<Recipe>(recipeMessage);
            
            Items.Add(recipe);
            Items.Sort(Recipe.Compare());
        }

        public void ReceiveUpdatedRecipe(string recipeMessage)
        {
            var recipe = JsonConvert.DeserializeObject<Recipe>(recipeMessage);

            var oldRecipe = Items.First(r => r.Id == recipe.Id);

            Items.Remove(oldRecipe);
            Items.Add(recipe);
            Items.Sort(Recipe.Compare());
        }

        private void BootstrapMessages()
        {
            _messagingCenter.Subscribe<NewRecipeViewModel, string>(
                this,
                Messages.AddRecipe,
                (sender, arg) => ReceiveNewRecipe(arg));

            _messagingCenter.Subscribe<UpdateRecipeViewModel, string>(
                this,
                Messages.UpdateRecipe,
                (sender, arg) => ReceiveUpdatedRecipe(arg));
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
                
                Items.Sort(Recipe.Compare());
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