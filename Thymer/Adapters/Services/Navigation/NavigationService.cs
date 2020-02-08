using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
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

        public async Task NavigateTo<TViewModel>(params (string name, string value)[] queryParams) where TViewModel : BaseViewModel
        {
            var route = $"{_routeBindings[typeof(TViewModel)]}";
            
            if (queryParams is null)
            {
                await Shell.Current.GoToAsync(route);
                return;
            }

            if (queryParams.Any())
            {
                var queryString = "?" + string.Join("&", queryParams.Select(q => $"{q.name}={q.value}"));

                route += queryString;
            }
            
            await Shell.Current.GoToAsync(route);
        }

        public async Task NavigateBackToRoot()
        {
            await Shell.Current.Navigation.PopToRootAsync();
        }

        public async Task NavigateBack()
        {
            await Shell.Current.Navigation.PopAsync();
        }
    }
}
