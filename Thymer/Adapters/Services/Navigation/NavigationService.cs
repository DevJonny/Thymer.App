using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Thymer.Adapters.ViewModels;
using Xamarin.Forms;

namespace Thymer.Adapters.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IDictionary<Type, string> _routeBindings;

        public NavigationService(IDictionary<Type, string> routeBindings)
        {
            _routeBindings = routeBindings;
        }

        public async Task NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var route = _routeBindings[typeof(TViewModel)];
            
            await Shell.Current.GoToAsync(route);
        }

        public async Task NavigateBackToRoot()
        {
            var route = _routeBindings[typeof(ItemsViewModel)];
            await Shell.Current.GoToAsync(route);
        }        
    }
}
