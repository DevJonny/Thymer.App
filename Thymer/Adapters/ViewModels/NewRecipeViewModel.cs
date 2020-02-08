using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Newtonsoft.Json;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
using Thymer.Services.Database;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class NewRecipeViewModel : BaseViewModel
    {
        public Recipe Recipe { get; } = new Recipe();
        public ObservableCollection<Step> Steps => Recipe.Steps;
        public ICommand Save { get; }
        public ICommand AddStep { get; }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
                Title = _name;
                Recipe.Title = _name;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value, nameof(Description));
                Recipe.Description = _description;
            }
        }

        public Step SelectedStep
        {
            get => _selectedStep;
            set => SetProperty(ref _selectedStep, value, nameof(SelectedStep));
        }

        private readonly INavigationService _navigationService;
        private readonly IAmADatabase _database;
        private readonly IMessagingCenter _messagingCenter;
        
        public NewRecipeViewModel(INavigationService navigationService, IAmADatabase database, IMessagingCenter messagingCenter)    
        {
            _navigationService = navigationService;
            _database = database;
            _messagingCenter = messagingCenter;

            Title = "New Recipe";
            Save = new Command(async () => await SaveNewRecipe());
            AddStep = new Command(async () => await AddStepToRecipe());
            
            _messagingCenter.Subscribe<AddStepViewModel, string>(
                this, 
                Messages.AddRecipeStep, 
                (sender, arg) => ReceiveStep(arg));
        }
        
        public async Task SaveNewRecipe()
        {
            var recipe = new Recipe(Recipe.Id, Name, Description, Recipe.Steps);
            
            await _database.AddRecipe(recipe);
            await _navigationService.NavigateBackToRoot();
        }

        public async Task AddStepToRecipe()
        {
            await _navigationService.NavigateTo<AddStepViewModel>(("recipeTitle", $"{Uri.EscapeDataString(Recipe.Title)}"));
        }

        public void ReceiveStep(string stepMessage)
        {
            var step = JsonConvert.DeserializeObject<Step>(stepMessage);
            
            Recipe.AddStep(step);
        }

        private string _name = string.Empty;
        private string _description = string.Empty;
        private Step _selectedStep = null;
    }
}