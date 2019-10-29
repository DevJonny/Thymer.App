using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.Services.Database;
using Thymer.Core.Exceptions;
using Thymer.Core.Models;
using Thymer.Models;
using Thymer.Services.Database;
using Thymer.Tests.TestDataBuilders;

namespace Thymer.Tests.ServiceTests
{
    public class DatabaseTests
    {
        private const string title = "Roast Beef";
        private const string description = "The best of the roasts";
        private const string stepOneName = "Beef";
        private const string stepTwoName = "Roast Potatoes";
        private const long stepOneDuration = 500;
        private const long stepTwoDuration = 250;

        static IAmADatabase _database;

        Establish context = () =>
        {
            _database = new Database(":memory:");
        };
        
        class When_adding_a_new_recipe_to_the_database
        {
            static Recipe recipe;

            private Establish context = () => recipe =
                new RecipeTestDataBuilder()
                    .WithSteps(new StepTestDataBuilder().Build(), new StepTestDataBuilder().Build())
                    .Build();

            Because of = () => _database.AddRecipe(recipe);

            It should_have_inserted_the_recipe = async () =>
            {
                var returnedStoredRecipe = await _database.Connection.Table<StoredRecipe>()
                    .FirstAsync(sr => sr.Id == recipe.Id);
                
                var returnedRecipe = JsonConvert.DeserializeObject<Recipe>(returnedStoredRecipe.Recipe);
                
                returnedRecipe.Should().BeEquivalentTo(recipe);
            };
        }

        class When_retrieving_a_recipe_id
        {
            static Recipe recipe;
            static Guid id;

            Establish context = async () =>
            {
                var recipeToSave = new RecipeTestDataBuilder()
                    .WithTitle(title)
                    .WithDescription(description)
                    .WithSteps(
                        new StepTestDataBuilder()
                            .WithName(stepOneName)
                            .WithDuration(stepOneDuration)
                            .Build(), 
                        new StepTestDataBuilder()
                            .WithName(stepTwoName)
                            .WithDuration(stepTwoDuration)
                            .Build())
                    .Build();

                id = recipeToSave.Id;

                var storedRecipe = new StoredRecipe(id, JsonConvert.SerializeObject(recipeToSave));
                
                await _database.Connection.InsertAsync(storedRecipe);
            };

            Because of = () =>
            {
                recipe = _database.GetRecipe(id).Result;
            };

            It should_return_correct_recipe = () =>
            {
                recipe.Id.Should().Be(id);
                recipe.Title.Should().Be(title);
                recipe.Description.Should().Be(description);
                recipe.Steps.Should().BeEquivalentTo(new List<Step>
                {
                    new Step(stepOneName, stepOneDuration),
                    new Step(stepTwoName, stepTwoDuration)
                }, options => options.Excluding(s => s.Id));
            };
        }

        class When_retrieving_all_recipes
        {
            static readonly List<Recipe> recipes = new List<Recipe>();
            static IReadOnlyList<Recipe> returnedRecipes;

            Establish context => async () =>
            {
                var recipeOne = new RecipeTestDataBuilder()
                    .WithSteps(
                        new StepTestDataBuilder().Build(), 
                        new StepTestDataBuilder().Build())
                    .Build();
                
                var recipeTwo = new RecipeTestDataBuilder()
                    .WithSteps(
                        new StepTestDataBuilder().Build())
                    .Build();
                
                recipes.Add(recipeOne);
                recipes.Add(recipeTwo);

                _database.Connection.InsertAllAsync(recipes);
            };

            Because of = () =>
            {
                returnedRecipes = new List<Recipe>(_database.GetAllRecipes().Result);
            };

            It should_return_all_recipes = () =>
            {
                returnedRecipes.Should().BeEquivalentTo(recipes);
            };
        }

        class When_retrieving_a_recipe_that_does_not_exist
        {
            static readonly Func<Task> getRecipe = async () => await _database.GetRecipe(Guid.Empty);

            It should_throw_recipe_does_not_exist_exception = () => getRecipe.Should().Throw<RecipeDoesNotExistException>();
        }
    }
}