using Thymer.Adapters.ViewModels;
using TinyIoC;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thymer.Adapters.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddStepPage : ContentPage
    {
        public AddStepPage()
        {
            BindingContext = TinyIoCContainer.Current.Resolve<AddStepViewModel>();

            InitializeComponent();
        }
    }
}