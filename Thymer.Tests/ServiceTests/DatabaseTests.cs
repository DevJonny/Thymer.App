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

namespace Thymer.Tests.ServiceTests
{
    public class DatabaseTests
    {
        static readonly string name = "Roast Beef"; 
        static readonly string description = "The best of the roasts"; 
        static readonly string stepOneName = "Beef";
        static readonly string stepTwoName = "Roast Potatoes";
        static readonly long stepOneDuration = 500;
        static readonly long stepTwoDuration = 250;
        
        static IAmADatabase _database;

        private Establish context = () =>
        {
            var dbPath = Path.Combine(Environment.CurrentDirectory, "test.db");
            
            File.Delete(dbPath);
            
            _database = new Database(dbPath);
        };
        
        class When_adding_a_new_recipe_to_the_database
        {
            static Recipe recipe;

            private Establish context = () =>
            {
                recipe = new Recipe(name, description);
                recipe.AddStep(new Step(stepOneName, stepOneDuration));
                recipe.AddStep(new Step(stepTwoName, stepTwoDuration));
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
                var recipeToSave = new Recipe(name, description);
                recipeToSave.AddStep(new Step(stepOneName, stepOneDuration));
                recipeToSave.AddStep(new Step(stepTwoName, stepTwoDuration));

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
                recipe.Title.Should().Be(name);
                recipe.Description.Should().Be(description);
                recipe.Steps.Should().BeEquivalentTo(new List<Step>
                {
                    new Step(stepOneName, stepOneDuration),
                    new Step(stepTwoName, stepTwoDuration)
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