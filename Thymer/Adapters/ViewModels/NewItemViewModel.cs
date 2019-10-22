using System.Threading.Tasks;
using System.Windows.Input;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public class NewItemViewModel : ViewModelBase
    {
        public Recipe Recipe { get; set;  }
        
        public string Name
        {
            get => _name;
            set
            {
                Recipe.Title = value;
                SetPropertyAndRaise(ref _name, value, nameof(Name));
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                Recipe.Description = value;
                SetPropertyAndRaise(ref _description, value, nameof(Description));
            }
        }
        
        private readonly INavigationService _navigationService;
        private readonly IMessagingCenter _messagingCenter;
        
        public NewItemViewModel(INavigationService navigationService, IMessagingCenter messagingCenter) : base(navigationService)
        {
            _navigationService = navigationService;
            _messagingCenter = messagingCenter;
            
            Recipe = new Recipe();
        }

        public async Task SaveNewRecipe()
        {
            _messagingCenter.Send(this, Messages.AddRecipe, Recipe);
            await _navigationService.NavigateBack();
        }

        public async Task Cancel()
        {
            await _navigationService.NavigateBack();
        }

        private string _name, _description;
    }
}