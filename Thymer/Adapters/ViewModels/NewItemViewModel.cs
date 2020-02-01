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

        private readonly INavigationService _navigationService;
        private readonly IAmADatabase _database;
        
        public NewItemViewModel(INavigationService navigationService, IAmADatabase database)    
        {
            _navigationService = navigationService;
            _database = database;
            
            Save = new Command(async () => await SaveNewRecipe());
        }

        public async Task SaveNewRecipe()
        {
            var recipe = new Recipe(Recipe.Id, Name, Description, Recipe.Steps);
            
            await _database.AddRecipe(recipe);
            await _navigationService.NavigateBackToRoot();
        }

        private string _name = string.Empty;
        private string _description = string.Empty;
    }
}