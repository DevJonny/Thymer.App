using FluentAssertions;
using Machine.Specifications;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Ports.Services;
using Thymer.Tests.TestDataBuilders;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("ItemsViewModel")]
    class ItemsViewModelTests : BaseViewModelTests
    {
        class When_items_view_model_is_created
        {
            static RecipeListViewModel vm;

            Establish context = () =>
            {
                var seed = new []
                {
                    new RecipeTestDataBuilder().WithSteps(new StepTestDataBuilder().Build(), new StepTestDataBuilder().Build()).Build(),
                    new RecipeTestDataBuilder().WithSteps(new StepTestDataBuilder().Build()).Build()
                };

                _database.Seed(seed);
            };

            Because of = () => vm = new RecipeListViewModel(_navigationService, _database, new StateService());

            It should_have_loaded_saved_recipes = () => vm.Items.Count.Should().Be(2);
        }

        class When_updating_a_recipe
        {
            static RecipeListViewModel _vm;
            static Recipe _recipe;
            static StateService _stateService;

            private Establish context = () =>
            {
                _recipe = new RecipeTestDataBuilder().WithSteps(new StepTestDataBuilder().Build()).Build();

                _database.Seed(new[] { _recipe });
                
                _stateService = new StateService();
                _vm = new RecipeListViewModel(_navigationService, _database, _stateService);
            };
            
            Because of = () => _vm.Update.Execute(_recipe); 
            
            It should_have_updated_app_state_with_selected_recipe = () => _stateService.Recipe.Should().BeEquivalentTo(_recipe);
            It should_have_navigated_to_update_recipe_view = () => _navigationService.LastNavigatedTo.Should().Be($"updateRecipe");
        }
    }
}