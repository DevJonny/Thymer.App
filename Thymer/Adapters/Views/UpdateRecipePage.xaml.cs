using System.ComponentModel;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class UpdateRecipePage : ContentPage
    {
        public UpdateRecipePage()
        {
            BindingContext = TinyIoCContainer.Current.Resolve<UpdateRecipeViewModel>();
            
            InitializeComponent();
        }
    }
}