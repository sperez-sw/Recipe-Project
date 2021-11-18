import { Component, OnInit } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { Recipe } from './recipe.model';

@Component({
  selector: 'app-recipes',
  templateUrl: './recipes.component.html',
  styleUrls: ['./recipes.component.scss']
})
export class RecipesComponent implements OnInit {
  selectedRecipe:Recipe;
  
  constructor(private recipeService: RecipeService) { }

  ngOnInit(): void {
    //No se usa más
    
    // this.recipeService.recipeSelected
    //   .subscribe(
    //     (recipe:Recipe)=>{
    //     this.selectedRecipe = recipe;
    // });
  }

}
