using System.Threading.Tasks;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Ports.Services;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class NewRecipeViewModel : BaseRecipeViewModel
    {
        public NewRecipeViewModel( INavigationService navigationService, IAmADatabase database, IMessagingCenter messagingCenter, StateService stateService) 
            : base(navigationService, database, messagingCenter, stateService)
        {
        }

        public override Recipe Recipe { get; set; } = new Recipe();

        public override async Task SaveRecipe()
        {
            var recipe = new Recipe(Recipe.Id, Name, Description, Recipe.Steps);
            
            await _database.AddRecipe(recipe);
            await _navigationService.NavigateBackToRoot();
        }
    }
}