using FluentAssertions;
using Machine.Specifications;
using Thymer.Adapters.ViewModels;
using Thymer.Ports.Messaging;

namespace Thymer.Tests.ViewModelTests
{
    [Subject("ItemsViewModel")]
    class ItemsViewModelTests : BaseViewModelTests
    {
        class When_items_vm_is_loaded
        {
            static ItemsViewModel vm;
            
            Because of = () => vm = new ItemsViewModel(_navigationService, _messagingCenter);

            It should_have_subscribed_to_add_item = () =>
                _messagingCenter.Subscribers.Should().ContainSingle().Which.Should().BeEquivalentTo((vm, AddItem: Messages.AddRecipe, true));
        }

        class When_load_items_command_is_triggered
        {
            static ItemsViewModel vm;

            Establish context = () =>
            {
                vm = new ItemsViewModel(_navigationService, _messagingCenter);
                
            };
                
            Because of = () => vm.LoadItemsCommand.Execute(null);

            private It should_have_loaded_saved_recipes = () =>
                vm.Items.Count.Should().Be(6);
        }
    }
}