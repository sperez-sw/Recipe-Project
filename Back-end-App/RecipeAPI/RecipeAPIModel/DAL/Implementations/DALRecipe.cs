using Dapper;
using MySql.Data.MySqlClient;
using PruebaAPI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeAPIModel.DAL.Implementations
{
    public class DALRecipe : IDALRecipe
    {
        private MySQLConfiguration _connectionString;
        public DALRecipe(MySQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }
        protected MySqlConnection dbConnection() 
        {
            return new MySqlConnection(_connectionString.ConnectionString);
        }
        

        async Task<IEnumerable<Recipe>> IDALRecipe.GetAllRecipes()
        {
            var db = dbConnection();
            //var sql = @"SELECT id, name, description, imagePath FROM recipes";
            //return await db.QueryAsync<Recipe>(sql,new {});

            var sql = @"SELECT r.*, i.* FROM recipes r 
            LEFT JOIN rec_ing ri ON ri.id_recipe = r.id 
               LEFT JOIN ingredients i ON ri.id_ing = i.id";
            return db //.id, r.name, r.description, r.imagePath
                .Query<Recipe, Ingredient, Recipe>(
                    sql, (recipe, ingredient) =>
                     {
                         recipe.ingredients = recipe.ingredients ?? new List<Ingredient>();
                         recipe.ingredients.Add(ingredient);
                         return recipe;
                     },
                    new { }, splitOn: "id").GroupBy(o => o.id).Select(group =>
                    {
                        var combinedRecipe = group.First();
                        combinedRecipe.ingredients = group.Select(recipe => recipe.ingredients.Single()).ToList();
                        return combinedRecipe;
                    });
        }

        async Task<Recipe> IDALRecipe.GetRecipe(int id)
        {
            var db = dbConnection();
            var sql = @"SELECT id, name, description, imagePath FROM recipes WHERE id = @Id";
            return await db.QueryFirstOrDefaultAsync<Recipe>(sql, new { Id = id});
        }

        async Task<bool> IDALRecipe.InsertRecipe(Recipe recipe)
        {
            var db = dbConnection();
            var sql = @"INSERT INTO recipes (id, name, description, imagePath) VALUES (@Id, @Name, @Description, @ImagePath)";
            var result = await db.ExecuteAsync(sql, new {recipe.id, recipe.name, recipe.description, recipe.imagePath});
            return result > 0;
        }

        async Task<bool> IDALRecipe.UpdateRecipe(Recipe recipe)
        {
            var db = dbConnection();
            var sql = @"UPDATE recipes SET name = @Name, description = @Description, imagePath = @ImagePath WHERE id = @Id";
            var result = await db.ExecuteAsync(sql, new { recipe.name, recipe.description, recipe.imagePath, recipe.id});
            return result > 0;
        }

        async Task<bool> IDALRecipe.DeleteRecipe(int id)
        {
            var db = dbConnection();
            var sql = @"DELETE FROM recipes WHERE id = @Id";
            var result = await db.ExecuteAsync(sql, new { id});
            return result > 0;
        }
    }
}
