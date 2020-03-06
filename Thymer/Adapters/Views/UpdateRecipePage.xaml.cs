using System.ComponentModel;
using System.Threading.Tasks;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Xamarin.Forms;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class UpdateRecipePage : ContentPage
    {
        private readonly UpdateRecipeViewModel _vm;
        
        public UpdateRecipePage()
        {
            BindingContext = _vm = TinyIoCContainer.Current.Resolve<UpdateRecipeViewModel>();
            
            InitializeComponent();
        }

        private async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (!(e.Item is Step step))
                return;
            
            await _vm.UpdateStep(step);

            StepListView.SelectedItem = null;
        }
    }
}