using System;
using System.Threading.Tasks;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    [QueryProperty("RecipeId", "recipeId")]
    public class UpdateRecipeViewModel : BaseRecipeViewModel
    {
        public string RecipeId
        {
            get => _recipeId;
            set
            {
                _recipeId = value;
                LoadRecipe().Wait();
            }
        }

        public override Recipe Recipe
        {
            get => _recipe; 
            set
            {
                _recipe = value;
                SetProperty(ref _recipe, value, nameof(Recipe));
            }
        }

        public UpdateRecipeViewModel(INavigationService navigationService, IAmADatabase database, IMessagingCenter messagingCenter) : base(navigationService, database, messagingCenter)
        {
            _recipe = new Recipe();
        }

        public override async Task SaveRecipe()
        {
            await _database.UpdateRecipe(_recipe);
        }

        private async Task LoadRecipe()
        {
            _recipe = await _database.GetRecipe(Guid.Parse(RecipeId));
        }

        private Recipe _recipe;
        private string _recipeId;
    }
}