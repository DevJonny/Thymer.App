using System.Collections.Generic;
using Thymer.Models;

namespace Thymer.Core.Models
{
    public class Recipe
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<Step> Steps { get; } = new List<Step>();

        public Recipe() { }

        public Recipe(string title, string description) => (Title, Description) = (title, description);
    }
}