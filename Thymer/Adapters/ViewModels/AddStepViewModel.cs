using System;
using System.Collections.Generic;
using System.Linq;
using MvvmHelpers;
using Thymer.Services.Database;
using Xamarin.Forms;

namespace Thymer.Adapters.ViewModels
{
    [QueryProperty("RecipeTitle", "recipeTitle")]
    public class AddStepViewModel : BaseViewModel
    {
        private readonly IAmADatabase _database;
        
        public AddStepViewModel(IAmADatabase database)
        {
            _database = database;
        }

        public string RecipeTitle
        {
            get => _recipeTitle;
            set
            {
                SetProperty(ref _recipeTitle, Uri.UnescapeDataString(value), nameof(RecipeTitle));
                Title = $"Add step to {_recipeTitle}";
            }
        }

        public string Name { get => _name; set => SetProperty(ref _name, value, nameof(Name)); }

        public string Duration
        {
            get => _duration; 
            set => SetProperty(ref _duration, value, nameof(Duration));
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

        public List<int> ZeroToTwentyFour => Enumerable.Range(0, 24).ToList();
        public List<int> ZeroToFiftyNine => Enumerable.Range(0, 59).ToList();

        private void UpdateDuration()
        {
            Duration = $"{Hours:00}:{Minutes:00}:{Seconds:00}";
        }
        
        private string _recipeTitle = string.Empty;
        private string _name = string.Empty;
        private string _duration = "00:00:00";
        private int _hours;
        private int _minutes;
        private int _seconds;
    }
}