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
        public ICommand Save { get; }
        public ICommand Cancel { get; }
        
        public string RecipeName
        {
            get => _recipeName;
            set => SetPropertyAndRaise(ref _recipeName, value, nameof(RecipeName));
        }

        public string Description
        {
            get => _description;
            set => SetPropertyAndRaise(ref _description, value, nameof(Description));
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
            var recipe = new Recipe(RecipeName, Description);
            
            _database.AddRecipe(recipe);
            await _navigationService.NavigateBack();
        }

        public async Task CancelNewRecipe()
        {
            await _navigationService.NavigateBack();
        }

        private string _recipeName = string.Empty;
        private string _description = string.Empty;
    }
}