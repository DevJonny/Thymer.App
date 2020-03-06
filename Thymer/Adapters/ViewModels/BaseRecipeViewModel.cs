using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Newtonsoft.Json;
using Thymer.Adapters.Services.Database;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
using Thymer.Ports.Services;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    public abstract class BaseRecipeViewModel : BaseViewModel
    {
        public abstract Recipe Recipe { get; set; }
        public ObservableCollection<Step> Steps => Recipe.Steps;
        public ICommand Save { get; }
        public ICommand AddStep { get; }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name));
                Title = _name;
                Recipe.Title = _name;
            }
        }

        public string Description
        {
            get => _description;
            set
            {
                SetProperty(ref _description, value, nameof(Description));
                Recipe.Description = _description;
            }
        }

        public Step SelectedStep
        {
            get => _selectedStep;
            set => SetProperty(ref _selectedStep, value, nameof(SelectedStep));
        }

        protected readonly INavigationService _navigationService;
        protected readonly IAmADatabase _database;
        protected readonly IMessagingCenter _messagingCenter;
        protected readonly StateService _stateService;
        
        public BaseRecipeViewModel(INavigationService navigationService, IAmADatabase database, IMessagingCenter messagingCenter, StateService stateService)    
        {
            _navigationService = navigationService;
            _database = database;
            _messagingCenter = messagingCenter;
            _stateService = stateService;

            Title = "New Recipe";
            Save = new Command(async () => await SaveRecipe());
            AddStep = new Command(async () => await AddStepToRecipe());
            
            _messagingCenter.Subscribe<AddStepViewModel, string>(
                this, 
                Messages.AddRecipeStep, 
                (sender, arg) => ReceiveStep(arg));
        }

        public abstract Task SaveRecipe();

        public async Task UpdateStep(Step step)
        {
            var recipeName = Uri.EscapeDataString(Recipe.Title);
            var existingStep = Uri.EscapeDataString($"{step.Id}|{step.Name}|{step.Hours}|{step.Minutes}|{step.Seconds}");
            
            await _navigationService.NavigateTo<AddStepViewModel>(("name", $"{recipeName}"), ("existingStep", existingStep));
        }

        public async Task AddStepToRecipe()
        {
            await _navigationService.NavigateTo<AddStepViewModel>(("recipeTitle", $"{Uri.EscapeDataString(Recipe.Title)}"));
        }

        public void ReceiveStep(string stepMessage)
        {
            var step = JsonConvert.DeserializeObject<Step>(stepMessage);
            
            Recipe.AddStep(step);
        }

        public void ReceiveUpdatedStep(string stepMessage)
        {
            var step = JsonConvert.DeserializeObject<Step>(stepMessage);

            Recipe.UpdateStep(step);
        }

        private string _name = string.Empty;
        private string _description = string.Empty;
        private Step _selectedStep = null;
    }
}