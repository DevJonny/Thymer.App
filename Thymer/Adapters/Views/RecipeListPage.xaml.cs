using System.ComponentModel;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;
using Thymer.Models;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class RecipeListPage : ContentPage
    {
        public RecipeListPage()
        {
            BindingContext = TinyIoCContainer.Current.Resolve<RecipeListViewModel>();
            InitializeComponent();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Shell.Current.GoToAsync("//addRecipe");

            // Manually deselect item.
            RecipeListView.SelectedItem = null;
        }
    }
}