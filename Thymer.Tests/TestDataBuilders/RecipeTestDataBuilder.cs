using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Thymer.Core.Models;
using Thymer.Models;

namespace Thymer.Tests.TestDataBuilders
{
    public class RecipeTestDataBuilder
    {
        private class RecipeSpecifications
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public string Title { get; set; } = RandomString;
            public string Description { get; set; } = RandomString;
            public ObservableCollection<Step> Steps { get; set; } = new ObservableCollection<Step>();

            private static string RandomString => $"{Guid.NewGuid()}";
        }

        private readonly RecipeSpecifications _recipeSpecifications;

        public RecipeTestDataBuilder()
        {
            _recipeSpecifications = new RecipeSpecifications();
        }

        public RecipeTestDataBuilder WithId(Guid id)
        {
            _recipeSpecifications.Id = id;

            return this;
        }

        public RecipeTestDataBuilder WithTitle(string title)
        {
            _recipeSpecifications.Title = title;

            return this;
        }

        public RecipeTestDataBuilder WithDescription(string description)
        {
            _recipeSpecifications.Description = description;

            return this;
        }

        public RecipeTestDataBuilder WithSteps(params Step[] steps)
        {
            foreach (var step in steps)
                _recipeSpecifications.Steps.Add(step);

            return this;
        }

        public Recipe Build()
        {
            return new Recipe(
                _recipeSpecifications.Id,
                _recipeSpecifications.Title,
                _recipeSpecifications.Description,
                _recipeSpecifications.Steps);
        }
    }
}