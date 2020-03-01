using System;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Tests.TestDataBuilders;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("UpdateRecipeViewModel")]
    public class UpdateRecipeViewModelTests : BaseViewModelTests
    {
        static readonly string name = "Roast Beef";
        static readonly string description = "The best of the roasts";
        static readonly Step _step = new Step(Guid.NewGuid(), "Step name", 1, 2, 3); 
        static readonly string uriEscapedStepName = Uri.EscapeDataString(_step.Name);

        static string UriEscapedRecipeName = Uri.EscapeDataString(name);
        static Recipe _recipe = new RecipeTestDataBuilder().WithTitle(name).WithDescription(description).WithSteps(_step).Build();

        class When_the_recipe_is_loaded
        {
            static UpdateRecipeViewModel vm;

            private Establish context = () =>
            {
                _stateService.Recipe = _recipe;
                vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);
            };

            Because of = () => vm.LoadRecipe();
            
            It should_have_populated_the_view_model = () => vm.Recipe.Should().BeEquivalentTo(_recipe);
            It should_have_populated_the_name = () => vm.Title.Should().Be(name);
            It should_have_populated_the_description = () => vm.Description.Should().BeEquivalentTo(description);
        }

        class When_the_updated_recipe_is_saved
        {
            static UpdateRecipeViewModel vm;

            private Establish context = () =>
            {
                _database.StoredRecipes.Add(_recipe);
                _stateService.Recipe = _recipe;
                
                vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);
            };
            
            Because of = () =>
            {
                vm.Title = $"Updated {_recipe.Title}";
                vm.Description = $"Updated {_recipe.Description}";
                vm.SaveRecipe();
            };

            It should_have_updated_the_recipe = () =>
                _database.StoredRecipes.Should().ContainSingle(r => r.Id == _recipe.Id)
                    .Which.Should().BeEquivalentTo(
                        new Recipe(
                            _recipe.Id, 
                            $"Updated {name}", 
                            $"Updated {description}", 
                            _recipe.Steps));
        }
        
        class When_adding_a_new_step
        {
            static UpdateRecipeViewModel vm;

            Establish context = () =>
            {
                vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService)
                {
                    Recipe = _recipe
                };
            };

            Because of = () => vm.AddStepToRecipe();

            It should_navigate_to_add_step_with_recipe_id = () => 
                _navigationService.LastNavigatedTo.Should().Be($"recipe/step?recipeTitle={Uri.EscapeDataString(_recipe.Title)}");
        }

        class When_updating_a_step
        {
            static UpdateRecipeViewModel vm;

            Establish context = () => vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);
            
            Because of = () => vm.UpdateStep(_step);

            It should_navigate_to_update_step_with_step_params = () =>
                _navigationService.LastNavigatedTo.Should().Be($"recipe/step?id={_step.Id}&name={uriEscapedStepName}&hours={_step.Hours}&minutes={_step.Minutes}&seconds={_step.Seconds}");
        }
        
        class When_receiving_a_new_step_with_duration_longer_than_existing
        {
            static UpdateRecipeViewModel _vm;
            static Step _existingStep, _newStep;
            static string _stepMessage;

            Establish context = () => 
            {
                _existingStep = new Step("The step that came before", 1, 2, 3);
                _recipe = new RecipeTestDataBuilder().WithSteps(_existingStep).Build();
                
                _vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService)
                {
                    Recipe = _recipe
                };
                
                _newStep = new Step(Guid.NewGuid(), "First step into a larger world", 4, 5, 6);
                _stepMessage = JsonConvert.SerializeObject(_newStep);
            };
            
            Because of = () => _vm.ReceiveStep(_stepMessage);

            It should_add_step_to_list_of_step = () => _vm.Recipe.Steps.Should().BeEquivalentTo(_newStep, _existingStep);
        }

        class When_receiving_an_updated_step
        {
            static UpdateRecipeViewModel _vm;
            static Step _updatedStep;
            static string _stepMessage;
            static Step _originalStep;

            private Establish context = () =>
            {
                _originalStep = new Step("The original step", 1, 2, 3);
                _recipe.Steps.Add(_originalStep);
                
                _vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService)
                {
                    Recipe = _recipe
                };

                _updatedStep = new Step(_originalStep.Id,"The updated step", 4, 5, 6);
                _stepMessage = JsonConvert.SerializeObject(_updatedStep);
            };
            
            Because of = () => _vm.ReceiveUpdatedStep(_stepMessage);

            It should_update_the_existing_step = () =>
                _vm.Recipe.Steps.Should().BeEquivalentTo(_step, _updatedStep);
        }
    }
}