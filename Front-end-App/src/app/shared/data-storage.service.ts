import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Recipe } from '../recipes/recipe.model';
import { RecipeService } from '../services/recipe.service';
import {map, tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class DataStorageService {

  constructor(private http: HttpClient, private recipeService: RecipeService) { }

  storeRecipes(){
    const recipes = this.recipeService.getRecipes();
    this.http.put('https://ng-course-recipe-book-29aea-default-rtdb.firebaseio.com/recipes.json',recipes).subscribe( res => {
      console.log(res)
    })
  }
  
  //'https://ng-course-recipe-book-29aea-default-rtdb.firebaseio.com/recipes.json'
  //'https://localhost:44352/api/Recipes'
  fetchRecipes(){
    return this.http
      .get<Recipe[]>('https://localhost:44352/api/Recipes')
      .pipe(
        map(recipes =>{
        return recipes.map(recipe => {
          debugger
          return {...recipe,ingredients: recipe.ingredients[0] ? recipe.ingredients : []};
        });
      }),
      tap(recipes => {
        this.recipeService.setRecipes(recipes)
      })
      )
    //   .subscribe( recipes=> {
      
    // })
  }
}
