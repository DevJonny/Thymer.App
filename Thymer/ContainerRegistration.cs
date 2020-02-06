using System;
using System.Collections.Generic;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.Services.Navigation;
using Thymer.Adapters.ViewModels;
using Thymer.Adapters.Views;
using Thymer.Services.Database;
using TinyIoC;
using Xamarin.Forms;

namespace Thymer
{
    public static class ContainerRegistration
    {
        public static void Register()
        {
            _viewModelRoutes.Clear();
            
            RegisterRoute<RecipeListViewModel, RecipeListPage>("recipes");
            RegisterRoute<NewRecipeViewModel, NewRecipePage>("addRecipe");
            RegisterRoute<AddStepViewModel, AddStepPage>("addStep");
            RegisterRoute<ItemDetailViewModel, ItemDetailPage>("recipe");

            TinyIoCContainer.Current.Register(_viewModelRoutes);
            TinyIoCContainer.Current.Register<INavigationService, NavigationService>();
            TinyIoCContainer.Current.Register<IAmADatabase>(new Database());
            TinyIoCContainer.Current.Register(MessagingCenter.Instance);
            
            TinyIoCContainer.Current.AutoRegister(type => type.Name.EndsWith("ViewModel"));
        }

        private static readonly IDictionary<Type, string> _viewModelRoutes = new Dictionary<Type, string>();
        
        private static void RegisterRoute<TViewModel, TPage>(string route)
        {
            _viewModelRoutes.Add(typeof(TViewModel), route);
            Routing.RegisterRoute(route, typeof(TPage));
        }
    }
}