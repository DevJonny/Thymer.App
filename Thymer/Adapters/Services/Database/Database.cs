using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using Thymer.Core.Exceptions;
using Thymer.Core.Models;

namespace Thymer.Adapters.Services.Database
{
    public class Database : IAmADatabase
    {
        public SQLiteAsyncConnection Connection { get; }

        private AsyncTableQuery<StoredRecipe> _recipeTable => Connection.Table<StoredRecipe>();
        
        public Database(string dbPath)
        {
            Connection = new SQLiteAsyncConnection(dbPath);
            Connection.CreateTableAsync<StoredRecipe>(CreateFlags.ImplicitPK).Wait();
        }

        public Database() 
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ThymerSQLite.db3");
            
            Connection = new SQLiteAsyncConnection(path);
            Connection.CreateTableAsync<StoredRecipe>().Wait();
        }
        
        public async Task AddRecipe(Recipe recipe)
        {
            var storedRecipe = new StoredRecipe(recipe.Id, recipe.ToString());

            await Connection.InsertAsync(storedRecipe);
        }

        public async Task UpdateRecipe(Recipe recipe)
        {
            var storedRecipe = new StoredRecipe(recipe.Id, recipe.ToString());
            
            await Connection.UpdateAsync(storedRecipe);
        }

        public async Task<Recipe> GetRecipe(Guid id)
        {
            var storedRecipe = await _recipeTable.Where(r => r.Id == id).FirstOrDefaultAsync();

            if (storedRecipe is null)
                throw new RecipeDoesNotExistException();

            return JsonConvert.DeserializeObject<Recipe>(storedRecipe.Recipe);
        }

        public IEnumerable<Recipe> GetAllRecipes()
        {
            var storedRecipes = _recipeTable.ToListAsync().GetAwaiter().GetResult();

            return storedRecipes.Select(sr => JsonConvert.DeserializeObject<Recipe>(sr.Recipe));
        }
    }
}