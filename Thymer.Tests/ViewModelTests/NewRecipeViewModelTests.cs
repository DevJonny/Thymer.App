using System;
using System.Collections.ObjectModel;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;

namespace Thymer.Tests.ViewModelTests
{ 
    [Subject("NewRecipeViewModel")]
    class NewRecipeViewModelTests : BaseViewModelTests
    {
        static readonly string name = "Roast Beef";
        static readonly string uriEscapedName = Uri.EscapeDataString(name);
        static readonly string description = "The best of the roasts";
        
        class When_new_item_view_model_is_loaded
        {
            static NewRecipeViewModel vm;

            Because of = () => vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);

            It should_have_set_timer_fields_to_defaults = () =>
            {
                vm.Title.Should().Be("New Recipe");
                vm.Recipe.Title.Should().BeEmpty();
                vm.Recipe.Description.Should().BeEmpty();
                vm.Recipe.Steps.Count.Should().Be(0);
            };
        }

        class When_saving_new_recipe
        {
            static NewRecipeViewModel vm;
            static Guid id;
            static Step _stepOne;
            static Step _longerStep;
            static string _message;

            Establish context = () =>
            {
                vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);

                id = vm.Recipe.Id;
                vm.Name = name;
                vm.Description = description;

                _stepOne = new Step("First step into a larger world", 1, 2, 3);
                _longerStep = new Step("A longer task than the first", 4, 5, 6);
                
                vm.Recipe.Steps.Add(_stepOne);
                vm.Recipe.Steps.Add(_longerStep);

                _message = JsonConvert.SerializeObject(vm.Recipe);
            };
            
            Because of = () => vm.SaveRecipe();

            It should_have_added_one_item = () =>
                _database.StoredRecipes
                    .Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new Recipe(id, name, description, new ObservableCollection<Step> {_longerStep, _stepOne}));

            It should_publish_new_recipe_message = () => _messagingCenter.WasSent(vm, Messages.AddRecipe, _message);
            It should_navigate_back_to_home = () => _navigationService.LastNavigatedTo.Should().Be("//root");
        }

        class When_setting_properties
        {
            static NewRecipeViewModel vm;
            
            Establish context = () =>
            {
                vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);
            };

            Because of = () =>
            {
                vm.Name = name;
                vm.Description = description;
            };

            It should_have_set_title = () => vm.Title.Should().Be(name);
            It should_have_set_recipe_title_to_name = () => vm.Recipe.Title.Should().Be(name);
            It should_have_set_recipe_description_to_description = () => vm.Recipe.Description.Should().Be(description);
        }

        class When_adding_a_new_step
        {
            static NewRecipeViewModel vm;

            Establish context = () =>
            {
                vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService) {Name = name};
            };

            Because of = () => vm.AddStepToRecipe();
            
            It should_navigate_to_add_step_with_recipe_id = () => _navigationService.LastNavigatedTo.Should().Be($"recipe/step?recipeTitle={uriEscapedName}");
        }

        class When_receiving_a_new_step_with_duration_longer_than_existing
        {
            static NewRecipeViewModel _vm;
            static Step _existingStep, _newStep;
            static string _stepMessage;

            Establish context = () => 
            {
                _existingStep = new Step("The step that came before", 1, 2, 3);
                
                _vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter, _stateService);
                _vm.Recipe.Steps.Add(_existingStep);
                
                _newStep = new Step(Guid.NewGuid(), "First step into a larger world", 4, 5, 6);
                _stepMessage = JsonConvert.SerializeObject(_newStep);
            };
            
            Because of = () => _vm.ReceiveStep(_stepMessage);

            It should_add_step_to_list_of_step = () => _vm.Recipe.Steps.Should().BeEquivalentTo(_newStep, _existingStep);
        }
    }
}