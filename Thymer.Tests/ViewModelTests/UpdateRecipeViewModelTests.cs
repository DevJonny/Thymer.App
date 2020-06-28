using System;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
using Thymer.Tests.TestDataBuilders;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("UpdateRecipeViewModel")]
    public class UpdateRecipeViewModelTests : BaseViewModelTests
    {
        static readonly string name = "Roast Beef";
        static readonly string description = "The best of the roasts";
        static Step _step;
        static string _uriEscapedStepName;
        static Recipe _recipe;
        static string _uriEscapedRecipeName;
        static UpdateRecipeViewModel vm;

        Establish context = () =>
        {
            _step = new Step(Guid.NewGuid(), "Step name", 1, 2, 3);
            _uriEscapedStepName = Uri.EscapeDataString(_step.Name);
            
            _recipe = new RecipeTestDataBuilder().WithTitle(name).WithDescription(description).WithSteps(_step).Build();
            _uriEscapedRecipeName = Uri.EscapeDataString(name);

            _stateService.Recipe = _recipe;
            
            vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);
        };

        class When_the_recipe_is_loaded
        {
            Because of = () => vm.LoadRecipe();
            
            It should_have_populated_the_view_model = () => vm.Recipe.Should().BeEquivalentTo(_recipe);
            It should_have_populated_the_name = () => vm.Title.Should().Be(name);
            It should_have_populated_the_description = () => vm.Description.Should().BeEquivalentTo(description);
        }

        class When_the_updated_recipe_is_saved
        {
            static string _title;
            static string _description;
            
            static string _message;

            Establish context = () =>
            {
                _title = $"Updated {_recipe.Title}";
                _description = $"Updated {_recipe.Description}";
                
                _database.StoredRecipes.Add(_recipe);

                _message = JsonConvert.SerializeObject(new Recipe(_recipe.Id, _title, _description, _recipe.Steps));
            };
            
            Because of = () =>
            {
                vm.Title = _title;
                vm.Description = _description;
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
            
            It should_publish_update_recipe_message = () => _messagingCenter.WasSent(vm, Messages.UpdateRecipe, _message);
            It should_navigate_to_root = () => _navigationService.NavigatedToRoot.Should().BeTrue();
        }
        
        class When_adding_a_new_step
        {
            Because of = () => vm.AddStepToRecipe();

            It should_navigate_to_add_step_with_recipe_id = () => 
                _navigationService.LastNavigatedTo.Should().Be($"recipe/step?recipeTitle={Uri.EscapeDataString(_recipe.Title)}");
        }

        class When_updating_a_step
        {
            static string existingStep;

            Establish context = () =>
            {
                existingStep = Uri.EscapeDataString($"{_step.Id}|{_step.Name}|{_step.Hours}|{_step.Minutes}|{_step.Seconds}");
            };
            
            Because of = () => vm.UpdateStep(_step);

            It should_navigate_to_update_step_with_step_params = () =>
                _navigationService.LastNavigatedTo.Should().Be($"recipe/step?name={_uriEscapedRecipeName}&existingStep={existingStep}");
        }
        
        class When_receiving_a_new_step_with_duration_longer_than_existing
        {
            static Step _existingStep, _newStep;
            static string _stepMessage;

            Establish context = () => 
            {
                _newStep = new Step(Guid.NewGuid(), "First step into a larger world", 4, 5, 6);
                _stepMessage = JsonConvert.SerializeObject(_newStep);
            };
            
            Because of = () => vm.ReceiveStep(_stepMessage);

            It should_add_step_to_list_of_step = () => vm.Recipe.Steps.Should().BeEquivalentTo(_newStep, _step);
        }

        class When_receiving_an_updated_step
        {
            static Step _updatedStep;
            static string _stepMessage;
            static Step _originalStep;

            Establish context = () =>
            {
                _originalStep = new Step("The original step", 1, 2, 3);
                _recipe.Steps.Add(_originalStep);

                _updatedStep = new Step(_originalStep.Id,"The updated step", 4, 5, 6);
                _stepMessage = JsonConvert.SerializeObject(_updatedStep);
            };
            
            Because of = () => vm.ReceiveUpdatedStep(_stepMessage);

            It should_update_the_existing_step = () => vm.Recipe.Steps.Should().BeEquivalentTo(_step, _updatedStep);
        }
    }
}