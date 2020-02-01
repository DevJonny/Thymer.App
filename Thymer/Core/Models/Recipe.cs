using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Thymer.Models;

namespace Thymer.Core.Models
{
    public class Recipe
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Step> Steps { get; } = new List<Step>();

        public Recipe() { }

        public Recipe(string title, string description) 
            => (Id, Title, Description) 
                = (Guid.NewGuid(), title, description);

        [JsonConstructor]
        public Recipe(Guid id, string title, string description, List<Step> steps)
            => (Id, Title, Description, Steps) 
                = (id, title, description, steps);

        public void AddStep(Step step)
        {
            Steps.Add(step);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class StoredRecipe
    {
        public Guid Id { get; set; }
        public string Recipe { get; set; }
        
        public StoredRecipe(Guid recipeId, string recipe)
            => (Id, Recipe) 
                = (recipeId, recipe);

        public StoredRecipe() { }
    }
}