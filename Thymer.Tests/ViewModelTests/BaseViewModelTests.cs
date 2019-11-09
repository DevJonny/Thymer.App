using Machine.Specifications;
using Thymer.Adapters.Services.Navigation;
using Thymer.Models;
using Thymer.Services;
using Thymer.Services.Database;
using Thymer.Tests.TestDoubles;
using TinyIoC;

namespace Thymer.Tests.ViewModelTests
{
    public class BaseViewModelTests
    {
        protected static INavigationService _navigationService;
        protected static FakeMessagingCenter _messagingCenter;
        protected static FakeDatabase _database;
        
        protected Establish context = () =>
        {
            _navigationService = new NavigationService(null, new ViewLocator());
            _messagingCenter = new FakeMessagingCenter();
            _database = new FakeDatabase();

            TinyIoCContainer.Current.Register<IDataStore<Item>>(new MockDataStore());
            TinyIoCContainer.Current.Register<IAmADatabase>(_database);
        };
    }
}