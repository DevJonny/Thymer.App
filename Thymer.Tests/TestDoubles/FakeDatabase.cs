using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Thymer.Adapters.Services.Database;
using Thymer.Core.Models;

namespace Thymer.Tests.TestDoubles
{
    public class FakeDatabase : IAmADatabase
    {
        public SQLiteAsyncConnection Connection { get; }

        public readonly List<Recipe> StoredRecipes = new List<Recipe>();

        public void Seed(IEnumerable<Recipe> seed)
        {
            StoredRecipes.AddRange(seed);
        }
        
        public Task AddRecipe(Recipe recipe)
        {
            StoredRecipes.Add(recipe);
            
            return Task.Delay(0);
        }

        public Task UpdateRecipe(Recipe recipe)
        {
            var original = StoredRecipes.First(r => r.Id == recipe.Id);

            StoredRecipes.Remove(original);
            StoredRecipes.Add(recipe);

            return Task.Delay(0);
        }

        public async Task<Recipe> GetRecipe(Guid id)
        {
            var recipe = StoredRecipes.FirstOrDefault(r => r.Id == id);
            
            return await Task.Run(() => recipe);
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            return StoredRecipes.AsEnumerable();
        }
    }
}