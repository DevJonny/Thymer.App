using System;
using System.ComponentModel;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;
using Thymer.Models;
using Thymer.Ports.Messaging;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewItemPage : ContentPage
    {
        public NewItemViewModel ViewModel;

        public NewItemPage()
        {
            InitializeComponent();

            BindingContext = ViewModel = TinyIoCContainer.Current.Resolve<NewItemViewModel>();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, Messages.AddRecipe, ViewModel.Recipe);
            await Navigation.PopModalAsync();
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}