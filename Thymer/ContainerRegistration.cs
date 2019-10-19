using Thymer.Adapters.Services.Navigation;
using Thymer.Models;
using Thymer.Services;
using TinyIoC;

namespace Thymer
{
    public static class ContainerRegistration
    {
        public static void Register()
        {
            TinyIoCContainer.Current.Register<INavigationService, NavigationService>(new NavigationService(null, new ViewLocator()));
            TinyIoCContainer.Current.Register<IDataStore<Item>>(new MockDataStore()).AsSingleton();
            
            TinyIoCContainer.Current.AutoRegister(type => type.Name.EndsWith("ViewModel"));
            TinyIoCContainer.Current.AutoRegister(type => type.Name.EndsWith("Page"));
            TinyIoCContainer.Current.AutoRegister(type => type.Name.EndsWith("View"));
        }
    }
}