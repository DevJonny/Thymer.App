using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmHelpers;
using Newtonsoft.Json;
using Thymer.Adapters.Services.Navigation;
using Thymer.Core.Models;
using Thymer.Ports.Messaging;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    [QueryProperty("RecipeTitle", "recipeTitle")]
    [QueryProperty("ExistingStep","existingStep")]
    public class AddStepViewModel : BaseViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IMessagingCenter _messagingCenter;
        
        public AddStepViewModel(IMessagingCenter messagingCenter, INavigationService navigationService)
        {
            _messagingCenter = messagingCenter;
            _navigationService = navigationService;

            Save = new Command(async () => await SaveStep());
        }

        public ICommand Save { get; }

        public string RecipeTitle
        {
            get => _recipeTitle;
            set
            {
                SetProperty(ref _recipeTitle, Uri.UnescapeDataString(value), nameof(RecipeTitle)); 
                Title = $"Add step to {_recipeTitle}";
            }
        }
        
        public string ExistingStep
        {
            get => _existingStep;
            set
            {
                _existingStep = Uri.UnescapeDataString(value);
                var parts = _existingStep.Split('|');

                Id = Guid.TryParse(parts[0], out Guid id) ? id : Guid.NewGuid();
                Name = parts[1];
                Hours = int.TryParse(parts[2], out var hours) ? hours : 0;
                Minutes = int.TryParse(parts[3], out var minutes) ? minutes : 0;
                Seconds = int.TryParse(parts[4], out var seconds) ? seconds : 0;
            }
        }

        public Guid Id
        {
            get => _id;
            set => SetProperty(ref _id, value, nameof(Id));
        }

        public string Name
        {
            get => _name;
            set
            {
                SetProperty(ref _name, value, nameof(Name)); 
                SaveEnabled = !string.IsNullOrWhiteSpace(_name);
            }
        }

        public string Duration
        {
            get => _duration; 
            private set => SetProperty(ref _duration, value, nameof(Duration));
        }

        public int Hours
        {
            get => _hours;
            set
            {
                SetProperty(ref _hours, value, nameof(Hours)); 
                UpdateDuration();
            }
        }

        public int Minutes
        {
            get => _minutes;
            set
            {
                SetProperty(ref _minutes, value, nameof(Minutes)); 
                UpdateDuration();
            }
        }

        public int Seconds
        {
            get => _seconds;
            set
            {
                SetProperty(ref _seconds, value, nameof(Seconds)); 
                UpdateDuration();
            }
        }

        public bool SaveEnabled
        {
            get => _saveEnabled; 
            set => SetProperty(ref _saveEnabled, value, nameof(SaveEnabled));
        }
        
        public Step Step { get; private set; } = new Step();
        
        public List<int> ZeroToTwentyFour => Enumerable.Range(0, 24).ToList();
        public List<int> ZeroToFiftyNine => Enumerable.Range(0, 59).ToList();

        private async Task SaveStep()
        {
            if (!SaveEnabled)
                return;

            Step = new Step(Step.Id, Name, Hours, Minutes, Seconds);
            
            _messagingCenter.Send(this, Messages.AddRecipeStep, JsonConvert.SerializeObject(Step));
            await _navigationService.NavigateBack();
        }
        
        private void UpdateDuration()
        {
            Duration = $"{Hours:00}:{Minutes:00}:{Seconds:00}";
        }
        
        private string _recipeTitle = string.Empty;
        private string _existingStep = string.Empty;
        private string _name = string.Empty;
        private string _duration = "00:00:00";
        private Guid _id;
        private int _hours, _minutes, _seconds;
        private bool _saveEnabled;
    }
}