using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Machine.Specifications;
using Newtonsoft.Json;
using Thymer.Adapters.Services.Database;
using Thymer.Core.Exceptions;
using Thymer.Core.Models;
using Thymer.Models;
using Thymer.Tests.TestDataBuilders;

namespace Thymer.Tests.ServiceTests
{
    [Subject("DatabaseService")]
    class DatabaseTests
    {
        private const string title = "Roast Beef";
        private const string description = "The best of the roasts";
        private const string stepOneName = "Beef";
        private const string stepTwoName = "Roast Potatoes";
        private const int stepOneHours = 1;
        private const int stepOneMinutes = 2;
        private const int stepOneSeconds = 3;
        private const int stepTwoHours = 4;
        private const int stepTwoMinutes = 5;
        private const int stepTwoSeconds = 6;

        static IAmADatabase _database;

        Establish context = () =>
        {
            _database = new Database(":memory:");
        };
        
        class When_retrieving_all_recipes
        {
            static List<Recipe> recipes;
            static List<Recipe> returnedRecipes;

            private Establish context = () =>
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

                recipes = new List<Recipe> {recipeOne, recipeTwo};

                var storedRecipes = recipes.Select(r => new StoredRecipe(r.Id, r.ToString()));

                _database.Connection.DeleteAllAsync<StoredRecipe>();
                
                var result = _database.Connection.InsertAllAsync(storedRecipes).Result;
            };

            Because of = () =>
            {
                returnedRecipes = new List<Recipe>(_database.GetAllRecipes());
            };

            It should_return_all_recipes = () =>
            {
                returnedRecipes.Should().BeEquivalentTo(recipes);
            };
        }
        
        class When_adding_a_new_recipe_to_the_database
        {
            static Recipe recipe;

            Establish context = () =>
            {
                recipe =
                    new RecipeTestDataBuilder()
                        .WithSteps(new StepTestDataBuilder().Build(), new StepTestDataBuilder().Build())
                        .Build();
            };

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
                            .WithHours(stepOneHours)
                            .WithMinutes(stepOneMinutes)
                            .WithSeconds(stepOneSeconds)
                            .Build(), 
                        new StepTestDataBuilder()
                            .WithName(stepTwoName)
                            .WithHours(stepTwoHours)
                            .WithMinutes(stepTwoMinutes)
                            .WithSeconds(stepTwoSeconds)
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
                    new Step(stepOneName, stepOneHours, stepOneMinutes, stepOneSeconds),
                    new Step(stepTwoName, stepTwoHours, stepTwoMinutes, stepTwoSeconds)
                }, options => options.Excluding(s => s.Id));
            };
        }
        
        class When_retrieving_a_recipe_that_does_not_exist
        {
            static readonly Func<Task> getRecipe = async () => await _database.GetRecipe(Guid.Empty);

            It should_throw_recipe_does_not_exist_exception = () => getRecipe.Should().Throw<RecipeDoesNotExistException>();
        }
    }
}