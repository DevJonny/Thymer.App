using Machine.Specifications;
using Thymer.Adapters.Services.Navigation;
using Thymer.Models;
using Thymer.Services;
using Thymer.Tests.TestDoubles;
using TinyIoC;

namespace Thymer.Tests.ViewModelTests
{
    public class BaseViewModelTests
    {
        protected static INavigationService _navigationService;
        protected static FakeMessagingCenter _messagingCenter;
        
        protected Establish context = () =>
        {
            _navigationService = new NavigationService(null, new ViewLocator());
            _messagingCenter = new FakeMessagingCenter();

            TinyIoCContainer.Current.Register<IDataStore<Item>>(new MockDataStore());
        };
    }
}