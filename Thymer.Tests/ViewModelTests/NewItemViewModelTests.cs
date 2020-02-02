using System;
using System.Collections.Generic;
using FluentAssertions;
using Machine.Specifications;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Models;

namespace Thymer.Tests.ViewModelTests
{ 
    [Subject("NewItemViewModel")]
    class NewItemViewModelTests : BaseViewModelTests
    {
        static readonly string name = "Roast Beef"; 
        static readonly string description = "The best of the roasts";
        
        class When_new_item_view_model_is_loaded
        {
            static NewItemViewModel vm;

            Because of = () => vm = new NewItemViewModel(_navigationService, _database);

            It should_have_set_timer_fields_to_defaults = () =>
            {
                vm.Recipe.Title.Should().BeEmpty();
                vm.Recipe.Description.Should().BeEmpty();
                vm.Recipe.Steps.Count.Should().Be(0);
            };
        }

        class When_saving_new_recipe
        {
            static NewItemViewModel vm;
            static Guid id;
            
            Establish context = () =>
            {
                vm = new NewItemViewModel(_navigationService, _database);

                id = vm.Recipe.Id;
                vm.Name = name;
                vm.Description = description;
            };
            
            Because of = () => vm.SaveNewRecipe();

            It should_have_added_one_item = () =>
                _database.StoredRecipes
                    .Should().ContainSingle()
                    .Which.Should().BeEquivalentTo(new Recipe(id, name, description, new List<Step>()));

            It should_navigate_back_to_home = () => _navigationService.LastNavigatedTo.Should().Be("//root");
        }

        class When_setting_properties
        {
            static NewItemViewModel vm;
            
            Establish context = () =>
            {
                vm = new NewItemViewModel(_navigationService, _database);
            };

            Because of = () =>
            {
                vm.Name = name;
                vm.Description = description;
            };

            It should_have_set_recipe_title_to_name = () => vm.Recipe.Title.Should().Be(name);
            It should_have_set_recipe_description_to_description = () => vm.Recipe.Description.Should().Be(description);
        }

        class When_adding_a_new_step
        {
            static NewItemViewModel vm;
            
            Establish context = () =>
            {
                vm = new NewItemViewModel(_navigationService, _database);  
            };
            
            Because of = () => vm.AddStepToRecipe();

            It should_navigate_to_add_step_with_recipe_id = () => _navigationService.LastNavigatedTo.Should().Be($"addStep?recipeId={vm.Recipe.Id}");
        }
    }
}