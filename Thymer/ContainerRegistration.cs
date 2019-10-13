using Thymer.Adapters.Services.Navigation;
using TinyIoC;

namespace Thymer
{
    public static class ContainerRegistration
    {
        public static void Register()
        {
            TinyIoCContainer.Current.Register<INavigationService, NavigationService>(new NavigationService(null, new ViewLocator()));
            TinyIoCContainer.Current.AutoRegister(type => type.Name.EndsWith("ViewModel"));
            TinyIoCContainer.Current.AutoRegister(type => type.Name.EndsWith("Page"));
            TinyIoCContainer.Current.AutoRegister(type => type.Name.EndsWith("View"));
        }
    }
}