using System;
using System.Threading.Tasks;
using SQLite;
using Thymer.Core.Models;

namespace Thymer.Services.Database
{
    public interface IAmADatabase
    {
        SQLiteAsyncConnection Connection { get; }
        
        Task AddRecipe(Recipe recipe);
        Task<Recipe> GetRecipe(Guid id);
    }
}