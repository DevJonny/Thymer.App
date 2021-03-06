﻿using System;
using System.ComponentModel;
using Thymer.Adapters.Services.Navigation;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Thymer.Models;
using TinyIoC;

namespace Thymer.Adapters.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel viewModel;

        public ItemDetailPage(ItemDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public ItemDetailPage()
        {
            InitializeComponent();

            var item = new Item
            {
                Text = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new ItemDetailViewModel(TinyIoCContainer.Current.Resolve<INavigationService>(), item);
            BindingContext = viewModel;
        }
    }
}