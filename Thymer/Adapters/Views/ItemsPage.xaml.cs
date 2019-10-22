﻿using System;
using System.ComponentModel;
using Thymer.Adapters.Services.Navigation;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;
using Thymer.Models;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemsPage : ContentPage
    {
        readonly ItemsViewModel viewModel;
        readonly INavigationService _navigationService;

        public ItemsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = TinyIoCContainer.Current.Resolve<ItemsViewModel>();
            
            _navigationService = TinyIoCContainer.Current.Resolve<INavigationService>();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Item;
            if (item == null)
                return;

            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(_navigationService, item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await _navigationService.NavigateTo<NewItemViewModel>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}