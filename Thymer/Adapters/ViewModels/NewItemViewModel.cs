using System;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Models;
using Thymer.Services.Database;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        public Recipe Recipe { get; } = new Recipe();
        public ICommand Save { get; }
        public ICommand AddStep { get; }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
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
        
        public NewItemViewModel(INavigationService navigationService, IAmADatabase database)    
        {
            _navigationService = navigationService;
            _database = database;
            
            Save = new Command(async () => await SaveNewRecipe());
            AddStep = new Command(async () => await AddStepToRecipe());
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

        private string _name = string.Empty;
        private string _description = string.Empty;
        private Step _selectedStep = null;
    }
}