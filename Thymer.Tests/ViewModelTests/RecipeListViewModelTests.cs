using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
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

            Because of = () => vm = new RecipeListViewModel(_navigationService, _database, _messagingCenter, new StateService());

            It should_have_loaded_saved_recipes = () => vm.Items.Count.Should().Be(2);
        }

        class When_updating_a_recipe
        {
            static RecipeListViewModel _vm;
            static Recipe _recipe;
            static StateService _stateService;

            Establish context = () =>
            {
                _recipe = new RecipeTestDataBuilder().WithSteps(new StepTestDataBuilder().Build()).Build();

                _database.Seed(new[] { _recipe });
                
                _stateService = new StateService();
                _vm = new RecipeListViewModel(_navigationService, _database, _messagingCenter, _stateService);
            };
            
            Because of = () => _vm.Update.Execute(_recipe); 
            
            It should_have_updated_app_state_with_selected_recipe = () => _stateService.Recipe.Should().BeEquivalentTo(_recipe);
            It should_have_navigated_to_update_recipe_view = () => _navigationService.LastNavigatedTo.Should().Be($"updateRecipe");
        }

        class When_receiving_a_new_recipe
        {
            static RecipeListViewModel _vm;
            static Recipe _originalRecipe, _newRecipe;
            static string _recipeMessage;

            Establish context = () =>
            {
                _originalRecipe = new RecipeTestDataBuilder().WithTitle("The First Recipe").Build();
                _newRecipe = new RecipeTestDataBuilder().WithTitle("Another Recipe").Build();
                
                _database.Seed(new [] { _originalRecipe });
                
                _vm = new RecipeListViewModel(_navigationService, _database, _messagingCenter, new StateService());

                _recipeMessage = JsonConvert.SerializeObject(_newRecipe);
            };
            
            Because of = () => _vm.ReceiveNewRecipe(_recipeMessage);

            It should_add_the_recipe = () => _vm.Items.Should().BeEquivalentTo(_newRecipe, _originalRecipe);
        }
        
        
        class When_receiving_a_updated_recipe
        {
            static RecipeListViewModel _vm;
            static Recipe _originalRecipe, _updatedRecipe;
            static string _recipeMessage;

            Establish context = () =>
            {
                _originalRecipe = new RecipeTestDataBuilder().Build();
                _updatedRecipe = new Recipe(_originalRecipe.Id, $"Updated {_originalRecipe.Title}", $"Updated {_originalRecipe.Description}", _originalRecipe.Steps);

                _database.Seed(new[] {_originalRecipe});

                _vm = new RecipeListViewModel(_navigationService, _database, _messagingCenter, new StateService());

                _recipeMessage = JsonConvert.SerializeObject(_updatedRecipe);
            };

            Because of = () => _vm.ReceiveUpdatedRecipe(_recipeMessage);

            It should_update_the_existing_recipe = () => _vm.Items.Should().BeEquivalentTo(_updatedRecipe);
        }
    }
}