using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;

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

            Because of = () => vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter);

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
            
            Establish context = () =>
            {
                vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter);

                id = vm.Recipe.Id;
                vm.Name = name;
                vm.Description = description;
            };
            
            Because of = () => vm.SaveNewRecipe();

            It should_have_added_one_item = () =>
                _database.StoredRecipes
                    .Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new Recipe(id, name, description, new ObservableCollection<Step>()));

            It should_navigate_back_to_home = () => _navigationService.LastNavigatedTo.Should().Be("//root");
        }

        class When_setting_properties
        {
            static NewRecipeViewModel vm;
            
            Establish context = () =>
            {
                vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter);
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

            Establish context = () => vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter) {Name = name};

            Because of = () => vm.AddStepToRecipe();

            It should_navigate_to_add_step_with_recipe_id = () => _navigationService.LastNavigatedTo.Should().Be($"addRecipe/addStep?recipeTitle={uriEscapedName}");
        }

        class When_receiving_a_new_step_with_duration_longer_than_existing
        {
            static NewRecipeViewModel _vm;
            static Step _existingStep, _newStep;
            static string _stepMessage;

            private Establish context = () => 
            {
                _existingStep = new Step("The step that came before", 1, 2, 3);
                
                _vm = new NewRecipeViewModel(_navigationService, _database, _messagingCenter);
                _vm.Recipe.Steps.Add(_existingStep);
                
                _newStep = new Step(Guid.NewGuid(), "First step into a larger world", 4, 5, 6);
                _stepMessage = JsonConvert.SerializeObject(_newStep);
            };
            
            Because of = () => _vm.ReceiveStep(_stepMessage);

            It should_add_step_to_list_of_step = () => _vm.Recipe.Steps.Should().BeEquivalentTo(_newStep, _existingStep);
        }
    }
}