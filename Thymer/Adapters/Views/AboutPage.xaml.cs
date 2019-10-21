using System.ComponentModel;
using Thymer.Adapters.ViewModels;
using TinyIoC;
using Xamarin.Forms;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();

            BindingContext = TinyIoCContainer.Current.Resolve<AboutViewModel>();
        }
    }
}