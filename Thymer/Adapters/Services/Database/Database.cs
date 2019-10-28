using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using Thymer.Core.Exceptions;
using Thymer.Core.Models;
using Thymer.Services.Database;

namespace Thymer.Adapters.Services.Database
{
    public class Database : IAmADatabase
    {
        public SQLiteAsyncConnection Connection { get; }

        private AsyncTableQuery<StoredRecipe> _recipeTable => Connection.Table<StoredRecipe>();
        
        public Database(string dbPath)
        {
            Connection = new SQLiteAsyncConnection(dbPath);
            Connection.CreateTableAsync<StoredRecipe>().Wait();
        }
        
        public async Task AddRecipe(Recipe recipe)
        {
            var storedRecipe = new StoredRecipe(recipe.Id, recipe.ToString());

            await Connection.InsertAsync(storedRecipe);
        }

        public async Task<Recipe> GetRecipe(Guid id)
        {
            var storedRecipe = await _recipeTable.Where(r => r.Id == id).FirstOrDefaultAsync();

            if (storedRecipe is null)
                throw new RecipeDoesNotExistException();

            return JsonConvert.DeserializeObject<Recipe>(storedRecipe.Recipe);
        }
    }
}