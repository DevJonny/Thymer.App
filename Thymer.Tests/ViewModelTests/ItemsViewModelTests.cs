using FluentAssertions;
using Machine.Specifications;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
using Thymer.Tests.TestDataBuilders;
using Thymer.Tests.TestDoubles;
using Xamarin.Forms;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("ItemsViewModel")]
    class ItemsViewModelTests : BaseViewModelTests
    {
        class When_items_view_model_is_created
        {
            static ItemsViewModel vm;

            Establish context = () =>
            {
                var seed = new []
                {
                    new RecipeTestDataBuilder().WithSteps(new StepTestDataBuilder().Build(), new StepTestDataBuilder().Build()).Build(),
                    new RecipeTestDataBuilder().WithSteps(new StepTestDataBuilder().Build()).Build()
                };

                _database.Seed(seed);
            };

            Because of = () => vm = new ItemsViewModel(_navigationService, _database);

            It should_have_loaded_saved_recipes = () => vm.Items.Count.Should().Be(2);
        }
    }
}