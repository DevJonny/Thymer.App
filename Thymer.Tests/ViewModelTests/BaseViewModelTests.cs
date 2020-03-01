using System;
using System.Collections.Generic;
using Machine.Specifications;
using Thymer.Adapters.Services.Database;
using Thymer.Models;
using Thymer.Ports.Services;
using Thymer.Services;
using Thymer.Tests.TestDoubles;
using TinyIoC;

namespace Thymer.Tests.ViewModelTests
{
    public class BaseViewModelTests
    {
        protected static FakeNavigationService _navigationService;
        protected static FakeMessagingCenter _messagingCenter;
        protected static FakeDatabase _database;
        protected static StateService _stateService = new StateService();
        
        protected Establish context = () =>
        {
            ContainerRegistration.Register(false);

            var routeMappings = TinyIoCContainer.Current.Resolve<IDictionary<Type, string>>();

            _navigationService = new FakeNavigationService(routeMappings);
            _messagingCenter = new FakeMessagingCenter();
            _database = new FakeDatabase();

            TinyIoCContainer.Current.Register<IDataStore<Item>>(new MockDataStore());
            TinyIoCContainer.Current.Register<IAmADatabase>(_database);
        };
    }
}