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
            var result = await db.QueryAsync<Recipe, Ingredient, Recipe>(
            sql,
            (recipe, ingredient) =>
            {
                if(recipe.ingredients == null)
                {
                    recipe.ingredients = new List<Ingredient>();
                }
                recipe.ingredients.Add(ingredient);
                return recipe;
            },
            splitOn: "id");

            return result.GroupBy(o => o.id).Select(group =>
            {
                var combinedRecipe = group.First();
                combinedRecipe.ingredients = group.Select(recipe => recipe.ingredients.Single()).ToList();
                return combinedRecipe;
            });

            /*var sql = @"SELECT r.*, i.* FROM recipes r 
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
            */
        }

        async Task<Recipe> IDALRecipe.GetRecipe(int id)
        {
            var db = dbConnection();
            //var sql = @"SELECT id, name, description, imagePath FROM recipes WHERE id = @Id";
            //return await db.QueryFirstOrDefaultAsync<Recipe>(sql, new { Id = id});

            var sql = @"SELECT r.*, i.* FROM recipes r 
                LEFT JOIN rec_ing ri ON ri.id_recipe = r.id 
               LEFT JOIN ingredients i ON ri.id_ing = i.id WHERE r.id = @Id";
            var result = await db.QueryAsync<Recipe, Ingredient, Recipe>(
                    sql, (recipe, ingredient) =>
                    {
                        recipe.ingredients = recipe.ingredients ?? new List<Ingredient>();
                        recipe.ingredients.Add(ingredient);
                        return recipe;
                    },
                    new { id }, splitOn: "id");

            return result.GroupBy(o => o.id).Select(group =>
            {
                var combinedRecipe = group.First();
                combinedRecipe.ingredients = group.Select(recipe => recipe.ingredients.Single()).ToList();
                return combinedRecipe;
            }).FirstOrDefault();
        }

        async Task<bool> IDALRecipe.InsertRecipe(Recipe recipe)
        {
            var db  = dbConnection();
            /*var sql = @"INSERT INTO recipes (name, description, imagePath) VALUES (@Name, @Description, @ImagePath)";
            var result = await db.ExecuteAsync(sql, new {recipe.name, recipe.description, recipe.imagePath});
            return result > 0;*/

            try
            {
                recipe.id = await db.QuerySingleAsync<int>(@"INSERT INTO recipes (name, description, imagePath) VALUES (@Name, @Description, @ImagePath);
            SELECT LAST_INSERT_ID();", new { recipe.name, recipe.description, recipe.imagePath });

                if (recipe.ingredients != null && recipe.ingredients.Count > 0)
                {
                    foreach (var ingredient in recipe.ingredients)
                    {
                        await db.QuerySingleAsync<int>(@"INSERT INTO ingredients (id,name, amount) VALUES (@Id, @Name, @Amount); SELECT LAST_INSERT_ID();",new { ingredient.id, ingredient.name, ingredient.amount });
                        var NewId = ingredient.id;
                        var result = await db.ExecuteAsync(@"INSERT INTO rec_ing(id_recipe,id_ing) 
                        VALUES(@IdRec,@IdIng)", new { IdRec = recipe.id, IdIng = NewId });
                        return result > 0;
                    }
                    /*await db.ExecuteAsync(@"INSERT INTO rec_ing(id_recipe,id_ing) 
                    VALUES(@IdRec,@IdIng)",
                    recipe.ingredients.Select(c => new { IdRec = recipe.id, IdIng = c.id }));*/
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        async Task<bool> IDALRecipe.UpdateRecipe(Recipe recipe)
        {
            var db = dbConnection();
            //var sql = @"UPDATE recipes SET name = @Name, description = @Description, imagePath = @ImagePath WHERE id = @Id";
            //var result = await db.ExecuteAsync(sql, new { recipe.name, recipe.description, recipe.imagePath, recipe.id});
            //return result > 0;

            try
            {
                recipe.id = await db.QuerySingleAsync<int>(@"UPDATE recipes SET name = @Name, description = @Description, imagePath = @ImagePath; SELECT LAST_INSERT_ID();", new { recipe.name, recipe.description, recipe.imagePath });

                if (recipe.ingredients != null && recipe.ingredients.Count > 0)
                {
                    foreach (var ingredient in recipe.ingredients)
                    {
                        await db.QuerySingleAsync<int>(@"INSERT INTO ingredients (id,name, amount) VALUES (@Id, @Name, @Amount) WHERE NOT EXISTS (SELECT * FROM ingredients WHERE id = @Id LIMIT 1); SELECT LAST_INSERT_ID();", new { ingredient.id, ingredient.name, ingredient.amount });
                        var NewId = ingredient.id;
                        var result = await db.ExecuteAsync(@"INSERT INTO rec_ing(id_recipe,id_ing) 
                        VALUES(@IdRec,@IdIng) WHERE NOT EXISTS (SELECT * FROM rec_ing WHERE id_recipe = @IdRec AND id_ing = @IdIng LIMIT 1)", new { IdRec = recipe.id, IdIng = NewId });
                        return result > 0;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
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
