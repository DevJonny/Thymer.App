using System;
using System.ComponentModel;
using Thymer.Adapters.ViewModels;
using Thymer.Ports.Messaging;
using Xamarin.Forms;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        private NewItemViewModel viewModel;
        
        public NewItemPage()
        {
            BindingContext = viewModel = TinyIoCContainer.Current.Resolve<NewItemViewModel>();
            
            InitializeComponent();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            await viewModel.SaveNewRecipe();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await viewModel.Cancel();
        }
    }
}