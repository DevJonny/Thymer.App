using System.Threading.Tasks;
using Newtonsoft.Json;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
using Thymer.Ports.Services;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class UpdateRecipeViewModel : BaseRecipeViewModel
    {
        public override Recipe Recipe
        {
            get => _recipe; 
            set
            {
                _recipe = value;
                SetProperty(ref _recipe, value, nameof(Recipe));
            }
        }

        public UpdateRecipeViewModel(INavigationService navigationService, IAmADatabase database, IMessagingCenter messagingCenter, StateService stateService) 
            : base(navigationService, database, messagingCenter, stateService)
        {
            LoadRecipe();
        }

        public override async Task SaveRecipe()
        {
            Recipe.Title = Title;
            Recipe.Description = Description;
            
            await _database.UpdateRecipe(Recipe);
            
            _messagingCenter.Send(this, Messages.UpdateRecipe, JsonConvert.SerializeObject(Recipe));
            
            await _navigationService.NavigateBackToRoot();
        }

        public void LoadRecipe()
        {
            Recipe = _stateService.Recipe;
            Title = _stateService.Recipe.Title;
            Description = _stateService.Recipe.Description;
        }

        private Recipe _recipe;
    }
}