import { EventEmitter, Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Recipe } from '../recipes/recipe.model';
import { Ingredient } from '../shared/ingredient.model';
import { ShoppingListService } from './shopping-list.service';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  recipeSelected = new Subject<Recipe>();//new EventEmitter<Recipe>();
  recipesChanged = new Subject<Recipe[]>();
  private recipes:Recipe[] = [];
  // [
  //   new Recipe('A Test Recipe','This is simply a test','https://www.simplyrecipes.com/thmb/OCi18J2V8OeKDFV3FxoeKvgq74E=/1423x1067/smart/filters:no_upscale()/__opt__aboutcom__coeus__resources__content_migration__simply_recipes__uploads__2012__07__grilled-sweet-potatoes-horiz-a-1600-7c8292daa98e4020b447f0dc97a45cb7.jpg',[new Ingredient('Meat',1),new Ingredient('Eggs',5)]),
  //   new Recipe('A Test Recipe','This is simply a test','https://www.simplyrecipes.com/thmb/OCi18J2V8OeKDFV3FxoeKvgq74E=/1423x1067/smart/filters:no_upscale()/__opt__aboutcom__coeus__resources__content_migration__simply_recipes__uploads__2012__07__grilled-sweet-potatoes-horiz-a-1600-7c8292daa98e4020b447f0dc97a45cb7.jpg',[new Ingredient('French Fires',3)])
  // ];
  constructor(private sLService:ShoppingListService) { }

  setRecipes(recipes: Recipe[]){
    this.recipes = recipes;
    this.recipesChanged.next(this.recipes.slice());
  }

  getRecipes(){
    //EL SLICE SIN PARAMETROS DEVUELVE UNA COPIA EXACTA DEL ARRAY, NO LA ORIGINAL, POR LO TANTO AL MODIFICARLA NO AFECTARIA A LA INICIALIZADA EN EL SERVICIO
    return this.recipes.slice();
  }

  getRecipe(index:number){
    return this.recipes.slice()[index];
  }

  addIngredientsToSL(ingredients:Ingredient[]){
    this.sLService.IngredientsAddedToSL(ingredients)
  }

  addRecipe(recipe:Recipe){
    this.recipes.push(recipe);
    this.recipesChanged.next(this.recipes.slice());
  }

  updateRecipe(index:number, newRecipe: Recipe){
    this.recipes[index] = newRecipe;
    this.recipesChanged.next(this.recipes.slice());
  }
  
  deleteRecipe(index:number){
    this.recipes.splice(index,1)
    this.recipesChanged.next(this.recipes.slice())
  }
}
