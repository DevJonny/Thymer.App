using FluentAssertions;
using Machine.Specifications;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;

namespace Thymer.Tests.ViewModelTests
{ 
    class NewItemViewModelTests : BaseViewModelTests
    {
        static readonly string name = "Roast Beef"; 
        static readonly string description = "The best of the roasts";
        
        class When_new_item_view_model_is_loaded
        {
            static NewItemViewModel vm;

            Because of = () => vm = new NewItemViewModel(_navigationService, _messagingCenter);

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

            Establish context = () =>
            {
                vm = new NewItemViewModel(_navigationService, _messagingCenter)
                {
                    Recipe = new Recipe(name, description)    
                };
            };
            
            Because of = () => vm.SaveNewRecipe.Execute(null);

            It should_have_sent_add_item = () =>
                _messagingCenter.SentMessages
                    .Should().ContainSingle()
                    .Which.Should().BeEquivalentTo((vm, Messages.AddRecipe, new Recipe(name, description)));
        }

        class When_setting_properties
        {
            static NewItemViewModel vm;
            
            Establish context = () =>
            {
                vm = new NewItemViewModel(_navigationService, _messagingCenter);
            };

            private Because of = () =>
            {
                vm.Name = name;
                vm.Description = description;
            };

            It should_have_set_recipe_title_to_name = () => vm.Recipe.Title.Should().Be(name);
            It should_have_set_recipe_description_to_description = () => vm.Recipe.Description.Should().Be(description);
        }
    }
}