﻿using System.Threading.Tasks;
using Thymer.Adapters.ViewModels;

namespace Thymer.Adapters.Services.Navigation
{
    public interface INavigationService
    {
        Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateBackToRoot();

        Task NavigateBack();
    }
}
