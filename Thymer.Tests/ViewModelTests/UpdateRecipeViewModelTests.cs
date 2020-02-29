using System;
using System.Collections.ObjectModel;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("UpdateRecipeViewModel")]
    public class UpdateRecipeViewModelTests : BaseViewModelTests
    {
        static readonly string name = "Roast Beef";
        static readonly string description = "The best of the roasts";
        static string UriEscapedRecipeName = Uri.EscapeDataString(name);
        static readonly Step _step = new Step(Guid.NewGuid(), "Step name", 1, 2, 3); 
        static readonly string uriEscapedStepName = Uri.EscapeDataString(_step.Name);
        static readonly Recipe _recipe = new Recipe(Guid.NewGuid(), name, description, new ObservableCollection<Step>
        {
            _step
        });

        class When_the_view_model_is_loaded
        {
            static UpdateRecipeViewModel vm;
            
            Establish context = () =>
            {
                _database.StoredRecipes.Add(_recipe);
                vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter);
            };

            Because of = () => vm.RecipeId = _recipe.Id.ToString();
            
            It should_have_populated_the_view_model = () => vm.Recipe.Should().BeEquivalentTo(_recipe);
        }

        class When_the_updated_recipe_is_saved
        {
            static UpdateRecipeViewModel vm;

            Establish context = () =>
            {
                _database.StoredRecipes.Add(_recipe);
                _database.StoredRecipes.Add(new Recipe("Another recipe", string.Empty));
                
                vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter)
                {
                    RecipeId = _recipe.Id.ToString()
                };

                vm.Recipe.Title = $"Updated {_recipe.Title}";
                vm.Recipe.Description = $"Updated {_recipe.Description}";
            };
            
            Because of = () => vm.SaveRecipe();

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
            static Recipe recipe;

            private Establish context = () =>
            {
                recipe = new Recipe(name, description);
                
                _database.StoredRecipes.Clear();
                _database.StoredRecipes.Add(recipe);

                vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter);
                vm.RecipeId = $"{recipe.Id}";
            };

            Because of = () => vm.AddStepToRecipe();

            It should_navigate_to_add_step_with_recipe_id = () => 
                _navigationService.LastNavigatedTo.Should().Be($"recipe/step?recipeTitle={UriEscapedRecipeName}");
        }

        class When_updating_a_step
        {
            static UpdateRecipeViewModel vm;

            Establish context = () => vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter);
            
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
                
                _vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter);
                _vm.Recipe.Steps.Add(_existingStep);
                
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
                _database.StoredRecipes.Clear();
                _database.StoredRecipes.Add(_recipe);
                _vm = new UpdateRecipeViewModel(_navigationService, _database, _messagingCenter)
                {
                    RecipeId = $"{_recipe.Id}"
                };
                _originalStep = new Step("The original step", 1, 2, 3);
                _vm.Recipe.Steps.Add(_originalStep);

                _updatedStep = new Step(_originalStep.Id,"The updated step", 4, 5, 6);
                _stepMessage = JsonConvert.SerializeObject(_updatedStep);
            };
            
            Because of = () => _vm.ReceiveUpdatedStep(_stepMessage);

            It should_update_the_existing_step = () =>
                _vm.Recipe.Steps.Should().BeEquivalentTo(_step, _updatedStep);
        }
    }
}