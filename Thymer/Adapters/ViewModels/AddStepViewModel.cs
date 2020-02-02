using System;
using MvvmHelpers;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    [QueryProperty("RecipeId", "recipeId")]
    public class AddStepViewModel : BaseViewModel
    {
        public string RecipeId
        {
            get => _recipeId;
            set => SetProperty(ref _recipeId, Uri.UnescapeDataString(value), nameof(RecipeId));
        }

        private string _recipeId = string.Empty;
    }
}