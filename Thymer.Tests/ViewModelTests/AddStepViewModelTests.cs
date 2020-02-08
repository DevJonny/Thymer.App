using System;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.ViewModels;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("AddStepViewModel")]
    public class AddStepViewModelTests : BaseViewModelTests
    {
        class When_add_step_view_model_is_loaded
        {
            static AddStepViewModel vm;

            Because of = () => vm = new AddStepViewModel(_messagingCenter, _navigationService);

            It should_have_empty_name = () => vm.Name.Should().BeEmpty();
            It should_have_zero_duration = () => vm.Duration.Should().Be("00:00:00");
            It should_have_zero_hours = () => vm.Hours.Should().Be(0);
            It should_have_zero_minutes = () => vm.Minutes.Should().Be(0);
            It should_have_zero_second = () => vm.Seconds.Should().Be(0);
            It should_have_save_disabled = () => vm.SaveEnabled.Should().BeFalse();
        }

        class When_setting_the_recipe_title
        {
            static AddStepViewModel _vm;
            static string _recipeTitle;

            Establish context = () =>
            {
                _vm = new AddStepViewModel(_messagingCenter, _navigationService);
                _recipeTitle = "Roast Beef";
            };
            
            Because of = () => _vm.RecipeTitle = Uri.EscapeDataString(_recipeTitle);
            
            It should_have_changed_title = () => _vm.Title.Should().BeEquivalentTo($"Add step to {_recipeTitle}");
        }

        class When_setting_the_step_name
        {
            static AddStepViewModel _vm;
            
            Establish context = () => { _vm = new AddStepViewModel(_messagingCenter, _navigationService); };
            
            Because of = () => _vm.Name = "First step into a larger world";
            
            It should_have_enabled_the_save_button = () => _vm.SaveEnabled.Should().BeTrue();
        }

        class When_setting_the_time
        {
            static AddStepViewModel _vm;
            
            Establish context = () =>
            {
                _vm = new AddStepViewModel(_messagingCenter, _navigationService);
            };

            Because of = () =>
            {
                _vm.Hours = 5;
                _vm.Minutes = 2;
                _vm.Seconds = 3;
            };
            
            It should_have_changed_the_duration = () => _vm.Duration.Should().Be("05:02:03");
        }

        class When_trying_to_save_without_a_title
        {
            static AddStepViewModel _vm;
            
            Establish context = () => _vm = new AddStepViewModel(_messagingCenter, _navigationService);
            
            Because of = () => _vm.Save.Execute(null);
            
            It should_not_publish_add_step_message = () => _messagingCenter.NothingSent();
            It should_not_navigated_back =() => _navigationService.NavigatedBack.Should().BeFalse();
        }
        
        class When_saving_the_step
        {
            static string _name = "My new step";
            static int _hours = 1;
            static int _minutes = 2;
            static int _seconds = 3;
            static AddStepViewModel _vm;
            static string _message;

            Establish context = () =>
            {
                _vm = new AddStepViewModel(_messagingCenter, _navigationService)
                {
                    Name = _name,
                    Hours = _hours,
                    Minutes = _minutes,
                    Seconds = _seconds
                };

                _message = JsonConvert.SerializeObject(new Step(_vm.Step.Id, _name, _hours, _minutes, _seconds));
            };

            Because of = () => _vm.Save.Execute(null);

            It should_publish_add_step_message = () => _messagingCenter.WasSent(_vm, Messages.AddRecipeStep, _message);
            It should_navigated_back =() => _navigationService.NavigatedBack.Should().BeTrue();
        }
    }
}