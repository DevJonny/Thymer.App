using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Services.Database;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class NewItemViewModel : ViewModelBase
    {
        public Recipe Recipe { get; } = new Recipe();
        public ICommand Save { get; }
        public ICommand Cancel { get; }

        public string Name
        {
            get => _name;
            set
            {
                SetPropertyAndRaise(ref _name, value, nameof(Name));
                Recipe.Title = _name;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                SetPropertyAndRaise(ref _description, value, nameof(Description));
                Recipe.Description = _description;
            }
        }

        private readonly INavigationService _navigationService;
        private readonly IAmADatabase _database;
        
        public NewItemViewModel(INavigationService navigationService, IAmADatabase database) : base(navigationService)
        {
            _navigationService = navigationService;
            _database = database;
            
            Save = new Command(async () => await SaveNewRecipe());
            Cancel = new Command(async () => await CancelNewRecipe());
        }

        public async Task SaveNewRecipe()
        {
            var recipe = new Recipe(Recipe.Id, Name, Description, Recipe.Steps);
            
            await _database.AddRecipe(recipe);
            await _navigationService.NavigateBackToRoot();
        }

        public async Task CancelNewRecipe()
        {
            await _navigationService.NavigateBackToRoot();
        }

        private string _name = string.Empty;
        private string _description = string.Empty;
    }
}