using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeAPIModel.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IDALRecipe _recipe;

        public RecipesController(IDALRecipe recipe)
        {
            _recipe = recipe;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipes()
        {
            return Ok(await _recipe.GetAllRecipes());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipe(int id)
        {
            return Ok(await _recipe.GetRecipe(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe([FromBody]Recipe recipe) 
        {
            if (recipe == null) 
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var created = await _recipe.InsertRecipe(recipe);
            return Created("created", created);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRecipe([FromBody] Recipe recipe) 
        {
            if (recipe == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _recipe.UpdateRecipe(recipe);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            await _recipe.DeleteRecipe(id);

            return NoContent();
        }
    }
}
