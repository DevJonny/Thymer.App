using System;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using SQLite;
using Thymer.Core.Extensions;

namespace Thymer.Core.Models
{
    [Table("Recipes")]
    public class Recipe
    {
        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ObservableCollection<Step> Steps { get; } = new ObservableCollection<Step>();

        public Recipe() { }

        public Recipe(string title, string description) 
            => (Id, Title, Description) 
                = (Guid.NewGuid(), title, description);

        [JsonConstructor]
        public Recipe(Guid id, string title, string description, ObservableCollection<Step> steps)
            => (Id, Title, Description, Steps) 
                = (id, title, description, steps);

        public void AddStep(Step step)
        {
            Steps.Add(step);
            
            Steps.Sort(Step.Compare());
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public void UpdateStep(Step step)
        {
            var oldStep = Steps.First(s => s.Id == step.Id);
            
            Steps.Remove(oldStep);
            Steps.Add(step);
            Steps.Sort(Step.Compare());
        }

        public static Comparison<Recipe> Compare()
        {
            return (recipe1, recipe2) => string.Compare(recipe1.Title, recipe2.Title, StringComparison.Ordinal);
        }
    }

    public class StoredRecipe
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public string Recipe { get; set; }
        
        public StoredRecipe(Guid recipeId, string recipe)
            => (Id, Recipe) 
                = (recipeId, recipe);

        public StoredRecipe() { }
    }
}