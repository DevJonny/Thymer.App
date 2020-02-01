using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmHelpers;
using Thymer.Adapters.Services.Navigation;

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
        
        public async Task NavigateTo<TViewModel>() where TViewModel : BaseViewModel
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