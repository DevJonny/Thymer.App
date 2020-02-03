using System;
using FluentAssertions;
using Machine.Specifications;
using Thymer.Adapters.ViewModels;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("AddStepViewModel")]
    public class AddStepViewModelTests : BaseViewModelTests
    {
        static readonly string _name = "The Beef";
        static readonly string _duration = "01:02:03";

        class When_add_step_view_model_is_loaded
        {
            static AddStepViewModel vm;

            Because of = () => vm = new AddStepViewModel(_database);

            It should_have_set_defaults = () =>
            {
                vm.Duration.Should().Be("00:00:00");
                vm.Hours.Should().Be(0);
                vm.Minutes.Should().Be(0);
                vm.Seconds.Should().Be(0);
            };
        }

        class When_setting_the_recipe_title
        {
            static AddStepViewModel _vm;
            static string _recipeTitle;

            Establish context = () =>
            {
                _vm = new AddStepViewModel(_database);
                _recipeTitle = "Roast Beef";
            };
            
            Because of = () => _vm.RecipeTitle = Uri.EscapeDataString(_recipeTitle);
            
            It should_have_changed_title = () => _vm.Title.Should().BeEquivalentTo($"Add step to {_recipeTitle}");
        }

        class When_setting_the_hours
        {
            static AddStepViewModel _vm;
            
            Establish context = () =>
            {
                _vm = new AddStepViewModel(_database);
            };

            private Because of = () =>
            {
                _vm.Hours = 5;
                _vm.Minutes = 2;
                _vm.Seconds = 3;
            };
            
            It should_have_changed_the_duration = () => _vm.Duration.Should().Be("05:02:03");
        }
    }
}