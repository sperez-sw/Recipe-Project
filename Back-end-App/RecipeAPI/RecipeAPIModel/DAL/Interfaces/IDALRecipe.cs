using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAPIModel.DAL
{
    public interface IDALRecipe
    {
        Task<IEnumerable<Recipe>> GetAllRecipes();

        Task<Recipe> GetRecipe(int id);

        Task<bool> InsertRecipe(Recipe recipe);

        Task<bool> UpdateRecipe(Recipe recipe);

        Task<bool> DeleteRecipe(int id);
    }
}
