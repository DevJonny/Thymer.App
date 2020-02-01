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
        private readonly string _previousPage;
        
        public FakeNavigationService(IDictionary<Type, string> routeMapping, string previousPage = null)
        {
            _routeMapping = routeMapping;
            _previousPage = previousPage;
        }
        
        public async Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            LastNavigatedTo = _routeMapping[typeof(TViewModel)];

            await Task.Run(() => {});
        }

        public async Task NavigateBackToRoot()
        {
            LastNavigatedTo = "//root";

            await Task.Run(() => { });
        }

        public async Task NavigateBack()
        {
            LastNavigatedTo = _previousPage;

            await Task.Run(() => { });
        }
    }
}