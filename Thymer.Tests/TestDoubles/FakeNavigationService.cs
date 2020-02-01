using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thymer.Adapters.Services.Navigation;
using Thymer.Adapters.ViewModels;

namespace Thymer.Tests.TestDoubles
{
    public class FakeNavigationService : INavigationService
    {
        public string LastNavigatedTo { get; private set; }

        private readonly IDictionary<Type, string> _routeMapping;
        
        public FakeNavigationService(IDictionary<Type, string> routeMapping)
        {
            _routeMapping = routeMapping;
        }
        
        public async Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            LastNavigatedTo = _routeMapping[typeof(TViewModel)];

            await Task.Run(() => {});
        }

        public async Task NavigateBackToRoot()
        {
            LastNavigatedTo = _routeMapping[typeof(ItemsViewModel)];

            await Task.Run(() => { });
        }
    }
}